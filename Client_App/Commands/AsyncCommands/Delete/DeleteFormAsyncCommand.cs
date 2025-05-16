using System.Collections;
using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Interfaces;
using Avalonia.Threading;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранную форму у выбранной организации.
/// </summary>
public class DeleteFormAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageDeleteReport

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да", IsDefault = true },
                    new ButtonDefinition { Name = "Нет", IsCancel = true }
                ],
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить отчет?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is "Да")
        {
            var mainWindow = Desktop.MainWindow as MainWindow;
            if (mainWindow!.SelectedReports.Any())
            {
                var selectedReports = new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);
                var selectedReportsFirst = mainWindow.SelectedReports.First() as Reports;
                if (parameter is IEnumerable param)
                {
                    foreach (var item in param)
                    {
                        selectedReportsFirst.Report_Collection.Remove((Report)item);
                    }
                }
                mainWindow.SelectedReports = selectedReports;
            }
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
    }
}