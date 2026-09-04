using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Properties;
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
using Client_App.ViewModels.ProgressBar;

using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Выгрузить только титульный лист
/// </summary>
public class ExcelExportTitleListAsyncCommand : ExcelBaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public ExcelExportTitleListAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
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

    public override bool CanExecute(object? parameter) => _formsTabControlVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not Reports reports) return;

        var cts = new CancellationTokenSource();
        ExportType = "Для_печати";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Определение имени файла");
        var fileName = await GetFileName(reports, progressBar, cts);

        progressBarVM.SetProgressBar(10, "Запрос пути сохранения");
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(15, "Создание временной БД", "Выгрузка отчёта для печати", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);

        progressBarVM.SetProgressBar(70, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath, reports.Master_DB);


        progressBarVM.SetProgressBar(80, "Выгрузка данных");
        await FillExcel(excelPackage, reports);

        progressBarVM.SetProgressBar(90, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

        progressBarVM.SetProgressBar(95, "Очистка временных данных");
        try
        {
            File.Delete(tmpDbPath);
        }
        catch
        {
            // ignored
        }

        progressBarVM.SetProgressBar(100, "Завершение выгрузки");
        GC.Collect();
        await progressBar.CloseAsync();
    }

    /// <summary>
    /// Выгрузка отчёта в Excel для печати (перегрузка для пакетной обработки без диалогов).
    /// </summary>
    /// <param name="report">Отчёт для выгрузки.</param>
    /// <param name="destinationFolder">Папка назначения.</param>
    /// <param name="suppressDialogs">Подавлять ли диалоги (true для пакетной обработки).</param>
    public async Task AsyncExecute(Reports reports, string destinationFolder, bool suppressDialogs = false)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Для_печати";

        AnyTaskProgressBar? progressBar = null;
        AnyTaskProgressBarVM? progressBarVM = null;

        if (!suppressDialogs)
        {
            progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
            progressBarVM = progressBar.AnyTaskProgressBarVM;
            progressBarVM.SetProgressBar(5, "Определение имени файла");
        }

        try
        {
            var fileName = await GetFileName(reports, progressBar, cts);
            var fullPath = Path.Combine(destinationFolder, fileName + ".xlsx");

            // Проверяем существование файла и генерируем уникальное имя при необходимости
            var originalFullPath = fullPath;
            var counter = 1;
            while (File.Exists(fullPath))
            {
                var fileNameWithoutExt = fileName + $"_{counter}";
                fullPath = Path.Combine(destinationFolder, fileNameWithoutExt + ".xlsx");
                counter++;
            }

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(10, "Запрос пути сохранения");
            }

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(15, "Создание временной БД", "Выгрузка отчёта для печати", ExportType);
            }
            var tmpDbPath = await CreateTempDataBase(progressBar, cts);

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(30, "Загрузка отчёта");
            }

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(70, "Инициализация Excel пакета");
            }
            using var excelPackage = await InitializeExcelPackage(fullPath, reports.Master_DB);

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(80, "Выгрузка данных");
            }
            await FillExcel(excelPackage, reports);

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(90, "Сохранение");
            }
            
            // Для пакетной обработки не показываем финальный диалог
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp: false, cts, progressBar, isBackground: suppressDialogs);

            // Очистка временных данных
            try
            {
                File.Delete(tmpDbPath);
            }
            catch
            {
                // ignored
            }

            if (!suppressDialogs && progressBarVM != null)
            {
                progressBarVM.SetProgressBar(100, "Завершение выгрузки");
            }
        }
        finally
        {
            if (!suppressDialogs && progressBar != null)
            {
                await progressBar.CloseAsync();
            }
            GC.Collect();
        }
    }

    #region CheckForm

    private static async Task CheckForm(Report exportReport, CancellationTokenSource cts, AnyTaskProgressBar progressBar)
    {
        var errorList = new List<CheckError>();
        try
        {
            errorList.Add(exportReport.FormNum_DB switch
            {
                "1.1" => CheckF11.Check_Total(exportReport.Reports, exportReport),
                "1.2" => CheckF12.Check_Total(exportReport.Reports, exportReport),
                "1.3" => CheckF13.Check_Total(exportReport.Reports, exportReport),
                "1.4" => CheckF14.Check_Total(exportReport.Reports, exportReport),
                "1.5" => CheckF15.Check_Total(exportReport.Reports, exportReport),
                "1.6" => CheckF16.Check_Total(exportReport.Reports, exportReport),
                "1.7" => CheckF17.Check_Total(exportReport.Reports, exportReport),
                "1.8" => CheckF18.Check_Total(exportReport.Reports, exportReport),
                //"2.1" => await new CheckF21().AsyncExecute(exportReport),
                //"2.2" => await new CheckF22().AsyncExecute(exportReport),
                //"2.3" => await new CheckF23().AsyncExecute(exportReport),
                //"2.4" => await new CheckF24().AsyncExecute(exportReport),
                //"2.5" => await new CheckF25().AsyncExecute(exportReport),
                //"2.6" => await new CheckF26().AsyncExecute(exportReport),
                //"2.7" => await new CheckF27().AsyncExecute(exportReport),
                //"2.8" => await new CheckF28().AsyncExecute(exportReport),
                //"2.9" => await new CheckF29().AsyncExecute(exportReport),
                //"2.10" => await new CheckF210().AsyncExecute(exportReport),
                //"2.11" => await new CheckF211().AsyncExecute(exportReport),
                _ => []
            });
        }
        catch (Exception ex)
        {
            //ignored
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

            var answer = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
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
    private static Task FillExcel(ExcelPackage excelPackage, Reports reps)
    {
        var worksheetTitle = excelPackage.Workbook.Worksheets[0];


        ExcelPrintTitleExport(reps.Master_DB.FormNum_DB, worksheetTitle, null, reps.Master_DB);

        var list = excelPackage.Workbook.Worksheets.ToList();
        while(excelPackage.Workbook.Worksheets.Count>1)
            excelPackage.Workbook.Worksheets.Delete(1);


        return Task.CompletedTask;
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
    private async Task<string> GetFileName(Reports reps, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        string formNum;

        string regNum = "";
        string okpo = "";

        formNum = RemoveForbiddenChars(reps.Master.FormNum_DB);

        string fileName = $"_{formNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";

        if (reps.Master.RegNoRep != null)
            regNum = RemoveForbiddenChars(reps.Master.RegNoRep.Value);

        if (reps.Master.OkpoRep != null)
            okpo = RemoveForbiddenChars(reps.Master.OkpoRep.Value);


        if (formNum[0] is '1' or '2')
            fileName = $"{regNum}_{okpo}_{formNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";
        else if (formNum[0] is '4')
        {
            string codeSubjectRF = reps.Master_DB.Rows40[0].CodeSubjectRF_DB;
            fileName = $"{codeSubjectRF}_{formNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";
        }

        return fileName;
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
        string formNumSample = rep.FormNum_DB;

        //Если мы хотим выгрузить только титульник, то берем любой шаблон с соответствующим титульником
        if (formNumSample.Split('.')[1] == "0")
            formNumSample = formNumSample.Split('.')[0] + ".1";

#if DEBUG
        var appFolderPath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Excel", $"{formNumSample}.xlsx");
#else
        var appFolderPath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Excel", $"{formNumSample}.xlsx");
#endif
        if (!File.Exists(appFolderPath))
        {
            throw new FileNotFoundException($"Шаблон Excel не найден: {appFolderPath}");
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(appFolderPath));

        var strTitle = $"{rep.FormNum_DB.Split('.')[0]}.0";
        if (strTitle is "3.0" or "4.0" or "5.0")
            strTitle = "Форма " + $"{strTitle}";
        var worksheetTitle = excelPackage.Workbook.Worksheets[strTitle];
        worksheetTitle.Cells.Style.ShrinkToFit = true;

        if (rep.FormNum_DB.Split('.')[1] != "0")
        {
            var strMain = rep.FormNum_DB;
            if (strMain is "4.1" or "5.1" or "5.2" or "5.3" or "5.4" or "5.5" or "5.6" or "5.7")
                strMain = "Форма " + $"{strMain}";
            var worksheetMain = excelPackage.Workbook.Worksheets[strMain];

            worksheetMain.Cells.Style.ShrinkToFit = true;
        }
        return Task.FromResult(excelPackage);
    }

    #endregion
}