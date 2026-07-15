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
            var (stockByDate, _) = await RunCheckInventoriesCoreAsync(testCase);
            return stockByDate;
        }

        public async Task<IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkActualInventoryError>>> RunCheckInventoriesErrorsAsync(
            SnkTestCase testCase)
        {
            var (_, errorsByDate) = await RunCheckInventoriesCoreAsync(testCase);
            return errorsByDate;
        }

        /// <summary>
        /// Список проинвентаризированных единиц (строки операций инвентаризации, код 10)
        /// на каждую дату проверки. На дату <see cref="SnkTestCase.EndDate"/> без инвентаризации
        /// список пуст. Это «таблица инвентаризации» из отчёта, без расчёта наличия.
        /// </summary>
        public async Task<IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>> RunCheckInventoriesInventoriedAsync(
            SnkTestCase testCase)
        {
            var operations = testCase.Operations
                .Where(x => x.OpDate <= testCase.EndDate)
                .ToList();

            var (inventoryList, _, _, _) = SnkTestOperationSplitter.Split(testCase.FormNum, operations);

            var inventoryDatesList = await GetInventoryDatesList(inventoryList, testCase.EndDate);

            return inventoryDatesList.ToDictionary(
                date => date,
                date => (IReadOnlyList<SnkStockSnapshot>)inventoryList
                    .Where(x => x.OpDate == date)
                    .Select(SnkTestOperationSplitter.ToSnapshot)
                    .ToList());
        }

        private async Task<(
            IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>,
            IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkActualInventoryError>>)>
            RunCheckInventoriesCoreAsync(SnkTestCase testCase)
        {
            var operations = testCase.Operations
                .Where(x => x.OpDate <= testCase.EndDate)
                .ToList();

            var (inventoryList, plusMinusList, rechargeList, zeroList) =
                SnkTestOperationSplitter.Split(testCase.FormNum, operations);

            var (summedInventoryList, inventoryDuplicateErrors) =
                await GetSummedInventoryDtoList(inventoryList, testCase.FormNum);

            var firstInventoryDate = summedInventoryList.Count == 0
                ? DateOnly.MinValue
                : summedInventoryList.Min(x => x.OpDate);

            var dictionary = await GetDictionary_UniqueUnitsWithOperations(
                testCase.FormNum,
                summedInventoryList,
                plusMinusList,
                rechargeList,
                zeroList);

            var inventoryDatesList = await GetInventoryDatesList(summedInventoryList, testCase.EndDate);

            var (unitInStockByDateDictionary, inventoryErrorsByDateDictionary) = await GetInventoryErrorsAndSnk(
                dictionary,
                inventoryDatesList,
                inventoryDuplicateErrors,
                firstInventoryDate,
                testCase.FormNum);

            var stockByDate = unitInStockByDateDictionary.ToDictionary(
                pair => pair.Key,
                pair => (IReadOnlyList<SnkStockSnapshot>)pair.Value
                    .Select(SnkTestOperationSplitter.ToSnapshot)
                    .ToList());

            var errorsByDate = inventoryErrorsByDateDictionary.ToDictionary(
                pair => pair.Key,
                pair => (IReadOnlyList<SnkActualInventoryError>)pair.Value
                    .Select(ToActualInventoryError)
                    .ToList());

            return (stockByDate, errorsByDate);
        }

        private static SnkActualInventoryError ToActualInventoryError(InventoryErrorsShortDto error) =>
            new(
                (SnkInventoryErrorType)(int)error.ErrorTypeEnum,
                error.Dto.PasNum,
                error.Dto.FacNum,
                error.Dto.Type,
                error.Dto.Radionuclids,
                error.Dto.PackNumber,
                error.Dto.OpCode,
                error.Dto.OpDate);
    }
}
