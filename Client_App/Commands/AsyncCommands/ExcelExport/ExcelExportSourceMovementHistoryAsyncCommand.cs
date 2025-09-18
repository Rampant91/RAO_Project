using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.DTO;
using Models.Forms.Form1;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Client_App.Resources.StaticStringMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Выгрузка в .xlsx истории движения источника.
/// </summary>
public partial class ExcelExportSourceMovementHistoryAsyncCommand : ExcelBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var cts = new CancellationTokenSource();
        ExportType = "История_движения_источника";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Проверка правильности заполнения паспорта", "Выгрузка в .xlsx", ExportType);

        //Костыль для старого и нового интерфейса. Нужно убрать, когда откажемся от старого интерфейса форм 1. TODO
        string? pasNum;
        string? factoryNum;
        if (parameter is object[])
        {
            StaticMethods.PassportUniqParam(parameter, out _, out _, out _, out pasNum, out factoryNum);
        }
        else
        {
            StaticMethods.NewPassportUniqParam(parameter, out _, out _, out _, out pasNum, out factoryNum);
        }

        await CheckPasParam(pasNum, factoryNum, progressBar, cts);

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{RemoveForbiddenChars(pasNum!)}_{RemoveForbiddenChars(factoryNum!)}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(9, "Создание временной БД", 
            $"Выгрузка движения источника{Environment.NewLine}" + $"{pasNum}_{factoryNum}", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(11, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(15, "Загрузка паспортов форм 1.1");
        var pasUniqList11 = await GetPasUniqData(db, "1.1", cts);

        progressBarVM.SetProgressBar(40, "Загрузка форм 1.1");
        var filteredForm11 = (await GetFilteredForm(db, pasUniqList11, "1.1", pasNum!, factoryNum!, cts)).Cast<Form11>();

        progressBarVM.SetProgressBar(50, "Заполнение строчек форм 1.1");
        await FillExcel_11(filteredForm11, excelPackage);

        progressBarVM.SetProgressBar(55, "Загрузка паспортов форм 1.5");
        var pasUniqList15 = await GetPasUniqData(db, "1.5", cts);

        progressBarVM.SetProgressBar(80, "Загрузка форм 1.5");
        var filteredForm15 = (await GetFilteredForm(db, pasUniqList15, "1.5", pasNum!, factoryNum!, cts)).Cast<Form15>();

        progressBarVM.SetProgressBar(90, "Заполнение строчек форм 1.5");
        await FillExcel_15(filteredForm15, excelPackage);

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

    #region CheckPasParam

    /// <summary>
    /// Проверяет корректность номера паспорта и заводского номера и в случае некорректных значений выводит сообщение и завершает выполнение команды.
    /// </summary>
    /// <param name="pasNum">Номер паспорта.</param>
    /// <param name="factoryNum">Заводской номер.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckPasParam(string? pasNum, string? factoryNum, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        if (string.IsNullOrEmpty(pasNum) || string.IsNullOrEmpty(factoryNum) || pasNum is "-" && factoryNum is "-")
        {
            #region MessageFailedToLoadPassportUniqParam

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Не удалось выполнить выгрузку в Excel истории движения источника,"
                                     + $"{Environment.NewLine}поскольку не заполнено одно из следующих полей:" +
                                     $"{Environment.NewLine}- номер паспорта (сертификата);" +
                                     $"{Environment.NewLine}- номер источника.",
                    MinHeight = 100,
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion

    #region FillExcel

    #region FillExcel_11

    /// <summary>
    /// Заполняет строчки в .xlsx файле.
    /// </summary>
    /// <param name="filteredForm11">Отфильтрованный список форм отчётности 1.1.</param>
    private Task FillExcel_11(IEnumerable<Form11> filteredForm11, ExcelPackage excelPackage)
    {
        var dto11List = new List<Form11ExtendedDTO>();

        #region BindDtoList

        foreach (var form11 in filteredForm11)
        {
            if (form11.Report?.Reports is null) continue;
            var rep = form11.Report;
            var reps = form11.Report.Reports;
            dto11List.Add(new Form11ExtendedDTO
            {
                RegNoRep = reps.Master.RegNoRep.Value,
                ShortJurLico = reps.Master.ShortJurLicoRep.Value,
                OkpoRep = reps.Master.OkpoRep.Value,
                FormNum = rep.FormNum_DB,
                StartPeriod = rep.StartPeriod_DB,
                EndPeriod = rep.EndPeriod_DB,
                CorrectionNumber = rep.CorrectionNumber_DB,
                RowCount = rep.Rows11.Count,
                NumberInOrder = form11.NumberInOrder_DB,
                OperationCode = form11.OperationCode_DB,
                OperationDate = form11.OperationDate_DB,
                PassportNumber = form11.PassportNumber_DB,
                Type = form11.Type_DB,
                Radionuclids = form11.Radionuclids_DB,
                FactoryNumber = form11.FactoryNumber_DB,
                Activity = form11.Activity_DB,
                Quantity = form11.Quantity_DB,
                CreatorOKPO = form11.CreatorOKPO_DB,
                CreationDate = form11.CreationDate_DB,
                Category = form11.Category_DB,
                SignedServicePeriod = form11.SignedServicePeriod_DB,
                PropertyCode = form11.PropertyCode_DB,
                Owner = form11.Owner_DB,
                DocumentVid = form11.DocumentVid_DB,
                DocumentNumber = form11.DocumentNumber_DB,
                DocumentDate = form11.DocumentDate_DB,
                ProviderOrRecieverOKPO = form11.ProviderOrRecieverOKPO_DB,
                TransporterOKPO = form11.TransporterOKPO_DB,
                PackName = form11.PackName_DB,
                PackType = form11.PackType_DB,
                PackNumber = form11.PackNumber_DB
            });
        }

        #endregion

        var lastRow = 1;
        Worksheet = excelPackage.Workbook.Worksheets.First(x => x.Name is "Операции по форме 1.1");
        foreach (var dto in dto11List
                     .OrderBy(x => DateOnly.TryParse(x.OperationDate, out var opDate) ? opDate : DateOnly.MaxValue)
                     .ThenBy(x => x.RegNoRep))
        {
            if (lastRow == 1)
            {
                #region BindingCells

                Worksheet.Cells[2, 1].Value = dto.RegNoRep;
                Worksheet.Cells[2, 2].Value = dto.ShortJurLico;
                Worksheet.Cells[2, 3].Value = dto.OkpoRep;
                Worksheet.Cells[2, 4].Value = dto.FormNum;
                Worksheet.Cells[2, 5].Value = ConvertToExcelDate(dto.StartPeriod, Worksheet, 2, 5);
                Worksheet.Cells[2, 6].Value = ConvertToExcelDate(dto.EndPeriod, Worksheet, 2, 6);
                Worksheet.Cells[2, 7].Value = dto.CorrectionNumber;
                Worksheet.Cells[2, 8].Value = dto.RowCount;
                Worksheet.Cells[2, 9].Value = dto.NumberInOrder;
                Worksheet.Cells[2, 10].Value = ConvertToExcelString(dto.OperationCode);
                Worksheet.Cells[2, 11].Value = ConvertToExcelDate(dto.OperationDate, Worksheet, 2, 11);
                Worksheet.Cells[2, 12].Value = ConvertToExcelString(dto.PassportNumber);
                Worksheet.Cells[2, 13].Value = ConvertToExcelString(dto.Type);
                Worksheet.Cells[2, 14].Value = ConvertToExcelString(dto.Radionuclids);
                Worksheet.Cells[2, 15].Value = ConvertToExcelString(dto.FactoryNumber);
                Worksheet.Cells[2, 16].Value = dto.Quantity is null ? "-" : dto.Quantity;
                Worksheet.Cells[2, 17].Value = ConvertToExcelDouble(dto.Activity);
                Worksheet.Cells[2, 18].Value = ConvertToExcelString(dto.CreatorOKPO);
                Worksheet.Cells[2, 19].Value = ConvertToExcelDate(dto.CreationDate, Worksheet, 2, 19);
                Worksheet.Cells[2, 20].Value = dto.Category is null ? "-" : dto.Category;
                Worksheet.Cells[2, 21].Value = dto.SignedServicePeriod is null ? "-" : dto.SignedServicePeriod;
                Worksheet.Cells[2, 22].Value = dto.PropertyCode is null ? "-" : dto.PropertyCode;
                Worksheet.Cells[2, 23].Value = ConvertToExcelString(dto.Owner);
                Worksheet.Cells[2, 24].Value = dto.DocumentVid is null ? "-" : dto.DocumentVid;
                Worksheet.Cells[2, 25].Value = ConvertToExcelString(dto.DocumentNumber);
                Worksheet.Cells[2, 26].Value = ConvertToExcelDate(dto.DocumentDate, Worksheet, 2, 26);
                Worksheet.Cells[2, 27].Value = ConvertToExcelString(dto.ProviderOrRecieverOKPO);
                Worksheet.Cells[2, 28].Value = ConvertToExcelString(dto.TransporterOKPO);
                Worksheet.Cells[2, 29].Value = ConvertToExcelString(dto.PackName);
                Worksheet.Cells[2, 30].Value = ConvertToExcelString(dto.PackType);
                Worksheet.Cells[2, 31].Value = ConvertToExcelString(dto.PackNumber);

                #endregion

                lastRow++;
                continue;
            }
            for (var currentRow = 2; currentRow <= lastRow + 1; currentRow++)
            {
                var opDateStr = Worksheet.Cells[currentRow, 11].Value?.ToString();
                if (new CustomStringDateComparer(StringComparer.CurrentCulture)
                        .Compare(dto.OperationDate, opDateStr) >= 0) continue;

                Worksheet.InsertRow(currentRow, 1);

                #region BindingCells

                Worksheet.Cells[currentRow, 1].Value = dto.RegNoRep;
                Worksheet.Cells[currentRow, 2].Value = dto.ShortJurLico;
                Worksheet.Cells[currentRow, 3].Value = dto.OkpoRep;
                Worksheet.Cells[currentRow, 4].Value = dto.FormNum;
                Worksheet.Cells[currentRow, 5].Value = ConvertToExcelDate(dto.StartPeriod, Worksheet, currentRow, 5);
                Worksheet.Cells[currentRow, 6].Value = ConvertToExcelDate(dto.EndPeriod, Worksheet, currentRow, 6);
                Worksheet.Cells[currentRow, 7].Value = dto.CorrectionNumber;
                Worksheet.Cells[currentRow, 8].Value = dto.RowCount;
                Worksheet.Cells[currentRow, 9].Value = dto.NumberInOrder;
                Worksheet.Cells[currentRow, 10].Value = ConvertToExcelString(dto.OperationCode);
                Worksheet.Cells[currentRow, 11].Value = ConvertToExcelDate(dto.OperationDate, Worksheet, currentRow, 11);
                Worksheet.Cells[currentRow, 12].Value = ConvertToExcelString(dto.PassportNumber);
                Worksheet.Cells[currentRow, 13].Value = ConvertToExcelString(dto.Type);
                Worksheet.Cells[currentRow, 14].Value = ConvertToExcelString(dto.Radionuclids);
                Worksheet.Cells[currentRow, 15].Value = ConvertToExcelString(dto.FactoryNumber);
                Worksheet.Cells[currentRow, 16].Value = dto.Quantity is null ? "-" : dto.Quantity;
                Worksheet.Cells[currentRow, 17].Value = ConvertToExcelDouble(dto.Activity);
                Worksheet.Cells[currentRow, 18].Value = ConvertToExcelString(dto.CreatorOKPO);
                Worksheet.Cells[currentRow, 19].Value = ConvertToExcelDate(dto.CreationDate, Worksheet, currentRow, 19);
                Worksheet.Cells[currentRow, 20].Value = dto.Category is null ? "-" : dto.Category;
                Worksheet.Cells[currentRow, 21].Value = dto.SignedServicePeriod is null ? "-" : dto.SignedServicePeriod;
                Worksheet.Cells[currentRow, 22].Value = dto.PropertyCode is null ? "-" : dto.PropertyCode;
                Worksheet.Cells[currentRow, 23].Value = ConvertToExcelString(dto.Owner);
                Worksheet.Cells[currentRow, 24].Value = dto.DocumentVid is null ? "-" : dto.DocumentVid;
                Worksheet.Cells[currentRow, 25].Value = ConvertToExcelString(dto.DocumentNumber);
                Worksheet.Cells[currentRow, 26].Value = ConvertToExcelDate(dto.DocumentDate, Worksheet, currentRow, 26);
                Worksheet.Cells[currentRow, 27].Value = ConvertToExcelString(dto.ProviderOrRecieverOKPO);
                Worksheet.Cells[currentRow, 28].Value = ConvertToExcelString(dto.TransporterOKPO);
                Worksheet.Cells[currentRow, 29].Value = ConvertToExcelString(dto.PackName);
                Worksheet.Cells[currentRow, 30].Value = ConvertToExcelString(dto.PackType);
                Worksheet.Cells[currentRow, 31].Value = ConvertToExcelString(dto.PackNumber);

                #endregion

                lastRow++;
                break;
            }
        }

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            Worksheet.Cells.AutoFitColumns();
        }

        var headersCellsString = "A1:A" + Worksheet.Dimension.End.Column;
        Worksheet.Cells[headersCellsString].Style.WrapText = true;
        Worksheet.Cells[headersCellsString].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        Worksheet.Cells[headersCellsString].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        return Task.CompletedTask;
    }

    #endregion

    #region FillExcel_15

    /// <summary>
    /// Заполняет строчки в .xlsx файле.
    /// </summary>
    /// <param name="filteredForm15">Отфильтрованный список форм отчётности 1.5.</param>
    private Task FillExcel_15(IEnumerable<Form15> filteredForm15, ExcelPackage excelPackage)
    {
        var dto15List = new List<Form15ExtendedDTO>();

        #region BindDtoList

        foreach (var form15 in filteredForm15)
        {
            if (form15.Report?.Reports is null) continue;
            var rep = form15.Report;
            var reps = form15.Report.Reports;
            dto15List.Add(new Form15ExtendedDTO
            {
                RegNoRep = reps.Master.RegNoRep.Value,
                ShortJurLico = reps.Master.ShortJurLicoRep.Value,
                OkpoRep = reps.Master.OkpoRep.Value,
                FormNum = rep.FormNum_DB,
                StartPeriod = rep.StartPeriod_DB,
                EndPeriod = rep.EndPeriod_DB,
                CorrectionNumber = rep.CorrectionNumber_DB,
                RowCount = rep.Rows15.Count,
                NumberInOrder = form15.NumberInOrder_DB,
                OperationCode = form15.OperationCode_DB,
                OperationDate = form15.OperationDate_DB,
                PassportNumber = form15.PassportNumber_DB,
                Type = form15.Type_DB,
                Radionuclids = form15.Radionuclids_DB,
                FactoryNumber = form15.FactoryNumber_DB,
                Activity = form15.Activity_DB,
                Quantity = form15.Quantity_DB,
                CreationDate = form15.CreationDate_DB,
                StatusRAO = form15.StatusRAO_DB,
                DocumentVid = form15.DocumentVid_DB,
                DocumentNumber = form15.DocumentNumber_DB,
                DocumentDate = form15.DocumentDate_DB,
                ProviderOrRecieverOKPO = form15.ProviderOrRecieverOKPO_DB,
                TransporterOKPO = form15.TransporterOKPO_DB,
                PackName = form15.PackName_DB,
                PackType = form15.PackType_DB,
                PackNumber = form15.PackNumber_DB,
                StoragePlaceName = form15.StoragePlaceName_DB,
                StoragePlaceCode = form15.StoragePlaceCode_DB,
                RefineOrSortRAOCode = form15.RefineOrSortRAOCode_DB,
                Subsidy = form15.Subsidy_DB,
                FcpNumber = form15.FcpNumber_DB,
                ContractNumber = form15.ContractNumber_DB
            });
        }

        #endregion

        var lastRow = 1;
        Worksheet = excelPackage.Workbook.Worksheets.First(x => x.Name is "Операции по форме 1.5");
        foreach (var dto in dto15List
                     .OrderBy(x => DateOnly.TryParse(x.OperationDate, out var opDate) ? opDate : DateOnly.MaxValue)
                     .ThenBy(x => x.RegNoRep))
        {
            if (lastRow == 1)
            {
                #region BindingCells

                Worksheet.Cells[2, 1].Value = dto.RegNoRep;
                Worksheet.Cells[2, 2].Value = dto.ShortJurLico;
                Worksheet.Cells[2, 3].Value = dto.OkpoRep;
                Worksheet.Cells[2, 4].Value = dto.FormNum;
                Worksheet.Cells[2, 5].Value = ConvertToExcelDate(dto.StartPeriod, Worksheet, 2, 5);
                Worksheet.Cells[2, 6].Value = ConvertToExcelDate(dto.EndPeriod, Worksheet, 2, 6);
                Worksheet.Cells[2, 7].Value = dto.CorrectionNumber;
                Worksheet.Cells[2, 8].Value = dto.RowCount;
                Worksheet.Cells[2, 9].Value = dto.NumberInOrder;
                Worksheet.Cells[2, 10].Value = ConvertToExcelString(dto.OperationCode);
                Worksheet.Cells[2, 11].Value = ConvertToExcelDate(dto.OperationDate, Worksheet, 2, 11);
                Worksheet.Cells[2, 12].Value = ConvertToExcelString(dto.PassportNumber);
                Worksheet.Cells[2, 13].Value = ConvertToExcelString(dto.Type);
                Worksheet.Cells[2, 14].Value = ConvertToExcelString(dto.Radionuclids);
                Worksheet.Cells[2, 15].Value = ConvertToExcelString(dto.FactoryNumber);
                Worksheet.Cells[2, 16].Value = dto.Quantity is null ? "-" : dto.Quantity;
                Worksheet.Cells[2, 17].Value = ConvertToExcelDouble(dto.Activity);
                Worksheet.Cells[2, 18].Value = ConvertToExcelDate(dto.CreationDate, Worksheet, 2, 18);
                Worksheet.Cells[2, 19].Value = ConvertToExcelString(dto.StatusRAO);
                Worksheet.Cells[2, 20].Value = dto.DocumentVid is null ? "-" : dto.DocumentVid;
                Worksheet.Cells[2, 21].Value = ConvertToExcelString(dto.DocumentNumber);
                Worksheet.Cells[2, 22].Value = ConvertToExcelDate(dto.DocumentDate, Worksheet, 2, 22);
                Worksheet.Cells[2, 23].Value = ConvertToExcelString(dto.ProviderOrRecieverOKPO);
                Worksheet.Cells[2, 24].Value = ConvertToExcelString(dto.TransporterOKPO);
                Worksheet.Cells[2, 25].Value = ConvertToExcelString(dto.PackName);
                Worksheet.Cells[2, 26].Value = ConvertToExcelString(dto.PackType);
                Worksheet.Cells[2, 27].Value = ConvertToExcelString(dto.PackNumber);
                Worksheet.Cells[2, 28].Value = ConvertToExcelString(dto.StoragePlaceName);
                Worksheet.Cells[2, 29].Value = ConvertToExcelString(dto.StoragePlaceCode);
                Worksheet.Cells[2, 30].Value = ConvertToExcelString(dto.RefineOrSortRAOCode);
                Worksheet.Cells[2, 31].Value = ConvertToExcelString(dto.Subsidy);
                Worksheet.Cells[2, 32].Value = ConvertToExcelString(dto.FcpNumber);
                Worksheet.Cells[2, 33].Value = ConvertToExcelString(dto.ContractNumber);

                #endregion

                lastRow++;
                continue;
            }
            for (var currentRow = 2; currentRow <= lastRow + 1; currentRow++)
            {
                var opDateStr = Worksheet.Cells[currentRow, 11].Value?.ToString();
                if (new CustomStringDateComparer(StringComparer.CurrentCulture)
                        .Compare(dto.OperationDate, opDateStr) >= 0) continue;

                Worksheet.InsertRow(currentRow, 1);

                #region BindingCells

                Worksheet.Cells[currentRow, 1].Value = dto.RegNoRep;
                Worksheet.Cells[currentRow, 2].Value = dto.ShortJurLico;
                Worksheet.Cells[currentRow, 3].Value = dto.OkpoRep;
                Worksheet.Cells[currentRow, 4].Value = dto.FormNum;
                Worksheet.Cells[currentRow, 5].Value = ConvertToExcelDate(dto.StartPeriod, Worksheet, currentRow, 5);
                Worksheet.Cells[currentRow, 6].Value = ConvertToExcelDate(dto.EndPeriod, Worksheet, currentRow, 6);
                Worksheet.Cells[currentRow, 7].Value = dto.CorrectionNumber;
                Worksheet.Cells[currentRow, 8].Value = dto.RowCount;
                Worksheet.Cells[currentRow, 9].Value = dto.NumberInOrder;
                Worksheet.Cells[currentRow, 10].Value = ConvertToExcelString(dto.OperationCode);
                Worksheet.Cells[currentRow, 11].Value = ConvertToExcelDate(dto.OperationDate, Worksheet, currentRow, 11);
                Worksheet.Cells[currentRow, 12].Value = ConvertToExcelString(dto.PassportNumber);
                Worksheet.Cells[currentRow, 13].Value = ConvertToExcelString(dto.Type);
                Worksheet.Cells[currentRow, 14].Value = ConvertToExcelString(dto.Radionuclids);
                Worksheet.Cells[currentRow, 15].Value = ConvertToExcelString(dto.FactoryNumber);
                Worksheet.Cells[currentRow, 16].Value = dto.Quantity is null ? "-" : dto.Quantity;
                Worksheet.Cells[currentRow, 17].Value = ConvertToExcelDouble(dto.Activity);
                Worksheet.Cells[currentRow, 18].Value = ConvertToExcelDate(dto.CreationDate, Worksheet, currentRow, 18);
                Worksheet.Cells[currentRow, 19].Value = ConvertToExcelString(dto.StatusRAO);
                Worksheet.Cells[currentRow, 20].Value = dto.DocumentVid is null ? "-" : dto.DocumentVid;
                Worksheet.Cells[currentRow, 21].Value = ConvertToExcelString(dto.DocumentNumber);
                Worksheet.Cells[currentRow, 22].Value = ConvertToExcelDate(dto.DocumentDate, Worksheet, currentRow, 22);
                Worksheet.Cells[currentRow, 23].Value = ConvertToExcelString(dto.ProviderOrRecieverOKPO);
                Worksheet.Cells[currentRow, 24].Value = ConvertToExcelString(dto.TransporterOKPO);
                Worksheet.Cells[currentRow, 25].Value = ConvertToExcelString(dto.PackName);
                Worksheet.Cells[currentRow, 26].Value = ConvertToExcelString(dto.PackType);
                Worksheet.Cells[currentRow, 27].Value = ConvertToExcelString(dto.PackNumber);
                Worksheet.Cells[currentRow, 28].Value = ConvertToExcelString(dto.StoragePlaceName);
                Worksheet.Cells[currentRow, 29].Value = ConvertToExcelString(dto.StoragePlaceCode);
                Worksheet.Cells[currentRow, 30].Value = ConvertToExcelString(dto.RefineOrSortRAOCode);
                Worksheet.Cells[currentRow, 31].Value = ConvertToExcelString(dto.Subsidy);
                Worksheet.Cells[currentRow, 32].Value = ConvertToExcelString(dto.FcpNumber);
                Worksheet.Cells[currentRow, 33].Value = ConvertToExcelString(dto.ContractNumber);

                #endregion

                lastRow++;
                break;
            }
        }

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            Worksheet.Cells.AutoFitColumns();
        }
        var headersCellsString = "A1:A" + Worksheet.Dimension.End.Column;
        Worksheet.Cells[headersCellsString].Style.WrapText = true;
        Worksheet.Cells[headersCellsString].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        Worksheet.Cells[headersCellsString].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        return Task.CompletedTask;
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполняет заголовки в .xlsx файле.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    private Task FillExcelHeaders(ExcelPackage excelPackage)
    {
        #region 1.1

        Worksheet = excelPackage.Workbook.Worksheets.Add("Операции по форме 1.1");

        Worksheet.Cells[1, 1].Value = "Рег. №";
        Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        Worksheet.Cells[1, 3].Value = "ОКПО";
        Worksheet.Cells[1, 4].Value = "Форма";
        Worksheet.Cells[1, 5].Value = "Дата начала периода";
        Worksheet.Cells[1, 6].Value = "Дата конца периода";
        Worksheet.Cells[1, 7].Value = "Номер корректировки";
        Worksheet.Cells[1, 8].Value = "Количество строк";
        Worksheet.Cells[1, 9].Value = "№ п/п";
        Worksheet.Cells[1, 10].Value = "код";
        Worksheet.Cells[1, 11].Value = "дата";
        Worksheet.Cells[1, 12].Value = "номер паспорта (сертификата)";
        Worksheet.Cells[1, 13].Value = "тип";
        Worksheet.Cells[1, 14].Value = "радионуклиды";
        Worksheet.Cells[1, 15].Value = "номер";
        Worksheet.Cells[1, 16].Value = "количество, шт";
        Worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
        Worksheet.Cells[1, 18].Value = "код ОКПО изготовителя";
        Worksheet.Cells[1, 19].Value = "дата выпуска";
        Worksheet.Cells[1, 20].Value = "категория";
        Worksheet.Cells[1, 21].Value = "НСС, мес";
        Worksheet.Cells[1, 22].Value = "код формы собственности";
        Worksheet.Cells[1, 23].Value = "код ОКПО правообладателя";
        Worksheet.Cells[1, 24].Value = "вид";
        Worksheet.Cells[1, 25].Value = "номер";
        Worksheet.Cells[1, 26].Value = "дата";
        Worksheet.Cells[1, 27].Value = "поставщика или получателя";
        Worksheet.Cells[1, 28].Value = "перевозчика";
        Worksheet.Cells[1, 29].Value = "наименование";
        Worksheet.Cells[1, 30].Value = "тип";
        Worksheet.Cells[1, 31].Value = "номер";

        #endregion

        #region 1.5

        Worksheet = excelPackage.Workbook.Worksheets.Add("Операции по форме 1.5");

        Worksheet.Cells[1, 1].Value = "Рег. №";
        Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        Worksheet.Cells[1, 3].Value = "ОКПО";
        Worksheet.Cells[1, 4].Value = "Форма";
        Worksheet.Cells[1, 5].Value = "Дата начала периода";
        Worksheet.Cells[1, 6].Value = "Дата конца периода";
        Worksheet.Cells[1, 7].Value = "Номер корректировки";
        Worksheet.Cells[1, 8].Value = "Количество строк";
        Worksheet.Cells[1, 9].Value = "№ п/п";
        Worksheet.Cells[1, 10].Value = "код";
        Worksheet.Cells[1, 11].Value = "дата";
        Worksheet.Cells[1, 12].Value = $"номер паспорта (сертификата) ЗРИ,{Environment.NewLine}акта определения характеристик ОЗИИ";
        Worksheet.Cells[1, 13].Value = "тип";
        Worksheet.Cells[1, 14].Value = "радионуклиды";
        Worksheet.Cells[1, 15].Value = "номер";
        Worksheet.Cells[1, 16].Value = "количество, шт";
        Worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
        Worksheet.Cells[1, 18].Value = "дата выпуска";
        Worksheet.Cells[1, 19].Value = "статус РАО";
        Worksheet.Cells[1, 20].Value = "вид";
        Worksheet.Cells[1, 21].Value = "номер";
        Worksheet.Cells[1, 22].Value = "дата";
        Worksheet.Cells[1, 23].Value = "поставщика или получателя";
        Worksheet.Cells[1, 24].Value = "перевозчика";
        Worksheet.Cells[1, 25].Value = "наименование";
        Worksheet.Cells[1, 26].Value = "тип";
        Worksheet.Cells[1, 27].Value = "заводской номер";
        Worksheet.Cells[1, 28].Value = "наименование";
        Worksheet.Cells[1, 29].Value = "код";
        Worksheet.Cells[1, 30].Value = "Код переработки / сортировки РАО";
        Worksheet.Cells[1, 31].Value = "Субсидия, %";
        Worksheet.Cells[1, 32].Value = "Номер мероприятия ФЦП";
        Worksheet.Cells[1, 33].Value = "Номер договора";

        #endregion

        return Task.CompletedTask;
    }

    #endregion

    #endregion

    #region GetFilteredForm

    /// <summary>
    /// Получение отфильтрованного списка форм отчётности 1.1/1.5.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="formPasList">Список уникальных параметров паспортов.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="pasNum">Искомый номер паспорта.</param>
    /// <param name="factoryNum">Искомый заводской номер.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отфильтрованный список форм отчётности 1.1/1.5.</returns>
    private static async Task<IEnumerable<Form1>> GetFilteredForm(DBModel db, List<PasUniqDataDTO> formPasList, string formNum, 
        string pasNum, string factoryNum, CancellationTokenSource cts)
    {
        List<Form1> filteredForm = [];
        foreach (var form in formPasList
                     .Where(form => ComparePasParam(form.PasNum + form.FacNum, pasNum + factoryNum)))
        {
            Form1 formToAdd = formNum switch
            {
                "1.1" => await db.form_11
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(form11 => form11.Report != null && form11.Report.Reports != null && form11.Report.Reports.Master_DB != null)
                    .Include(form11 => form11.Report).ThenInclude(rep => rep!.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(form11 => form11.Report).ThenInclude(rep => rep!.Rows11)
                    .FirstAsync(form11 => form11.Id == form.Id, cts.Token),

                "1.5" => await db.form_15
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(form15 => form15.Report != null && form15.Report.Reports != null && form15.Report.Reports.Master_DB != null)
                    .Include(form15 => form15.Report).ThenInclude(rep => rep!.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(form15 => form15.Report).ThenInclude(rep => rep!.Rows15)
                    .FirstAsync(form15 => form15.Id == form.Id, cts.Token),

                _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
            };
            filteredForm.Add(formToAdd);
        }
        return filteredForm;
    }

    #endregion

    #region GetPasUniqData

    /// <summary>
    /// Получение списка уникальных параметров паспортов для форм отчётности 1.1/1.5.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список уникальных параметров паспортов.</returns>
    private static async Task<List<PasUniqDataDTO>> GetPasUniqData(DBModel db, string formNum, CancellationTokenSource cts)
    {
        var query = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable);

        return formNum switch
        {
            "1.1" => await query.Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
                .Where(x => x.DBObservable != null)
                .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.1")
                    .SelectMany(rep => rep.Rows11
                        .Select(form11 =>
                            new PasUniqDataDTO(form11.Id,
                                form11.FactoryNumber_DB,
                                form11.PassportNumber_DB))))
                .ToListAsync(cts.Token),

            "1.5" => await query.Include(x => x.Report_Collection).ThenInclude(x => x.Rows15)
                .Where(x => x.DBObservable != null)
                .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.5")
                    .SelectMany(rep => rep.Rows15
                        .Select(form15 =>
                            new PasUniqDataDTO(form15.Id,
                                form15.FactoryNumber_DB,
                                form15.PassportNumber_DB))))
                .ToListAsync(cts.Token),

            _ => throw new NotImplementedException()
        };
    }

    #endregion

    #region RemoveForbiddenChars

    /// <summary>
    /// Удаление запрещённых символов.
    /// </summary>
    /// <param name="str">Строчка.</param>
    /// <returns>Строчка без запрещённых символов.</returns>
    private static string RemoveForbiddenChars(string str)
    {
        str = str.Replace(" ", "").Replace(Environment.NewLine, "");
        str = ForbiddenCharsRegex().Replace(str, "_");
        return str;
    }

    /// <summary>
    /// Регулярное выражение, находящее запрещённые символы.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex("[\\\\/:*?\"<>|]")]
    private static partial Regex ForbiddenCharsRegex();

    #endregion

    #region PasUniqDataDTO

    /// <summary>
    /// DTO уникальных данных паспорта.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="facNum">Заводской номер.</param>
    /// <param name="pasNum">Номер паспорта.</param>
    private class PasUniqDataDTO(int id, string facNum, string pasNum)
    {
        public readonly int Id = id;

        public readonly string FacNum = facNum;

        public readonly string PasNum = pasNum;
    }

    #endregion
}