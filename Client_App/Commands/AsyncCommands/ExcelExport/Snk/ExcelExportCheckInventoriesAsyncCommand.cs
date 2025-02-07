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
using System.Reactive;
using Models.Forms;

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

        progressBarVM.SetProgressBar(17, "Формирование периодов инвентаризаций");
        var inventoryDatesTupleList = await GetInventoryDatesTupleList(inventoryDatesList);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(19, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, inventoryDatesList);

        var reportIds = await GetReportIds(db, selectedReports.Id, cts);

        progressBarVM.SetProgressBar(20, "Формирование списка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, reportIds, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(25, "Загрузка операций перезарядки");
        var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, selectedReports.Id, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(25, "Загрузка нулевых операций");
        var zeroFormsDtoList = await GetZeroFormsDtoList(db, selectedReports.Id, rechargeFormsDtoList, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(35, "Формирование списка учётных единиц");
        var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);

        await GetInventoryErrorsAndSnk(db, uniqueUnitWithAllOperationDictionary, inventoryFormsDtoList, inventoryDatesList, inventoryDatesTupleList, inventoryDuplicateErrors, excelPackage, progressBarVM, cts);

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
                new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                AutoReplaceSimilarChars(x.FacNum),
                x.OpCode,
                DateOnly.FromDateTime(DateTime.Parse(x.OpDate)),
                AutoReplaceSimilarChars(x.PackNumber),
                AutoReplaceSimilarChars(x.PasNum),
                x.Quantity ?? 0,
                AutoReplaceSimilarChars(x.Radionuclids),
                AutoReplaceSimilarChars(x.Type)))
            .Union(rechargeFormsDtoList)
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();
    }

    private static async Task GetInventoryErrorsAndSnk(
        DBModel db,
        Dictionary<UniqueUnitDto, List<ShortForm11DTO>> uniqueUnitWithAllOperationDictionary, 
        List<ShortForm11DTO> inventoryFormsDtoList,
        List<DateOnly> inventoryDatesList,
        List<(DateOnly, DateOnly)> inventoryDatesTupleList, 
        List<ShortForm11DTO> inventoryDuplicateErrorsDtoList,
        ExcelPackage excelPackage,
        AnyTaskProgressBarVM progressBarVM, 
        CancellationTokenSource cts)
    {
        List<ShortForm11DTO> unitInStockDtoList = [];

        unitInStockDtoList.AddRange(uniqueUnitWithAllOperationDictionary
            .Where(unit => Enumerable
                .Any(unit.Value, x => 
                    x.OpCode == "10" && x.OpDate == inventoryDatesTupleList[0].Item1))
            .Select(unit => unit.Value.First()));

        Dictionary<DateOnly, List<ShortForm11DTO>> unitInStockDictionary = [];
        unitInStockDictionary.Add(inventoryDatesList[0], unitInStockDtoList.ToList());

        foreach (var (firstInventoryDate, secondInventoryDate) in inventoryDatesTupleList)
        {
            //1. Нет в инвентаризациях, но последняя операция - плюсовая.
            List<ShortForm11DTO> registeredAndNotInventoriedErrorsList = [];

            //2. Есть в первой инвентаризации, нет во второй, нет +- операций.
            List<ShortForm11DTO> missingFromInventoryErrorsList = [];

            //3. Есть во второй инвентаризации, последняя операция не плюсовая.
            List<ShortForm11DTO> givenUnitIsInventoriedErrorsList = [];

            //4. Нет в первой инвентаризации, первая операция на передачу.
            List<ShortForm11DTO> unInventoriedUnitGivenAwayErrorsList = [];

            //5. Двойное снятие с учёта
            List<ShortForm11DTO> reDeRegistrationErrorsList = [];

            //6. Двойная постановка на учёт
            List<ShortForm11DTO> reRegistrationErrorsList = [];

            //7. Все нулевые операции, идущие сразу после отсутствующей инвентаризации.
            List<ShortForm11DTO> zeroOperationErrorsList = [];

            //8. Для пустых зав.№ и № паспорта, отдано большее количество, чем было на момент операции.
            List<ShortForm11DTO> quantityGivenExceedsAvailable = [];

            ////9. Для не пустого зав.№ или № паспорта, повторная операция инвентаризации.
            //var currentInventoryDuplicateErrorsDtoList = inventoryDuplicateErrorsDtoList
            //    .Where(x => x.OpDate == secondInventoryDate);

            foreach (var (unit, allOperations) in uniqueUnitWithAllOperationDictionary)
            {
                var currentOperations = allOperations
                    .Where(x => x.OpDate >= firstInventoryDate && x.OpDate <= secondInventoryDate)
                    .OrderBy(x => x.OpDate)
                    .ThenBy(x => x.RepDto.StartPeriod)
                    .ThenBy(x => x.NumberInOrder)
                    .ToList();

                #region GetErrors

                var firstPlusMinusOperation = currentOperations
                    .FirstOrDefault(x => PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode));

                var lastPlusMinusOperation = currentOperations
                    .LastOrDefault(x => PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode));

                var firstInventoryOperation = currentOperations
                    .FirstOrDefault(x => x.OpCode == "10" && x.OpDate == firstInventoryDate);

                var secondInventoryOperation = currentOperations
                    .FirstOrDefault(x => x.OpCode == "10" && x.OpDate == secondInventoryDate);

                //1. Нет в инвентаризациях, но последняя операция - плюсовая.
                if (firstInventoryOperation is null
                    && secondInventoryOperation is null
                    && PlusOperation.Contains(lastPlusMinusOperation?.OpCode))
                {
                    registeredAndNotInventoriedErrorsList.Add(currentOperations.Last());
                }

                //2. Есть в первой инвентаризации, нет во второй, нет +- операций.
                if (firstInventoryOperation is not null
                    && secondInventoryOperation is null
                    && !currentOperations.Any(x => PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode)))
                {
                    missingFromInventoryErrorsList.Add(firstInventoryOperation);
                }

                //3. Есть во второй инвентаризации, последняя операция не плюсовая.
                if (secondInventoryOperation is not null
                    && MinusOperation.Contains(lastPlusMinusOperation?.OpCode))
                {
                    givenUnitIsInventoriedErrorsList.Add(secondInventoryOperation);
                }

                //4. Нет в первой инвентаризации, первая операция на передачу.
                if (firstInventoryOperation is null
                    && firstPlusMinusOperation is not null
                    && MinusOperation.Contains(firstPlusMinusOperation.OpCode))
                {
                    unInventoriedUnitGivenAwayErrorsList.Add(firstPlusMinusOperation);
                }

                //8. Для пустых зав.№ и № паспорта, отдано большее количество, чем было на момент операции.
                if (firstInventoryOperation is null 
                    && currentOperations.Count > 0
                    && currentOperations.FirstOrDefault(x => x.OpCode != "10") != null
                    && IsZeroOperation(currentOperations.FirstOrDefault(x => x.OpCode != "10")))
                {
                    zeroOperationErrorsList
                        .AddRange(currentOperations
                            .Where(x => x.OpCode != "10")
                            .TakeWhile(IsZeroOperation));
                }

                #endregion

                #region GetInStock

                #region EmptySerialNum

                if (SerialNumbersIsEmpty(unit.PasNum, unit.FacNum))
                {
                    var quantity = currentOperations
                        .FirstOrDefault(x => x.OpCode == "10")
                        ?.Quantity ?? 0;

                    var operationsWithoutDuplicates = await GetOperationsWithoutDuplicates(currentOperations);

                    foreach (var operation in operationsWithoutDuplicates)
                    {
                        if (PlusOperation.Contains(operation.OpCode))
                        {
                            quantity += operation.Quantity;
                        }
                        else if (MinusOperation.Contains(operation.OpCode))
                        {
                            //8. Для пустых зав.№ и № паспорта, отдано большее количество, чем было на момент операции.
                            if (quantity < operation.Quantity)
                            {
                                quantityGivenExceedsAvailable.Add(operation);
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

                    var currentUnit = unitInStockDtoList
                        .FirstOrDefault(x => x.PasNum == unit.PasNum
                                             && x.FacNum == unit.FacNum
                                             && x.Radionuclids == unit.Radionuclids
                                             && x.Type == unit.Type
                                             && x.PackNumber == unit.PackNumber);

                    if (currentUnit != null) unitInStockDtoList.Remove(currentUnit);

                    if (quantity > 0)
                    {
                        lastOperationWithUnit.Quantity = quantity;
                        unitInStockDtoList.Add(lastOperationWithUnit);
                    }
                }

                #endregion

                #region NotEmptySerialNum
                
                else
                {
                    var inStock = unitInStockDtoList.Any(x => x.PasNum == unit.PasNum
                                                           && x.FacNum == unit.FacNum
                                                           && x.Radionuclids == unit.Radionuclids
                                                           && x.Type == unit.Type
                                                           && x.PackNumber == unit.PackNumber
                                                           && x.Quantity == unit.Quantity);

                    foreach (var form in currentOperations)
                    {
                        if (PlusOperation.Contains(form.OpCode))
                        {
                            //6. Двойная постановка на учёт
                            if (inStock) reRegistrationErrorsList.Add(form);
                            else inStock = true;
                        }
                        else if (MinusOperation.Contains(form.OpCode))
                        {
                            //5. Двойное снятие с учёта
                            if (!inStock) reDeRegistrationErrorsList.Add(form);
                            else inStock = false;
                        }
                    }
                    if (inStock)
                    {
                        var lastOperationWithUnit = currentOperations
                            .OrderByDescending(x => x.OpDate)
                            .FirstOrDefault();

                        if (lastOperationWithUnit != null)
                        {
                            unitInStockDtoList.Add(lastOperationWithUnit);
                        }
                    }
                    else
                    {
                        var unitInStock = unitInStockDtoList.FirstOrDefault(x => x.PasNum == unit.PasNum
                                                    && x.FacNum == unit.FacNum
                                                    && x.Radionuclids == unit.Radionuclids
                                                    && x.Type == unit.Type
                                                    && x.PackNumber == unit.PackNumber
                                                    && x.Quantity == unit.Quantity);
                        if (unitInStock != null)
                        {
                            unitInStockDtoList.Remove(unitInStock);
                        }
                    }
                }

                #endregion

                #endregion
            }
            var currentUnitInStockDtoList = new List<ShortForm11DTO>();
            currentUnitInStockDtoList.AddRange(unitInStockDtoList);
            unitInStockDictionary.Add(secondInventoryDate, currentUnitInStockDtoList);
        }

        foreach (var inventoryDate in inventoryDatesList)
        {
            var currentInventoryList = inventoryFormsDtoList
                .Where(x => x.OpDate == inventoryDate)
                .ToList();

            unitInStockDictionary.TryGetValue(inventoryDate, out var currentUnitInStockDtoList);
            currentUnitInStockDtoList ??= [];

            var fullFormsSnkList = await GetFullFormsSnkList(db, currentUnitInStockDtoList, progressBarVM, cts);
            var fullFormsInventoryList = await GetFullFormsSnkList(db, currentInventoryList, progressBarVM, cts);

            var snkWorksheet = excelPackage.Workbook.Worksheets
                .First(x => x.Name == $"СНК на {inventoryDate:dd.MM.yy}");
            var errorsWorksheet = excelPackage.Workbook.Worksheets
                .First(x => x.Name == $"Ошибки на {inventoryDate:dd.MM.yy}");

            var fullFormsSnkOrderedList = fullFormsSnkList
                .OrderBy(x => fullFormsInventoryList
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

            var fullFormsInventoryOrderedList = fullFormsInventoryList
                .OrderBy(x => fullFormsInventoryList
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

            var countMatches = fullFormsSnkOrderedList
                .Count(x => fullFormsInventoryOrderedList
                    .Any(y => x.PasNum == y.PasNum
                              && x.Type == y.Type
                              && x.Radionuclids == y.Radionuclids
                              && x.FacNum == y.FacNum
                              && x.Quantity == y.Quantity
                              && x.PackNumber == y.PackNumber));

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
                snkWorksheet.Cells[snkRow, 9].Value = form.CreationDate;
                snkWorksheet.Cells[snkRow, 10].Value = form.Category;
                snkWorksheet.Cells[snkRow, 11].Value = form.SignedServicePeriod;
                snkWorksheet.Cells[snkRow, 12].Value = form.PackNumber;
                snkRow++;
            }

            var snkTable = snkWorksheet.Tables.Add(snkWorksheet.Cells[2, 1, snkRow - 1, 12], $"СНК_{inventoryDate}");
            snkTable.TableStyle = TableStyles.Medium2;
            snkWorksheet.Cells[1, 1, snkRow - 1, 12].Style.Border.BorderAround(ExcelBorderStyle.Thick);

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
                snkWorksheet.Cells[inventoryRow, 21].Value = inventoryForm.CreationDate;
                snkWorksheet.Cells[inventoryRow, 22].Value = inventoryForm.Category;
                snkWorksheet.Cells[inventoryRow, 23].Value = inventoryForm.SignedServicePeriod;
                snkWorksheet.Cells[inventoryRow, 24].Value = inventoryForm.PackNumber;
                inventoryRow++;
            }

            var inventoryTable = snkWorksheet.Tables.Add(snkWorksheet.Cells[2, 13, inventoryRow - 1, 24], $"Инвентаризация_{inventoryDate}");
            inventoryTable.TableStyle = TableStyles.Medium2;
            snkWorksheet.Cells[1, 13, inventoryRow - 1, 24].Style.Border.BorderAround(ExcelBorderStyle.Thick);

            if (countMatches > 0)
            {
                for(var column = 1; column <= 24; column++)
                {
                    for (var row = 3; row <= countMatches + 2; row++)
                    {
                        snkWorksheet.Cells[row, column].Style.Fill.SetBackground(System.Drawing.Color.LightGreen, ExcelFillStyle.LightGray);
                    }
                }
            }

            var currentErrorsRow = 3;
        }

    }

    #region IsZeroOperation

    private static bool IsZeroOperation(ShortForm11DTO? dto)
    {
        if (dto is null)
        {
            return false;
        }
        var isZeroOperation = !PlusOperation.Contains(dto.OpCode)
                              && !MinusOperation.Contains(dto.OpCode)
                              && dto.OpCode != "10";

        return isZeroOperation;
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
                .Where(x => x.Id == unit.Id && x.Report != null && x.Report.Reports != null && x.Report.Reports.DBObservable != null)
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

            progressBarDoubleValue += (double)25 / unitInStockDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка {currentUnitNum} формы из {unitInStockDtoList.Count}",
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
            Worksheet.Cells[1, 5].Value = "Номер строчки";
            Worksheet.Cells[1, 6].Value = "Номер паспорта (сертификата)";
            Worksheet.Cells[1, 7].Value = "тип";
            Worksheet.Cells[1, 8].Value = "радионуклиды";
            Worksheet.Cells[1, 9].Value = "номер";
            Worksheet.Cells[1, 10].Value = "количество, шт.";
            Worksheet.Cells[1, 11].Value = "суммарная активность, Бк";
            Worksheet.Cells[1, 12].Value = "код ОКПО изготовителя";
            Worksheet.Cells[1, 13].Value = "дата выпуска";
            Worksheet.Cells[1, 14].Value = "категория";
            Worksheet.Cells[1, 15].Value = "НСС, мес";
            Worksheet.Cells[1, 16].Value = "Номер УКТ";

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

    #region GetInventoryDatesTupleList

    /// <summary>
    /// Формирование списка кортежей периодов инвентаризаций.
    /// </summary>
    /// <param name="inventoryDates">Отсортированный по возрастанию список дат операций инвентаризации.</param>
    /// <returns>Список кортежей периодов инвентаризаций.</returns>
    private static Task<List<(DateOnly, DateOnly)>> GetInventoryDatesTupleList(List<DateOnly> inventoryDates)
    {
        List<(DateOnly, DateOnly)> inventoryDatesTupleList = [];
        for (var i = 0; i < inventoryDates.Count - 1; i++)
        {
            inventoryDatesTupleList.Add((inventoryDates[i], inventoryDates[i + 1]));
        }

        return Task.FromResult(inventoryDatesTupleList);
    }

    #endregion
}