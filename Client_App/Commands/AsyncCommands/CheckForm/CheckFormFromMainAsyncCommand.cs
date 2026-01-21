using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms.Forms1;
using DynamicData;
using MessageBox.Avalonia.DTO;
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
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;

namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Проверяет отчёт из главного окна, не открывая его.
/// </summary>
/// <returns>Открывает окно с отчётом об ошибках.</returns>
public class CheckFormFromMainAsyncCommand : BaseAsyncCommand
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
        Report? par;
        if (parameter is IKeyCollection collection)
            par = collection.ToList<Report>().First();
        else if (parameter is Report)
            par = (Report)parameter;
        else
            return;

        await using var db = new DBModel(StaticConfiguration.DBPath);

        var cts = new CancellationTokenSource();

        #region GetReportFromDB

        var rep = await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsQueryable()
            .AsSplitQuery()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(x => x.Rows11.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows12.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows13.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows14.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows15.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows16.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows17.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows18.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows19.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows21.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows22.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows23.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows24.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows25.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows26.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows27.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows28.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows29.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows210.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows211.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows212.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Notes.OrderBy(x => x.Order))
            .Where(x => x.Reports != null && x.Reports.DBObservable != null)
            .FirstOrDefaultAsync(x => x.Id == par.Id, cts.Token);

        #endregion

        if (rep is null) return;
        List<CheckError> errorList = [];
        try
        {
            errorList.Add(rep.FormNum_DB switch
            {
                "1.1" => CheckF11.Check_Total(rep.Reports, rep),
                "1.2" => CheckF12.Check_Total(rep.Reports, rep),
                "1.3" => CheckF13.Check_Total(rep.Reports, rep),
                "1.4" => CheckF14.Check_Total(rep.Reports, rep),
                "1.5" => CheckF15.Check_Total(rep.Reports, rep),
                "1.6" => CheckF16.Check_Total(rep.Reports, rep),
                "1.7" => CheckF17.Check_Total(rep.Reports, rep),
                "1.8" => CheckF18.Check_Total(rep.Reports, rep),
                "2.1" => await new CheckF21().AsyncExecute(rep),
                "2.2" => await new CheckF22().AsyncExecute(rep),
                "2.3" => await new CheckF23().AsyncExecute(rep),
                "2.6" => await new CheckF26().AsyncExecute(rep),
                "2.7" => await new CheckF27().AsyncExecute(rep),
                "2.8" => await new CheckF28().AsyncExecute(rep),
                "2.9" => await new CheckF29().AsyncExecute(rep),
                "2.10" => await new CheckF210().AsyncExecute(rep),
                "2.11" => await new CheckF211().AsyncExecute(rep),
                _ => throw new NotImplementedException()
            });
        }
        catch (NotImplementedException)
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
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
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
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }
        if (errorList.Count == 0)
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
                    .ShowDialog(Desktop.MainWindow));

                #endregion
        }
        else
        {
            if (Desktop.Windows.Any(x => x.Name == "FormCheckerWindow"))
            {
                Desktop.Windows.First(x => x.Name == "FormCheckerWindow").Close();
            }

            switch (rep.FormNum_DB)
            {
                case "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8":
                {
                    var vm = await CreateFormVM(rep.FormNum_DB, rep);
                    await Dispatcher.UIThread.InvokeAsync(() => new Views.Forms.NewCheckForm(vm, errorList).Show());

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
            "1.3" => new Form_12VM(rep.Reports) { Report = rep },
            "1.4" => new Form_12VM(rep.Reports) { Report = rep },
            "1.5" => new Form_12VM(rep.Reports) { Report = rep },
            "1.6" => new Form_12VM(rep.Reports) { Report = rep },
            "1.7" => new Form_12VM(rep.Reports) { Report = rep },
            "1.8" => new Form_12VM(rep.Reports) { Report = rep },
            _ => null
        };

        return Task.FromResult(vm);
    }
}