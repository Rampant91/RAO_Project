using MsBox.Avalonia;
using System.Collections;
using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Models.Interfaces;
using Avalonia.Threading;
using Client_App.ViewModels;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранный отчёт у выбранной организации.
/// </summary>
public class DeleteReportAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;

        #region MessageDeleteReport

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            }).ShowWindowDialogAsync(Desktop.MainWindow));

        #endregion

        if (answer is not "Да" || mainWindow!.SelectedReports is null || !mainWindow.SelectedReports.Any()) return;

        var selectedReports = new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);
        var selectedReportsFirst = mainWindow.SelectedReports.First() as Reports;

        var enumerable = parameter as IEnumerable;
        var report = enumerable!.Cast<Report>().FirstOrDefault();

        if (report is null) return;

        selectedReportsFirst!.Report_Collection.Remove(report);

        //await ReportDeletionLogger.LogDeletionAsync(report);

        mainWindow.SelectedReports = selectedReports;
        var mainWindowVM = (mainWindow.DataContext as MainWindowVM)!;
        mainWindowVM.UpdateTotalReportCount();

        await StaticConfiguration.DBModel.SaveChangesAsync();
    }
}