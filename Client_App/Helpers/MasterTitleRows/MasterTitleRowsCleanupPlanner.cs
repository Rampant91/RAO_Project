using System.Collections.Generic;
using System.Linq;

namespace Client_App.Helpers.MasterTitleRows;

/// <summary>
/// Ссылка на строку титула для планирования очистки (без привязки к EF).
/// </summary>
public readonly record struct MasterTitleRowRef(int Id, int? ReportId, int NumberInOrder, int Score);

/// <summary>
/// Конфликт: несколько заполненных строк с одним NumberInOrder — ничего не удаляем в этом слоте.
/// </summary>
public readonly record struct MasterTitleRowsCleanupConflict(
    int ReportId,
    int NumberInOrder,
    IReadOnlyList<int> FilledIds);

/// <summary>
/// План безопасной очистки: только удаление пустых дублей.
/// </summary>
public sealed class MasterTitleRowsCleanupPlan
{
    public MasterTitleRowsCleanupPlan(
        IReadOnlyList<int> idsToDelete,
        IReadOnlyList<MasterTitleRowsCleanupConflict> conflicts)
    {
        IdsToDelete = idsToDelete;
        Conflicts = conflicts;
    }

    public IReadOnlyList<int> IdsToDelete { get; }
    public IReadOnlyList<MasterTitleRowsCleanupConflict> Conflicts { get; }
}

/// <summary>
/// Правила v1: удалять только пустые (score==0) дубли NumberInOrder 1/2 на одном ReportId.
/// При ≥2 заполненных в одном order — skip удаления в этом order.
/// </summary>
public static class MasterTitleRowsCleanupPlanner
{
    public const int CurrentRulesVersion = 1;

    /// <summary>
    /// Есть ли Master с более чем двумя строками титула (быстрый dirty-check).
    /// Строки с ReportId == null не учитываются.
    /// </summary>
    public static bool HasExcessRows(IEnumerable<MasterTitleRowRef> rows) =>
        rows
            .Where(static r => r.ReportId is not null)
            .GroupBy(static r => r.ReportId!.Value)
            .Any(static g => g.Count() > 2);

    public static MasterTitleRowsCleanupPlan Plan(IEnumerable<MasterTitleRowRef> rows)
    {
        var delete = new List<int>();
        var conflicts = new List<MasterTitleRowsCleanupConflict>();

        foreach (var group in rows
                     .Where(static r => r.ReportId is not null)
                     .GroupBy(static r => r.ReportId!.Value))
        {
            PlanReport(group.Key, group.ToList(), delete, conflicts);
        }

        return new MasterTitleRowsCleanupPlan(delete, conflicts);
    }

    private static void PlanReport(
        int reportId,
        List<MasterTitleRowRef> reportRows,
        List<int> delete,
        List<MasterTitleRowsCleanupConflict> conflicts)
    {
        foreach (var order in new[] { 1, 2 })
        {
            var candidates = reportRows.Where(r => r.NumberInOrder == order).ToList();
            if (candidates.Count <= 1)
            {
                continue;
            }

            var filled = candidates.Where(static c => c.Score > 0).ToList();
            var empty = candidates.Where(static c => c.Score == 0).ToList();

            if (filled.Count >= 2)
            {
                conflicts.Add(new MasterTitleRowsCleanupConflict(
                    reportId,
                    order,
                    filled.Select(static f => f.Id).OrderBy(static id => id).ToList()));
                continue;
            }

            if (filled.Count == 1)
            {
                delete.AddRange(empty.Select(static e => e.Id));
                continue;
            }

            // Все пустые: оставляем меньший Id.
            var keepId = candidates.Min(static c => c.Id);
            delete.AddRange(candidates.Where(c => c.Id != keepId).Select(static c => c.Id));
        }
    }
}
