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

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Все формы и Excel -> Выбранная организация -> Все формы.
/// </summary>
public class ExcelExportAllAsyncCommand(MainWindowVM mainWindowVM) : ExcelExportBaseAllAsyncCommand
{
    private HashSet<string> Form1SplitFormNums = [];

    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        IsSelectedOrg = parameter is "SelectedOrg";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка всех отчётов", ExportType);
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;
        var selectedReports = mainWindowVM.SelectedReports;

        progressBarVM.SetProgressBar(3, "Определение имени файла");
        var fileName = await GetFileName(selectedReports, progressBar, cts);

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения");
        var (fullPath, openTemp) = !isBackgroundCommand
            ? await ExcelGetFullPath(fileName, cts, progressBar)
            : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

        progressBarVM.SetProgressBar(7, "Создание временной БД", 
            "Выгрузка всех отчётов", "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        if (!isBackgroundCommand)
        {
            progressBarVM.SetProgressBar(10, "Подсчёт количества организаций");
            await CountReports(db, progressBar, cts);
        }

        var count = 0;
        while (File.Exists(fullPath))
        {
            fullPath = Path.Combine(folderPath, fileName + $"_{++count}.xlsx");
        }

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(15, "Загрузка списка отчётов");
        var repsList = await GetReportsList(db, cts);

        progressBarVM.SetProgressBar(16, "Определение форм для разбиения");
        Form1SplitFormNums = IsSelectedOrg
            ? []
            : await GetForm1FormsNeedingSplit(db, repsList, selectedReportsId: null, cts.Token);

        progressBarVM.SetProgressBar(17, "Создание страниц и заполнение заголовков");
        var formNums = await CreateWorksheetsAndFillHeaders(excelPackage, repsList, Form1SplitFormNums);

        progressBarVM.SetProgressBar(20, "Загрузка форм");
        await GetFullReportForeachReps(db, repsList, formNums, progressBarVM, excelPackage, cts);

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

    #region CountReports

    /// <summary>
    /// Подсчёт количества организаций. При = 0 || > 10 выводит сообщение и завершает операцию.
    /// </summary>
    /// <param name="dbReadOnly">Модель временной БД.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private async Task CountReports(DBModel dbReadOnly, AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        var countReports = await dbReadOnly.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Where(x => x.DBObservableId != null)
            .CountAsync(cts.Token);

        switch (countReports)
        {
            case 0:
            {
                #region MessageExcelExportFail

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        CanResize = true,
                        ContentTitle = "Выгрузка в .xlsx",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Выгрузка не выполнена, поскольку в базе отсутствуют формы отчетности организаций.",
                        MinHeight = 175,
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    })
                    .ShowDialog(progressBar ?? Desktop.MainWindow));

                    #endregion

                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                break;
            }
            case > 10 when !IsSelectedOrg:
            {
                #region MessageLongOperation

                var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Да", IsDefault = true },
                            new ButtonDefinition { Name = "Отменить выгрузку", IsCancel = true }
                        ],
                        ContentTitle = "Выгрузка",
                        ContentHeader = "Уведомление",
                        ContentMessage = $"Текущая база содержит {countReports} форм организаций," +
                                         $"{Environment.NewLine}выгрузка может занять длительный период времени. " +
                                         $"{Environment.NewLine}Продолжить операцию?",
                        MinWidth = 450,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }).ShowDialog(progressBar ?? Desktop.MainWindow));

                #endregion

                if (answer is not "Да") await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                break;
            }
        }
    }

    #endregion

    #region CreateWorksheetsAndFillHeaders

    /// <summary>
    /// Определяет список номеров форм отчётности, по которым у организаций имеется отчётность. Создаёт соответствующие страницы в .xlsx.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <param name="repsList">Список организаций вместе с их отчётами.</param>
    /// <param name="formsNeedingSplit">Формы 1.x, для которых нужны пары листов по периодам.</param>
    /// <returns>HashSet номеров форм отчётности.</returns>
    private Task<HashSet<string>> CreateWorksheetsAndFillHeaders(
        ExcelPackage excelPackage, List<Reports> repsList, HashSet<string> formsNeedingSplit)
    {
        HashSet<string> formNums = [];
        foreach (var rep in repsList
                     .SelectMany(reps => reps.Report_Collection)
                     .OrderBy(x => byte.Parse(x.FormNum_DB.Split('.')[0]))
                     .ThenBy(x => byte.Parse(x.FormNum_DB.Split('.')[1]))
                     .ToList())
        {
            formNums.Add(rep.FormNum_DB);
        }
        foreach (var formNum in formNums)
        {
            if (formsNeedingSplit.Contains(formNum))
            {
                foreach (var mode in new[] { Form1DateSplitMode.Period22_24, Form1DateSplitMode.Period25_27 })
                {
                    var suffix = Form1SplitSheetSuffix(mode);
                    Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}{suffix}");
                    WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}{suffix}");
                    FillHeaders(formNum);
                }
            }
            else
            {
                Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}");
                WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}");
                FillHeaders(formNum);
            }
        }
        return Task.FromResult(formNums);
    }

    #endregion

    #region FillExcel

    /// <summary>
    /// Заполняет строчки для каждой страницы документа.
    /// </summary>
    /// <param name="formNums">HashSet имеющихся номеров отчётов.</param>
    /// <param name="repsWithRows">Организация вместе с коллекцией отчётов со строчками.</param>
    /// <param name="excelPackage">Пакет Excel.</param>
    private Task FillExcel(HashSet<string> formNums, Reports repsWithRows, ExcelPackage excelPackage)
    {
        foreach (var formNum in formNums)
        {
            CurrentReports = repsWithRows;
            if (Form1SplitFormNums.Contains(formNum))
            {
                foreach (var mode in new[] { Form1DateSplitMode.Period22_24, Form1DateSplitMode.Period25_27 })
                {
                    var suffix = Form1SplitSheetSuffix(mode);
                    CurrentForm1DateSplit = mode;
                    Worksheet = excelPackage.Workbook.Worksheets[$"Отчеты {formNum}{suffix}"];
                    WorksheetPrim = excelPackage.Workbook.Worksheets[$"Примечания {formNum}{suffix}"];
                    CurrentRow = Worksheet.Dimension.End.Row + 1;
                    CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
                    FillExportForms(formNum);
                }
                CurrentForm1DateSplit = Form1DateSplitMode.None;
            }
            else
            {
                Worksheet = excelPackage.Workbook.Worksheets[$"Отчеты {formNum}"];
                WorksheetPrim = excelPackage.Workbook.Worksheets[$"Примечания {formNum}"];
                CurrentRow = Worksheet.Dimension.End.Row + 1;
                CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
                FillExportForms(formNum);
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region GetForm1FormsNeedingSplit

    private async Task<HashSet<string>> GetForm1FormsNeedingSplit(
        DBModel db, List<Reports> repsList, int? selectedReportsId, CancellationToken ct)
    {
        HashSet<string> formNums = [];
        foreach (var rep in repsList.SelectMany(reps => reps.Report_Collection))
            formNums.Add(rep.FormNum_DB);

        HashSet<string> result = [];
        foreach (var formNum in formNums.Where(IsForm1Number))
        {
            var rowCount = await CountForm1RowsAsync(db, formNum, selectedReportsId, ct);
            if (rowCount > Form1SheetRowSplitThreshold)
                result.Add(formNum);
        }
        return result;
    }

    #endregion

    #region GetFileName

    /// <summary>
    /// Определение имени файла.
    /// </summary>
    /// <param name="selectedReports">Выбранная организация.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Имя файла.</returns>
    private async Task<string> GetFileName(Reports? selectedReports, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        string fileName;
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        if (!IsSelectedOrg)
        {
            ExportType = "Все_формы";
            fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
            return fileName;
        }
        if (selectedReports is null || selectedReports.Report_Collection.Count == 0)
        {
            #region MessageExcelExportFail

            var msg = "Выгрузка не выполнена, поскольку ";
            msg += selectedReports is null
                ? "не выбрана организация."
                : "у выбранной организации" +
                  $"{Environment.NewLine}отсутствуют формы отчетности.";
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = msg,
                    MinHeight = 150,
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        CurrentReports = selectedReports!;
        ExportType = "Выбранная_организация_Все_формы";
        progressBarVM.ExportName = $"Выгрузка всех отчётов " +
                                   $"{selectedReports!.Master.RegNoRep.Value}_{selectedReports.Master.OkpoRep.Value}";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% (Определение имени файла)";

        var regNum = RemoveForbiddenChars(CurrentReports.Master_DB.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(CurrentReports.Master_DB.OkpoRep.Value);
        fileName = $"{ExportType}_{regNum}_{okpo}_{Assembly.GetExecutingAssembly().GetName().Version}";
        return fileName;
    }

    #endregion

    #region GetFullReportForeachReps

    /// <summary>
    /// Для каждой организации в коллекции загружает из БД отчёты со строчками нужной формы и заполняет .xlsx файл.
    /// </summary>
    private async Task GetFullReportForeachReps(DBModel dbReadOnly, List<Reports> repsList, HashSet<string> formNums, 
        AnyTaskProgressBarVM progressBarVM, ExcelPackage excelPackage, CancellationTokenSource cts)
    {
        const int progressStart = 20;
        const int progressEnd = 95;

        var totalReports = repsList.Sum(r => r.Report_Collection.Count(rep => formNums.Contains(rep.FormNum_DB)));
        var loadedReports = 0;

        foreach (var reps in repsList.OrderBy(x => x.Master_DB.RegNoRep.Value))
        {
            var ordered = reps.Report_Collection
                .OrderBy(x => x.FormNum_DB)
                .ThenBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate)
                    ? stDate
                    : DateOnly.MaxValue)
                .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate)
                    ? endDate
                    : DateOnly.MaxValue)
                .ToList();
            var repsWithRows = new Reports { Master = reps.Master };
            var orgStatus = $"Загрузка отчётов {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}";

            foreach (var stub in ordered.Where(r => formNums.Contains(r.FormNum_DB)))
            {
                var formNum = stub.FormNum_DB;
                progressBarVM.SetProgressBar(
                    MapLoadProgress(loadedReports, totalReports, progressStart, progressEnd),
                    $"Загрузка отчёта {formNum} {stub.StartPeriod_DB}–{stub.EndPeriod_DB} ({loadedReports + 1} из {totalReports})",
                    orgStatus);

                var rep = await GetReportWithRowsForFormAsync(stub.Id, formNum, dbReadOnly, cts.Token);
                repsWithRows.Report_Collection.Add(rep);
                loadedReports++;

                progressBarVM.SetProgressBar(
                    MapLoadProgress(loadedReports, totalReports, progressStart, progressEnd),
                    $"Загрузка отчёта {formNum} {stub.StartPeriod_DB}–{stub.EndPeriod_DB} ({loadedReports} из {totalReports})",
                    orgStatus);
            }

            await FillExcel(formNums, repsWithRows, excelPackage);
        }
    }

    #endregion

    #region GetReportsList

    /// <summary>
    /// Загружает из БД список организаций вместе с их отчётностью (без строчек).
    /// </summary>
    /// <param name="dbReadOnly">Модель временной БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список организаций.</returns>
    private async Task<List<Reports>> GetReportsList(DBModel dbReadOnly, CancellationTokenSource cts)
    {
        var repsList = new List<Reports>();
        if (IsSelectedOrg)
        {
            repsList.Add(CurrentReports);
        }
        else
        {
            repsList.AddRange(
                await dbReadOnly.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(reports => reports.Report_Collection)
                    .Where(x => x.DBObservable != null)
                    .ToListAsync(cts.Token));
        }
        return repsList
            .Where(x => x.Master_DB.FormNum_DB.StartsWith('1') 
                        || x.Master_DB.FormNum_DB.StartsWith('2'))
            .ToList();
    }

    #endregion
}