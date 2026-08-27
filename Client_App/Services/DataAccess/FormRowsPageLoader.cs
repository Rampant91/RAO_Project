using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Interfaces;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Постраничная загрузка строк форм 1.x из БД (без полного Include всех строк).
/// Страницы грузятся AsNoTracking и затем Attach как Unchanged к tracked Report.
/// При pending Added/Deleted — живой Skip/Take после merge, не SQL Skip по сырому снимку.
/// </summary>
public static class FormRowsPageLoader
{
    public static async Task<int> CountAsync(DBModel db, int reportId, string formNum, CancellationToken ct = default)
    {
        return formNum switch
        {
            "1.1" => await db.form_11.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            "1.2" => await db.form_12.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            "1.3" => await db.form_13.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            "1.4" => await db.form_14.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            "1.5" => await db.form_15.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            "1.6" => await db.form_16.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            "1.7" => await db.form_17.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            "1.8" => await db.form_18.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            "1.9" => await db.form_19.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),
            _ => 0
        };
    }

    public static async Task<int> GetMaxNumberInOrderAsync(
        DBModel db, int reportId, string formNum, CancellationToken ct = default)
    {
        return formNum switch
        {
            "1.1" => await db.form_11.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            "1.2" => await db.form_12.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            "1.3" => await db.form_13.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            "1.4" => await db.form_14.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            "1.5" => await db.form_15.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            "1.6" => await db.form_16.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            "1.7" => await db.form_17.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            "1.8" => await db.form_18.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            "1.9" => await db.form_19.AsNoTracking().Where(f => f.ReportId == reportId)
                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,
            _ => 0
        };
    }

    public static bool SupportsDbPaging(string formNum) =>
        formNum is "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8" or "1.9";

    /// <summary>
    /// Загружает страницу строк без записи в Report (для prefetch-кэша).
    /// </summary>
    public static async Task<List<Form>> LoadPageListAsync(
        DBModel db, int reportId, string formNum, int skip, int take, CancellationToken ct = default)
    {
        return formNum switch
        {
            "1.1" => (await PageAsync(db.form_11, reportId, skip, take, ct)).Cast<Form>().ToList(),
            "1.2" => (await PageAsync(db.form_12, reportId, skip, take, ct)).Cast<Form>().ToList(),
            "1.3" => (await PageAsync(db.form_13, reportId, skip, take, ct)).Cast<Form>().ToList(),
            "1.4" => (await PageAsync(db.form_14, reportId, skip, take, ct)).Cast<Form>().ToList(),
            "1.5" => (await PageAsync(db.form_15, reportId, skip, take, ct)).Cast<Form>().ToList(),
            "1.6" => (await PageAsync(db.form_16, reportId, skip, take, ct)).Cast<Form>().ToList(),
            "1.7" => (await PageAsync(db.form_17, reportId, skip, take, ct)).Cast<Form>().ToList(),
            "1.8" => (await PageAsync(db.form_18, reportId, skip, take, ct)).Cast<Form>().ToList(),
            "1.9" => (await PageAsync(db.form_19, reportId, skip, take, ct)).Cast<Form>().ToList(),
            _ => []
        };
    }

    public static async Task<List<int>> LoadOrderedIdsAsync(
        DBModel db, int reportId, string formNum, CancellationToken ct = default)
    {
        return formNum switch
        {
            "1.1" => await db.form_11.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            "1.2" => await db.form_12.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            "1.3" => await db.form_13.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            "1.4" => await db.form_14.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            "1.5" => await db.form_15.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            "1.6" => await db.form_16.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            "1.7" => await db.form_17.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            "1.8" => await db.form_18.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            "1.9" => await db.form_19.AsNoTracking().Where(f => f.ReportId == reportId)
                .OrderBy(f => f.NumberInOrder_DB).ThenBy(f => f.Id).Select(f => f.Id).ToListAsync(ct),
            _ => []
        };
    }

    public static async Task<List<(int Id, int Number)>> LoadOrderedIdNumbersAsync(
        DBModel db, int reportId, string formNum, CancellationToken ct = default)
    {
        return formNum switch
        {
            "1.1" => await ProjectIdNumbers(db.form_11, reportId, ct),
            "1.2" => await ProjectIdNumbers(db.form_12, reportId, ct),
            "1.3" => await ProjectIdNumbers(db.form_13, reportId, ct),
            "1.4" => await ProjectIdNumbers(db.form_14, reportId, ct),
            "1.5" => await ProjectIdNumbers(db.form_15, reportId, ct),
            "1.6" => await ProjectIdNumbers(db.form_16, reportId, ct),
            "1.7" => await ProjectIdNumbers(db.form_17, reportId, ct),
            "1.8" => await ProjectIdNumbers(db.form_18, reportId, ct),
            "1.9" => await ProjectIdNumbers(db.form_19, reportId, ct),
            _ => []
        };
    }

    public static async Task<List<Form>> LoadByIdsAsync(
        DBModel db, string formNum, IReadOnlyList<int> ids, CancellationToken ct = default)
    {
        if (ids == null || ids.Count == 0)
            return [];

        var loaded = formNum switch
        {
            "1.1" => await QueryByIds(db.form_11, ids, ct),
            "1.2" => await QueryByIds(db.form_12, ids, ct),
            "1.3" => await QueryByIds(db.form_13, ids, ct),
            "1.4" => await QueryByIds(db.form_14, ids, ct),
            "1.5" => await QueryByIds(db.form_15, ids, ct),
            "1.6" => await QueryByIds(db.form_16, ids, ct),
            "1.7" => await QueryByIds(db.form_17, ids, ct),
            "1.8" => await QueryByIds(db.form_18, ids, ct),
            "1.9" => await QueryByIds(db.form_19, ids, ct),
            _ => []
        };

        var byId = new Dictionary<int, Form>(loaded.Count);
        foreach (var form in loaded)
            byId.TryAdd(form.Id, form);

        var ordered = new List<Form>(ids.Count);
        foreach (var id in ids)
        {
            if (byId.TryGetValue(id, out var form))
                ordered.Add(form);
        }

        return ordered;
    }

    public static async Task<List<(int Id, string Code)>> LoadIdAndOperationCodesAsync(
        DBModel db, int reportId, string formNum, CancellationToken ct = default)
    {
        return formNum switch
        {
            "1.1" => await ProjectOpCodes(db.form_11, reportId, ct),
            "1.2" => await ProjectOpCodes(db.form_12, reportId, ct),
            "1.3" => await ProjectOpCodes(db.form_13, reportId, ct),
            "1.4" => await ProjectOpCodes(db.form_14, reportId, ct),
            _ => []
        };
    }

    /// <summary>
    /// Текущая страница живого набора: Deleted исключены, Added вставлены, Modified — tracked instance.
    /// </summary>
    public static async Task<List<Form>> LoadMergedVisiblePageAsync(
        DBModel uiDb, int reportId, string formNum, int skip, int take, CancellationToken ct = default)
    {
        FormRowMutationService.CollectPending(uiDb, reportId, formNum,
            out var added, out var trackedById, out var excludeIds);

        if (added.Count == 0 && excludeIds.Count == 0)
        {
            List<Form> page;
            await using (var snap = new DBModel(StaticConfiguration.DBPath))
            {
                page = await LoadPageListAsync(snap, reportId, formNum, skip, take, ct)
                    .ConfigureAwait(false);
            }

            return FormRowMutationService.PreferTracked(page, trackedById);
        }

        List<int> dbIds;
        await using (var snap = new DBModel(StaticConfiguration.DBPath))
        {
            dbIds = await LoadOrderedIdsAsync(snap, reportId, formNum, ct).ConfigureAwait(false);
        }

        var slots = FormRowMutationService.BuildLiveSlots(dbIds, added, excludeIds);
        if (skip >= slots.Count || take <= 0)
            return [];

        var pageSlots = slots.Skip(Math.Max(0, skip)).Take(take).ToList();
        var idsToLoad = new List<int>();
        foreach (var slot in pageSlots)
        {
            if (slot.Added != null)
                continue;
            if (slot.Id is int id && id > 0 && !trackedById.ContainsKey(id))
                idsToLoad.Add(id);
        }

        var loaded = new Dictionary<int, Form>();
        if (idsToLoad.Count > 0)
        {
            await using var snap = new DBModel(StaticConfiguration.DBPath);
            var forms = await LoadByIdsAsync(snap, formNum, idsToLoad, ct).ConfigureAwait(false);
            foreach (var form in forms)
                loaded[form.Id] = form;
        }

        var result = new List<Form>(pageSlots.Count);
        foreach (var slot in pageSlots)
        {
            if (slot.Added != null)
                result.Add(slot.Added);
            else if (slot.Id is int id)
            {
                if (trackedById.TryGetValue(id, out var keep))
                    result.Add(keep);
                else if (loaded.TryGetValue(id, out var form))
                    result.Add(form);
            }
        }

        return result;
    }

    /// <summary>
    /// Все живые строки (для паспорта 1.7): снимок AsNoTracking + pending, без Apply в UI-отчёт.
    /// </summary>
    public static async Task<List<Form>> LoadAllLiveRowsAsync(
        DBModel uiDb, int reportId, string formNum, CancellationToken ct = default)
    {
        FormRowMutationService.CollectPending(uiDb, reportId, formNum,
            out var added, out var trackedById, out var excludeIds);

        List<int> dbIds;
        await using (var snap = new DBModel(StaticConfiguration.DBPath))
        {
            dbIds = await LoadOrderedIdsAsync(snap, reportId, formNum, ct).ConfigureAwait(false);
        }

        var slots = FormRowMutationService.BuildLiveSlots(dbIds, added, excludeIds);
        var idsToLoad = new List<int>();
        foreach (var slot in slots)
        {
            if (slot.Added != null)
                continue;
            if (slot.Id is int id && id > 0 && !trackedById.ContainsKey(id))
                idsToLoad.Add(id);
        }

        var loaded = new Dictionary<int, Form>();
        if (idsToLoad.Count > 0)
        {
            await using var snap = new DBModel(StaticConfiguration.DBPath);
            var forms = await LoadByIdsAsync(snap, formNum, idsToLoad, ct).ConfigureAwait(false);
            foreach (var form in forms)
                loaded[form.Id] = form;
        }

        var result = new List<Form>(slots.Count);
        foreach (var slot in slots)
        {
            if (slot.Added != null)
                result.Add(slot.Added);
            else if (slot.Id is int id)
            {
                if (trackedById.TryGetValue(id, out var keep))
                    result.Add(keep);
                else if (loaded.TryGetValue(id, out var form))
                    result.Add(form);
            }
        }

        return result;
    }

    public static void ApplyPageToReport(Report report, string formNum, IReadOnlyList<Form> items) =>
        ApplyPageToReport(StaticConfiguration.DBModel, report, formNum, items);

    public static void ApplyPageToReport(
        DBModel db, Report report, string formNum, IReadOnlyList<Form> items)
    {
        EnsureReportTracked(db, report);

        FormRowMutationService.CollectPending(db, report.Id, formNum,
            out _, out var trackedById, out _);
        var resolved = FormRowMutationService.PreferTracked(items, trackedById);

        var keepIds = new HashSet<int>();
        foreach (var form in resolved)
        {
            if (form.Id > 0)
                keepIds.Add(form.Id);
        }

        DetachUnchangedNotInKeep(db, report, formNum, keepIds);

        switch (formNum)
        {
            case "1.1":
                Replace(report.Rows11, resolved.Cast<Form11>().ToList());
                break;
            case "1.2":
                Replace(report.Rows12, resolved.Cast<Form12>().ToList());
                break;
            case "1.3":
                Replace(report.Rows13, resolved.Cast<Form13>().ToList());
                break;
            case "1.4":
                Replace(report.Rows14, resolved.Cast<Form14>().ToList());
                break;
            case "1.5":
                Replace(report.Rows15, resolved.Cast<Form15>().ToList());
                break;
            case "1.6":
                Replace(report.Rows16, resolved.Cast<Form16>().ToList());
                break;
            case "1.7":
                Replace(report.Rows17, resolved.Cast<Form17>().ToList());
                break;
            case "1.8":
                Replace(report.Rows18, resolved.Cast<Form18>().ToList());
                break;
            case "1.9":
                Replace(report.Rows19, resolved.Cast<Form19>().ToList());
                break;
        }

        AttachPageForms(db, report, formNum);
    }

    /// <summary>
    /// Загружает страницу строк в коллекцию отчёта, заменяя текущее содержимое.
    /// </summary>
    public static async Task LoadPageIntoReportAsync(
        DBModel db, Report report, string formNum, int skip, int take, CancellationToken ct = default)
    {
        var items = await LoadMergedVisiblePageAsync(db, report.Id, formNum, skip, take, ct);
        ApplyPageToReport(db, report, formNum, items);
    }

    /// <summary>
    /// Регистрирует новую строку в ChangeTracker (критично при paging / AsNoTracking-оболочках).
    /// </summary>
    public static void TrackNewFormRow(DBModel db, Report report, Form form)
    {
        EnsureReportTracked(db, report);
        form.Report = report;
        form.ReportId = report.Id;
        var entry = db.Entry(form);
        if (entry.State == EntityState.Detached)
            db.Add(form);
    }

    /// <summary>
    /// Помечает строку Deleted и убирает из коллекции (нужно при Attach-страницах paging).
    /// </summary>
    public static void TrackDeletedFormRow(DBModel db, Report report, Form form)
    {
        if (form.Id > 0)
            FormRowMutationService.RemoveFormRowById(db, report, form.Id);
        else
            FormRowMutationService.RemoveFormRowInstance(db, report, form);
    }

    /// <summary>
    /// Экземпляр строки из Report.Rows / ChangeTracker (после reload Ensure не совпадает с selection).
    /// </summary>
    public static Form ResolveFormInstance(DBModel db, Report report, Form form)
    {
        if (form.Id > 0)
        {
            var tracked = FindTrackedFormById(db, form.Id);
            if (tracked != null)
                return tracked;

            foreach (var key in report.Rows)
            {
                if (key is Form row && row.Id == form.Id)
                    return row;
            }
        }

        return form;
    }

    /// <summary>
    /// Есть ли незакоммиченные изменения строк данной формы отчёта.
    /// </summary>
    public static bool HasPendingFormRowChanges(DBModel db, int reportId, string formNum)
    {
        if (db == null || reportId <= 0 || string.IsNullOrEmpty(formNum))
            return false;

        foreach (var entry in db.ChangeTracker.Entries())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
                continue;
            if (entry.Entity is not Form form)
                continue;
            if (form.ReportId != reportId)
                continue;
            if (!string.IsNullOrEmpty(form.FormNum_DB) && form.FormNum_DB != formNum)
                continue;
            return true;
        }

        return false;
    }

    public static bool HasPendingAddOrDelete(DBModel db, int reportId, string formNum)
    {
        if (db == null || reportId <= 0 || string.IsNullOrEmpty(formNum))
            return false;

        foreach (var entry in db.ChangeTracker.Entries())
        {
            if (entry.State is not (EntityState.Added or EntityState.Deleted))
                continue;
            if (entry.Entity is not Form form)
                continue;
            if (form.ReportId != reportId)
                continue;
            if (!string.IsNullOrEmpty(form.FormNum_DB) && form.FormNum_DB != formNum)
                continue;
            return true;
        }

        return false;
    }

    private static Form? FindTrackedFormById(DBModel db, int id)
    {
        if (id <= 0)
            return null;

        foreach (var entry in db.ChangeTracker.Entries())
        {
            if (entry.Entity is Form f && f.Id == id)
                return f;
        }

        return null;
    }

    private static async Task<List<T>> PageAsync<T>(
        DbSet<T> set, int reportId, int skip, int take, CancellationToken ct)
        where T : Form
    {
        return await set
            .AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);
    }

    private static async Task<List<(int Id, int Number)>> ProjectIdNumbers<T>(
        DbSet<T> set, int reportId, CancellationToken ct)
        where T : Form
    {
        var rows = await set.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .Select(f => new { f.Id, f.NumberInOrder_DB })
            .ToListAsync(ct)
            .ConfigureAwait(false);
        return rows.Select(x => (x.Id, x.NumberInOrder_DB)).ToList();
    }

    private static async Task<List<(int Id, string Code)>> ProjectOpCodes<T>(
        DbSet<T> set, int reportId, CancellationToken ct)
        where T : Form1
    {
        var rows = await set.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .Select(f => new { f.Id, f.OperationCode_DB })
            .ToListAsync(ct)
            .ConfigureAwait(false);
        return rows.Select(x => (x.Id, x.OperationCode_DB ?? "")).ToList();
    }

    private static async Task<List<Form>> QueryByIds<T>(
        DbSet<T> set, IReadOnlyList<int> ids, CancellationToken ct)
        where T : Form
    {
        return await FirebirdInClause.QueryAllAsync(ids, async batch =>
        {
            var rows = await set.AsNoTracking()
                .Where(f => batch.Contains(f.Id))
                .ToListAsync(ct)
                .ConfigureAwait(false);
            return rows.Cast<Form>().ToList();
        }).ConfigureAwait(false);
    }

    private static void Replace<T>(ObservableCollectionWithItemPropertyChanged<T> target, List<T> items)
        where T : class, IKey
    {
        target.Clear();
        target.AddRange(items);
    }

    private static void EnsureReportTracked(DBModel db, Report report)
    {
        var entry = db.Entry(report);
        if (entry.State != EntityState.Detached)
            return;

        var local = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == report.Id);
        if (local != null && !ReferenceEquals(local, report))
        {
            // Уже есть другой tracked экземпляр — не Attach дубликат.
            return;
        }

        db.Attach(report);
    }

    private static void DetachUnchangedNotInKeep(
        DBModel db, Report report, string formNum, IReadOnlySet<int> keepIds)
    {
        foreach (var entry in db.ChangeTracker.Entries().ToList())
        {
            if (entry.Entity is not Form form)
                continue;
            if (form.ReportId != report.Id)
                continue;
            if (!string.IsNullOrEmpty(form.FormNum_DB) && form.FormNum_DB != formNum)
                continue;
            if (entry.State != EntityState.Unchanged)
                continue;
            if (form.Id > 0 && keepIds.Contains(form.Id))
                continue;
            entry.State = EntityState.Detached;
        }
    }

    private static void AttachPageForms(DBModel db, Report report, string formNum)
    {
        foreach (var form in EnumerateFormRows(report, formNum))
        {
            form.ReportId = report.Id;
            form.Report = report;
            var entry = db.Entry(form);
            if (entry.State != EntityState.Detached)
                continue;

            if (form.Id > 0)
                db.Attach(form);
            else
                db.Add(form);
        }
    }

    private static IEnumerable<Form> EnumerateFormRows(Report report, string formNum) =>
        formNum switch
        {
            "1.1" => report.Rows11.Cast<Form>(),
            "1.2" => report.Rows12.Cast<Form>(),
            "1.3" => report.Rows13.Cast<Form>(),
            "1.4" => report.Rows14.Cast<Form>(),
            "1.5" => report.Rows15.Cast<Form>(),
            "1.6" => report.Rows16.Cast<Form>(),
            "1.7" => report.Rows17.Cast<Form>(),
            "1.8" => report.Rows18.Cast<Form>(),
            "1.9" => report.Rows19.Cast<Form>(),
            _ => Enumerable.Empty<Form>()
        };
}
