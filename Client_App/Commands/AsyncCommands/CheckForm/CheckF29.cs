using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using DynamicData;
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
/// Проверка отчётов по форме 2.9. 
/// </summary>
public class CheckF29 : CheckBase
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

        Reports? reps29Prev = null;
        foreach (var _ in db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == form20RegNo))
            .Include(x => x.Report_Collection)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == yearPrevious && y.FormNum_DB == "2.9")).ThenInclude(report => report.Rows29)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == yearPrevious && y.FormNum_DB == "2.9")).ThenInclude(x => x.Rows210))
            if (_.Report_Collection.Count > 0) { reps29Prev = _; break; }

        Reports? reps28Cur = null;
        foreach (var _ in db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == form20RegNo))
            .Include(x => x.Report_Collection)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == yearPrevious && y.FormNum_DB == "2.8")).ThenInclude(report => report.Rows28)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == yearPrevious && y.FormNum_DB == "2.8")).ThenInclude(x => x.Rows210))
            if (_.Report_Collection.Count > 0) { reps28Cur = _; break; }

        await db2.DisposeAsync();

        List<Form29> Rows29Cur = rep.Rows29.ToList();
        List<Form29> Rows29Prev = new();
        List<Form28> Rows28Cur = new();
        if (reps29Prev != null && reps29Prev.Report_Collection != null)
        {
            foreach (var key in reps29Prev.Report_Collection)
            {
                var report = (Report)key;
                report.Rows29 = new(report.Rows29.OrderBy(x => x.NumberInOrder_DB));
                Rows29Prev.AddRange(report.Rows29);
            }
        }
        if (reps28Cur != null && reps28Cur.Report_Collection != null)
        {
            foreach (var key in reps28Cur.Report_Collection)
            {
                var report = (Report)key;
                report.Rows28 = new(report.Rows28.OrderBy(x => x.NumberInOrder_DB));
                Rows28Cur.AddRange(report.Rows28);
            }
        }

        Check_NameFrom28(errorList, rep, Rows28Cur);
        Check_ActivityMinmax(errorList, rep);
        Check_ActivityPrevPercent(errorList, rep, Rows29Prev);

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

    #region Check_NameFrom28

    private static void Check_NameFrom28(List<CheckError> errorList, Report rep, List<Form28> Rows28Cur)
    {
        List<Form29> Rows29Cur = new(rep.Rows29);
        List<string> Names29 = new();
        List<string> Names28 = new();
        foreach (Form29 row29 in Rows29Cur)
        {
            if (string.IsNullOrWhiteSpace(row29.WasteSourceName_DB) || row29.WasteSourceName_DB.Trim() == "-") continue;
            bool found = false;
            foreach (Form28 row28 in Rows28Cur)
            {
                if (string.IsNullOrWhiteSpace(row28.WasteSourceName_DB) || row28.WasteSourceName_DB.Trim() == "-") continue;
                if (row29.WasteSourceName_DB.Trim().Equals(row28.WasteSourceName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_29",
                    Row = $"{row29.NumberInOrder_DB}",
                    Column = GraphsList["WasteSourceName_DB"],
                    Value = row29.WasteSourceName_DB,
                    Message = $"Наименование, номер выпуска сточных вод не соответствует сведениям, представленным в форме 2.8."
                });
            }
        }
    }

    #endregion
    #region Check_ActivityMinmax

    private static void Check_ActivityMinmax(List<CheckError> errorList, Report rep)
    {
        foreach (Form29 row in rep.Rows29)
        {
            double.TryParse(row.FactedActivity_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var factedActivityValue);
            double.TryParse(row.AllowedActivity_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var allowedActivityValue);
            if (allowedActivityValue < factedActivityValue)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_29",
                    Row = $"{row.NumberInOrder_DB}",
                    Column = $"{GraphsList["FactedActivity_DB"]}",
                    Value = row.FactedActivity_DB,
                    Message = $"Величина фактического сброса превышает разрешенное. Проверьте правильность сведений."
                });
            }
        }
    }

    #endregion
    #region Check_ActivityPrevPercent

    private static void Check_ActivityPrevPercent(List<CheckError> errorList, Report rep, List<Form29> Rows29Prev)
    {
        foreach (Form29 rowCur in rep.Rows29)
        {
            foreach (Form29 rowPrev in Rows29Prev)
            {
                if (rowCur.WasteSourceName_DB.Trim().Equals(rowPrev.WasteSourceName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)
                    && rowCur.RadionuclidName_DB.Trim().Equals(rowPrev.RadionuclidName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    if (!(double.TryParse(rowCur.FactedActivity_DB, out var factedActivityCur)
                        && double.TryParse(rowPrev.FactedActivity_DB, out var factedActivityPrev)
                        && factedActivityCur <= 1.2 * factedActivityPrev
                        && factedActivityPrev <= 1.2 * factedActivityCur))
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_29",
                            Row = $"{rowCur.NumberInOrder_DB}",
                            Column = $"{GraphsList["FactedActivity_DB"]}",
                            Value = $"{rowCur.FactedActivity_DB} (предыдущий год: {rowPrev.FactedActivity_DB})",
                            Message = $"Сведения о фактическом сбросе за предыдущий год существенно отличаются от данных представленных в отчете. Проверьте правильность сведений."
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
        { "WasteSourceName_DB", "02 - Наименование, номер выпуска сточных вод" },
        { "RadionuclidName_DB", "03 - Наименование радионуклида" },
        { "AllowedActivity_DB", "04 - Активность радионуклида, Бк - допустимая" },
        { "FactedActivity_DB", "05 - Активность радионуклида, Бк - фактическая" }
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