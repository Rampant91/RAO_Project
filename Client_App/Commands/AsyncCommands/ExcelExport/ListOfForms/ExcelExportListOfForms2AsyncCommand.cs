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

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

/// <summary>
/// Excel -> Список форм 2.
/// </summary>
public class ExcelExportListOfForms2AsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список_форм_2";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка в .xlsx", ExportType);
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;

        progressBarVM.SetProgressBar(5, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(9, "Подсчёт количества организаций");
        await ReportsCountCheck(db, "2.0", progressBar, cts);

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
        var (minYear, maxYear) = !isBackgroundCommand
            ? await InputDateRange(progressBar, cts)
            : (0, 9999);    //default value

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        var worksheet = await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(20, "Получение списка организаций");
        var repsList = await GetReportsList(db, "2.0", cts);

        progressBarVM.SetProgressBar(23, "Загрузка списков форм");
        List<List<Tuple<int, int>>> tuplesList =
        [
            await GetTuple(db, "2.1", progressBarVM, cts),
            await GetTuple(db, "2.2", progressBarVM, cts),
            await GetTuple(db, "2.3", progressBarVM, cts),
            await GetTuple(db, "2.4", progressBarVM, cts),
            await GetTuple(db, "2.5", progressBarVM, cts),
            await GetTuple(db, "2.6", progressBarVM, cts),
            await GetTuple(db, "2.7", progressBarVM, cts),
            await GetTuple(db, "2.8", progressBarVM, cts),
            await GetTuple(db, "2.9", progressBarVM, cts),
            await GetTuple(db, "2.10", progressBarVM, cts),
            await GetTuple(db, "2.11", progressBarVM, cts),
            await GetTuple(db, "2.12", progressBarVM, cts)
        ];

        progressBarVM.SetProgressBar(85, "Заполнение строк");
        await FillExcel(repsList, minYear, maxYear, tuplesList, worksheet);

        progressBarVM.SetProgressBar(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar, isBackgroundCommand);

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
    /// <param name="tupleLists">Список списков кортежей.</param>
    /// <param name="worksheet">Excel страница.</param>
    /// /// <returns>CompletedTask.</returns>
    private static async Task FillExcel(List<Reports> repsList, int minYear, int maxYear, List<List<Tuple<int, int>>> tupleLists,
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
                    "2.1" => tupleLists[0],
                    "2.2" => tupleLists[1],
                    "2.3" => tupleLists[2],
                    "2.4" => tupleLists[3],
                    "2.5" => tupleLists[4],
                    "2.6" => tupleLists[5],
                    "2.7" => tupleLists[6],
                    "2.8" => tupleLists[7],
                    "2.9" => tupleLists[8],
                    "2.10" => tupleLists[9],
                    "2.11" => tupleLists[10],
                    "2.12" => tupleLists[11],
                    _ => throw new ArgumentOutOfRangeException()
                };
                var tuple = tupleList.Find(x => x.Item1 == rep.Id) ?? new Tuple<int, int>(rep.Id, 0);
                worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                worksheet.Cells[row, 3].Value = reps.Master.ShortJurLicoRep.Value;
                worksheet.Cells[row, 4].Value = rep.FormNum_DB;
                worksheet.Cells[row, 5].Value = rep.Year_DB;
                worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
                worksheet.Cells[row, 7].Value = tuple.Item2;
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
    /// <returns>Excel лист.</returns>
    private static Task<ExcelWorksheet> FillExcelHeaders(ExcelPackage excelPackage)
    {
        var worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 2");

        #region Headers

        worksheet.Cells[1, 1].Value = "Рег №";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Отчетный год";
        worksheet.Cells[1, 6].Value = "Номер кор";
        worksheet.Cells[1, 7].Value = "Количество строк";

        #endregion

        worksheet.Column(3).AutoFit();

        return Task.FromResult(worksheet);
    }

    #endregion

    #region GetTuple

    /// <summary>
    /// Получение кортежа из id и количества строчек для форм 2.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из id и количества строчек для форм 2.</returns>
    private static async Task<List<Tuple<int, int>>> GetTuple(DBModel db, string formNum, 
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        var progressBarValue = progressBarVM.ValueBar;
        progressBarVM.SetProgressBar(progressBarValue + 5, $"Загрузка списка форм {formNum}");

        return formNum switch
        {
            "2.1" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows21)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows21.Count))
                .ToListAsync(cts.Token),

            "2.2" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows22)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows22.Count))
                .ToListAsync(cts.Token),

            "2.3" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows23)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows23.Count))
                .ToListAsync(cts.Token),

            "2.4" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows24)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows24.Count))
                .ToListAsync(cts.Token),

            "2.5" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows25)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows25.Count))
                .ToListAsync(cts.Token),

            "2.6" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows26)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows26.Count))
                .ToListAsync(cts.Token),

            "2.7" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows27)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows27.Count))
                .ToListAsync(cts.Token),

            "2.8" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows28)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows28.Count))
                .ToListAsync(cts.Token),

            "2.9" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows29)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows29.Count))
                .ToListAsync(cts.Token),

            "2.10" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows210)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows210.Count))
                .ToListAsync(cts.Token),

            "2.11" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows211)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows211.Count))
                .ToListAsync(cts.Token),

            "2.12" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows212)
                .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == formNum)
                .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows212.Count))
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
}