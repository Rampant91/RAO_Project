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
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Аналитика -> Разрывы и пересечения.
/// </summary>
public class ExcelExportIntersectionsAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Разрывы_и_пересечения";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка в .xlsx", ExportType);
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = !isBackgroundCommand
            ? await ExcelGetFullPath(fileName, cts, progressBar)
            : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

        progressBarVM.SetProgressBar(7, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(12, "Подсчёт количества организаций");
        await ReportsCountCheck(db, progressBar, cts);

        var count = 0;
        while (File.Exists(fullPath))
        {
            fullPath = Path.Combine(folderPath, fileName + $"_{++count}.xlsx");
        }

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(20, "Получение списка отчётов");
        var listSortRep = await GetSortedRepList(db, cts);

        progressBarVM.SetProgressBar(35, "Поиск пересечений");
        await GetListToCompareForEachRepAndCompareAndFillExcel(listSortRep, progressBarVM);

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

    #region CompareReportAndFillExcel

    /// <summary>
    /// Для каждого DTO отчёта из списка, сравнивает отчёт со всеми последующими, находит совпадения/пересечения/разрывы и заполняет .xlsx.
    /// </summary>
    /// <param name="listToCompare">Список DTO отчётов для сравнения.</param>
    /// <param name="repStart">Дата начала периода.</param>
    /// <param name="repEnd">Дата окончания периода.</param>
    /// <param name="repStartOriginal">Дата начала периода в отчёте для сравнения.</param>
    /// <param name="row">Номер ряда.</param>
    /// <returns>Номер ряда.</returns>
    private Task<int> CompareReportAndFillExcel(List<ReportForSort> listToCompare, DateOnly repStart, DateOnly repEnd, DateOnly repStartOriginal, int row)
    {
        var isNext = true;
        var order13Date = new DateOnly(2022, 1, 1);
        foreach (var repToCompare in listToCompare)
        {
            var repToCompareStart = repToCompare.StartPeriod;
            var repToCompareEnd = repToCompare.EndPeriod;
            var repToCompareStartOriginal = new DateOnly();
            if (repToCompareStart < order13Date && repToCompareEnd < order13Date)
            {
                repToCompareStartOriginal = repToCompareStart;
                repToCompareStart = repToCompareStart.AddDays(-1);
            }
            var minEndDate = repEnd < repToCompareEnd
                ? repEnd
                : repToCompareEnd;
            if ((repStart == repToCompareStart && repEnd == repToCompareEnd
                 || repStart < repToCompareEnd && repEnd > repToCompareStart
                 || isNext && repEnd < repToCompareStart)
                && !(repToCompareStart == order13Date && repEnd.AddDays(1) == repToCompareStart))
            {
                var repStartToExcel = repStartOriginal == new DateOnly() || repStartOriginal == repStart
                    ? repStart
                    : repStartOriginal;
                var repToCompareStartToExcel = repToCompareStartOriginal == new DateOnly() || repToCompareStartOriginal == repToCompareStart
                    ? repToCompareStart
                    : repToCompareStartOriginal;

                #region FillExcel

                Worksheet.Cells[row, 1].Value = repToCompare.RegNoRep;
                Worksheet.Cells[row, 2].Value = repToCompare.OkpoRep;
                Worksheet.Cells[row, 3].Value = repToCompare.ShortYr;
                Worksheet.Cells[row, 4].Value = repToCompare.FormNum;
                Worksheet.Cells[row, 5].Value = ConvertToExcelDate(repStartToExcel.ToShortDateString(), Worksheet, row, 5);
                Worksheet.Cells[row, 6].Value = ConvertToExcelDate(repEnd.ToShortDateString(), Worksheet, row, 6);
                Worksheet.Cells[row, 7].Value = ConvertToExcelDate(repToCompareStartToExcel.ToShortDateString(), Worksheet, row, 7);
                Worksheet.Cells[row, 8].Value = ConvertToExcelDate(repToCompareEnd.ToShortDateString(), Worksheet, row, 8);
                if (repStart == repToCompareStart && repEnd == repToCompareEnd)
                {
                    Worksheet.Cells[row, 9].Value = $"{repStartToExcel.ToShortDateString()}-{repEnd.ToShortDateString()}";
                    Worksheet.Cells[row, 10].Value = "совпадение";
                }
                else if (repStart < repToCompareEnd && repEnd > repToCompareStart)
                {
                    Worksheet.Cells[row, 9].Value = $"{repToCompareStartToExcel.ToShortDateString()}-{minEndDate.ToShortDateString()}";
                    Worksheet.Cells[row, 10].Value = "пересечение";
                }
                else if (isNext && repEnd < repToCompareStart)
                {
                    Worksheet.Cells[row, 9].Value = $"{repEnd.ToShortDateString()}-{repToCompareStartToExcel.ToShortDateString()}";
                    Worksheet.Cells[row, 10].Value = "разрыв";
                }

                #endregion

                row++;
            }
            isNext = false;
        }
        return Task.FromResult(row);
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполняет заголовки Excel пакета.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    private Task FillExcelHeaders(ExcelPackage excelPackage)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add("Разрывы и пересечения");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Рег.№";
        Worksheet.Cells[1, 2].Value = "ОКПО";
        Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        Worksheet.Cells[1, 4].Value = "Форма";
        Worksheet.Cells[1, 5].Value = "Начало прошлого периода";
        Worksheet.Cells[1, 6].Value = "Конец прошлого периода";
        Worksheet.Cells[1, 7].Value = "Начало периода";
        Worksheet.Cells[1, 8].Value = "Конец периода";
        Worksheet.Cells[1, 9].Value = "Зона разрыва";
        Worksheet.Cells[1, 10].Value = "Вид несоответствия";

        #endregion

        if (OperatingSystem.IsWindows()) Worksheet.Column(3).AutoFit();   // Под Astra Linux эта команда крашит программу без GDI дров

        for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
        {
            if (Worksheet.Cells[1, col].Value is "Сокращенное наименование") continue;

            if (OperatingSystem.IsWindows()) Worksheet.Column(col).AutoFit();
        }
        Worksheet.View.FreezePanes(2, 1);

        return Task.CompletedTask;
    }

    #endregion

    #region GetCompareListForEachRepAndCompareAndFillExcel

    /// <summary>
    /// Для каждого DTO отчёта из списка, получает список отчётов на сравнение, проверяет на совпадения/пересечения/разрывы и заполняет .xlsx.
    /// </summary>
    /// <param name="listSortRep">Список DTO отчётов.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    private async Task GetListToCompareForEachRepAndCompareAndFillExcel(List<ReportForSort> listSortRep, AnyTaskProgressBarVM progressBarVM)
    {
        var row = 2;
        var repIndex = 0;
        var order13Date = new DateOnly(2022, 1, 1);
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var rep in listSortRep)
        {

            var repStart = rep.StartPeriod;
            var repEnd = rep.EndPeriod;
            var repStartOriginal = new DateOnly();
            if (repStart < order13Date && repEnd < order13Date)
            {
                repStartOriginal = repStart;
                repStart = repStart.AddDays(-1);
            }
            var listToCompare = listSortRep
                .Skip(repIndex + 1)
                .Where(x => x.RegNoRep == rep.RegNoRep
                            && x.OkpoRep == rep.OkpoRep
                            && x.FormNum == rep.FormNum)
                .ToList();

            row = await CompareReportAndFillExcel(listToCompare, repStart, repEnd, repStartOriginal, row);

            progressBarDoubleValue += ((double)60 / listSortRep.Count);
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue), "Поиск пересечений");
            repIndex++;
        }
    }

    #endregion

    #region GetSortedRepList

    /// <summary>
    /// Получение отсортированного списка DTO отчётов.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отсортированный список DTO отчётов.</returns>
    private static async Task<List<ReportForSort>> GetSortedRepList(DBModel db, CancellationTokenSource cts)
    {
        var listSortRep = await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Reports.Master_DB.FormNum_DB == "1.0")
            .Select(rep => new ReportForSortDTO
            (
                rep.Reports.Master_DB.RegNoRep.Value,
                rep.Reports.Master_DB.OkpoRep.Value,
                rep.FormNum_DB,
                rep.StartPeriod_DB,
                rep.EndPeriod_DB,
                rep.Reports.Master_DB.ShortJurLicoRep.Value))
            .ToListAsync(cts.Token);

        return listSortRep
            .Where(rep => DateOnly.TryParse(rep.StartPeriod, out _) 
                                        && DateOnly.TryParse(rep.EndPeriod, out _))
            .Select(rep =>
            {
                var start = DateOnly.Parse(rep.StartPeriod);
                var end = DateOnly.Parse(rep.EndPeriod);
                return new ReportForSort
                {
                    RegNoRep = rep.RegNoRep,
                    OkpoRep = rep.OkpoRep,
                    FormNum = rep.FormNum,
                    StartPeriod = start,
                    EndPeriod = end,
                    ShortYr = rep.ShortYr,
                };
            })
            .OrderBy(x => x.RegNoRep)
            .ThenBy(x => x.FormNum)
            .ThenBy(x => x.StartPeriod)
            .ThenBy(x => x.EndPeriod)
            .ToList();
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
                        "Не удалось совершить выгрузку списка разрывов и пересечений дат," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствуют отчеты по форме 1.",
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

    #region ReportForSortDTO

    /// <summary>
    /// DTO отчёта.
    /// </summary>
    /// <param name="regNoRep">Рег.№.</param>
    /// <param name="okpoRep">ОКПО.</param>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="startPeriod">Начало периода.</param>
    /// <param name="endPeriod">Конец периода.</param>
    /// <param name="shortYr">Сокращённое наименование.</param>
    private class ReportForSortDTO(string regNoRep, string okpoRep, string formNum, string startPeriod, string endPeriod, string shortYr)
    {
        public readonly string RegNoRep = regNoRep;

        public readonly string OkpoRep = okpoRep;

        public readonly string FormNum = formNum;

        public readonly string StartPeriod = startPeriod;

        public readonly string EndPeriod = endPeriod;

        public readonly string ShortYr = shortYr;
    }

    #endregion
}