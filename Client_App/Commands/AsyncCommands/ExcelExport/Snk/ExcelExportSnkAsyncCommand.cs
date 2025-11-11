using Avalonia.Threading;
using Client_App.Views.ProgressBar;
using System.Threading;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using Client_App.Views;
using Models.DBRealization;
using System.Reflection;
using System.Collections.Generic;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using static Client_App.Resources.StaticStringMethods;
using Client_App.ViewModels.ProgressBar;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Выгрузка в .xlsx СНК на дату.
/// </summary>
public class ExcelExportSnkAsyncCommand : ExcelExportSnkBaseAsyncCommand
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
            $"Выгрузка СНК {formNum}", "СНК");
        var selectedReports = await CheckRepsAndRepPresence(formNum, progressBar, cts);

        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"СНК_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(7, "Запрос даты формирования СНК");
        var (endSnkDate, snkParams) = await AskSnkEndDate(progressBar, cts);

        progressBarVM.SetProgressBar(8, "Запрос пути сохранения",
            $"Выгрузка СНК {formNum} {regNum}_{okpo}_{endSnkDate}", ExportType);
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Заполнение заголовков");
        var excelSnkWorksheet = await FillExcelHeaders(formNum, excelPackage, endSnkDate);

        progressBarVM.SetProgressBar(15, "Загрузка инвентаризационных отчётов");
        var inventoryReportDtoList = await GetInventoryReportDtoList(db, selectedReports.Id, formNum, endSnkDate, cts);

        progressBarVM.SetProgressBar(20, "Загрузка операций инвентаризации");
        var (firstSnkDate, inventoryFormsDtoList, _) = await GetInventoryFormsDtoList(db, inventoryReportDtoList, formNum, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(24, "Загрузка списка отчётов");
        var reportIds = await GetReportIds(db, selectedReports.Id, formNum, cts);

        progressBarVM.SetProgressBar(25, "Загрузка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, reportIds, formNum, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(30, "Загрузка операций перезарядки");
        var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, selectedReports.Id, formNum, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(35, "Формирование списка учётных единиц");
        var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(formNum, inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);

        progressBarVM.SetProgressBar(50, "Формирование СНК");
        var unitInStockDtoList = await GetUnitInStockDtoList(uniqueUnitWithAllOperationDictionary, formNum, firstSnkDate, progressBarVM);

        progressBarVM.SetProgressBar(60, "Загрузка форм");
        var fullFormsSnkList = await GetFullFormsSnkList(db, unitInStockDtoList, formNum, progressBarVM, cts);

        progressBarVM.SetProgressBar(85, "Проверка наличия");
        await CheckPresenceInSnk(fullFormsSnkList, endSnkDate, progressBar, cts);

        progressBarVM.SetProgressBar(90, "Заполнение строчек в .xlsx");
        await FillExcel(fullFormsSnkList, formNum, excelSnkWorksheet);

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

    #region CheckPresenceInSnk

    /// <summary>
    /// Проверяет наличие учётных единиц и при их отсутствии выводит сообщение.
    /// </summary>
    /// <param name="fullFormsSnkList">Список форм с данными отчётов и организации.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckPresenceInSnk(List<SnkFormDTO> fullFormsSnkList, DateOnly endSnkDate,
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        if (fullFormsSnkList.Count == 0)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = $"На {endSnkDate.ToShortDateString()} учётные единицы в наличии отсутствуют.",
                    MinWidth = 400,
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
    /// <param name="formNum">Номер формы.</param>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <param name="date">Дата, на которую формируется СНК.</param>
    private static async Task<ExcelWorksheet> FillExcelHeaders(string formNum, ExcelPackage excelPackage, DateOnly date)
    {
        var worksheet = excelPackage.Workbook.Worksheets.Add($"СНК на {date.ToShortDateString()}");

        switch (formNum)
        {
            case "1.1":
            {
                #region Headers

                worksheet.Cells[1, 1].Value = "№ п/п";
                worksheet.Cells[1, 2].Value = "Номер паспорта (сертификата)";
                worksheet.Cells[1, 3].Value = "тип";
                worksheet.Cells[1, 4].Value = "радионуклиды";
                worksheet.Cells[1, 5].Value = "номер";
                worksheet.Cells[1, 6].Value = "количество, шт.";
                worksheet.Cells[1, 7].Value = "суммарная активность, Бк";
                worksheet.Cells[1, 8].Value = "код ОКПО изготовителя";
                worksheet.Cells[1, 9].Value = "дата выпуска";
                worksheet.Cells[1, 10].Value = "категория";
                worksheet.Cells[1, 11].Value = "НСС, мес";
                worksheet.Cells[1, 12].Value = "Номер УКТ";

                #endregion

                break;
            }
            case "1.3":
            {
                #region Headers

                worksheet.Cells[1, 1].Value = "№ п/п";
                worksheet.Cells[1, 2].Value = "Номер паспорта (сертификата)";
                worksheet.Cells[1, 3].Value = "тип";
                worksheet.Cells[1, 4].Value = "радионуклиды";
                worksheet.Cells[1, 5].Value = "номер";
                worksheet.Cells[1, 6].Value = "активность, Бк";
                worksheet.Cells[1, 7].Value = "код ОКПО изготовителя";
                worksheet.Cells[1, 8].Value = "дата выпуска";
                worksheet.Cells[1, 9].Value = "агрегатное состояние";
                worksheet.Cells[1, 10].Value = "Номер УКТ";

                #endregion

                break;
            }
        }

        await AutoFitColumns(worksheet);

        return worksheet;
    }

    #region AutoFitColumns

    /// <summary>
    /// Для текущей страницы Excel пакета подбирает ширину колонок и замораживает первую строчку.
    /// </summary>
    private static Task AutoFitColumns(ExcelWorksheet worksheet)
    {
        for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            if (OperatingSystem.IsWindows()) worksheet.Column(col).AutoFit();
        }
        worksheet.View.FreezePanes(2, 1);
        return Task.CompletedTask;
    }

    #endregion

    #endregion

    #region FillExcel

    /// <summary>
    /// Заполняет заголовки Excel пакета.
    /// </summary>
    /// <param name="fullFormsSnkList">Список полных форм СНК.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="worksheet">Лист Excel пакета.</param>
    private static Task FillExcel(List<SnkFormDTO> fullFormsSnkList, string formNum, ExcelWorksheet worksheet)
    {
        var currentRow = 2;
        var currentForm = 1;
        foreach (var form in fullFormsSnkList)
        {
            switch (formNum)
            {
                #region 1.1
                
                case "1.1":
                {
                    var form11 = (SnkForm11DTO)form;

                    worksheet.Cells[currentRow, 1].Value = currentForm;
                    worksheet.Cells[currentRow, 2].Value = ConvertToExcelString(form11.PasNum);
                    worksheet.Cells[currentRow, 3].Value = ConvertToExcelString(form11.Type);
                    worksheet.Cells[currentRow, 4].Value = ConvertToExcelString(form11.Radionuclids);
                    worksheet.Cells[currentRow, 5].Value = ConvertToExcelString(form11.FacNum);
                    worksheet.Cells[currentRow, 6].Value = form11.Quantity is 0 ? "-" : form11.Quantity;
                    worksheet.Cells[currentRow, 7].Value = ConvertToExcelDouble(form11.Activity);
                    worksheet.Cells[currentRow, 8].Value = ConvertToExcelString(form11.CreatorOKPO);
                    worksheet.Cells[currentRow, 9].Value = ConvertToExcelDate(form11.CreationDate, worksheet, currentRow, 9);
                    worksheet.Cells[currentRow, 10].Value = form11.Category is 0 ? "-" : form11.Category;
                    worksheet.Cells[currentRow, 11].Value = form11.SignedServicePeriod is 0 ? "-" : form11.SignedServicePeriod;
                    worksheet.Cells[currentRow, 12].Value = ConvertToExcelString(form11.PackNumber);

                    currentRow++;
                    currentForm++;

                    break;
                }

                #endregion

                #region 1.3
                
                case "1.3":
                {
                    var form13 = (SnkForm13DTO)form;

                    for (var i = 0; i < form13.Quantity; i++)
                    {
                        worksheet.Cells[currentRow, 1].Value = currentForm;
                        worksheet.Cells[currentRow, 2].Value = ConvertToExcelString(form13.PasNum);
                        worksheet.Cells[currentRow, 3].Value = ConvertToExcelString(form13.Type);
                        worksheet.Cells[currentRow, 4].Value = ConvertToExcelString(form13.Radionuclids);
                        worksheet.Cells[currentRow, 5].Value = ConvertToExcelString(form13.FacNum);
                        worksheet.Cells[currentRow, 6].Value = ConvertToExcelDouble(form13.Activity);
                        worksheet.Cells[currentRow, 7].Value = ConvertToExcelString(form13.CreatorOKPO);
                        worksheet.Cells[currentRow, 8].Value = ConvertToExcelDate(form13.CreationDate, worksheet, currentRow, 8);
                        worksheet.Cells[currentRow, 9].Value = form13.AggregateState is 0 ? "-" : form13.AggregateState;
                        worksheet.Cells[currentRow, 10].Value = ConvertToExcelString(form13.PackNumber);

                        currentRow++;
                        currentForm++;
                    }

                    break;
                }

                #endregion
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region GetFullForm

    /// <summary>
    /// Загрузка из БД полных форм вместе с данными отчётов и организации.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="unitInStockDtoList">Список DTO учётных единиц в наличии на дату.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список форм с данными отчётов и организации.</returns>
    private static async Task<List<SnkFormDTO>> GetFullFormsSnkList(DBModel db, List<ShortFormDTO> unitInStockDtoList, string formNum,
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<SnkFormDTO> formsList = [];
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;
        foreach (var unit in unitInStockDtoList)
        {
            SnkFormDTO form = formNum switch
            {
                "1.1" => await db.form_11
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x =>
                        x.Id == unit.Id
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

                "1.3" => await db.form_13
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x =>
                        x.Id == unit.Id
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
                _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
            };

            formsList.Add(form);

            progressBarDoubleValue += (double)25 / unitInStockDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка {currentUnitNum} формы из {unitInStockDtoList.Count}",
                "Загрузка форм");
            currentUnitNum++;
        }
        return formsList;

    }

    #endregion
}