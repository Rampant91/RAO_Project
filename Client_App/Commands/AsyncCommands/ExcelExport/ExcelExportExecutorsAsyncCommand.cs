using System;
using System.IO;
using System.Linq;
using Avalonia.Threading;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;
using System.Reflection;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Список исполнителей.
/// </summary>
public class ExcelExportExecutorsAsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
    private Reports CurrentReports;
    private int _currentRow;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список_исполнителей";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", 
            "Выгрузка списка исполнителей", "Выгрузка в .xlsx");
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;

        progressBarVM.SetProgressBar(3, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = !isBackgroundCommand
            ? await ExcelGetFullPath(fileName, cts, progressBar)
            : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

        progressBarVM.SetProgressBar(5, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Подсчёт количества организаций");
        await CountReports(db, progressBar, cts);

        var count = 0;
        while (File.Exists(fullPath))
        {
            fullPath = Path.Combine(folderPath, fileName + $"_{++count}.xlsx");
        }

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(15, "Обработка форм 1");
        await GetExecutorsList1(db, excelPackage, cts);

        progressBarVM.SetProgressBar(55, "Обработка форм 2");
        await GetExecutorsList2(db, excelPackage, cts);

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
    /// Подсчёт количества организаций. При = 0 выводит сообщение и завершает операцию.
    /// </summary>
    /// <param name="db">Модель временной БД.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CountReports(DBModel db, AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        var countReports = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Where(x => x.DBObservable != null)
            .CountAsync(cts.Token);

        if (countReports == 0)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Выгрузка не выполнена, поскольку в базе отсутствуют формы отчетности организаций.",
                    MinHeight = 150,
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion

    #region FillExecutorsHeaders

    /// <summary>
    /// Заполняет заголовки в выгрузке в .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы (1 - оперативная, 2 - годовая).</param>
    private Task FillExecutorsHeaders(char formNum)
    {
        switch (formNum)
        {
            case '1':
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Форма";
                Worksheet.Cells[1, 5].Value = "Дата начала периода";
                Worksheet.Cells[1, 6].Value = "Дата конца периода";
                Worksheet.Cells[1, 7].Value = "Номер корректировки";
                Worksheet.Cells[1, 8].Value = "ФИО исполнителя";
                Worksheet.Cells[1, 9].Value = "Должность";
                Worksheet.Cells[1, 10].Value = "Телефон";
                Worksheet.Cells[1, 11].Value = "Электронная почта";
                break;
            case '2':
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Форма";
                Worksheet.Cells[1, 5].Value = "Отчетный год";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "ФИО исполнителя";
                Worksheet.Cells[1, 8].Value = "Должность";
                Worksheet.Cells[1, 9].Value = "Телефон";
                Worksheet.Cells[1, 10].Value = "Электронная почта";
                break;
        }
        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns(); // Под Astra Linux эта команда крашит программу без GDI дров
        }
        Worksheet.View.FreezePanes(2, 1);
        return Task.CompletedTask;
    }

    #endregion

    #region FillExecutors

    /// <summary>
    /// Выгрузка строчек данных в .xlsx.
    /// </summary>
    /// <param name="rep">Отчёт.</param>
    private Task FillExecutors(Report rep)
    {
        switch (rep.FormNum_DB[0])
        {
            case '1':
                Worksheet.Cells[_currentRow, 1].Value = CurrentReports.Master.RegNoRep.Value;
                Worksheet.Cells[_currentRow, 2].Value = CurrentReports.Master.ShortJurLicoRep.Value;
                Worksheet.Cells[_currentRow, 3].Value = CurrentReports.Master.OkpoRep.Value;
                Worksheet.Cells[_currentRow, 4].Value = rep.FormNum_DB;
                Worksheet.Cells[_currentRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, _currentRow, 5);
                Worksheet.Cells[_currentRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, _currentRow, 6);
                Worksheet.Cells[_currentRow, 7].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[_currentRow, 8].Value = rep.FIOexecutor_DB;
                Worksheet.Cells[_currentRow, 9].Value = rep.GradeExecutor_DB;
                Worksheet.Cells[_currentRow, 10].Value = rep.ExecPhone_DB;
                Worksheet.Cells[_currentRow, 11].Value = rep.ExecEmail_DB;
                break;
            case '2':
                Worksheet.Cells[_currentRow, 1].Value = CurrentReports.Master.RegNoRep.Value;
                Worksheet.Cells[_currentRow, 2].Value = CurrentReports.Master.ShortJurLicoRep.Value;
                Worksheet.Cells[_currentRow, 3].Value = CurrentReports.Master.OkpoRep.Value;
                Worksheet.Cells[_currentRow, 4].Value = rep.FormNum_DB;
                Worksheet.Cells[_currentRow, 5].Value = rep.Year_DB;
                Worksheet.Cells[_currentRow, 6].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[_currentRow, 7].Value = rep.FIOexecutor_DB;
                Worksheet.Cells[_currentRow, 8].Value = rep.GradeExecutor_DB;
                Worksheet.Cells[_currentRow, 9].Value = rep.ExecPhone_DB;
                Worksheet.Cells[_currentRow, 10].Value = rep.ExecEmail_DB;
                break;
        }
        return Task.CompletedTask;
    }

    #endregion

    #region GetExecutorsList1

    /// <summary>
    /// Получение списка исполнителей по формам отчётности 1.х.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <param name="cts">Токен.</param>
    private async Task GetExecutorsList1(DBModel db, ExcelPackage excelPackage, CancellationTokenSource cts)
    {
        var repsList =  await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(reports => reports.Report_Collection)
            .Where(x => x.DBObservable != null)
            .ToListAsync(cts.Token);

        _currentRow = 2;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Формы 1");
        await FillExecutorsHeaders('1');
        foreach (var reps in repsList)
        {
            CurrentReports = reps;
            foreach (var rep in CurrentReports.Report_Collection
                         .Where(x => x.FormNum_DB.Split('.')[0] == "1")
                         .OrderBy(x => x.FormNum_DB.Split('.')[1])
                         .ThenByDescending(x => DateOnly.TryParse(x.StartPeriod_DB, out var stPer) 
                             ? stPer 
                             : DateOnly.MaxValue)
                         .ThenByDescending(x => DateOnly.TryParse(x.EndPeriod_DB, out var endPer)
                             ? endPer
                             : DateOnly.MaxValue))
            {
                await FillExecutors(rep);
                _currentRow++;
            }
        }
    }

    #endregion

    #region GetExecutorsList2

    /// <summary>
    /// Получение списка исполнителей по формам отчётности 2.х.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <param name="cts">Токен.</param>
    private async Task GetExecutorsList2(DBModel db, ExcelPackage excelPackage, CancellationTokenSource cts)
    {
        var repsList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(reports => reports.Report_Collection)
            .Where(x => x.DBObservable != null)
            .ToListAsync(cts.Token);

        _currentRow = 2;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Формы 2");
        await FillExecutorsHeaders('2');
        foreach (var reps in repsList)
        {
            CurrentReports = reps;
            foreach (var rep in CurrentReports.Report_Collection
                         .Where(x => x.FormNum_DB.Split('.')[0] == "2")
                         .OrderBy(x => x.FormNum_DB.Split('.')[1])
                         .ThenByDescending(x => DateOnly.TryParse(x.Year_DB, out var year) 
                             ? year 
                             : DateOnly.MaxValue))
            {
                await FillExecutors(rep);
                _currentRow++;
            }
        }
    }

    #endregion
}