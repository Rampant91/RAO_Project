using Avalonia.Threading;
using System;
using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;
using DynamicData;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

public class ExcelExportSnkAsyncCommand : ExcelBaseAsyncCommand
{
    #region Properties
    
    private static readonly string[] MinusOperation =
    [
        "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "47", "65", "67", "71", "72", "81", "82", "83", "84", "98"
    ];

    private static readonly string[] PlusOperation =
    [
        "11", "12", "17", "31", "32", "35", "37", "38", "39", "58", "73", "74", "75", "85", "86", "87", "88", "97"
    ];

    #endregion

    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var formNum = parameter as string;

        var mainWindow = Desktop.MainWindow as MainWindow;
        var selectedReports = mainWindow!.SelectedReports.First() as Reports;
        if (selectedReports is null)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"СНК_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(9, "Создание временной БД", 
        $"Выгрузка СНК {formNum} {regNum}_{okpo}", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(11, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Формирование списка инвентаризационных отчётов");
        var inventoryReportDtoList = await GetInventoryReportDtoList(db, selectedReports.Id, cts);

        var firstInventoryReportDto = inventoryReportDtoList
            .OrderBy(x => x.StartPeriod)
            .ThenBy(x => x.EndPeriod)
            .First();
        var firstInventoryDate = firstInventoryReportDto.StartPeriod;

        progressBarVM.SetProgressBar(15, "Формирование списка операций инвентаризации");
        var inventoryFormsDtoList = await GetInventoryFormsDtoList(db, inventoryReportDtoList, cts);

        progressBarVM.SetProgressBar(17, "Формирование списка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, selectedReports.Id, cts);

        progressBarVM.SetProgressBar(19, "Формирование списка всех операций");
        var unionFormsDtoList = await GetUnionFormsDtoList(inventoryFormsDtoList, plusMinusFormsDtoList, firstInventoryDate);

        progressBarVM.SetProgressBar(21, "Формирование списка уникальных учётных единиц");
        var uniqueAccountingUnitDtoList = await GetUniqueAccountingUnitDtoList(unionFormsDtoList);

        var forms11DtoList = await GetForms11DtoList(db, inventoryFormsDtoList, plusMinusFormsDtoList, uniqueAccountingUnitDtoList, cts);
    }

    #region GetInventoryFormsDtoList

    /// <summary>
    /// Получение списка DTO операций инвентаризации.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="inventoryReportDtoList">Список DTO отчётов, содержащих операцию инвентаризации.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO операций инвентаризации.</returns>
    private static async Task<List<ShortForm11DTO>> GetInventoryFormsDtoList(DBModel db, List<ShortReportDTO> inventoryReportDtoList, 
        CancellationTokenSource cts)
    {
        List<ShortForm11DTO> inventoryFormsDtoList = [];
        foreach (var reportDto in inventoryReportDtoList)
        {
            var currentInventoryFormsStringDateDtoList = await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Rows11)
                .Where(rep => rep.Id == reportDto.Id)
                .SelectMany(rep => rep.Rows11
                    .Where(form => form.OperationCode_DB == "10")
                    .Select(form11 => new ShortForm11StringDateDTO
                    {
                        Id = form11.Id,
                        RepId = reportDto.Id,
                        StDate = reportDto.StartPeriod,
                        EndDate = reportDto.EndPeriod,
                        FacNum = form11.FactoryNumber_DB,
                        OpCode = form11.OperationCode_DB,
                        OpDate = form11.OperationDate_DB,
                        PasNum = form11.PassportNumber_DB,
                        Type = form11.Type_DB
                    }))
                .ToListAsync(cts.Token);

            var currentInventoryFormsDtoList = currentInventoryFormsStringDateDtoList
                .Where(x => DateOnly.TryParse(x.OpDate, out _))
                .Select(x => new ShortForm11DTO(
                    x.Id,
                    reportDto,
                    x.FacNum,
                    x.OpCode,
                    DateOnly.Parse(x.OpDate),
                    x.PasNum,
                    x.Type))
                .ToList();

            inventoryFormsDtoList.AddRange(currentInventoryFormsDtoList);
        }
        return inventoryFormsDtoList
            .OrderBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ThenBy(x => x.OpDate)
            .ToList();
    }

    #endregion

    #region GetInventaryReportDtoList

    /// <summary>
    /// Получение списка DTO отчётов по форме 1.1, содержащих хотя бы одну операцию с кодом 10.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id выбранной организации.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO отчётов по форме 1.1.</returns>
    private static async Task<List<ShortReportDTO>> GetInventoryReportDtoList(DBModel db, int repsId, CancellationTokenSource cts)
    {
        var inventoryReportDtoList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1" && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
                .Select(rep => new ShortReportStringDateDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB)))
            .ToListAsync(cts.Token);

        return inventoryReportDtoList
            .Where(x => DateOnly.TryParse(x.StartPeriod, out _) 
                        && DateOnly.TryParse(x.EndPeriod, out _))
            .Select(x => new ShortReportDTO(
                x.Id,
                DateOnly.Parse(x.StartPeriod),
                DateOnly.Parse(x.EndPeriod)))
            .OrderBy(x => x.StartPeriod)
            .ThenBy(x => x.EndPeriod)
            .ToList();
    }

    #endregion

    #region GetPlusMinusFormsDtoList

    /// <summary>
    /// Получение списка DTO форм с операциями приёма передачи.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id организации.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO форм с операциями приёма передачи.</returns>
    private static async Task<List<ShortForm11DTO>> GetPlusMinusFormsDtoList(DBModel db, int repsId, CancellationTokenSource cts)
    {
        var plusMinusOperationDtoList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .SelectMany(rep => rep.Rows11
                    .Where(form => form.Report != null 
                                   && (PlusOperation.Contains(form.OperationCode_DB) 
                                       || MinusOperation.Contains(form.OperationCode_DB)))))
                .Select(form11 => new ShortForm11StringDatesDTO
                {
                    Id = form11.Id,
                    RepId = form11.Report.Id,
                    StDate = form11.Report.StartPeriod_DB,
                    EndDate = form11.Report.EndPeriod_DB,
                    FacNum = form11.FactoryNumber_DB,
                    OpCode = form11.OperationCode_DB,
                    OpDate = form11.OperationDate_DB,
                    PasNum = form11.PassportNumber_DB,
                    Type = form11.Type_DB
                })
            .ToListAsync(cts.Token);

        return plusMinusOperationDtoList
            .Where(x => DateOnly.TryParse(x.OpDate, out _)
                                             && DateOnly.TryParse(x.StDate, out _)
                                             && DateOnly.TryParse(x.EndDate, out _))
            .Select(x => new ShortForm11DTO(
                x.Id,
                new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                x.FacNum,
                x.OpCode,
                DateOnly.Parse(x.OpDate),
                x.PasNum,
                x.Type))
            .OrderBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ThenBy(x => x.OpDate)
            .ToList();
    }

    #endregion

    #region GetUniqueAccountingUnitDtoList

    /// <summary>
    /// Получение отсортированного списка DTO уникальных учётных единиц с операциями инвентаризации, приёма или передачи.
    /// </summary>
    /// <param name="unionFormsDtoList">Список DTO всех операций инвентаризации, приёма или передачи.</param>
    /// <returns>Список DTO уникальных учётных единиц с операциями инвентаризации, приёма или передачи.</returns>
    private static Task<List<ShortForm11DTO>> GetUniqueAccountingUnitDtoList(List<ShortForm11DTO> unionFormsDtoList)
    {
        var uniqueAccountingUnitDtoList = unionFormsDtoList
            .DistinctBy(x => x.FacNum + x.Type + x.PasNum)
            .OrderBy(x => x.FacNum + x.Type + x.PasNum)
            .ToList();

        return Task.FromResult(uniqueAccountingUnitDtoList);
    }

    #endregion

    #region GetUnionFormsDtoList

    private static Task<List<ShortForm11DTO>> GetUnionFormsDtoList(List<ShortForm11DTO> inventoryFormsDtoList, 
        List<ShortForm11DTO> plusMinusFormsDtoList, DateOnly firstInventoryDate)
    {
        var unionFormsDtoList = inventoryFormsDtoList
            .Union(plusMinusFormsDtoList)
            .Where(x => x.OpDate >= firstInventoryDate)
            .ToList();

        return Task.FromResult(unionFormsDtoList);
    }

    #endregion

    private static async Task<List<ShortForm11DTO>> GetForms11DtoList(DBModel db, List<ShortForm11DTO> inventoryFormsDtoList, 
        List<ShortForm11DTO> plusMinusFormsDtoList, List<ShortForm11DTO> uniqueAccountingUnitDtoList, CancellationTokenSource cts)
    {
        var inventoryDates = inventoryFormsDtoList
            .Select(x => x.OpDate)
            .Distinct()
            .Order()
            .ToList();
        inventoryDates.Add(DateOnly.FromDateTime(DateTime.Now));

        List<(DateOnly, DateOnly)> inventoryDatesTupleList = [];
        for (var i = 0; i < inventoryDates.Count - 1; i++)
        {
            inventoryDatesTupleList.Add((inventoryDates[i], inventoryDates[i+1]));
        }

        List<ShortForm11DTO> unitInStockList = [];

        //ЗРИ, отсутствующие на момент операции передачи
        List<ShortForm11DTO> incorrectlyTransferredUnitsList = [];

        //ЗРИ, уже присутствующие на момент получения
        List<ShortForm11DTO> incorrectlyReceivedUnitsList = [];

        //ЗРИ, указанные в предыдущей инвентаризации, не снятые с учёта и не отражённые в следующей инвентаризации
        List<ShortForm11DTO> erroneouslyMissingUnitsList = [];

        //ЗРИ, указанные в предыдущей инвентаризации, снятые с учёта и отражённые в следующей инвентаризации
        List<ShortForm11DTO> mistakenlyPresentUnitsList = [];

        //ЗРИ, не указанные в предыдущей инвентаризации, без операций учёта и отражённые в следующей инвентаризации
        List<ShortForm11DTO> missingRegistrationUnitsList = [];

        //ЗРИ, отсутствующие на момент следующей инвентаризации и отражённые в ней
        List<ShortForm11DTO> erroneousInventoryPreviouslyTransferredUnitsList = [];

        //ЗРИ, указанные в предыдущей инвентаризации, повторно полученные и не проинвентаризированные
        List<ShortForm11DTO> previouslyInventoriedReobtainedNotInventoriedUnitsList = [];

        foreach (var unit in uniqueAccountingUnitDtoList)
        {
            foreach (var (firstInventoryDate, secondInventoryDate) in inventoryDatesTupleList)
            {
                var isLastInventory = secondInventoryDate == inventoryDates[^1];

                if (firstInventoryDate == inventoryDates[0])
                {
                    unitInStockList
                        .AddRange(inventoryFormsDtoList
                            .Where(x => x.OpDate == firstInventoryDate)
                            .DistinctBy(x => x.FacNum + x.Type + x.PasNum));
                }

                var inventoryWithCurrentUnit = inventoryFormsDtoList
                    .Where(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum
                                && x.OpDate >= firstInventoryDate && x.OpDate <= secondInventoryDate)
                    .DistinctBy(x => x.OpDate)
                    .OrderBy(x => x.OpDate)
                    .ToList();

                var inStock = unitInStockList.Any(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum);

                var operationsWithCurrentUnit = plusMinusFormsDtoList
                    .Where(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum
                                && x.OpDate >= firstInventoryDate && x.OpDate <= secondInventoryDate)
                    .OrderBy(x => x.OpDate)
                    .ToList();

                if (inventoryWithCurrentUnit.Count == 0 && operationsWithCurrentUnit.Count == 0) continue;

                var isReobtained = false;
                foreach (var operation in operationsWithCurrentUnit)
                {
                    switch (inStock)
                    {
                        case true when PlusOperation.Contains(operation.OpCode):
                            incorrectlyReceivedUnitsList.Add(operation);
                            isReobtained = true;
                            break;
                        case false when PlusOperation.Contains(operation.OpCode):
                            inStock = true;
                            break;
                        case false when MinusOperation.Contains(operation.OpCode):
                            incorrectlyTransferredUnitsList.Add(operation);
                            break;
                        case true when MinusOperation.Contains(operation.OpCode):
                            inStock = false;
                            break;
                    }
                }

                var lastOperationWithUnit = operationsWithCurrentUnit
                    .Union(inventoryWithCurrentUnit
                        .Where(x => x.OpDate >= firstInventoryDate || x.OpDate == secondInventoryDate))
                    .OrderByDescending(x => x.OpDate)
                    .First();

                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && !inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && inStock
                    && !isLastInventory)
                {
                    erroneouslyMissingUnitsList.Add(lastOperationWithUnit);
                }

                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !inStock
                    && !isLastInventory)
                {
                    mistakenlyPresentUnitsList.Add(lastOperationWithUnit);
                }

                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !inStock
                    && !isLastInventory)
                {
                    missingRegistrationUnitsList.Add(lastOperationWithUnit);
                }

                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !inStock
                    && !isLastInventory)
                {
                    erroneousInventoryPreviouslyTransferredUnitsList.Add(lastOperationWithUnit);
                }

                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && !inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && isReobtained
                    && !isLastInventory)
                {
                    previouslyInventoriedReobtainedNotInventoriedUnitsList.Add(lastOperationWithUnit);
                }

                unitInStockList.Remove(unit);
                if (inStock)
                {
                    unitInStockList.Add(lastOperationWithUnit);
                }
            }

        }
        return unitInStockList;
    }

    #region DTO

    private class ShortForm11DTO(int id, ShortReportDTO repDto, string facNum, string opCode, DateOnly opDate, string pasNum, string type)
    {
        public readonly int Id = id;

        public readonly ShortReportDTO RepDto = repDto;

        public readonly string FacNum = facNum;

        public readonly string OpCode = opCode;

        public readonly DateOnly OpDate = opDate;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;
    }

    private class ShortForm11StringDateDTO
    {
        public int Id { get; set; }

        public int RepId { get; set; }

        public DateOnly StDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string FacNum { get; set; }

        public string OpCode { get; set; }

        public string OpDate { get; set; }

        public string PasNum { get; set; }

        public string Type { get; set; }
    }

    private class ShortForm11StringDatesDTO
    {
        public int Id { get; set; }

        public int RepId { get; set; }

        public string StDate { get; set; }

        public string EndDate { get; set; }

        public string FacNum { get; set; }

        public string OpCode { get; set; }

        public string OpDate { get; set; }

        public string PasNum { get; set; }

        public string Type { get; set; }
    }

    private class ShortReportStringDateDTO(int id, string startPeriod, string endPeriod)
    {
        public readonly int Id = id;

        public readonly string StartPeriod = startPeriod;

        public readonly string EndPeriod = endPeriod;
    }

    private class ShortReportDTO(int id, DateOnly startPeriod, DateOnly endPeriod)
    {
        public readonly int Id = id;

        public readonly DateOnly StartPeriod = startPeriod;

        public readonly DateOnly EndPeriod = endPeriod;
    }

    #endregion
}