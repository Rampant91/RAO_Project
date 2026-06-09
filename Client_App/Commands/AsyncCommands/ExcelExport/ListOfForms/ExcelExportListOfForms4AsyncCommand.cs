using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using Models.DBRealization;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

/// <summary>
/// Excel -> Список форм 4.
/// </summary>
public class ExcelExportListOfForms4AsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список_форм_4";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка в .xlsx", ExportType);
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;

        progressBarVM.SetProgressBar(5, "Запрос периода");
        var (minYear, maxYear) = !isBackgroundCommand
            ? await InputDateRange(progressBar, cts)
            : (0, 9999);

        progressBarVM.SetProgressBar(8, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Подсчёт количества РИАЦ");
        await ReportsCountCheck(db, "4.0", progressBar, cts);

        progressBarVM.SetProgressBar(13, "Запрос пути сохранения", "Выгрузка в .xlsx", ExportType);
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = !isBackgroundCommand
            ? await ExcelGetFullPath(fileName, cts, progressBar)
            : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

        fullPath = ResolveUniqueFilePath(fullPath, isBackgroundCommand ? folderPath : null);

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        var worksheet = FillExcelHeaders(excelPackage);

        var orgsList = await GetReportsList(db, "4.0", progressBarVM, cts);

        var prepared = PrepareForm4Export(orgsList, minYear, maxYear);

        var rowCounts = await LoadForm4RowCounts(db, prepared.ReportIdsByForm, progressBarVM, cts);

        FillExcel(
            prepared.Orgs,
            rowCounts,
            worksheet,
            progressBarVM,
            ProgressRowCountsEndForm4,
            ProgressExcelFillEnd);

        progressBarVM.SetProgressBar(ProgressSaveStart, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar, isBackgroundCommand);

        progressBarVM.SetProgressBar(ProgressCleanup, "Очистка временных данных");
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
    /// Для каждого отчёта каждого РИАЦ заполняет количество строк в .xlsx.
    /// </summary>
    private static void FillExcel(
        List<FormListOrgExportData> orgsList,
        Dictionary<int, int> rowCounts,
        ExcelWorksheet worksheet,
        AnyTaskProgressBarVM progressBarVM,
        int progressExcelStart,
        int progressExcelEnd)
    {
        var totalRows = CountExportRows(orgsList);
        var excelFillRange = progressExcelEnd - progressExcelStart;
        double progressValue = progressExcelStart;
        var rowsWritten = 0;

        progressBarVM.SetProgressBar(progressExcelStart, "Запись в Excel");

        const int firstDataRow = 2;
        var row = firstDataRow;
        foreach (var org in orgsList.OrderBy(x => x.RegNo))
        {
            foreach (var rep in OrderForm4Reports(org.Reports))
            {
                rowCounts.TryGetValue(rep.ReportId, out var rowCount);
                worksheet.Cells[row, 1].Value = org.RegNo;
                worksheet.Cells[row, 2].Value = org.ShortJurLico;
                worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                worksheet.Cells[row, 4].Value = rep.Year_DB;
                worksheet.Cells[row, 5].Value = rep.CorrectionNumber_DB;
                worksheet.Cells[row, 6].Value = rowCount;
                row++;

                if (totalRows <= 0)
                    continue;

                progressValue += (double)excelFillRange / totalRows;
                rowsWritten++;
                if (rowsWritten % ProgressUpdateRowInterval == 0 || rowsWritten == totalRows)
                {
                    progressBarVM.SetProgressBar(
                        Math.Min(progressExcelEnd, (int)Math.Floor(progressValue)),
                        $"Запись в Excel: {org.RegNo}");
                }
            }
        }

        if (row > firstDataRow)
            ApplyAlternatingRowColors(worksheet, firstDataRow, row - 1);

        ApplyFormListExcelStyle(worksheet, HeaderRowHeightForm4);
        ApplyForm4ListColumnWidths(worksheet);
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков в .xlsx.
    /// </summary>
    private static ExcelWorksheet FillExcelHeaders(ExcelPackage excelPackage)
    {
        var worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 4");

        worksheet.Cells[1, 1].Value = "Код субъекта РФ";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование" + HeaderFilterLineBreak + "РИАЦ";
        worksheet.Cells[1, 3].Value = "Номер формы" + HeaderFilterLineBreak;
        worksheet.Cells[1, 4].Value = "Год";
        worksheet.Cells[1, 5].Value = "Номер корректировки" + HeaderFilterLineBreak;
        worksheet.Cells[1, 6].Value = "Количество строк" + HeaderFilterLineBreak;

        return worksheet;
    }

    #endregion

    #region LoadForm4RowCounts

    /// <summary>
    /// Загружает количество строк для отчётов выгрузки.
    /// </summary>
    private static async Task<Dictionary<int, int>> LoadForm4RowCounts(
        DBModel db,
        IReadOnlyDictionary<string, List<int>> reportIdsByForm,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var token = cts.Token;
        var result = new Dictionary<int, int>();
        var formCount = Form4ChildFormNumbers.Length;

        for (var formIndex = 0; formIndex < formCount; formIndex++)
        {
            var formNum = Form4ChildFormNumbers[formIndex];
            SetRowCountsProgress(
                progressBarVM,
                formIndex + 1,
                formCount,
                ProgressRowCountsEndForm4,
                formNum);

            if (!reportIdsByForm.TryGetValue(formNum, out var reportIds) || reportIds.Count == 0)
                continue;

            if (formNum == "4.1")
                await AppendFormRowCountsAsync(db.form_41, reportIds, result, token);
        }

        return result;
    }

    #endregion

    #region InputDateRange

    /// <summary>
    /// Запрос у пользователя периода, за который необходимо выполнить выборку.
    /// </summary>
    private static async Task<(int minYear, int maxYear)> InputDateRange(
        AnyTaskProgressBar? progressBar,
        CancellationTokenSource cts)
    {
        var res = await Dispatcher.UIThread.InvokeAsync(() =>
        {
            var window = new AskYearPeriodMessageWindow();
            return window.ShowDialog<(string command, int initialYear, int residualYear)>(Desktop.MainWindow);
        });

        if (res.command is not "Ок")
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        return (res.initialYear, res.residualYear);
    }

    #endregion
}
