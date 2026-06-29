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
    [Theory]
    [MemberData(nameof(SnkTestCases.All), MemberType = typeof(SnkTestCases))]
    public async Task Scenario_SnkExportAndCheckInventories_MatchExpected(string name, SnkTestCase testCase)
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

        var snkOnEndDate = await SnkOperationScenarioRunner.RunSnkExportAsync(testCase, testCase.EndDate);
        SnkStockAssertions.Equal(testCase.ExpectedSnkStock, snkOnEndDate);

        var checkByDate = await SnkOperationScenarioRunner.RunCheckInventoriesAsync(testCase);
        Assert.Equal(checkInventoryDates, [.. checkByDate.Keys.Order()]);

        foreach (var (inventoryDate, expectedStock) in testCase.ExpectedInventoryStockByDate)
        {
            var snkExportStock = await SnkOperationScenarioRunner.RunSnkExportAsync(testCase, inventoryDate);

            Assert.True(
                checkByDate.ContainsKey(inventoryDate),
                $"Проверка инвентаризаций не вернула СНК на дату {inventoryDate:dd.MM.yyyy} (кейс «{name}»).");

            SnkStockAssertions.Equal(expectedStock, checkByDate[inventoryDate]);
            SnkStockAssertions.Equal(snkExportStock, checkByDate[inventoryDate]);
        }
    }
}
