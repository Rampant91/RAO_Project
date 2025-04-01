using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Проверка наличия формы 2.2.
/// </summary>
public partial class ExcelExportBalance22AsyncCommand : ExcelExportBaseAllAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;
        var cts = new CancellationTokenSource();

        var reportYear = DateTime.Now.Year - 1;
        var selectedReports = (Reports?)mainWindow?.SelectedReports?.FirstOrDefault();
        ExportType = $"Отсутствие_2.2";

        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Определение имени файла");
        var fileName = await GetFileName(selectedReports!);

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения");
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД", "Выгрузка форм", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Это БД с формами 1.0 - выбрать БД с формами 2.0", IsDefault = true },
                        new ButtonDefinition { Name = "Это БД с формами 2.0 - выбрать БД с формами 1.0", IsDefault = true },
                        new ButtonDefinition { Name = "Это БД с формами 1.0 и 2.0", IsDefault = true },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                ],
                CanResize = true,
                ContentTitle = "Проверка формы",
                ContentHeader = "Ошибка",
                ContentMessage = "Пожалуйста, уточните содержание текущей БД и по желанию" +
                                 $"{Environment.NewLine}подгрузите второй файл БД с недостающими формами.",
                MinWidth = 400,
                MinHeight = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(mainWindow));
        if (answer is "Отмена")
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            return;
        }
        string dbExtraPath;
        if (answer is "Это БД с формами 1.0 - выбрать БД с формами 2.0" or "Это БД с формами 2.0 - выбрать БД с формами 1.0")
        {
            OpenFileDialog dial = new() { AllowMultiple = false };
            var filter = new FileDialogFilter
            {
                Extensions = { "RAODB" }
            };
            dial.Filters = [filter];
            string[]? dbWithForm1 = null;
            dbWithForm1 = await dial.ShowAsync(mainWindow!);
            if (dbWithForm1 is null)
            {
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                return;
            }
            dbExtraPath = dbWithForm1![0];
        }
        else
        {
            dbExtraPath = tmpDbPath;
        }
        var DBswap = answer is "Это БД с формами 2.0 - выбрать БД с формами 1.0";
        if (DBswap)
        {
            string DbPathPH = tmpDbPath;
            tmpDbPath = dbExtraPath;
            dbExtraPath = DbPathPH;
        }
        await using var db = new DBModel(tmpDbPath);
        await using var db2 = new DBModel(dbExtraPath);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath, progressBar, cts);

        progressBarVM.SetProgressBar(20, "Заполнение заголовков");
        await FillExcelHeaders();


        progressBarVM.SetProgressBar(25, "Загрузка форм");
        await GetReportRowsAndFillExcel(db, progressBarVM, cts);

        progressBarVM.SetProgressBar(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

        progressBarVM.SetProgressBar(98, "Очистка временных данных");
        try
        {
            File.Delete(DBswap ? dbExtraPath : tmpDbPath);
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
    /// <param name="selectedReports">Выбранная организация.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckRepsAndRepPresence(DBModel db, Reports? selectedReports,
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var isAnyRepWithValidFormNums = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Where(x => x.DBObservable != null)
            .Any(reps => reps.Report_Collection
                .Any(rep => rep.FormNum_DB == "1.5" || rep.FormNum_DB == "1.6" || rep.FormNum_DB == "1.7" || rep.FormNum_DB == "1.8" || rep.FormNum_DB == "2.2"));
        if (!isAnyRepWithValidFormNums)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Не удалось совершить выгрузку," +
                        $"{Environment.NewLine}поскольку в текущей базе не было найдено ни одной формы 1.5-1.8.",
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

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков Excel.
    /// </summary>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <returns>Успешно выполненная Task.</returns>
    private Task FillExcelHeaders()
    {
        /*int masterHeaderLength;
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
        }*/
        return Task.CompletedTask;
    }

    #endregion

    #region GetFileName

    /// <summary>
    /// Формирование имени файла.
    /// </summary>
    /// <param name="selectedReports">Выбранная организация.</param>
    /// <returns>Имя файла.</returns>
    private Task<string> GetFileName(Reports selectedReports)
    {
        string fileName;
        ExportType = $"Отсутствие_2.2";
        fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
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
    private static async Task<Report?> GetReportWithRows(int repId, DBModel dbReadOnly, CancellationTokenSource cts)
    {
        return await dbReadOnly.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(rep => rep.Rows15.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows16.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows17.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows18.OrderBy(form => form.NumberInOrder_DB))
                .FirstOrDefaultAsync(rep => rep.Id == repId, cts.Token);
    }

    #endregion
    #region GetReportWithRows22

    /// <summary>
    /// Получение отчёта вместе со строчками из БД.
    /// </summary>
    /// <param name="repId">Id отчёта.</param>
    /// <param name="dbReadOnly">Модель временной БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отчёт вместе со строчками.</returns>
    private static async Task<Report?> GetReportWithRows22(int repId, DBModel dbReadOnly, CancellationTokenSource cts)
    {
        return await dbReadOnly.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(rep => rep.Rows22.OrderBy(form => form.NumberInOrder_DB))
                .FirstOrDefaultAsync(rep => rep.Id == repId, cts.Token);
    }

    #endregion

    #region GetReportsList

    /// <summary>
    /// Формирование списка организаций без строчек форм отчётности.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="db2">Модель БД с формами 2.2.</param>
    /// <param name="reportYear">Год, для которого выполняется анализ</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список организаций без строчек форм отчётности.</returns>
    private static async Task<List<Reports>> GetReportsListExpected(DBModel db, DBModel db2, int reportYear,
        CancellationTokenSource cts)
    {
        var repsList = new List<Reports>();
        var repYearStr = reportYear.ToString();
        var repYearPrevStr = (reportYear - 1).ToString();
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
                                .Any(rep =>
                                ((rep.FormNum_DB == "1.5"
                                || rep.FormNum_DB == "1.6"
                                || rep.FormNum_DB == "1.7"
                                || rep.FormNum_DB == "1.8")
                                && (rep.EndPeriod_DB.Length >= 4 && rep.EndPeriod_DB.Substring(rep.EndPeriod_DB.Length - 4) == repYearStr))
                                ))
                .ToListAsync(cts.Token));
        repsList.AddRange(
            await db2.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(reports => reports.Report_Collection)
                .Where(reps => reps.DBObservable != null
                            && reps.Report_Collection
                                .Any(rep => (rep.FormNum_DB == "2.2" && rep.Year_DB == repYearPrevStr)
                                ))
                .ToListAsync(cts.Token));
        return repsList;
    }

    #endregion

    #region GetForm22Existing

    /// <summary>
    /// Формирование списка организаций без строчек форм отчётности.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="reportYear">Год, для которого выполняется анализ</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список организаций без строчек форм отчётности.</returns>
    private static async Task<List<Reports>> GetForm22Existing(DBModel db, int reportYear,
        CancellationTokenSource cts)
    {
        var repsList = new List<Reports>();
        var repYearStr = (reportYear - 1).ToString();
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
                                .Any(rep =>
                                rep.FormNum_DB == "2.2" && rep.Year_DB == repYearStr
                                ))
                .ToListAsync(cts.Token));
        return repsList;
    }

    #endregion

    #region GetReportRowsAndFillExcel

    /// <summary>
    /// Загрузка из БД строчек форм отчётности для всех организаций из списка.
    /// </summary>
    /// <param name="repsList">Список организаций.</param>
    /// <param name="db">Модель БД.</param>
    /// <param name="db2">Модель БД с формами 2.2.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private async Task GetReportRowsAndFillExcel(DBModel db, AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var orgList = new List<Reports>();
        orgList.AddRange(
            await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .ToListAsync(cts.Token));
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
    private async Task<ExcelPackage> InitializeExcelPackage(string fullPath, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
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
        Worksheet = excelPackage.Workbook.Worksheets.Add($"Отсутствие 2.2");
        Worksheet.View.FreezePanes(2, 1);
        return excelPackage;
    }

    #endregion
}