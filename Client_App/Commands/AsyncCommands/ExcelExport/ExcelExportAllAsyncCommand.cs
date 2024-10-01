using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.ViewModels;
using Client_App.Views;
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
public class ExcelExportAllAsyncCommand : ExcelExportBaseAllAsyncCommand
{
    private AnyTaskProgressBar progressBar;

    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        IsSelectedOrg = parameter is "SelectedOrg";
        string fileName;
        var mainWindow = Desktop.MainWindow as MainWindow;

        await Dispatcher.UIThread.InvokeAsync(() => progressBar = new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        progressBarVM.ExportType = "Выгрузка в .xlsx";
        progressBarVM.ExportName = "Выгрузка всех отчётов";
        progressBarVM.ValueBar = 2;
        var loadStatus = "Создание временной БД";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tmpDbPath = CreateTempDataBase();
        await using var dbReadOnly = new DBModel(tmpDbPath);

        #region CountReports
        
        var countReports = await dbReadOnly.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Where(x => x.DBObservableId != null)
            .CountAsync(cancellationToken: cts.Token);
        switch (countReports)
        { 
            case 0:
            {
                #region MessageExcelExportFail

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Выгрузка не выполнена, поскольку в базе отсутствуют формы отчетности.",
                        MinHeight = 150,
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(mainWindow));

                #endregion

                await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
                return;
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
                    }).ShowDialog(Desktop.MainWindow));

                #endregion

                if (answer is not "Да")
                {
                    await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
                    return;
                }

                break;
            }
        }

        #endregion

        if (IsSelectedOrg)
        {
            var selectedReports = (Reports?)mainWindow?.SelectedReports?.FirstOrDefault();
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
                        ContentTitle = "Выгрузка в Excel",
                        ContentHeader = "Уведомление",
                        ContentMessage = msg,
                        MinHeight = 150,
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(mainWindow));

                #endregion

                return;
            }
            CurrentReports = selectedReports;
            ExportType = "Выбранная_организация_Все_формы";

            progressBarVM.ExportName = $"Выгрузка всех отчётов " +
                                       $"{selectedReports.Master.RegNoRep.Value}_{selectedReports.Master.OkpoRep.Value}";
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            var regNum = RemoveForbiddenChars(CurrentReports.Master_DB.RegNoRep.Value);
            var okpo = RemoveForbiddenChars(CurrentReports.Master_DB.OkpoRep.Value);
            fileName = $"{ExportType}_{regNum}_{okpo}_{Assembly.GetExecutingAssembly().GetName().Version}";
        }
        else
        {
            ExportType = "Все_формы";
            fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        var operationStart = DateTime.Now;

        loadStatus = "Загрузка списка отчётов";
        progressBarVM.ValueBar = 8;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;

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
                        .ToArrayAsync(cancellationToken: cts.Token));
        }

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
            Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}");
            WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}");
            FillHeaders(formNum);
        }

        loadStatus = "Загрузка форм";
        progressBarVM.ValueBar = 10;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var reps in repsList.OrderBy(x => x.Master_DB.RegNoRep.Value))
        {
            progressBarVM.ExportName = $"Загрузка отчётов {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}";
            var repsWithRows = new Reports { Master = reps.Master };

            foreach (var rep in reps.Report_Collection
                         .OrderBy(x => x.FormNum_DB)
                         .ThenBy(x => DateOnly.TryParse(x.StartPeriod_DB, out _)))
            {
                loadStatus = $"Загрузка отчёта {rep.FormNum_DB}_{rep.StartPeriod_DB}_{rep.EndPeriod_DB}";
                progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

                var repWithRows = await GetReportWithRows(rep.Id, dbReadOnly);

                repsWithRows.Report_Collection.Add(repWithRows);

                progressBarDoubleValue += (double)85 / (repsList.Count * reps.Report_Collection.Count);
                progressBarVM.ValueBar = (int)Math.Floor(progressBarDoubleValue);
                progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
            }
            foreach (var formNum in formNums)
            {
                CurrentReports = repsWithRows;
                Worksheet = excelPackage.Workbook.Worksheets[$"Отчеты {formNum}"];
                WorksheetPrim = excelPackage.Workbook.Worksheets[$"Примечания {formNum}"];
                CurrentRow = Worksheet.Dimension.End.Row + 1;
                CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
                FillExportForms(formNum);
            }
        }

        loadStatus = "Сохранение";
        progressBarVM.ValueBar = 95;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var operationEnd = DateTime.Now;
        var diffInSeconds = (int)(operationEnd - operationStart).TotalSeconds;

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts);

        loadStatus = "Удаление временных файлов";
        progressBarVM.ValueBar = 98;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        if (File.Exists(tmpDbPath))
        {
            try
            {
                File.Delete(tmpDbPath);
            }
            catch (Exception ex)
            {
                var msg = $"{Environment.NewLine}Message: {ex.Message}" + 
                          $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
            }
        }

        #region MessageExcelExportFail

        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                CanResize = true,
                ContentTitle = "Выгрузка в Excel",
                ContentHeader = "Уведомление",
                ContentMessage = $"Время выгрузки составило {diffInSeconds} секунд.",
                MinHeight = 150,
                MinWidth = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .Show(mainWindow));

        #endregion

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }

    #region GetReportWithRows
    
    /// <summary>
    /// Получение отчёта вместе со строчками из БД.
    /// </summary>
    /// <param name="repId">Id отчёта.</param>
    /// <param name="dbReadOnly">Модель временной БД.</param>
    /// <returns>Отчёт вместе со строчками.</returns>
    private static async Task<Report> GetReportWithRows(int repId, DBModel dbReadOnly)
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
                .FirstAsync(rep => rep.Id == repId);
    }

    #endregion
}