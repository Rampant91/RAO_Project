using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views.ProgressBar;
using MsBox.Avalonia.Dto;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Проверяет отчёт из главного окна, не открывая его.
/// </summary>
/// <returns>Открывает окно с отчётом об ошибках.</returns>
public class CheckReportFromMainAsyncCommand : BaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public CheckReportFromMainAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
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

    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        try
        {
            await AsyncExecute(parameter);
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
        IsExecute = false;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        Report? par;
        if (parameter is IKeyCollection collection)
            par = collection.ToList<Report>().First();
        else if (parameter is Report)
            par = (Report)parameter;
        else
            return;

        var cts = new CancellationTokenSource();
        AnyTaskProgressBar? progressBar = null;

        Report? rep = null;
        List<CheckError> errorList = [];
        try
        {
            if (par.FormNum_DB is "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8")
            {
                progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
                var checkProgress = ReportCheckProgress.ForStandalone(progressBar.AnyTaskProgressBarVM);
                var run = await ReportCheckRunner.RunAsync(new ReportCheckRunner.Options
                {
                    ReportId = par.Id,
                    FormNum = par.FormNum_DB,
                    Progress = checkProgress,
                    CancellationToken = cts.Token
                });
                rep = run.Report;
                errorList.AddRange(run.Errors);
            }
            else
            {
                await using var db = new DBModel(StaticConfiguration.DBPath);
                rep = await Services.DataAccess.ReportCheckSnapshotLoader.LoadAsync(
                    db, par.Id, par.FormNum_DB, cts.Token);
                if (rep is null)
                {
                    return;
                }

                errorList.AddRange(await ReportCheckRunner.RunForm2OrLegacyAsync(rep, db, cts.Token));
            }
        }
        catch (NotImplementedException)
        {
            #region MessageCheckFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {par.FormNum_DB}",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Функция проверки данных форм находится в процессе реализации.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion

            return;
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Warning(msg);

            #region MessageCheckFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {par.FormNum_DB}",
                    ContentHeader = "Уведомление",
                    ContentMessage = "В ходе выполнения проверки формы возникла непредвиденная ошибка.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion

            return;
        }
        finally
        {
            if (progressBar is not null)
            {
                await progressBar.CloseAsync();
            }
        }

        if (rep is null)
        {
            return;
        }

        if (errorList.Count == 0)
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                    ContentHeader = "Уведомление",
                    ContentMessage = "По результатам проверки формы, ошибок не выявлено.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion
        }
        else
        {
            if (Desktop.Windows.FirstOrDefault(x => x.Name == "FormCheckerWindow") is { } checkerWindow)
            {
                await Dispatcher.UIThread.InvokeAsync(checkerWindow.Close);
            }

            switch (rep.FormNum_DB)
            {
                case "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8":
                {
                    var vm = await CreateFormVM(rep.FormNum_DB, rep);
                    if (vm is null)
                    {
                        return;
                    }

                    await Dispatcher.UIThread.InvokeAsync(() => new Views.Forms.NewCheckForm(vm, errorList));

                    break;
                }
                case "2.1" or "2.2" or "2.3" or "2.4" or "2.5" or "2.6" or "2.7" or "2.8" or "2.9" or "2.10" or "2.11" or "2.12":
                {
                    await Dispatcher.UIThread.InvokeAsync(() => new Views.CheckForm(new ChangeOrCreateVM(rep.FormNum_DB, rep), errorList));

                    break;
                }
                default: return;
            }
        }
    }

    private static Task<BaseFormVM?> CreateFormVM(string formNum, Report rep)
    {
        BaseFormVM? vm = formNum switch
        {
            "1.1" => new Form_11VM(rep.Reports) { Report = rep },
            "1.2" => new Form_12VM(rep.Reports) { Report = rep },
            "1.3" => new Form_13VM(rep.Reports) { Report = rep },
            "1.4" => new Form_14VM(rep.Reports) { Report = rep },
            "1.5" => new Form_15VM(rep.Reports) { Report = rep },
            "1.6" => new Form_16VM(rep.Reports) { Report = rep },
            "1.7" => new Form_17VM(rep.Reports) { Report = rep },
            "1.8" => new Form_18VM(rep.Reports) { Report = rep },
            _ => null
        };

        return Task.FromResult(vm);
    }
}
