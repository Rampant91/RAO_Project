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
/// Проверка отчётов по форме 2.10. 
/// </summary>
public class CheckF210 : CheckBase
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

        Reports? reps210Prev = null;
        foreach (var _ in db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == form20RegNo))
            .Include(x => x.Report_Collection)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == yearPrevious && y.FormNum_DB == "2.10"))
            .ThenInclude(x => x.Rows210))
            if (_.Report_Collection.Count > 0) { reps210Prev = _; break; }
        await db2.DisposeAsync();

        List<Form210> Rows210Cur = rep.Rows210.ToList();
        List<Form210> Rows210Prev = new();
        if (reps210Prev != null && reps210Prev.Report_Collection != null)
        {
            foreach (var key in reps210Prev.Report_Collection)
            {
                var report = (Report)key;
                report.Rows210 = new(report.Rows210.OrderBy(x => x.NumberInOrder_DB));
                Rows210Prev.AddRange(report.Rows210);
            }
        }

        Check_NameComparePrev(errorList, rep, Rows210Prev);
        Check_RadiationCode(errorList, rep);
        Check_RadiationMinmax(errorList, rep);

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

    #region Check_NameComparePrev

    private static void Check_NameComparePrev(List<CheckError> errorList, Report rep, List<Form210> Rows210Prev)
    {
        foreach (Form210 rowCur in rep.Rows210)
        {
            if (rowCur.IndicatorName_DB.ToLowerInvariant().Trim() is "h" or "н")
            {
                bool found = false;
                foreach (Form210 rowPrev in Rows210Prev)
                {
                    if (rowCur.PlotName_DB.Trim().Equals(rowPrev.PlotName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    errorList.Add(new CheckError
                    {
                        FormNum = "form_210",
                        Row = $"{rowCur.NumberInOrder_DB}",
                        Column = $"{GraphsList["PlotName_DB"]}",
                        Value = $"{rowCur.PlotName_DB}",
                        Message = $"Сведения о наличии данного участка на конец предыдущего года отсутствуют.  Представьте сведения о выявлении загрязненного участка территории."
                    });
                }
            }
        }
    }

    #endregion
    #region Check_RadiationCode

    private static void Check_RadiationCode(List<CheckError> errorList, Report rep)
    {
        foreach (Form210 row in rep.Rows210)
        {
            if (double.TryParse(row.AvgGammaRaysDosePower_DB, out var dosePower)
                && row.PlotCode_DB.Length >= 3)
            {
                if (dosePower < 0) continue;
                char plotCode3 = row.PlotCode_DB.ElementAt(2);
                char plotCode3Expected = '?';
                if (dosePower <= 0.036)
                {
                    plotCode3Expected = '0';
                    if (plotCode3 != plotCode3Expected)
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_210",
                            Row = $"{row.NumberInOrder_DB}",
                            Column = $"{GraphsList["PlotCode_DB"]}",
                            Value = $"3-ий символ в графе 5: {plotCode3}; значение в графе 7: {row.AvgGammaRaysDosePower_DB}",
                            Message = $"В 3-ем символе кода участка (графа 5) неверно указан идентификатор диапазона по мощности дозы гамма-излучения, для значения, указанного в графе 7 (для символа 0 ожидается значение, меньшее либо равное 0,036; для значения {row.AvgGammaRaysDosePower_DB} ожидается символ {plotCode3Expected})."
                        });
                    }
                }
                else if (dosePower <= 0.12)
                {
                    plotCode3Expected = '1';
                    if (plotCode3 != plotCode3Expected)
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_210",
                            Row = $"{row.NumberInOrder_DB}",
                            Column = $"{GraphsList["PlotCode_DB"]}",
                            Value = $"3-ий символ в графе 5: {plotCode3}; значение в графе 7: {row.AvgGammaRaysDosePower_DB}",
                            Message = $"В 3-ем символе кода участка (графа 5) неверно указан идентификатор диапазона по мощности дозы гамма-излучения, для значения, указанного в графе 7 (для символа 1 ожидается значение, большее 0,036 и меньшее либо равное 0,12; для значения {row.AvgGammaRaysDosePower_DB} ожидается символ {plotCode3Expected})."
                        });
                    }
                }
                else if (dosePower <= 0.6)
                {
                    plotCode3Expected = '2';
                    if (plotCode3 != plotCode3Expected)
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_210",
                            Row = $"{row.NumberInOrder_DB}",
                            Column = $"{GraphsList["PlotCode_DB"]}",
                            Value = $"3-ий символ в графе 5: {plotCode3}; значение в графе 7: {row.AvgGammaRaysDosePower_DB}",
                            Message = $"В 3-ем символе кода участка (графа 5) неверно указан идентификатор диапазона по мощности дозы гамма-излучения, для значения, указанного в графе 7 (для символа 1 ожидается значение, большее 0,12 и меньшее либо равное 0,6; для значения {row.AvgGammaRaysDosePower_DB} ожидается символ {plotCode3Expected})."
                        });
                    }
                }
                else if (dosePower <= 2.4)
                {
                    plotCode3Expected = '3';
                    if (plotCode3 != plotCode3Expected)
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_210",
                            Row = $"{row.NumberInOrder_DB}",
                            Column = $"{GraphsList["PlotCode_DB"]}",
                            Value = $"3-ий символ в графе 5: {plotCode3}; значение в графе 7: {row.AvgGammaRaysDosePower_DB}",
                            Message = $"В 3-ем символе кода участка (графа 5) неверно указан идентификатор диапазона по мощности дозы гамма-излучения, для значения, указанного в графе 7 (для символа 1 ожидается значение, большее 0,6 и меньшее либо равное 2,4; для значения {row.AvgGammaRaysDosePower_DB} ожидается символ {plotCode3Expected})."
                        });
                    }
                }
                else if (dosePower <= 6.0)
                {
                    plotCode3Expected = '4';
                    if (plotCode3 != plotCode3Expected)
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_210",
                            Row = $"{row.NumberInOrder_DB}",
                            Column = $"{GraphsList["PlotCode_DB"]}",
                            Value = $"3-ий символ в графе 5: {plotCode3}; значение в графе 7: {row.AvgGammaRaysDosePower_DB}",
                            Message = $"В 3-ем символе кода участка (графа 5) неверно указан идентификатор диапазона по мощности дозы гамма-излучения, для значения, указанного в графе 7 (для символа 4 ожидается значение, большее 2,4 и меньшее либо равное 6,0; для значения {row.AvgGammaRaysDosePower_DB} ожидается символ {plotCode3Expected})."
                        });
                    }
                }
                else
                {
                    plotCode3Expected = '5';
                    if (plotCode3 != plotCode3Expected)
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_210",
                            Row = $"{row.NumberInOrder_DB}",
                            Column = $"{GraphsList["PlotCode_DB"]}",
                            Value = $"3-ий символ в графе 5: {plotCode3}; значение в графе 7: {row.AvgGammaRaysDosePower_DB}",
                            Message = $"В 3-ем символе кода участка (графа 5) неверно указан идентификатор диапазона по мощности дозы гамма-излучения, для значения, указанного в графе 7 (для символа 5 ожидается значение, большее 6,0; для значения {row.AvgGammaRaysDosePower_DB} ожидается символ {plotCode3Expected})."
                        });
                    }
                }
            }
        }
    }

    #endregion
    #region Check_RadiationMinmax

    private static void Check_RadiationMinmax(List<CheckError> errorList, Report rep)
    {
        foreach (Form210 row in rep.Rows210)
        {
            double.TryParse(row.MaxGammaRaysDosePower_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var doseMaxValue);
            double.TryParse(row.AvgGammaRaysDosePower_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var doseAverageValue);
            if (doseAverageValue > doseMaxValue)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_28",
                    Row = $"{row.NumberInOrder_DB}",
                    Column = $"{GraphsList["MaxGammaRaysDosePower_DB"]}",
                    Value = row.MaxGammaRaysDosePower_DB,
                    Message = $"Максимальная мощность дозы гамма-излучения не может быть меньше среднего значения. Уточните приведенные сведения."
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
        { "IndicatorName_DB", "02 - Наименование показателя" },
        { "PlotName_DB", "03 - Наименование участка" },
        { "PlotKadastrNumber_DB", "04 - Кадастровый номер участка" },
        { "PlotCode_DB", "05 - Код участка" },
        { "InfectedArea_DB", "06 - Площадь загрязненной территории, кв. м" },
        { "AvgGammaRaysDosePower_DB", "07 - Мощность дозы гамма-излучения, мкЗв/час - средняя" },
        { "MaxGammaRaysDosePower_DB", "08 - Мощность дозы гамма-излучения, мкЗв/час - максимальная" },
        { "WasteDensityAlpha_DB", "09 - Плотность загрязнения (средняя), Бк/кв. м - альфа-излучающие радионуклиды" },
        { "WasteDensityBeta_DB", "10 - Плотность загрязнения (средняя), Бк/кв. м - бета-излучающие радионуклиды" },
        { "FcpNumber_DB", "11 - Номер мероприятия ФЦП" },
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