using Client_App.Services;
﻿using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views;
using Models.Collections;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Изменить Формы организации (1.0, 2.0, 4.0, 5.0).
/// </summary>
public class NewChangeReportsAsyncCommand : BaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public NewChangeReportsAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
    {
        _formsTabControlVM = formsTabControlVM;

        formsTabControlVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _formsTabControlVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = (Desktop.MainWindow as MainWindow)!;
        var mainWindowVM = (mainWindow.DataContext as MainWindowVM)!;

        var selectedReports = parameter as Reports
                              ?? _formsTabControlVM.SelectedReports
                              ?? mainWindowVM.SelectedReports;
        if (selectedReports is null) return;

        if (await ReportExportLock.TryBlockOrganizationAccessAsync(selectedReports.Id))
            return;

        await OrganizationFormOpener.TryOpenAsync(mainWindow, mainWindowVM, selectedReports);
    }
}
