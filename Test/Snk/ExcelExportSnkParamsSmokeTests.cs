using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;
using Xunit;

namespace Test.Snk;

/// <summary>
/// Smoke-тесты для запасных режимов расчёта:
/// - игнорирование части параметров идентификации (SnkParamsDto-поведение),
/// - эквивалентность представления количества для пустых зав./паспорт в 1.1.
/// </summary>
public class ExcelExportSnkParamsSmokeTests
{
    [Fact]
    public async Task SnkParams_IgnoringAnySingleField_DoesNotBreakThreeCommands_Form11()
    {
        var baseCase = new SnkTestCase
        {
            Name = "Params-Form11",
            FormNum = "1.1",
            EndDate = new DateOnly(2026, 1, 1),
            Operations =
            [
                Op("10", new DateOnly(2022, 1, 19), "P-901", "F-811", "Тип-Смок", "кобальт-60", "УКТ-51", 1),
                Op("10", new DateOnly(2022, 1, 19), "P-901", "F-812", "Тип-Смок", "кобальт-60", "УКТ-51", 1),
            ],
            ExpectedSnkStock = [],
            ExpectedInventoryStockByDate = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>()
        };

        // Базовый режим (учитываются все поля)
        await AssertThreeCommandsConsistent(baseCase);

        // По одному "игнорируемому" параметру
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkPasNum: false));
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkType: false));
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkRadionuclids: false));
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkFacNum: false));
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkPackNumber: false));
    }

    [Fact]
    public async Task SnkParams_IgnoringAnySingleField_DoesNotBreakThreeCommands_Form13()
    {
        var baseCase = new SnkTestCase
        {
            Name = "Params-Form13",
            FormNum = "1.3",
            EndDate = new DateOnly(2026, 1, 1),
            Operations =
            [
                Op("10", new DateOnly(2022, 1, 19), "P-303", "F-821", "Тип-Смок3", "цезий-137", "УКТ-50", 1),
                Op("10", new DateOnly(2022, 1, 19), "P-303", "F-822", "Тип-Смок3", "цезий-137", "УКТ-50", 1),
            ],
            ExpectedSnkStock = [],
            ExpectedInventoryStockByDate = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>()
        };

        await AssertThreeCommandsConsistent(baseCase);
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkPasNum: false));
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkType: false));
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkRadionuclids: false));
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkFacNum: false));
        await AssertThreeCommandsConsistent(ApplyIgnoredParams(baseCase, checkPackNumber: false));
    }

    [Fact]
    public async Task Form11_EmptySerial_ManyRowsByOne_EqualsSingleRowByQuantity_ForAllThreeCommands()
    {
        var endDate = new DateOnly(2026, 1, 1);
        var firstInvDate = new DateOnly(2022, 1, 19);

        var manyRowsByOne = new SnkTestCase
        {
            Name = "EmptySerial-manyRows",
            FormNum = "1.1",
            EndDate = endDate,
            Operations =
            [
                Op("10", firstInvDate, "P-900", "F-001", "Тип-База", "кобальт-60", "УКТ-0", 1),
                Op("10", firstInvDate, "", "", "Тип-Кол", "кобальт-60", "", 1),
                Op("10", firstInvDate, "", "", "Тип-Кол", "кобальт-60", "", 1),
                Op("10", firstInvDate, "", "", "Тип-Кол", "кобальт-60", "", 1),
                Op("10", firstInvDate, "", "", "Тип-Кол", "кобальт-60", "", 1),
            ],
            ExpectedSnkStock = [],
            ExpectedInventoryStockByDate = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>()
        };

        var singleRowByQuantity = new SnkTestCase
        {
            Name = "EmptySerial-singleRow",
            FormNum = "1.1",
            EndDate = endDate,
            Operations =
            [
                Op("10", firstInvDate, "P-900", "F-001", "Тип-База", "кобальт-60", "УКТ-0", 1),
                Op("10", firstInvDate, "", "", "Тип-Кол", "кобальт-60", "", 4),
            ],
            ExpectedSnkStock = [],
            ExpectedInventoryStockByDate = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>()
        };

        await AssertThreeCommandsEquivalent(manyRowsByOne, singleRowByQuantity);
    }

    private static async Task AssertThreeCommandsEquivalent(SnkTestCase leftCase, SnkTestCase rightCase)
    {
        var leftExport = await SnkOperationScenarioRunner.RunSnkExportAsync(leftCase, leftCase.EndDate);
        var rightExport = await SnkOperationScenarioRunner.RunSnkExportAsync(rightCase, rightCase.EndDate);
        SnkStockAssertions.Equal(leftExport, rightExport);

        var leftCheck = await SnkOperationScenarioRunner.RunCheckInventoriesAsync(leftCase);
        var rightCheck = await SnkOperationScenarioRunner.RunCheckInventoriesAsync(rightCase);

        Assert.Equal([.. leftCheck.Keys.Order()], [.. rightCheck.Keys.Order()]);
        foreach (var date in leftCheck.Keys.Order())
        {
            SnkStockAssertions.Equal(leftCheck[date], rightCheck[date]);
        }

        var leftLast = await SnkOperationScenarioRunner.RunLastInventorySnkAsync(leftCase, leftCase.EndDate);
        var rightLast = await SnkOperationScenarioRunner.RunLastInventorySnkAsync(rightCase, rightCase.EndDate);
        SnkStockAssertions.Equal(leftLast.Stock, rightLast.Stock);
        Assert.Equal(leftLast.CountUnits, rightLast.CountUnits);
    }

    private static async Task AssertThreeCommandsConsistent(SnkTestCase testCase)
    {
        var export = await SnkOperationScenarioRunner.RunSnkExportAsync(testCase, testCase.EndDate);
        var checkByDate = await SnkOperationScenarioRunner.RunCheckInventoriesAsync(testCase);
        var lastInventory = await SnkOperationScenarioRunner.RunLastInventorySnkAsync(testCase, testCase.EndDate);

        Assert.True(
            checkByDate.ContainsKey(testCase.EndDate),
            $"Проверка инвентаризаций не вернула СНК на EndDate {testCase.EndDate:dd.MM.yyyy}.");

        SnkStockAssertions.Equal(export, checkByDate[testCase.EndDate]);
        SnkStockAssertions.Equal(export, lastInventory.Stock);
        Assert.Equal(export.Sum(x => x.Quantity), lastInventory.CountUnits);
    }

    private static SnkTestCase ApplyIgnoredParams(
        SnkTestCase source,
        bool checkPasNum = true,
        bool checkType = true,
        bool checkRadionuclids = true,
        bool checkFacNum = true,
        bool checkPackNumber = true)
    {
        var transformedOperations = source.Operations
            .Select(x => x with
            {
                PasNum = checkPasNum ? x.PasNum : string.Empty,
                Type = checkType ? x.Type : string.Empty,
                Radionuclids = checkRadionuclids ? x.Radionuclids : string.Empty,
                FacNum = checkFacNum ? x.FacNum : string.Empty,
                PackNumber = checkPackNumber ? x.PackNumber : string.Empty
            })
            .ToList();

        return new SnkTestCase
        {
            Name = source.Name + " [params-smoke]",
            FormNum = source.FormNum,
            EndDate = source.EndDate,
            Operations = transformedOperations,
            ExpectedSnkStock = source.ExpectedSnkStock,
            ExpectedInventoryStockByDate = source.ExpectedInventoryStockByDate,
            AllowedInventoryVsSnkDifferenceByDate = source.AllowedInventoryVsSnkDifferenceByDate,
            HasIntentionalInventoryErrors = source.HasIntentionalInventoryErrors,
            ExpectedInventoryErrorsByDate = source.ExpectedInventoryErrorsByDate
        };
    }

    private static SnkTestOperationSpec Op(
        string opCode,
        DateOnly opDate,
        string pasNum,
        string facNum,
        string type,
        string radionuclids,
        string packNumber,
        int quantity) =>
        new(opCode, opDate, pasNum, facNum, type, radionuclids, packNumber, quantity);
}
