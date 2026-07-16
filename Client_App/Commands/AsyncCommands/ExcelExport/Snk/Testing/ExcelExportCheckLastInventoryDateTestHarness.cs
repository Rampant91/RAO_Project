using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;
using Client_App.ViewModels.ProgressBar;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

public partial class ExcelExportCheckLastInventoryDateAsyncCommand
{
    /// <summary>
    /// Внутренний доступ к расчёту СНК для unit-тестов (без БД и Excel).
    /// </summary>
    public sealed class LastInventoryTestHarness : ExcelExportCheckLastInventoryDateAsyncCommand
    {
        /// <summary>
        /// Порог просрочки инвентаризации: строго больше 365 + 14 дней (как в <see cref="CheckRepsInventoryDate"/>).
        /// </summary>
        public static int InventoryExpiryThresholdDays => ExcelExportCheckLastInventoryDateAsyncCommand.InventoryExpiryThresholdDays;

        public static LastInventoryTestHarness Instance { get; } = new();

        private LastInventoryTestHarness()
        {
        }

        public override Task AsyncExecute(object? parameter) => Task.CompletedTask;

        public override bool CanExecute(object? parameter) => false;

        /// <summary>
        /// Определяет, просрочена ли инвентаризация на дату <paramref name="asOfDate"/>.
        /// Соответствует логике <see cref="CheckRepsInventoryDate"/> для списка дат op.10.
        /// </summary>
        public static bool IsInventoryExpired(DateOnly asOfDate, IReadOnlyList<DateOnly> inventoryDates)
        {
            return ExcelExportCheckLastInventoryDateAsyncCommand.IsInventoryExpired(asOfDate, inventoryDates);
        }

        /// <summary>
        /// Последняя дата инвентаризации или <c>null</c>, если op.10 не было.
        /// </summary>
        public static DateOnly? GetLastInventoryDate(IReadOnlyList<DateOnly> inventoryDates) =>
            ExcelExportCheckLastInventoryDateAsyncCommand.GetLastInventoryDate(inventoryDates);

        /// <summary>
        /// Организация попадает в список «должников», если просрочена и в СНК есть хотя бы одна единица.
        /// Соответствует фильтру в конце <see cref="CheckSnk"/>.
        /// </summary>
        public static bool IncludeInDebtorsList(int countUnits) =>
            ExcelExportCheckLastInventoryDateAsyncCommand.IncludeInDebtorsList(countUnits);

        /// <summary>
        /// Расчёт СНК на дату, как в <see cref="CheckSnk"/> (список единиц + сумма количеств).
        /// Пайплайн идентичен выгрузке СНК: словарь операций → <c>GetUnitInStockDtoList</c>.
        /// </summary>
        public async Task<LastInventorySnkResult> RunComputeSnkAsync(SnkTestCase testCase, DateOnly asOfDate)
        {
            var operations = testCase.Operations
                .Where(x => x.OpDate <= asOfDate)
                .ToList();

            var (inventoryList, plusMinusList, rechargeList, _) =
                SnkTestOperationSplitter.Split(testCase.FormNum, operations);

            var firstInventoryDate = inventoryList.Count == 0
                ? DateOnly.MinValue
                : inventoryList.Min(x => x.OpDate);

            var dictionary = await GetDictionary_UniqueUnitsWithOperations(
                testCase.FormNum,
                inventoryList,
                plusMinusList,
                rechargeList,
                zeroFormsDtoList: null);

            var unitsInStock = await GetUnitInStockDtoList(
                dictionary,
                testCase.FormNum,
                firstInventoryDate,
                new AnyTaskProgressBarVM());

            var stock = unitsInStock.Select(SnkTestOperationSplitter.ToSnapshot).ToList();
            var countUnits = unitsInStock.Sum(x => x.Quantity);

            return new LastInventorySnkResult(stock, countUnits);
        }
    }
}
