using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Список форм 2.
/// </summary>
public class ExcelExportListOfForms2AsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список_форм_2";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения", "Выгрузка в .xlsx", ExportType);
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(7, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(11, "Подсчёт количества организаций");
        await ReportsCountCheck(db, progressBar, cts);

        progressBarVM.SetProgressBar(13, "Запрос периода");
        var (minYear, maxYear) = await InputDateRange(progressBar, cts);

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(20, "Получение списка организаций");
        var repsList = await GetReportsList(db, cts);

        progressBarVM.SetProgressBar(25, "Загрузка списка форм 2.1");
        var tuple21 = await GetTuple21(db, cts);

        progressBarVM.SetProgressBar(30, "Загрузка списка форм 2.2");
        var tuple22 = await GetTuple22(db, cts);

        progressBarVM.SetProgressBar(35, "Загрузка списка форм 2.3");
        var tuple23 = await GetTuple23(db, cts);

        progressBarVM.SetProgressBar(40, "Загрузка списка форм 2.4");
        var tuple24 = await GetTuple24(db, cts);

        progressBarVM.SetProgressBar(45, "Загрузка списка форм 2.5");
        var tuple25 = await GetTuple25(db, cts);

        progressBarVM.SetProgressBar(50, "Загрузка списка форм 2.6");
        var tuple26 = await GetTuple26(db, cts);

        progressBarVM.SetProgressBar(55, "Загрузка списка форм 2.7");
        var tuple27 = await GetTuple27(db, cts);

        progressBarVM.SetProgressBar(60, "Загрузка списка форм 2.8");
        var tuple28 = await GetTuple28(db, cts);

        progressBarVM.SetProgressBar(65, "Загрузка списка форм 2.9");
        var tuple29 = await GetTuple29(db, cts);

        progressBarVM.SetProgressBar(70, "Загрузка списка форм 2.10");
        var tuple210 = await GetTuple210(db, cts);

        progressBarVM.SetProgressBar(75, "Загрузка списка форм 2.11");
        var tuple211 = await GetTuple211(db, cts);

        progressBarVM.SetProgressBar(80, "Загрузка списка форм 2.12");
        var tuple212 = await GetTuple212(db, cts);

        progressBarVM.SetProgressBar(85, "Заполнение строк");
        await FillExcel(repsList, minYear, maxYear, tuple21, tuple22, tuple23, tuple24, tuple25, tuple26, tuple27, tuple28, tuple29, tuple210, tuple211, tuple212);

        progressBarVM.SetProgressBar(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

        progressBarVM.SetProgressBar(98, "Очистка временных данных");
        try
        {
            File.Delete(tmpDbPath);
        }
        catch
        {
            // ignored
        }

        progressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    #region FillExcel

    /// <summary>
    /// Для каждого отчёта каждой организации заполняет количество строк в .xlsx.
    /// </summary>
    /// <param name="repsList">Список организаций.</param>
    /// <param name="minYear">Минимальное значение года, задающее период выгрузки.</param>
    /// <param name="maxYear">Максимальное значение года, задающее период выгрузки.</param>
    /// <param name="tuple21">Кортеж из id и количества строчек для форм 2.1.</param>
    /// <param name="tuple22">Кортеж из id и количества строчек для форм 2.2.</param>
    /// <param name="tuple23">Кортеж из id и количества строчек для форм 2.3.</param>
    /// <param name="tuple24">Кортеж из id и количества строчек для форм 2.4.</param>
    /// <param name="tuple25">Кортеж из id и количества строчек для форм 2.5.</param>
    /// <param name="tuple26">Кортеж из id и количества строчек для форм 2.6.</param>
    /// <param name="tuple27">Кортеж из id и количества строчек для форм 2.7.</param>
    /// <param name="tuple28">Кортеж из id и количества строчек для форм 2.8.</param>
    /// <param name="tuple29">Кортеж из id и количества строчек для форм 2.9.</param>
    /// <param name="tuple210">Кортеж из id и количества строчек для форм 2.10.</param>
    /// <param name="tuple211">Кортеж из id и количества строчек для форм 2.11.</param>
    /// <param name="tuple212">Кортеж из id и количества строчек для форм 2.12.</param>
    private Task FillExcel(List<Reports> repsList, int minYear, int maxYear,
        List<Tuple<int, int>> tuple21, List<Tuple<int, int>> tuple22, List<Tuple<int, int>> tuple23, List<Tuple<int, int>> tuple24, 
        List<Tuple<int, int>> tuple25, List<Tuple<int, int>> tuple26, List<Tuple<int, int>> tuple27, List<Tuple<int, int>> tuple28, 
        List<Tuple<int, int>> tuple29, List<Tuple<int, int>> tuple210, List<Tuple<int, int>> tuple211, List<Tuple<int, int>> tuple212)
    {
        var row = 2;
        foreach (var reps in repsList
                     .OrderBy(x => x.Master_DB.RegNoRep.Value)
                     .ThenBy(x => x.Master_DB.OkpoRep.Value))
        {
            var repList = reps.Report_Collection
                .Where(x =>
                {
                    if (minYear == 0 && maxYear == 9999) return true;
                    if (x.Year_DB?.Length != 4 || !int.TryParse(x.Year_DB, out var currentRepsYear)) return false;
                    return currentRepsYear >= minYear && currentRepsYear <= maxYear;
                })
                .OrderBy(x => byte.TryParse(x.FormNum_DB[2..], out var formNum) ? formNum : byte.MaxValue)
                .ThenBy(x => x.Year_DB)
                .ThenBy(x => x.CorrectionNumber_DB)
                .ToList();
            foreach (var rep in repList)
            {
                var tupleList = rep.FormNum_DB switch
                {
                    "2.1" => tuple21,
                    "2.2" => tuple22,
                    "2.3" => tuple23,
                    "2.4" => tuple24,
                    "2.5" => tuple25,
                    "2.6" => tuple26,
                    "2.7" => tuple27,
                    "2.8" => tuple28,
                    "2.9" => tuple29,
                    "2.10" => tuple210,
                    "2.11" => tuple211,
                    "2.12" => tuple212,
                };
                var tuple = tupleList.Find(x => x.Item1 == rep.Id) ?? new Tuple<int, int>(rep.Id, 0);
                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                Worksheet.Cells[row, 4].Value = rep.Year_DB;
                Worksheet.Cells[row, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[row, 6].Value = tuple.Item2;
                row++;
            }
        }
        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns();  // Под Astra Linux эта команда крашит программу без GDI дров
        }
        Worksheet.View.FreezePanes(2, 1);
        return Task.CompletedTask;
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков в .xlsx.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    private Task FillExcelHeaders(ExcelPackage excelPackage)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 2");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Рег.№";
        Worksheet.Cells[1, 2].Value = "ОКПО";
        Worksheet.Cells[1, 3].Value = "Форма";
        Worksheet.Cells[1, 4].Value = "Отчетный год";
        Worksheet.Cells[1, 5].Value = "Номер кор.";
        Worksheet.Cells[1, 6].Value = "Количество строк";

        #endregion

        return Task.CompletedTask;
    }

    #endregion

    #region GetReportsList

    /// <summary>
    /// Получение списка организаций, с головным отчётом по форме 2.0.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список организаций, с головным отчётом по форме 2.0.</returns>
    private static async Task<List<Reports>> GetReportsList(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows20)
            .Include(reps => reps.Report_Collection)
            .Where(reps => reps.DBObservable != null && reps.Master_DB.FormNum_DB == "2.0")
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuples

    #region GetTuple21

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.1.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.1.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple21(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows21)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.1")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows21.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple22

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.2.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.2.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple22(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows22)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.2")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows22.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple23

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.3.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.3.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple23(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows23)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.3")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows23.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple24

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.4.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.4.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple24(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows24)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.4")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows24.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple25

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.5.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.5.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple25(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows25)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.5")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows25.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple26

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.6.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.6.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple26(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows26)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.6")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows26.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple27

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.7.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.7.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple27(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows27)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.7")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows27.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple28

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.8.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.8.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple28(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows28)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.8")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows28.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple29

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.9.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.9.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple29(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows29)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.9")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows29.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple210

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.10.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.10.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple210(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows210)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.10")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows210.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple211

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.11.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.11.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple211(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows211)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.11")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows211.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple212

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.12.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.12.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple212(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows212)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "2.12")
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows212.Count))
            .ToListAsync(cts.Token);
    }

    #endregion

    #endregion

    #region InputDateRange

    /// <summary>
    /// Запрос у пользователя периода, за который необходимо выполнить выборку.
    /// </summary>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из года начала периода и года его окончания.</returns>
    private static async Task<(int minYear, int maxYear)> InputDateRange(AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        #region MessageInputYearRange

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxInputWindow(new MessageBoxInputParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ],
                    ContentTitle = "Задать период",
                    ContentMessage = "Введите год или период лет через дефис (прим: 2022-2024)." +
                                     $"{Environment.NewLine}Если поле незаполненно или года введены некорректно," +
                                     $"{Environment.NewLine}то выгрузка будет осуществляться без фильтра по годам.",
                    MinWidth = 600,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

        #endregion

        if (res.Button is null or "Отмена")
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        var minYear = 0;
        var maxYear = 9999;
        if (res.Message != null)
        {
            if (!res.Message.Contains('-'))
            {
                if (int.TryParse(res.Message, out var parseYear) && parseYear.ToString().Length == 4)
                {
                    minYear = parseYear;
                    maxYear = parseYear;
                }
            }
            else if (res.Message.Length > 4)
            {
                var firstResHalf = res.Message.Split('-')[0].Trim();
                var secondResHalf = res.Message.Split('-')[1].Trim();
                if (int.TryParse(firstResHalf, out var minYearParse) && minYearParse.ToString().Length == 4)
                {
                    minYear = minYearParse;
                }
                if (int.TryParse(secondResHalf, out var maxYearParse) && maxYearParse.ToString().Length == 4)
                {
                    maxYear = maxYearParse;
                }
            }
        }
        return (minYear, maxYear);
    }

    #endregion

    #region ReportsCountCheck

    /// <summary>
    /// Подсчёт количества организаций. При количестве равном 0, выводится сообщение, операция завершается.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task ReportsCountCheck(DBModel db, AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        var countReports = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB)
            .Where(x => x.DBObservable != null && x.Master_DB.FormNum_DB == "2.0")
            .CountAsync(cts.Token);

        if (countReports == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка всех отчетов по форме 2 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствует отчетность по формам 2./",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion
}