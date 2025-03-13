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
using Client_App.ViewModels.ProgressBar;
using Models.Collections;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

public abstract class ExcelExportSnkBaseAsyncCommand : ExcelBaseAsyncCommand
{
    #region Properties

    /// <summary>
    /// Коды операций снятия с учёта.
    /// </summary>
    private protected static readonly string[] MinusOperation =
    [
        "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "47", "65", "67", "68", "71", "72", "81", "82", "83", "84", "98"
    ];


    /// <summary>
    /// Коды операций постановки на учёт.
    /// </summary>
    private protected static readonly string[] PlusOperation =
    [
        "11", "12", "17", "18", "31", "32", "35", "37", "38", "39", "58", "73", "74", "75", "85", "86", "87", "88", "97"
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

    private protected class ShortForm11DTO(int id, int  numberInOrder, ShortReportDTO repDto, string opCode, DateOnly opDate, string pasNum, string type, 
        string radionuclids, string facNum, int quantity, string packNumber)
    {
        public readonly int Id = id;

        public readonly int NumberInOrder = numberInOrder;

        public readonly ShortReportDTO RepDto = repDto;

        public readonly string OpCode = opCode;

        public readonly DateOnly OpDate = opDate;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;

        public readonly string Radionuclids = radionuclids;

        public readonly string FacNum = facNum;

        public int Quantity = quantity;

        public readonly string PackNumber = packNumber;
    }

    private class ShortForm11StringDateDTO
    {
        public int Id { get; set; }

        public int RepId { get; set; }

        public DateOnly StDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int NumberInOrder { get; set; }

        public string OpCode { get; set; }

        public string OpDate { get; set; }

        public string PasNum { get; set; }

        public string Type { get; set; }

        public string Radionuclids { get; set; }

        public string FacNum { get; set; }

        public int? Quantity { get; set; }

        public string PackNumber { get; set; }
    }

    private protected class ShortForm11StringDatesDTO
    {
        public int Id { get; set; }

        public int NumberInOrder { get; set; }

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

    #region UniqueUnitDto
    
    private protected class UniqueUnitDto(string facNum, string pasNum, string radionuclids, string type, int quantity, string packNumber)
    {
        public string FacNum { get; } = facNum;

        public string PasNum { get; } = pasNum;

        public string Radionuclids { get; } = radionuclids;

        public string Type { get; } = type;

        public int Quantity { get; } = quantity;

        public string PackNumber { get; } = packNumber;
    }

    #endregion

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
            .Replace('А', 'A')
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('В', 'B')
            .Replace('г', 'r')
            .Replace('Е', 'E')
            .Replace('е', 'e')
            .Replace('Ё', 'E')
            .Replace('ё', 'e')
            .Replace('К', 'K')
            .Replace('к', 'k')
            .Replace('М', 'M')
            .Replace('м', 'm')
            .Replace('Н', 'H')
            .Replace('О', 'O')
            .Replace('о', 'o')
            .Replace('0', 'O')
            .Replace('Р', 'P')
            .Replace('р', 'p')
            .Replace('С', 'C')
            .Replace('с', 'c')
            .Replace('Т', 'T')
            .Replace('У', 'Y')
            .Replace('у', 'y')
            .Replace('Х', 'X')
            .Replace('х', 'x')
            .ToLower();
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
    private protected static async Task CheckRepsAndRepPresence(string formNum, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;
        var selectedReports = (Reports?)mainWindow?.SelectedReports?.FirstOrDefault();

        if (selectedReports is null)
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
                    MinHeight = 100,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion

    #region GetDictionary_UniqueUnitsWithOperations

    /// <summary>
    /// Формирует словарь из уникальных учётных единиц и списков операций с ними.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO операций инвентаризации.</param>
    /// <param name="plusMinusFormsDtoList">Список DTO операций приема/передачи.</param>
    /// <param name="rechargeFormsDtoList">Список DTO операций перезарядки.</param>
    /// <param name="zeroFormsFtoList">Список DTO нулевых операций (не приёма-передача и не инвентаризация).</param>
    /// <returns>Словарь из уникальных учётных единиц и списков операций с ними.</returns>
    private protected static async Task<Dictionary<UniqueUnitDto, List<ShortForm11DTO>>> GetDictionary_UniqueUnitsWithOperations(
        List<ShortForm11DTO> inventoryFormsDtoList,
        List<ShortForm11DTO> plusMinusFormsDtoList,
        List<ShortForm11DTO> rechargeFormsDtoList,
        List<ShortForm11DTO>? zeroFormsFtoList = null)
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

        List<ShortForm11DTO> unionOperationList = [];
        if (zeroFormsFtoList is null)
        {
            unionOperationList = firstDateInventoryList
                .Union(plusMinusFormsDtoList)
                .Union(rechargeFormsDtoList)
                .ToList();
        }
        else
        {
            unionOperationList = inventoryFormsDtoList
                .Union(plusMinusFormsDtoList)
                .Union(rechargeFormsDtoList)
                .Union(zeroFormsFtoList)
                .ToList();
        }

        var groupedOperationList = await GetGroupedOperationList(unionOperationList);

        Dictionary<UniqueUnitDto, List<ShortForm11DTO>> uniqueUnitWithAllOperationDictionary = [];
        foreach (var group in groupedOperationList)
        {
            foreach (var form in group)
            {
                var dto = new UniqueUnitDto(form.FacNum, form.PasNum, form.Radionuclids, form.Type, form.Quantity, form.PackNumber);

                var filteredDictionary = uniqueUnitWithAllOperationDictionary
                    .Where(keyValuePair => keyValuePair.Key.PasNum == form.PasNum
                                           && keyValuePair.Key.FacNum == form.FacNum
                                           && keyValuePair.Key.Radionuclids == form.Radionuclids
                                           && keyValuePair.Key.Type == form.Type
                                           && (keyValuePair.Key.PackNumber == form.PackNumber || form.OpCode is "53" or "54")
                                           && (SerialNumbersIsEmpty(keyValuePair.Key.PasNum, keyValuePair.Key.FacNum)
                                               || keyValuePair.Key.Quantity == form.Quantity))
                    .ToDictionary();

                // Если запись в словаре отсутствует, то добавляем новую и переходим к следующей форме.
                if (filteredDictionary.Count == 0)
                {
                    uniqueUnitWithAllOperationDictionary.Add(dto, [form]);
                    continue;
                }

                // Если операция приема/передачи/инвентаризации/нулевая и есть совпадение с имеющейся, то добавляем операцию к уже имеющейся в словаре.
                if (form.OpCode is not "53" and not "54")
                {
                    filteredDictionary.First().Value.Add(form);
                }

                // Если операция перезарядки, то суммируем количество, если серийные номера пусты и заменяем запись в словаре
                else
                {
                    var lastForm = filteredDictionary
                        .SelectMany(x => x.Value)
                        .OrderByDescending(y => y.OpDate)
                        .First();
                    var pairWithLastOpDate = filteredDictionary
                        .First(x => x.Value.Contains(lastForm));

                    if (SerialNumbersIsEmpty(pairWithLastOpDate.Key.PasNum, pairWithLastOpDate.Key.FacNum))
                    {
                        var quantity = await SumQuantityForEmptySerialNums(pairWithLastOpDate);
                        if (form.Quantity != quantity) continue;
                    }
                    pairWithLastOpDate.Value.Add(form);
                    uniqueUnitWithAllOperationDictionary.Remove(pairWithLastOpDate.Key);
                    uniqueUnitWithAllOperationDictionary.Add(dto, pairWithLastOpDate.Value);
                }
            }
        }
        return await Task.FromResult(uniqueUnitWithAllOperationDictionary);
    }

    #region GetGroupedOperationList

    /// <summary>
    /// Группирует список DTO операций, каждая группа заканчивается операцией перезарядки с кодом 53/54, возвращает список таких групп операций.
    /// </summary>
    /// <param name="unionOperationList">Список DTO операций.</param>
    /// <returns>Список сгруппированных DTO операций.</returns>
    private static Task<List<List<ShortForm11DTO>>> GetGroupedOperationList(List<ShortForm11DTO> unionOperationList)
    {
        List<List<ShortForm11DTO>> groupedOperationList = [];
        List<ShortForm11DTO> currentGroup = [];
        var opCount = 0;
        foreach (var form in unionOperationList
                     .OrderBy(x => x.OpDate)
                     .ThenBy(x => x.RepDto.StartPeriod)
                     .ThenBy(x => x.RepDto.EndPeriod)
                     .ThenBy(x => x.NumberInOrder))
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
                groupedOperationList.Add([.. currentGroup]);
                currentGroup.Clear();
            }
        }
        if (groupedOperationList.Count == 0) groupedOperationList.Add(currentGroup);
        return Task.FromResult(groupedOperationList);
    }

    #endregion

    #region SumQuantityForEmptySerialNums

    /// <summary>
    /// Рассчитывает количество, путём сложения количества в первой операции инвентаризации и операциях приёма/передачи.
    /// </summary>
    /// <param name="pairWithLastOpDate">Пара ключ-значение из DTO уникальной учётной единицы и списка операций с ней.</param>
    /// <returns>Суммированное количество.</returns>
    private static Task<int> SumQuantityForEmptySerialNums(KeyValuePair<UniqueUnitDto, List<ShortForm11DTO>> pairWithLastOpDate)
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
        return Task.FromResult(quantity);
    }

    #endregion

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
    /// <returns>Список DTO операций инвентаризации, отсортированный по датам, с фильтром по дате от 01.01.2022 до введённой пользователем даты.</returns>
    private protected static async Task<(DateOnly, List<ShortForm11DTO>, List<ShortForm11DTO>)> GetInventoryFormsDtoList(DBModel db, List<ShortReportDTO> inventoryReportDtoList,
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
                        NumberInOrder = form11.NumberInOrder_DB,
                        OpCode = form11.OperationCode_DB,
                        OpDate = form11.OperationDate_DB,
                        PasNum = snkParams == null || snkParams.CheckPasNum ? form11.PassportNumber_DB : string.Empty,
                        Type = snkParams == null || snkParams.CheckType ? form11.Type_DB : string.Empty,
                        Radionuclids = snkParams == null || snkParams.CheckRadionuclids ? form11.Radionuclids_DB : string.Empty,
                        FacNum = snkParams == null || snkParams.CheckFacNum ? form11.FactoryNumber_DB : string.Empty,
                        Quantity = form11.Quantity_DB,
                        PackNumber = snkParams == null || snkParams.CheckPackNumber ? form11.PackNumber_DB : string.Empty
                    }))
                .ToListAsync(cts.Token);

            var currentInventoryFormsDtoList = currentInventoryFormsStringDateDtoList
                .Where(x => DateTime.TryParse(x.OpDate, out var opDateTime) 
                            && DateOnly.FromDateTime(opDateTime) >= DateOnly.Parse("01.01.2022")
                            && DateOnly.FromDateTime(opDateTime) <= endSnkDate)
                .Select(x => new ShortForm11DTO(
                    x.Id,
                    x.NumberInOrder,
                    reportDto,
                    x.OpCode,
                    DateOnly.FromDateTime(DateTime.Parse(x.OpDate)),
                    AutoReplaceSimilarChars(x.PasNum),
                    AutoReplaceSimilarChars(x.Type),
                    AutoReplaceSimilarChars(x.Radionuclids),
                    AutoReplaceSimilarChars(x.FacNum),
                    x.Quantity ?? 0,
                    AutoReplaceSimilarChars(x.PackNumber)
                    ));

            inventoryFormsDtoList.AddRange(currentInventoryFormsDtoList);
        }
        var (summedInventoryFormsDtoList, inventoryDuplicateErrors) = await GetSummedInventoryDtoList(inventoryFormsDtoList);

        var firstInventoryDate = summedInventoryFormsDtoList.Count == 0
            ? new DateOnly(2022, 1, 1)
            : summedInventoryFormsDtoList
                .OrderBy(x => x.OpDate)
                .Select(x => x.OpDate)
                .First();

        var orderedInventoryFormsDtoList = summedInventoryFormsDtoList
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();

        return (firstInventoryDate, orderedInventoryFormsDtoList, inventoryDuplicateErrors);
    }

    #region GetSummedInventoryDtoList

    /// <summary>
    /// Суммирует операции инвентаризации для первой даты по количеству и возвращает список DTO.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO операций инвентаризации.</param>
    /// <returns>Список DTO операций инвентаризации, просуммированный по количеству для первой даты.</returns>
    private static Task<(List<ShortForm11DTO>, List<ShortForm11DTO>)> GetSummedInventoryDtoList(List<ShortForm11DTO> inventoryFormsDtoList)
    {
        List<ShortForm11DTO> newInventoryFormsDtoList = [];
        List<ShortForm11DTO> inventoryDuplicateErrors = [];

        foreach (var form in inventoryFormsDtoList)
        {
            var matchingForm = newInventoryFormsDtoList.FirstOrDefault(x =>
                x.OpDate == form.OpDate
                && x.PasNum == form.PasNum
                && x.FacNum == form.FacNum
                && x.Radionuclids == form.Radionuclids
                && x.Type == form.Type
                && x.PackNumber == form.PackNumber);

            if (matchingForm != null)
            {
                if (SerialNumbersIsEmpty(form.PasNum, form.FacNum))
                {
                    matchingForm.Quantity += form.Quantity;
                }
                else
                {
                    inventoryDuplicateErrors.Add(matchingForm);
                }
            }
            else
            {
                newInventoryFormsDtoList.Add(form);
            }
        }
        return Task.FromResult((newInventoryFormsDtoList, inventoryDuplicateErrors));
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
    /// <param name="reportIds">Id отчётов по форме 1.1 у выбранной организации.</param>
    /// <param name="firstSnkDate">>Дата первой инвентаризации после 01.01.2022, либо эта дата.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO форм с операциями приёма передачи, отсортированный по датам.</returns>
    private protected static async Task<List<ShortForm11DTO>> GetPlusMinusFormsDtoList(DBModel db, List<int> reportIds, DateOnly firstSnkDate, DateOnly endSnkDate,
        CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
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

        return plusMinusOperationDtoList
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
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();
    }

    #endregion

    private protected static async Task<List<int>> GetReportIds(DBModel db, int repsId, CancellationTokenSource cts)
    {
        return await db.ReportsCollectionDbSet
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
    }

    #region GetRechargeFormsDtoList

    /// <summary>
    /// Получение списка DTO форм с операциями перезарядки.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id организации.</param>
    /// <param name="firstSnkDate">Дата первой инвентаризации после 01.01.2022, либо эта дата.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO форм с операциями перезарядки, отсортированный по датам.</returns>
    private protected static async Task<List<ShortForm11DTO>> GetRechargeFormsDtoList(DBModel db, int repsId, DateOnly firstSnkDate, DateOnly endSnkDate,
        CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
        snkParams ??= new SnkParamsDto(true, true, true, true, true);

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

        return rechargeOperationDtoList
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
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();
    }

    #endregion

    #region GetUnitInStockDtoList

    /// <summary>
    /// Для каждой учётной единицы из словаря проверяется её наличие и выводится в общий список наличного количества (СНК).
    /// </summary>
    /// <param name="uniqueUnitWithAllOperationDictionary">Словарь из уникальной учётной единицы и списка всех операций с ней.</param>
    /// <param name="firstInventoryDate">Дата первой инвентаризации.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <returns>Список DTO учётных единиц в наличии (СНК).</returns>
    private protected static async Task<List<ShortForm11DTO>> GetUnitInStockDtoList(Dictionary<UniqueUnitDto, List<ShortForm11DTO>> uniqueUnitWithAllOperationDictionary, 
        DateOnly firstInventoryDate, AnyTaskProgressBarVM progressBarVM)
    {
        List<ShortForm11DTO> unitInStockList = [];
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;
        foreach (var (unit, operations) in uniqueUnitWithAllOperationDictionary)
        {
            #region SerialNumEmpty
            
            if (SerialNumbersIsEmpty(unit.PasNum, unit.FacNum))
            {
                var quantity = operations
                    .FirstOrDefault(x => x.OpCode == "10" && x.OpDate == firstInventoryDate)
                    ?.Quantity ?? 0;

                var operationsWithoutDuplicates = await GetOperationsWithoutDuplicates(operations);

                foreach (var operation in operationsWithoutDuplicates)
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

                var lastOperationWithUnit = operations
                    .OrderByDescending(x => x.OpDate)
                    .FirstOrDefault();

                if (lastOperationWithUnit == null) continue;

                var currentUnit = unitInStockList
                    .FirstOrDefault(x => x.PasNum == unit.PasNum
                                         && x.FacNum == unit.FacNum
                                         && x.Radionuclids == unit.Radionuclids
                                         && x.Type == unit.Type
                                         && x.PackNumber == unit.PackNumber);

                if (currentUnit != null) unitInStockList.Remove(currentUnit);

                if (quantity > 0)
                {
                    lastOperationWithUnit.Quantity = quantity;
                    unitInStockList.Add(lastOperationWithUnit);
                }
            }

            #endregion

            #region SerialNumNotEmpty
            
            else
            {
                var inStock = operations.Any(x => x.OpCode == "10" && x.OpDate == firstInventoryDate);

                var currentOperationsWithoutMutuallyExclusive = await GetOperationsWithoutMutuallyExclusive(operations);
                foreach (var form in currentOperationsWithoutMutuallyExclusive)
                {
                    if (PlusOperation.Contains(form.OpCode)) inStock = true;
                    else if (MinusOperation.Contains(form.OpCode)) inStock = false;
                }
                if (inStock)
                {
                    var lastOperationWithUnit = currentOperationsWithoutMutuallyExclusive
                        .OrderByDescending(x => x.OpDate)
                        .FirstOrDefault();

                    if (lastOperationWithUnit != null)
                    {
                        unitInStockList.Add(lastOperationWithUnit);
                    }
                }

            }

            #endregion

            progressBarDoubleValue += (double)10 / uniqueUnitWithAllOperationDictionary.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {currentUnitNum} единиц из {uniqueUnitWithAllOperationDictionary.Count}",
                "Проверка наличия");
            currentUnitNum++;
        }
        return unitInStockList;
    }

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

    private protected static Task<List<ShortForm11DTO>> GetOperationsWithoutMutuallyExclusive(List<ShortForm11DTO> currentOperations)
    {
        var currentOperationsGroupedByDate = currentOperations.Where(x =>
                !PlusOperation.Contains(x.OpCode) && !MinusOperation.Contains(x.OpCode)
                || PlusOperation.Contains(x.OpCode) || MinusOperation.Contains(x.OpCode))
            .GroupBy(x => x.OpDate)
            .ToList();

        var currentOperationWithoutMutuallyExclusive = new List<ShortForm11DTO>();
        foreach (var group in currentOperationsGroupedByDate)
        {
            var formsList = group.ToList();
            foreach (var form in formsList)
            {
                var currentFormIsPlus = PlusOperation.Contains(form.OpCode);
                var currentFormIsMinus = MinusOperation.Contains(form.OpCode);

                var duplicate = currentOperationWithoutMutuallyExclusive.FirstOrDefault(x =>
                    x.OpDate == form.OpDate
                    && (currentFormIsMinus && MinusOperation.Contains(x.OpCode)
                        || currentFormIsPlus && PlusOperation.Contains(x.OpCode)));

                if (duplicate is not null)
                {
                    currentOperationWithoutMutuallyExclusive.Remove(duplicate);
                }
                else
                {
                    currentOperationWithoutMutuallyExclusive.Add(form);
                }
            }
        }
        return Task.FromResult(currentOperationWithoutMutuallyExclusive);
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