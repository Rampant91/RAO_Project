using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.Views;
using Models.Collections;

namespace Client_App.Commands.AsyncCommands;

internal class CompareReportsTitleFormAsyncCommand : BaseAsyncCommand
{
    public Report BaseMasterReport;
    public Report ImportMasterReport;
    //public CompareReportsTitleFormVM ViewModel;

    public CompareReportsTitleFormAsyncCommand(Report baseMasterReport, Report importMasterReport)
    {
        BaseMasterReport = baseMasterReport;
        ImportMasterReport = importMasterReport;
    }

    public override async Task<Report> AsyncExecute(object? parameter)
    {
        var compareReportsTitleFormWindow = new CompareReportsTitleForm(BaseMasterReport, ImportMasterReport);
        //ViewModel = (compareReportsTitleFormWindow.DataContext as CompareReportsTitleFormVM)!;
        var newRep = await ShowPopup(compareReportsTitleFormWindow) as Report;
        return newRep!;
    }

    private Task<object> ShowPopup<TPopup>(TPopup popup) where TPopup : Window
    {
        var task = new TaskCompletionSource<object>();
        //BaseMasterReport.Rows10[0].SubjectRF_DB = ViewModel.SubjectRF;
        popup.Closed += (s, a) => task.SetResult(BaseMasterReport);
        popup.ShowDialog(Desktop.MainWindow);
        popup.Focus();
        return task.Task;
    }
}