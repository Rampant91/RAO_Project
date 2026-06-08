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
            : (0, 9999);

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        var worksheet = FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(20, "Получение списка организаций");
        var orgsList = await GetReportsList(db, "2.0", cts);

        progressBarVM.SetProgressBar(22, "Подготовка списка отчётов");
        var prepared = PrepareForm2Export(orgsList, minYear, maxYear);

        progressBarVM.SetProgressBar(23, "Загрузка списков форм");
        var rowCounts = await LoadForm2RowCounts(db, prepared.ReportIdsByForm, progressBarVM, cts);

        progressBarVM.SetProgressBar(85, "Заполнение строк");
        FillExcel(prepared.Orgs, rowCounts, worksheet);

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
    private static void FillExcel(
        List<FormListOrgExportData> orgsList,
        Dictionary<int, int> rowCounts,
        ExcelWorksheet worksheet)
    {
        var row = 2;
        foreach (var org in orgsList
                     .OrderBy(x => x.RegNo)
                     .ThenBy(x => x.Okpo))
        {
            foreach (var rep in OrderForm2Reports(org.Reports))
            {
                rowCounts.TryGetValue(rep.ReportId, out var rowCount);
                worksheet.Cells[row, 1].Value = org.RegNo;
                worksheet.Cells[row, 2].Value = org.Okpo;
                worksheet.Cells[row, 3].Value = org.ShortJurLico;
                worksheet.Cells[row, 4].Value = rep.FormNum_DB;
                worksheet.Cells[row, 5].Value = rep.Year_DB;
                worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
                worksheet.Cells[row, 7].Value = rowCount;
                row++;
            }
        }

        ApplyFormListColumnWidths(worksheet);
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков в .xlsx.
    /// </summary>
    private static ExcelWorksheet FillExcelHeaders(ExcelPackage excelPackage)
    {
        var worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 2");

        worksheet.Cells[1, 1].Value = "Рег №";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Отчетный год";
        worksheet.Cells[1, 6].Value = "Номер кор";
        worksheet.Cells[1, 7].Value = "Количество строк";

        worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;

        return worksheet;
    }

    #endregion

    #region LoadForm2RowCounts

    /// <summary>
    /// Загружает количество строк для отчётов выгрузки.
    /// </summary>
    private static async Task<Dictionary<int, int>> LoadForm2RowCounts(
        DBModel db,
        IReadOnlyDictionary<string, List<int>> reportIdsByForm,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var token = cts.Token;
        var result = new Dictionary<int, int>();

        foreach (var formNum in Form2ChildFormNumbers)
        {
            var progressBarValue = progressBarVM.ValueBar;
            progressBarVM.SetProgressBar(progressBarValue + 5, $"Загрузка списка форм {formNum}");

            if (!reportIdsByForm.TryGetValue(formNum, out var reportIds) || reportIds.Count == 0)
                continue;

            switch (formNum)
            {
                case "2.1":
                    await AppendForm2RowCountsAsync(db.form_21, reportIds, result, token);
                    break;
                case "2.2":
                    await AppendForm2RowCountsAsync(db.form_22, reportIds, result, token);
                    break;
                case "2.3":
                    await AppendForm2RowCountsAsync(db.form_23, reportIds, result, token);
                    break;
                case "2.4":
                    await AppendForm2RowCountsAsync(db.form_24, reportIds, result, token);
                    break;
                case "2.5":
                    await AppendForm2RowCountsAsync(db.form_25, reportIds, result, token);
                    break;
                case "2.6":
                    await AppendForm2RowCountsAsync(db.form_26, reportIds, result, token);
                    break;
                case "2.7":
                    await AppendForm2RowCountsAsync(db.form_27, reportIds, result, token);
                    break;
                case "2.8":
                    await AppendForm2RowCountsAsync(db.form_28, reportIds, result, token);
                    break;
                case "2.9":
                    await AppendForm2RowCountsAsync(db.form_29, reportIds, result, token);
                    break;
                case "2.10":
                    await AppendForm2RowCountsAsync(db.form_210, reportIds, result, token);
                    break;
                case "2.11":
                    await AppendForm2RowCountsAsync(db.form_211, reportIds, result, token);
                    break;
                case "2.12":
                    await AppendForm2RowCountsAsync(db.form_212, reportIds, result, token);
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
    private static async Task<(int minYear, int maxYear)> InputDateRange(AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
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
