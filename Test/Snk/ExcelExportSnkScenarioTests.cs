using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;
using Xunit;

namespace Test.Snk;

/// <summary>
/// Data-driven проверка СНК: блок операций, дата расчёта и ожидаемые результаты.
/// Выгрузка СНК проверяется на <see cref="SnkTestCase.EndDate"/>.
/// Проверка инвентаризаций проверяется на каждую уникальную дату операции с кодом 10 и на <see cref="SnkTestCase.EndDate"/>.
/// СНК из проверки инвентаризаций на дату операции с кодом 10 должен совпадать с выгрузкой СНК на эту же дату
/// (по ключу единицы: паспорт, тип, радионуклиды, зав.№, УКТ, количество).
/// Эталоны задаются вручную в <see cref="SnkTestCases"/>.
/// </summary>
public class ExcelExportSnkScenarioTests
{
    /// <summary>
    /// Структурная самопроверка кейса (без запуска алгоритма): эталоны внутренне согласованы.
    /// Ловит опечатки в самих тестовых данных раньше, чем непонятные падения основных проверок:
    /// набор дат инвентаризаций совпадает с датами op.10 (+ EndDate); даты допустимых расхождений
    /// входят в даты инвентаризаций; внутри каждого списка наличия нет дублей учётных единиц.
    /// </summary>
    [Theory]
    [MemberData(nameof(SnkTestCases.All), MemberType = typeof(SnkTestCases))]
    public void TestCase_IsInternallyConsistent(string name, SnkTestCase testCase)
    {
        var inventoryDates = testCase.Operations
            .Where(x => x.OpCode == "10" && x.OpDate <= testCase.EndDate)
            .Select(x => x.OpDate)
            .Append(testCase.EndDate)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(inventoryDates, [.. testCase.ExpectedInventoryStockByDate.Keys.Order()]);

        foreach (var allowedDate in testCase.AllowedInventoryVsSnkDifferenceByDate.Keys)
        {
            Assert.Contains(allowedDate, inventoryDates);
        }

        AssertNoDuplicateUnits($"{name}: ExpectedSnkStock", testCase.ExpectedSnkStock);
        foreach (var (date, stock) in testCase.ExpectedInventoryStockByDate)
        {
            AssertNoDuplicateUnits($"{name}: наличие на {date:dd.MM.yyyy}", stock);
        }
    }

    private static void AssertNoDuplicateUnits(string context, IReadOnlyList<SnkStockSnapshot> stock)
    {
        var duplicates = stock
            .GroupBy(x => (x.PasNum, x.FacNum, x.Type, x.Radionuclids, x.PackNumber))
            .Where(g => g.Count() > 1)
            .Select(g => $"{g.Key.PasNum}/{g.Key.FacNum} {g.Key.Type} {g.Key.Radionuclids} УКТ={g.Key.PackNumber}")
            .ToList();

        Assert.True(
            duplicates.Count == 0,
            $"{context}: в списке наличия есть дублирующиеся единицы: {string.Join(", ", duplicates)}.");
    }

    [Theory]
    [MemberData(nameof(SnkTestCases.All), MemberType = typeof(SnkTestCases))]
    public async Task SnkExport_MatchesExpectedStockOnEndDate(string name, SnkTestCase testCase)
    {
        var snkOnEndDate = await SnkOperationScenarioRunner.RunSnkExportAsync(testCase, testCase.EndDate);

        SnkStockAssertions.Equal(testCase.ExpectedSnkStock, snkOnEndDate);
    }

    [Theory]
    [MemberData(nameof(SnkTestCases.All), MemberType = typeof(SnkTestCases))]
    public async Task CheckInventories_MatchesExpectedStockByDate(string name, SnkTestCase testCase)
    {
        var checkInventoryDates = testCase.Operations
            .Where(x => x.OpCode == "10" && x.OpDate <= testCase.EndDate)
            .Select(x => x.OpDate)
            .Append(testCase.EndDate)
            .Distinct()
            .Order()
            .ToList();

        var expectedInventoryDates = testCase.ExpectedInventoryStockByDate
            .Keys
            .Order()
            .ToList();

        Assert.Equal(checkInventoryDates, expectedInventoryDates);

        var checkByDate = await SnkOperationScenarioRunner.RunCheckInventoriesAsync(testCase);
        Assert.Equal(checkInventoryDates, [.. checkByDate.Keys.Order()]);

        foreach (var (inventoryDate, expectedStock) in testCase.ExpectedInventoryStockByDate)
        {
            Assert.True(
                checkByDate.ContainsKey(inventoryDate),
                $"Проверка инвентаризаций не вернула СНК на дату {inventoryDate:dd.MM.yyyy} (кейс «{name}»).");

            SnkStockAssertions.Equal(expectedStock, checkByDate[inventoryDate]);
        }
    }

    [Theory]
    [MemberData(nameof(SnkTestCases.All), MemberType = typeof(SnkTestCases))]
    public async Task CheckInventoriesStock_MatchesSnkExportStockOnEveryCheckDate(string name, SnkTestCase testCase)
    {
        var checkByDate = await SnkOperationScenarioRunner.RunCheckInventoriesAsync(testCase);

        foreach (var inventoryDate in checkByDate.Keys.Order())
        {
            var snkExportStock = await SnkOperationScenarioRunner.RunSnkExportAsync(testCase, inventoryDate);

            SnkStockAssertions.Equal(snkExportStock, checkByDate[inventoryDate]);
        }
    }

    /// <summary>
    /// Любое расхождение «единица проинвентаризирована, но отсутствует в расчётном СНК на ту же дату»
    /// должно быть явно разрешено в <see cref="SnkTestCase.AllowedInventoryVsSnkDifferenceByDate"/>
    /// (например, ЗРИ проинвентаризировали и в тот же день передали). Иначе это считается ошибкой.
    /// </summary>
    [Theory]
    [MemberData(nameof(SnkTestCases.All), MemberType = typeof(SnkTestCases))]
    public async Task CheckInventories_InventoryVsSnkDifferenceIsAllowed(string name, SnkTestCase testCase)
    {
        if (testCase.HasIntentionalInventoryErrors)
        {
            return;
        }

        var snkByDate = await SnkOperationScenarioRunner.RunCheckInventoriesAsync(testCase);
        var inventoriedByDate = await SnkOperationScenarioRunner.RunCheckInventoriesInventoriedAsync(testCase);

        foreach (var (inventoryDate, inventoried) in inventoriedByDate)
        {
            var snk = snkByDate.TryGetValue(inventoryDate, out var stock)
                ? stock
                : [];

            var allowed = testCase.AllowedInventoryVsSnkDifferenceByDate.TryGetValue(inventoryDate, out var diff)
                ? diff
                : [];

            SnkStockAssertions.InventoryDifferenceIsAllowed(inventoryDate, inventoried, snk, allowed);
        }
    }

    /// <summary>
    /// Проверяет типы и учётные единицы ошибок проверки инвентаризаций (все кейсы с явными ожиданиями).
    /// </summary>
    [Theory]
    [MemberData(nameof(SnkTestCases.WithExpectedInventoryErrors), MemberType = typeof(SnkTestCases))]
    public async Task CheckInventories_MatchesExpectedInventoryErrors(string name, SnkTestCase testCase)
    {
        var actualErrors = await SnkOperationScenarioRunner.RunCheckInventoriesErrorsAsync(testCase);
        SnkStockAssertions.InventoryErrorsEqual(testCase.ExpectedInventoryErrorsByDate, actualErrors);
    }

    /// <summary>
    /// Negative control: на корректных сценариях ошибки проверки инвентаризаций не формируются.
    /// </summary>
    [Theory]
    [MemberData(nameof(SnkTestCases.WithoutExpectedInventoryErrors), MemberType = typeof(SnkTestCases))]
    public async Task CheckInventories_HasNoErrorsOnAnyCheckDate(string name, SnkTestCase testCase)
    {
        var actualErrors = await SnkOperationScenarioRunner.RunCheckInventoriesErrorsAsync(testCase);
        SnkStockAssertions.InventoryErrorsAreEmpty(actualErrors);
    }
}
