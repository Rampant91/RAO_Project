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
/// Проверка отчётов по форме 2.3. 
/// </summary>
public class CheckF23 : CheckBase
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

        Reports? reps23Prev = null;
        foreach (var _ in db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == form20RegNo))
            .Include(x => x.Report_Collection)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == yearPrevious && y.FormNum_DB == "2.3"))
            .ThenInclude(x => x.Rows210))
            if (_.Report_Collection.Count > 0) { reps23Prev = _; break; }

        Reports? reps22Cur = null;
        foreach (var _ in db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == form20RegNo))
            .Include(x => x.Report_Collection)
            .Include(x => x.Report_Collection.Where(y => y.Year_DB == repYear && y.FormNum_DB == "2.2"))
            .ThenInclude(x => x.Rows210))
            if (_.Report_Collection.Count > 0) { reps22Cur = _; break; }

        await db2.DisposeAsync();

        List<Form23> Rows23Cur = rep.Rows23.ToList();
        List<Form23> Rows23Prev = new();
        List<Form22> Rows22Cur = new();
        if (reps23Prev != null && reps23Prev.Report_Collection != null)
        {
            foreach (var key in reps23Prev.Report_Collection)
            {
                var report = (Report)key;
                report.Rows23 = new(report.Rows23.OrderBy(x => x.NumberInOrder_DB));
                Rows23Prev.AddRange(report.Rows23);
            }
        }
        if (reps22Cur != null && reps22Cur.Report_Collection != null)
        {
            foreach (var key in reps22Cur.Report_Collection)
            {
                var report = (Report)key;
                report.Rows22 = new(report.Rows22.OrderBy(x => x.NumberInOrder_DB));
                Rows22Cur.AddRange(report.Rows22);
            }
        }

        Check_StoragesFrom22(errorList, rep, Rows22Cur);
        Check_DocumentExpiryDate(errorList, rep);
        Check_ProjectVolume(errorList, rep, Rows23Prev);
        Check_RAOFrom22(errorList, rep, Rows22Cur);

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

    #region Check_StoragesFrom22

    private static void Check_StoragesFrom22(List<CheckError> errorList, Report rep, List<Form22> Rows22Cur)
    {
        List<Form23> Rows23Cur = new(rep.Rows23);
        List<(string, string)> Storages22 = new();
        List<string> Storages23 = new();
        foreach (Form23 row in Rows23Cur)
        {
            if (string.IsNullOrWhiteSpace(row.StoragePlaceName_DB) || row.StoragePlaceName_DB.Trim() == "-") continue;
            Storages23.Add(
                $"{row.StoragePlaceName_DB.Trim().ToLowerInvariant()}{row.StoragePlaceCode_DB.Trim().ToLowerInvariant()}"
            );
        }
        foreach (Form22 row in Rows22Cur)
        {
            if (string.IsNullOrWhiteSpace(row.StoragePlaceName_DB) || row.StoragePlaceName_DB.Trim() == "-") continue;
            Storages22.Add((
                $"{row.StoragePlaceName_DB} {row.StoragePlaceCode_DB}",
                $"{row.StoragePlaceName_DB.Trim().ToLowerInvariant()}{row.StoragePlaceCode_DB.Trim().ToLowerInvariant()}"
            ));
        }
        foreach (var storage22 in Storages22)
        {
            if (!Storages23.Contains(storage22.Item2))
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_23",
                    Row = "-",
                    Column = "-",
                    Value = "",
                    Message = $"Заполните сведения об имеющемся разрешительном документе на размещение РАО в {storage22.Item1}."
                });
            }
        }
    }

    #endregion
    #region Check_DocumentExpiryDate

    private static void Check_DocumentExpiryDate(List<CheckError> errorList, Report rep)
    {
        List<Form23> Rows23Cur = new(rep.Rows23);
        if (!int.TryParse(rep.Year_DB.Trim(), out int yearCurRaw)) return;
        DateTime YearEnd = new(yearCurRaw, 12, DateTime.DaysInMonth(yearCurRaw, 12));
        foreach (Form23 row in Rows23Cur)
        {
            if (string.IsNullOrWhiteSpace(row.ExpirationDate_DB)
                || row.ExpirationDate_DB.Trim() == "-"
                || !DateTime.TryParse(row.ExpirationDate_DB, out DateTime ExpirationDate)) continue;
            if (ExpirationDate < YearEnd)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_23",
                    Row = $"{row.NumberInOrder_DB}",
                    Column = GraphsList["ExpirationDate_DB"],
                    Value = row.ExpirationDate_DB,
                    Message = $"Срок действия документа закончился. Необходимо привести сведения об актуальном документе."
                });
            }
        }
    }

    #endregion
    #region Check_ProjectVolume

    private static void Check_ProjectVolume(List<CheckError> errorList, Report rep, List<Form23> Rows23Prev)
    {
        List<Form23> Rows23Cur = new(rep.Rows23);
        List<(string, double, string)> Storages23Prev = new();
        List<(string, double, string, int)> Storages23Cur = new();
        foreach (Form23 row in Rows23Cur)
        {
            if (string.IsNullOrWhiteSpace(row.StoragePlaceName_DB) || row.StoragePlaceName_DB.Trim() == "-") continue;
            double.TryParse(row.ProjectVolume_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var volume);
            Storages23Cur.Add((
                $"{row.StoragePlaceName_DB.Trim().ToLowerInvariant()}{row.StoragePlaceCode_DB.Trim().ToLowerInvariant()}",
                volume,
                row.ProjectVolume_DB,
                row.NumberInOrder_DB
            ));
        }
        foreach (Form23 row in Rows23Prev)
        {
            if (string.IsNullOrWhiteSpace(row.StoragePlaceName_DB) || row.StoragePlaceName_DB.Trim() == "-") continue;
            double.TryParse(row.ProjectVolume_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var volume);
            Storages23Prev.Add((
                $"{row.StoragePlaceName_DB.Trim().ToLowerInvariant()}{row.StoragePlaceCode_DB.Trim().ToLowerInvariant()}",
                volume,
                row.ProjectVolume_DB
            ));
        }
        foreach (var storage23Prev in Storages23Prev)
        {
            var storage23Cur = Storages23Cur.FirstOrDefault(x => x.Item1 == storage23Prev.Item1);
            if (storage23Cur == default) continue;
            if (storage23Prev.Item2 != storage23Cur.Item2)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_23",
                    Row = $"{storage23Cur.Item4}",
                    Column = GraphsList["ProjectVolume_DB"],
                    Value = $"{storage23Cur.Item3}",
                    Message = $"Сведения о проектном объеме не совпадают с данными за предыдущий год. Необходимо проверить данные."
                });
            }
        }
    }

    #endregion
    #region Check_RAOFrom22

    private static void Check_RAOFrom22(List<CheckError> errorList, Report rep, List<Form22> Rows22Cur)
    {
        List<Form23> Rows23Cur = new(rep.Rows23);
        List<(string, string)> Storages22 = new();
        List<string> Storages23 = new();
        foreach (Form23 row in Rows23Cur)
        {
            if (string.IsNullOrWhiteSpace(row.StoragePlaceName_DB) || row.StoragePlaceName_DB.Trim() == "-") continue;
            Storages23.Add(
                $"{row.StoragePlaceName_DB.Trim().ToLowerInvariant()}{row.StoragePlaceCode_DB.Trim().ToLowerInvariant()}"
            );
        }
        foreach (Form22 row in Rows22Cur)
        {
            if (string.IsNullOrWhiteSpace(row.StoragePlaceName_DB) || row.StoragePlaceName_DB.Trim() == "-") continue;
            Storages22.Add((
                $"{row.StoragePlaceName_DB} {row.StoragePlaceCode_DB}",
                $"{row.StoragePlaceName_DB.Trim().ToLowerInvariant()}{row.StoragePlaceCode_DB.Trim().ToLowerInvariant()}"
            ));
        }
        foreach (var storage22 in Storages22)
        {
            if (!Storages23.Contains(storage22.Item2))
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_23",
                    Row = "-",
                    Column = "-",
                    Value = "",
                    Message = $"Заполните сведения об имеющемся разрешительном документе на размещение РАО в {storage22.Item1}."
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
        { "StoragePlaceName_DB", "02 - Пункт хранения РАО - наименование" },
        { "StoragePlaceCode_DB", "03 - Пункт хранения РАО - код" },
        { "ProjectVolume_DB", "04 - Пункт хранения РАО - проектный объём, куб. м" },
        { "CodeRAO_DB", "05 - Разрешено к размещению - код РАО" },
        { "Volume_DB", "06 - Разрешено к размещению - количество РАО - объём, куб. м" },
        { "Mass_DB", "07 - Разрешено к размещению - количество РАО - масса, т" },
        { "QuantityOZIII_DB", "08 - Разрешено к размещению - количество ОЗИИИ, шт." },
        { "SummaryActivity_DB", "09 - Разрешено к размещению - суммарная активность, Бк." },
        { "DocumentNumber_DB", "10 - Наименование и реквизиты документа на размещение РАО - номер" },
        { "DocumentDate_DB", "11 - Наименование и реквизиты документа на размещение РАО - дата" },
        { "ExpirationDate_DB", "12 - Наименование и реквизиты документа на размещение РАО - срок действия" },
        { "DocumentName_DB", "13 - Наименование и реквизиты документа на размещение РАО - наименование документа" }
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