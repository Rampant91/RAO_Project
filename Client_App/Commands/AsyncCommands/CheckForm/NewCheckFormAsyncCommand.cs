using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using Models.CheckForm;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Views.Forms;
using Microsoft.EntityFrameworkCore;

namespace Client_App.Commands.AsyncCommands.CheckForm;

public class NewCheckFormAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        try
        {
            await Task.Run(() => AsyncExecute(parameter));
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

        await using var db = new DBModel(StaticConfiguration.DBPath);
        List<CheckError> result = [];
        try
        {
            switch (rep.FormNum_DB)
            {
                case "1.1":
                    result.AddRange(CheckF11.Check_Total(reps, rep));
                    break;
                case "1.2":
                    result.AddRange(CheckF12.Check_Total(reps, rep));
                    break;
                case "1.3":
                    result.AddRange(CheckF13.Check_Total(reps, rep));
                    break;
                case "1.4":
                    result.AddRange(CheckF14.Check_Total(reps, rep));
                    break;
                case "1.5":
                    result.AddRange(CheckF15.Check_Total(reps, rep));
                    break;
                case "1.6":
                    result.AddRange(CheckF16.Check_Total(reps, rep));
                    break;
                case "1.7":
                    result.AddRange(CheckF17.Check_Total(reps, rep));
                    break;
                case "1.8":
                    result.AddRange(CheckF18.Check_Total(reps, rep));
                    break;
                case "2.1":
                    var rep21 = await db.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsQueryable()
                        .AsSplitQuery()
                        .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                        .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows40)
                        .Include(x => x.Rows21.OrderBy(form => form.NumberInOrder_DB))
                        .Include(x => x.Notes.OrderBy(note => note.Order))
                        .Where(x => x.Reports != null && x.Reports.DBObservable != null)
                    .FirstOrDefaultAsync(x => x.Id == rep.Id, cts.Token);

                    result.AddRange(await new CheckF21().AsyncExecute(rep21));
                    break;
                case "2.2":
                    var rep22 = await db.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsQueryable()
                        .AsSplitQuery()
                        .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                        .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows40)
                        .Include(x => x.Rows22.OrderBy(form => form.NumberInOrder_DB))
                        .Include(x => x.Notes.OrderBy(note => note.Order))
                        .Where(x => x.Reports != null && x.Reports.DBObservable != null)
                    .FirstOrDefaultAsync(x => x.Id == rep.Id, cts.Token);

                    result.AddRange(await new CheckF22().AsyncExecute(rep22));
                    break;
                case "2.3":
                    result.AddRange(await CheckF23.Check_Total(rep));
                    break;
                case "2.6":
                    result.AddRange(await CheckF26.Check_Total(rep));
                    break;
                case "2.7":
                    result.AddRange(await CheckF27.Check_Total(rep));
                    break;
                case "2.8":
                    result.AddRange(await CheckF28.Check_Total(rep));
                    break;
                case "2.9":
                    result.AddRange(await CheckF29.Check_Total(rep));
                    break;
                case "2.10":
                    result.AddRange(await CheckF210.Check_Total(rep));
                    break;
                case "2.11":
                    result.AddRange(await CheckF211.Check_Total(rep));
                    break;
                case "2.12":
                    result.AddRange(await CheckF212.Check_Total(rep));
                    break;
                case "4.1":
                    result.AddRange(await CheckF41.Check_Total(rep));
                    break;
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
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(window ?? Desktop.MainWindow));

                    #endregion

                    return;
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
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(window ?? Desktop.MainWindow));

            #endregion

            return;
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
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(window ?? Desktop.MainWindow));

            #endregion
        }
        else
        {
            if (Desktop.Windows.Any(x => x.Name == "FormCheckerWindow"))
            {
                Desktop.Windows.First(x => x.Name == "FormCheckerWindow").Close();
            }
            await Dispatcher.UIThread.InvokeAsync(() => new NewCheckForm(formVM, result));
        }
    }
}