using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using Client_App.Views.Forms;
using Client_App.Views.ProgressBar;
using MsBox.Avalonia.Dto;
using Models.CheckForm;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Проверяет открытую форму (новый UI форм), кнопка "Проверить".
/// </summary>
public class NewCheckFormAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        try
        {
            await Task.Run(async () => await AsyncExecute(parameter));
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
        var cts = new CancellationTokenSource();

        var reps = formVM.Reports;
        var rep = formVM.Report;

        var window = Desktop.Windows.FirstOrDefault(x => x.Name == rep.FormNum_DB);

        AnyTaskProgressBar? progressBar = null;
        List<CheckError> result = [];
        try
        {
            if (rep.FormNum_DB is "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8")
            {
                progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
                var checkProgress = ReportCheckProgress.ForStandalone(progressBar.AnyTaskProgressBarVM);
                checkProgress.OnLoadComplete(ReportCheckSnapshotLoader.CountLoadedRows(rep));
                result.AddRange(await Task.Run(
                    () => ReportCheckRunner.ExecuteCheck(reps, rep, checkProgress),
                    cts.Token));
            }
            else if (IsForm2Or41(rep.FormNum_DB))
            {
                await using var db = new DBModel(StaticConfiguration.DBPath);
                var snapshot = await ReportCheckSnapshotLoader.LoadAsync(db, rep.Id, rep.FormNum_DB, cts.Token)
                               ?? rep;
                result.AddRange(await ReportCheckRunner.RunForm2OrLegacyAsync(snapshot, db, cts.Token));
            }
            else
            {
                #region MessageCheckFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Функция проверки данных форм находится в процессе реализации.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).ShowWindowDialogAsync(window ?? Desktop.MainWindow));

                #endregion

                return;
            }
        }
        catch (NotImplementedException)
        {
            #region MessageCheckFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Функция проверки данных форм находится в процессе реализации.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(window ?? Desktop.MainWindow));

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
                    ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                    ContentHeader = "Уведомление",
                    ContentMessage = "В ходе выполнения проверки формы возникла непредвиденная ошибка.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(window ?? Desktop.MainWindow));

            #endregion

            return;
        }
        finally
        {
            if (progressBar is not null)
            {
                try
                {
                    await progressBar.CloseAsync();
                }
                catch
                {
                    // Already closed.
                }
            }
        }

        if (result.Count == 0)
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
                }).ShowWindowDialogAsync(window ?? Desktop.MainWindow));

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
                case "4.1":
                    await Dispatcher.UIThread.InvokeAsync(() => new NewCheckForm41(formVM, result));
                    break;
                default:
                    await Dispatcher.UIThread.InvokeAsync(() => new NewCheckForm(formVM, result));
                    break;
            }
        }
    }

    private static bool IsForm2Or41(string formNum) =>
        formNum is "2.1" or "2.2" or "2.3" or "2.6" or "2.7" or "2.8" or "2.9" or "2.10" or "2.11" or "4.1";
}
