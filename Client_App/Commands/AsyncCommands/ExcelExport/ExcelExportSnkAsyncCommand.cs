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
using Client_App.ViewModels.ProgressBar;

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
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod, out var endDate) ? endDate : DateOnly.MaxValue)
            .First();
        DateOnly.TryParse(firstInventoryReportDto.StartPeriod, out var firstInventoryDate);

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
        return await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(rep => rep.Id == firstInventoryReportId)
            .SelectMany(rep => rep.Rows11
                .Where(form => form.OperationCode_DB == "10")
                .Select(form => new ShortForm11DTO(form.Id, new ShortReportDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB), 
                    form.FactoryNumber_DB, form.OperationCode_DB, form.OperationDate_DB, form.PassportNumber_DB, form.Type_DB)))
            .ToListAsync(cts.Token);
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
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1" && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
                .Select(rep => new ShortReportDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB)))
            .ToListAsync(cts.Token);
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
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.Id == repsId)
            .SelectMany(reps => reps.Report_Collection)
            .SelectMany(rep => rep.Rows11
                .Where(form => PlusOperation.Contains(form.OperationCode_DB) || MinusOperation.Contains(form.OperationCode_DB))
                .Select(form => new ShortForm11DTO(form.Id, new ShortReportDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB), form.FactoryNumber_DB, form.OperationCode_DB,
                    form.OperationDate_DB, form.PassportNumber_DB, form.Type_DB)))
            .ToListAsync(cts.Token);
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
            .ThenBy(dto => DateOnly.TryParse(dto.OpDate, out var opDate) ? opDate : DateOnly.MaxValue)
            .ToList();

        return Task.FromResult(uniqueAccountingUnitSortedDtoList);
    }

    #endregion

    #region GetUnionFormsDtoList

    private static Task<List<ShortForm11DTO>> GetUnionFormsDtoList(List<ShortForm11DTO> inventoryFormsDtoList, List<ShortForm11DTO> plusMinusFormsDtoList,
        DateOnly firstInventoryDate)
    {
        var unionFormsDtoList = inventoryFormsDtoList
            .Union(plusMinusFormsDtoList)
            .Where(x =>
            {
                var date = DateOnly.TryParse(x.OpDate, out var opDate) ? opDate : DateOnly.MaxValue;
                return date >= firstInventoryDate;
            })
            .ToList();

        return Task.FromResult(unionFormsDtoList);
    }

    #endregion


    private static async Task<List<ShortForm11DTO>> GetForms11DtoList(DBModel db, List<ShortForm11DTO> inventoryFormsDtoList, 
        List<ShortForm11DTO> plusMinusFormsDtoList, List<ShortForm11DTO> uniqueAccountingUnitDtoList, CancellationTokenSource cts)
    {
        List<ShortForm11DTO> unitDoubleMinusList = [];
        List<ShortForm11DTO> unitDoublePlusList = [];
        List<ShortForm11DTO> unitInStockList = [];
        var countUnit = 0;
        foreach (var unit in uniqueAccountingUnitDtoList)
        {
            countUnit++;
            var inStock = false;

            var operationsWithCurrentUnitGroupedByReport = plusMinusFormsDtoList
                .Where(x => DateOnly.TryParse(x.OpDate, out _)
                            && x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum)
                .OrderBy(x => DateOnly.Parse(x.OpDate))
                .GroupBy(x => x.RepDto.Id)
                .ToArray();

            var inventoryWithCurrentUnitGroupedByReport = inventoryFormsDtoList
                .Where(x => DateOnly.TryParse(x.OpDate, out _)
                            && x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum)
                .OrderBy(x => DateOnly.Parse(x.OpDate))
                .DistinctBy(x => x.OpDate)
                .GroupBy(x => x.RepDto.Id)
                .ToArray();

            foreach(var inventoryGroup in inventoryWithCurrentUnitGroupedByReport)
            {
                var repId = inventoryGroup.Key;
                var repDto = await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(rep => rep.Id == repId)
                    .Select(rep => new ShortReportDTO(repId, rep.StartPeriod_DB, rep.EndPeriod_DB))
                    .FirstAsync(cts.Token);


            }

            foreach (var operationInOneReport in operationsWithCurrentUnitGroupedByReport)
            {
                var plusMinusCount = 0;
                foreach (var operation in operationInOneReport)
                {
                    if (MinusOperation.Contains(operation.OpCode)) plusMinusCount--;
                    else if (PlusOperation.Contains(operation.OpCode)) plusMinusCount++;
                }
                switch (plusMinusCount)
                {
                    case > 1:
                    {
                        if (inStock) unitDoublePlusList.AddRange(operationInOneReport);
                        inStock = true;
                        break;
                    }
                    case 1:
                    {
                        inStock = true;
                        break;
                    }
                    case < 0:
                    {
                        if (!inStock) unitDoubleMinusList.AddRange(operationInOneReport);
                        inStock = false;
                        break;
                    }
                }
            }
            if (inStock) unitInStockList.Add(unit);
        }
        return unitInStockList;
    }

    private class ShortForm11DTO(int id, ShortReportDTO repDto, string facNum, string opCode, string opDate, string pasNum, string type)
    {
        public readonly int Id = id;

        public readonly ShortReportDTO RepDto = repDto;

        public readonly string FacNum = facNum;

        public readonly string OpCode = opCode;

        public readonly string OpDate = opDate;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;
    }

    private class ShortReportDTO(int id, string startPeriod, string endPeriod)
    {
        public readonly int Id = id;

        public readonly string StartPeriod = startPeriod;

        public readonly string EndPeriod = endPeriod;
    }
}