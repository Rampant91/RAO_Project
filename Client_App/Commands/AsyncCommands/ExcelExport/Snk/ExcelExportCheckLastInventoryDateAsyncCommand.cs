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
        var formNum = "1.1";
        ExportType = "Просроченная_инвентаризация_1.1";

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(7, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Проверка наличия отчётов", "Выгрузка списка организаций", ExportType);
        await CheckRepsAndRepPresence(db, progressBar, cts);

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(15, "Загрузка списка организаций");
        var repsDtoList = await GetReportsDtoList(db, cts);

        progressBarVM.SetProgressBar(20, "Проверка даты инвентаризации");
        var filteredRepsDtoList = await CheckRepsInventoryDate(tmpDbPath, repsDtoList, progressBarVM, cts);

        progressBarVM.SetProgressBar(40, "Проверка наличия СНК");
        var repsWithUnitsDtoList = await CheckSnk(tmpDbPath, filteredRepsDtoList, formNum, progressBarVM, cts);

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
        var anyReps = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Where(x => x.DBObservable != null)
            .AnyAsync(cts.Token);

        var anyRep = await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.FormNum_DB == "1.1")
            .AnyAsync(cts.Token);

        if (!anyReps)
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
        else if (!anyRep)
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

    /// <summary>
    /// Для каждой организации из списка, загружаются даты операций инвентаризации, определяется последняя дата и сравнивается с текущей.
    /// Если разница превышает год и 2 недели, то добавляем данную организацию в список организаций с просроченной инвентаризацией.
    /// </summary>
    /// <param name="tmpDbPath">Полный путь к временному файлу БД.</param>
    /// <param name="repsDtoList">Список DTO организаций.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO организаций с просроченной инвентаризацией.</returns>
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
                .Include(reps => reps.DBObservable)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                .Where(reps => reps.DBObservable != null && reps.Id == repsDto.Id)
                .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.1" && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
                    .Select(rep => rep.Id))
                .ToListAsync(cts.Token);

            List<string> inventoryFormsDtoList = [];
            foreach (var reportId in inventoryReportIdList)
            {
                var currentInventoryFormsStringDateList = await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows11)
                    .Where(rep => rep.Reports != null && rep.Reports.DBObservable != null && rep.Id == reportId)
                    .SelectMany(rep => rep.Rows11
                        .Where(form => form.OperationCode_DB == "10")
                        .Select(form11 => form11.OperationDate_DB))
                    .ToListAsync(cts.Token);
                inventoryFormsDtoList.AddRange(currentInventoryFormsStringDateList);
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
            progressBarDoubleValue += (double)20 / repsDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {currentRepNum} из {repsDtoList.Count} дат инвентаризации",
                "Проверка последней инвентаризации");
        });
        return [.. repsWithExpiredInventory];
    }

    #endregion

    #region CheckSnk

    /// <summary>
    /// Проверяет наличие учётных единиц у организаций из списка, возвращает список DTO организаций, у которых есть учётные единицы в наличии.
    /// </summary>
    /// <param name="tmpDbPath">Путь к временному файлу БД.</param>
    /// <param name="dtoList">Список DTO организаций.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO организаций, у которых есть учётные единицы в наличии.</returns>
    private static async Task<List<ShortReportsDto>> CheckSnk(string tmpDbPath, List<ShortReportsDto> dtoList, string formNum,
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentRepsNum = 0;

        #region TestParallelRealization

        //var dtoBag = new ConcurrentBag<ShortReportsDto>(dtoList);
        //ParallelOptions parallelOptions = new()
        //{
        //    CancellationToken = cts.Token,
        //    MaxDegreeOfParallelism = Environment.ProcessorCount
        //};
        //progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
        //    $"Проверено 0 из {dtoList.Count} СНК организаций",
        //    "Проверка последней инвентаризации");
        //Parallel.ForEach(dtoBag, parallelOptions, async (dto, token) =>
        //{
        //    await using var db = new DBModel(tmpDbPath);
        //    currentRepsNum++;

        //    var inventoryReportDtoList = await GetInventoryReportDtoList(db, dto.Id, currentDate, cts);

        //    var (firstSnkDate, inventoryFormsDtoList) = await GetInventoryFormsDtoList(db, inventoryReportDtoList, currentDate, cts);

        //    var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, dto.Id, firstSnkDate, currentDate, cts);

        //    var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, dto.Id, firstSnkDate, currentDate, cts);

        //    var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);

        //    dto.CountUnits = (await GetUnitInStockDtoList(uniqueUnitWithAllOperationDictionary, progressBarVM)).Count;

        //    progressBarDoubleValue += (double)50 / dtoList.Count;
        //    progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
        //        $"Проверено {currentRepsNum} из {dtoList.Count} СНК организаций",
        //        "Проверка последней инвентаризации");
        //});

        //await Parallel.ForEachAsync(dtoBag, parallelOptions, async (dto, token) =>
        //{
        //    await using var db = new DBModel(tmpDbPath);
        //    currentRepsNum++;

        //    var inventoryReportDtoList = await GetInventoryReportDtoList(db, dto.Id, currentDate, cts);

        //    var (firstSnkDate, inventoryFormsDtoList) = await GetInventoryFormsDtoList(db, inventoryReportDtoList, currentDate, cts);

        //    var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, dto.Id, firstSnkDate, currentDate, cts);

        //    var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, dto.Id, firstSnkDate, currentDate, cts);

        //    var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);

        //    dto.CountUnits = (await GetUnitInStockDtoList(uniqueUnitWithAllOperationDictionary, progressBarVM)).Count;

        //    progressBarDoubleValue += (double)50 / dtoList.Count;
        //    progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
        //        $"Проверено {currentRepsNum} из {dtoList.Count} СНК организаций",
        //        "Проверка последней инвентаризации");
        //});

        #endregion

        await using var db = new DBModel(tmpDbPath);
        foreach (var dto in dtoList)
        {
            currentRepsNum++;

            var inventoryReportDtoList = await GetInventoryReportDtoList(db, dto.Id, formNum, currentDate, cts);

            var (firstSnkDate, inventoryFormsDtoList, _) = await GetInventoryFormsDtoList(db, inventoryReportDtoList, formNum, currentDate, cts);

            var reportIds = await GetReportIds(db, dto.Id, formNum, cts);

            var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, reportIds, formNum, firstSnkDate, currentDate, cts);

            var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, dto.Id, formNum, firstSnkDate, currentDate, cts);

            var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(formNum, inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);

            dto.CountUnits = (await GetUnitInStockDtoList(uniqueUnitWithAllOperationDictionary, formNum, firstSnkDate, progressBarVM)).Count;

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
    /// Заполняет строчки Excel пакета.
    /// </summary>
    /// <param name="filteredRepsDtoList">Список DTO организаций, с просроченной инвентаризацией.</param>
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
    /// Получение списка DTO организаций, имеющих отчёты по форме 1.1.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>,
    /// <returns>Список DTO организаций, имеющих отчёты по форме 1.1.</returns>
    private static async Task<List<ShortReportsDto>> GetReportsDtoList(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Report_Collection)
            .Where(reps => reps.DBObservable != null && reps.Report_Collection
                .Any(rep => rep.FormNum_DB == "1.1"))
            .Select(x => new ShortReportsDto
            {
                Id = x.Id,
                Okpo = x.Master_DB.OkpoRep.Value,
                ShortName = x.Master_DB.ShortJurLicoRep.Value,
                RegNum = x.Master_DB.RegNoRep.Value
            })
            .ToListAsync(cts.Token);
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