using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41.Testing;

namespace Test.Pairing41;

/// <summary>Группа V — smoke: идеальные пары, пустой результат.</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> ValidCases()
    {
        yield return V01_AllPaired_EmptyResult();
    }

    /// <summary>V01. Идеальные пары 1.1↔1.5 и 1.2↔1.6 → непарных нет.</summary>
    private static Pairing41TestCase V01_AllPaired_EmptyResult() => new()
    {
        Name = "V01. Идеальные пары по всем направлениям, непарных нет.",
        // 1.1 Id=1 ↔ 1.5 Id=101; 1.2 Id=2 ↔ 1.6 Id=202
        Form11 = [Row11(1)],
        Form15 = [Row11(101)],
        Form12 = [Row12(2)],
        Form16 = [Row16From12(202)],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired14 = [],
        ExpectedUnpaired15 = [],
        ExpectedUnpaired16 = []
    };
}
