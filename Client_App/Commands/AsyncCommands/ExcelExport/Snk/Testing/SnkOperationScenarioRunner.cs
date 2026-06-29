using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

/// <summary>
/// Запуск тестовых сценариев СНК без обращения к БД.
/// </summary>
public static class SnkOperationScenarioRunner
{
    /// <summary>СНК на дату (выгрузка СНК).</summary>
    public static Task<IReadOnlyList<SnkStockSnapshot>> RunSnkExportAsync(SnkTestCase testCase, DateOnly asOfDate) =>
        ExcelExportSnkTestHarness.Instance.RunSnkExportAsync(testCase, asOfDate);

    /// <summary>СНК на каждую дату инвентаризации (проверка инвентаризаций).</summary>
    public static Task<IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>> RunCheckInventoriesAsync(
        SnkTestCase testCase) =>
        ExcelExportCheckInventoriesAsyncCommand.InventoriesTestHarness.Instance.RunCheckInventoriesAsync(testCase);

    /// <summary>Список проинвентаризированных единиц (строки op.10) на каждую дату проверки.</summary>
    public static Task<IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>> RunCheckInventoriesInventoriedAsync(
        SnkTestCase testCase) =>
        ExcelExportCheckInventoriesAsyncCommand.InventoriesTestHarness.Instance.RunCheckInventoriesInventoriedAsync(testCase);

    public static Task<SnkScenarioResult> RunAsync(SnkTestScenario scenario) =>
        ExcelExportSnkTestHarness.Instance.RunScenarioAsync(scenario);
}
