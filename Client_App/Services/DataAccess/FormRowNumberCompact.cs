using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Сплошная нумерация 1..n по каноническому порядку Id (как в гриде).
/// SQL-пачки, без 20k сущностей в ChangeTracker.
/// </summary>
public static class FormRowNumberCompact
{
    public const int SqlBatchSize = 200;

    /// <summary>Виртуальный № п/п на странице: (page-1)*size + index (1-based).</summary>
    public static int DisplayNumber(int currentPage, int rowCount, int indexOnPage)
    {
        var page = Math.Max(1, currentPage);
        var size = Math.Max(1, rowCount);
        var index = Math.Max(0, indexOnPage);
        return (page - 1) * size + index + 1;
    }

    public static int LiveCount(int dbCount, int addedCount, int deletedCount) =>
        Math.Max(0, dbCount - deletedCount + addedCount);

    public static bool NeedsCompact(IReadOnlyList<(int Id, int Number)> currentOrdered)
    {
        if (currentOrdered == null)
            return false;
        for (var i = 0; i < currentOrdered.Count; i++)
        {
            if (currentOrdered[i].Number != i + 1)
                return true;
        }

        return false;
    }

    public static List<(int Id, int Number)> Assignments(IReadOnlyList<int> orderedIds)
    {
        var list = new List<(int Id, int Number)>(orderedIds?.Count ?? 0);
        if (orderedIds == null)
            return list;
        for (var i = 0; i < orderedIds.Count; i++)
            list.Add((orderedIds[i], i + 1));
        return list;
    }

    /// <summary>
    /// Порядок Id после Save: живые из БД без только что вставленных,
    /// затем splice Added по InsertBeforeId / InsertBeforeForm.
    /// </summary>
    public static List<int> BuildCompactIdOrder(
        IReadOnlyList<int> dbIdsOrderedAfterSave,
        IReadOnlyList<Models.Forms.Form> newlySaved)
    {
        if (dbIdsOrderedAfterSave == null)
            return [];

        if (newlySaved == null || newlySaved.Count == 0)
            return dbIdsOrderedAfterSave as List<int> ?? [.. dbIdsOrderedAfterSave];

        var newIds = new HashSet<int>();
        foreach (var form in newlySaved)
        {
            if (form?.Id > 0)
                newIds.Add(form.Id);
        }

        var list = new List<int>(dbIdsOrderedAfterSave.Count);
        foreach (var id in dbIdsOrderedAfterSave)
        {
            if (!newIds.Contains(id))
                list.Add(id);
        }

        foreach (var form in newlySaved)
        {
            if (form == null || form.Id <= 0)
                continue;
            list.Insert(IndexToInsertSaved(list, form), form.Id);
        }

        return list;
    }

    public static string TableName(string formNum) => formNum switch
    {
        "1.1" => "form_11",
        "1.2" => "form_12",
        "1.3" => "form_13",
        "1.4" => "form_14",
        "1.5" => "form_15",
        "1.6" => "form_16",
        "1.7" => "form_17",
        "1.8" => "form_18",
        "1.9" => "form_19",
        _ => ""
    };

    public static async Task ApplySqlAsync(
        DBModel db,
        string formNum,
        int reportId,
        IReadOnlyList<(int Id, int Number)> assignments,
        Func<int, int, Task>? progress = null,
        CancellationToken ct = default)
    {
        var table = TableName(formNum);
        if (db == null || string.IsNullOrEmpty(table) || reportId <= 0 ||
            assignments == null || assignments.Count == 0)
            return;

        var total = assignments.Count;
        var done = 0;
        for (var offset = 0; offset < assignments.Count; offset += SqlBatchSize)
        {
            ct.ThrowIfCancellationRequested();
            var take = Math.Min(SqlBatchSize, assignments.Count - offset);
            var sql = BuildUpdateCaseSql(table, reportId, assignments, offset, take);
            await db.Database.ExecuteSqlRawAsync(sql, ct).ConfigureAwait(false);
            done += take;
            if (progress != null)
                await progress(done, total).ConfigureAwait(false);
        }
    }

    internal static string BuildUpdateCaseSql(
        string table,
        int reportId,
        IReadOnlyList<(int Id, int Number)> assignments,
        int offset,
        int take)
    {
        var sb = new StringBuilder(64 + take * 24);
        var ids = new StringBuilder(take * 12);
        sb.Append("UPDATE \"").Append(table).Append("\" SET \"NumberInOrder_DB\" = CASE \"Id\"");
        for (var i = 0; i < take; i++)
        {
            var (id, number) = assignments[offset + i];
            sb.Append(" WHEN ").Append(id).Append(" THEN ").Append(number);
            if (i > 0)
                ids.Append(',');
            ids.Append(id);
        }

        sb.Append(" END WHERE \"ReportId\" = ").Append(reportId)
            .Append(" AND \"Id\" IN (").Append(ids).Append(')');
        return sb.ToString();
    }

    private static int IndexToInsertSaved(List<int> list, Models.Forms.Form form)
    {
        if (form.InsertBeforeForm != null && form.InsertBeforeForm.Id > 0)
        {
            var i = list.IndexOf(form.InsertBeforeForm.Id);
            if (i >= 0)
                return i;
        }

        if (form.InsertBeforeId is > 0)
        {
            var i = list.IndexOf(form.InsertBeforeId.Value);
            if (i >= 0)
                return i;
        }

        return list.Count;
    }
}
