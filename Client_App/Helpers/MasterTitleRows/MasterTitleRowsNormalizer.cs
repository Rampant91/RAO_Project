using System.Collections.Generic;
using System.Linq;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Helpers.MasterTitleRows;

/// <summary>
/// Схлопывание Rows10/Rows20 Master до канонической пары Ord=1 / Ord=2
/// (для импорта — правит коллекцию в памяти; после вызова [0]=юрлицо, [1]=обособленное).
/// <para>
/// В отличие от cleanup при старте: при нескольких заполненных в одном order
/// оставляет лучшую (и пишет warning), чтобы импорт не тащил 4 строки в БД.
/// </para>
/// </summary>
public static class MasterTitleRowsNormalizer
{
    /// <summary>
    /// Оставляет ровно две строки в порядке NumberInOrder 1, затем 2.
    /// </summary>
    public static IReadOnlyList<Form10> NormalizeForm10InPlace(Report master, out IReadOnlyList<string> warnings)
    {
        warnings = [];
        if (master is null)
        {
            return [];
        }

        var (legal, division, selectWarnings) = MasterTitleRowsSelector.SelectForm10(master.Rows10);
        warnings = selectWarnings;

        legal ??= CreateEmptyForm10(1);
        division ??= CreateEmptyForm10(2);
        legal.NumberInOrder_DB = 1;
        division.NumberInOrder_DB = 2;

        RebuildForm10(master, legal, division);
        return master.Rows10.ToList();
    }

    /// <summary>
    /// Оставляет ровно две строки Form 2.0 в порядке NumberInOrder 1, затем 2.
    /// </summary>
    public static IReadOnlyList<Form20> NormalizeForm20InPlace(Report master, out IReadOnlyList<string> warnings)
    {
        warnings = [];
        if (master is null)
        {
            return [];
        }

        var (legal, division, selectWarnings) = MasterTitleRowsSelector.SelectForm20(master.Rows20);
        warnings = selectWarnings;

        legal ??= CreateEmptyForm20(1);
        division ??= CreateEmptyForm20(2);
        legal.NumberInOrder_DB = 1;
        division.NumberInOrder_DB = 2;

        RebuildForm20(master, legal, division);
        return master.Rows20.ToList();
    }

    private static void RebuildForm10(Report master, Form10 legal, Form10 division)
    {
        master.Rows10.Clear();
        master.Rows10.Add(legal);
        master.Rows10.Add(division);
        master.Rows10.Sorted = false;
        master.Rows10.QuickSort();
    }

    private static void RebuildForm20(Report master, Form20 legal, Form20 division)
    {
        master.Rows20.Clear();
        master.Rows20.Add(legal);
        master.Rows20.Add(division);
        master.Rows20.Sorted = false;
        master.Rows20.QuickSort();
    }

    private static Form10 CreateEmptyForm10(int order)
    {
        var row = (Form10)FormCreator.Create("1.0");
        row.NumberInOrder_DB = order;
        return row;
    }

    private static Form20 CreateEmptyForm20(int order)
    {
        var row = (Form20)FormCreator.Create("2.0");
        row.NumberInOrder_DB = order;
        return row;
    }
}
