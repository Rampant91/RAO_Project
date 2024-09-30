using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.Views;
using Models.Collections;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Сравнение головных отчётов и открытие соответствующего окна.
/// </summary>
/// <param name="baseMasterReport">Имеющийся в БД головной отчёт.</param>
/// <param name="importMasterReport">Импортируемый головоной отчёт.</param>
/// <param name="repsWhereTitleFormCheckIsCancel">Список организаций (рег.№, ОКПО), где проверка отменена.</param>
public class CompareReportsTitleFormAsyncCommand(
    Report baseMasterReport,
    Report importMasterReport,
    List<(string, string)> repsWhereTitleFormCheckIsCancel)
    : BaseAsyncCommand
{
    //public CompareReportsTitleFormVM ViewModel;

    public override async Task<Report> AsyncExecute(object? parameter)
    {
        var compareReportsTitleFormWindow = new CompareReportsTitleForm(baseMasterReport, importMasterReport, repsWhereTitleFormCheckIsCancel);
        //ViewModel = (compareReportsTitleFormWindow.DataContext as CompareReportsTitleFormVM)!;
        var newRep = await ShowPopup(compareReportsTitleFormWindow) as Report;
        return newRep!;
    }

    private Task<object> ShowPopup<TPopup>(TPopup popup) where TPopup : Window
    {
        var task = new TaskCompletionSource<object>();
        //BaseMasterReport.Rows10[0].SubjectRF_DB = ViewModel.SubjectRF;
        popup.Closed += (s, a) => task.SetResult(baseMasterReport);
        popup.ShowDialog(Desktop.MainWindow);
        popup.Focus();
        return task.Task;
    }
}