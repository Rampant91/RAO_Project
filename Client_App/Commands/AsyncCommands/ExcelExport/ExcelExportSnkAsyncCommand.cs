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
using DynamicData;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System.Reactive;

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
        var inventoryFormsDtoList = await GetInventoryFormsDtoList(db, firstInventoryReportDto.Id, cts);

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
    /// <param name="firstInventoryReportId">Id первого отчёта, содержащего операцию инвентаризации.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO операций инвентаризации.</returns>
    private static async Task<List<ShortForm11DTO>> GetInventoryFormsDtoList(DBModel db, int firstInventoryReportId, CancellationTokenSource cts)
    {
        var inventoryFormsDtoList2 = await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Rows11)
            .Where(rep => rep.Id == firstInventoryReportId)
            .SelectMany(rep => rep.Rows11
                .Where(form => form.OperationCode_DB == "10")
                .Select(form => new ShortForm12StringDateDTO(form.Id, form.Id, form.OperationDate_DB, form.PassportNumber_DB,
                    form.FactoryNumber_DB, form.OperationCode_DB, form.OperationDate_DB, form.PassportNumber_DB, form.Type_DB)))
            .ToListAsync(cts.Token);

        var inventoryFormsDtoList = await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(rep => rep.Id == firstInventoryReportId)
            .SelectMany(rep => rep.Rows11
                .Where(form => form.OperationCode_DB == "10") 
                .Select(form => new ShortForm11StringDateDTO(form.Id, new ShortReportStringDateDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB),
                    form.FactoryNumber_DB, form.OperationCode_DB, form.OperationDate_DB, form.PassportNumber_DB, form.Type_DB)))
            .ToListAsync(cts.Token);
        
        return inventoryFormsDtoList
            .Where(x => DateOnly.TryParse(x.OpDate, out _)
                        && DateOnly.TryParse(x.RepDto.StartPeriod, out _)
                        && DateOnly.TryParse(x.RepDto.EndPeriod, out _))
            .Select(x => new ShortForm11DTO(
                x.Id, 
                new ShortReportDTO(x.RepDto.Id, DateOnly.Parse(x.RepDto.StartPeriod), DateOnly.Parse(x.RepDto.EndPeriod)), 
                x.FacNum, 
                x.OpCode, 
                DateOnly.Parse(x.OpDate), 
                x.PasNum, 
                x.Type))
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
        var plusMinusOperationList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.Id == repsId)
            .SelectMany(reps => reps.Report_Collection)
            .SelectMany(rep => rep.Rows11
                .Where(form => PlusOperation.Contains(form.OperationCode_DB) || MinusOperation.Contains(form.OperationCode_DB))
                .Select(form => new ShortForm11StringDateDTO(form.Id, new ShortReportStringDateDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB), form.FactoryNumber_DB, form.OperationCode_DB,
                    form.OperationDate_DB, form.PassportNumber_DB, form.Type_DB)))
            .ToListAsync(cts.Token);

        return plusMinusOperationList
            .Where(x => DateOnly.TryParse(x.OpDate, out _) 
                        && DateOnly.TryParse(x.RepDto.StartPeriod, out _)
                        && DateOnly.TryParse(x.RepDto.EndPeriod, out _))
            .Select(x => new ShortForm11DTO(
                x.Id,
                new ShortReportDTO(x.RepDto.Id, DateOnly.Parse(x.RepDto.StartPeriod), DateOnly.Parse(x.RepDto.EndPeriod)),
                x.FacNum,
                x.OpCode,
                DateOnly.Parse(x.OpDate),
                x.PasNum,
                x.Type))
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
        List<ShortForm11DTO> uniqueAccountingUnitDtoList = [];

        foreach (var formDto in unionFormsDtoList
                     .Where(formDto => uniqueAccountingUnitDtoList
                         .All(dto => dto.FacNum + dto.Type + dto.PasNum != formDto.FacNum + formDto.Type + formDto.PasNum)))
        {
            uniqueAccountingUnitDtoList.Add(formDto);
        }

        var uniqueAccountingUnitSortedDtoList = uniqueAccountingUnitDtoList
            .OrderBy(dto => dto.FacNum + dto.Type + dto.PasNum)
            .ThenBy(dto => dto.OpDate)
            .ToList();

        return Task.FromResult(uniqueAccountingUnitSortedDtoList);
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
        var countUnit = 0;

        var inventoryDates = inventoryFormsDtoList
            .Select(x => x.OpDate)
            .Distinct()
            .Order()
            .ToList();
        inventoryDates.Add(DateOnly.FromDateTime(DateTime.Now));

        var isFirstInventory = true;
        List<ShortForm11DTO> unitInStockList = [];
        var previousInventory = DateOnly.MinValue;
        foreach (var inventoryDate in inventoryDates)
        {
            if (isFirstInventory)
            {
                unitInStockList
                    .AddRange(inventoryFormsDtoList
                        .Where(x => x.OpDate == inventoryDates.First())
                );

                previousInventory = inventoryDate;
                isFirstInventory = false;
                continue;
            }

            //ЗРИ, указанные в предыдущей инвентаризации, не снятые с учёта и не отражённые в следующей инвентаризации
            List<ShortForm11DTO> erroneouslyMissingUnitsList = [];

            //ЗРИ, указанные в предыдущей инвентаризации, снятые с учёта и отражённые в следующей инвентаризации
            List<ShortForm11DTO> mistakenlyPresentUnitsList = [];

            //ЗРИ, не указанные в предыдущей инвентаризации, без операций учёта и отражённые в следующей инвентаризации
            List<ShortForm11DTO> missingRegistrationUnitsList = [];

            //ЗРИ, отсутствующие на момент операции передачи
            List<ShortForm11DTO> incorrectlyTransferredUnitsList = [];

            //ЗРИ, уже присутствующие на момент получения
            List<ShortForm11DTO> incorrectlyReceivedUnitsList = [];

            foreach (var unit in uniqueAccountingUnitDtoList)
            {
                countUnit++;

                var inventoryWithCurrentUnit = inventoryFormsDtoList
                    .Where(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum)
                    .OrderBy(x => x.OpDate)
                    .DistinctBy(x => x.OpDate)
                    .ToArray();

                var inStock = unitInStockList.Contains(unit);

                var operationsWithCurrentUnit = plusMinusFormsDtoList
                    .Where(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum)
                    .OrderBy(x => x.OpDate)
                    .ToArray();

                foreach (var operation in operationsWithCurrentUnit)
                {
                    if (inStock && PlusOperation.Contains(operation.OpCode))
                    {
                        incorrectlyReceivedUnitsList.Add(operation);
                    }
                    else if (!inStock && MinusOperation.Contains(operation.OpCode))
                    {
                        incorrectlyTransferredUnitsList.Add(operation);
                    }
                    inStock = PlusOperation.Contains(operation.OpCode);
                }

                var lastOperationWithUnit = operationsWithCurrentUnit
                    .Union(inventoryWithCurrentUnit
                        .Where(x => x.OpDate == previousInventory || x.OpDate == inventoryDate))
                    .OrderByDescending(x => x.OpDate)
                    .First();

                unitInStockList.Remove(unit);
                if (inStock)
                {
                    unitInStockList.Add(lastOperationWithUnit);
                }
            }
        }
        return unitInStockList;
    }

    private class ShortForm11StringDateDTO(int id, ShortReportStringDateDTO repDto, string facNum, string opCode, string opDate, string pasNum, string type)
    {
        public readonly int Id = id;

        public readonly ShortReportStringDateDTO RepDto = repDto;

        public readonly string FacNum = facNum;

        public readonly string OpCode = opCode;

        public readonly string OpDate = opDate;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;
    }

    private class ShortForm12StringDateDTO(int id, int repId, string stPer, string enPer, string facNum, string opCode, string opDate, string pasNum, string type)
    {
        public readonly int Id = id;

        public readonly int RepId = repId;

        public readonly string StPer = stPer;

        public readonly string EnPer = enPer;

        public readonly string FacNum = facNum;

        public readonly string OpCode = opCode;

        public readonly string OpDate = opDate;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;
    }

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
}