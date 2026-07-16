using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;
using Xunit;

namespace Test.Snk;

/// <summary>
/// Тесты проверки последней инвентаризации (<see cref="ExcelExportCheckLastInventoryDateAsyncCommand"/>).
/// Главный контракт: СНК на дату совпадает с выгрузкой СНК; <c>CountUnits</c> = сумма количеств.
/// </summary>
public class ExcelExportLastInventoryScenarioTests
{
    /// <summary>
    /// СНК из проверки последней инвентаризации на <see cref="SnkTestCase.EndDate"/>
    /// должен совпадать с выгрузкой СНК на ту же дату (по ключу единицы и количеству).
    /// </summary>
    [Theory]
    [MemberData(nameof(SnkTestCases.All), MemberType = typeof(SnkTestCases))]
    public async Task LastInventory_Snk_MatchesSnkExportOnEndDate(string name, SnkTestCase testCase)
    {
        var asOfDate = testCase.EndDate;

        var exportStock = await SnkOperationScenarioRunner.RunSnkExportAsync(testCase, asOfDate);
        var lastInventory = await SnkOperationScenarioRunner.RunLastInventorySnkAsync(testCase, asOfDate);

        SnkStockAssertions.Equal(exportStock, lastInventory.Stock);
        Assert.Equal(exportStock.Sum(x => x.Quantity), lastInventory.CountUnits);
    }

    /// <summary>
    /// Фильтр «должников»: организация включается в отчёт только при ненулевом СНК.
    /// </summary>
    [Theory]
    [MemberData(nameof(SnkTestCases.All), MemberType = typeof(SnkTestCases))]
    public async Task LastInventory_DebtorFilter_MatchesCountUnits(string name, SnkTestCase testCase)
    {
        var lastInventory = await SnkOperationScenarioRunner.RunLastInventorySnkAsync(testCase, testCase.EndDate);

        Assert.Equal(
            lastInventory.CountUnits > 0,
            ExcelExportCheckLastInventoryDateAsyncCommand.LastInventoryTestHarness.IncludeInDebtorsList(lastInventory.CountUnits));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(4, true)]
    public void LastInventory_IncludeInDebtorsList(int countUnits, bool expected)
    {
        Assert.Equal(expected, ExcelExportCheckLastInventoryDateAsyncCommand.LastInventoryTestHarness.IncludeInDebtorsList(countUnits));
    }

    /// <summary>
    /// Порог просрочки: строго больше 379 дней (365 + 14).
    /// </summary>
    [Theory]
    [MemberData(nameof(InventoryExpiryCases))]
    public void LastInventory_IsInventoryExpired(DateOnly asOfDate, DateOnly[] inventoryDates, bool expectedExpired)
    {
        Assert.Equal(
            expectedExpired,
            ExcelExportCheckLastInventoryDateAsyncCommand.LastInventoryTestHarness.IsInventoryExpired(asOfDate, inventoryDates));
    }

    public static IEnumerable<object[]> InventoryExpiryCases()
    {
        var today = new DateOnly(2026, 1, 1);
        var threshold = ExcelExportCheckLastInventoryDateAsyncCommand.LastInventoryTestHarness.InventoryExpiryThresholdDays;
        var lastOnThreshold = today.AddDays(-threshold);
        var lastOverThreshold = today.AddDays(-(threshold + 1));

        yield return [today, Array.Empty<DateOnly>(), true];
        yield return [today, new[] { lastOnThreshold }, false];
        yield return [today, new[] { lastOverThreshold }, true];
        yield return [today, new[] { new DateOnly(2025, 12, 1) }, false];
        yield return [today, new[] { new DateOnly(2024, 1, 1), new DateOnly(2025, 6, 1) }, false];
    }

    [Fact]
    public void LastInventory_GetLastInventoryDate_NoInventory_ReturnsNull()
    {
        Assert.Null(ExcelExportCheckLastInventoryDateAsyncCommand.LastInventoryTestHarness.GetLastInventoryDate([]));
    }

    [Fact]
    public void LastInventory_GetLastInventoryDate_ReturnsMaxDate()
    {
        var dates = new[] { new DateOnly(2022, 1, 19), new DateOnly(2023, 11, 29) };
        Assert.Equal(new DateOnly(2023, 11, 29), ExcelExportCheckLastInventoryDateAsyncCommand.LastInventoryTestHarness.GetLastInventoryDate(dates));
    }
}
