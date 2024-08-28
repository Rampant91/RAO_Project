using System;
using System.Threading.Tasks;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Interfaces.Logger;
using Models.DBRealization;

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
        await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();
    }
}