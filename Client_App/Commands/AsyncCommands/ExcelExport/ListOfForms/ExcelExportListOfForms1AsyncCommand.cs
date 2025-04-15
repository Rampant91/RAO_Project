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
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

/// <summary>
/// Excel -> Список форм 1.
/// </summary>
public class ExcelExportListOfForms1AsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
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
        await ReportsCountCheck(db, "1.0", progressBar, cts);

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
            : (DateOnly.MinValue, DateOnly.MaxValue);   //default value

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        var worksheet = await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(20, "Получение списка организаций");
        var repsList = await GetReportsList(db, "1.0", cts);

        progressBarVM.SetProgressBar(23, "Загрузка списков форм");
        List<List<Tuple<int, int, int>>> tuplesList =
        [
            await GetTuple(db, "1.1", progressBarVM, cts),
            await GetTuple(db, "1.2", progressBarVM, cts),
            await GetTuple(db, "1.3", progressBarVM, cts),
            await GetTuple(db, "1.4", progressBarVM, cts),
            await GetTuple(db, "1.5", progressBarVM, cts),
            await GetTuple(db, "1.6", progressBarVM, cts),
            await GetTuple(db, "1.7", progressBarVM, cts),
            await GetTuple(db, "1.8", progressBarVM, cts),
            await GetTuple(db, "1.9", progressBarVM, cts),
        ];

        progressBarVM.SetProgressBar(90, "Заполнение строк");
        await FillExcel(repsList, startDate, endDate, tuplesList, worksheet);

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
    /// <param name="tupleLists">Список списков кортежей.</param>
    /// <param name="worksheet">Excel страница.</param>
    private static async Task FillExcel(List<Reports> repsList, DateOnly startDate, DateOnly endDate, List<List<Tuple<int, int, int>>> tupleLists, 
        ExcelWorksheet worksheet)
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
                    "1.1" => tupleLists[0],
                    "1.2" => tupleLists[1],
                    "1.3" => tupleLists[2],
                    "1.4" => tupleLists[3],
                    "1.5" => tupleLists[4],
                    "1.6" => tupleLists[5],
                    "1.7" => tupleLists[6],
                    "1.8" => tupleLists[7],
                    "1.9" => tupleLists[8],
                    _ => throw new ArgumentOutOfRangeException()
                };
                var tuple = tupleList.Find(x => x.Item1 == rep.Id) ?? new Tuple<int, int, int>(rep.Id, 0, 0);
                worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                worksheet.Cells[row, 3].Value = reps.Master.ShortJurLicoRep.Value;
                worksheet.Cells[row, 4].Value = rep.FormNum_DB;
                worksheet.Cells[row, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, worksheet, row, 5);
                worksheet.Cells[row, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, worksheet, row, 6);
                worksheet.Cells[row, 7].Value = rep.CorrectionNumber_DB;
                worksheet.Cells[row, 8].Value = tuple.Item2;
                worksheet.Cells[row, 9].Value = InventoryCheck(repRowsCount: tuple.Item2, countCode10: tuple.Item3).TrimStart();
                row++;
            }
        }
        await AutoFitColumns(worksheet);
    }

    #region AutoFitColumns

    /// <summary>
    /// Для текущей страницы Excel пакета подбирает ширину колонок и замораживает первую строчку.
    /// </summary>
    private static Task AutoFitColumns(ExcelWorksheet worksheet)
    {
        for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            if (OperatingSystem.IsWindows() && col != 3) worksheet.Column(col).AutoFit();
        }
        worksheet.View.FreezePanes(2, 1);

        return Task.CompletedTask;
    }

    #endregion

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков в .xlsx.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <returns>Excel страница.</returns>
    private static Task<ExcelWorksheet> FillExcelHeaders(ExcelPackage excelPackage)
    {
        var worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 1");

        #region Headers

        worksheet.Cells[1, 1].Value = "Рег №";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала";
        worksheet.Cells[1, 6].Value = "Дата конца";
        worksheet.Cells[1, 7].Value = "Номер кор";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "Инвентаризация";

        #endregion

        worksheet.Column(3).AutoFit();

        return Task.FromResult(worksheet);
    }

    #endregion

    #region GetTuple

    /// <summary>
    /// Получение кортежа из id, количества строчек и количества операций с кодом 10 для форм 1.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id, количества строчек и количества операций с кодом 10 для форм 1.</returns>
    private protected static async Task<List<Tuple<int, int, int>>> GetTuple(DBModel db, string formNum, 
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        var progressBarValue = progressBarVM.ValueBar;
        progressBarVM.SetProgressBar(progressBarValue + 7, $"Загрузка списка форм {formNum}");

        return formNum switch
        {
            "1.1" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows11)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows11.Count,
                    rep.Rows11.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            "1.2" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows12)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows12.Count,
                    rep.Rows12.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            "1.3" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows13)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows13.Count,
                    rep.Rows13.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            "1.4" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows14)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows14.Count,
                    rep.Rows14.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            "1.5" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows15)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows15.Count,
                    rep.Rows15.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            "1.6" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows16)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows16.Count,
                    rep.Rows16.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            "1.7" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows17)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows17.Count,
                    rep.Rows17.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            "1.8" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows18)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows18.Count,
                    rep.Rows18.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            "1.9" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows19)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int, int>(
                    rep.Id,
                    rep.Rows19.Count,
                    rep.Rows19.Count(form => form.OperationCode_DB == "10")))
                .ToListAsync(cts.Token),

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };
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
}