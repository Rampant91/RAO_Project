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
using Client_App.ViewModels;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранную форму у выбранной организации.
/// </summary>
public class NewDeleteFormAsyncCommand : BaseAsyncCommand
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
            var mainWindowVM = mainWindow.DataContext as MainWindowVM;
            if (mainWindowVM.SelectedReports != null)
            {
                var selectedReports = mainWindowVM.SelectedReports;
                var selectedIndex = mainWindowVM.ReportsCollection.IndexOf(selectedReports);

                if (parameter is Report selectedReport) 
                {
                    selectedReports.Report_Collection.Remove(selectedReport);
                }



                mainWindowVM.UpdateReports();
                mainWindowVM.SelectedReports = mainWindowVM.ReportsCollection[selectedIndex];   // Чтобы не слетала выбранная организация

                mainWindowVM.UpdateReport();
            }
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
    }
}