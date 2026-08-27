using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Resources.CustomComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Save;

/// <summary>
/// Сохраняет комментарий отчёта с главного окна (и прочие pending-изменения ChangeTracker).
/// Отчёт в гриде — AsNoTracking stub: правки Comments не в трекере, копируем в tracked по Id.
/// </summary>
public class SaveReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var db = StaticConfiguration.DBModel;
        try
        {
            if (parameter is Report stub && stub.Id != 0)
            {
                var tracked = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == stub.Id)
                              ?? await db.ReportCollectionDbSet.FirstOrDefaultAsync(r => r.Id == stub.Id);

                if (tracked is null)
                {
                    ServiceExtension.LoggerManager.Warning(
                        $"Сохранение комментария: отчёт Id={stub.Id} не найден в БД.",
                        ErrorCodeLogger.DataBase);
                }
                else
                {
                    tracked.Comments_DB = stub.Comments_DB;
                }
            }

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }

        try
        {
            if (ReportsStorage.LocalReports?.Reports_Collection is null)
                return;

            var comparator = new CustomReportsComparer();
            var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
            ReportsStorage.LocalReports.Reports_Collection.Clear();
            ReportsStorage.LocalReports.Reports_Collection
                .AddRange(tmpReportsList
                    .OrderBy(x => x.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(x => x.Master_DB.OkpoRep.Value, comparator));
        }
        catch (Exception ex)
        {
            var msg = $"Пересортировка LocalReports после сохранения комментария." +
                      $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Warning(msg, ErrorCodeLogger.Application);
        }
    }
}
