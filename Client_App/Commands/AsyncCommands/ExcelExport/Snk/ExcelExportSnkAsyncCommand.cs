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
using Models.Collections;
using Models.DBRealization;
using System.Reflection;
using System.Collections.Generic;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using static Client_App.Resources.StaticStringMethods;
using Client_App.ViewModels.ProgressBar;
using AvaloniaEdit;
using System.Text.RegularExpressions;

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
        await CheckRepsAndRepPresence(formNum, progressBar, cts);
        var selectedReports = mainWindow!.SelectedReports.First() as Reports;

        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"СНК_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения", 
            $"Выгрузка СНК {formNum} {regNum}_{okpo}", ExportType);
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(8, "Запрос даты формирования СНК");
        var (endSnkDate, snkParams) = await AskSnkEndDate(progressBar, cts);

        progressBarVM.SetProgressBar(10, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, endSnkDate);

        progressBarVM.SetProgressBar(15, "Загрузка инвентаризационных отчётов");
        var inventoryReportDtoList = await GetInventoryReportDtoList(db, selectedReports.Id, endSnkDate, cts);

        progressBarVM.SetProgressBar(17, "Загрузка операций инвентаризации");
        var inventoryFormsDtoList = await GetInventoryFormsDtoList(db, inventoryReportDtoList, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(20, "Загрузка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, selectedReports.Id, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(21, "Загрузка операций перезарядки");
        var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, selectedReports.Id, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(22, "Формирование списка учётных единиц");
        var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);

        progressBarVM.SetProgressBar(30, "Формирование СНК");
        var unitInStockDtoList = await GetUnitInStockDtoList(uniqueUnitWithAllOperationDictionary, progressBarVM);

        //progressBarVM.SetProgressBar(23, "Формирование списка всех операций");
        //var unionFormsDtoList = await GetUnionFormsDtoList(inventoryFormsDtoList, plusMinusFormsDtoList);

        //progressBarVM.SetProgressBar(25, "Формирование списка уникальных учётных единиц");
        //var uniqueAccountingUnitDtoList = await GetUniqueAccountingUnitDtoList(unionFormsDtoList);

        //progressBarVM.SetProgressBar(30, "Формирование СНК");
        //var unitInStockDtoList = await GetUnitInStockDtoList2(inventoryFormsDtoList, plusMinusFormsDtoList, uniqueAccountingUnitDtoList, progressBarVM);

        progressBarVM.SetProgressBar(60, "Загрузка форм");
        var fullFormsSnkList = await GetFullFormsSnkList(db, unitInStockDtoList, progressBarVM, cts);

        progressBarVM.SetProgressBar(85, "Проверка наличия");
        await CheckPresenceInSnk(fullFormsSnkList, endSnkDate, progressBar, cts);

        progressBarVM.SetProgressBar(90, "Заполнение строчек в .xlsx");
        await FillExcel(fullFormsSnkList);

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



    #region GetDictionary_UniqueUnitsWithOperations

    private class UniqueUnitDto(string facNum, string pasNum, string radionuclids, string type, int quantity, string packNumber)
    {
        public string FacNum { get; } = facNum;

        public string PasNum { get; } = pasNum;

        public string Radionuclids { get; } = radionuclids;

        public string Type { get; } = type;

        public int Quantity { get; } = quantity;

        public string PackNumber { get; } = packNumber;
    }

    /// <summary>
    /// Формирует словарь из уникальных учётных единиц и списков операций с ними.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO операций инвентаризации.</param>
    /// <param name="plusMinusFormsDtoList">Список DTO операций приема/передачи.</param>
    /// <param name="rechargeFormsDtoList">Список DTO операций перезарядки.</param>
    /// <returns>Словарь из уникальных учётных единиц и списков операций с ними.</returns>
    private static async Task<Dictionary<UniqueUnitDto, List<ShortForm11DTO>>> GetDictionary_UniqueUnitsWithOperations(
        List<ShortForm11DTO> inventoryFormsDtoList,
        List<ShortForm11DTO> plusMinusFormsDtoList,
        List<ShortForm11DTO> rechargeFormsDtoList)
    {
        var firstInventoryDate = inventoryFormsDtoList.Count == 0
            ? DateOnly.MinValue
            : inventoryFormsDtoList
                .OrderBy(x => x.OpDate)
                .Select(x => x.OpDate)
                .First();

        var firstDateInventoryList = inventoryFormsDtoList
            .Where(x => x.OpDate == firstInventoryDate)
            .ToList();

        var unionOperationList = firstDateInventoryList
            .Union(plusMinusFormsDtoList)
            .Union(rechargeFormsDtoList)
            .ToList();

        List<List<ShortForm11DTO>> groupedOperationList = [];
        List<ShortForm11DTO> currentGroup = [];
        var opCount = 0;
        foreach (var form in unionOperationList.OrderBy(x => x.OpDate))
        {
            opCount++;
            if (form.OpCode is not ("53" or "54"))
            {
                currentGroup.Add(form);
                if (opCount == unionOperationList.Count) groupedOperationList.Add([.. currentGroup]);
            }
            else
            {
                currentGroup.Add(form);
                groupedOperationList.Add([..currentGroup]);
                currentGroup.Clear();
            }
        }
        if (groupedOperationList.Count == 0) groupedOperationList.Add(currentGroup);

        Dictionary<UniqueUnitDto, List<ShortForm11DTO>> uniqueUnitWithAllOperationDictionary = [];
        var count = 0;
        
        foreach (var group in groupedOperationList)
        {
            count++;
            var count2 = 0;
            foreach (var form in group)
            {
                count2++;
                if (count == 64 && count2 == 474)
                {

                }
                if (form.OpCode is not ("53" or "54"))
                {
                    if (!uniqueUnitWithAllOperationDictionary
                            .Any(keyValuePair => keyValuePair.Key.PasNum == form.PasNum
                                                 && keyValuePair.Key.FacNum == form.FacNum
                                                 && keyValuePair.Key.Radionuclids == form.Radionuclids
                                                 && keyValuePair.Key.Type == form.Type
                                                 && keyValuePair.Key.PackNumber == form.PackNumber
                                                 && (SerialNumbersIsEmpty(keyValuePair.Key.PasNum, keyValuePair.Key.FacNum)
                                                     || keyValuePair.Key.Quantity == form.Quantity)))
                    {
                        var dto = new UniqueUnitDto(form.FacNum, form.PasNum, form.Radionuclids, form.Type, form.Quantity, form.PackNumber);
                        uniqueUnitWithAllOperationDictionary.Add(dto, [form]);
                    }
                    else
                    {
                        uniqueUnitWithAllOperationDictionary
                            .First(keyValuePair => keyValuePair.Key.PasNum == form.PasNum 
                                                   && keyValuePair.Key.FacNum == form.FacNum
                                                   && keyValuePair.Key.Radionuclids == form.Radionuclids
                                                   && keyValuePair.Key.Type == form.Type
                                                   && keyValuePair.Key.PackNumber == form.PackNumber 
                                                   && (SerialNumbersIsEmpty(keyValuePair.Key.PasNum, keyValuePair.Key.FacNum)
                                                       || keyValuePair.Key.Quantity == form.Quantity))
                            .Value.Add(form);
                    }
                }
                else
                {
                    var filteredDictionary = uniqueUnitWithAllOperationDictionary
                        .Where(keyValuePair => keyValuePair.Key.PasNum == form.PasNum
                                               && keyValuePair.Key.FacNum == form.FacNum
                                               && keyValuePair.Key.Radionuclids == form.Radionuclids
                                               && keyValuePair.Key.Type == form.Type 
                                               && (SerialNumbersIsEmpty(keyValuePair.Key.PasNum, keyValuePair.Key.FacNum)
                                                   || keyValuePair.Key.Quantity == form.Quantity))
                        .ToDictionary();

                    var lastForm = filteredDictionary
                        .SelectMany(x => x.Value)
                        .OrderByDescending(y => y.OpDate)
                        .First();
                    var pairWithLastOpDate = filteredDictionary
                        .First(x => x.Value.Contains(lastForm));

                    if (SerialNumbersIsEmpty(pairWithLastOpDate.Key.PasNum, pairWithLastOpDate.Key.FacNum))
                    {
                        var quantity = pairWithLastOpDate.Value
                            .FirstOrDefault(x => x.OpCode == "10")
                            ?.Quantity ?? 0; ;
                        foreach (var form11Dto in pairWithLastOpDate.Value)
                        {
                            if (PlusOperation.Contains(form11Dto.OpCode))
                            {
                                quantity += form11Dto.Quantity;
                            }
                            else if (MinusOperation.Contains(form11Dto.OpCode))
                            {
                                quantity -= form11Dto.Quantity;
                                quantity = Math.Max(0, quantity);
                            }
                        }
                        if (form.Quantity == quantity)
                        {
                            pairWithLastOpDate.Value.Add(form);
                            var dto = new UniqueUnitDto(form.FacNum, form.PasNum, form.Radionuclids, form.Type, form.Quantity, form.PackNumber);
                            uniqueUnitWithAllOperationDictionary.Remove(pairWithLastOpDate.Key);
                            uniqueUnitWithAllOperationDictionary.Add(dto, pairWithLastOpDate.Value);
                        }
                    }
                    else
                    {
                        pairWithLastOpDate.Value.Add(form);
                        var dto = new UniqueUnitDto(form.FacNum, form.PasNum, form.Radionuclids, form.Type, form.Quantity, form.PackNumber);
                        uniqueUnitWithAllOperationDictionary.Remove(pairWithLastOpDate.Key);
                        uniqueUnitWithAllOperationDictionary.Add(dto, pairWithLastOpDate.Value);
                    }

                }
            }
        }
        return await Task.FromResult(uniqueUnitWithAllOperationDictionary);
    }

    private static bool SerialNumbersIsEmpty(string? pasNum, string? facNum)
    {
        var regex = new Regex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]");
        var num1 = (pasNum ?? string.Empty)
            .ToLower()
            .Replace(" ", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("/", "")
            .Replace("\\", "");
        num1 = regex.Replace(num1, "");

        var num2 = (facNum ?? string.Empty)
            .ToLower()
            .Replace(" ", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("/", "")
            .Replace("\\", "");
        num2 = regex.Replace(num2, "");
        List<string> validStrings =
        [
            "",
            "-",
            AutoReplaceSimilarChars("бн"),
            AutoReplaceSimilarChars("прим"),
            AutoReplaceSimilarChars("примечание"),
        ];
        return validStrings.Contains(num1) && validStrings.Contains(num2);
    }

    #endregion

    #region CheckPresenceInSnk

    /// <summary>
    /// Проверяет наличие учётных единиц и при их отсутствии выводит сообщение.
    /// </summary>
    /// <param name="fullFormsSnkList">Список форм с данными отчётов и организации.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckPresenceInSnk(List<SnkForm11DTO> fullFormsSnkList, DateOnly endSnkDate,
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

    #region CheckRepsAndRepPresence

    /// <summary>
    /// Проверяет наличие выбранной организации. Проверяет наличие хотя бы одного отчёта, с выбранным номером формы.
    /// В случае отсутствия выводит соответствующее сообщение и закрывает команду.
    /// </summary>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckRepsAndRepPresence(string formNum, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;

        //Аннотации врут, не убирай
        if (mainWindow!.SelectedReports is null || mainWindow.SelectedReports.First() is not Reports selectedReports)  
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        else if (selectedReports.Report_Collection.All(rep => rep.FormNum_DB != formNum))
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Не удалось совершить выгрузку СНК," +
                        $"{Environment.NewLine}поскольку у выбранной организации отсутствуют отчёты по форме {formNum}.",
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
    /// <param name="date">Дата, на которую формируется СНК.</param>
    private async Task FillExcelHeaders(ExcelPackage excelPackage, DateOnly date)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add($"СНК на {date.ToShortDateString()}");

        #region Headers

        Worksheet.Cells[1, 1].Value = "№ п/п";
        Worksheet.Cells[1, 2].Value = "Номер паспорта (сертификата)";
        Worksheet.Cells[1, 3].Value = "тип";
        Worksheet.Cells[1, 4].Value = "радионуклиды";
        Worksheet.Cells[1, 5].Value = "номер";
        Worksheet.Cells[1, 6].Value = "количество, шт.";
        Worksheet.Cells[1, 7].Value = "суммарная активность, Бк";
        Worksheet.Cells[1, 8].Value = "код ОКПО изготовителя";
        Worksheet.Cells[1, 9].Value = "дата выпуска";
        Worksheet.Cells[1, 10].Value = "категория";
        Worksheet.Cells[1, 11].Value = "НСС, мес";
        Worksheet.Cells[1, 12].Value = "Номер УКТ";

        #endregion

        await AutoFitColumns();
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

    #region FillExcel

    /// <summary>
    /// Заполняет заголовки Excel пакета.
    /// </summary>
    /// <param name="fullFormsSnkList">Список полных форм СНК.</param>
    private Task FillExcel(List<SnkForm11DTO> fullFormsSnkList)
    {
        var currentRow = 2;
        var currentForm = 1;
        foreach (var form in fullFormsSnkList)
        {
            Worksheet.Cells[currentRow, 1].Value = currentForm;
            Worksheet.Cells[currentRow, 2].Value = ConvertToExcelString(form.PasNum);
            Worksheet.Cells[currentRow, 3].Value = ConvertToExcelString(form.Type);
            Worksheet.Cells[currentRow, 4].Value = ConvertToExcelString(form.Radionuclids);
            Worksheet.Cells[currentRow, 5].Value = ConvertToExcelString(form.FacNum);
            Worksheet.Cells[currentRow, 6].Value = form.Quantity is 0 ? "-" : form.Quantity;
            Worksheet.Cells[currentRow, 7].Value = ConvertToExcelDouble(form.Activity);
            Worksheet.Cells[currentRow, 8].Value = ConvertToExcelString(form.CreatorOKPO);
            Worksheet.Cells[currentRow, 9].Value = ConvertToExcelDate(form.CreationDate, Worksheet, currentRow, 8);
            Worksheet.Cells[currentRow, 10].Value = form.Category is 0 ? "-" : form.Category;
            Worksheet.Cells[currentRow, 11].Value = form.SignedServicePeriod is 0 ? "-" : form.SignedServicePeriod;
            Worksheet.Cells[currentRow, 12].Value = ConvertToExcelString(form.PackNumber);
            currentRow++;
            currentForm++;
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
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список форм с данными отчётов и организации.</returns>
    private static async Task<List<SnkForm11DTO>> GetFullFormsSnkList(DBModel db, List<ShortForm11DTO> unitInStockDtoList,
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<SnkForm11DTO> formsList = [];
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;
        foreach (var unit in unitInStockDtoList)
        {
            var form = await db.form_11
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id == unit.Id && x.Report != null && x.Report.Reports != null)
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
                    unit.PackNumber))
                .FirstAsync(cts.Token);

            formsList.Add(form);

            progressBarDoubleValue += (double)30 / unitInStockDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка {currentUnitNum} формы из {unitInStockDtoList.Count}",
                "Загрузка форм");
            currentUnitNum++;
        }
        return formsList;

    }

    #endregion

    #region GetUnitInStockDtoList

    /// <summary>
    /// Формирует список DTO учётных единиц в наличии на дату.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO инвентаризационных операций.</param>
    /// <param name="plusMinusFormsDtoList">Список DTO операций приёма-передачи.</param>
    /// <param name="uniqueAccountingUnitDtoList">Список DTO уникальных учётных единиц.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <returns>Список DTO учётных единиц в наличии на дату.</returns>
    private static Task<List<ShortForm11DTO>> GetUnitInStockDtoList2(List<ShortForm11DTO> inventoryFormsDtoList, 
        List<ShortForm11DTO> plusMinusFormsDtoList, List<UniqueAccountingUnitDTO> uniqueAccountingUnitDtoList, 
        AnyTaskProgressBarVM progressBarVM)
    {
        var firstInventoryDate = inventoryFormsDtoList.Count == 0
            ? DateOnly.MinValue
            : inventoryFormsDtoList
                .OrderBy(x => x.OpDate)
                .Select(x => x.OpDate)
                .First();

        var unitInStockList = inventoryFormsDtoList
            .Where(x => x.OpDate == firstInventoryDate)
            .ToList();

        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;

        foreach (var unit in uniqueAccountingUnitDtoList)
        {
            var quantity = unitInStockList
                .FirstOrDefault(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type 
                                     == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type)
                ?.Quantity ?? 0;
            
            var inventoryWithCurrentUnit = inventoryFormsDtoList
                .Where(x => x.OpDate >= firstInventoryDate 
                            && x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type 
                            == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type)
            .DistinctBy(x => x.OpDate)
            .OrderBy(x => x.OpDate)
            .ToList();

            var operationsWithCurrentUnit = plusMinusFormsDtoList
                .Where(x => x.OpDate >= firstInventoryDate 
                            && x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type 
                            == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type)
            .OrderBy(x => x.OpDate)
            .ToList();

            List<ShortForm11DTO> operationsWithCurrentUnitWithoutDuplicates = [];
            foreach (var group in operationsWithCurrentUnit.GroupBy(x => x.OpDate))
            {
                var countPlus = group
                    .Where(x => PlusOperation.Contains(x.OpCode))
                    .Sum(x => x.Quantity);

                var countMinus = group
                    .Where(x => MinusOperation.Contains(x.OpCode))
                    .Sum(x => x.Quantity);

                var givenReceivedPerDayAmount = countPlus - countMinus;

                switch (givenReceivedPerDayAmount)
                {
                    case > 0:
                    {
                        var lastOp = group.Last(x => PlusOperation.Contains(x.OpCode));
                        lastOp.Quantity = givenReceivedPerDayAmount;
                        operationsWithCurrentUnitWithoutDuplicates.Add(lastOp);
                        break;
                    }
                    case 0:
                    {
                        break;
                    }
                    case < 0:
                    {
                        var lastOp = group.Last(x => MinusOperation.Contains(x.OpCode));
                        lastOp.Quantity = int.Abs(givenReceivedPerDayAmount);
                        operationsWithCurrentUnitWithoutDuplicates.Add(lastOp);
                        break;
                    }
                }
            }
            foreach (var operation in operationsWithCurrentUnitWithoutDuplicates)
            {
                if (PlusOperation.Contains(operation.OpCode))
                {
                    quantity += operation.Quantity;
                }
                else if (MinusOperation.Contains(operation.OpCode))
                {
                    quantity -= operation.Quantity;
                    quantity = Math.Max(0, quantity);
                }
            }
            var lastOperationWithUnit = operationsWithCurrentUnit
                .Union(inventoryWithCurrentUnit
                    .Where(x => x.OpDate >= firstInventoryDate))
                .OrderByDescending(x => x.OpDate)
                .FirstOrDefault();

            if (lastOperationWithUnit == null) continue;

            var currentUnit = unitInStockList
                .FirstOrDefault(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type
                                     == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type);

            if (currentUnit != null) unitInStockList.Remove(currentUnit);
            if (quantity > 0)
            {
                lastOperationWithUnit.Quantity = quantity;
                unitInStockList.Add(lastOperationWithUnit);
            }
            progressBarDoubleValue += (double)30 / uniqueAccountingUnitDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
            $"Проверено {currentUnitNum} единиц из {uniqueAccountingUnitDtoList.Count}",
                "Проверка наличия");
            currentUnitNum++;
        }
        return Task.FromResult(unitInStockList);
    }

    private static Task<List<ShortForm11DTO>> GetUnitInStockDtoList(
        Dictionary<UniqueUnitDto, List<ShortForm11DTO>> uniqueUnitWithAllOperationDictionary, 
        AnyTaskProgressBarVM progressBarVM)
    {
        List<ShortForm11DTO> unitInStockList = [];
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;
        foreach (var unit in uniqueUnitWithAllOperationDictionary)
        {
            if (SerialNumbersIsEmpty(unit.Key.PasNum, unit.Key.FacNum))
            {
                var quantity = unit.Value
                    .FirstOrDefault(x => x.OpCode == "10")
                    ?.Quantity ?? 0;

                List<ShortForm11DTO> operationsWithCurrentUnitWithoutDuplicates = [];
                foreach (var group in unit.Value.GroupBy(x => x.OpDate))
                {
                    var countPlus = group
                        .Where(x => PlusOperation.Contains(x.OpCode))
                        .Sum(x => x.Quantity);

                    var countMinus = group
                        .Where(x => MinusOperation.Contains(x.OpCode))
                        .Sum(x => x.Quantity);

                    var givenReceivedPerDayAmount = countPlus - countMinus;

                    switch (givenReceivedPerDayAmount)
                    {
                        case > 0:
                        {
                            var lastOp = group.Last(x => PlusOperation.Contains(x.OpCode));
                            lastOp.Quantity = givenReceivedPerDayAmount;
                            operationsWithCurrentUnitWithoutDuplicates.Add(lastOp);
                            break;
                        }
                        case 0:
                        {
                            break;
                        }
                        case < 0:
                        {
                            var lastOp = group.Last(x => MinusOperation.Contains(x.OpCode));
                            lastOp.Quantity = int.Abs(givenReceivedPerDayAmount);
                            operationsWithCurrentUnitWithoutDuplicates.Add(lastOp);
                            break;
                        }
                    }
                }

                foreach (var operation in operationsWithCurrentUnitWithoutDuplicates)
                {
                    if (PlusOperation.Contains(operation.OpCode))
                    {
                        quantity += operation.Quantity;
                    }
                    else if (MinusOperation.Contains(operation.OpCode))
                    {
                        quantity -= operation.Quantity;
                        quantity = Math.Max(0, quantity);
                    }
                }

                var lastOperationWithUnit = unit.Value
                    .OrderByDescending(x => x.OpDate)
                    .FirstOrDefault();

                if (lastOperationWithUnit == null) continue;

                var currentUnit = unitInStockList
                    .FirstOrDefault(x => x.PasNum == unit.Key.PasNum 
                                         && x.FacNum == unit.Key.FacNum 
                                         && x.Radionuclids == unit.Key.Radionuclids 
                                         && x.Type == unit.Key.Type
                                         && x.PackNumber == unit.Key.PackNumber);

                if (currentUnit != null) unitInStockList.Remove(currentUnit);

                if (quantity > 0)
                {
                    lastOperationWithUnit.Quantity = quantity;
                    unitInStockList.Add(lastOperationWithUnit);
                }
            }
            else
            {
                var inStock = unit.Value
                    .Any(x => x.OpCode == "10");
                foreach (var form in unit.Value)
                {
                    if (PlusOperation.Contains(form.OpCode)) inStock = true;
                    else if (MinusOperation.Contains(form.OpCode)) inStock = false;
                }
                if (inStock)
                {
                    var lastOperationWithUnit = unit.Value
                        .OrderByDescending(x => x.OpDate)
                        .FirstOrDefault();

                    if (lastOperationWithUnit == null) continue;

                    unitInStockList.Add(lastOperationWithUnit);
                }

            }
            progressBarDoubleValue += (double)30 / uniqueUnitWithAllOperationDictionary.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {currentUnitNum} единиц из {uniqueUnitWithAllOperationDictionary.Count}",
                "Проверка наличия");
            currentUnitNum++;
        }

        return Task.FromResult(unitInStockList);
    }

    #endregion

    private static Task<object> ShowPopup<TPopup>(TPopup popup) where TPopup : Window
    {
        var task = new TaskCompletionSource<object>();
        popup.ShowDialog(Desktop.MainWindow);
        popup.Focus();
        return task.Task;
    }
}