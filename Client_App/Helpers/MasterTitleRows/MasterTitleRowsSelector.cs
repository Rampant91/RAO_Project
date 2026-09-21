using System;
using System.Collections.Generic;
using System.Linq;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Helpers.MasterTitleRows;

/// <summary>
/// Выбор канонической пары Ord=1 / Ord=2 по полноте полей (общий для конвертера и импорта).
/// При нескольких заполненных в одном order берётся лучшая; в warnings — пометка.
/// </summary>
public static class MasterTitleRowsSelector
{
    public static (Form10? Legal, Form10? Division, IReadOnlyList<string> Warnings) SelectForm10(
        IEnumerable<Form10>? rows)
    {
        var list = rows?.Where(static r => r is not null).ToList() ?? [];
        var warnings = new List<string>();
        var legal = PickBest(list, order: 1, "Form 1.0", warnings);
        var division = PickBest(list, order: 2, "Form 1.0", warnings);
        return (legal, division, warnings);
    }

    public static (Form20? Legal, Form20? Division, IReadOnlyList<string> Warnings) SelectForm20(
        IEnumerable<Form20>? rows)
    {
        var list = rows?.Where(static r => r is not null).ToList() ?? [];
        var warnings = new List<string>();
        var legal = PickBest(list, order: 1, "Form 2.0", warnings);
        var division = PickBest(list, order: 2, "Form 2.0", warnings);
        return (legal, division, warnings);
    }

    private static Form10? PickBest(List<Form10> rows, int order, string label, List<string> warnings)
    {
        var candidates = rows.Where(r => r.NumberInOrder_DB == order).ToList();
        if (candidates.Count == 0)
        {
            return null;
        }

        var ranked = candidates
            .Select(r => (Row: r, Score: MasterTitleRowFullness.Score(r)))
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Row.Id)
            .ToList();

        if (ranked.Count(x => x.Score > 0) > 1)
        {
            warnings.Add(
                $"{label}: несколько заполненных строк с NumberInOrder={order}, " +
                $"выбрана Id={ranked[0].Row.Id}.");
        }

        return ranked[0].Row;
    }

    private static Form20? PickBest(List<Form20> rows, int order, string label, List<string> warnings)
    {
        var candidates = rows.Where(r => r.NumberInOrder_DB == order).ToList();
        if (candidates.Count == 0)
        {
            return null;
        }

        var ranked = candidates
            .Select(r => (Row: r, Score: MasterTitleRowFullness.Score(r)))
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Row.Id)
            .ToList();

        if (ranked.Count(x => x.Score > 0) > 1)
        {
            warnings.Add(
                $"{label}: несколько заполненных строк с NumberInOrder={order}, " +
                $"выбрана Id={ranked[0].Row.Id}.");
        }

        return ranked[0].Row;
    }
}
