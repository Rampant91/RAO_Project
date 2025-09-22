using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using DynamicData;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
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
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> forms) return;
        var repParam = (Report)forms.First();
        var repId = repParam.Id;

        var cts = new CancellationTokenSource();
        ExportType = "Для_печати";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Определение имени файла");
        var fileName = await GetFileName(repParam, progressBar, cts);

        progressBarVM.SetProgressBar(10, "Запрос пути сохранения");
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(15, "Создание временной БД", "Выгрузка отчёта для печати", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);

        progressBarVM.SetProgressBar(30, "Загрузка отчёта");
        var rep = await GetReportWithRows(repId, tmpDbPath, cts);

        progressBarVM.SetProgressBar(70, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath, rep);

        progressBarVM.SetProgressBar(75, "Проверка отчёта");
        await CheckForm(rep, cts, progressBar);

        progressBarVM.SetProgressBar(80, "Выгрузка данных");
        await FillExcel(excelPackage, rep);

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

        if (errorList.Any(x => x.IsCritical))
        {
            #region ExportTerminatedDueToCriticalErrors

            await Dispatcher.UIThread.InvokeAsync(() =>
                MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в .RAODB",
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
        var worksheetTitle = excelPackage.Workbook.Worksheets[0];
        var worksheetMain = excelPackage.Workbook.Worksheets[1];

        ExcelPrintTitleExport(rep.FormNum_DB, worksheetTitle, rep, rep.Reports.Master);
        ExcelPrintSubMainExport(rep.FormNum_DB, worksheetMain, rep);
        ExcelPrintNotesExport(rep.FormNum_DB, worksheetMain, rep);
        ExcelPrintRowsExport(rep.FormNum_DB, worksheetMain, rep);
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
    private async Task<string> GetFileName(Report rep, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var formNum = RemoveForbiddenChars(rep.FormNum_DB);
        var regNum = RemoveForbiddenChars(rep.Reports.Master.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(rep.Reports.Master.OkpoRep.Value);
        var corNum = Convert.ToString(rep.CorrectionNumber_DB);
        string fileName;
        switch (formNum[0])
        {
            case '1':
                var startPeriod = RemoveForbiddenChars(rep.StartPeriod_DB);
                var endPeriod = RemoveForbiddenChars(rep.EndPeriod_DB);
                fileName = $"{regNum}_{okpo}_{formNum}_{startPeriod}_{endPeriod}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";
                break;
            case '2':
                var year = RemoveForbiddenChars(rep.Year_DB);
                fileName = $"{regNum}_{okpo}_{formNum}_{year}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}_{ExportType}";
                break;
            default:
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                fileName = "";
                break;
        }
        return fileName;
    }

    #endregion

    #region GetReportWithRows

    /// <summary>
    /// Получение отчёта вместе со строчками из БД.
    /// </summary>
    /// <param name="repId">Id отчёта.</param>
    /// <param name="dbPath">Полный путь к временной БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отчёт вместе со строчками.</returns>
    private static async Task<Report> GetReportWithRows(int repId, string dbPath, CancellationTokenSource cts)
    {
        await using var db = new DBModel(dbPath);
        var rep = await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(rep => rep.Reports).ThenInclude(reps => reps.DBObservable)
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows20)
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

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(appFolderPath));
        var worksheetTitle = excelPackage.Workbook.Worksheets[$"{rep.FormNum_DB.Split('.')[0]}.0"];
        var worksheetMain = excelPackage.Workbook.Worksheets[rep.FormNum_DB];
        worksheetTitle.Cells.Style.ShrinkToFit = true;
        worksheetMain.Cells.Style.ShrinkToFit = true;
        return Task.FromResult(excelPackage);
    }

    #endregion
}