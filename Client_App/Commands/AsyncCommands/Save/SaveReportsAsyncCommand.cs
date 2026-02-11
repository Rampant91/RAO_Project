using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels;
using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Save;

//  Сохраняет текущую базу, используется только для сохранения комментария формы
public class SaveReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        try
        {
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }

        var comparator = new CustomReportsComparer();
        var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
        ReportsStorage.LocalReports.Reports_Collection.Clear();
        ReportsStorage.LocalReports.Reports_Collection
            .AddRange(tmpReportsList
                .OrderBy(x => x.Master_DB.RegNoRep.Value, comparator)
                .ThenBy(x => x.Master_DB.OkpoRep.Value, comparator));

        //await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();
    }
}