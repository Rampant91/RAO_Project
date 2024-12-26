using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Threading;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Views.ProgressBar;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Client_App.ViewModels.ProgressBar;
using OfficeOpenXml;
using System.Reflection;
using static Client_App.Resources.StaticStringMethods;
using System.Collections.Concurrent;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Excel -> Проверка последней инвентаризации
/// </summary>
public class ExcelExportCheckLastInventoryDateAsyncCommand : ExcelExportSnkBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        ExportType = "Просроченная_инвентаризация_1.1";

        progressBarVM.SetProgressBar(5, "Создание временной БД", "Выгрузка списка организаций", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(8, "Проверка наличия отчётов");
        await CheckRepsAndRepPresence(db, progressBar, cts);

        progressBarVM.SetProgressBar(10, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(15, "Загрузка списка организаций");
        var repsDtoList = await GetReportsDtoList(db, cts);

        progressBarVM.SetProgressBar(20, "Проверка даты инвентаризации");
        var filteredRepsDtoList = await CheckRepsInventoryDate(tmpDbPath, repsDtoList, progressBarVM, cts);

        progressBarVM.SetProgressBar(40, "Проверка наличия СНК");
        var repsWithUnitsDtoList = await CheckSnk(db, filteredRepsDtoList, progressBarVM, cts);

        progressBarVM.SetProgressBar(90, "Заполнение строчек в .xlsx");
        await FillExcel(repsWithUnitsDtoList);

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

    #region CheckRepsAndRepPresence

    /// <summary>
    /// Проверяет наличие в БД хотя бы одной организации и хотя бы одного отчёта по форме 1.1.
    /// В случае отсутствия выводит соответствующее сообщение и закрывает команду.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckRepsAndRepPresence(DBModel db, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var countReps = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .CountAsync(cts.Token);

        var countRep = await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.1")
            .CountAsync(cts.Token);

        if (countReps == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Не удалось совершить выгрузку, поскольку в БД отсутствуют записи организаций.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        else if (countRep == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Не удалось совершить выгрузку, поскольку в БД отсутствуют отчёты по форме 1.1.",
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

    #region CheckRepsInventoryDate

    private static async Task<List<ShortReportsDto>> CheckRepsInventoryDate(string tmpDbPath, List<ShortReportsDto> repsDtoList,
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentRepNum = 0;

        ConcurrentBag<ShortReportsDto> repsWithExpiredInventory = [];
        ParallelOptions parallelOptions = new()
        {
            CancellationToken = cts.Token,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        await Parallel.ForEachAsync(repsDtoList, parallelOptions, async (repsDto, token) =>
        {
            await using var db = new DBModel(tmpDbPath);
            var inventoryReportIdList = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                .Where(reps => reps.Id == repsDto.Id)
                .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.1" && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
                    .Select(rep => rep.Id))
                .ToListAsync(cts.Token);

            List<string> inventoryFormsDtoList = [];
            foreach (var reportId in inventoryReportIdList)
            {
                var currentInventoryFormsStringDateDtoList = await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.Rows11)
                    .Where(rep => rep.Id == reportId)
                    .SelectMany(rep => rep.Rows11
                        .Where(form => form.OperationCode_DB == "10")
                        .Select(form11 => form11.OperationDate_DB))
                    .ToListAsync(cts.Token);
                inventoryFormsDtoList.AddRange(currentInventoryFormsStringDateDtoList);
            }

            if (inventoryFormsDtoList.Count == 0)
            {
                repsWithExpiredInventory.Add(repsDto);
                return;
            }

            var lastInventoryDate = inventoryFormsDtoList
                .Where(x => DateOnly.TryParse(x, out _))
                .Select(DateOnly.Parse)
                .Max();

            if (DateOnly.FromDateTime(DateTime.Now).DayNumber - lastInventoryDate.DayNumber > 365 + 14)
            {
                repsDto.LastInventoryDate = lastInventoryDate;
                repsWithExpiredInventory.Add(repsDto);
            }
            currentRepNum++;
            progressBarDoubleValue += (double)30 / repsDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {currentRepNum} из {repsDtoList.Count} дат инвентаризации",
                "Проверка последней инвентаризации");
        });
        return repsWithExpiredInventory.ToList();
    }

    #endregion

    #region CheckSnk

    private static async Task<List<ShortReportsDto>> CheckSnk(DBModel db, List<ShortReportsDto> dtoList,
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentRepsNum = 0;
        foreach (var dto in dtoList)
        {
            currentRepsNum++;

            var inventoryReportDtoList = await GetInventoryReportDtoList(db, dto.Id, currentDate, cts);

            var inventoryFormsDtoList = await GetInventoryFormsDtoList(db, inventoryReportDtoList, currentDate, cts);

            var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, dto.Id, currentDate, cts);

            var unionFormsDtoList = await GetUnionFormsDtoList(inventoryFormsDtoList, plusMinusFormsDtoList);

            var uniqueAccountingUnitDtoList = await GetUniqueAccountingUnitDtoList(unionFormsDtoList);

            dto.CountUnits = await GetUnitInStockCount(inventoryFormsDtoList, plusMinusFormsDtoList, uniqueAccountingUnitDtoList);

            progressBarDoubleValue += (double)50 / dtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {currentRepsNum} из {dtoList.Count} СНК организаций",
                "Проверка последней инвентаризации");
        }

        return dtoList
            .Where(x => x.CountUnits != 0)
            .OrderBy(x => x.RegNum)
            .ThenBy(x => x.Okpo)
            .ToList();
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполняет заголовки Excel пакета.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    private async Task FillExcelHeaders(ExcelPackage excelPackage)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add("Просроченная инвентаризация");

        #region Headers

        Worksheet.Cells[1, 1].Value = "№ п/п";
        Worksheet.Cells[1, 2].Value = "ОКПО";
        Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        Worksheet.Cells[1, 4].Value = "Рег.№";
        Worksheet.Cells[1, 5].Value = "Количество учётных единиц в наличии";
        Worksheet.Cells[1, 6].Value = "Дата последней инвентаризации";

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
    /// <param name="filteredRepsDtoList"></param>
    private Task FillExcel(List<ShortReportsDto> filteredRepsDtoList)
    {
        var currentRow = 2;
        var currentReps = 1;
        foreach (var repsDto in filteredRepsDtoList)
        {
            Worksheet.Cells[currentRow, 1].Value = currentReps;
            Worksheet.Cells[currentRow, 2].Value = ConvertToExcelString(repsDto.Okpo);
            Worksheet.Cells[currentRow, 3].Value = ConvertToExcelString(repsDto.ShortName);
            Worksheet.Cells[currentRow, 4].Value = ConvertToExcelString(repsDto.RegNum);
            Worksheet.Cells[currentRow, 5].Value = repsDto.CountUnits;
            Worksheet.Cells[currentRow, 6].Value = repsDto.LastInventoryDate == DateOnly.MinValue
                ? "-"
                : ConvertToExcelDate(repsDto.LastInventoryDate.ToShortDateString(), Worksheet, currentRow, 6);

            currentRow++;
            currentReps++;
        }

        return Task.CompletedTask;
    }

    #endregion

    #region GetReportsDtoList

    /// <summary>
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    private static async Task<List<ShortReportsDto>> GetReportsDtoList(DBModel db, CancellationTokenSource cts)
    {
        var repsDtoList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Report_Collection)
            .Where(reps => reps.Report_Collection
                .Any(rep => rep.FormNum_DB == "1.1"))
            .Select(x => new ShortReportsDto
            {
                Id = x.Id,
                Okpo = x.Master_DB.OkpoRep.Value,
                ShortName = x.Master_DB.ShortJurLicoRep.Value,
                RegNum = x.Master_DB.RegNoRep.Value
            })
            .ToListAsync(cts.Token);

        return repsDtoList;
    }

    #endregion

    #region GetUnitInStockCount

    private static Task<int> GetUnitInStockCount(List<ShortForm11DTO> inventoryFormsDtoList,
        List<ShortForm11DTO> plusMinusFormsDtoList, List<UniqueAccountingUnitDTO> uniqueAccountingUnitDtoList)
    {
        var firstInventoryDate = inventoryFormsDtoList.Count == 0
            ? DateOnly.MinValue
            : inventoryFormsDtoList
                .OrderBy(x => x.OpDate)
                .Select(x => x.OpDate)
                .First();

        var groupList = inventoryFormsDtoList
            .Where(x => x.OpDate == firstInventoryDate)
            .GroupBy(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type)
            .ToList();

        List<ShortForm11DTO> unitInStockList = [];
        foreach (var group in groupList)
        {
            var quantity = group.Sum(dto => dto.Quantity);
            var summedDto = group.First();
            summedDto.Quantity = quantity;
            unitInStockList.Add(summedDto);
        }

        foreach (var unit in uniqueAccountingUnitDtoList)
        {
            var inStock = unitInStockList
                .Any(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type 
                          == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type);

            var inventoryWithCurrentUnit = inventoryFormsDtoList
                .Where(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type 
                            == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type 
                            && x.OpDate >= firstInventoryDate)
                .DistinctBy(x => x.OpDate)
                .OrderBy(x => x.OpDate)
                .ToList();

            var operationsWithCurrentUnit = plusMinusFormsDtoList
                .Where(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type 
                            == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type 
                            && x.OpDate >= firstInventoryDate)
                .OrderBy(x => x.OpDate)
                .ToList();

            List<ShortForm11DTO> operationsWithCurrentUnitWithoutDuplicates = [];
            foreach (var group in operationsWithCurrentUnit.GroupBy(x => x.OpDate))
            {
                var countMinus = group.Count(x => MinusOperation.Contains(x.OpCode));
                var countPlus = group.Count(x => PlusOperation.Contains(x.OpCode));
                var countStock = (inStock ? 1 : 0) + countPlus - countMinus;

                switch (countStock)
                {
                    case >= 1:
                    {
                        operationsWithCurrentUnitWithoutDuplicates
                            .Add(group
                                .Last(x => PlusOperation.Contains(x.OpCode)));
                        break;
                    }
                    case < 1:
                    {
                        operationsWithCurrentUnitWithoutDuplicates
                            .Add(group
                                .Last(x => MinusOperation.Contains(x.OpCode)));
                        break;
                    }
                }
            }
            foreach (var operation in operationsWithCurrentUnitWithoutDuplicates)
            {
                inStock = inStock switch
                {
                    false when PlusOperation.Contains(operation.OpCode) => true,
                    true when MinusOperation.Contains(operation.OpCode) => false,
                    _ => inStock
                };
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
            if (inStock)
            {
                unitInStockList.Add(lastOperationWithUnit);
            }
        }
        return Task.FromResult(unitInStockList.Count);
    }

    #endregion

    #region ShortReportsDTO

    private class ShortReportsDto
    {
        public int Id { get; set; }

        public int CountUnits { get; set; }

        public DateOnly LastInventoryDate { get; set; }

        public string Okpo { get; set; }

        public string ShortName { get; set; }

        public string RegNum { get; set; }
    }

    #endregion
}