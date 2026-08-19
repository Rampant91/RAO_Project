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
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Client_App.Services.DataAccess;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;
using System.Diagnostics;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x.
/// </summary>
public partial class ExcelExportFormsAsyncCommand(MainWindowVM mainWindowVM) : ExcelExportBaseAllAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;
        var cts = new CancellationTokenSource();

        var forSelectedOrg = parameter!.ToString()!.Contains("Org");
        var selectedReports = mainWindowVM.SelectedReports;
        var formNum = OnlyDigitsRegex().Replace(parameter.ToString()!, "");
        ExportType = $"Выгрузка форм {formNum}";

        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Определение имени файла");
        var fileName = await GetFileName(formNum, forSelectedOrg, selectedReports, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД", "Выгрузка форм", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(15, "Проверка наличия отчётов");
        await CheckRepsAndRepPresence(db, formNum, forSelectedOrg, selectedReports, progressBar, cts);

        progressBarVM.SetProgressBar(17, "Запрос пути сохранения");
        var (chosenPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);
        var directory = Path.GetDirectoryName(chosenPath);
        if (string.IsNullOrEmpty(directory))
            directory = Environment.CurrentDirectory;
        var chosenBaseName = Path.GetFileNameWithoutExtension(chosenPath);

        var needSplit = false;
        if (Form1SheetSplitEnabled && !forSelectedOrg && IsForm1Number(formNum))
        {
            progressBarVM.SetProgressBar(16, "Подсчёт строк формы");
            needSplit = await CountForm1RowsAsync(db, formNum, selectedReportsId: null, cts.Token)
                        > Form1SheetRowSplitThreshold;
        }

        progressBarVM.SetProgressBar(22, "Получение списка организаций");
        var repsList = await GetReportsList(db, forSelectedOrg, selectedReports!, formNum, cts);

        if (!needSplit)
        {
            progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
            using var excelPackage = await InitializeExcelPackage(chosenPath, formNum, progressBar, cts);

            progressBarVM.SetProgressBar(20, "Заполнение заголовков");
            await FillExcelHeaders(formNum);

            progressBarVM.SetProgressBar(25, "Загрузка форм");
            await GetReportRowsAndFillExcel(repsList, db, progressBarVM, formNum, cts);

            progressBarVM.SetProgressBar(95, "Сохранение");
            await ExcelSaveAndOpen(excelPackage, chosenPath, openTemp, cts, progressBar);
        }
        else
        {
            var beforeSuffix = Form1SplitFileSuffix(Form1DateSplitMode.Period22_24);
            var afterSuffix = Form1SplitFileSuffix(Form1DateSplitMode.Period25_27);

            var pathBefore = ResolveUniqueFilePath(
                Path.Combine(directory, $"{chosenBaseName}{beforeSuffix}.xlsx"),
                directory);
            var pathAfter = ResolveUniqueFilePath(
                Path.Combine(directory, $"{chosenBaseName}{afterSuffix}.xlsx"),
                directory);

            progressBarVM.SetProgressBar(18, "Инициализация Excel пакетов");
            using var excelPackageBefore = await InitializeExcelPackage(pathBefore, formNum, progressBar, cts);
            await FillExcelHeaders(formNum);

            using var excelPackageAfter = await InitializeExcelPackage(pathAfter, formNum, progressBar, cts);
            await FillExcelHeaders(formNum);

            progressBarVM.SetProgressBar(25, "Загрузка форм");
            await GetReportRowsAndFillExcelSplit(
                repsList, db, progressBarVM, formNum, excelPackageBefore, excelPackageAfter, cts);

            progressBarVM.SetProgressBar(95, "Сохранение");
            await SaveSplitExcelPackagesAndOpen(
                excelPackageBefore, pathBefore, excelPackageAfter, pathAfter, openTemp, cts, progressBar);
        }

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
            .Include(x => x.DBObservable)
            .Where(x => x.DBObservable != null)
            .Any(reps => reps.Report_Collection
                .Any(rep => rep.FormNum_DB == formNum));

        if (forSelectedOrg && selectedReports is null)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            return;
        }

        var selectedOrgMissingForm = forSelectedOrg
            && !await OrgReportsQuery.HasFormNumAsync(db, selectedReports!.Id, formNum, cts.Token);

        if (selectedOrgMissingForm || (!forSelectedOrg && !isAnyRepWithSameFormNum))
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Не удалось совершить выгрузку форм {formNum}," +
                        $"{Environment.NewLine}поскольку эти формы отсутствуют в текущей организации/базе.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
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
        Worksheet.Cells[Worksheet.Dimension.Address].AutoFilter = true;

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
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Прогрессбар.</param>
    /// <returns>Имя файла.</returns>
    private async Task<string> GetFileName(string formNum, bool forSelectedOrg, Reports? selectedReports, CancellationTokenSource cts,
        AnyTaskProgressBar? progressBar = null)
    {
        string fileName;

        if (forSelectedOrg && selectedReports is null)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        switch (forSelectedOrg)
        {
            case true:
            {
                ExportType = $"Выбранная_организация_Формы_{formNum}";
                OrgMatchQuery.EnsureTitleRowsLoaded(selectedReports);
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
        return fileName;
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
            var fromDb = await OrgReportsQuery.LoadOrgWithReportShellsForFormAsync(
                db, selectedReports.Id, formNum, cts.Token);
            if (fromDb != null)
                repsList.Add(fromDb);
        }
        else
        {
            IQueryable<Reports> query = db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Where(reps => reps.DBObservable != null
                               && reps.Report_Collection.Any(rep => rep.FormNum_DB == formNum));

            query = IsForm1Number(formNum)
                ? query.Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                : query.Include(x => x.Master_DB).ThenInclude(x => x.Rows20);

            query = query.Include(reports => reports.Report_Collection
                .Where(rep => rep.FormNum_DB == formNum));

            repsList.AddRange(await query.ToListAsync(cts.Token));
        }
        return repsList;
    }

    #endregion

    #region GetReportRowsAndFillExcel

    /// <summary>
    /// Загрузка из БД строчек форм отчётности для всех организаций из списка.
    /// </summary>
    private async Task GetReportRowsAndFillExcel(
        List<Reports> repsList,
        DBModel db,
        AnyTaskProgressBarVM progressBarVM,
        string formNum,
        CancellationTokenSource cts)
    {
        const int progressStart = 25;
        const int progressEnd = 95;
        var totalReports = repsList.Sum(r => r.Report_Collection.Count(x => x.FormNum_DB == formNum));
        var loadedReports = 0;

        foreach (var reps in repsList.OrderBy(x => x.Master_DB.RegNoRep.Value))
        {
            var orderedReps = reps.Report_Collection
                .Where(x => x.FormNum_DB == formNum)
                .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
                .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
                .ToList();
            var repsWithRows = new Reports { Master = reps.Master };
            var orgStatus = $"Загрузка отчётов {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}";

            for (var i = 0; i < orderedReps.Count; i++)
            {
                var stub = orderedReps[i];
                progressBarVM.SetProgressBar(
                    MapLoadProgress(loadedReports, totalReports, progressStart, progressEnd),
                    $"Загрузка отчёта {formNum} {stub.StartPeriod_DB}–{stub.EndPeriod_DB} ({loadedReports + 1} из {totalReports})",
                    orgStatus);

                var rep = await GetReportWithRowsForFormAsync(stub.Id, formNum, db, cts.Token);
                repsWithRows.Report_Collection.Add(rep);
                loadedReports++;

                progressBarVM.SetProgressBar(
                    MapLoadProgress(loadedReports, totalReports, progressStart, progressEnd),
                    $"Загрузка отчёта {formNum} {stub.StartPeriod_DB}–{stub.EndPeriod_DB} ({loadedReports} из {totalReports})",
                    orgStatus);
            }

            CurrentReports = repsWithRows;
            CurrentRow = Worksheet.Dimension.End.Row + 1;
            CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
            FillExportForms(formNum);
        }
    }

    /// <summary>
    /// Одна загрузка из БД и заполнение двух Excel-пакетов (до 2023 / с 2024).
    /// </summary>
    private async Task GetReportRowsAndFillExcelSplit(
        List<Reports> repsList,
        DBModel db,
        AnyTaskProgressBarVM progressBarVM,
        string formNum,
        ExcelPackage packageBefore,
        ExcelPackage packageAfter,
        CancellationTokenSource cts)
    {
        var wsBefore = packageBefore.Workbook.Worksheets[$"Отчеты {formNum}"];
        var primBefore = packageBefore.Workbook.Worksheets[$"Примечания {formNum}"];
        var wsAfter = packageAfter.Workbook.Worksheets[$"Отчеты {formNum}"];
        var primAfter = packageAfter.Workbook.Worksheets[$"Примечания {formNum}"];

        const int progressStart = 25;
        const int progressEnd = 95;
        var totalReports = repsList.Sum(r => r.Report_Collection.Count(x => x.FormNum_DB == formNum));
        var loadedReports = 0;

        foreach (var reps in repsList.OrderBy(x => x.Master_DB.RegNoRep.Value))
        {
            var orderedReps = reps.Report_Collection
                .Where(x => x.FormNum_DB == formNum)
                .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
                .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
                .ToList();
            var repsWithRows = new Reports { Master = reps.Master };
            var orgStatus = $"Загрузка отчётов {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}";

            for (var i = 0; i < orderedReps.Count; i++)
            {
                var stub = orderedReps[i];
                progressBarVM.SetProgressBar(
                    MapLoadProgress(loadedReports, totalReports, progressStart, progressEnd),
                    $"Загрузка отчёта {formNum} {stub.StartPeriod_DB}–{stub.EndPeriod_DB} ({loadedReports + 1} из {totalReports})",
                    orgStatus);

                var rep = await GetReportWithRowsForFormAsync(stub.Id, formNum, db, cts.Token);
                repsWithRows.Report_Collection.Add(rep);
                loadedReports++;

                progressBarVM.SetProgressBar(
                    MapLoadProgress(loadedReports, totalReports, progressStart, progressEnd),
                    $"Загрузка отчёта {formNum} {stub.StartPeriod_DB}–{stub.EndPeriod_DB} ({loadedReports} из {totalReports})",
                    orgStatus);
            }

            CurrentReports = repsWithRows;

            CurrentForm1DateSplit = Form1DateSplitMode.Period22_24;
            Worksheet = wsBefore;
            WorksheetPrim = primBefore;
            CurrentRow = Worksheet.Dimension.End.Row + 1;
            CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
            FillExportForms(formNum);

            CurrentForm1DateSplit = Form1DateSplitMode.Period25_27;
            Worksheet = wsAfter;
            WorksheetPrim = primAfter;
            CurrentRow = Worksheet.Dimension.End.Row + 1;
            CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
            FillExportForms(formNum);
        }

        CurrentForm1DateSplit = Form1DateSplitMode.None;
    }

    #endregion

    #region SaveSplitExcelPackagesAndOpen

    private static async Task SaveSplitExcelPackagesAndOpen(
        ExcelPackage packageBefore,
        string pathBefore,
        ExcelPackage packageAfter,
        string pathAfter,
        bool openTemp,
        CancellationTokenSource cts,
        AnyTaskProgressBar progressBar)
    {
        try
        {
            await packageBefore.SaveAsync(cancellationToken: cts.Token);
            await packageAfter.SaveAsync(cancellationToken: cts.Token);
        }
        catch (ObjectDisposedException)
        {
            return;
        }
        catch (Exception)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Не удалось сохранить файлы по указанным путям:" +
                                     $"{Environment.NewLine}{pathBefore}" +
                                     $"{Environment.NewLine}{pathAfter}",
                    MinWidth = 400,
                    MinHeight = 175,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(Desktop.MainWindow));

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            return;
        }

        if (openTemp)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = pathBefore,
                UseShellExecute = true
            });
            Process.Start(new ProcessStartInfo
            {
                FileName = pathAfter,
                UseShellExecute = true
            });
            return;
        }

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок" },
                    new ButtonDefinition { Name = "Открыть выгрузку" }
                ],
                ContentTitle = "Выгрузка в .xlsx",
                ContentHeader = "Уведомление",
                ContentMessage = "Выгрузка сохранена в два файла:" +
                                 $"{Environment.NewLine}{pathBefore}" +
                                 $"{Environment.NewLine}{pathAfter}",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            })
            .ShowDialog(Desktop.MainWindow));

        if (answer is "Открыть выгрузку")
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = pathBefore,
                UseShellExecute = true
            });
            Process.Start(new ProcessStartInfo
            {
                FileName = pathAfter,
                UseShellExecute = true
            });
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