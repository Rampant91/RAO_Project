using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms;
using Client_App.Views.Forms;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Models.CheckForm;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.CheckForm;

public class NewCheckFormAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
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
        var cts = new CancellationTokenSource();
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
                var run = await ReportCheckRunner.RunAsync(new ReportCheckRunner.Options
                {
                    ReportId = rep.Id,
                    FormNum = rep.FormNum_DB,
                    Progress = checkProgress,
                    CancellationToken = cts.Token
                });
                result.AddRange(run.Errors);
            }
            else
            {
                await using var db = new DBModel(StaticConfiguration.DBPath);
                switch (rep.FormNum_DB)
                {
                    case "2.1":
                    case "2.2":
                    case "2.3":
                    case "2.6":
                    case "2.7":
                    case "2.8":
                    case "2.9":
                    case "2.10":
                    case "2.11":
                    case "4.1":
                    {
                        var snapshot = await Services.DataAccess.ReportCheckSnapshotLoader.LoadAsync(
                            db, rep.Id, rep.FormNum_DB, cts.Token);
                        if (snapshot is null)
                        {
                            return;
                        }

                        result.AddRange(
                            await ReportCheckRunner.RunForm2OrLegacyAsync(snapshot, db, cts.Token));
                        break;
                    }
                    default:
                    {
                        #region MessageCheckFailed

                        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                            {
                                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                                ContentHeader = "Уведомление",
                                ContentMessage = "Функция проверки данных форм находится в процессе реализации.",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                                Topmost = true,
                            })
                            .ShowDialog(window ?? Desktop.MainWindow));

                        #endregion

                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Warning(msg);

            #region MessageCheckFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                    ContentHeader = "Уведомление",
                    ContentMessage = "В ходе выполнения проверки формы возникла непредвиденная ошибка.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(window ?? Desktop.MainWindow));

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

        if (result.Count == 0)
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                    ContentHeader = "Уведомление",
                    ContentMessage = "По результатам проверки формы, ошибок не выявлено.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(window ?? Desktop.MainWindow));

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
}
