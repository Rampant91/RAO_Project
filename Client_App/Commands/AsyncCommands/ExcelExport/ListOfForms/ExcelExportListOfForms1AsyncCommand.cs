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

        fullPath = ResolveUniqueFilePath(fullPath, isBackgroundCommand ? folderPath : null);

        progressBarVM.SetProgressBar(13, "Запрос периода");

        var (startDate, endDate) = !isBackgroundCommand
            ? await InputDateRange(progressBar, cts)
            : (DateOnly.MinValue, DateOnly.MaxValue);

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        var worksheet = FillExcelHeaders(excelPackage);

        var orgsList = await GetReportsList(db, "1.0", progressBarVM, cts);

        var prepared = PrepareForm1Export(orgsList, startDate, endDate);

        var rowCounts = await LoadForm1RowCounts(db, prepared.ReportIdsByForm, progressBarVM, cts);

        FillExcel(
            prepared.Orgs,
            rowCounts,
            worksheet,
            progressBarVM,
            ProgressRowCountsEndForm1,
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
    /// Для каждого отчёта каждой организации заполняет количество строк в .xlsx.
    /// </summary>
    private static void FillExcel(
        List<FormListOrgExportData> orgsList,
        Dictionary<int, (int RowCount, int Code10Count)> rowCounts,
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
        foreach (var org in orgsList
                     .OrderBy(x => x.RegNo)
                     .ThenBy(x => x.Okpo))
        {
            foreach (var rep in OrderForm1Reports(org.Reports))
            {
                rowCounts.TryGetValue(rep.ReportId, out var counts);
                worksheet.Cells[row, 1].Value = org.RegNo;
                worksheet.Cells[row, 2].Value = org.Okpo;
                worksheet.Cells[row, 3].Value = org.ShortJurLico;
                worksheet.Cells[row, 4].Value = rep.FormNum_DB;
                worksheet.Cells[row, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, worksheet, row, 5);
                worksheet.Cells[row, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, worksheet, row, 6);
                worksheet.Cells[row, 7].Value = rep.CorrectionNumber_DB;
                worksheet.Cells[row, 8].Value = counts.RowCount;
                worksheet.Cells[row, 9].Value = InventoryCheck(repRowsCount: counts.RowCount, countCode10: counts.Code10Count).TrimStart();
                row++;

                if (totalRows <= 0)
                    continue;

                progressValue += (double)excelFillRange / totalRows;
                rowsWritten++;
                if (rowsWritten % ProgressUpdateRowInterval == 0 || rowsWritten == totalRows)
                {
                    progressBarVM.SetProgressBar(
                        Math.Min(progressExcelEnd, (int)Math.Floor(progressValue)),
                        $"Запись в Excel: {org.RegNo}_{org.Okpo}");
                }
            }
        }

        if (row > firstDataRow)
            ApplyAlternatingRowColors(worksheet, firstDataRow, row - 1);

        ApplyFormListExcelStyle(worksheet, HeaderRowHeightForm1);
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков в .xlsx.
    /// </summary>
    private static ExcelWorksheet FillExcelHeaders(ExcelPackage excelPackage)
    {
        var worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 1");

        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода" + HeaderFilterLineBreak;
        worksheet.Cells[1, 6].Value = "Дата конца периода" + HeaderFilterLineBreak;
        worksheet.Cells[1, 7].Value = "Номер корректировки" + HeaderFilterLineBreak;
        worksheet.Cells[1, 8].Value = "Количество строк" + HeaderFilterLineBreak;
        worksheet.Cells[1, 9].Value = "Инвентаризация";

        return worksheet;
    }

    #endregion

    #region LoadForm1RowCounts

    /// <summary>
    /// Загружает количество строк и операций с кодом 10 для отчётов выгрузки.
    /// </summary>
    private static async Task<Dictionary<int, (int RowCount, int Code10Count)>> LoadForm1RowCounts(
        DBModel db,
        IReadOnlyDictionary<string, List<int>> reportIdsByForm,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var token = cts.Token;
        var result = new Dictionary<int, (int RowCount, int Code10Count)>();
        var formCount = Form1ChildFormNumbers.Length;

        for (var formIndex = 0; formIndex < formCount; formIndex++)
        {
            var formNum = Form1ChildFormNumbers[formIndex];
            SetRowCountsProgress(
                progressBarVM,
                formIndex + 1,
                formCount,
                ProgressRowCountsEndForm1,
                formNum);

            if (!reportIdsByForm.TryGetValue(formNum, out var reportIds) || reportIds.Count == 0)
                continue;

            switch (formNum)
            {
                case "1.1":
                    await AppendForm1RowCountsAsync(db.form_11, reportIds, result, token);
                    break;
                case "1.2":
                    await AppendForm1RowCountsAsync(db.form_12, reportIds, result, token);
                    break;
                case "1.3":
                    await AppendForm1RowCountsAsync(db.form_13, reportIds, result, token);
                    break;
                case "1.4":
                    await AppendForm1RowCountsAsync(db.form_14, reportIds, result, token);
                    break;
                case "1.5":
                    await AppendForm1RowCountsAsync(db.form_15, reportIds, result, token);
                    break;
                case "1.6":
                    await AppendForm1RowCountsAsync(db.form_16, reportIds, result, token);
                    break;
                case "1.7":
                    await AppendForm1RowCountsAsync(db.form_17, reportIds, result, token);
                    break;
                case "1.8":
                    await AppendForm1RowCountsAsync(db.form_18, reportIds, result, token);
                    break;
                case "1.9":
                    await AppendForm1RowCountsAsync(db.form_19, reportIds, result, token);
                    break;
            }
        }

        return result;
    }

    #endregion

    #region InputDateRange

    /// <summary>
    /// Запрос у пользователя периода, за который необходимо выполнить выборку.
    /// </summary>
    private static async Task<(DateOnly startDateOnly, DateOnly endDateOnly)> InputDateRange(AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var res = await Dispatcher.UIThread.InvokeAsync(() =>
        {
            var window = new AskDatePeriodMessageWindow();
            return window.ShowDialog<(string command, DateOnly initialDate, DateOnly residualDate)>(Desktop.MainWindow);
        });

        if (res.command is not "Ок")
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        return (res.initialDate, res.residualDate);
    }

    #endregion
}
