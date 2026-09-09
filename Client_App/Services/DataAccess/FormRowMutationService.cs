using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Мутации строк открытого отчёта: один экземпляр Form на Id в коллекции и ChangeTracker.
/// Легаси Form 2 (ChangeOrCreateVM) этот сервис не использует.
/// </summary>
public static class FormRowMutationService
{
    /// <summary>
    /// Слот живого порядка: сохранённый Id или ещё не сохранённая строка.
    /// </summary>
    public readonly struct LiveSlot
    {
        public LiveSlot(int? id, Form? added)
        {
            Id = id;
            Added = added;
        }

        public int? Id { get; }
        public Form? Added { get; }
    }

    /// <summary>
    /// Собрать полный набор для сессии: снимок БД + незакоммиченные Added/Modified, без Deleted.
    /// Added дописываются в конец (для тестов / PassportFill). Живой порядок с InsertBefore — <see cref="BuildLiveSlots"/>.
    /// </summary>
    public static List<Form> MergeSessionRows(
        IReadOnlyList<Form> dbSnapshot,
        IReadOnlyList<Form> added,
        IReadOnlyDictionary<int, Form> keepById,
        IReadOnlySet<int> excludeIds)
    {
        var result = new List<Form>(dbSnapshot.Count + added.Count);
        foreach (var row in dbSnapshot)
        {
            if (row.Id > 0 && excludeIds.Contains(row.Id))
                continue;
            if (row.Id > 0 && keepById.TryGetValue(row.Id, out var keep))
                result.Add(keep);
            else
                result.Add(row);
        }

        foreach (var row in added)
            result.Add(row);

        return result;
    }

    /// <summary>
    /// Отцепить все Form данного отчёта/типа из трекера (не только текущую коллекцию).
    /// </summary>
    public static void DetachAllFormRows(DBModel db, int reportId, string formNum)
    {
        foreach (var entry in db.ChangeTracker.Entries().ToList())
        {
            if (entry.Entity is not Form form)
                continue;
            if (form.ReportId != reportId)
                continue;
            if (!string.IsNullOrEmpty(form.FormNum_DB) && form.FormNum_DB != formNum)
                continue;
            if (entry.State != EntityState.Detached)
                entry.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Полный набор строк 1.x в UI-контекст: снимок из отдельного DBModel + merge pending.
    /// </summary>
    public static async Task LoadFullSetIntoReportAsync(
        DBModel uiDb, Report report, string formNum, CancellationToken ct = default)
    {
        if (uiDb == null || report == null || report.Id <= 0 ||
            !FormRowsPageLoader.SupportsDbPaging(formNum))
            return;

        CollectPending(uiDb, report.Id, formNum,
            out var added, out var keepById, out var excludeIds);

        DetachAllFormRows(uiDb, report.Id, formNum);

        List<Form> snapshot;
        try
        {
            snapshot = await MainWindowDbGate.RunAsync(StaticConfiguration.DBPath, async (snap, token) =>
                await FormRowsPageLoader
                    .LoadPageListAsync(snap, report.Id, formNum, 0, 1_000_000, token)
                    .ConfigureAwait(false), ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("FormRowMutationService.LoadFullSetIntoReportAsync failed", ex);
            throw;
        }

        var merged = MergeSessionRows(snapshot, added, keepById, excludeIds);
        FormRowsPageLoader.ApplyPageToReport(uiDb, report, formNum, merged);

        // Attach сбрасывает снимок: вернуть Modified, иначе правки ячеек не уедут в SaveChanges.
        foreach (var form in keepById.Values)
        {
            var entry = uiDb.Entry(form);
            if (entry.State == EntityState.Unchanged)
                entry.State = EntityState.Modified;
        }
    }

    /// <summary>
    /// Удалить сохранённую строку по Id из коллекции и трекера (без второго Attach).
    /// </summary>
    public static void RemoveFormRowById(DBModel db, Report report, int formId)
    {
        if (db == null || report == null || formId <= 0)
            return;

        foreach (var item in FindInCollectionById(report, formId))
            report.Rows.Remove(item);

        var tracked = FindTrackedById(db, formId);
        if (tracked != null)
        {
            report.Rows.Remove(tracked);
            var state = db.Entry(tracked).State;
            if (state == EntityState.Added)
                db.Entry(tracked).State = EntityState.Detached;
            else if (state is not (EntityState.Deleted or EntityState.Detached))
                db.Remove(tracked);
            return;
        }

        var leftover = FindInCollectionById(report, formId).FirstOrDefault();
        if (leftover == null)
            return;

        var entry = db.Entry(leftover);
        if (entry.State == EntityState.Detached)
        {
            db.Attach(leftover);
            db.Remove(leftover);
        }
        else if (entry.State == EntityState.Added)
            entry.State = EntityState.Detached;
        else if (entry.State != EntityState.Deleted)
            db.Remove(leftover);
    }

    /// <summary>
    /// Разобрать параметр команды удаления (коллекция, IList грида, одна строка).
    /// </summary>
    public static Form[] EnumerateForms(object? parameter)
    {
        if (parameter is Form single)
            return [single];
        if (parameter is IEnumerable enumerable and not string)
            return enumerable.OfType<Form>().ToArray();
        return [];
    }

    /// <summary>
    /// Оставить только строки, которые ещё в коллекции отчёта (по Id или ссылке).
    /// Команда часто держит экземпляры со страницы до сессии — сопоставляем с живыми.
    /// </summary>
    public static List<Form> FilterLiveForms(IReadOnlyList<Form> candidates, IReadOnlyList<Form> liveRows)
    {
        var result = new List<Form>();
        if (candidates == null || liveRows == null || candidates.Count == 0 || liveRows.Count == 0)
            return result;

        var byId = new Dictionary<int, Form>();
        foreach (var row in liveRows)
        {
            if (row?.Id > 0)
                byId.TryAdd(row.Id, row);
        }

        var seenIds = new HashSet<int>();
        var seenUnsaved = new HashSet<Form>();
        foreach (var candidate in candidates)
        {
            if (candidate == null)
                continue;

            if (candidate.Id > 0)
            {
                if (!byId.TryGetValue(candidate.Id, out var live))
                    continue;
                if (!seenIds.Add(live.Id))
                    continue;
                result.Add(live);
            }
            else
            {
                Form? live = null;
                foreach (var row in liveRows)
                {
                    if (ReferenceEquals(row, candidate))
                    {
                        live = row;
                        break;
                    }
                }

                if (live == null || !seenUnsaved.Add(live))
                    continue;
                result.Add(live);
            }
        }

        return result;
    }

    /// <summary>
    /// После SetOrder явно пометить № п/п: Attach сбрасывает снимок, DetectChanges
    /// может не увидеть автосвойство без сеттера INPC — иначе в БД уходит только DELETE и остаются разрывы.
    /// Detached после сессии — Attach и всё равно IsModified (UPDATE запишет текущий номер).
    /// </summary>
    public static int MarkNumberInOrderChanged(DBModel db, IEnumerable<Form> rows)
    {
        var count = 0;
        if (db == null || rows == null)
            return 0;

        foreach (var form in rows)
        {
            if (form == null || form.Id <= 0)
                continue;

            var entry = db.Entry(form);
            if (entry.State is EntityState.Deleted or EntityState.Added)
                continue;

            if (entry.State == EntityState.Detached)
            {
                db.Attach(form);
                entry = db.Entry(form);
            }

            entry.Property(nameof(Form.NumberInOrder_DB)).IsModified = true;
            count++;
        }

        return count;
    }

    /// <summary>
    /// Удалить ещё не сохранённую строку (Id == 0) по ссылке.
    /// </summary>
    public static void RemoveFormRowInstance(DBModel db, Report report, Form form)
    {
        if (db == null || report == null || form == null)
            return;

        report.Rows.Remove(form);
        var entry = db.Entry(form);
        if (entry.State == EntityState.Added)
            entry.State = EntityState.Detached;
        else if (entry.State is not (EntityState.Detached or EntityState.Deleted))
            db.Remove(form);
    }

    /// <summary>
    /// Pending текущего отчёта/типа: Added, tracked Unchanged/Modified по Id, Deleted Ids.
    /// </summary>
    public static void CollectPending(
        DBModel db, int reportId, string formNum,
        out List<Form> added,
        out Dictionary<int, Form> trackedById,
        out HashSet<int> excludeIds)
    {
        added = [];
        trackedById = [];
        excludeIds = [];
        if (db == null || reportId <= 0)
            return;

        foreach (var entry in db.ChangeTracker.Entries())
        {
            if (entry.Entity is not Form form)
                continue;
            if (form.ReportId != reportId)
                continue;
            if (!string.IsNullOrEmpty(form.FormNum_DB) && form.FormNum_DB != formNum)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    added.Add(form);
                    break;
                case EntityState.Deleted when form.Id > 0:
                    excludeIds.Add(form.Id);
                    break;
                case EntityState.Modified or EntityState.Unchanged when form.Id > 0:
                    trackedById[form.Id] = form;
                    break;
            }
        }
    }

    public static int GetPendingDelta(DBModel db, int reportId, string formNum)
    {
        CollectPending(db, reportId, formNum, out var added, out _, out var exclude);
        return added.Count - exclude.Count;
    }

    public static List<Form> GetAddedForms(DBModel db, int reportId, string formNum)
    {
        CollectPending(db, reportId, formNum, out var added, out _, out _);
        return added;
    }

    /// <summary>
    /// Живой порядок: Id из БД без Deleted, Added вставлены по InsertBeforeId / InsertBeforeForm.
    /// </summary>
    public static List<LiveSlot> BuildLiveSlots(
        IReadOnlyList<int> dbIdsOrdered,
        IReadOnlyList<Form> added,
        IReadOnlySet<int> excludeIds)
    {
        var list = new List<LiveSlot>((dbIdsOrdered?.Count ?? 0) + (added?.Count ?? 0));
        if (dbIdsOrdered != null)
        {
            foreach (var id in dbIdsOrdered)
            {
                if (excludeIds != null && excludeIds.Contains(id))
                    continue;
                list.Add(new LiveSlot(id, null));
            }
        }

        if (added == null)
            return list;

        foreach (var form in added)
        {
            if (form == null)
                continue;
            list.Insert(IndexToInsert(list, form), new LiveSlot(null, form));
        }

        return list;
    }

    public static List<Form> PreferTracked(
        IReadOnlyList<Form> page, IReadOnlyDictionary<int, Form> trackedById)
    {
        if (page == null || page.Count == 0)
            return [];
        if (trackedById == null || trackedById.Count == 0)
            return [.. page];

        var result = new List<Form>(page.Count);
        foreach (var form in page)
        {
            if (form.Id > 0 && trackedById.TryGetValue(form.Id, out var tracked))
                result.Add(tracked);
            else
                result.Add(form);
        }

        return result;
    }

    private static int IndexToInsert(List<LiveSlot> list, Form added)
    {
        if (added.InsertBeforeForm != null)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (ReferenceEquals(list[i].Added, added.InsertBeforeForm))
                    return i;
                if (added.InsertBeforeForm.Id > 0 && list[i].Id == added.InsertBeforeForm.Id)
                    return i;
            }
        }

        if (added.InsertBeforeId is > 0)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Id == added.InsertBeforeId)
                    return i;
            }
        }

        return list.Count;
    }

    private static List<Form> FindInCollectionById(Report report, int formId)
    {
        var list = new List<Form>();
        foreach (var key in report.Rows.GetEnumerable().ToList())
        {
            if (key is Form f && f.Id == formId)
                list.Add(f);
        }

        return list;
    }

    private static Form? FindTrackedById(DBModel db, int formId)
    {
        foreach (var entry in db.ChangeTracker.Entries())
        {
            if (entry.Entity is Form f && f.Id == formId)
                return f;
        }

        return null;
    }
}
