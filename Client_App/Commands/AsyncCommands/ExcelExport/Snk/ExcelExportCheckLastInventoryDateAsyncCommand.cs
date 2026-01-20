using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Messages;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using OfficeOpenXml;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Excel -> Проверка последней инвентаризации
/// </summary>
public partial class ExcelExportCheckLastInventoryDateAsyncCommand : ExcelExportSnkBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        ExportType = "Просроченная_инвентаризация";

        progressBarVM.SetProgressBar(5, "Запрос форм для проверки СНК", "Проверка инвентаризаций", ExportType);
        var (region, formNums, snkParams) = await GetRegionAndFormNums(progressBar, cts);

        progressBarVM.SetProgressBar(7, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(9, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Проверка наличия отчётов");
        await CheckRepsAndRepPresence(db, region, formNums, progressBar, cts);

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage);

        progressBarVM.SetProgressBar(15, "Загрузка списка организаций");
        var repsDtoList = await GetReportsDtoList(db, region, formNums, cts);

        progressBarVM.SetProgressBar(20, "Проверка даты инвентаризации");
        var filteredRepsDtoList = await CheckRepsInventoryDate(tmpDbPath, repsDtoList, formNums, progressBarVM, cts);

        progressBarVM.SetProgressBar(40, "Проверка наличия СНК");
        var repsWithUnitsDtoList = await CheckSnk(tmpDbPath, filteredRepsDtoList, progressBarVM, cts, snkParams);

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

    #region GetRegionAndFormNums

    /// <summary>
    /// Открывает окно, запрашивающее номера проверяемых форм и регион.
    /// </summary>
    /// <param name="progressBar"></param>
    /// <param name="cts"></param>
    /// <returns>Кортеж из региона и списка номеров форм.</returns>
    private static async Task<(string, List<string>, SnkParamsDto)> GetRegionAndFormNums(AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var vm = new GetRegionAndFormNumsVM();
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var getRegionAndFormNumsWindow = new GetRegionAndFormNums();
            await getRegionAndFormNumsWindow.ShowDialog(Desktop.MainWindow);
            vm = getRegionAndFormNumsWindow._vm;
        });

        if (!vm.Ok)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        else if (!vm.CheckForm11 && !vm.CheckForm13)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбран ни один номер формы.",
                    MinWidth = 400,
                    MinHeight = 115,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        else if (vm is { CheckPasNum: false, CheckType: false, CheckRadionuclids: false, CheckFacNum: false, CheckPackNumber: false })
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбран ни один из параметров, для определения учётной единицы.",
                    MinWidth = 400,
                    MinHeight = 115,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }


        List<string> formNums = [];
        if (vm.CheckForm11) formNums.Add("1.1");
        if (vm.CheckForm13) formNums.Add("1.3");

        var snkParamsDto = new SnkParamsDto(
            vm.CheckPasNum,
            vm.CheckType,
            vm.CheckRadionuclids,
            vm.CheckFacNum,
            vm.CheckPackNumber);

        var region = RegionRegex().IsMatch(vm.Region)
            ? vm.Region
            : string.Empty;

        return (region, [..formNums], snkParamsDto);
    }

    #endregion

    #region CheckRepsAndRepPresence

    /// <summary>
    /// Проверяет наличие в БД хотя бы одной организации и хотя бы одного отчёта по формам 1.1, 1.3.
    /// В случае отсутствия выводит соответствующее сообщение и закрывает команду.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="region">Регион.</param>
    /// <param name="formNums">Номера форм.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckRepsAndRepPresence(DBModel db, string region, List<string> formNums,
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        #region CheckRepsPresence

        var anyReps = (await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Where(x => x.DBObservable != null)
            .Select(x => x.Master_DB.Rows10[0].RegNo_DB)
            .ToListAsync(cts.Token))
            .Any(x => x.StartsWith(region));

        if (!anyReps)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Не удалось совершить выгрузку, поскольку в БД отсутствуют организации с указанным регионом.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        #endregion

        #region CheckRepPresence

        var anyRep = (await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Where(x =>
                x.Reports != null
                && x.Reports.DBObservable != null
                && formNums.Contains(x.FormNum_DB))
            .Select(x => x.Reports.Master_DB.Rows10[0].RegNo_DB)
            .ToListAsync(cts.Token))
            .Any(x => x.StartsWith(region));

        if (!anyRep)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Не удалось совершить выгрузку, поскольку в БД отсутствуют отчёты по формам {string.Join(", ", formNums)}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        #endregion
    }

    #endregion

    #region CheckRepsInventoryDate

    /// <summary>
    /// Для каждой организации из списка, загружаются даты операций инвентаризации, определяется последняя дата и сравнивается с текущей.
    /// Если разница превышает год и 2 недели, то добавляем данную организацию в список организаций с просроченной инвентаризацией.
    /// </summary>
    /// <param name="tmpDbPath">Полный путь к временному файлу БД.</param>
    /// <param name="repsDtoList">Список DTO организаций.</param>
    /// <param name="formNums">Номера форм.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO организаций с просроченной инвентаризацией.</returns>
    private static async Task<List<ShortReportsDto>> CheckRepsInventoryDate(string tmpDbPath, List<ShortReportsDto> repsDtoList,
        List<string> formNums, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
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

            var hasAnyForm11 = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(reps => reps.DBObservable)
                .Include(reps => reps.Report_Collection)
                .Where(reps => reps.DBObservable != null && reps.Id == repsDto.Id)
                .AnyAsync(reps => reps.Report_Collection
                    .Any(rep => rep.FormNum_DB == "1.1"), cts.Token);

            var hasAnyForm13 = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(reps => reps.DBObservable)
                .Include(reps => reps.Report_Collection)
                .Where(reps => reps.DBObservable != null && reps.Id == repsDto.Id)
                .AnyAsync(reps => reps.Report_Collection
                    .Any(rep => rep.FormNum_DB == "1.3"), cts.Token);

            #region 1.1

            if (formNums.Contains("1.1") && hasAnyForm11)
            {
                var repsDto11 = new ShortReportsDto
                {
                    FormNum = "1.1",
                    Id = repsDto.Id,
                    Okpo = repsDto.Okpo,
                    RegNum = repsDto.RegNum,
                    ShortName = repsDto.ShortName
                };

                var inventoryReport11IdList = await db.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(reps => reps.DBObservable)
                    .Include(reps => reps.Report_Collection).ThenInclude(rep => rep.Rows11)
                    .Where(reps => reps.DBObservable != null && reps.Id == repsDto11.Id)
                    .SelectMany(reps => reps.Report_Collection
                        .Where(rep => rep.FormNum_DB == "1.1" && rep.Rows11.Any(form11 => form11.OperationCode_DB == "10"))
                        .Select(rep => rep.Id))
                    .ToListAsync(cts.Token);

                List<string> inventoryForms11DtoList = [];
                foreach (var reportId in inventoryReport11IdList)
                {
                    var currentInventoryFormsStringDateList = await db.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Include(rep => rep.Reports).ThenInclude(reps => reps.DBObservable)
                        .Include(rep => rep.Rows11)
                        .Where(rep => rep.Reports != null && rep.Reports.DBObservable != null && rep.Id == reportId)
                        .SelectMany(rep => rep.Rows11
                            .Where(form11 => form11.OperationCode_DB == "10")
                            .Select(form11 => form11.OperationDate_DB))
                        .ToListAsync(cts.Token);

                    inventoryForms11DtoList.AddRange(currentInventoryFormsStringDateList);
                }

                if (inventoryForms11DtoList.Count == 0)
                {
                    repsWithExpiredInventory.Add(repsDto11);
                }
                else
                {
                    var lastInventoryDate11 = inventoryForms11DtoList
                        .Where(x => DateOnly.TryParse(x, out _))
                        .Select(DateOnly.Parse)
                        .Max();

                    if (DateOnly.FromDateTime(DateTime.Now).DayNumber - lastInventoryDate11.DayNumber > 365 + 14)
                    {
                        repsDto11.LastInventoryDate = lastInventoryDate11;
                        repsWithExpiredInventory.Add(repsDto11);
                    }
                }
            }

            #endregion

            #region 1.3

            if (formNums.Contains("1.3") && hasAnyForm13)
            {
                var repsDto13 = new ShortReportsDto
                {
                    FormNum = "1.3",
                    Id = repsDto.Id,
                    Okpo = repsDto.Okpo,
                    RegNum = repsDto.RegNum,
                    ShortName = repsDto.ShortName
                };

                var inventoryReport13IdList = await db.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(reps => reps.DBObservable)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
                    .Where(reps => reps.DBObservable != null && reps.Id == repsDto13.Id)
                    .SelectMany(reps => reps.Report_Collection
                        .Where(rep => rep.FormNum_DB == "1.3" && rep.Rows13.Any(form => form.OperationCode_DB == "10"))
                        .Select(rep => rep.Id))
                    .ToListAsync(cts.Token);

                List<string> inventoryForms13DtoList = [];
                foreach (var reportId in inventoryReport13IdList)
                {
                    var currentInventoryFormsStringDateList = await db.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                        .Include(x => x.Rows13)
                        .Where(rep => rep.Reports != null && rep.Reports.DBObservable != null && rep.Id == reportId)
                        .SelectMany(rep => rep.Rows13
                            .Where(form13 => form13.OperationCode_DB == "10")
                            .Select(form13 => form13.OperationDate_DB))
                        .ToListAsync(cts.Token);

                    inventoryForms13DtoList.AddRange(currentInventoryFormsStringDateList);
                }

                if (inventoryForms13DtoList.Count == 0)
                {
                    repsWithExpiredInventory.Add(repsDto13);
                }
                else
                {
                    var lastInventoryDate13 = inventoryForms13DtoList
                        .Where(x => DateOnly.TryParse(x, out _))
                        .Select(DateOnly.Parse)
                        .Max();

                    if (DateOnly.FromDateTime(DateTime.Now).DayNumber - lastInventoryDate13.DayNumber > 365 + 14)
                    {
                        repsDto13.LastInventoryDate = lastInventoryDate13;
                        repsWithExpiredInventory.Add(repsDto13);
                    }
                }
            }

            #endregion

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
    /// <param name="repsDtoList">Список DTO организаций.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO организаций, у которых есть учётные единицы в наличии.</returns>
    private static async Task<List<ShortReportsDto>> CheckSnk(string tmpDbPath, List<ShortReportsDto> repsDtoList,
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentRepsNum = 0;

        #region TestParallelRealization

        //var dtoBag = new ConcurrentBag<ShortReportsDto>(repsDtoList);
        //ParallelOptions parallelOptions = new()
        //{
        //    CancellationToken = cts.Token,
        //    MaxDegreeOfParallelism = Environment.ProcessorCount
        //};
        //progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
        //    $"Проверено 0 из {repsDtoList.Count} СНК организаций",
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

        //    progressBarDoubleValue += (double)50 / repsDtoList.Count;
        //    progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
        //        $"Проверено {currentRepsNum} из {repsDtoList.Count} СНК организаций",
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

        //    progressBarDoubleValue += (double)50 / repsDtoList.Count;
        //    progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
        //        $"Проверено {currentRepsNum} из {repsDtoList.Count} СНК организаций",
        //        "Проверка последней инвентаризации");
        //});

        #endregion

        await using var db = new DBModel(tmpDbPath);
        foreach (var repsDto in repsDtoList)
        {
            currentRepsNum++;

            var inventoryReportDtoList = await GetInventoryReportDtoList(db, repsDto.Id, repsDto.FormNum, currentDate, cts);

            var (firstSnkDate, inventoryFormsDtoList, _) = 
                await GetInventoryFormsDtoList(db, inventoryReportDtoList, repsDto.FormNum, currentDate, cts, snkParams);

            var reportIds = await GetReportIds(db, repsDto.Id, repsDto.FormNum, cts);

            var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, reportIds, repsDto.FormNum, firstSnkDate, currentDate, cts, snkParams);

            var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, repsDto.Id, repsDto.FormNum, firstSnkDate, currentDate, cts, snkParams);

            var uniqueUnitWithAllOperationDictionary = 
                await GetDictionary_UniqueUnitsWithOperations(repsDto.FormNum, inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);

            var unitInStockDtoList = await GetUnitInStockDtoList(uniqueUnitWithAllOperationDictionary, repsDto.FormNum, firstSnkDate, progressBarVM);
            repsDto.CountUnits = unitInStockDtoList.Sum(x => x.Quantity);

            progressBarDoubleValue += (double)50 / repsDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {currentRepsNum} из {repsDtoList.Count} СНК организаций",
                "Проверка последней инвентаризации");
        }
        return repsDtoList
            .Where(x => x.CountUnits != 0)
            .OrderBy(x => x.RegNum)
            .ThenBy(x => x.Okpo)
            .ThenBy(x => x.FormNum)
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
        Worksheet.Cells[1, 5].Value = "№ формы";
        Worksheet.Cells[1, 6].Value = "Количество учётных единиц в наличии";
        Worksheet.Cells[1, 7].Value = "Дата последней инвентаризации";

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
            Worksheet.Cells[currentRow, 5].Value = ConvertToExcelString(repsDto.FormNum);
            Worksheet.Cells[currentRow, 6].Value = repsDto.CountUnits;
            Worksheet.Cells[currentRow, 7].Value = repsDto.LastInventoryDate == DateOnly.MinValue
                ? "-"
                : ConvertToExcelDate(repsDto.LastInventoryDate.ToShortDateString(), Worksheet, currentRow, 7);

            currentRow++;
            currentReps++;
        }

        return Task.CompletedTask;
    }

    #endregion

    #region GetReportsDtoList

    /// <summary>
    /// Получение списка DTO организаций, имеющих отчёты по формам 1.1, 1.3.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="formNums">Номера форм.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="region">Регион.</param>
    /// <returns>Список DTO организаций, имеющих отчёты по формам 1.1, 1.3.</returns>
    private static async Task<List<ShortReportsDto>> GetReportsDtoList(DBModel db, string region, List<string> formNums, 
        CancellationTokenSource cts)
    {
        return (await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection)
                .Where(x =>
                    x.DBObservable != null
                    && x.Master_DB != null
                    && x.Report_Collection
                        .Any(y => formNums.Contains(y.FormNum_DB)))
                .Select(x => new ShortReportsDto
                {
                    Id = x.Id,
                    Okpo = x.Master_DB.OkpoRep.Value,
                    ShortName = x.Master_DB.ShortJurLicoRep.Value,
                    RegNum = x.Master_DB.RegNoRep.Value
                })
                .ToListAsync(cts.Token))
            .Where(x => x.RegNum.StartsWith(region))
            .ToList();
    }

    #endregion

    #region ShortReportsDTO

    private class ShortReportsDto
    {
        public int Id { get; set; }

        public int CountUnits { get; set; }

        public string FormNum { get; set; } = "";

        public DateOnly LastInventoryDate { get; set; }

        public string Okpo { get; set; }

        public string ShortName { get; set; }

        public string RegNum { get; set; }
    }

    #endregion

    #region Regex

    [GeneratedRegex(@"^\d{2}$")]
    private static partial Regex RegionRegex();

    #endregion
}