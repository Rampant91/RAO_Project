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
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Список форм 1.
/// </summary>
public class ExcelExportListOfForms1AsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список_форм_1";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка в .xlsx", ExportType);
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;

        progressBarVM.SetProgressBar(5, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(9, "Подсчёт количества организаций");
        await ReportsCountCheck(db, progressBar, cts);

        progressBarVM.SetProgressBar(11, "Запрос пути сохранения", "Выгрузка в .xlsx", ExportType);
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = !isBackgroundCommand
            ? await ExcelGetFullPath(fileName, cts, progressBar)
            : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

        var count = 0;
        while (File.Exists(fullPath))
        {
            fullPath = Path.Combine(folderPath, fileName + $"_{++count}.xlsx");
        }

        progressBarVM.SetProgressBar(13, "Запрос периода");
        var (startDate, endDate) = !isBackgroundCommand
            ? await InputDateRange(progressBar, cts)
            : (DateOnly.MinValue, DateOnly.MaxValue);

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(20, "Получение списка организаций");
        var repsList = await GetReportsList(db, cts);

        progressBarVM.SetProgressBar(25, "Загрузка списка форм 1.1");
        var tuple11 = await GetTuple11(db, cts);

        progressBarVM.SetProgressBar(32, "Загрузка списка форм 1.2");
        var tuple12 = await GetTuple12(db, cts);

        progressBarVM.SetProgressBar(39, "Загрузка списка форм 1.3");
        var tuple13 = await GetTuple13(db, cts);

        progressBarVM.SetProgressBar(46, "Загрузка списка форм 1.4");
        var tuple14 = await GetTuple14(db, cts);

        progressBarVM.SetProgressBar(53, "Загрузка списка форм 1.5");
        var tuple15 = await GetTuple15(db, cts);

        progressBarVM.SetProgressBar(60, "Загрузка списка форм 1.6");
        var tuple16 = await GetTuple16(db, cts);

        progressBarVM.SetProgressBar(67, "Загрузка списка форм 1.7");
        var tuple17 = await GetTuple17(db, cts);

        progressBarVM.SetProgressBar(74, "Загрузка списка форм 1.8");
        var tuple18 = await GetTuple18(db, cts);

        progressBarVM.SetProgressBar(81, "Загрузка списка форм 1.9");
        var tuple19 = await GetTuple19(db, cts);

        progressBarVM.SetProgressBar(90, "Заполнение строк");
        await FillExcel(repsList, startDate, endDate, tuple11, tuple12, tuple13, tuple14, tuple15, tuple16, tuple17, tuple18, tuple19);

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
    /// <param name="startDate">Дата начала периода.</param>
    /// <param name="endDate">Дата окончания периода.</param>
    /// <param name="tuple11">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.1.</param>
    /// <param name="tuple12">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.2.</param>
    /// <param name="tuple13">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.3.</param>
    /// <param name="tuple14">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.4.</param>
    /// <param name="tuple15">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.5.</param>
    /// <param name="tuple16">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.6.</param>
    /// <param name="tuple17">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.7.</param>
    /// <param name="tuple18">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.8.</param>
    /// <param name="tuple19">Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.9.</param>
    private Task FillExcel(List<Reports> repsList, DateOnly startDate, DateOnly endDate,
    List<Tuple<int, int, int>> tuple11, List<Tuple<int, int, int>> tuple12, List<Tuple<int, int, int>> tuple13,
    List<Tuple<int, int, int>> tuple14, List<Tuple<int, int, int>> tuple15, List<Tuple<int, int, int>> tuple16,
    List<Tuple<int, int, int>> tuple17, List<Tuple<int, int, int>> tuple18, List<Tuple<int, int, int>> tuple19)
    {
        var row = 2;
        foreach (var reps in repsList
                     .OrderBy(x => x.Master_DB.RegNoRep.Value)
                     .ThenBy(x => x.Master_DB.OkpoRep.Value))
        {
            var repList = reps.Report_Collection
                .Where(x =>
                {
                    if (startDate == DateOnly.MinValue && endDate == DateOnly.MaxValue) return true;
                    if (!DateOnly.TryParse(x.EndPeriod_DB, out var repEndDateTime)) return false;
                    return repEndDateTime >= startDate && repEndDateTime <= endDate;
                })
                .OrderBy(x => x.FormNum_DB)
                .ThenBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDateOnly) ? stDateOnly : DateOnly.MaxValue)
                .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDateOnly) ? endDateOnly : DateOnly.MaxValue)
                .ThenBy(x => x.CorrectionNumber_DB)
                .ToList();
            foreach (var rep in repList)
            {
                var tupleList = rep.FormNum_DB switch
                {
                    "1.1" => tuple11,
                    "1.2" => tuple12,
                    "1.3" => tuple13,
                    "1.4" => tuple14,
                    "1.5" => tuple15,
                    "1.6" => tuple16,
                    "1.7" => tuple17,
                    "1.8" => tuple18,
                    "1.9" => tuple19
                };
                var tuple = tupleList.Find(x => x.Item1 == rep.Id) ?? new Tuple<int, int, int>(rep.Id, 0, 0);
                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                Worksheet.Cells[row, 4].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, row, 4);
                Worksheet.Cells[row, 5].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, row, 5);
                Worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[row, 7].Value = tuple.Item2;
                Worksheet.Cells[row, 8].Value = InventoryCheck(repRowsCount: tuple.Item2, countCode10: tuple.Item3).TrimStart();
                row++;
            }
        }
        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns(); // Под Astra Linux эта команда крашит программу без GDI дров
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
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 1");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Рег.№";
        Worksheet.Cells[1, 2].Value = "ОКПО";
        Worksheet.Cells[1, 3].Value = "Форма";
        Worksheet.Cells[1, 4].Value = "Дата начала";
        Worksheet.Cells[1, 5].Value = "Дата конца";
        Worksheet.Cells[1, 6].Value = "Номер кор.";
        Worksheet.Cells[1, 7].Value = "Количество строк";
        Worksheet.Cells[1, 8].Value = "Инвентаризация";

        #endregion

        return Task.CompletedTask;
    }

    #endregion

    #region GetTuples

    #region GetTuple11

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.1.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.1.</returns>
    private static async Task<List<Tuple<int,int,int>>> GetTuple11(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows11)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.1")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows11.Count,
                rep.Rows11.Count(form11 => form11.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple12

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.2.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.2.</returns>
    private static async Task<List<Tuple<int, int, int>>> GetTuple12(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows12)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.2")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows12.Count,
                rep.Rows12.Count(form12 => form12.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple13

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.3.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.3.</returns>
    private static async Task<List<Tuple<int, int, int>>> GetTuple13(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows13)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.3")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows13.Count,
                rep.Rows13.Count(form13 => form13.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple14

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.4.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.4.</returns>
    private static async Task<List<Tuple<int, int, int>>> GetTuple14(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows14)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.4")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows14.Count,
                rep.Rows14.Count(form14 => form14.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple15

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.5.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.5.</returns>
    private static async Task<List<Tuple<int, int, int>>> GetTuple15(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows15)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.5")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows15.Count,
                rep.Rows15.Count(form15 => form15.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple16

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.6.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.6.</returns>
    private static async Task<List<Tuple<int, int, int>>> GetTuple16(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows16)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.6")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows16.Count,
                rep.Rows16.Count(form16 => form16.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple17

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.7.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.7.</returns>
    private static async Task<List<Tuple<int, int, int>>> GetTuple17(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows17)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.7")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows17.Count,
                rep.Rows17.Count(form17 => form17.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple18

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.8.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.8.</returns>
    private static async Task<List<Tuple<int, int, int>>> GetTuple18(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows18)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.8")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows18.Count,
                rep.Rows18.Count(form18 => form18.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetTuple19

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.9.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.9.</returns>
    private static async Task<List<Tuple<int, int, int>>> GetTuple19(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Rows19)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.9")
            .Select(rep => new Tuple<int, int, int>(
                rep.Id,
                rep.Rows19.Count,
                rep.Rows19.Count(form19 => form19.OperationCode_DB == "10")))
            .ToListAsync(cts.Token);
    }

    #endregion

    #endregion

    #region GetReportsList

    /// <summary>
    /// Получение списка организаций, с головным отчётом по форме 1.0.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список организаций, с головным отчётом по форме 1.0.</returns>
    private static async Task<List<Reports>> GetReportsList(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
            .Include(reps => reps.Report_Collection)
            .Where(reps => reps.DBObservable != null && reps.Master_DB.FormNum_DB == "1.0")
            .ToListAsync(cts.Token);
    }

    #endregion

    #region InputDateRange

    /// <summary>
    /// Запрос у пользователя периода, за который необходимо выполнить выборку.
    /// </summary>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из даты начала периода и даты его окончания.</returns>
    private static async Task<(DateOnly startDateOnly, DateOnly endDateOnly)> InputDateRange(AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        #region MessageInputDateRange

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxInputWindow(new MessageBoxInputParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ],
                    ContentTitle = "Задать период",
                    ContentMessage = "Введите период дат через дефис (прим: 01.01.2022-07.03.2023)." +
                                     $"{Environment.NewLine}Если даты незаполненны или введены некорректно," +
                                     $"{Environment.NewLine}то выгрузка будет осуществляться без фильтра по датам.",
                    MinWidth = 600,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

        #endregion

        if (res.Button is null or "Отмена")
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        var startDate = DateOnly.MinValue;
        var endDate = DateOnly.MaxValue;
        if (res.Message != null && res.Message.Contains('-') && res.Message.Length > 6)
        {
            var firstPeriodHalf = res.Message.Split('-')[0].Trim();
            var secondPeriodHalf = res.Message.Split('-')[1].Trim();
            if (DateOnly.TryParse(firstPeriodHalf, out var parseStartDate))
            {
                startDate = parseStartDate;
            }
            if (DateOnly.TryParse(secondPeriodHalf, out var parseEndDate))
            {
                endDate = parseEndDate;
            }
        }
        return (startDate, endDate);
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
            .Where(x => x.DBObservable != null && x.Master_DB.FormNum_DB == "1.0")
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
                        "Не удалось совершить выгрузку списка всех отчетов по форме 1 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствует отчетность по формам 1./",
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