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
/// Проверка отчётов по форме 2.11. 
/// </summary>
public class CheckF211 : CheckBase
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

        Reports? reps210Cur = await db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows20)
            .Include(reps => reps.Report_Collection
                .Where(report =>
                    (report.FormNum_DB == "2.10")
                    && (report.Year_DB == yearPrevious)))
            .ThenInclude(x => x.Rows210)
            .Where(reps => reps.DBObservable != null)
            .FirstOrDefaultAsync(reps => reps.Master_DB.Rows20
                .Any(form20 => form20.RegNo_DB == form20RegNo), cts.Token);

        await db2.DisposeAsync();

        List<Form211> Rows211Cur = rep.Rows211.ToList();
        List<Form210> Rows210Cur = new();
        if (reps210Cur != null && reps210Cur.Report_Collection != null)
        {
            foreach (var key in reps210Cur.Report_Collection)
            {
                var report = (Report)key;
                report.Rows210 = new(report.Rows210.OrderBy(x => x.NumberInOrder_DB));
                Rows210Cur.AddRange(report.Rows210);
            }
        }

        Check_Indicator210(errorList, rep, Rows210Cur);
        Check_PlotCode1(errorList, rep);
        Check_PlotCode2(errorList, rep);

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

    #region Check_Indicator210

    private static void Check_Indicator210(List<CheckError> errorList, Report rep, List<Form210> Rows210Cur)
    {
        foreach (Form211 row211 in rep.Rows211)
        {
            string keyname211 = $"{row211.PlotName_DB}{row211.PlotKadastrNumber_DB}{row211.PlotCode_DB}{row211.InfectedArea_DB}".Replace(" ", "");
            foreach (Form210 row210 in Rows210Cur)
            {
                string keyname210 = $"{row210.PlotName_DB}{row210.PlotKadastrNumber_DB}{row210.PlotCode_DB}{row210.InfectedArea_DB}".Replace(" ", "");
                if (keyname211.Equals(keyname210, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (row210.IndicatorName_DB.Trim().ToLowerInvariant().Replace('н','h') is not "h")
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_211",
                            Row = $"{row211.NumberInOrder_DB}",
                            Column = $"-",
                            Value = $"-",
                            Message = $"Форма заполняется только для загрязненных территорий, имеющихся в наличии на конец отчетного года."
                        });
                    }
                    break;
                }
            }
        }
    }

    #endregion
    #region Check_PlotCode1

    private static void Check_PlotCode1(List<CheckError> errorList, Report rep)
    {
        foreach (Form211 row in rep.Rows211)
        {
            if (string.IsNullOrWhiteSpace(row.PlotCode_DB) || row.PlotCode_DB.Trim().Length < 1) continue;
            bool filled = string.IsNullOrWhiteSpace(row.SpecificActivityOfPlot_DB) || row.SpecificActivityOfPlot_DB.Trim() == "-";
            bool symbolisvalid = row.PlotCode_DB.Trim().ElementAt(0) is '1';
            if (filled && !symbolisvalid)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_211",
                    Row = $"{row.NumberInOrder_DB}",
                    Column = $"{GraphsList["PlotCode_DB"]}",
                    Value = $"-",
                    Message = $"Несоответствие значения в графе 7 первому символу в коде загрязненного участка (графа 4)."
                });
            }
        }
    }

    #endregion
    #region Check_PlotCode2

    private static void Check_PlotCode2(List<CheckError> errorList, Report rep)
    {
        foreach (Form211 row in rep.Rows211)
        {
            if (string.IsNullOrWhiteSpace(row.PlotCode_DB) || row.PlotCode_DB.Trim().Length < 1) continue;
            bool filled8 = string.IsNullOrWhiteSpace(row.SpecificActivityOfLiquidPart_DB) || row.SpecificActivityOfLiquidPart_DB.Trim() == "-";
            bool filled9 = string.IsNullOrWhiteSpace(row.SpecificActivityOfDensePart_DB) || row.SpecificActivityOfDensePart_DB.Trim() == "-";
            bool symbolisvalid = row.PlotCode_DB.Trim().ElementAt(0) is '2';
            if ((filled8 || filled9) && !symbolisvalid)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_211",
                    Row = $"{row.NumberInOrder_DB}",
                    Column = $"{GraphsList["PlotCode_DB"]}",
                    Value = $"-",
                    Message = $"Несоответствие значения в графах 8, 9 первому символу в коде загрязненного участка (графа 4)."
                });
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
        { "PlotName_DB", "02 - Наименование участка" },
        { "PlotKadastrNumber_DB", "03 - Кадастровый номер участка" },
        { "PlotCode_DB", "04 - Код участка" },
        { "InfectedArea_DB", "05 - Площадь загрязненной территории, кв. м" },
        { "Radionuclids_DB", "06 - Наименование радионуклида" },
        { "SpecificActivityOfPlot_DB", "07 - Удельная активность радионуклида, Бк/г - земельный участок" },
        { "SpecificActivityOfLiquidPart_DB", "08 - Удельная активность радионуклида, Бк/г - водный объект - жидкая фаза" },
        { "SpecificActivityOfDensePart_DB", "09 - Удельная активность радионуклида, Бк/г - водный объект - донные отложения" },
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
                ContentTitle = "Выгрузка в Excel",
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
                                    ContentTitle = "Выгрузка в Excel",
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