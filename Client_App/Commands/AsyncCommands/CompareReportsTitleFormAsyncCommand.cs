using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.Commands.AsyncCommands;

internal class CompareReportsTitleFormAsyncCommand : BaseAsyncCommand
{
    public Report BaseMasterReport;
    public Report ImportMasterReport;
    public CompareReportsTitleFormVM ViewModel;

    public CompareReportsTitleFormAsyncCommand(Report baseMasterReport, Report importMasterReport)
    {
        BaseMasterReport = baseMasterReport;
        ImportMasterReport = importMasterReport;
    }

    public override async Task<Report> AsyncExecute(object? parameter)
    {
        var compareReportsTitleFormWindow = new CompareReportsTitleForm(BaseMasterReport, ImportMasterReport);
        ViewModel = (compareReportsTitleFormWindow.DataContext as CompareReportsTitleFormVM)!;
        //await compareReportsTitleFormWindow.ShowDialog<Reports>(Desktop.MainWindow);
        //var newReps = compareReportsTitleFormWindow.NewReps;
        try
        {
            var a = await ShowPopup(compareReportsTitleFormWindow);
        }
        catch (Exception ex)
        {

        }
        
        return null;
    }

    private Task<object> ShowPopup<TPopup>(TPopup popup) where TPopup : Window
    {
        var task = new TaskCompletionSource<object>();
        BaseMasterReport.Rows10[0].SubjectRF_DB = ViewModel.SubjectRF;
        popup.Closed += (s, a) => task.SetResult(BaseMasterReport);
        popup.ShowDialog(Desktop.MainWindow);
        popup.Focus();
        return task.Task;
    }
}