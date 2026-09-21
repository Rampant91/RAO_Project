using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Helpers.MasterTitleRows;

/// <summary>
/// Результат попытки очистки титулов Form 1.0 / 2.0 при старте.
/// </summary>
public sealed class MasterTitleRowsCleanupResult
{
    public bool Ran { get; init; }
    public bool Failed { get; init; }
    public int DeletedForm10 { get; init; }
    public int DeletedForm20 { get; init; }
    public int ConflictCount { get; init; }

    public static MasterTitleRowsCleanupResult Skipped { get; } = new() { Ran = false };

    public static MasterTitleRowsCleanupResult FromFailure() => new() { Ran = true, Failed = true };
}

/// <summary>
/// Применение безопасной очистки пустых дублей form_10 / form_20 на уже загруженном контексте.
/// </summary>
public static class MasterTitleRowsCleanupRunner
{
    /// <summary>
    /// Есть ли пустые дубли, которые v1 может безопасно удалить.
    /// Conflict-only Master (две заполненные) сюда не попадают — без лишнего прогресса на каждом старте.
    /// </summary>
    public static bool NeedsCleanup(DataContext db)
    {
        var form10Local = db.form_10.Local.ToList();
        var form20Local = db.form_20.Local.ToList();

        var refs10 = form10Local
            .Select(static r => new MasterTitleRowRef(
                r.Id, r.ReportId, r.NumberInOrder_DB, MasterTitleRowFullness.Score(r)))
            .ToList();
        var refs20 = form20Local
            .Select(static r => new MasterTitleRowRef(
                r.Id, r.ReportId, r.NumberInOrder_DB, MasterTitleRowFullness.Score(r)))
            .ToList();

        if (!MasterTitleRowsCleanupPlanner.HasExcessRows(refs10)
            && !MasterTitleRowsCleanupPlanner.HasExcessRows(refs20))
        {
            return false;
        }

        var plan10 = MasterTitleRowsCleanupPlanner.Plan(refs10);
        var plan20 = MasterTitleRowsCleanupPlanner.Plan(refs20);
        return plan10.IdsToDelete.Count > 0 || plan20.IdsToDelete.Count > 0;
    }

    /// <summary>
    /// Если нечего удалять — сразу выход.
    /// Ошибка не пробрасывается: пишется в лог, старт приложения продолжается.
    /// </summary>
    public static async Task<MasterTitleRowsCleanupResult> TryRunAsync(
        DataContext db,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var form10Local = db.form_10.Local.ToList();
            var form20Local = db.form_20.Local.ToList();

            var refs10 = form10Local
                .Select(static r => new MasterTitleRowRef(
                    r.Id,
                    r.ReportId,
                    r.NumberInOrder_DB,
                    MasterTitleRowFullness.Score(r)))
                .ToList();
            var refs20 = form20Local
                .Select(static r => new MasterTitleRowRef(
                    r.Id,
                    r.ReportId,
                    r.NumberInOrder_DB,
                    MasterTitleRowFullness.Score(r)))
                .ToList();

            if (!MasterTitleRowsCleanupPlanner.HasExcessRows(refs10)
                && !MasterTitleRowsCleanupPlanner.HasExcessRows(refs20))
            {
                return MasterTitleRowsCleanupResult.Skipped;
            }

            var plan10 = MasterTitleRowsCleanupPlanner.Plan(refs10);
            var plan20 = MasterTitleRowsCleanupPlanner.Plan(refs20);

            if (plan10.IdsToDelete.Count == 0 && plan20.IdsToDelete.Count == 0)
            {
                return MasterTitleRowsCleanupResult.Skipped;
            }

            var deleted10 = RemoveForm10(db, form10Local, plan10.IdsToDelete);
            var deleted20 = RemoveForm20(db, form20Local, plan20.IdsToDelete);

            if (deleted10 > 0 || deleted20 > 0)
            {
                await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            var conflictCount = plan10.Conflicts.Count + plan20.Conflicts.Count;
            if (deleted10 > 0 || deleted20 > 0 || conflictCount > 0)
            {
                ServiceExtension.LoggerManager.Info(
                    $"{Environment.NewLine}MasterTitleRowsCleanup v{MasterTitleRowsCleanupPlanner.CurrentRulesVersion}: " +
                    $"form_10 removed {deleted10}, form_20 removed {deleted20}, conflicts {conflictCount}.",
                    ErrorCodeLogger.DataBase);
            }

            foreach (var c in plan10.Conflicts.Concat(plan20.Conflicts))
            {
                ServiceExtension.LoggerManager.Warning(
                    $"{Environment.NewLine}MasterTitleRowsCleanup conflict ReportId={c.ReportId} " +
                    $"NumberInOrder={c.NumberInOrder} filledIds=[{string.Join(",", c.FilledIds)}]",
                    ErrorCodeLogger.DataBase);
            }

            return new MasterTitleRowsCleanupResult
            {
                Ran = true,
                DeletedForm10 = deleted10,
                DeletedForm20 = deleted20,
                ConflictCount = conflictCount
            };
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}MasterTitleRowsCleanup failed." +
                      $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
            return MasterTitleRowsCleanupResult.FromFailure();
        }
    }

    private static int RemoveForm10(DataContext db, List<Form10> local, IReadOnlyList<int> ids)
    {
        if (ids.Count == 0)
        {
            return 0;
        }

        var idSet = ids.ToHashSet();
        var removed = 0;
        foreach (var row in local.Where(r => idSet.Contains(r.Id)).ToList())
        {
            DetachFromMasterRows10(db, row);
            db.form_10.Remove(row);
            removed++;
        }

        return removed;
    }

    private static int RemoveForm20(DataContext db, List<Form20> local, IReadOnlyList<int> ids)
    {
        if (ids.Count == 0)
        {
            return 0;
        }

        var idSet = ids.ToHashSet();
        var removed = 0;
        foreach (var row in local.Where(r => idSet.Contains(r.Id)).ToList())
        {
            DetachFromMasterRows20(db, row);
            db.form_20.Remove(row);
            removed++;
        }

        return removed;
    }

    private static void DetachFromMasterRows10(DataContext db, Form10 row)
    {
        if (row.Report is not null)
        {
            row.Report.Rows10.Remove(row);
            return;
        }

        if (row.ReportId is not int reportId)
        {
            return;
        }

        var master = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == reportId);
        master?.Rows10.Remove(row);
    }

    private static void DetachFromMasterRows20(DataContext db, Form20 row)
    {
        if (row.Report is not null)
        {
            row.Report.Rows20.Remove(row);
            return;
        }

        if (row.ReportId is not int reportId)
        {
            return;
        }

        var master = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == reportId);
        master?.Rows20.Remove(row);
    }
}
