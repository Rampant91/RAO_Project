using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;
using Client_App.ViewModels;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

public partial class ExcelExportCheckInventoriesAsyncCommand
{
    /// <summary>
    /// Внутренний доступ к GetInventoryErrorsAndSnk для unit-тестов.
    /// </summary>
    internal sealed class InventoriesTestHarness : ExcelExportCheckInventoriesAsyncCommand
    {
        public static InventoriesTestHarness Instance { get; } = new();

        private InventoriesTestHarness() : base(null!)
        {
        }

        public override Task AsyncExecute(object? parameter) => Task.CompletedTask;

        public override bool CanExecute(object? parameter) => false;

        public async Task<IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>> RunCheckInventoriesAsync(
            SnkTestCase testCase)
        {
            var operations = testCase.Operations
                .Where(x => x.OpDate <= testCase.EndDate)
                .ToList();

            var (inventoryList, plusMinusList, rechargeList) =
                SnkTestOperationSplitter.Split(testCase.FormNum, operations);

            var firstInventoryDate = inventoryList.Count == 0
                ? DateOnly.MinValue
                : inventoryList.Min(x => x.OpDate);

            var dictionary = await GetDictionary_UniqueUnitsWithOperations(
                testCase.FormNum,
                inventoryList,
                plusMinusList,
                rechargeList,
                []);

            var inventoryDatesList = await GetInventoryDatesList(inventoryList, testCase.EndDate);

            var (unitInStockByDateDictionary, _) = await GetInventoryErrorsAndSnk(
                dictionary,
                inventoryDatesList,
                [],
                firstInventoryDate,
                testCase.FormNum);

            return unitInStockByDateDictionary.ToDictionary(
                pair => pair.Key,
                pair => (IReadOnlyList<SnkStockSnapshot>)pair.Value
                    .Select(SnkTestOperationSplitter.ToSnapshot)
                    .ToList());
        }
    }
}
