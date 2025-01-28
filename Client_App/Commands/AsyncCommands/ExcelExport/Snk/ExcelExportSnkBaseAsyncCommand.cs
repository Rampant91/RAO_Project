using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

public abstract class ExcelExportSnkBaseAsyncCommand : ExcelBaseAsyncCommand
{
    #region Properties

    /// <summary>
    /// Коды операций снятия с учёта.
    /// </summary>
    private protected static readonly string[] MinusOperation =
    [
        "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "47", "65", "67", "71", "72", "81", "82", "83", "84", "98"
    ];


    /// <summary>
    /// Коды операций постановки на учёт.
    /// </summary>
    private protected static readonly string[] PlusOperation =
    [
        "11", "12", "17", "31", "32", "35", "37", "38", "39", "58", "73", "74", "75", "85", "86", "87", "88", "97"
    ];

    #endregion

    #region DTO

    private protected class SnkForm11DTO(string facNum, string pasNum, int quantity, string radionuclids, string type, string activity,
        string creatorOKPO, string creationDate, short? category, float? signedServicePeriod, string packNumber)
    {
        public readonly string PasNum = pasNum;

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

    private protected class ShortForm11DTO(int id, ShortReportDTO repDto, string facNum, string opCode, DateOnly opDate, string packNumber, 
        string pasNum, int quantity, string radionuclids, string type)
    {
        public readonly int Id = id;

        public readonly ShortReportDTO RepDto = repDto;

        public readonly string FacNum = facNum;

        public readonly string OpCode = opCode;

        public readonly DateOnly OpDate = opDate;

        public readonly string PackNumber = packNumber;

        public readonly string PasNum = pasNum;

        public int Quantity = quantity;

        public readonly string Radionuclids = radionuclids;

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

        public string PackNumber { get; set; }

        public string PasNum { get; set; }

        public int? Quantity { get; set; }

        public string Radionuclids { get; set; }

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

        public string PackNumber { get; set; }

        public string PasNum { get; set; }

        public int? Quantity { get; set; }

        public string Radionuclids { get; set; }

        public string Type { get; set; }
    }

    private protected class SnkParamsDto(bool pasNum, bool type, bool radionuclids, bool facNum, bool packNum)
    {
        public readonly bool CheckPasNum = pasNum;

        public readonly bool CheckType = type;

        public readonly bool CheckRadionuclids = radionuclids;

        public readonly bool CheckFacNum = facNum;

        public readonly bool CheckPackNumber = packNum;
    }

    private class ShortReportStringDateDTO(int id, string startPeriod, string endPeriod)
    {
        public readonly int Id = id;

        public readonly string StartPeriod = startPeriod;

        public readonly string EndPeriod = endPeriod;
    }

    private protected class ShortReportDTO(int id, DateOnly startPeriod, DateOnly endPeriod)
    {
        public readonly int Id = id;

        public readonly DateOnly StartPeriod = startPeriod;

        public readonly DateOnly EndPeriod = endPeriod;
    }

    private protected class UniqueAccountingUnitDTO
    {
        public string FacNum { get; set; }

        public string PackNumber { get; set; }

        public string PasNum { get; set; }

        public string Radionuclids { get; set; }

        public string Type { get; set; }
    }

    #endregion

    #region Methods

    #region AskSnkEndDate

    /// <summary>
    /// Запрос ввода даты формирования СНК.
    /// </summary>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из даты, на которую необходимо сформировать СНК и dto bool флагов, по каким параметрам определять учётную единицу.</returns>
    private protected static async Task<(DateOnly, SnkParamsDto)> AskSnkEndDate(AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var date = DateOnly.MinValue;

        var vm = new GetSnkParamsVM();
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var getSnkParamsWindow = new GetSnkParams();
            await getSnkParamsWindow.ShowDialog(Desktop.MainWindow);
            vm = getSnkParamsWindow._vm;
        });

        if (!vm.Ok)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        else if (!DateOnly.TryParse(vm.Date, out date))
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Не удалось распознать введённую дату, " +
                                     $"{Environment.NewLine}выгрузка будет выполнена на текущую системную дату.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            date = DateOnly.Parse(DateTime.Now.ToShortDateString());
        }
        else if (date < DateOnly.Parse("01.01.2022"))
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Выгрузка не выполнена, поскольку введена дата ранее вступления в силу приказа.",
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
        var snkParamsDto = new SnkParamsDto(
            vm.CheckPasNum,
            vm.CheckType,
            vm.CheckRadionuclids,
            vm.CheckFacNum,
            vm.CheckPackNumber);
        return (date, snkParamsDto);
    }

    #endregion

    #region AutoReplaceSimilarChars

    private protected static string AutoReplaceSimilarChars(string? str)
    {
        return new Regex(@"[\\/:*?""<>|.,_\-;:\s+]")
            .Replace(str ?? string.Empty, "")
            .ToLower()
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
            .Replace('к', 'k')
            .Replace('м', 'm')
            .Replace('о', 'o')
            .Replace('0', 'o')
            .Replace('р', 'p')
            .Replace('с', 'c')
            .Replace('т', 't')
            .Replace('у', 'y')
            .Replace('х', 'x');
    }

    #endregion

    #region GetInventoryFormsDtoList

    /// <summary>
    /// Получение списка DTO операций инвентаризации.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="inventoryReportDtoList">Список DTO отчётов, содержащих операцию инвентаризации.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO операций инвентаризации, отсортированный по датам.</returns>
    private protected static async Task<(DateOnly, List<ShortForm11DTO>)> GetInventoryFormsDtoList(DBModel db, List<ShortReportDTO> inventoryReportDtoList,
        DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
        List<ShortForm11DTO> inventoryFormsDtoList = [];
        foreach (var reportDto in inventoryReportDtoList)
        {
            var currentInventoryFormsStringDateDtoList = await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows11)
                .Where(rep => rep.Reports != null && rep.Reports.DBObservable != null && rep.Id == reportDto.Id)
                .SelectMany(rep => rep.Rows11
                    .Where(form => form.OperationCode_DB == "10")
                    .Select(form11 => new ShortForm11StringDateDTO
                    {
                        Id = form11.Id,
                        RepId = reportDto.Id,
                        StDate = reportDto.StartPeriod,
                        EndDate = reportDto.EndPeriod,
                        FacNum = snkParams == null || snkParams.CheckFacNum ? form11.FactoryNumber_DB : string.Empty,
                        OpCode = form11.OperationCode_DB,
                        OpDate = form11.OperationDate_DB,
                        PackNumber = snkParams == null || snkParams.CheckPackNumber ? form11.PackNumber_DB : string.Empty,
                        PasNum = snkParams == null || snkParams.CheckPasNum ? form11.PassportNumber_DB : string.Empty,
                        Quantity = form11.Quantity_DB,
                        Radionuclids = snkParams == null || snkParams.CheckRadionuclids ? form11.Radionuclids_DB : string.Empty,
                        Type = snkParams == null || snkParams.CheckType ? form11.Type_DB : string.Empty
                    }))
                .ToListAsync(cts.Token);

            var currentInventoryFormsDtoList = currentInventoryFormsStringDateDtoList
                .Where(x => DateOnly.TryParse(x.OpDate, out var opDateOnly)
                            && opDateOnly >= DateOnly.Parse("01.01.2022")
                            && opDateOnly <= endSnkDate)
                .Select(x => new ShortForm11DTO(
                    x.Id,
                    reportDto,
                    AutoReplaceSimilarChars(x.FacNum),
                    x.OpCode,
                    DateOnly.Parse(x.OpDate),
                    AutoReplaceSimilarChars(x.PackNumber),
                    AutoReplaceSimilarChars(x.PasNum),
                    x.Quantity ?? 0,
                    AutoReplaceSimilarChars(x.Radionuclids),
                    AutoReplaceSimilarChars(x.Type)))
                .ToList();

            inventoryFormsDtoList.AddRange(currentInventoryFormsDtoList);
        }
        inventoryFormsDtoList = await GetSummedInventoryDtoList(inventoryFormsDtoList);

        var firstInventoryDate = inventoryFormsDtoList.Count == 0
            ? new DateOnly(2022, 1, 1)
            : inventoryFormsDtoList
                .OrderBy(x => x.OpDate)
                .Select(x => x.OpDate)
                .First();

        var orderedInventoryFormsDtoList = inventoryFormsDtoList
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();

        return (firstInventoryDate, orderedInventoryFormsDtoList);
    }

    #region GetSummedInventoryDtoList

    /// <summary>
    /// Суммирует операции инвентаризации для первой даты по количеству и возвращает список DTO.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO операций инвентаризации.</param>
    /// <returns>Список DTO операций инвентаризации, просуммированный по количеству для первой даты.</returns>
    private static Task<List<ShortForm11DTO>> GetSummedInventoryDtoList(List<ShortForm11DTO> inventoryFormsDtoList)
    {
        List<ShortForm11DTO> newInventoryFormsDtoList = [];

        var firstDate = inventoryFormsDtoList.Count == 0
            ? DateOnly.MinValue
            : inventoryFormsDtoList
                .OrderBy(x => x.OpDate)
                .Select(x => x.OpDate)
                .First();

        foreach (var form in inventoryFormsDtoList)
        {
            var matchingForm = newInventoryFormsDtoList.FirstOrDefault(x =>
                x.OpDate == firstDate
                && SerialNumbersIsEmpty(form.PasNum, form.FacNum)
                && x.Radionuclids == form.Radionuclids
                && x.Type == form.Type
                && x.PackNumber == form.PackNumber);

            if (matchingForm != null)
            {
                matchingForm.Quantity += form.Quantity;
            }
            else
            {
                newInventoryFormsDtoList.Add(form);
            }
        }
        return Task.FromResult(newInventoryFormsDtoList);
    }

    #endregion

    #endregion

    #region GetInventoryReportDtoList

    /// <summary>
    /// Получение списка DTO отчётов по форме 1.1, содержащих хотя бы одну операцию с кодом 10.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id выбранной организации.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO отчётов по форме 1.1, отсортированный по датам.</returns>
    private protected static async Task<List<ShortReportDTO>> GetInventoryReportDtoList(DBModel db, int repsId, DateOnly endSnkDate,
        CancellationTokenSource cts)
    {
        var inventoryReportDtoList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
            .Where(reps => reps.DBObservable != null && reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1" && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
                .Select(rep => new ShortReportStringDateDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB)))
            .ToListAsync(cts.Token);

        return inventoryReportDtoList
            .Where(x => DateOnly.TryParse(x.StartPeriod, out var stPer)
                        && DateOnly.TryParse(x.EndPeriod, out var endDate)
                        && endDate >= DateOnly.Parse("01.01.2022")
                        && stPer <= endSnkDate)
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
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO форм с операциями приёма передачи, отсортированный по датам.</returns>
    private protected static async Task<List<ShortForm11DTO>> GetPlusMinusFormsDtoList(DBModel db, int repsId, DateOnly firstSnkDate, DateOnly endSnkDate,
        CancellationTokenSource cts, SnkParamsDto? snkParams = null)
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

        var plusMinusOperationDtoList = await db.form_11
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Report)
            .Where(x => x.Report != null
                        && reportIds.Contains(x.Report.Id)
                        && (PlusOperation.Contains(x.OperationCode_DB)
                            || MinusOperation.Contains(x.OperationCode_DB)))
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

        return plusMinusOperationDtoList
            .Where(x => DateOnly.TryParse(x.OpDate, out var opDateOnly)
                                             && DateOnly.TryParse(x.StDate, out _)
                                             && DateOnly.TryParse(x.EndDate, out _)
                                             && opDateOnly >= firstSnkDate
                                             && opDateOnly <= endSnkDate)
            .Select(x => new ShortForm11DTO(
                x.Id,
                new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                AutoReplaceSimilarChars(x.FacNum),
                x.OpCode,
                DateOnly.Parse(x.OpDate),
                AutoReplaceSimilarChars(x.PackNumber),
                AutoReplaceSimilarChars(x.PasNum),
                x.Quantity ?? 0,
                AutoReplaceSimilarChars(x.Radionuclids),
                AutoReplaceSimilarChars(x.Type)))
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();
    }

    #endregion

    #region GetRechargeFormsDtoList

    /// <summary>
    /// Получение списка DTO форм с операциями перезарядки.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id организации.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO форм с операциями перезарядки, отсортированный по датам.</returns>
    private protected static async Task<List<ShortForm11DTO>> GetRechargeFormsDtoList(DBModel db, int repsId, DateOnly firstSnkDate, DateOnly endSnkDate,
        CancellationTokenSource cts, SnkParamsDto? snkParams = null)
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

        var rechargeOperationDtoList = await db.form_11
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Report)
            .Where(x => x.Report != null
                        && reportIds.Contains(x.Report.Id)
                        && (x.OperationCode_DB == "53" || x.OperationCode_DB == "54"))
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

        return rechargeOperationDtoList
            .Where(x => DateOnly.TryParse(x.OpDate, out var opDateOnly)
                                             && DateOnly.TryParse(x.StDate, out _)
                                             && DateOnly.TryParse(x.EndDate, out _)
                                             && opDateOnly >= firstSnkDate
                                             && opDateOnly <= endSnkDate)
            .Select(x => new ShortForm11DTO(
                x.Id,
                new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                AutoReplaceSimilarChars(x.FacNum),
                x.OpCode,
                DateOnly.Parse(x.OpDate),
                AutoReplaceSimilarChars(x.PackNumber),
                AutoReplaceSimilarChars(x.PasNum),
                x.Quantity ?? 0,
                AutoReplaceSimilarChars(x.Radionuclids),
                AutoReplaceSimilarChars(x.Type)))
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();
    }

    #endregion

    #region GetUniqueAccountingUnitDtoList

    /// <summary>
    /// Получение отсортированного списка DTO уникальных учётных единиц с операциями инвентаризации, приёма или передачи.
    /// </summary>
    /// <param name="unionFormsDtoList">Список DTO всех операций инвентаризации, приёма или передачи.</param>
    /// <returns>Список DTO уникальных учётных единиц с операциями инвентаризации, приёма или передачи.</returns>
    private protected static Task<List<UniqueAccountingUnitDTO>> GetUniqueAccountingUnitDtoList(List<ShortForm11DTO> unionFormsDtoList)
    {
        var uniqueAccountingUnitDtoList = unionFormsDtoList
            .Select(x => new UniqueAccountingUnitDTO
            {
                FacNum = x.FacNum,
                Radionuclids = x.Radionuclids,
                PackNumber = x.PackNumber,
                PasNum = x.PasNum,
                Type = x.Type
            })
            .DistinctBy(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type)
            .OrderBy(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type)
            .ToList();

        return Task.FromResult(uniqueAccountingUnitDtoList);
    }

    #endregion

    #region GetUnionFormsDtoList

    private protected static Task<List<ShortForm11DTO>> GetUnionFormsDtoList(List<ShortForm11DTO> inventoryFormsDtoList,
        List<ShortForm11DTO> plusMinusFormsDtoList)
    {
        var unionFormsDtoList = inventoryFormsDtoList
            .Union(plusMinusFormsDtoList)
            .ToList();

        return Task.FromResult(unionFormsDtoList);
    }

    #endregion

    #region SerialNumbersIsEmpty
    
    private protected static bool SerialNumbersIsEmpty(string? pasNum, string? facNum)
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
            AutoReplaceSimilarChars("без номера"),
            AutoReplaceSimilarChars("прим"),
            AutoReplaceSimilarChars("примечание"),
        ];
        return validStrings.Contains(num1) && validStrings.Contains(num2);
    } 
    
    #endregion

    #endregion
}