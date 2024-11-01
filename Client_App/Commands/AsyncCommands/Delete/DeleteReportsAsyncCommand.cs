using System;
using System.Collections;
using Models.Collections;
using Models.DBRealization;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Avalonia.Threading;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранную организацию.
/// </summary>
internal class DeleteReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageDeleteReports

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
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
            .ShowWindowDialogAsync(Desktop.MainWindow));

        #endregion

        if (answer is not "Да") return;
        if (parameter is IEnumerable param)
        {
            foreach (var item in param)
            {
                var rep = item as Reports;
                rep?.Report_Collection.Clear();
                ReportsStorage.LocalReports.Reports_Collection.Remove((Reports)item);
            }
        }

        try
        {
            await StaticConfiguration.DBModel.SaveChangesAsync().ConfigureAwait(false);
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