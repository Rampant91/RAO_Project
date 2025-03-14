using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Views;
using Client_App.Views.ProgressBar;
using Models.Collections;
using Models.DBRealization;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using Client_App.ViewModels.ProgressBar;
using Microsoft.EntityFrameworkCore;
using DynamicData;
using static Client_App.Resources.StaticStringMethods;

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
            $"Проверка инвентаризаций", ExportType);
        await CheckRepsAndRepPresence(formNum, progressBar, cts);

        var selectedReports = mainWindow!.SelectedReports.First() as Reports;
        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"СНК_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(6, "Запрос даты формирования СНК");
        var(endSnkDate, snkParams) = await AskSnkEndDate(progressBar, cts);

        progressBarVM.SetProgressBar(8, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(13, "Формирование списка инвентаризационных отчётов");
        var inventoryReportDtoList = await GetInventoryReportDtoList(db, selectedReports.Id, endSnkDate, cts);

        progressBarVM.SetProgressBar(14, "Проверка наличия инвентаризации");
        await CheckInventoryFormPresence(inventoryReportDtoList, formNum, progressBar, cts);

        progressBarVM.SetProgressBar(15, "Формирование списка операций инвентаризации");
        var (firstSnkDate, inventoryFormsDtoList, inventoryDuplicateErrors) = 
            await GetInventoryFormsDtoList(db, inventoryReportDtoList, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(16, "Получение списка дат инвентаризаций");
        var inventoryDatesList = await GetInventoryDatesList(inventoryFormsDtoList);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(19, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, inventoryDatesList);

        progressBarVM.SetProgressBar(20, "Загрузка списка отчётов");
        var reportIds = await GetReportIds(db, selectedReports.Id, cts);

        progressBarVM.SetProgressBar(21, "Формирование списка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, reportIds, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(25, "Загрузка операций перезарядки");
        var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, selectedReports.Id, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(30, "Загрузка нулевых операций");
        var zeroFormsDtoList = await GetZeroFormsDtoList(db, selectedReports.Id, rechargeFormsDtoList, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(35, "Формирование списка учётных единиц");
        var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList, zeroFormsDtoList);

        progressBarVM.SetProgressBar(40, "Формирование списков СНК и ошибок");
        await GetInventoryErrorsAndSnk(db, uniqueUnitWithAllOperationDictionary, inventoryFormsDtoList, inventoryDatesList, inventoryDuplicateErrors, firstSnkDate, excelPackage, progressBarVM, cts);

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

    private static async Task<List<ShortForm11DTO>> GetZeroFormsDtoList(DBModel db, int repsId, List<ShortForm11DTO> rechargeFormsDtoList, DateOnly firstSnkDate, DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
        var reportIds = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Report_Collection)
            .Where(reps => reps.DBObservable != null && reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1"))
            .Select(rep => rep.Id)
            .ToListAsync(cts.Token);

        var zeroOperationDtoList = await db.form_11
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Report)
            .Where(x => x.Report != null
                        && reportIds.Contains(x.Report.Id)
                        && !PlusOperation.Contains(x.OperationCode_DB)
                        && !MinusOperation.Contains(x.OperationCode_DB)
                        && x.OperationCode_DB != "10"
                        && x.OperationCode_DB != "53"
                        && x.OperationCode_DB != "54")
            .Select(form => new ShortForm11StringDatesDTO
            {
                Id = form.Id,
                NumberInOrder = form.NumberInOrder_DB,
                RepId = form.Report!.Id,
                StDate = form.Report.StartPeriod_DB,
                EndDate = form.Report.EndPeriod_DB,
                FacNum = snkParams == null || snkParams.CheckFacNum ? form.FactoryNumber_DB : string.Empty,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PackNumber = snkParams == null || snkParams.CheckPackNumber ? form.PackNumber_DB : string.Empty,
                PasNum = snkParams == null || snkParams.CheckPasNum ? form.PassportNumber_DB : string.Empty,
                Quantity = form.Quantity_DB,
                Radionuclids = snkParams == null || snkParams.CheckRadionuclids ? form.Radionuclids_DB : string.Empty,
                Type = snkParams == null || snkParams.CheckType ? form.Type_DB : string.Empty
            })
            .ToListAsync(cts.Token);

        return zeroOperationDtoList
            .Where(x => DateTime.TryParse(x.OpDate, out var opDateTime)
                        && DateOnly.TryParse(x.StDate, out _)
                        && DateOnly.TryParse(x.EndDate, out _)
                        && DateOnly.FromDateTime(opDateTime) >= firstSnkDate
                        && DateOnly.FromDateTime(opDateTime) <= endSnkDate)
            .Select(x => new ShortForm11DTO(
                x.Id,
                x.NumberInOrder,
                new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                x.OpCode,
                DateOnly.FromDateTime(DateTime.Parse(x.OpDate)),
                AutoReplaceSimilarChars(x.PasNum),
                AutoReplaceSimilarChars(x.Type),
                AutoReplaceSimilarChars(x.Radionuclids),
                AutoReplaceSimilarChars(x.FacNum),
                x.Quantity ?? 0,
                AutoReplaceSimilarChars(x.PackNumber)
                ))
            .Union(rechargeFormsDtoList)
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();
    }

    private static async Task GetInventoryErrorsAndSnk(DBModel db, Dictionary<UniqueUnitDto, List<ShortForm11DTO>> uniqueUnitWithAllOperationDictionary, 
        List<ShortForm11DTO> inventoryFormsDtoList, List<DateOnly> inventoryDatesList, List<ShortForm11DTO> inventoryDuplicateErrorsDtoList,
        DateOnly primaryInventoryDate, ExcelPackage excelPackage, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<ShortForm11DTO> unitInStockDtoList = [];

        Dictionary<DateOnly, List<ShortForm11DTO>> unitInStockByDateDictionary = [];
        Dictionary<DateOnly, List<InventoryErrorsShortDto>> inventoryErrorsByDateDictionary = [];

        var currentInventoryDateIndex = 0;
        foreach (var inventoryDate in inventoryDatesList)
        {
            // Инициализируем список ошибок, сразу добавляя в него повторные операции инвентаризации.
            var errorsDtoList = new List<InventoryErrorsShortDto>()
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
                unitInStockByDateDictionary.Add(inventoryDate, unitInStockDtoList.ToList());

                // Добавляем в словарь ошибок текущую дату и список ошибок на эту дату.
                inventoryErrorsByDateDictionary.Add(inventoryDate, errorsDtoList.ToList());

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

                #region GetErrors

                var secondInventoryOperation = currentOperations
                    .FirstOrDefault(x => x.OpCode == "10" && x.OpDate == inventoryDate);

                #endregion

                #region GetInStock

                #region SerialNumIsEmpty

                if (SerialNumbersIsEmpty(unit.PasNum, unit.FacNum))
                {
                    var currentPackNumber = currentOperations.FirstOrDefault()?.PackNumber ?? unit.PackNumber;

                    var currentUnitInStock = unitInStockDtoList
                        .FirstOrDefault(x =>
                            x.PasNum == unit.PasNum
                            && x.FacNum == unit.FacNum
                            && x.Radionuclids == unit.Radionuclids
                            && x.Type == unit.Type
                            && x.PackNumber == currentPackNumber);

                    var quantity = currentUnitInStock?.Quantity ?? 0;

                    //2. Есть в СНК на первую инвентаризацию, нет второй инвентаризации, нет минусовых операций.
                    if (currentUnitInStock != null 
                        && quantity != 0 
                        && secondInventoryOperation is null
                        && !currentOperations.Any(x => MinusOperation.Contains(x.OpCode))
                        && inventoryDate != inventoryDatesList[^1])
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.MissingFromInventoryUnit, currentUnitInStock));
                    }

                    var operationsWithoutDuplicates = await GetOperationsWithoutDuplicates(currentOperations);

                    var firstPlusMinusOperation = operationsWithoutDuplicates
                        .FirstOrDefault(x => PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode));

                    var lastPlusMinusOperation = operationsWithoutDuplicates
                        .LastOrDefault(x => PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode));

                    //1. Нет во второй инвентаризации, но последняя +- операция плюсовая.
                    if (secondInventoryOperation is null
                        && lastPlusMinusOperation is not null
                        && PlusOperation.Contains(lastPlusMinusOperation.OpCode)
                        && inventoryDate != inventoryDatesList[^1])
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.RegisteredAndNotInventoriedUnit, lastPlusMinusOperation));
                    }

                    foreach (var operation in operationsWithoutDuplicates)
                    {
                        if (PlusOperation.Contains(operation.OpCode))
                        {
                            quantity += operation.Quantity;
                        }
                        else if (MinusOperation.Contains(operation.OpCode))
                        {
                            //4. Снятие с учёта не стоявшего на учёте ЗРИ.
                            if (quantity == 0)
                            {
                                errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.UnInventoriedUnitGivenAway, firstPlusMinusOperation!));
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
                        && (currentUnitInStock is null || quantity == 0))
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

                #region SerialNumIsNotEmpty

                else
                {
                    var operationsWithoutMutuallyExclusive = await GetOperationsWithoutMutuallyExclusive(currentOperations);
                    var allOperationsWithoutMutuallyExclusive = await GetOperationsWithoutMutuallyExclusive(allOperations);

                    #region GetErrors

                    var firstPlusMinusOperation = operationsWithoutMutuallyExclusive
                        .FirstOrDefault(x => PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode));

                    var lastPlusMinusOperation = operationsWithoutMutuallyExclusive
                        .LastOrDefault(x => PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode));

                    //1. Нет во второй инвентаризации, но последняя +- операция плюсовая.
                    if (secondInventoryOperation is null
                        && lastPlusMinusOperation is not null
                        && PlusOperation.Contains(lastPlusMinusOperation.OpCode)
                        && inventoryDate != inventoryDatesList[^1])
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.RegisteredAndNotInventoriedUnit, lastPlusMinusOperation));
                    }

                    for (var i = 0; i < operationsWithoutMutuallyExclusive.Count; i++)
                    {
                        var currentForm = operationsWithoutMutuallyExclusive[i];
                        var previousPlusMinusOperation = operationsWithoutMutuallyExclusive
                            .GetRange(0, i)
                            .Where(x => PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode))
                            .Reverse()
                            .FirstOrDefault();

                        if (MinusOperation.Contains(currentForm.OpCode) 
                            && previousPlusMinusOperation is not null 
                            && MinusOperation.Contains(previousPlusMinusOperation.OpCode))
                        {
                            //5. Двойное снятие с учёта
                            errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.ReDeRegistration, currentForm));
                        }
                        else if (PlusOperation.Contains(currentForm.OpCode) 
                                 && previousPlusMinusOperation is not null &&
                                 PlusOperation.Contains(previousPlusMinusOperation.OpCode))
                        {
                            //7. Двойная постановка на учёт
                            errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.ReRegistration, currentForm));
                        }
                    }

                    #endregion

                    #region GetInStock

                    var inStock = allOperationsWithoutMutuallyExclusive.Any(x => x.OpCode == "10" && x.OpDate == primaryInventoryDate);
                    var  inStockOnPreviousInventoryDate = inStock;

                    foreach (var form in allOperationsWithoutMutuallyExclusive.Where(x => x.OpDate <= previousInventoryDate))
                    {
                        if (PlusOperation.Contains(form.OpCode)) inStockOnPreviousInventoryDate = true;
                        else if (MinusOperation.Contains(form.OpCode)) inStockOnPreviousInventoryDate = false;
                    }

                    //2. Есть в СНК на первую инвентаризацию, нет второй инвентаризации, нет минусовых операций.
                    if (inStockOnPreviousInventoryDate
                        && secondInventoryOperation is null
                        && !currentOperations.Any(x => MinusOperation.Contains(x.OpCode))
                        && inventoryDate != inventoryDatesList[^1])
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(
                            InventoryErrorTypeEnum.MissingFromInventoryUnit, allOperationsWithoutMutuallyExclusive.Last(x => x.OpDate <= inventoryDate)));
                    }

                    //4. Снятие с учёта не стоявшего на учёте ЗРИ.
                    if (!inStockOnPreviousInventoryDate
                        && firstPlusMinusOperation is not null
                        && MinusOperation.Contains(firstPlusMinusOperation.OpCode))
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.UnInventoriedUnitGivenAway, firstPlusMinusOperation));
                    }

                    //6. Постановка на учёт имеющегося в наличии ЗРИ.
                    if (inStockOnPreviousInventoryDate
                        && firstPlusMinusOperation is not null
                        && PlusOperation.Contains(firstPlusMinusOperation.OpCode))
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.InventoriedUnitReceived, firstPlusMinusOperation));
                    }

                    foreach (var form in allOperations.Where(x => x.OpDate <= inventoryDate))
                    {
                        if (IsZeroOperation(form) 
                            && !inStock 
                            && form.OpDate >= previousInventoryDate 
                            && form.OpDate <= inventoryDate
                            && form.OpDate >= primaryInventoryDate)
                        {
                            //8. Нулевые операции с отсутствующим в наличии ЗРИ.
                            errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.ZeroOperationWithUnInventoriedUnit, form));
                        }
                        if (PlusOperation.Contains(form.OpCode)) inStock = true;
                        else if (MinusOperation.Contains(form.OpCode)) inStock = false;
                    }

                    var lastOperationWithUnit = operationsWithoutMutuallyExclusive
                        .OrderByDescending(x => x.OpDate)
                        .FirstOrDefault();

                    if (lastOperationWithUnit == null) continue;

                    var currentPackNumber = operationsWithoutMutuallyExclusive.FirstOrDefault()?.PackNumber ?? unit.PackNumber;

                    var unitInStock = unitInStockDtoList.FirstOrDefault(x => x.PasNum == unit.PasNum
                                                                             && x.FacNum == unit.FacNum
                                                                             && x.Radionuclids == unit.Radionuclids
                                                                             && x.Type == unit.Type
                                                                             && x.PackNumber == currentPackNumber
                                                                             && x.Quantity == unit.Quantity);

                    //3. Есть во второй инвентаризации, но отсутствует в СНК на дату второй инвентаризации.
                    if (secondInventoryOperation is not null
                        && !inStock)
                    {
                        errorsDtoList.Add(new InventoryErrorsShortDto(InventoryErrorTypeEnum.GivenUnitIsInventoried, secondInventoryOperation));
                    }

                    if (unitInStock is not null)
                    {
                        unitInStockDtoList.Remove(unitInStock!);
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
            unitInStockByDateDictionary.Add(inventoryDate, unitInStockDtoList.ToList());

            // Добавляем в словарь ошибок текущую дату и список ошибок на эту дату.
            inventoryErrorsByDateDictionary.Add(inventoryDate, errorsDtoList.ToList());

            currentInventoryDateIndex++;
        }

        #region FillSnkPages

        double progressBarDoubleValue = progressBarVM.ValueBar;
        // Заполняем страницы с СНК
        foreach (var (inventoryDate, unitInStockOnDateDtoList) in unitInStockByDateDictionary)
        {
            var currentInventoryDtoList = inventoryFormsDtoList
                    .Where(x => x.OpDate == inventoryDate)
                    .ToList();

            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка полных форм на {inventoryDate}",
                "Загрузка полных форм");

            var fullFormsSnkList = await GetFullFormsSnkList(db, unitInStockOnDateDtoList, inventoryDate, progressBarVM, cts);
            var fullFormsInventoryList = await GetFullFormsSnkList(db, currentInventoryDtoList, inventoryDate, progressBarVM, cts);

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
                .ThenBy(x => x.Type)
                .ThenBy(x => x.Radionuclids)
                .ThenBy(x => x.Quantity)
                .ThenBy(x => x.PackNumber)
                .ToList();

            var snkRow = 3;
            foreach (var form in fullFormsSnkOrderedList)
            {
                snkWorksheet.Cells[snkRow, 1].Value = snkRow - 2;
                snkWorksheet.Cells[snkRow, 2].Value = form.PasNum;
                snkWorksheet.Cells[snkRow, 3].Value = form.Type;
                snkWorksheet.Cells[snkRow, 4].Value = form.Radionuclids;
                snkWorksheet.Cells[snkRow, 5].Value = form.FacNum;
                snkWorksheet.Cells[snkRow, 6].Value = form.Quantity;
                snkWorksheet.Cells[snkRow, 7].Value = form.Activity;
                snkWorksheet.Cells[snkRow, 8].Value = form.CreatorOKPO;
                snkWorksheet.Cells[snkRow, 9].Value = ConvertToExcelDate(form.CreationDate, snkWorksheet, snkRow, 9);
                snkWorksheet.Cells[snkRow, 10].Value = form.Category;
                snkWorksheet.Cells[snkRow, 11].Value = form.SignedServicePeriod;
                snkWorksheet.Cells[snkRow, 12].Value = form.PackNumber;
                snkRow++;
            }

            var snkTable = snkWorksheet.Tables.Add(snkWorksheet.Cells[2, 1, snkRow - 1, 12], $"СНК_{inventoryDate}");
            snkTable.TableStyle = TableStyles.Medium2;
            snkWorksheet.Cells[1, 1, snkRow - 1, 12].Style.Border.BorderAround(ExcelBorderStyle.Thick);

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
                    .ThenBy(x => x.Type)
                    .ThenBy(x => x.Radionuclids)
                    .ThenBy(x => x.Quantity)
                    .ThenBy(x => x.PackNumber)
                    .ToList();

            var inventoryRow = 3;
            foreach (var inventoryForm in fullFormsInventoryOrderedList)
            {
                snkWorksheet.Cells[inventoryRow, 13].Value = inventoryRow - 2;
                snkWorksheet.Cells[inventoryRow, 14].Value = inventoryForm.PasNum;
                snkWorksheet.Cells[inventoryRow, 15].Value = inventoryForm.Type;
                snkWorksheet.Cells[inventoryRow, 16].Value = inventoryForm.Radionuclids;
                snkWorksheet.Cells[inventoryRow, 17].Value = inventoryForm.FacNum;
                snkWorksheet.Cells[inventoryRow, 18].Value = inventoryForm.Quantity;
                snkWorksheet.Cells[inventoryRow, 19].Value = inventoryForm.Activity;
                snkWorksheet.Cells[inventoryRow, 20].Value = inventoryForm.CreatorOKPO;
                snkWorksheet.Cells[inventoryRow, 21].Value = ConvertToExcelDate(inventoryForm.CreationDate, snkWorksheet, inventoryRow, 21);
                snkWorksheet.Cells[inventoryRow, 22].Value = inventoryForm.Category;
                snkWorksheet.Cells[inventoryRow, 23].Value = inventoryForm.SignedServicePeriod;
                snkWorksheet.Cells[inventoryRow, 24].Value = inventoryForm.PackNumber;
                inventoryRow++;
            }

            var inventoryTable = snkWorksheet.Tables.Add(snkWorksheet.Cells[2, 13, inventoryRow - 1, 24], $"Инвентаризация_{inventoryDate}");
            inventoryTable.TableStyle = TableStyles.Medium2;
            snkWorksheet.Cells[1, 13, inventoryRow - 1, 24].Style.Border.BorderAround(ExcelBorderStyle.Thick);

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
                for (var column = 1; column <= 24; column++)
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

        #endregion

        #region FillInventoryErrorsPages

        foreach (var (inventoryDate, inventoryErrorsDtoList) in inventoryErrorsByDateDictionary)
        {
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка полных форм ошибок на {inventoryDate}");

            var fullFormsErrorsList = 
                await GetFullFormsSnkList(db, inventoryErrorsDtoList, inventoryErrorsByDateDictionary.Count, progressBarVM, cts);

            var errorsWorksheet = excelPackage.Workbook.Worksheets
                .First(x => x.Name == $"Ошибки на {inventoryDate:dd.MM.yy}");

            var fullFormsErrorsOrderedList = fullFormsErrorsList
                .OrderBy(x => x.StPer)
                .ThenBy(x => x.EndPer)
                .ThenBy(x => x.RowNumber)
                .ThenBy(x => x.ErrorType)
                .ThenBy(x => x.PasNum)
                .ThenBy(x => x.Type)
                .ThenBy(x => x.Radionuclids)
                .ThenBy(x => x.Quantity)
                .ThenBy(x => x.PackNumber)
                .ToList();

            var errorsRow = 2;
            foreach (var form in fullFormsErrorsOrderedList)
            {
                errorsWorksheet.Cells[errorsRow, 1].Value = errorsRow - 1;
                errorsWorksheet.Cells[errorsRow, 2].Value = GetErrorDescriptionByType(form.ErrorType);
                errorsWorksheet.Cells[errorsRow, 3].Value = ConvertToExcelDate(form.StPer.ToShortDateString(), errorsWorksheet, errorsRow, 3);
                errorsWorksheet.Cells[errorsRow, 4].Value = ConvertToExcelDate(form.EndPer.ToShortDateString(), errorsWorksheet, errorsRow, 4);
                errorsWorksheet.Cells[errorsRow, 5].Value = form.RowNumber;
                errorsWorksheet.Cells[errorsRow, 6].Value = form.OpCode;
                errorsWorksheet.Cells[errorsRow, 7].Value = form.OpDate;
                errorsWorksheet.Cells[errorsRow, 8].Value = form.PasNum;
                errorsWorksheet.Cells[errorsRow, 9].Value = form.Type;
                errorsWorksheet.Cells[errorsRow, 10].Value = form.Radionuclids;
                errorsWorksheet.Cells[errorsRow, 11].Value = form.FacNum;
                errorsWorksheet.Cells[errorsRow, 12].Value = form.Quantity;
                errorsWorksheet.Cells[errorsRow, 13].Value = form.Activity;
                errorsWorksheet.Cells[errorsRow, 14].Value = form.CreatorOKPO;
                errorsWorksheet.Cells[errorsRow, 15].Value = ConvertToExcelDate(form.CreationDate, errorsWorksheet, errorsRow, 15);
                errorsWorksheet.Cells[errorsRow, 16].Value = form.Category;
                errorsWorksheet.Cells[errorsRow, 17].Value = form.SignedServicePeriod;
                errorsWorksheet.Cells[errorsRow, 18].Value = form.PackNumber;
                errorsRow++;
            }

            var errorsTable = errorsWorksheet.Tables
                .Add(errorsWorksheet.Cells[1, 1, errorsWorksheet.Dimension.Rows, errorsWorksheet.Dimension.Columns], $"Ошибки_{inventoryDate}");
            errorsTable.TableStyle = TableStyles.Medium2;
            errorsWorksheet.Cells[1, 1, errorsWorksheet.Dimension.Rows, errorsWorksheet.Dimension.Columns].Style.Border.BorderAround(ExcelBorderStyle.Thick);

            progressBarDoubleValue += (double)20 / inventoryErrorsByDateDictionary.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка полных форм ошибок на {inventoryDate}");
        }

        #endregion
    }

    #region IsZeroOperation

    private static bool IsZeroOperation(ShortForm11DTO? dto)
    {
        if (dto is null) return false;

        return !PlusOperation.Contains(dto.OpCode)
               && !MinusOperation.Contains(dto.OpCode)
               && dto.OpCode is not ("10" or "63" or "64");
    }

    #endregion

    #region GetFullForm

    /// <summary>
    /// Загрузка из БД полных форм вместе с данными отчётов и организации.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="unitInStockDtoList">Список DTO учётных единиц в наличии на дату.</param>
    /// <param name="inventoryDate">Дата инвентаризации, на которую загружаются формы.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список форм с данными отчётов и организации.</returns>
    private static async Task<List<SnkForm11DTO>> GetFullFormsSnkList(DBModel db, List<ShortForm11DTO> unitInStockDtoList, DateOnly inventoryDate,
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
                .FirstAsync(cts.Token);

            formsList.Add(form);

            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка {currentUnitNum} формы из {unitInStockDtoList.Count}",
                $"Загрузка форм на {inventoryDate}");
            currentUnitNum++;
        }
        return formsList;

    }

    private static async Task<List<InventoryErrorForm11DTO>> GetFullFormsSnkList(DBModel db, 
        List<InventoryErrorsShortDto> inventoryErrorsDtoList, int datesCount, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<InventoryErrorForm11DTO> formsList = [];
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;
        foreach (var error in inventoryErrorsDtoList)
        {
            var form = await db.form_11
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
                .FirstAsync(cts.Token);

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

    #region GetOperationsWithoutDuplicates

    private static Task<List<ShortForm11DTO>> GetOperationsWithoutDuplicates(List<ShortForm11DTO> operationList)
    {
        List<ShortForm11DTO> operationsWithoutDuplicates = [];
        foreach (var group in operationList.GroupBy(x => x.OpDate))
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
                    operationsWithoutDuplicates.Add(lastOp);
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
                    operationsWithoutDuplicates.Add(lastOp);
                    break;
                }
            }
        }
        return Task.FromResult(operationsWithoutDuplicates);
    }

    #endregion

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
    private async Task FillExcelHeaders(ExcelPackage excelPackage, List<DateOnly> inventoryDatesList)
    {
        var count = 0;
        foreach (var date in inventoryDatesList)
        {
            count++;

            #region SNK_On_Date_Page
            
            #region SNK_Table

            Worksheet = excelPackage.Workbook.Worksheets.Add($"СНК на {date:dd.MM.yy}");

            Worksheet.Cells[1, 1, 1, 12].Merge = true;
            Worksheet.Cells[1, 1, 1, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            Worksheet.Cells[1, 1].Value = $"СНК на {date.ToShortDateString()}";

            Worksheet.Cells[2, 1].Value = "№ п/п";
            Worksheet.Cells[2, 2].Value = "Номер паспорта (сертификата)";
            Worksheet.Cells[2, 3].Value = "тип";
            Worksheet.Cells[2, 4].Value = "радионуклиды";
            Worksheet.Cells[2, 5].Value = "номер";
            Worksheet.Cells[2, 6].Value = "количество, шт.";
            Worksheet.Cells[2, 7].Value = "суммарная активность, Бк";
            Worksheet.Cells[2, 8].Value = "код ОКПО изготовителя";
            Worksheet.Cells[2, 9].Value = "дата выпуска";
            Worksheet.Cells[2, 10].Value = "категория";
            Worksheet.Cells[2, 11].Value = "НСС, мес";
            Worksheet.Cells[2, 12].Value = "Номер УКТ";

            //var range = Worksheet.Cells[2, 1, 50, 12];
            //var tab = Worksheet.Tables.Add(range, $"Table{count}_1");
            //tab.TableStyle = TableStyles.Medium2;
            //Worksheet.Cells[1, 1, 50, 12].Style.Border.BorderAround(ExcelBorderStyle.Thick);

            #endregion

            #region Inventory_Table

            Worksheet.Cells[1, 13, 1, 24].Merge = true;
            Worksheet.Cells[1, 13, 1, 24].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            Worksheet.Cells[1, 13].Value = $"Инвентаризация на {date.ToShortDateString()}";

            Worksheet.Cells[2, 13].Value = "№ п/п";
            Worksheet.Cells[2, 14].Value = "Номер паспорта (сертификата)";
            Worksheet.Cells[2, 15].Value = "тип";
            Worksheet.Cells[2, 16].Value = "радионуклиды";
            Worksheet.Cells[2, 17].Value = "номер";
            Worksheet.Cells[2, 18].Value = "количество, шт.";
            Worksheet.Cells[2, 19].Value = "суммарная активность, Бк";
            Worksheet.Cells[2, 20].Value = "код ОКПО изготовителя";
            Worksheet.Cells[2, 21].Value = "дата выпуска";
            Worksheet.Cells[2, 22].Value = "категория";
            Worksheet.Cells[2, 23].Value = "НСС, мес";
            Worksheet.Cells[2, 24].Value = "Номер УКТ";

            //var range2 = Worksheet.Cells[2, 13, 50, 24];
            //var tab2 = Worksheet.Tables.Add(range2, $"Table{count}_2");
            //tab2.TableStyle = TableStyles.Medium2;
            //Worksheet.Cells[1, 13, 50, 24].Style.Border.BorderAround(ExcelBorderStyle.Thick);

            #endregion

            await AutoFitColumnsAndFreezeRows(2); 
            
            #endregion

            #region Errors_On_Date_Page

            Worksheet = excelPackage.Workbook.Worksheets.Add($"Ошибки на {date:dd.MM.yy}");

            #region Headers

            Worksheet.Cells[1, 1].Value = "№ п/п";
            Worksheet.Cells[1, 2].Value = "Описание ошибки";
            Worksheet.Cells[1, 3].Value = "Дата начала периода";
            Worksheet.Cells[1, 4].Value = "Дата конца периода";
            Worksheet.Cells[1, 5].Value = "Номер строки";
            Worksheet.Cells[1, 6].Value = "Код операции";
            Worksheet.Cells[1, 7].Value = "Дата операции";
            Worksheet.Cells[1, 8].Value = "Номер паспорта (сертификата)";
            Worksheet.Cells[1, 9].Value = "тип";
            Worksheet.Cells[1, 10].Value = "радионуклиды";
            Worksheet.Cells[1, 11].Value = "номер";
            Worksheet.Cells[1, 12].Value = "количество, шт.";
            Worksheet.Cells[1, 13].Value = "суммарная активность, Бк";
            Worksheet.Cells[1, 14].Value = "код ОКПО изготовителя";
            Worksheet.Cells[1, 15].Value = "дата выпуска";
            Worksheet.Cells[1, 16].Value = "категория";
            Worksheet.Cells[1, 17].Value = "НСС, мес";
            Worksheet.Cells[1, 18].Value = "Номер УКТ";

            #endregion

            //var range3 = Worksheet.Cells[1, 1, 50, 16];
            //var tab3 = Worksheet.Tables.Add(range3, $"Table{count}");
            //tab3.TableStyle = TableStyles.Medium2;
            //range3.Style.Border.BorderAround(ExcelBorderStyle.Thick);

            await AutoFitColumnsAndFreezeRows(1);
            
            #endregion
        }
    }

    #region AutoFitColumns

    /// <summary>
    /// Для текущей страницы Excel пакета подбирает ширину колонок и замораживает n строчек.
    /// </summary>
    private Task AutoFitColumnsAndFreezeRows(byte numberOfRowsToFreeze)
    {
        for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
        {
            if (OperatingSystem.IsWindows()) Worksheet.Column(col).AutoFit();
        }
        for (var row = 2; row <= numberOfRowsToFreeze + 1; row++)
        {
            Worksheet.View.FreezePanes(row, 1);
        }
        return Task.CompletedTask;
    }

    #endregion

    #endregion

    #region GetInventoryDatesList

    /// <summary>
    /// Формирование списка операций инвентаризации.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO операций инвентаризации, отсортированный по датам.</param>
    /// <returns>Список операций инвентаризации.</returns>
    private static Task<List<DateOnly>> GetInventoryDatesList(List<ShortForm11DTO> inventoryFormsDtoList)
    {
        var inventoryDates = inventoryFormsDtoList
            .Select(x => x.OpDate)
            .Distinct()
            .Order()
            .ToList();
        inventoryDates.Add(DateOnly.FromDateTime(DateTime.Now));

        return Task.FromResult(inventoryDates);
    }

    #endregion

    private class InventoryErrorForm11DTO(InventoryErrorTypeEnum errorType, DateOnly stPer, DateOnly endPer, int rowNumber, string facNum, 
        string opCode, string opDate, string pasNum, int quantity, string radionuclids, string type, string activity, string creatorOKPO, 
        string creationDate, short? category, float? signedServicePeriod, string packNumber)
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

        public readonly int Quantity = quantity;

        public readonly string Activity = activity;

        public readonly string CreatorOKPO = creatorOKPO;

        public readonly string CreationDate = creationDate;

        public readonly short? Category = category;

        public readonly float? SignedServicePeriod = signedServicePeriod;

        public readonly string PackNumber = packNumber;
    }

    private class InventoryErrorsShortDto(InventoryErrorTypeEnum errorTypeEnum, ShortForm11DTO dto)
    {
        public readonly InventoryErrorTypeEnum ErrorTypeEnum = errorTypeEnum;

        public readonly ShortForm11DTO Dto = dto;
    }

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
}