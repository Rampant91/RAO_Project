using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Properties;
using Client_App.Services;
using Client_App.ViewModels;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using DynamicData;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Выбранная форма -> Выгрузка Excel -> Для печати.
/// </summary>
public class ExcelExportFormPrintAsyncCommand : ExcelBaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public ExcelExportFormPrintAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
    {
        _formsTabControlVM = formsTabControlVM;

        formsTabControlVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReport))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _formsTabControlVM.SelectedReport is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        Report? repParam;
        int repId;
        if (parameter is ObservableCollectionWithItemPropertyChanged<IKey> forms)
        {
            if (forms.Count == 0)
                return;
            repParam = (Report)forms.First();
            repId = repParam.Id;
        }
        else if (parameter is Report report)
        {
            repParam = report;
            repId = repParam.Id;
        }
        else
            return;

        var cts = new CancellationTokenSource();
        ExportType = "Для_печати";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        var organizationId = ReportExportLock.ResolveOrganizationId(repParam!, _formsTabControlVM.SelectedReports);
        if (organizationId <= 0)
        {
            await progressBar.CloseAsync();
            return;
        }

        using var exportLock = ReportExportLock.Acquire(repId, organizationId);
        using var releaseExportLockOnCancel = cts.Token.Register(exportLock.Dispose);

        try
        {
            progressBarVM.SetProgressBar(5, "Загрузка отчёта", "Выгрузка отчёта для печати", ExportType);
            var rep = await GetReportWithRows(repId, cts);

            progressBarVM.SetProgressBar(10, "Определение имени файла", "Выгрузка отчёта для печати", ExportType);
            var fileName = await GetFileName(rep, progressBar, cts);

            progressBarVM.SetProgressBar(15, "Запрос пути сохранения");
            var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

            progressBarVM.SetProgressBar(70, "Инициализация Excel пакета");
            using var excelPackage = await InitializeExcelPackage(fullPath, rep);

            progressBarVM.SetProgressBar(75, "Проверка отчёта");
            await CheckForm(rep, cts, progressBar);

            progressBarVM.SetProgressBar(80, "Выгрузка данных");
            await FillExcel(excelPackage, rep);

            progressBarVM.SetProgressBar(90, "Сохранение");
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

            progressBarVM.SetProgressBar(100, "Завершение выгрузки");
        }
        finally
        {
            GC.Collect();
            try
            {
                await progressBar.CloseAsync();
            }
            catch
            {
                // Окно могло быть уже закрыто при отмене через CancelCommandAndCloseProgressBarWindow.
            }
        }
    }

    /// <summary>
    /// Выгрузка отчёта в Excel для печати (перегрузка для пакетной обработки без диалогов).
    /// </summary>
    /// <param name="report">Отчёт для выгрузки.</param>
    /// <param name="destinationFolder">Папка назначения.</param>
    /// <param name="suppressDialogs">Подавлять ли диалоги (true для пакетной обработки).</param>
    public async Task AsyncExecute(Report report, string destinationFolder, bool suppressDialogs = false)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Для_печати";

        AnyTaskProgressBar? progressBar = null;
        AnyTaskProgressBarVM? progressBarVM = null;

        if (!suppressDialogs)
        {
            progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
            progressBarVM = progressBar.AnyTaskProgressBarVM;
            progressBarVM.SetProgressBar(5, "Загрузка отчёта", "Выгрузка отчёта для печати", ExportType);
        }

        var organizationId = ReportExportLock.ResolveOrganizationId(report, _formsTabControlVM.SelectedReports);
        IDisposable? exportLock = null;
        if (organizationId > 0)
        {
            exportLock = ReportExportLock.Acquire(report.Id, organizationId);
        }

        try
        {
            var rep = await GetReportWithRows(report.Id, cts);

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(10, "Определение имени файла", "Выгрузка отчёта для печати", ExportType);
            }

            var fileName = await GetFileName(rep, progressBar, cts);
            var fullPath = Path.Combine(destinationFolder, fileName + ".xlsx");

            // Проверяем существование файла и генерируем уникальное имя при необходимости
            var counter = 1;
            while (File.Exists(fullPath))
            {
                var fileNameWithoutExt = fileName + $"_{counter}";
                fullPath = Path.Combine(destinationFolder, fileNameWithoutExt + ".xlsx");
                counter++;
            }

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(15, "Запрос пути сохранения");
            }

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(70, "Инициализация Excel пакета");
            }
            using var excelPackage = await InitializeExcelPackage(fullPath, rep);

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(75, "Проверка отчёта");
            }
            // Проверка на ошибки только при одиночной выгрузке
            if (!suppressDialogs)
            {
                await CheckForm(rep, cts, progressBar);
            }

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(80, "Выгрузка данных");
            }
            await FillExcel(excelPackage, rep);

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(90, "Сохранение");
            }
            
            // Для пакетной обработки не показываем финальный диалог
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp: false, cts, progressBar, isBackground: suppressDialogs);

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(100, "Завершение выгрузки");
            }
        }
        finally
        {
            exportLock?.Dispose();

            if (!suppressDialogs && progressBar != null)
            {
                await progressBar.CloseAsync();
            }
            GC.Collect();
        }
    }

    #region CheckForm

    private static async Task CheckForm(Report exportReport, CancellationTokenSource cts, AnyTaskProgressBar? progressBar)
    {
        if (cts.Token.IsCancellationRequested)
        {
            return;
        }

        if (exportReport.FormNum_DB is not ("1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8"))
        {
            return;
        }

        if (progressBar is null)
        {
            return;
        }

        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var checkProgress = ReportCheckProgress.ForExportPhase(
            progressBarVM,
            75,
            80,
            progressBarVM.ExportType ?? "Для_печати");
        checkProgress.SetOrgHeader(
            exportReport.Reports?.Master_DB?.RegNoRep?.Value ?? string.Empty,
            exportReport.Reports?.Master_DB?.OkpoRep?.Value ?? string.Empty,
            exportReport.FormNum_DB,
            $"{exportReport.StartPeriod_DB}-{exportReport.EndPeriod_DB}");
        checkProgress.OnLoadComplete(Services.DataAccess.ReportCheckSnapshotLoader.CountLoadedRows(exportReport));

        var checkTask = Task.Run(
            () => ReportCheckRunner.ExecuteCheck(exportReport.Reports, exportReport, checkProgress),
            cts.Token);

        var cancelWait = Task.Delay(Timeout.Infinite, cts.Token);
        if (await Task.WhenAny(checkTask, cancelWait) != checkTask)
        {
            return;
        }

        List<CheckError> errorList;
        try
        {
            errorList = await checkTask;
        }
        catch (Exception)
        {
            return;
        }

        if (cts.Token.IsCancellationRequested)
        {
            return;
        }

        if (!errorList.Any(x => x.IsCritical)) return;

        if (!Settings.Default.AppLaunchedInNorao)
        {
            #region ExportTerminatedDueToCriticalErrors

            await Dispatcher.UIThread.InvokeAsync(() =>
                MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в .xlsx",
                        ContentHeader = "Ошибка",
                        ContentMessage = "Выгрузка отчёта невозможна из-за наличия в нём критических ошибок (выделены красным)." +
                                         $"{Environment.NewLine}Устраните ошибки и повторите операцию выгрузки.",
                        MinWidth = 250,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }).ShowDialog(Desktop.MainWindow));

            #endregion

            await Dispatcher.UIThread.InvokeAsync(() => new Views.CheckForm(new ChangeOrCreateVM(exportReport.FormNum_DB, exportReport), errorList));

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        else
        {
            #region ReportHasCriticalErrors

            var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да" },
                        new ButtonDefinition { Name = "Отмена" }
                    ],
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"В отчёте присутствуют критические ошибки (выделены красным). " +
                                     $"{Environment.NewLine}Всё равно выгрузить отчёт?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            if (answer is "Да") return;

            await Dispatcher.UIThread.InvokeAsync(() => new Views.CheckForm(new ChangeOrCreateVM(exportReport.FormNum_DB, exportReport), errorList));

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion

    #region FillExcel

    /// <summary>
    /// Заполняет .xlsx строчками данных.
    /// </summary>
    /// <param name="excelPackage">Пакет Excel.</param>
    /// <param name="rep">Отчёт.</param>
    /// <returns>Успешно выполненная Task.</returns>
    private static Task FillExcel(ExcelPackage excelPackage, Report rep)
    {
        var (titleName, mainName) = GetPrintWorksheetNames(rep.FormNum_DB);
        var worksheetTitle = excelPackage.Workbook.Worksheets[titleName]
            ?? throw new InvalidOperationException($"В шаблоне не найден лист «{titleName}».");
        var worksheetMain = excelPackage.Workbook.Worksheets[mainName]
            ?? throw new InvalidOperationException($"В шаблоне не найден лист «{mainName}».");

        if (rep.Reports?.Master is null)
            throw new InvalidOperationException("У отчёта отсутствует головная форма организации.");

        ExcelPrintTitleExport(rep.FormNum_DB, worksheetTitle, rep, rep.Reports.Master);

        ExcelPrintSubMainExport(rep.FormNum_DB, worksheetMain, rep);

        var notesExported = (worksheetTitle.Name is "1.0" or "2.0" or "Форма 5.0")
                            && worksheetMain.Name is not "Форма 5.7";
        if (notesExported)
            ExcelPrintNotesExport(rep.FormNum_DB, worksheetMain, rep);

        ExcelPrintRowsExport(rep.FormNum_DB, worksheetMain, rep);

        ApplyExcelExecutorWrapText(rep.FormNum_DB, worksheetMain, rep, notesExported);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Имена листов шаблона печати для номера формы.
    /// </summary>
    private static (string TitleName, string MainName) GetPrintWorksheetNames(string formNum)
    {
        var titleName = $"{formNum.Split('.')[0]}.0";
        if (titleName is "4.0" or "5.0")
            titleName = "Форма " + titleName;

        var mainName = formNum;
        if (mainName is "4.1" or "5.1" or "5.2" or "5.3" or "5.4" or "5.5" or "5.6" or "5.7")
            mainName = "Форма " + mainName;

        return (titleName, mainName);
    }

    #endregion

    #region GetFileName

    /// <summary>
    /// Определение имени файла.
    /// </summary>
    /// <param name="rep">Отчёт.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Имя файла.</returns>
    private async Task<string> GetFileName(Report rep, AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        var formNum = RemoveForbiddenChars(rep.FormNum_DB);
        if (string.IsNullOrEmpty(formNum))
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            return "";
        }

        var regNum = "";
        var okpo = "";
        var corNum = "";

        if (rep.Reports?.Master?.RegNoRep != null)
            regNum = RemoveForbiddenChars(rep.Reports.Master.RegNoRep.Value);

        if (rep.Reports?.Master?.OkpoRep != null)
            okpo = RemoveForbiddenChars(rep.Reports.Master.OkpoRep.Value);

        corNum = Convert.ToString(rep.CorrectionNumber_DB);

        string fileName;
        switch (formNum[0])
        {
            case '1':
                {
                    var startPeriod = RemoveForbiddenChars(rep.StartPeriod_DB);
                    var endPeriod = RemoveForbiddenChars(rep.EndPeriod_DB);
                    fileName = $"{regNum}_{okpo}_{formNum}_{startPeriod}_{endPeriod}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";
                    break;
                }
            case '2':
                {
                    var year = RemoveForbiddenChars(rep.Year_DB);
                    fileName = $"{regNum}_{okpo}_{formNum}_{year}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";
                    break;
                }
            case '4':
                {
                    var codeSubjectRF = "";
                    var row40 = rep.Reports?.Master_DB?.Rows40?.FirstOrDefault();
                    if (row40?.CodeSubjectRF?.Value != null)
                        codeSubjectRF = RemoveForbiddenChars(row40.CodeSubjectRF.Value);
                    var year = RemoveForbiddenChars(rep.Year_DB);
                    fileName = $"{codeSubjectRF}_{formNum}_{year}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";
                    break;
                }
            case '5':
                {
                    var year = RemoveForbiddenChars(rep.Year_DB);
                    fileName = $"{formNum}_{year}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";
                    break;
                }
            default:
                {
                    await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                    fileName = "";
                    break;
                }
        }
        return fileName;
    }

    #endregion

    #region GetReportWithRows

    /// <summary>
    /// Получение отчёта вместе со строчками из основной БД (снимок AsNoTracking в память).
    /// </summary>
    /// <param name="repId">Id отчёта.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отчёт вместе со строчками.</returns>
    private static async Task<Report> GetReportWithRows(int repId, CancellationTokenSource cts)
    {
        await using var db = new DBModel(StaticConfiguration.DBPath);
        var rep = await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(rep => rep.Reports).ThenInclude(reps => reps.DBObservable)
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows20)
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows40)
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows50)
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
                .Include(rep => rep.Rows41.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows51.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows52.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows53.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows54.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows55.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows56.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows57.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Notes.OrderBy(note => note.Order))
                .Where(rep => rep.Reports != null && rep.Reports.DBObservable != null)
                .FirstAsync(rep => rep.Id == repId, cts.Token);
        await rep.SortAsync();
        return rep;
    }
        

    #endregion

    #region InitializeExcelPackage

    /// <summary>
    /// Инициализация Excel пакета.
    /// </summary>
    /// <param name="fullPath">Полный путь до .xlsx файла.</param>
    /// <param name="rep">Отчёт.</param>
    /// <returns>Пакет Excel.</returns>
    private static Task<ExcelPackage> InitializeExcelPackage(string fullPath, Report rep)
    {
#if DEBUG
        var appFolderPath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Excel", $"{rep.FormNum_DB}.xlsx");
#else
        var appFolderPath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Excel", $"{rep.FormNum_DB}.xlsx");
#endif
        if (!File.Exists(appFolderPath))
        {
            throw new FileNotFoundException($"Шаблон Excel не найден: {appFolderPath}");
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(appFolderPath));

        var (titleName, mainName) = GetPrintWorksheetNames(rep.FormNum_DB);
        var worksheetTitle = excelPackage.Workbook.Worksheets[titleName]
            ?? throw new InvalidOperationException($"В шаблоне не найден лист «{titleName}» ({appFolderPath}).");
        _ = excelPackage.Workbook.Worksheets[mainName]
            ?? throw new InvalidOperationException($"В шаблоне не найден лист «{mainName}» ({appFolderPath}).");

        worksheetTitle.Cells.Style.ShrinkToFit = true;
        return Task.FromResult(excelPackage);
    }

    #endregion
}