using Client_App.ViewModels.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

public abstract partial class ExcelExportSnkBaseAsyncCommand
{
    private const string Form11Num = "1.1";

    #region Form11InStockCount

    /// <summary>
    /// Id всех организаций в БД, у которых есть форма 1.1 с хотя бы одной операцией инвентаризации (код 10).
    /// </summary>
    private protected static async Task<List<int>> GetForm11ReportsCollectionIdsAsync(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsQueryable()
            .Where(reps =>
                reps.DBObservable != null
                && reps.Report_Collection
                    .Any(rep => rep.FormNum_DB == Form11Num
                                && rep.Rows11.Any(form => form.OperationCode_DB == "10")))
            .Select(reps => reps.Id)
            .ToListAsync(cts.Token);
    }

    /// <summary>
    /// Количество источников в наличии (СНК) по форме 1.1 для одной организации на указанную дату.
    /// </summary>
    private protected static async Task<int> GetForm11InStockQuantityForOrganizationAsync(
        DBModel db,
        int reportsCollectionId,
        DateOnly endSnkDate,
        SnkParamsDto snkParams,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var inventoryReportDtoList = await GetInventoryReportDtoList(db, reportsCollectionId, Form11Num, endSnkDate, cts);
        var (firstSnkDate, inventoryFormsDtoList, _) =
            await GetInventoryFormsDtoList(db, inventoryReportDtoList, Form11Num, endSnkDate, cts, snkParams);

        if (inventoryFormsDtoList.Count is 0)
        {
            return 0;
        }

        var reportIds = await GetReportIds(db, reportsCollectionId, Form11Num, cts);
        var plusMinusFormsDtoList =
            await GetPlusMinusFormsDtoList(db, reportIds, Form11Num, firstSnkDate, endSnkDate, cts, snkParams);
        var rechargeFormsDtoList =
            await GetRechargeFormsDtoList(db, reportsCollectionId, Form11Num, firstSnkDate, endSnkDate, cts, snkParams);

        var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(
            Form11Num, inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);

        var unitInStockDtoList = await GetUnitInStockDtoList(
            uniqueUnitWithAllOperationDictionary, Form11Num, firstSnkDate, progressBarVM);

        return unitInStockDtoList.Sum(x => x.Quantity);
    }

    /// <summary>
    /// Суммарное количество источников в наличии (СНК) по форме 1.1 по всем организациям БД на указанную дату.
    /// </summary>
    private protected static async Task<int> GetForm11InStockTotalQuantityAsync(
        DBModel db,
        DateOnly endSnkDate,
        SnkParamsDto snkParams,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var reportsCollectionIds = await GetForm11ReportsCollectionIdsAsync(db, cts);
        if (reportsCollectionIds.Count is 0)
        {
            return 0;
        }

        var total = 0;
        var currentOrgNum = 0;
        var progressBarDoubleValue = (double)progressBarVM.ValueBar;

        foreach (var reportsCollectionId in reportsCollectionIds)
        {
            currentOrgNum++;
            total += await GetForm11InStockQuantityForOrganizationAsync(
                db, reportsCollectionId, endSnkDate, snkParams, progressBarVM, cts);

            progressBarDoubleValue += (double)70 / reportsCollectionIds.Count;
            progressBarVM.SetProgressBar(
                (int)System.Math.Floor(progressBarDoubleValue),
                $"Проверено {currentOrgNum} из {reportsCollectionIds.Count} организаций",
                "Подсчёт источников в наличии (форма 1.1)");
        }

        return total;
    }

    #endregion
}
