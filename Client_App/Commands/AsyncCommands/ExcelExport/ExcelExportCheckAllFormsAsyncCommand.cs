﻿using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System;
using Client_App.ViewModels;
using System.Threading;
using OfficeOpenXml;
using System.IO;
using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using Client_App.Interfaces.Logger;
using Client_App.Views.ProgressBar;
using Models.CheckForm;
using System.Collections.Generic;
using System.Reflection;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.ViewModels.ProgressBar;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Проверяет все формы организации из главного окна и сохраняет в .xlsx
/// </summary>
public class ExcelExportCheckAllFormsAsyncCommand : ExcelBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not IKeyCollection collection) return;
        var par = collection.ToList<Reports>().First();
        var cts = new CancellationTokenSource();
        ExportType = $"Проверка_отчётов_{par.Master_DB.RegNoRep.Value}_{par.Master_DB.OkpoRep.Value}";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения",
            $"Проверка отчётов {par.Master_DB.RegNoRep.Value}_{par.Master_DB.OkpoRep.Value}", ExportType);
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД",
            "Проверка отчётов на ошибки", "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(15, "Загрузка отчётов");
        var reps = await GetReportsWithRows(tmpDbPath, par, progressBarVM, cts);

        progressBarVM.SetProgressBar(45, "Проверка отчётов");
        var errorsList = await CheckReportCollection(reps, progressBarVM);

        progressBarVM.SetProgressBar(55, "Заполнение строчек отчёта");
        var countCheckedRep = await FillExcel(errorsList, progressBarVM);

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

        #region MessageCheckComplete

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager

            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                CanResize = true,
                ContentTitle = "Проверка форм",
                ContentHeader = "Уведомление",
                ContentMessage =
                    $"Проверка форм организации {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value} завершена." +
                    $"{Environment.NewLine}Проверено {countCheckedRep} из {reps.Report_Collection.Count} отчётов.",
                MinWidth = 400,
                MinHeight = 170,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .Show(progressBar ?? Desktop.MainWindow)
        );

        #endregion

        progressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }
    
    #region CheckReportCollection

    /// <summary>
    /// Проверяет каждый отчёт у организации и возвращает словарь из отчётов и списков их ошибок.
    /// </summary>
    /// <param name="reps">Организация.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <returns>Словарь отчётов и списков их ошибок.</returns>
    private static async Task<Dictionary<Report, List<CheckError>?>> CheckReportCollection(Reports reps, AnyTaskProgressBarVM progressBarVM)
    {
        double progressBarDoubleValue = progressBarVM.ValueBar;
        Dictionary<Report, List<CheckError>?> errorsDictionary = [];

        foreach (var rep in reps.Report_Collection
                     .OrderBy(x => x.FormNum_DB)
                     .ThenBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stPer) ? stPer : DateOnly.MaxValue)
                     .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endPer) ? endPer : DateOnly.MaxValue))
        {
            progressBarVM.SetProgressBar($"Проверка отчёта {rep.FormNum_DB} {rep.StartPeriod_DB}-{rep.EndPeriod_DB}");
            List<CheckError>? errorList;
            try
            {
                errorList = rep.FormNum_DB switch
                {
                    "1.1" => CheckF11.Check_Total(rep.Reports, rep),
                    "1.2" => CheckF12.Check_Total(rep.Reports, rep),
                    "1.3" => CheckF13.Check_Total(rep.Reports, rep),
                    "1.4" => CheckF14.Check_Total(rep.Reports, rep),
                    "1.5" => CheckF15.Check_Total(rep.Reports, rep),
                    "1.6" => CheckF16.Check_Total(rep.Reports, rep),
                    "1.7" => CheckF17.Check_Total(rep.Reports, rep),
                    "1.8" => CheckF18.Check_Total(rep.Reports, rep),
                    _ => throw new Exception()
                };
            }
            catch (Exception ex)
            {
                var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                          $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                ServiceExtension.LoggerManager.Warning(msg);

                #region MessageCheckFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            $"В ходе выполнения проверки формы {rep.FormNum_DB} {rep.StartPeriod_DB}-{rep.EndPeriod_DB} " +
                            $"возникла непредвиденная ошибка.",
                        MinWidth = 400,
                        MinHeight = 170,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .Show(Desktop.MainWindow));

                #endregion

                continue;
            }
            errorsDictionary.Add(rep, errorList);
            progressBarDoubleValue += (double)10 / (reps.Report_Collection.Count);
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверка отчёта {rep.FormNum_DB}_{rep.StartPeriod_DB}_{rep.EndPeriod_DB}",
                $"Проверка отчётов {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}");
        }
        return errorsDictionary;
    }

    #endregion

    #region FillExcel

    /// <summary>
    /// Заполняет в один .xlsx файл каждый список ошибок из словаря.
    /// </summary>
    /// <param name="errorsDictionary">Словарь из отчётов и списков ошибок.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <returns>Количество успешно проверенных отчётов.</returns>
    private async Task<int> FillExcel(Dictionary<Report, List<CheckError>?> errorsDictionary, AnyTaskProgressBarVM progressBarVM)
    {
        var currentRow = 2;
        var countCheckedRep = 0;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var (rep, errorList) in errorsDictionary)
        {
            progressBarVM.SetProgressBar($"Сохранение {rep.FormNum_DB}_{rep.StartPeriod_DB}_{rep.EndPeriod_DB}");

            if (errorList is null)
            {
                countCheckedRep++;
                continue;
            }
            var checkFormVM = new CheckFormVM(new ChangeOrCreateVM(rep.FormNum_DB, rep), errorList);
            foreach (var error in checkFormVM.CheckError)
            {
                Worksheet.Cells[currentRow, 1].Value = currentRow - 1;
                Worksheet.Cells[currentRow, 2].Value = rep.Reports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[currentRow, 3].Value = rep.FormNum_DB;
                Worksheet.Cells[currentRow, 4].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, currentRow, 4);
                Worksheet.Cells[currentRow, 5].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, currentRow, 5);
                Worksheet.Cells[currentRow, 6].Value = error.Row;
                Worksheet.Cells[currentRow, 7].Value = error.Column;
                Worksheet.Cells[currentRow, 8].Value = error.Value;
                Worksheet.Cells[currentRow, 9].Value = error.Message;
                currentRow++;
            }
            countCheckedRep++;

            progressBarDoubleValue += (double)40 / errorsDictionary.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue), 
                $"Сохранение {rep.FormNum_DB}_{rep.StartPeriod_DB}_{rep.EndPeriod_DB}");
        }

        await AutoFitColumns();

        return countCheckedRep;
    }

    #region AutoFitColumns

    /// <summary>
    /// Для текущей страницы Excel пакета подбирает ширину колонок и замораживает первую строчку.
    /// </summary>
    private Task AutoFitColumns()
    {
        for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
        {
            if (OperatingSystem.IsWindows()) Worksheet.Column(col).AutoFit();
        }
        Worksheet.View.FreezePanes(2, 1);
        return Task.CompletedTask;
    }

    #endregion

    #endregion

    #region FillExcelHeaders

    private Task FillExcelHeaders(ExcelPackage excelPackage)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add("Проверка всех форм");

        #region FillHeaders

        Worksheet.Cells[1, 1].Value = "№ п/п";
        Worksheet.Cells[1, 2].Value = "Рег.№";
        Worksheet.Cells[1, 3].Value = "№ формы";
        Worksheet.Cells[1, 4].Value = "Дата начала периода";
        Worksheet.Cells[1, 5].Value = "Дата конца периода";
        Worksheet.Cells[1, 6].Value = "№ строки";
        Worksheet.Cells[1, 7].Value = "Графа";
        Worksheet.Cells[1, 8].Value = "Значение";
        Worksheet.Cells[1, 9].Value = "Сообщение";

        #endregion

        return Task.CompletedTask;
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
                .FirstAsync(rep => rep.Id == repId, cts.Token);
    }

    #endregion

    #region GetReportsWithRows

    /// <summary>
    /// Загружает из БД все отчёты вместе со строчками форм.
    /// </summary>
    /// <param name="tmpDbPath">Путь к временному файлу БД.</param>
    /// <param name="repsWithOutRows">Организация вместе с коллекцией отчётов без строчек форм.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Организацию вместе с коллекцией отчётов со строчками форм.</returns>
    private static async Task<Reports> GetReportsWithRows(string tmpDbPath, Reports repsWithOutRows, 
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        await using var db = new DBModel(tmpDbPath);
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var repsWithRows = new Reports { Master = repsWithOutRows.Master };
        foreach (var rep in repsWithOutRows.Report_Collection
                     .OrderBy(x => x.FormNum_DB)
                     .ThenBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
                     .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue))
        {
            var repWithRows = await GetReportWithRows(rep.Id, db, cts);
            repsWithRows.Report_Collection.Add(repWithRows);
            progressBarDoubleValue += (double)30 / repsWithOutRows.Report_Collection.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка отчёта {rep.FormNum_DB}_{rep.StartPeriod_DB}_{rep.EndPeriod_DB}",
                $"Загрузка отчётов {repsWithOutRows.Master_DB.RegNoRep.Value}_{repsWithOutRows.Master_DB.OkpoRep.Value}");
        }
        return repsWithRows;
    }

    #endregion
}