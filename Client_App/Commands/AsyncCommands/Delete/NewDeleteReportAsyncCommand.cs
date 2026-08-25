using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Services;
using Client_App.ViewModels;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранный отчёт у выбранной организации.
/// </summary>
public class NewDeleteFormAsyncCommand : BaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public NewDeleteFormAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
    {
        _formsTabControlVM = formsTabControlVM;

        formsTabControlVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReport))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _formsTabControlVM.SelectedReport is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is Report reportToDelete
            && await ReportExportLock.TryBlockReportAccessAsync(reportToDelete.Id))
        {
            return;
        }

        if (parameter is null
            && _formsTabControlVM.SelectedReport is { } selected
            && await ReportExportLock.TryBlockReportAccessAsync(selected.Id))
        {
            return;
        }

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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
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

                if (parameter is Report selectedReport) 
                {
                    selectedReports.Report_Collection.Remove(selectedReport);
                }


                mainWindowVM.UpdateReportCollection();
                mainWindowVM.UpdateFormsPageInfo();

                mainWindowVM.SelectedReports = selectedReports;
            }
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
    }
}