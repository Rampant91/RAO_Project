using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views;
using Client_App.Views.ProgressBar;
using DynamicData;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static Client_App.Resources.StaticStringMethods;
using CustomSnkEqualityComparer = Client_App.Resources.CustomComparers.SnkComparers.CustomSnkEqualityComparer;
using CustomSnkRadionuclidsEqualityComparer = Client_App.Resources.CustomComparers.SnkComparers.CustomSnkRadionuclidsEqualityComparer;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Excel -> Проверка инвентаризаций.
/// </summary>
public class ExcelExportCheckInventoriesAsyncCommand : ExcelExportSnkBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var mainWindow = Desktop.MainWindow as MainWindow;
        var formNum = (parameter as string)!;

        progressBarVM.SetProgressBar(5, "Проверка наличия отчётов", 
            $"Проверка инвентаризаций {formNum}", ExportType);
        await CheckRepsAndRepPresence(formNum, progressBar, cts);

        var selectedReports = mainWindow!.SelectedReports.First() as Reports;
        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"Проверка_инвентаризаций_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(6, "Запрос даты формирования СНК");
        var(endSnkDate, snkParams) = await AskSnkEndDate(progressBar, cts);

        progressBarVM.SetProgressBar(8, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(13, "Формирование списка инвентаризационных отчётов");
        var inventoryReportDtoList = await GetInventoryReportDtoList(db, selectedReports.Id, formNum, endSnkDate, cts);

        progressBarVM.SetProgressBar(14, "Проверка наличия инвентаризации");
        await CheckInventoryFormPresence(inventoryReportDtoList, formNum, progressBar, cts);

        progressBarVM.SetProgressBar(15, "Формирование списка операций инвентаризации");
        var (firstSnkDate, inventoryFormsDtoList, inventoryDuplicateErrors) = 
            await GetInventoryFormsDtoList(db, inventoryReportDtoList, formNum, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(16, "Получение списка дат инвентаризаций");
        var inventoryDatesList = await GetInventoryDatesList(inventoryFormsDtoList, endSnkDate);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(19, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, inventoryDatesList, formNum);

        List<int> reportIds = [];
        progressBarVM.SetProgressBar(20, "Загрузка списка отчётов");
        reportIds = await GetReportIds(db, selectedReports.Id, formNum, cts);

        progressBarVM.SetProgressBar(21, "Формирование списка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, reportIds, formNum, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(25, "Загрузка операций перезарядки");
        var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, selectedReports.Id, formNum, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(30, "Загрузка нулевых операций");
        var zeroFormsDtoList = await GetZeroFormsDtoList(db, reportIds, rechargeFormsDtoList, firstSnkDate, endSnkDate, formNum, cts, snkParams);

        progressBarVM.SetProgressBar(35, "Формирование списка учётных единиц");
        var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(formNum, inventoryFormsDtoList, plusMinusFormsDtoList, 
            rechargeFormsDtoList, zeroFormsDtoList);

        progressBarVM.SetProgressBar(40, "Формирование списков СНК и ошибок");
        var (unitInStockByDateDictionary, inventoryErrorsByDateDictionary) = await GetInventoryErrorsAndSnk(uniqueUnitWithAllOperationDictionary, 
            inventoryDatesList, inventoryDuplicateErrors, firstSnkDate, formNum);

        progressBarVM.SetProgressBar(45, "Загрузка и заполнение СНК");
        await FillSnkPages(db, unitInStockByDateDictionary, inventoryFormsDtoList, formNum, excelPackage, progressBarVM, cts);

        progressBarVM.SetProgressBar(75, "Загрузка и заполнение ошибок");
        await FillInventoryErrorsPages(db, inventoryErrorsByDateDictionary, formNum, excelPackage, progressBarVM, cts);

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

    #region CheckInventoryFormPresence

    /// <summary>
    /// Проверяет наличие у организации хотя бы одного отчёта, содержащего форму с кодом операции 10.
    /// При отсутствии таковой выводит сообщение и завершает выполнение команды.
    /// </summary>
    /// <param name="inventoryReportDtoList">Список DTO отчётов по форме 1.1, отсортированный по датам.</param>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckInventoryFormPresence(List<ShortReportDTO> inventoryReportDtoList, string formNum,
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        if (inventoryReportDtoList.Count == 0)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = $"Выгрузка не выполнена, поскольку у организации отсутствуют формы {formNum} с кодом операции 10.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполняет заголовки Excel пакета.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <param name="inventoryDatesList">Список дат инвентаризации вместе с текущей датой/датой введённой пользователем.</param>
    /// <param name="formNum">Номер формы.</param>
    private static async Task FillExcelHeaders(ExcelPackage excelPackage, List<DateOnly> inventoryDatesList, string formNum)
    {
        foreach (var date in inventoryDatesList)
        {
            var snkWorksheet = excelPackage.Workbook.Worksheets.Add($"СНК на {date:dd.MM.yy}");

            await FillSnkTable(snkWorksheet, date, formNum);

            await FillInventoryTable(snkWorksheet, date, formNum);

            await AutoFitColumnsAndFreezeRows(snkWorksheet, 2);

            var errorsWorksheet = excelPackage.Workbook.Worksheets.Add($"Ошибки на {date:dd.MM.yy}");

            await FillErrorsTable(errorsWorksheet, formNum);

            await AutoFitColumnsAndFreezeRows(errorsWorksheet, 1);
        }
    }

    #region AutoFitColumns

    /// <summary>
    /// Для текущей страницы Excel пакета подбирает ширину колонок и замораживает n строчек.
    /// </summary>
    private static Task AutoFitColumnsAndFreezeRows(ExcelWorksheet worksheet, byte numberOfRowsToFreeze)
    {
        for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            if (OperatingSystem.IsWindows()) worksheet.Column(col).AutoFit();
        }
        for (var row = 2; row <= numberOfRowsToFreeze + 1; row++)
        {
            worksheet.View.FreezePanes(row, 1);
        }
        return Task.CompletedTask;
    }

    #endregion

    #region FillErrorsTable

    /// <summary>
    /// Заполняет лист, содержащий таблицу ошибок инвентаризации.
    /// </summary>
    /// <param name="worksheet">Лист Excel пакета.</param>
    /// <param name="formNum">Номер формы.</param>
    private static Task FillErrorsTable(ExcelWorksheet worksheet, string formNum)
    {
        switch (formNum)
        {
            case "1.1":
            {
                worksheet.Cells[1, 1].Value = "№ п/п";
                worksheet.Cells[1, 2].Value = "Описание ошибки";
                worksheet.Cells[1, 3].Value = "Дата начала периода";
                worksheet.Cells[1, 4].Value = "Дата конца периода";
                worksheet.Cells[1, 5].Value = "Номер строки";
                worksheet.Cells[1, 6].Value = "Код операции";
                worksheet.Cells[1, 7].Value = "Дата операции";
                worksheet.Cells[1, 8].Value = "Номер паспорта (сертификата)";
                worksheet.Cells[1, 9].Value = "тип";
                worksheet.Cells[1, 10].Value = "радионуклиды";
                worksheet.Cells[1, 11].Value = "номер";
                worksheet.Cells[1, 12].Value = "количество, шт.";
                worksheet.Cells[1, 13].Value = "суммарная активность, Бк";
                worksheet.Cells[1, 14].Value = "код ОКПО изготовителя";
                worksheet.Cells[1, 15].Value = "дата выпуска";
                worksheet.Cells[1, 16].Value = "категория";
                worksheet.Cells[1, 17].Value = "НСС, мес";
                worksheet.Cells[1, 18].Value = "Номер УКТ";
                break;
            }
            case "1.3":
            {
                worksheet.Cells[1, 1].Value = "№ п/п";
                worksheet.Cells[1, 2].Value = "Описание ошибки";
                worksheet.Cells[1, 3].Value = "Дата начала периода";
                worksheet.Cells[1, 4].Value = "Дата конца периода";
                worksheet.Cells[1, 5].Value = "Номер строки";
                worksheet.Cells[1, 6].Value = "Код операции";
                worksheet.Cells[1, 7].Value = "Дата операции";
                worksheet.Cells[1, 8].Value = "Номер паспорта (сертификата)";
                worksheet.Cells[1, 9].Value = "тип";
                worksheet.Cells[1, 10].Value = "радионуклиды";
                worksheet.Cells[1, 11].Value = "номер";
                worksheet.Cells[1, 12].Value = "активность, Бк";
                worksheet.Cells[1, 13].Value = "код ОКПО изготовителя";
                worksheet.Cells[1, 14].Value = "дата выпуска";
                worksheet.Cells[1, 15].Value = "агрегатное состояние";
                worksheet.Cells[1, 16].Value = "Номер УКТ";
                break;
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region FillInventoryTable

    /// <summary>
    /// Заполняет лист, содержащий таблицу инвентаризации на дату.
    /// </summary>
    /// <param name="worksheet">Лист Excel пакета.</param>
    /// <param name="date">Дата инвентаризации.</param>
    /// <param name="formNum">Номер формы.</param>
    private static Task FillInventoryTable(ExcelWorksheet worksheet, DateOnly date, string formNum)
    {
        switch (formNum)
        {
            case "1.1":
            {
                worksheet.Cells[1, 13, 1, 24].Merge = true;
                worksheet.Cells[1, 13, 1, 24].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 13].Value = $"Инвентаризация на {date.ToShortDateString()}";

                worksheet.Cells[2, 13].Value = "№ п/п";
                worksheet.Cells[2, 14].Value = "Номер паспорта (сертификата)";
                worksheet.Cells[2, 15].Value = "тип";
                worksheet.Cells[2, 16].Value = "радионуклиды";
                worksheet.Cells[2, 17].Value = "номер";
                worksheet.Cells[2, 18].Value = "количество, шт.";
                worksheet.Cells[2, 19].Value = "суммарная активность, Бк";
                worksheet.Cells[2, 20].Value = "код ОКПО изготовителя";
                worksheet.Cells[2, 21].Value = "дата выпуска";
                worksheet.Cells[2, 22].Value = "категория";
                worksheet.Cells[2, 23].Value = "НСС, мес";
                worksheet.Cells[2, 24].Value = "Номер УКТ";
                break;
            }
            case "1.3":
            {
                worksheet.Cells[1, 11, 1, 20].Merge = true;
                worksheet.Cells[1, 11, 1, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 11].Value = $"Инвентаризация на {date.ToShortDateString()}";

                worksheet.Cells[2, 11].Value = "№ п/п";
                worksheet.Cells[2, 12].Value = "Номер паспорта (сертификата)";
                worksheet.Cells[2, 13].Value = "тип";
                worksheet.Cells[2, 14].Value = "радионуклиды";
                worksheet.Cells[2, 15].Value = "номер";
                worksheet.Cells[2, 16].Value = "активность, Бк";
                worksheet.Cells[2, 17].Value = "код ОКПО изготовителя";
                worksheet.Cells[2, 18].Value = "дата выпуска";
                worksheet.Cells[2, 19].Value = "агрегатное состояние";
                worksheet.Cells[2, 20].Value = "Номер УКТ";
                break;
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region FillSnkTable

    /// <summary>
    /// Заполняет лист, содержащий таблицу СНК на дату.
    /// </summary>
    /// <param name="worksheet">Лист Excel пакета.</param>
    /// <param name="date">Дата инвентаризации.</param>
    /// <param name="formNum">Номер формы.</param>
    private static Task FillSnkTable(ExcelWorksheet worksheet, DateOnly date, string formNum)
    {
        switch (formNum)
        {
            case "1.1":
            {
                worksheet.Cells[1, 1, 1, 12].Merge = true;
                worksheet.Cells[1, 1, 1, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1].Value = $"СНК на {date.ToShortDateString()}";

                worksheet.Cells[2, 1].Value = "№ п/п";
                worksheet.Cells[2, 2].Value = "Номер паспорта (сертификата)";
                worksheet.Cells[2, 3].Value = "тип";
                worksheet.Cells[2, 4].Value = "радионуклиды";
                worksheet.Cells[2, 5].Value = "номер";
                worksheet.Cells[2, 6].Value = "количество, шт.";
                worksheet.Cells[2, 7].Value = "суммарная активность, Бк";
                worksheet.Cells[2, 8].Value = "код ОКПО изготовителя";
                worksheet.Cells[2, 9].Value = "дата выпуска";
                worksheet.Cells[2, 10].Value = "категория";
                worksheet.Cells[2, 11].Value = "НСС, мес";
                worksheet.Cells[2, 12].Value = "Номер УКТ";
                break;
            }
            case "1.3":
            {
                worksheet.Cells[1, 1, 1, 10].Merge = true;
                worksheet.Cells[1, 1, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1].Value = $"СНК на {date.ToShortDateString()}";

                worksheet.Cells[2, 1].Value = "№ п/п";
                worksheet.Cells[2, 2].Value = "Номер паспорта (сертификата)";
                worksheet.Cells[2, 3].Value = "тип";
                worksheet.Cells[2, 4].Value = "радионуклиды";
                worksheet.Cells[2, 5].Value = "номер";
                worksheet.Cells[2, 6].Value = "активность, Бк";
                worksheet.Cells[2, 7].Value = "код ОКПО изготовителя";
                worksheet.Cells[2, 8].Value = "дата выпуска";
                worksheet.Cells[2, 9].Value = "агрегатное состояние";
                worksheet.Cells[2, 10].Value = "Номер УКТ";
                break;
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #endregion

    #region FillInventoryErrorsPages

    private static async Task FillInventoryErrorsPages(DBModel db, Dictionary<DateOnly, List<InventoryErrorsShortDto>> inventoryErrorsByDateDictionary, 
        string formNum, ExcelPackage excelPackage, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var (inventoryDate, inventoryErrorsDtoList) in inventoryErrorsByDateDictionary)
        {
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка полных форм ошибок на {inventoryDate}");

            var fullFormsErrorsList = 
                await GetFullFormsErrorsList(db, inventoryErrorsDtoList, inventoryErrorsByDateDictionary.Count, formNum, progressBarVM, cts);

            var errorsWorksheet = excelPackage.Workbook.Worksheets
                .First(x => x.Name == $"Ошибки на {inventoryDate:dd.MM.yy}");

            switch (formNum)
            {
                #region 1.1
                
                case "1.1":
                {
                    var fullFormsErrorsOrderedList = fullFormsErrorsList.Cast<InventoryErrorForm11DTO>()
                        .OrderBy(x => x.PasNum)
                        .ThenBy(x => x.FacNum)
                        .ThenBy(x => x.Type)
                        .ThenBy(x => x.PackNumber)
                        .ThenBy(x => x.Radionuclids)
                        .ThenBy(x => x.Quantity)
                        .ThenBy(x => x.StPer)
                        .ThenBy(x => x.EndPer)
                        .ThenBy(x => x.RowNumber)
                        .ToList();

                    var errorsRow = 2;
                    foreach (var form11 in fullFormsErrorsOrderedList)
                    {
                        errorsWorksheet.Cells[errorsRow, 1].Value = errorsRow - 1;
                        errorsWorksheet.Cells[errorsRow, 2].Value = GetErrorDescriptionByType(form11.ErrorType);
                        errorsWorksheet.Cells[errorsRow, 3].Value = ConvertToExcelDate(form11.StPer.ToShortDateString(), errorsWorksheet, errorsRow, 3);
                        errorsWorksheet.Cells[errorsRow, 4].Value = ConvertToExcelDate(form11.EndPer.ToShortDateString(), errorsWorksheet, errorsRow, 4);
                        errorsWorksheet.Cells[errorsRow, 5].Value = form11.RowNumber;
                        errorsWorksheet.Cells[errorsRow, 6].Value = form11.OpCode;
                        errorsWorksheet.Cells[errorsRow, 7].Value = form11.OpDate;
                        errorsWorksheet.Cells[errorsRow, 8].Value = form11.PasNum;
                        errorsWorksheet.Cells[errorsRow, 9].Value = form11.Type;
                        errorsWorksheet.Cells[errorsRow, 10].Value = form11.Radionuclids;
                        errorsWorksheet.Cells[errorsRow, 11].Value = form11.FacNum;
                        errorsWorksheet.Cells[errorsRow, 12].Value = form11.Quantity;
                        errorsWorksheet.Cells[errorsRow, 13].Value = form11.Activity;
                        errorsWorksheet.Cells[errorsRow, 14].Value = form11.CreatorOKPO;
                        errorsWorksheet.Cells[errorsRow, 15].Value = ConvertToExcelDate(form11.CreationDate, errorsWorksheet, errorsRow, 15);
                        errorsWorksheet.Cells[errorsRow, 16].Value = form11.Category;
                        errorsWorksheet.Cells[errorsRow, 17].Value = form11.SignedServicePeriod;
                        errorsWorksheet.Cells[errorsRow, 18].Value = form11.PackNumber;

                        errorsRow++;
                    }

                    break;
                }

                #endregion

                #region 1.3
                
                case "1.3":
                {
                    var fullFormsErrorsOrderedList = fullFormsErrorsList.Cast<InventoryErrorForm13DTO>()
                        .OrderBy(x => x.PasNum)
                        .ThenBy(x => x.FacNum)
                        .ThenBy(x => x.Type)
                        .ThenBy(x => x.PackNumber)
                        .ThenBy(x => x.Radionuclids)
                        .ThenBy(x => x.StPer)
                        .ThenBy(x => x.EndPer)
                        .ThenBy(x => x.RowNumber)
                        .ToList();

                    var errorsRow = 2;
                    foreach (var form13 in fullFormsErrorsOrderedList)
                    {
                        errorsWorksheet.Cells[errorsRow, 1].Value = errorsRow - 1;
                        errorsWorksheet.Cells[errorsRow, 2].Value = GetErrorDescriptionByType(form13.ErrorType);
                        errorsWorksheet.Cells[errorsRow, 3].Value = ConvertToExcelDate(form13.StPer.ToShortDateString(), errorsWorksheet, errorsRow, 3);
                        errorsWorksheet.Cells[errorsRow, 4].Value = ConvertToExcelDate(form13.EndPer.ToShortDateString(), errorsWorksheet, errorsRow, 4);
                        errorsWorksheet.Cells[errorsRow, 5].Value = form13.RowNumber;
                        errorsWorksheet.Cells[errorsRow, 6].Value = form13.OpCode;
                        errorsWorksheet.Cells[errorsRow, 7].Value = form13.OpDate;
                        errorsWorksheet.Cells[errorsRow, 8].Value = form13.PasNum;
                        errorsWorksheet.Cells[errorsRow, 9].Value = form13.Type;
                        errorsWorksheet.Cells[errorsRow, 10].Value = form13.Radionuclids;
                        errorsWorksheet.Cells[errorsRow, 11].Value = form13.FacNum;
                        errorsWorksheet.Cells[errorsRow, 12].Value = form13.Activity;
                        errorsWorksheet.Cells[errorsRow, 13].Value = form13.CreatorOKPO;
                        errorsWorksheet.Cells[errorsRow, 14].Value = ConvertToExcelDate(form13.CreationDate, errorsWorksheet, errorsRow, 14);
                        errorsWorksheet.Cells[errorsRow, 15].Value = form13.AggregateState;
                        errorsWorksheet.Cells[errorsRow, 16].Value = form13.PackNumber;

                        errorsRow++;
                    }

                    break;
                }

                #endregion
            }

            var errorsTable = errorsWorksheet.Tables
                .Add(errorsWorksheet.Cells[1, 1, errorsWorksheet.Dimension.Rows, errorsWorksheet.Dimension.Columns], $"Ошибки_{inventoryDate}");
            errorsTable.TableStyle = TableStyles.Medium2;
            errorsWorksheet.Cells[1, 1, errorsWorksheet.Dimension.Rows, errorsWorksheet.Dimension.Columns].Style.Border.BorderAround(ExcelBorderStyle.Thick);

            progressBarDoubleValue += (double)20 / inventoryErrorsByDateDictionary.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка полных форм ошибок на {inventoryDate}");
        }
    }

    #endregion

    #region FillSnkPages

    private static async Task FillSnkPages(DBModel db, Dictionary<DateOnly, List<ShortFormDTO>> unitInStockByDateDictionary, 
        List<ShortFormDTO> inventoryFormsDtoList, string formNum, 
        ExcelPackage excelPackage, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var (inventoryDate, unitInStockOnDateDtoList) in unitInStockByDateDictionary)
        {
            var currentInventoryDtoList = inventoryFormsDtoList
                    .Where(x => x.OpDate == inventoryDate)
                    .ToList();

            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка полных форм на {inventoryDate}",
                "Загрузка полных форм");

            var fullFormsSnkList = await GetFullFormsSnkList(db, unitInStockOnDateDtoList, inventoryDate, formNum, progressBarVM, cts);
            var fullFormsInventoryList = await GetFullFormsSnkList(db, currentInventoryDtoList, inventoryDate, formNum, progressBarVM, cts);

            #region SnkTable

            var snkWorksheet = excelPackage.Workbook.Worksheets
                .First(x => x.Name == $"СНК на {inventoryDate:dd.MM.yy}");

            var fullFormsSnkOrderedList = fullFormsSnkList
                .OrderByDescending(x => fullFormsInventoryList
                    .Any(y => x.PasNum == y.PasNum
                              && x.Type == y.Type
                              && x.Radionuclids == y.Radionuclids
                              && x.FacNum == y.FacNum
                              && x.Quantity == y.Quantity
                              && x.PackNumber == y.PackNumber))
                .ThenBy(x => x.PasNum)
                .ThenBy(x => x.FacNum)
                .ThenBy(x => x.Type)
                .ThenBy(x => x.Radionuclids)
                .ThenBy(x => x.Quantity)
                .ThenBy(x => x.PackNumber)
                .ToList();

            var snkRow = 3;
            foreach (var form in fullFormsSnkOrderedList)
            {
                switch (formNum)
                {
                    #region 1.1

                    case "1.1":
                    {
                        var form11 = (SnkForm11DTO)form;

                        snkWorksheet.Cells[snkRow, 1].Value = snkRow - 2;
                        snkWorksheet.Cells[snkRow, 2].Value = form11.PasNum;
                        snkWorksheet.Cells[snkRow, 3].Value = form11.Type;
                        snkWorksheet.Cells[snkRow, 4].Value = form11.Radionuclids;
                        snkWorksheet.Cells[snkRow, 5].Value = form11.FacNum;
                        snkWorksheet.Cells[snkRow, 6].Value = form11.Quantity;
                        snkWorksheet.Cells[snkRow, 7].Value = form11.Activity;
                        snkWorksheet.Cells[snkRow, 8].Value = form11.CreatorOKPO;
                        snkWorksheet.Cells[snkRow, 9].Value = ConvertToExcelDate(form11.CreationDate, snkWorksheet, snkRow, 9);
                        snkWorksheet.Cells[snkRow, 10].Value = form11.Category;
                        snkWorksheet.Cells[snkRow, 11].Value = form11.SignedServicePeriod;
                        snkWorksheet.Cells[snkRow, 12].Value = form11.PackNumber;
                        break;
                    }

                    #endregion

                    #region 1.3

                    case "1.3":
                    {
                        var form13 = (SnkForm13DTO)form;

                        snkWorksheet.Cells[snkRow, 1].Value = snkRow - 2;
                        snkWorksheet.Cells[snkRow, 2].Value = form13.PasNum;
                        snkWorksheet.Cells[snkRow, 3].Value = form13.Type;
                        snkWorksheet.Cells[snkRow, 4].Value = form13.Radionuclids;
                        snkWorksheet.Cells[snkRow, 5].Value = form13.FacNum;
                        snkWorksheet.Cells[snkRow, 6].Value = form13.Activity;
                        snkWorksheet.Cells[snkRow, 7].Value = form13.CreatorOKPO;
                        snkWorksheet.Cells[snkRow, 8].Value = ConvertToExcelDate(form13.CreationDate, snkWorksheet, snkRow, 8);
                        snkWorksheet.Cells[snkRow, 9].Value = form13.AggregateState;
                        snkWorksheet.Cells[snkRow, 10].Value = form13.PackNumber;
                        break;
                    }

                    #endregion
                }

                snkRow++;
            }

            var columnNum = formNum switch
            {
                "1.1" => 12,
                "1.3" => 10,
                _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
            };

            var snkTable = snkWorksheet.Tables.Add(snkWorksheet.Cells[2, 1, snkRow - 1, columnNum], $"СНК_{inventoryDate}");
            snkTable.TableStyle = TableStyles.Medium2;
            snkWorksheet.Cells[1, 1, snkRow - 1, columnNum].Style.Border.BorderAround(ExcelBorderStyle.Thick);

            #endregion

            #region InventoryTable

            var fullFormsInventoryOrderedList = fullFormsInventoryList
                    .OrderByDescending(x => fullFormsSnkList
                        .Any(y => x.PasNum == y.PasNum
                                  && x.Type == y.Type
                                  && x.Radionuclids == y.Radionuclids
                                  && x.FacNum == y.FacNum
                                  && x.Quantity == y.Quantity
                                  && x.PackNumber == y.PackNumber))
                    .ThenBy(x => x.PasNum)
                    .ThenBy(x => x.FacNum)
                    .ThenBy(x => x.Type)
                    .ThenBy(x => x.Radionuclids)
                    .ThenBy(x => x.Quantity)
                    .ThenBy(x => x.PackNumber)
                    .ToList();

            var inventoryRow = 3;
            foreach (var inventoryForm in fullFormsInventoryOrderedList)
            {
                switch (formNum)
                {
                    #region 1.1

                    case "1.1":
                    {
                        var inventoryForm11 = (SnkForm11DTO)inventoryForm;

                        snkWorksheet.Cells[inventoryRow, 13].Value = inventoryRow - 2;
                        snkWorksheet.Cells[inventoryRow, 14].Value = inventoryForm11.PasNum;
                        snkWorksheet.Cells[inventoryRow, 15].Value = inventoryForm11.Type;
                        snkWorksheet.Cells[inventoryRow, 16].Value = inventoryForm11.Radionuclids;
                        snkWorksheet.Cells[inventoryRow, 17].Value = inventoryForm11.FacNum;
                        snkWorksheet.Cells[inventoryRow, 18].Value = inventoryForm11.Quantity;
                        snkWorksheet.Cells[inventoryRow, 19].Value = inventoryForm11.Activity;
                        snkWorksheet.Cells[inventoryRow, 20].Value = inventoryForm11.CreatorOKPO;
                        snkWorksheet.Cells[inventoryRow, 21].Value = ConvertToExcelDate(inventoryForm11.CreationDate, snkWorksheet, inventoryRow, 21);
                        snkWorksheet.Cells[inventoryRow, 22].Value = inventoryForm11.Category;
                        snkWorksheet.Cells[inventoryRow, 23].Value = inventoryForm11.SignedServicePeriod;
                        snkWorksheet.Cells[inventoryRow, 24].Value = inventoryForm11.PackNumber;

                        break;
                    }

                    #endregion

                    #region 1.3

                    case "1.3":
                    {
                        var inventoryForm13 = (SnkForm13DTO)inventoryForm;

                        snkWorksheet.Cells[inventoryRow, 11].Value = inventoryRow - 2;
                        snkWorksheet.Cells[inventoryRow, 12].Value = inventoryForm13.PasNum;
                        snkWorksheet.Cells[inventoryRow, 13].Value = inventoryForm13.Type;
                        snkWorksheet.Cells[inventoryRow, 14].Value = inventoryForm13.Radionuclids;
                        snkWorksheet.Cells[inventoryRow, 15].Value = inventoryForm13.FacNum;
                        snkWorksheet.Cells[inventoryRow, 16].Value = inventoryForm13.Activity;
                        snkWorksheet.Cells[inventoryRow, 17].Value = inventoryForm13.CreatorOKPO;
                        snkWorksheet.Cells[inventoryRow, 18].Value = ConvertToExcelDate(inventoryForm13.CreationDate, snkWorksheet, inventoryRow, 18);
                        snkWorksheet.Cells[inventoryRow, 19].Value = inventoryForm13.AggregateState;
                        snkWorksheet.Cells[inventoryRow, 20].Value = inventoryForm13.PackNumber;

                        break;
                    }

                    #endregion
                }

                inventoryRow++;
            }

            var inventoryTable = snkWorksheet.Tables.Add(snkWorksheet.Cells[2, columnNum + 1, inventoryRow - 1, columnNum * 2], $"Инвентаризация_{inventoryDate}");
            inventoryTable.TableStyle = TableStyles.Medium2;
            snkWorksheet.Cells[1, columnNum + 1, inventoryRow - 1, columnNum * 2].Style.Border.BorderAround(ExcelBorderStyle.Thick);

            #endregion

            #region HighlightMatchesWithColor

            var countMatches = fullFormsSnkOrderedList
                    .Count(x => fullFormsInventoryOrderedList
                        .Any(y => x.PasNum == y.PasNum
                                  && x.Type == y.Type
                                  && x.Radionuclids == y.Radionuclids
                                  && x.FacNum == y.FacNum
                                  && x.Quantity == y.Quantity
                                  && x.PackNumber == y.PackNumber));

            if (countMatches > 0)
            {
                for (var column = 1; column <= columnNum * 2; column++)
                {
                    for (var row = 3; row <= countMatches + 2; row++)
                    {
                        snkWorksheet.Cells[row, column].Style.Fill.SetBackground(System.Drawing.Color.LightGreen, ExcelFillStyle.LightGray);
                    }
                }
            }

            #endregion

            progressBarDoubleValue += (double)30 / unitInStockByDateDictionary.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка полных форм на {inventoryDate}");
        }
    }

    #endregion

    #region GetFullForm

    /// <summary>
    /// Загрузка из БД полных форм вместе с данными отчётов и организации.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="unitInStockDtoList">Список DTO учётных единиц в наличии на дату.</param>
    /// <param name="inventoryDate">Дата инвентаризации, на которую загружаются формы.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список форм с данными отчётов и организации.</returns>
    private static async Task<List<SnkFormDTO>> GetFullFormsSnkList(DBModel db, List<ShortFormDTO> unitInStockDtoList, DateOnly inventoryDate,
        string formNum, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<SnkFormDTO> formsList = [];
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;

        foreach (var unit in unitInStockDtoList)
        {
            SnkFormDTO formDto = formNum switch
            {
                #region 1.1

                "1.1" => await db.form_11
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == unit.Id
                                && x.Report != null
                                && x.Report.Reports != null
                                && x.Report.Reports.DBObservable != null)
                    .Select(x => new SnkForm11DTO(
                        x.FactoryNumber_DB,
                        x.PassportNumber_DB,
                        unit.Quantity,
                        x.Radionuclids_DB,
                        x.Type_DB,
                        x.Activity_DB,
                        x.CreatorOKPO_DB,
                        x.CreationDate_DB,
                        x.Category_DB,
                        x.SignedServicePeriod_DB,
                        x.PackNumber_DB))
                    .FirstAsync(cts.Token),

                #endregion

                #region 1.3

                "1.3" => await db.form_13
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == unit.Id
                                && x.Report != null
                                && x.Report.Reports != null
                                && x.Report.Reports.DBObservable != null)
                    .Select(x => new SnkForm13DTO(
                        x.FactoryNumber_DB,
                        x.PassportNumber_DB,
                        unit.Quantity,
                        x.Radionuclids_DB,
                        x.Type_DB,
                        x.Activity_DB,
                        x.CreatorOKPO_DB,
                        x.CreationDate_DB,
                        x.AggregateState_DB,
                        x.PackNumber_DB))
                    .FirstAsync(cts.Token),

                #endregion

                _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
            };

            formsList.Add(formDto);

            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка {currentUnitNum} формы из {unitInStockDtoList.Count}",
                $"Загрузка форм на {inventoryDate}");
            currentUnitNum++;
        }
        return formsList;

    }

    private static async Task<List<InventoryErrorFormDTO>> GetFullFormsErrorsList(DBModel db, List<InventoryErrorsShortDto> inventoryErrorsDtoList, 
        int datesCount, string formNum, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<InventoryErrorFormDTO> formsList = [];
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;
        foreach (var error in inventoryErrorsDtoList)
        {
            InventoryErrorFormDTO form = formNum switch
            {
                #region 1.1

                "1.1" => await db.form_11
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == error.Dto.Id
                                && x.Report != null
                                && x.Report.Reports != null
                                && x.Report.Reports.DBObservable != null)
                    .Select(x => new InventoryErrorForm11DTO(
                        error.ErrorTypeEnum,
                        error.Dto.RepDto.StartPeriod,
                        error.Dto.RepDto.EndPeriod,
                        x.NumberInOrder_DB,
                        x.FactoryNumber_DB,
                        x.OperationCode_DB,
                        x.OperationDate_DB,
                        x.PassportNumber_DB,
                        error.Dto.Quantity,
                        x.Radionuclids_DB,
                        x.Type_DB,
                        x.Activity_DB,
                        x.CreatorOKPO_DB,
                        x.CreationDate_DB,
                        x.Category_DB,
                        x.SignedServicePeriod_DB,
                        x.PackNumber_DB))
                    .FirstAsync(cts.Token),

                #endregion

                #region 1.3
                
                "1.3" => await db.form_13
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == error.Dto.Id
                                && x.Report != null
                                && x.Report.Reports != null
                                && x.Report.Reports.DBObservable != null)
                    .Select(x => new InventoryErrorForm13DTO(
                        error.ErrorTypeEnum,
                        error.Dto.RepDto.StartPeriod,
                        error.Dto.RepDto.EndPeriod,
                        x.NumberInOrder_DB,
                        x.FactoryNumber_DB,
                        x.OperationCode_DB,
                        x.OperationDate_DB,
                        x.PassportNumber_DB,
                        x.Radionuclids_DB,
                        x.Type_DB,
                        x.Activity_DB,
                        x.CreatorOKPO_DB,
                        x.CreationDate_DB,
                        x.AggregateState_DB,
                        x.PackNumber_DB))
                    .FirstAsync(cts.Token),

                #endregion

                _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
            };

            formsList.Add(form);

            progressBarDoubleValue += (double)25 / inventoryErrorsDtoList.Count / datesCount;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка {currentUnitNum} формы из {inventoryErrorsDtoList.Count}",
                "Загрузка форм");
            currentUnitNum++;
        }
        return formsList;

    }

    #endregion

    #region GetInventoryDatesList

    /// <summary>
    /// Формирование списка операций инвентаризации.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO операций инвентаризации, отсортированный по датам.</param>
    /// <param name="endSnkDate">Дата, по которую нужно проверить инвентаризации.</param>
    /// <returns>Список операций инвентаризации.</returns>
    private static Task<List<DateOnly>> GetInventoryDatesList(List<ShortFormDTO> inventoryFormsDtoList, DateOnly endSnkDate)
    {
        var inventoryDates = inventoryFormsDtoList
            .Select(x => x.OpDate)
            .Concat([endSnkDate])
            .Distinct()
            .Order()
            .ToList();

        return Task.FromResult(inventoryDates);
    }

    #endregion

    #region GetInventoryErrorsAndSnk

    /// <summary>
    /// Формирование СНК на все даты инвентаризаций, а также списков ошибок на эти даты.
    /// </summary>
    /// <param name="uniqueUnitWithAllOperationDictionary"></param>
    /// <param name="inventoryDatesList">Список дат инвентаризации.</param>
    /// <param name="inventoryDuplicateErrorsDtoList"></param>
    /// <param name="primaryInventoryDate">Дата первой инвентаризации начиная с 01.01.2022.</param>
    /// <param name="formNum">Номер формы.</param>
    private static async Task<(Dictionary<DateOnly, List<ShortFormDTO>>, Dictionary<DateOnly, List<InventoryErrorsShortDto>>)> GetInventoryErrorsAndSnk(
        Dictionary<UniqueUnitDto, List<ShortFormDTO>> uniqueUnitWithAllOperationDictionary, List<DateOnly> inventoryDatesList,
        List<ShortFormDTO> inventoryDuplicateErrorsDtoList, DateOnly primaryInventoryDate, string formNum)
    {
        List<ShortFormDTO> unitInStockDtoList = [];

        Dictionary<DateOnly, List<ShortFormDTO>> unitInStockByDateDictionary = [];
        Dictionary<DateOnly, List<InventoryErrorsShortDto>> inventoryErrorsByDateDictionary = [];

        var plusOperationArray = GetPlusOperationsArray(formNum);
        var minusOperationArray = GetMinusOperationsArray(formNum);

        var currentInventoryDateIndex = 0;
        var comparer = new CustomSnkEqualityComparer();
        var radsComparer = new CustomSnkRadionuclidsEqualityComparer();
        foreach (var inventoryDate in inventoryDatesList)
        {
            // Инициализируем список ошибок, сразу добавляя в него повторные операции инвентаризации.
            var errorsDtoList = new List<InventoryErrorsShortDto>
            {
                inventoryDuplicateErrorsDtoList
                    .Where(x => x.OpDate == inventoryDate)
                    .Select(dto => new InventoryErrorsShortDto(InventoryErrorTypeEnum.InventoryDuplicate, dto))
            };

            #region FirstInventory

            // Если есть операции инвентаризации (кроме текущей даты, которая есть всегда), добавляем для первой инвентаризации СНК.
            if (currentInventoryDateIndex == 0)
            {
                //Добавляем в СНК учётные единицы из первой инвентаризации
                unitInStockDtoList.AddRange(uniqueUnitWithAllOperationDictionary
                    .Where(unit => Enumerable
                        .Any(unit.Value, x =>
                            x.OpCode == "10" && x.OpDate == inventoryDate))
                    .Select(unit => unit.Value.First()));

                // Добавляем в словарь СНК текущую дату инвентаризации и СНК на эту дату.
                unitInStockByDateDictionary.Add(inventoryDate, [.. unitInStockDtoList]);

                // Добавляем в словарь ошибок текущую дату и список ошибок на эту дату.
                inventoryErrorsByDateDictionary.Add(inventoryDate, [.. errorsDtoList]);

                currentInventoryDateIndex++;

                continue;
            }

            #endregion

            var previousInventoryDate = inventoryDatesList[currentInventoryDateIndex - 1];

            foreach (var (unit, allOperations) in uniqueUnitWithAllOperationDictionary)
            {
                var currentOperations = allOperations
                    .Where(x => x.OpDate >= previousInventoryDate && x.OpDate <= inventoryDate)
                    .OrderBy(x => x.OpDate)
                    .ThenBy(x => x.RepDto.StartPeriod)
                    .ThenBy(x => x.NumberInOrder)
                    .ToList();

                var secondInventoryOperation = currentOperations
                    .FirstOrDefault(x => x.OpCode == "10" && x.OpDate == inventoryDate);

                #region GetInStock

                #region 1.3 || SerialNumEmpty

                if (formNum is "1.3" || SerialNumbersIsEmpty(unit.PasNum, unit.FacNum))
                {
                    var currentPackNumber = currentOperations.FirstOrDefault()?.PackNumber ?? unit.PackNumber;

                    var currentUnitInStock = unitInStockDtoList
                        .FirstOrDefault(x =>
                            comparer.Equals(x.PasNum, unit.PasNum)
                            && comparer.Equals(x.FacNum, unit.FacNum)
                            && radsComparer.Equals(x.Radionuclids, unit.Radionuclids)
                            && comparer.Equals(x.Type, unit.Type)
                            && comparer.Equals(x.PackNumber, currentPackNumber));

                    var quantity = currentUnitInStock?.Quantity ?? 0;

                    //2. Есть в СНК на первую инвентаризацию, нет второй инвентаризации, нет минусовых операций.
                    if (currentUnitInStock != null
                        && quantity != 0
                        && secondInventoryOperation is null
                        && !currentOperations.Any(x => minusOperationArray.Contains(x.OpCode))
                        && inventoryDate != inventoryDatesList[^1])
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.MissingFromInventoryUnit, currentUnitInStock));
                    }

                    var operationsWithoutDuplicates = await GetOperationsWithoutDuplicates(currentOperations, formNum);

                    var firstPlusMinusOperation = operationsWithoutDuplicates
                        .FirstOrDefault(x => plusOperationArray.Contains(x.OpCode) || minusOperationArray.Contains(x.OpCode));

                    var lastPlusMinusOperation = operationsWithoutDuplicates
                        .LastOrDefault(x => plusOperationArray.Contains(x.OpCode) || minusOperationArray.Contains(x.OpCode));

                    //1. Нет во второй инвентаризации, но последняя +- операция плюсовая.
                    if (secondInventoryOperation is null
                        && lastPlusMinusOperation is not null
                        && plusOperationArray.Contains(lastPlusMinusOperation.OpCode)
                        && inventoryDate != inventoryDatesList[^1])
                    {
                        errorsDtoList
                            .Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.RegisteredAndNotInventoriedUnit, lastPlusMinusOperation));
                    }

                    foreach (var operation in operationsWithoutDuplicates)
                    {
                        if (plusOperationArray.Contains(operation.OpCode))
                        {
                            quantity += operation.Quantity;
                        }
                        else if (minusOperationArray.Contains(operation.OpCode))
                        {
                            //4. Снятие с учёта не стоявшего на учёте ЗРИ.
                            if (quantity == 0)
                            {
                                errorsDtoList
                                    .Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.UnInventoriedUnitGivenAway, firstPlusMinusOperation!));
                            }
                            //9. Для пустых зав.№ и № паспорта, отдано большее количество, чем было на момент операции.
                            else if (quantity < operation.Quantity)
                            {
                                errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.QuantityGivenExceedsAvailable, operation));
                            }
                            else
                            {
                                quantity -= operation.Quantity;
                            }
                        }
                    }

                    var lastOperationWithUnit = currentOperations
                        .OrderByDescending(x => x.OpDate)
                        .FirstOrDefault();

                    if (lastOperationWithUnit == null) continue;

                    //3. Есть во второй инвентаризации, но отсутствует в СНК на дату второй инвентаризации.
                    if (secondInventoryOperation is not null
                        && currentUnitInStock is null 
                        && quantity <= 0)
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.GivenUnitIsInventoried, secondInventoryOperation));
                    }

                    if (currentUnitInStock is not null)
                    {
                        unitInStockDtoList.Remove(currentUnitInStock);
                    }

                    if (quantity > 0)
                    {
                        lastOperationWithUnit.Quantity = quantity;
                        unitInStockDtoList.Add(lastOperationWithUnit);
                    }
                }

                #endregion

                #region SerialNumNotEmpty

                else
                {
                    var operationsWithoutMutuallyExclusive = await GetOperationsWithoutMutuallyCompensating(currentOperations, formNum);
                    var allOperationsWithoutMutuallyExclusive = await GetOperationsWithoutMutuallyCompensating(allOperations, formNum);

                    #region GetErrors

                    var firstPlusMinusOperation = operationsWithoutMutuallyExclusive
                        .FirstOrDefault(x => plusOperationArray.Contains(x.OpCode) || minusOperationArray.Contains(x.OpCode));

                    var lastPlusMinusOperation = operationsWithoutMutuallyExclusive
                        .LastOrDefault(x => plusOperationArray.Contains(x.OpCode) || minusOperationArray.Contains(x.OpCode));

                    //1. Нет во второй инвентаризации, но последняя +- операция плюсовая.
                    if (secondInventoryOperation is null
                        && lastPlusMinusOperation is not null
                        && plusOperationArray.Contains(lastPlusMinusOperation.OpCode)
                        && inventoryDate != inventoryDatesList[^1])
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.RegisteredAndNotInventoriedUnit, lastPlusMinusOperation));
                    }

                    for (var i = 0; i < operationsWithoutMutuallyExclusive.Count; i++)
                    {
                        var currentForm = operationsWithoutMutuallyExclusive[i];
                        var previousPlusMinusOperation = operationsWithoutMutuallyExclusive
                            .GetRange(0, i)
                            .Where(x => plusOperationArray.Contains(x.OpCode) || minusOperationArray.Contains(x.OpCode))
                            .Reverse()
                            .FirstOrDefault();

                        if (minusOperationArray.Contains(currentForm.OpCode)
                            && previousPlusMinusOperation is not null
                            && minusOperationArray.Contains(previousPlusMinusOperation.OpCode))
                        {
                            //5. Двойное снятие с учёта
                            errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.ReDeRegistration, currentForm));
                        }
                        else if (plusOperationArray.Contains(currentForm.OpCode)
                                 && previousPlusMinusOperation is not null 
                                 && plusOperationArray.Contains(previousPlusMinusOperation.OpCode))
                        {
                            //7. Двойная постановка на учёт
                            errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.ReRegistration, currentForm));
                        }
                    }

                    #endregion

                    #region GetInStock

                    var inStock = allOperationsWithoutMutuallyExclusive.Any(x => x.OpCode == "10" && x.OpDate == primaryInventoryDate);
                    var inStockOnPreviousInventoryDate = inStock;
                    
                    foreach (var form in allOperationsWithoutMutuallyExclusive.Where(x => x.OpDate <= previousInventoryDate))
                    {
                        if (plusOperationArray.Contains(form.OpCode)) inStockOnPreviousInventoryDate = true;
                        else if (minusOperationArray.Contains(form.OpCode)) inStockOnPreviousInventoryDate = false;
                    }

                    //2. Есть в СНК на первую инвентаризацию, нет второй инвентаризации, нет минусовых операций.
                    if (inStockOnPreviousInventoryDate
                        && secondInventoryOperation is null
                        && !currentOperations.Any(x => minusOperationArray.Contains(x.OpCode))
                        && inventoryDate != inventoryDatesList[^1])
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(
                            InventoryErrorTypeEnum.MissingFromInventoryUnit,
                            allOperationsWithoutMutuallyExclusive.Last(x => x.OpDate <= inventoryDate)));
                    }

                    //4. Снятие с учёта не стоявшего на учёте ЗРИ.
                    if (!inStockOnPreviousInventoryDate
                        && firstPlusMinusOperation is not null
                        && minusOperationArray.Contains(firstPlusMinusOperation.OpCode))
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.UnInventoriedUnitGivenAway, firstPlusMinusOperation));
                    }

                    //6. Постановка на учёт имеющегося в наличии ЗРИ.
                    if (inStockOnPreviousInventoryDate
                        && firstPlusMinusOperation is not null
                        && plusOperationArray.Contains(firstPlusMinusOperation.OpCode))
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.InventoriedUnitReceived, firstPlusMinusOperation));
                    }

                    foreach (var form in allOperations.Where(x => x.OpDate <= inventoryDate))
                    {
                        if (IsZeroOperation(form, formNum)
                            && !inStock
                            && form.OpDate >= previousInventoryDate
                            && form.OpDate <= inventoryDate
                            && form.OpDate >= primaryInventoryDate)
                        {
                            //8. Нулевые операции с отсутствующим в наличии ЗРИ.
                            errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.ZeroOperationWithUnInventoriedUnit, form));
                        }
                        if (plusOperationArray.Contains(form.OpCode)) inStock = true;
                        else if (minusOperationArray.Contains(form.OpCode)) inStock = false;
                    }

                    var lastOperationWithUnit = operationsWithoutMutuallyExclusive
                        .OrderByDescending(x => x.OpDate)
                        .FirstOrDefault();

                    if (lastOperationWithUnit == null) continue;

                    var currentPackNumber = operationsWithoutMutuallyExclusive.FirstOrDefault()?.PackNumber ?? unit.PackNumber;

                    var unitInStock = unitInStockDtoList.FirstOrDefault(x =>
                        comparer.Equals(x.PasNum, unit.PasNum)
                        && comparer.Equals(x.FacNum, unit.FacNum)
                        && radsComparer.Equals(x.Radionuclids, unit.Radionuclids)
                        && comparer.Equals(x.Type, unit.Type)
                        && comparer.Equals(x.PackNumber, currentPackNumber)
                        && x.Quantity == unit.Quantity);

                    //3. Есть во второй инвентаризации, но отсутствует в СНК на дату второй инвентаризации.
                    if (secondInventoryOperation is not null && !inStock)
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.GivenUnitIsInventoried, secondInventoryOperation));
                    }

                    if (unitInStock is not null)
                    {
                        unitInStockDtoList.Remove(unitInStock);
                    }

                    if (inStock)
                    {
                        unitInStockDtoList.Add(lastOperationWithUnit);
                    }

                    #endregion
                }

                #endregion

                #endregion
            }

            // Добавляем в словарь СНК текущую дату инвентаризации и СНК на эту дату.
            unitInStockByDateDictionary.Add(inventoryDate, [.. unitInStockDtoList]);

            // Добавляем в словарь ошибок текущую дату и список ошибок на эту дату.
            inventoryErrorsByDateDictionary.Add(inventoryDate, [.. errorsDtoList]);

            currentInventoryDateIndex++;
        }

        return (unitInStockByDateDictionary, inventoryErrorsByDateDictionary);
    }

    #endregion

    #region GetOperationsWithoutDuplicates

    private static Task<List<ShortFormDTO>> GetOperationsWithoutDuplicates(List<ShortFormDTO> operationList, string formNum)
    {
        var plusOperationArray = GetPlusOperationsArray(formNum);
        var minusOperationArray = GetMinusOperationsArray(formNum);

        List<ShortFormDTO> operationsWithoutDuplicates = [];
        foreach (var group in operationList.GroupBy(x => x.OpDate))
        {
            var countPlus = group
                .Where(x => plusOperationArray.Contains(x.OpCode))
                .Sum(x => x.Quantity);

            var countMinus = group
                .Where(x => minusOperationArray.Contains(x.OpCode))
                .Sum(x => x.Quantity);

            var givenReceivedPerDayAmount = countPlus - countMinus;

            switch (givenReceivedPerDayAmount)
            {
                case > 0:
                {
                    var lastOp = group.Last(x => plusOperationArray.Contains(x.OpCode));
                    lastOp.Quantity = givenReceivedPerDayAmount;
                    operationsWithoutDuplicates.Add(lastOp);
                    break;
                }
                case 0:
                {
                    break;
                }
                case < 0:
                {
                    var lastOp = group.Last(x => minusOperationArray.Contains(x.OpCode));
                    lastOp.Quantity = int.Abs(givenReceivedPerDayAmount);
                    operationsWithoutDuplicates.Add(lastOp);
                    break;
                }
            }
        }
        return Task.FromResult(operationsWithoutDuplicates);
    }

    #endregion

    #region GetZeroFormsDtoList

    private static async Task<List<ShortFormDTO>> GetZeroFormsDtoList(DBModel db, List<int> reportIds, List<ShortFormDTO> rechargeFormsDtoList, 
        DateOnly firstSnkDate, DateOnly endSnkDate, string formNum, CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
        var plusOperationsArray = GetPlusOperationsArray(formNum);
        var minusOperationsArray = GetMinusOperationsArray(formNum);

        var zeroOperationDtoList = formNum switch
        {
            #region 1.1

            "1.1" => await db.form_11
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report)
                .Where(x => x.Report != null
                            && reportIds.Contains(x.Report.Id)
                            && !plusOperationsArray.Contains(x.OperationCode_DB)
                            && !minusOperationsArray.Contains(x.OperationCode_DB)
                            && x.OperationCode_DB != "10"
                            && x.OperationCode_DB != "53"
                            && x.OperationCode_DB != "54")
                .Select(form => new ShortFormStringDatesDTO
                {
                    Id = form.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    RepId = form.Report!.Id,
                    StDate = form.Report.StartPeriod_DB,
                    EndDate = form.Report.EndPeriod_DB,
                    FacNum = snkParams == null || snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams == null || snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams == null || snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Quantity = form.Quantity_DB,
                    Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams == null || snkParams.CheckType
                        ? form.Type_DB
                        : string.Empty
                })
                .ToListAsync(cts.Token),

            #endregion

            #region 1.3

            "1.3" => await db.form_13
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report)
                .Where(x => x.Report != null
                            && reportIds.Contains(x.Report.Id)
                            && !plusOperationsArray.Contains(x.OperationCode_DB)
                            && !minusOperationsArray.Contains(x.OperationCode_DB)
                            && x.OperationCode_DB != "10"
                            && x.OperationCode_DB != "53"
                            && x.OperationCode_DB != "54")
                .Select(form => new ShortFormStringDatesDTO
                {
                    Id = form.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    RepId = form.Report!.Id,
                    StDate = form.Report.StartPeriod_DB,
                    EndDate = form.Report.EndPeriod_DB,
                    FacNum = snkParams == null || snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams == null || snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams == null || snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Quantity = 1,
                    Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams == null || snkParams.CheckType
                        ? form.Type_DB
                        : string.Empty
                })
                .ToListAsync(cts.Token),

            #endregion

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        return zeroOperationDtoList
            .Where(x => DateTime.TryParse(x.OpDate, out var opDateTime)
                        && DateOnly.TryParse(x.StDate, out _)
                        && DateOnly.TryParse(x.EndDate, out _)
                        && DateOnly.FromDateTime(opDateTime) >= firstSnkDate
                        && DateOnly.FromDateTime(opDateTime) <= endSnkDate)
            .Select(x => new ShortFormDTO
            {
                Id = x.Id,
                NumberInOrder = x.NumberInOrder,
                RepDto = new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                OpCode = x.OpCode,
                OpDate = DateOnly.FromDateTime(DateTime.Parse(x.OpDate)),
                PasNum = x.PasNum,
                Type = x.Type,
                Radionuclids = x.Radionuclids,
                FacNum = x.FacNum,
                Quantity = x.Quantity ?? 0,
                PackNumber = x.PackNumber
            })
            .Union(rechargeFormsDtoList)
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();
    }

    #endregion

    #region IsZeroOperation

    private static bool IsZeroOperation(ShortFormDTO? dto, string formNum)
    {
        if (dto is null) return false;
        return !GetPlusOperationsArray(formNum).Contains(dto.OpCode)
               && !GetMinusOperationsArray(formNum).Contains(dto.OpCode)
               && dto.OpCode is not ("10" or "63" or "64");
    }

    #endregion

    #region DTO

    #region InventoryErrorForm

    private abstract class InventoryErrorFormDTO(InventoryErrorTypeEnum errorType, DateOnly stPer, DateOnly endPer, int rowNumber, string facNum,
        string opCode, string opDate, string pasNum, string radionuclids, string type, string activity, string creatorOKPO,
        string creationDate, string packNumber)
    {
        public readonly InventoryErrorTypeEnum ErrorType = errorType;

        public readonly DateOnly StPer = stPer;

        public readonly DateOnly EndPer = endPer;

        public readonly int RowNumber = rowNumber;

        public readonly string PasNum = pasNum;

        public readonly string OpCode = opCode;

        public readonly string OpDate = opDate;

        public readonly string Type = type;

        public readonly string Radionuclids = radionuclids;

        public readonly string FacNum = facNum;

        public readonly string Activity = activity;

        public readonly string CreatorOKPO = creatorOKPO;

        public readonly string CreationDate = creationDate;

        public readonly string PackNumber = packNumber;
    }

    private class InventoryErrorForm11DTO(InventoryErrorTypeEnum errorType, DateOnly stPer, DateOnly endPer, int rowNumber, string facNum, 
        string opCode, string opDate, string pasNum, int quantity, string radionuclids, string type, string activity, string creatorOKPO, 
        string creationDate, short? category, float? signedServicePeriod, string packNumber) 
        : InventoryErrorFormDTO(errorType, stPer, endPer, rowNumber, facNum, opCode, opDate, pasNum, radionuclids, type, activity, creatorOKPO,
        creationDate, packNumber)
    {
        public readonly int Quantity = quantity;

        public readonly short? Category = category;

        public readonly float? SignedServicePeriod = signedServicePeriod;
    }

    private class InventoryErrorForm13DTO(InventoryErrorTypeEnum errorType, DateOnly stPer, DateOnly endPer, int rowNumber, string facNum,
        string opCode, string opDate, string pasNum, string radionuclids, string type, string activity, string creatorOKPO,
        string creationDate, short? aggregateState, string packNumber)
        : InventoryErrorFormDTO(errorType, stPer, endPer, rowNumber, facNum, opCode, opDate, pasNum, radionuclids, type, activity, creatorOKPO,
            creationDate, packNumber)
    {
        public readonly short? AggregateState = aggregateState;
    }

    #endregion

    private class InventoryErrorsShortDto(InventoryErrorTypeEnum errorTypeEnum, ShortFormDTO dto)
    {
        public readonly InventoryErrorTypeEnum ErrorTypeEnum = errorTypeEnum;

        public readonly ShortFormDTO Dto = dto;
    }

    #endregion

    #region InventoryErrorTypeEnum

    /// <summary>
    /// Перечисление типов ошибок.
    /// </summary>
    private enum InventoryErrorTypeEnum
    {
        /// <summary>
        /// 0. Для заполненного зав.№ и № паспорта, повторная операция инвентаризации.
        /// </summary>
        InventoryDuplicate = 0,

        /// <summary>
        /// 1. Нет в инвентаризациях, но последняя операция - плюсовая.
        /// </summary>
        RegisteredAndNotInventoriedUnit = 1,

        /// <summary>
        /// 2. В инвентаризации отсутствует ранее стоявший на учёте ЗРИ (был в наличии на момент прошлой инвентаризации, нет операций передачи).
        /// </summary>
        MissingFromInventoryUnit = 2,

        /// <summary>
        /// 3. Есть во второй инвентаризации, последняя операция не плюсовая.
        /// </summary>
        GivenUnitIsInventoried = 3,

        /// <summary>
        /// 4. Снятие с учёта не стоявшего на учёте ЗРИ.
        /// </summary>
        UnInventoriedUnitGivenAway = 4,

        /// <summary>
        /// 5. Двойное снятие с учёта.
        /// </summary>
        ReDeRegistration = 5,

        /// <summary>
        /// 6. Постановка на учёт имеющегося в наличии ЗРИ.
        /// </summary>
        InventoriedUnitReceived = 6,

        /// <summary>
        /// 7. Двойная постановка на учёт
        /// </summary>
        ReRegistration = 7,

        /// <summary>
        /// 8. Нулевые операции с отсутствующим в наличии ЗРИ.
        /// </summary>
        ZeroOperationWithUnInventoriedUnit = 8,

        /// <summary>
        /// 9. Для пустых зав.№ и № паспорта, отдано большее количество, чем было на момент операции.
        /// </summary>
        QuantityGivenExceedsAvailable = 9
    }

    private static string GetErrorDescriptionByType(InventoryErrorTypeEnum type)
    {
        return type switch
        {
            InventoryErrorTypeEnum.InventoryDuplicate => "Повторная операция инвентаризации ЗРИ в ту же дату. (0)",
            InventoryErrorTypeEnum.RegisteredAndNotInventoriedUnit => "В инвентаризации отсутствует поставленный на учёт ЗРИ. (1)",
            InventoryErrorTypeEnum.MissingFromInventoryUnit => "В инвентаризации отсутствует ранее стоявший на учёте ЗРИ (был в наличии на момент прошлой инвентаризации, нет операций передачи). (2)",
            InventoryErrorTypeEnum.GivenUnitIsInventoried => "Проинвентаризирован ЗРИ, который был ранее снят с учёта или не был получен. (3)",
            InventoryErrorTypeEnum.UnInventoriedUnitGivenAway => "Снятие с учёта не стоявшего на учёте ЗРИ. (4)",
            InventoryErrorTypeEnum.ReDeRegistration => "Повторная операция снятия ЗРИ с учёта. (5)",
            InventoryErrorTypeEnum.InventoriedUnitReceived => "Постановка на учёт имеющегося в наличии ЗРИ. (6)",
            InventoryErrorTypeEnum.ReRegistration => "Повторная операция постановки ЗРИ на учёт. (7)",
            InventoryErrorTypeEnum.ZeroOperationWithUnInventoriedUnit => "Операция с отсутствующим в наличии ЗРИ. (нулевая) (8)",
            InventoryErrorTypeEnum.QuantityGivenExceedsAvailable => "Снятие с учёта большего количества ЗРИ, чем было в наличии. (9)",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    #endregion
}