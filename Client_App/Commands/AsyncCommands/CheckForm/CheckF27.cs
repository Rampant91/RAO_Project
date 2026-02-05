using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Проверка отчётов по форме 2.7. 
/// </summary>
public class CheckF27 : CheckBase
{
    public override bool CanExecute(object? parameter) => true;

    #region AsyncExecute

    public override async Task<List<CheckError>> AsyncExecute(object? parameter)
    {
        return await Check_Total(parameter);
    }

    #endregion

    #region MainCheck

    public static async Task<List<CheckError>> Check_Total(object? parameter)
    {
        var cts = new CancellationTokenSource();
        List<CheckError> errorList = [];
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var rep = parameter as Report;
        if (rep is null)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        var db2 = new DBModel(StaticConfiguration.DBPath);

        string form20RegNo = rep!.Reports.Master_DB.RegNoRep.Value;
        string form20Okpo = rep!.Reports.Master_DB.OkpoRep.Value;

        string repYear = rep.Year_DB;
        string repFormNum = rep.FormNum_DB;

        if (string.IsNullOrWhiteSpace(form20RegNo))
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }


        int yearRealCurrent;
        int.TryParse(repYear, out yearRealCurrent);
        string yearPrevious = (yearRealCurrent - 1).ToString();

        Reports? reps27Prev = null;
        foreach (var _ in db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == form20RegNo))
            .Include(x => x.Report_Collection)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == yearPrevious && y.FormNum_DB == "2.7")).ThenInclude(report => report.Rows27)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == yearPrevious && y.FormNum_DB == "2.7")).ThenInclude(x => x.Rows210))
            if (_.Report_Collection.Count > 0) { reps27Prev = _; break; }

        await db2.DisposeAsync();

        List<Form27> Rows27Cur = rep.Rows27.ToList();
        List<Form27> Rows27Prev = new();
        if (reps27Prev != null && reps27Prev.Report_Collection != null)
        {
            foreach (var key in reps27Prev.Report_Collection)
            {
                var report = (Report)key;
                report.Rows27 = new(report.Rows27.OrderBy(x => x.NumberInOrder_DB));
                Rows27Prev.AddRange(report.Rows27);
            }
        }

        Check_HeaderExpirationDate(errorList, rep);
        Check_ExhaustMinmaxCur(errorList, rep);
        Check_ExhaustCheckPrev(errorList, rep, Rows27Prev);

        errorList.Sort((i, j) =>
            int.TryParse(i.Row.Split(',')[0], out var iRowReal)
            && int.TryParse(j.Row.Split(',')[0], out var jRowReal)
                ? iRowReal - jRowReal
                : string.Compare(i.Row, j.Row));
        var index = 0;
        foreach (var error in errorList)
        {
            if (GraphsList.TryGetValue(error.Column, out var columnFrontName))
            {
                error.Column = columnFrontName;
            }
            index++;
            error.Index = index;
        }

        progressBarVM.SetProgressBar(100, "Завершение проверки");
        await progressBar.CloseAsync();

        return errorList;
    }

    #endregion

    #region Check_HeaderExpirationDate

    private static void Check_HeaderExpirationDate(List<CheckError> errorList, Report rep)
    {
        if (DateTime.TryParse(rep.ValidThru27_DB, out var validThru27)
            && int.TryParse(rep.Year_DB, out var Year)
            && validThru27 < new DateTime(Year, 12, DateTime.DaysInMonth(Year, 12)))
        {
            errorList.Add(new CheckError
            {
                FormNum = "form_27",
                Row = "-",
                Column = "-",
                Value = "",
                Message = $"Срок действия разрешения истек. Приведите сведения об актуальном разрешении."
            });
        }
    }

    #endregion
    #region Check_ExhaustMinmaxCur

    private static void Check_ExhaustMinmaxCur(List<CheckError> errorList, Report rep)
    {
        foreach (Form27 row in rep.Rows27)
        {
            double.TryParse(row.AllowedWasteValue_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var allowedWasteValue);
            double.TryParse(row.FactedWasteValue_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var factedWasteValue);
            if (allowedWasteValue < factedWasteValue)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_27",
                    Row = $"{row.NumberInOrder_DB}",
                    Column = $"{GraphsList["FactedWasteValue_DB"]}",
                    Value = row.FactedWasteValue_DB,
                    Message = $"Величина фактического выброса превышает разрешенное. Проверьте правильность сведений."
                });
            }
        }
    }

    #endregion
    #region Check_ExhaustCheckPrev

    private static void Check_ExhaustCheckPrev(List<CheckError> errorList, Report rep, List<Form27> Rows27Prev)
    {
        foreach (Form27 rowCur in rep.Rows27)
        {
            foreach (Form27 rowPrev in Rows27Prev)
            {
                if (rowCur.ObservedSourceNumber_DB.Trim().Equals(rowPrev.ObservedSourceNumber_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)
                    && rowCur.RadionuclidName_DB.Replace(" ","").Equals(rowPrev.RadionuclidName_DB.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!(double.TryParse(rowCur.WasteOutbreakPreviousYear_DB, out var wasteOutbreakCur)
                        && double.TryParse(rowPrev.FactedWasteValue_DB, out var wasteOutbreakPrev)
                        && wasteOutbreakCur == wasteOutbreakPrev))
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_27",
                            Row = $"{rowCur.NumberInOrder_DB}",
                            Column = $"{GraphsList["WasteOutbreakPreviousYear_DB"]}",
                            Value = rowCur.WasteOutbreakPreviousYear_DB,
                            Message = $"Сведения о фактическом выбросе за предыдущий год не совпадают с данными представленными в отчете. Проверьте правильность сведений."
                        });
                    }
                    break;
                }
            }
        }
    }

    #endregion

    #region CancelCommandAndCloseProgressBarWindow

    /// <summary>
    /// Отмена исполняемой команды и закрытие окна прогрессбара.
    /// </summary>
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <returns></returns>
    private static async Task CancelCommandAndCloseProgressBarWindow(CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null)
    {
        await cts.CancelAsync();
        if (progressBar is not null) await progressBar.CloseAsync();
        cts.Token.ThrowIfCancellationRequested();
    }

    #endregion

    #region Properties

    private static readonly Dictionary<string, string> GraphsList = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "ObservedSourceNumber_DB", "02 - Наименование, номер источника выбросов" },
        { "RadionuclidName_DB", "03 - Наименование радионуклида" },
        { "AllowedWasteValue_DB", "04 - Выброс радиоуклида в атмосферу за отчётный год, Бк - разрешенный" },
        { "FactedWasteValue_DB", "05 - Выброс радиоуклида в атмосферу за отчётный год, Бк - фактический" },
        { "WasteOutbreakPreviousYear_DB", "06 - Выброс радиоуклида в атмосферу за предыдущий год, Бк - фактический" },
    };

    #endregion

    #region ExcelGetFullPath

    /// <summary>
    /// Выводит сообщение, дающее выбор, открывать временную копию или сохранить файл.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <returns>Полный путь до файла и флаг, нужно ли открывать временную копию.</returns>
    private protected static async Task<(string fullPath, bool openTemp)> ExcelGetFullPath(string fileName, CancellationTokenSource cts,
        AnyTaskProgressBar? progressBar = null)
    {
        #region MessageSaveOrOpenTemp

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Сохранить" },
                    new ButtonDefinition { Name = "Открыть временную копию" }
                ],
                CanResize = true,
                ContentTitle = "Выгрузка в .xlsx",
                ContentHeader = "Уведомление",
                ContentMessage = "Что бы вы хотели сделать с данной выгрузкой?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        var fullPath = "";
        var openTemp = res is "Открыть временную копию";

        switch (res)
        {
            case "Открыть временную копию":
                {
                    DirectoryInfo tmpFolder = new(Path.Combine(BaseVM.SystemDirectory, "RAO", "temp"));
                    var count = 0;

                    fullPath = Path.Combine(tmpFolder.FullName, fileName + ".xlsx");
                    while (File.Exists(fullPath))
                    {
                        fullPath = Path.Combine(tmpFolder.FullName, fileName + $"_{++count}.xlsx");
                    }

                    break;
                }
            case "Сохранить":
                {
                    SaveFileDialog dial = new();
                    var filter = new FileDialogFilter
                    {
                        Name = "Excel",
                        Extensions = { "xlsx" }
                    };
                    dial.Filters.Add(filter);
                    dial.InitialFileName = fileName;
                    fullPath = await dial.ShowAsync(Desktop.MainWindow);
                    if (string.IsNullOrEmpty(fullPath)) await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                    if (!fullPath.EndsWith(".xlsx")) fullPath += ".xlsx"; //В проводнике Linux в имя файла не подставляется расширение из фильтра, добавляю руками если его нет
                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            File.Delete(fullPath!);
                        }
                        catch
                        {
                            #region MessageFailedToSaveFile

                            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                                {
                                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                    ContentTitle = "Выгрузка в .xlsx",
                                    ContentHeader = "Ошибка",
                                    ContentMessage =
                                        $"Не удалось сохранить файл по пути: {fullPath}" +
                                        $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                        $"{Environment.NewLine}и используется другим процессом.",
                                    MinWidth = 400,
                                    MinHeight = 150,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(Desktop.MainWindow));

                            #endregion

                            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                        }
                    }

                    break;
                }
            default:
                {
                    await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                    break;
                }
        }
        return (fullPath, openTemp);
    }

    #endregion
}