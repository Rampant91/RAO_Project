using System;
using System.Collections;
using System.Collections.Generic;
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
        if (parameter is IEnumerable param)
        {
            foreach (var item in param)
            {
                var rep = item as Reports;
                rep?.Report_Collection.Clear();
                ReportsStorage.LocalReports.Reports_Collection.Remove((Reports)item);
            }
        }

        await using var db = new DBModel(StaticConfiguration.DBPath);
        var a = parameter as IEnumerable;
        var list = a.Cast<Reports>().ToList();
        //var 


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