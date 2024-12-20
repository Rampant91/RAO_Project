using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x.
/// </summary>
public partial class ExcelExportFormsAsyncCommand : ExcelExportBaseAllAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;
        var cts = new CancellationTokenSource();

        var forSelectedOrg = parameter!.ToString()!.Contains("Org");
        var selectedReports = (Reports?)mainWindow?.SelectedReports?.FirstOrDefault();
        var formNum = OnlyDigitsRegex().Replace(parameter.ToString()!, "");
        ExportType = $"Выгрузка форм {formNum}";

        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Определение имени файла");
        var fileName = await GetFileName(formNum, forSelectedOrg, selectedReports!);

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения");
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД", "Выгрузка форм", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(15, "Проверка наличия отчётов");
        await CheckRepsAndRepPresence(db, formNum, forSelectedOrg, selectedReports, progressBar, cts);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath, formNum, progressBar, cts);

        progressBarVM.SetProgressBar(20, "Заполнение заголовков");
        await FillExcelHeaders(formNum);

        progressBarVM.SetProgressBar(22, "Получение списка организаций");
        var repsList = await GetReportsList(db, forSelectedOrg, selectedReports!, formNum, cts);

        progressBarVM.SetProgressBar(25, "Загрузка форм");
        await GetReportRowsAndFillExcel(repsList, db, progressBarVM, formNum, cts);

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

    #region CheckRepsAndRepPresence

    /// <summary>
    /// Проверяет наличие выбранной организации, в случае если запущена команда для неё.
    /// Проверяет наличие хотя бы одного отчёта, с выбранным номером формы. В случае отсутствия выводит соответствующее сообщение и закрывает команду.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="forSelectedOrg">Флаг, выполняется ли команда для выбранной организации или для всех организаций в БД.</param>
    /// <param name="selectedReports">Выбранная организация.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckRepsAndRepPresence(DBModel db, string formNum, bool forSelectedOrg, Reports? selectedReports, 
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var isAnyRepWithSameFormNum = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Any(reps => reps.Report_Collection
                .Any(rep => rep.FormNum_DB == formNum));
        switch (forSelectedOrg)
        {
            case true when selectedReports is null:
            {
                #region MessageExcelExportFail

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);

                return;
            }
            case true when selectedReports.Report_Collection.All(rep => rep.FormNum_DB != formNum):
            case false when !isAnyRepWithSameFormNum:
            {
                #region MessageRepsNotFound

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            $"Не удалось совершить выгрузку форм {formNum}," +
                            $"{Environment.NewLine}поскольку эти формы отсутствуют в текущей базе.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion
                
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                
                return;
            }
        }
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков Excel.
    /// </summary>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <returns>Успешно выполненная Task.</returns>
    private Task FillExcelHeaders(string formNum)
    {
        int masterHeaderLength;
        if (formNum.Split('.')[0] == "1")
        {
            masterHeaderLength = Form10.ExcelHeader(Worksheet, 1, 1, id: "ID") + 1;
            masterHeaderLength = Form10.ExcelHeader(WorksheetPrim, 1, 1, id: "ID") + 1;
        }
        else
        {
            masterHeaderLength = Form20.ExcelHeader(Worksheet, 1, 1, id: "ID") + 1;
            masterHeaderLength = Form20.ExcelHeader(WorksheetPrim, 1, 1, id: "ID") + 1;
        }

        var t = Report.ExcelHeader(Worksheet, formNum, 1, masterHeaderLength);
        Report.ExcelHeader(WorksheetPrim, formNum, 1, masterHeaderLength);
        masterHeaderLength += t;
        masterHeaderLength--;

        FillHeaders(formNum);
        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns();
            WorksheetPrim.Cells.AutoFitColumns();
        }
        return Task.CompletedTask;
    }

    #endregion

    #region GetFileName

    /// <summary>
    /// Формирование имени файла.
    /// </summary>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="forSelectedOrg">Флаг, выполняется ли команда для выбранной организации или для всех организаций в БД.</param>
    /// <param name="selectedReports">Выбранная организация.</param>
    /// <returns>Имя файла.</returns>
    private Task<string> GetFileName(string formNum, bool forSelectedOrg, Reports selectedReports)
    {
        string fileName;
        switch (forSelectedOrg)
        {
            case true:
            {
                ExportType = $"Выбранная_организация_Формы_{formNum}";
                var regNum = StaticStringMethods.RemoveForbiddenChars(selectedReports.Master.RegNoRep.Value);
                var okpo = StaticStringMethods.RemoveForbiddenChars(selectedReports.Master.OkpoRep.Value);
                fileName = $"{ExportType}_{regNum}_{okpo}_{Assembly.GetExecutingAssembly().GetName().Version}";
                break;
            }
            case false:
            {
                ExportType = $"Формы_{formNum}";
                fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
                break;
            }

        }
        return Task.FromResult(fileName);
    }

    #endregion

    #region GetReportWithRows

    /// <summary>
    /// Получение отчёта вместе со строчками из БД.
    /// </summary>
    /// <param name="repId">Id отчёта.</param>
    /// <param name="dbReadOnly">Модель временной БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отчёт вместе со строчками.</returns>
    private static async Task<Report> GetReportWithRows(int repId, DBModel dbReadOnly, CancellationTokenSource cts)
    {
        return await dbReadOnly.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(rep => rep.Rows11.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows12.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows13.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows14.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows15.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows16.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows17.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows18.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows19.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows21.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows22.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows23.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows24.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows25.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows26.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows27.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows28.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows29.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows210.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows211.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows212.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Notes.OrderBy(note => note.Order))
                .FirstAsync(rep => rep.Id == repId, cts.Token);
    }

    #endregion

    #region GetReportsList

    /// <summary>
    /// Формирование списка организаций без строчек форм отчётности.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="forSelectedOrg">Флаг, выполняется ли команда для выбранной организации или всех организаций в БД.</param>
    /// <param name="selectedReports">Выбранная организация.</param>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список организаций без строчек форм отчётности.</returns>
    private static async Task<List<Reports>> GetReportsList(DBModel db, bool forSelectedOrg, Reports selectedReports, string formNum, 
        CancellationTokenSource cts)
    {
        var repsList = new List<Reports>();
        if (forSelectedOrg)
        {
            repsList.Add(selectedReports);
        }
        else
        {
            repsList.AddRange(
                await db.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(reports => reports.Report_Collection)
                    .Where(reps => reps.DBObservable != null 
                                && reps.Report_Collection
                                    .Any(rep => rep.FormNum_DB == formNum))
                    .ToListAsync(cts.Token));
        }
        return repsList;
    }

    #endregion

    #region GetReportRowsAndFillExcel

    /// <summary>
    /// Загрузка из БД строчек форм отчётности для всех организаций из списка.
    /// </summary>
    /// <param name="repsList">Список организаций.</param>
    /// <param name="db">Модель БД.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="cts">Токен.</param>
    private async Task GetReportRowsAndFillExcel(List<Reports> repsList, DBModel db, AnyTaskProgressBarVM progressBarVM, string formNum,
        CancellationTokenSource cts)
    {
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var reps in repsList.OrderBy(x => x.Master_DB.RegNoRep.Value))
        {
            var repsWithRows = new Reports { Master = reps.Master };
            foreach (var rep in reps.Report_Collection
                         .Where(x => x.FormNum_DB == formNum)
                         .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
                         .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue))
            {
                var repWithRows = await GetReportWithRows(rep.Id, db, cts);
                repsWithRows.Report_Collection.Add(repWithRows);
                progressBarDoubleValue += (double)70 / (repsList.Count * reps.Report_Collection.Count);
                progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                    $"Загрузка отчёта {rep.FormNum_DB}_{rep.StartPeriod_DB}_{rep.EndPeriod_DB}",
                    $"Загрузка отчётов {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}");
            }
            CurrentReports = repsWithRows;
            CurrentRow = Worksheet.Dimension.End.Row + 1;
            CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
            FillExportForms(formNum);
        }
    }

    #endregion

    #region InitializeExcelPackage

    /// <summary>
    /// Инициализация Excel пакета.
    /// </summary>
    /// <param name="fullPath">Полный путь к файлу.</param>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Excel пакет.</returns>
    private async Task<ExcelPackage> InitializeExcelPackage(string fullPath, string formNum, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}");
        WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}");
        Worksheet.View.FreezePanes(2, 1);
        return excelPackage;
    }

    #endregion

    #region Regex
    
    /// <summary>
    /// Проверяет, что строчка содержит только цифры.
    /// </summary>
    [GeneratedRegex(@"[^\d.]")]
    private static partial Regex OnlyDigitsRegex();

    #endregion
}