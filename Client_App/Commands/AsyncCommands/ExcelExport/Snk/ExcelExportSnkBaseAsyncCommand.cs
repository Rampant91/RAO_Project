﻿using Models.DBRealization;
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
using Client_App.Resources.CustomComparers;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

public abstract partial class ExcelExportSnkBaseAsyncCommand : ExcelBaseAsyncCommand
{
    #region Properties

    private protected static string[] GetMinusOperationsArray(string formNum)
    {
        return formNum switch
        {
            "1.1" or "1.3" or "1.4" =>
            [
                "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "47", "65", "67", "68", "71", "72",
                "81", "82", "83", "84", "98"
            ],

            _ => []
        };
    }

    private protected static string[] GetPlusOperationsArray(string formNum)
    {
        return formNum switch
        {
            "1.1" =>
            [
                "11", "12", "17", "18", "31", "32", "35", "37", "38", "39", "58", "73", "74", "75", "85", "86", "87", "88", "97"
            ],

            "1.3" or "1.4" => 
            [
                "11", "12", "15", "17", "18", "31", "32", "35", "37", "38", "39", "58", "65", "73", "74", "75", "85",
                "86", "87", "88", "97"
            ],

            _ => []
        };
    }

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

    #region ShortFormDTO
    
    private protected class ShortFormDTO
    {
        public int Id { get; set; }

        public int NumberInOrder { get; set; }

        public ShortReportDTO RepDto { get; set; }

        public string OpCode { get; set; }

        public DateOnly OpDate { get; set; }

        public string PasNum { get; set; }

        public string Type { get; set; }

        public string Radionuclids { get; set; }

        public string FacNum { get; set; }

        public int Quantity { get; set; }

        public string PackNumber { get; set; }
    }

    //private protected class ShortFormDTO : ShortFormDTO
    //{
    //    public int Quantity { get; set; }
    //}

    //private protected class ShortForm13DTO : ShortFormDTO; 
    
    #endregion

    #region ShortFormStringDateDTO

    private class ShortFormDateOnlyDTO
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

        public int? Quantity { get; set; }

        public string Radionuclids { get; set; }

        public string FacNum { get; set; }

        public string PackNumber { get; set; }
    }
    
    #endregion

    private protected class ShortFormStringDatesDTO
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
    /// <param name="formNum">Номер формы.</param>
    /// <param name="inventoryFormsDtoList">Список DTO операций инвентаризации.</param>
    /// <param name="plusMinusFormsDtoList">Список DTO операций приема/передачи.</param>
    /// <param name="rechargeFormsDtoList">Список DTO операций перезарядки.</param>
    /// <param name="zeroFormsFtoList">Список DTO нулевых операций (не приёма-передача и не инвентаризация).</param>
    /// <returns>Словарь из уникальных учётных единиц и списков операций с ними.</returns>
    private protected static async Task<Dictionary<UniqueUnitDto, List<ShortFormDTO>>> GetDictionary_UniqueUnitsWithOperations(string formNum,
        List<ShortFormDTO> inventoryFormsDtoList, List<ShortFormDTO> plusMinusFormsDtoList, List<ShortFormDTO> rechargeFormsDtoList,
        List<ShortFormDTO>? zeroFormsFtoList = null)
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

        List<ShortFormDTO> unionOperationList;
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

        var comparer = new CustomSnkEqualityComparer();
        var radsComparer = new CustomSnkRadionuclidsEqualityComparer();
        Dictionary<UniqueUnitDto, List<ShortFormDTO>> uniqueUnitWithAllOperationDictionary = [];
        foreach (var group in groupedOperationList)
        {
            foreach (var form in group)
            {
                var dto = new UniqueUnitDto(form.FacNum, form.PasNum, form.Radionuclids, form.Type, form.Quantity, form.PackNumber);

                var filteredDictionary = uniqueUnitWithAllOperationDictionary
                    .Where(keyValuePair =>
                        comparer.Equals(keyValuePair.Key.PasNum, form.PasNum)
                        && comparer.Equals(keyValuePair.Key.FacNum, form.FacNum)
                        && radsComparer.Equals(keyValuePair.Key.Radionuclids, form.Radionuclids)
                        && comparer.Equals(keyValuePair.Key.Type, form.Type)
                        && (comparer.Equals(keyValuePair.Key.PackNumber, form.PackNumber) || form.OpCode is "53" or "54")
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

                    if (SerialNumbersIsEmpty(pairWithLastOpDate.Key.PasNum, pairWithLastOpDate.Key.FacNum) && formNum != "1.3")
                    {
                        var quantity = await SumQuantityForEmptySerialNums(pairWithLastOpDate, formNum);
                        if (form.Quantity != quantity) continue;
                    }
                    pairWithLastOpDate.Value.Add(form);
                    uniqueUnitWithAllOperationDictionary.Remove(pairWithLastOpDate.Key);
                    uniqueUnitWithAllOperationDictionary.Add(dto, pairWithLastOpDate.Value);
                }
            }
        }

        uniqueUnitWithAllOperationDictionary = uniqueUnitWithAllOperationDictionary
            .OrderBy(x => x.Key.PasNum)
            .ThenBy(x => x.Key.FacNum)
            .ToDictionary();

        return await Task.FromResult(uniqueUnitWithAllOperationDictionary);
    }

    #region GetGroupedOperationList

    /// <summary>
    /// Группирует список DTO операций, каждая группа заканчивается операцией перезарядки с кодом 53/54, возвращает список таких групп операций.
    /// </summary>
    /// <param name="unionOperationList">Список DTO операций.</param>
    /// <returns>Список сгруппированных DTO операций.</returns>
    private static Task<List<List<ShortFormDTO>>> GetGroupedOperationList(List<ShortFormDTO> unionOperationList)
    {
        List<List<ShortFormDTO>> groupedOperationList = [];
        List<ShortFormDTO> currentGroup = [];
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
    /// <param name="formNum">Номер формы.</param>
    /// <returns>Суммированное количество.</returns>
    private static Task<int> SumQuantityForEmptySerialNums(KeyValuePair<UniqueUnitDto, List<ShortFormDTO>> pairWithLastOpDate, string formNum)
    {
        var quantity = pairWithLastOpDate.Value
            .FirstOrDefault(x => x.OpCode == "10")
            ?.Quantity ?? 0; ;
        foreach (var form11Dto in pairWithLastOpDate.Value)
        {
            if (GetPlusOperationsArray(formNum).Contains(form11Dto.OpCode))
            {
                quantity += form11Dto.Quantity;
            }
            else if (GetMinusOperationsArray(formNum).Contains(form11Dto.OpCode))
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
    /// <param name="formNum">Номер формы.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO операций инвентаризации, отсортированный по датам, с фильтром по дате от 01.01.2022 до введённой пользователем даты.</returns>
    private protected static async Task<(DateOnly, List<ShortFormDTO>, List<ShortFormDTO>)> GetInventoryFormsDtoList(DBModel db, 
        List<ShortReportDTO> inventoryReportDtoList, string formNum, DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
        List<ShortFormDTO> inventoryFormsDtoList = [];
        foreach (var reportDto in inventoryReportDtoList)
        {
            switch (formNum)
            {
                #region 1.1
                
                case "1.1":
                {
                    var currentInventoryForms11StringDateDtoList = await db.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                        .Include(x => x.Rows11)
                        .Where(rep => rep.Reports != null && rep.Reports.DBObservable != null && rep.Id == reportDto.Id)
                        .SelectMany(rep => rep.Rows11
                            .Where(form => form.OperationCode_DB == "10")
                            .Select(form11 => new ShortFormDateOnlyDTO
                            {
                                Id = form11.Id,
                                RepId = reportDto.Id,
                                StDate = reportDto.StartPeriod,
                                EndDate = reportDto.EndPeriod,
                                NumberInOrder = form11.NumberInOrder_DB,
                                OpCode = form11.OperationCode_DB,
                                OpDate = form11.OperationDate_DB,
                                PasNum = snkParams == null || snkParams.CheckPasNum
                                    ? form11.PassportNumber_DB
                                    : string.Empty,
                                Type = snkParams == null || snkParams.CheckType
                                    ? form11.Type_DB
                                    : string.Empty,
                                Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                                    ? form11.Radionuclids_DB
                                    : string.Empty,
                                FacNum = snkParams == null || snkParams.CheckFacNum
                                    ? form11.FactoryNumber_DB
                                    : string.Empty,
                                Quantity = form11.Quantity_DB,
                                PackNumber = snkParams == null || snkParams.CheckPackNumber
                                    ? form11.PackNumber_DB
                                    : string.Empty
                            }))
                        .ToListAsync(cts.Token);

                    var currentInventoryForms11DtoList = currentInventoryForms11StringDateDtoList
                        .Where(x => DateTime.TryParse(x.OpDate, out var opDateTime)
                                    && DateOnly.FromDateTime(opDateTime) >= DateOnly.Parse("01.01.2022")
                                    && DateOnly.FromDateTime(opDateTime) <= endSnkDate)
                        .Select(x => new ShortFormDTO
                        {
                            Id = x.Id,
                            NumberInOrder = x.NumberInOrder,
                            RepDto = reportDto,
                            OpCode = x.OpCode,
                            OpDate = DateOnly.FromDateTime(DateTime.Parse(x.OpDate)),
                            PasNum = x.PasNum,
                            Type = x.Type,
                            Radionuclids = x.Radionuclids,
                            FacNum = x.FacNum,
                            Quantity = x.Quantity ?? 0,
                            PackNumber = x.PackNumber
                        });
                    inventoryFormsDtoList.AddRange(currentInventoryForms11DtoList);
                    break;
                }

                #endregion

                #region 1.3
                
                case "1.3":
                {
                    var currentInventoryForms13StringDateDtoList = await db.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                        .Include(x => x.Rows13)
                        .Where(rep => rep.Reports != null && rep.Reports.DBObservable != null && rep.Id == reportDto.Id)
                        .SelectMany(rep => rep.Rows13
                            .Where(form => form.OperationCode_DB == "10")
                            .Select(form13 => new ShortFormDateOnlyDTO
                            {
                                Id = form13.Id,
                                RepId = reportDto.Id,
                                StDate = reportDto.StartPeriod,
                                EndDate = reportDto.EndPeriod,
                                NumberInOrder = form13.NumberInOrder_DB,
                                OpCode = form13.OperationCode_DB,
                                OpDate = form13.OperationDate_DB,
                                PasNum = snkParams == null || snkParams.CheckPasNum
                                    ? form13.PassportNumber_DB
                                    : string.Empty,
                                Type = snkParams == null || snkParams.CheckType
                                    ? form13.Type_DB
                                    : string.Empty,
                                Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                                    ? form13.Radionuclids_DB
                                    : string.Empty,
                                FacNum = snkParams == null || snkParams.CheckFacNum
                                    ? form13.FactoryNumber_DB
                                    : string.Empty,
                                PackNumber = snkParams == null || snkParams.CheckPackNumber
                                    ? form13.PackNumber_DB
                                    : string.Empty
                            }))
                        .ToListAsync(cts.Token);

                    var currentInventoryForms13DtoList = currentInventoryForms13StringDateDtoList
                        .Where(x => DateTime.TryParse(x.OpDate, out var opDateTime)
                                    && DateOnly.FromDateTime(opDateTime) >= DateOnly.Parse("01.01.2022")
                                    && DateOnly.FromDateTime(opDateTime) <= endSnkDate)
                        .Select(x => new ShortFormDTO
                        {
                            Id = x.Id,
                            NumberInOrder = x.NumberInOrder,
                            RepDto = new ShortReportDTO(x.RepId, x.StDate, x.EndDate),
                            OpCode = x.OpCode,
                            OpDate = DateOnly.FromDateTime(DateTime.Parse(x.OpDate)),
                            PasNum = x.PasNum,
                            Type = x.Type,
                            Radionuclids = x.Radionuclids,
                            FacNum = x.FacNum,
                            Quantity = 1,
                            PackNumber = x.PackNumber
                        });
                    inventoryFormsDtoList.AddRange(currentInventoryForms13DtoList);
                    break;
                }

                #endregion
            }
        }

        var (summedInventoryFormsDtoList, inventoryDuplicateErrors) = await GetSummedInventoryDtoList(inventoryFormsDtoList, formNum);

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
    private static Task<(List<ShortFormDTO>, List<ShortFormDTO>)> GetSummedInventoryDtoList(List<ShortFormDTO> inventoryFormsDtoList, string formNum)
    {
        List<ShortFormDTO> newInventoryFormsDtoList = [];
        List<ShortFormDTO> inventoryDuplicateErrors = [];

        var comparer = new CustomSnkEqualityComparer();
        var radsComparer = new CustomSnkRadionuclidsEqualityComparer();
        foreach (var form in inventoryFormsDtoList)
        {
            var matchingForm = newInventoryFormsDtoList.FirstOrDefault(x =>
                x.OpDate == form.OpDate
                && comparer.Equals(x.PasNum, form.PasNum)
                && comparer.Equals(x.FacNum, form.FacNum)
                && radsComparer.Equals(x.Radionuclids, form.Radionuclids)
                && comparer.Equals(x.Type, form.Type)
                && comparer.Equals(x.PackNumber, form.PackNumber));

            if (matchingForm != null)
            {
                if (formNum is "1.3" || SerialNumbersIsEmpty(form.PasNum, form.FacNum))
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
    /// Получение списка DTO отчётов, содержащих хотя бы одну операцию с кодом 10.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id выбранной организации.</param>
    /// <param name="formNum">Номер формы</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO отчётов, отсортированный по датам.</returns>
    private protected static async Task<List<ShortReportDTO>> GetInventoryReportDtoList(DBModel db, int repsId, string formNum, DateOnly endSnkDate,
        CancellationTokenSource cts)
    {
        var inventoryReportDtoList = formNum switch
        {
            #region 1.1

            "1.1" => await db.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                    .Where(reps => reps.DBObservable != null && reps.Id == repsId)
                    .SelectMany(reps => reps.Report_Collection
                        .Where(rep => rep.FormNum_DB == formNum && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
                        .Select(rep => new ShortReportStringDateDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB)))
                    .ToListAsync(cts.Token),

            #endregion

            #region 1.3
            
            "1.3" => await db.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
                    .Where(reps => reps.DBObservable != null && reps.Id == repsId)
                    .SelectMany(reps => reps.Report_Collection
                        .Where(rep => rep.FormNum_DB == formNum && rep.Rows13.Any(form => form.OperationCode_DB == "10"))
                        .Select(rep => new ShortReportStringDateDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB)))
                    .ToListAsync(cts.Token),

            #endregion

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

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
    /// <param name="reportIds">Список Id отчётов у выбранной организации.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="firstSnkDate">>Дата первой инвентаризации после 01.01.2022, либо эта дата.</param>
    /// <param name="endSnkDate">Дата, по которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO форм с операциями приёма передачи, отсортированный по датам.</returns>
    private protected static async Task<List<ShortFormDTO>> GetPlusMinusFormsDtoList(DBModel db, List<int> reportIds, string formNum,
        DateOnly firstSnkDate, DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
        var plusOperationArray = GetPlusOperationsArray(formNum);
        var minusOperationArray = GetMinusOperationsArray(formNum);
        

        var plusMinusOperationDtoList = formNum switch
        {
            "1.1" => await db.form_11
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report)
                .Where(x => x.Report != null
                            && reportIds.Contains(x.Report.Id)
                            && (plusOperationArray.Contains(x.OperationCode_DB)
                                || minusOperationArray.Contains(x.OperationCode_DB)))
                .Select(form => new ShortFormStringDatesDTO
                {
                    Id = form.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    RepId = form.Report!.Id,
                    StDate = form.Report.StartPeriod_DB,
                    EndDate = form.Report.EndPeriod_DB,
                    FacNum = snkParams == null || snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams == null || snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams == null || snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Quantity = form.Quantity_DB,
                    Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams == null || snkParams.CheckType
                        ? form.Type_DB
                        : string.Empty
                })
                .ToListAsync(cts.Token),

            "1.3" => await db.form_13
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report)
                .Where(x => x.Report != null
                            && reportIds.Contains(x.Report.Id)
                            && (plusOperationArray.Contains(x.OperationCode_DB)
                                || minusOperationArray.Contains(x.OperationCode_DB)))
                .Select(form => new ShortFormStringDatesDTO
                {
                    Id = form.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    RepId = form.Report!.Id,
                    StDate = form.Report.StartPeriod_DB,
                    EndDate = form.Report.EndPeriod_DB,
                    FacNum = snkParams == null || snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams == null || snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams == null || snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams == null || snkParams.CheckType
                        ? form.Type_DB
                        : string.Empty
                })
                .ToListAsync(cts.Token),

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        return plusMinusOperationDtoList
            .Where(x => DateTime.TryParse(x.OpDate, out var opDateTime)
                                             && DateOnly.TryParse(x.StDate, out _)
                                             && DateOnly.TryParse(x.EndDate, out _)
                                             && DateOnly.FromDateTime(opDateTime) >= firstSnkDate
                                             && DateOnly.FromDateTime(opDateTime) <= endSnkDate)
            .Select(x => new ShortFormDTO 
            {
                Id = x.Id,
                NumberInOrder = x.NumberInOrder,
                RepDto = new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                OpCode = x.OpCode,
                OpDate = DateOnly.FromDateTime(DateTime.Parse(x.OpDate)),
                PasNum = x.PasNum,
                Type = x.Type,
                Radionuclids = x.Radionuclids,
                FacNum = x.FacNum,
                Quantity = x.Quantity ?? 0,
                PackNumber = x.PackNumber
            })
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
    /// <param name="formNum">Номер формы.</param>
    /// <param name="firstSnkDate">Дата первой инвентаризации после 01.01.2022, либо эта дата.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.
    /// Может быть null, тогда поиск ведётся по всем параметрам.</param>
    /// <returns>Список DTO форм с операциями перезарядки, отсортированный по датам.</returns>
    private protected static async Task<List<ShortFormDTO>> GetRechargeFormsDtoList(DBModel db, int repsId, string formNum, 
        DateOnly firstSnkDate, DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto? snkParams = null)
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
                .Where(rep => rep.FormNum_DB == formNum))
            .Select(rep => rep.Id)
            .ToListAsync(cts.Token);

        var rechargeOperationDtoList = formNum switch
        {
            "1.1" => await db.form_11
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report)
                .Where(x => x.Report != null
                            && reportIds.Contains(x.Report.Id)
                            && (x.OperationCode_DB == "53" || x.OperationCode_DB == "54"))
                .Select(form => new ShortFormStringDatesDTO
                {
                    Id = form.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    RepId = form.Report!.Id,
                    StDate = form.Report.StartPeriod_DB,
                    EndDate = form.Report.EndPeriod_DB,
                    FacNum = snkParams == null || snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams == null || snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams == null || snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Quantity = form.Quantity_DB,
                    Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams == null || snkParams.CheckType
                        ? form.Type_DB
                        : string.Empty
                })
                .ToListAsync(cts.Token),

            "1.3" => await db.form_13
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report)
                .Where(x => x.Report != null
                            && reportIds.Contains(x.Report.Id)
                            && (x.OperationCode_DB == "53" || x.OperationCode_DB == "54"))
                .Select(form => new ShortFormStringDatesDTO
                {
                    Id = form.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    RepId = form.Report!.Id,
                    StDate = form.Report.StartPeriod_DB,
                    EndDate = form.Report.EndPeriod_DB,
                    FacNum = snkParams == null || snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams == null || snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams == null || snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams == null || snkParams.CheckType
                        ? form.Type_DB
                        : string.Empty
                })
                .ToListAsync(cts.Token),

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        return rechargeOperationDtoList
            .Where(x => DateTime.TryParse(x.OpDate, out var opDateTime)
                                             && DateOnly.TryParse(x.StDate, out _)
                                             && DateOnly.TryParse(x.EndDate, out _)
                                             && DateOnly.FromDateTime(opDateTime) >= firstSnkDate
                                             && DateOnly.FromDateTime(opDateTime) <= endSnkDate)
            .Select(x => new ShortFormDTO 
            {
                Id = x.Id,
                NumberInOrder = x.NumberInOrder,
                RepDto = new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                OpCode = x.OpCode,
                OpDate = DateOnly.FromDateTime(DateTime.Parse(x.OpDate)),
                PasNum = x.PasNum,
                Type = x.Type,
                Radionuclids = x.Radionuclids,
                FacNum = x.FacNum,
                Quantity = x.Quantity ?? 0,
                PackNumber = x.PackNumber
            })
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ToList();
    }

    #endregion

    #region GetReportIds

    /// <summary>
    /// Возвращает id всех отчётов по заданной форме у выбранной организации.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id организации.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="cts">Токен.</param>
    /// <returns></returns>
    private protected static async Task<List<int>> GetReportIds(DBModel db, int repsId, string formNum, CancellationTokenSource cts)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Report_Collection)
            .Where(reps => reps.DBObservable != null && reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == formNum))
            .Select(rep => rep.Id)
            .ToListAsync(cts.Token);
    }

    #endregion

    #region GetUnitInStockDtoList

    /// <summary>
    /// Для каждой учётной единицы из словаря проверяется её наличие и выводится в общий список наличного количества (СНК).
    /// </summary>
    /// <param name="uniqueUnitWithAllOperationDictionary">Словарь из уникальной учётной единицы и списка всех операций с ней.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="firstInventoryDate">Дата первой инвентаризации.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <returns>Список DTO учётных единиц в наличии (СНК).</returns>
    private protected static async Task<List<ShortFormDTO>> GetUnitInStockDtoList(
        Dictionary<UniqueUnitDto, List<ShortFormDTO>> uniqueUnitWithAllOperationDictionary, string formNum,
        DateOnly firstInventoryDate, AnyTaskProgressBarVM progressBarVM)
    {
        var plusOperationArray = GetPlusOperationsArray(formNum);
        var minusOperationArray = GetMinusOperationsArray(formNum);

        List<ShortFormDTO> unitInStockList = [];
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;
        var comparer = new CustomSnkEqualityComparer();
        var radsComparer = new CustomSnkRadionuclidsEqualityComparer();
        foreach (var (unit, operations) in uniqueUnitWithAllOperationDictionary)
        {
            #region SerialNumEmpty
            
            if (SerialNumbersIsEmpty(unit.PasNum, unit.FacNum))
            {
                var quantity = operations
                    .FirstOrDefault(x => x.OpCode == "10" && x.OpDate == firstInventoryDate)
                    ?.Quantity ?? 0;

                var operationsWithoutDuplicates = await GetOperationsWithoutDuplicates(operations, formNum);

                foreach (var operation in operationsWithoutDuplicates)
                {
                    if (plusOperationArray.Contains(operation.OpCode))
                    {
                        quantity += operation.Quantity;
                    }
                    else if (minusOperationArray.Contains(operation.OpCode))
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
                    .FirstOrDefault(x => comparer.Equals(x.PasNum, unit.PasNum)
                                         && comparer.Equals(x.FacNum, unit.FacNum)
                                         && radsComparer.Equals(x.Radionuclids, unit.Radionuclids)
                                         && comparer.Equals(x.Type, unit.Type)
                                         && comparer.Equals(x.PackNumber, unit.PackNumber));

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

                var currentOperationsWithoutMutuallyExclusive = await GetOperationsWithoutMutuallyCompensating(operations, formNum);
                foreach (var form in currentOperationsWithoutMutuallyExclusive)
                {
                    if (plusOperationArray.Contains(form.OpCode)) inStock = true;
                    else if (minusOperationArray.Contains(form.OpCode)) inStock = false;
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

    /// <summary>
    /// Для форм 1.1 с незаполненными зав.№ и № паспорта, заменяет в списке операций множество +- операций в одну дату, на одну эквивалентную им операцию.
    /// </summary>
    /// <param name="operationList">Список операций.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <returns>Список операций, в котором множество +- операций в одну дату заменено на одну эквивалентную им операцию.</returns>
    private static Task<List<ShortFormDTO>> GetOperationsWithoutDuplicates(List<ShortFormDTO> operationList, string formNum)
    {
        var plusOperationsArray = GetPlusOperationsArray(formNum);
        var minusOperationsArray = GetMinusOperationsArray(formNum);

        List<ShortFormDTO> operationsWithoutDuplicates = [];
        foreach (var group in operationList.GroupBy(x => x.OpDate))
        {
            var countPlus = group
                .Where(x => plusOperationsArray.Contains(x.OpCode))
                .Sum(x => x.Quantity);

            var countMinus = group
                .Where(x => minusOperationsArray.Contains(x.OpCode))
                .Sum(x => x.Quantity);

            var givenReceivedPerDayAmount = countPlus - countMinus;

            switch (givenReceivedPerDayAmount)
            {
                case > 0:
                {
                    var lastOp = group.Last(x => plusOperationsArray.Contains(x.OpCode));
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
                    var lastOp = group.Last(x => minusOperationsArray.Contains(x.OpCode));
                    lastOp.Quantity = int.Abs(givenReceivedPerDayAmount);
                    operationsWithoutDuplicates.Add(lastOp);
                    break;
                }
            }
        }
        return Task.FromResult(operationsWithoutDuplicates);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="operationList">Список операций.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <returns></returns>
    private protected static Task<List<ShortFormDTO>> GetOperationsWithoutMutuallyCompensating(List<ShortFormDTO> operationList, string formNum)
    {
        var plusOperationsArray = GetPlusOperationsArray(formNum);
        var minusOperationsArray = GetMinusOperationsArray(formNum);

        var operationsGroupedByDate = operationList.Where(x =>
                !plusOperationsArray.Contains(x.OpCode) && !minusOperationsArray.Contains(x.OpCode)
                || plusOperationsArray.Contains(x.OpCode) || minusOperationsArray.Contains(x.OpCode))
            .GroupBy(x => x.OpDate)
            .ToList();

        var operationWithoutMutuallyExclusive = new List<ShortFormDTO>();
        foreach (var group in operationsGroupedByDate)
        {
            var formsList = group.ToList();
            foreach (var form in formsList)
            {
                var currentFormIsPlus = plusOperationsArray.Contains(form.OpCode);
                var currentFormIsMinus = minusOperationsArray.Contains(form.OpCode);

                var duplicate = operationWithoutMutuallyExclusive.FirstOrDefault(x =>
                    x.OpDate == form.OpDate
                    && (currentFormIsMinus && plusOperationsArray.Contains(x.OpCode)
                        || currentFormIsPlus && minusOperationsArray.Contains(x.OpCode)));

                if (duplicate is not null)
                {
                    operationWithoutMutuallyExclusive.Remove(duplicate);
                }
                else
                {
                    operationWithoutMutuallyExclusive.Add(form);
                }
            }
        }
        return Task.FromResult(operationWithoutMutuallyExclusive);
    }

    #endregion

    #region GetUniqueAccountingUnitDtoList

    /// <summary>
    /// Получение отсортированного списка DTO уникальных учётных единиц с операциями инвентаризации, приёма или передачи.
    /// </summary>
    /// <param name="unionFormsDtoList">Список DTO всех операций инвентаризации, приёма или передачи.</param>
    /// <returns>Список DTO уникальных учётных единиц с операциями инвентаризации, приёма или передачи.</returns>
    private protected static Task<List<UniqueAccountingUnitDTO>> GetUniqueAccountingUnitDtoList(List<ShortFormDTO> unionFormsDtoList)
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
        var regex = MyRegex();
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

    #region AutoReplaceSimilarChars

    private static string AutoReplaceSimilarChars(string? str)
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

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    private static partial Regex MyRegex();

    #endregion

    #endregion

    #endregion
}