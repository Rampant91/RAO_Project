using System;
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
        Assert.True(
            expected.Count == actual.Count,
            $"Количество строк СНК не совпадает. Ожидалось: {expected.Count}, фактически: {actual.Count}." +
            $"{Environment.NewLine}Ожидалось:{Environment.NewLine}{FormatStock(expected)}" +
            $"{Environment.NewLine}Фактически:{Environment.NewLine}{FormatStock(actual)}");

        foreach (var exp in expected)
        {
            var act = actual.SingleOrDefault(x => UnitKeyEquals(exp, x));
            Assert.True(
                act is not null,
                $"В СНК не найдена единица {FormatUnitKey(exp)}." +
                $"{Environment.NewLine}Фактически:{Environment.NewLine}{FormatStock(actual)}");

            Assert.Equal(exp.Quantity, act!.Quantity);
        }
    }

    /// <summary>
    /// Проверяет, что любое расхождение «единица есть в инвентаризации, но нет в расчётном СНК»
    /// на указанную дату допустимо (присутствует в списке <paramref name="allowedDifference"/>).
    /// Например, ЗРИ проинвентаризировали и в тот же день передали — это не ошибка.
    /// </summary>
    public static void InventoryDifferenceIsAllowed(
        DateOnly date,
        IReadOnlyList<SnkStockSnapshot> inventoried,
        IReadOnlyList<SnkStockSnapshot> snk,
        IReadOnlyList<SnkStockSnapshot> allowedDifference)
    {
        var onlyInInventory = inventoried
            .Where(inv => !snk.Any(s => UnitKeyEquals(inv, s)))
            .ToList();

        foreach (var unit in onlyInInventory)
        {
            Assert.True(
                allowedDifference.Any(a => UnitKeyEquals(unit, a)),
                $"На дату {date:dd.MM.yyyy} единица {FormatUnitKey(unit)} есть в инвентаризации, " +
                $"но отсутствует в расчётном СНК, и это расхождение не указано как допустимое." +
                $"{Environment.NewLine}Инвентаризация:{Environment.NewLine}{FormatStock(inventoried)}" +
                $"{Environment.NewLine}СНК:{Environment.NewLine}{FormatStock(snk)}");
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

    private static string FormatStock(IReadOnlyList<SnkStockSnapshot> stock) =>
        string.Join(
            Environment.NewLine,
            stock.Select(x => $"- {FormatUnitKey(x)}, количество={x.Quantity}, op={x.OpCode}, дата={x.OpDate:dd.MM.yyyy}"));

    /// <summary>
    /// Сравнивает ожидаемые и фактические ошибки проверки инвентаризаций по датам.
    /// </summary>
    public static void InventoryErrorsEqual(
        IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkExpectedInventoryError>> expectedByDate,
        IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkActualInventoryError>> actualByDate)
    {
        foreach (var (date, expectedErrors) in expectedByDate)
        {
            Assert.True(
                actualByDate.ContainsKey(date),
                $"Проверка инвентаризаций не вернула список ошибок на дату {date:dd.MM.yyyy}.");

            var actualErrors = actualByDate[date];

            Assert.True(
                expectedErrors.Count == actualErrors.Count,
                $"На дату {date:dd.MM.yyyy} ожидалось ошибок: {expectedErrors.Count}, фактически: {actualErrors.Count}." +
                $"{Environment.NewLine}Ожидалось:{Environment.NewLine}{FormatExpectedErrors(expectedErrors)}" +
                $"{Environment.NewLine}Фактически:{Environment.NewLine}{FormatActualErrors(actualErrors)}");

            foreach (var expected in expectedErrors)
            {
                var matches = actualErrors.Where(actual => ErrorMatches(expected, actual)).ToList();
                Assert.True(
                    matches.Count > 0,
                    $"На дату {date:dd.MM.yyyy} не найдена ожидаемая ошибка {FormatExpectedError(expected)}." +
                    $"{Environment.NewLine}Фактически:{Environment.NewLine}{FormatActualErrors(actualErrors)}");
            }
        }

        var unexpectedErrors = actualByDate
            .Where(pair => pair.Value.Count > 0 && !expectedByDate.ContainsKey(pair.Key))
            .ToList();

        Assert.True(
            unexpectedErrors.Count == 0,
            "Обнаружены неожиданные ошибки на датах, не указанных в эталоне:" +
            string.Join(
                Environment.NewLine,
                unexpectedErrors.Select(pair =>
                    $"- {pair.Key:dd.MM.yyyy}:{Environment.NewLine}{FormatActualErrors(pair.Value)}")));
    }

    private static bool ErrorMatches(SnkExpectedInventoryError expected, SnkActualInventoryError actual) =>
        expected.ErrorType == actual.ErrorType
        && expected.PasNum == actual.PasNum
        && expected.FacNum == actual.FacNum
        && expected.Type == actual.Type
        && expected.Radionuclids == actual.Radionuclids
        && expected.PackNumber == actual.PackNumber
        && (expected.OpCode is null || expected.OpCode == actual.OpCode)
        && (expected.OpDate is null || expected.OpDate == actual.OpDate);

    private static string FormatExpectedError(SnkExpectedInventoryError error) =>
        $"тип={(int)error.ErrorType} {error.PasNum}/{error.FacNum} {error.Type} УКТ={error.PackNumber}";

    private static string FormatExpectedErrors(IReadOnlyList<SnkExpectedInventoryError> errors) =>
        string.Join(Environment.NewLine, errors.Select(x => $"- {FormatExpectedError(x)}"));

    private static string FormatActualErrors(IReadOnlyList<SnkActualInventoryError> errors) =>
        string.Join(
            Environment.NewLine,
            errors.Select(x =>
                $"- тип={(int)x.ErrorType} {x.PasNum}/{x.FacNum} {x.Type} УКТ={x.PackNumber}, op={x.OpCode}, дата={x.OpDate:dd.MM.yyyy}"));
}
