using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;
using Xunit;

namespace Test.Snk;

internal static class SnkStockAssertions
{
    /// <summary>
    /// Сравнивает СНК по ключевым полям учётной единицы: паспорт, тип, радионуклиды,
    /// заводской номер, номер УКТ и количество. OpCode и OpDate не сравниваются.
    /// </summary>
    public static void Equal(IReadOnlyList<SnkStockSnapshot> expected, IReadOnlyList<SnkStockSnapshot> actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        foreach (var exp in expected)
        {
            var act = actual.SingleOrDefault(x => UnitKeyEquals(exp, x));
            Assert.True(
                act is not null,
                $"В СНК не найдена единица {FormatUnitKey(exp)}.");

            Assert.Equal(exp.Quantity, act!.Quantity);
        }
    }

    private static bool UnitKeyEquals(SnkStockSnapshot left, SnkStockSnapshot right) =>
        left.PasNum == right.PasNum
        && left.FacNum == right.FacNum
        && left.Type == right.Type
        && left.Radionuclids == right.Radionuclids
        && left.PackNumber == right.PackNumber;

    private static string FormatUnitKey(SnkStockSnapshot unit) =>
        $"{unit.PasNum}/{unit.FacNum} {unit.Type} {unit.Radionuclids} УКТ={unit.PackNumber}";
}
