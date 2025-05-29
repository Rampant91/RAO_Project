using System;
using System.Collections;
using System.Linq;
using Models.Collections;
using Models.DBRealization;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Avalonia.Threading;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранную организацию.
/// </summary>
internal class DeleteReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageDeleteReports

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                ],
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить организацию?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is not "Да") return;

        try
        {
            var enumerable = parameter as IEnumerable;
            var repsList = enumerable!.Cast<Reports>().ToList();

            await using var db = new DBModel(StaticConfiguration.DBPath);
            db.ReportCollectionDbSet.Remove(repsList[0].Master_DB);
            db.ReportsCollectionDbSet.Remove(repsList[0]);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }

        //await Local_Reports.Reports_Collection.QuickSortAsync();
    }
}