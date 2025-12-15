using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Messages;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CustomSnkEqualityComparer = Client_App.Resources.CustomComparers.SnkComparers.CustomSnkEqualityComparer;
using CustomSnkNumberEqualityComparer = Client_App.Resources.CustomComparers.SnkComparers.CustomSnkNumberEqualityComparer;
using CustomSnkRadionuclidsEqualityComparer = Client_App.Resources.CustomComparers.SnkComparers.CustomSnkRadionuclidsEqualityComparer;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Базовый класс выгрузки СНК в Excel.
/// </summary>
public abstract partial class ExcelExportSnkBaseAsyncCommand : ExcelBaseAsyncCommand
{
    private sealed class SnkGroupKeyComparer : IEqualityComparer<(string PasNum, string FacNum, string Radionuclids, string Type)>
    {
        private readonly CustomSnkNumberEqualityComparer _numberComparer = new();
        private readonly CustomSnkRadionuclidsEqualityComparer _radsComparer = new();
        private readonly CustomSnkEqualityComparer _stringComparer = new();

        public bool Equals((string PasNum, string FacNum, string Radionuclids, string Type) x,
            (string PasNum, string FacNum, string Radionuclids, string Type) y)
        {
            return _numberComparer.Equals(x.PasNum, y.PasNum)
                   && _numberComparer.Equals(x.FacNum, y.FacNum)
                   && _radsComparer.Equals(x.Radionuclids, y.Radionuclids)
                   && _stringComparer.Equals(x.Type, y.Type);
        }

        public int GetHashCode((string PasNum, string FacNum, string Radionuclids, string Type) obj)
        {
            return HashCode.Combine(
                _numberComparer.GetHashCode(obj.PasNum),
                _numberComparer.GetHashCode(obj.FacNum),
                _radsComparer.GetHashCode(obj.Radionuclids),
                _stringComparer.GetHashCode(obj.Type));

            //return 0;
        }
    }

    #region Properties

    /// <summary>
    /// Получение массива операций на передачу (минусовых) для форм 1.1, 1.3, 1.4.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <returns>Массив операций на передачу (минусовых) для форм 1.1, 1.3, 1.4.</returns>
    private protected static string[] GetMinusOperationsArray(string formNum)
    {
        return formNum switch
        {
            "1.1" =>
            [
                "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "47", "65", "67", "68", "71", "72",
                "81", "82", "83", "84", "98"
            ],

            "1.3" or "1.4" =>
            [
                "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "47", "67", "68", "71", "72",
                "81", "82", "83", "84", "98"
            ],

            _ => []
        };
    }

    /// <summary>
    /// Получение массива операций на получение (плюсовых) для форм 1.1, 1.3, 1.4.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <returns>Массив операций на получение (плюсовых) для форм 1.1, 1.3, 1.4.</returns>
    private protected static string[] GetPlusOperationsArray(string formNum)
    {
        return formNum switch
        {
            "1.1" =>
            [
                "11", "12", "15", "17", "18", "31", "32", "35", "37", "38", "39", "58", "73", "74", "75", "85", "86", "87", "88", "97"
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

    #region Methods

    #region AskSnkEndDate

    /// <summary>
    /// Запрос ввода даты формирования СНК/проверки инвентаризации.
    /// </summary>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Кортеж из даты, на которую необходимо сформировать СНК и dto bool флагов, по каким параметрам определять учётную единицу.</returns>
    private protected async Task<(DateOnly, SnkParamsDto)> AskSnkEndDate(AnyTaskProgressBar progressBar,
        CancellationTokenSource cts)
    {
        var vm = new GetSnkParamsVM();
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var getSnkParamsWindow = new GetSnkParams();
            getSnkParamsWindow._vm.CommandName = this switch
            {
                ExcelExportSnkAsyncCommand => "СНК",
                ExcelExportCheckInventoriesAsyncCommand => "Проверка инвентаризаций",
                _ => ""
            };
            await getSnkParamsWindow.ShowDialog(Desktop.MainWindow);
            vm = getSnkParamsWindow._vm;
        });

        var date = DateOnly.MinValue;
        if (DateTime.TryParse(vm.Date, out var dateTime)) date = DateOnly.FromDateTime(dateTime); 

        if (!vm.Ok)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        else if (!DateTime.TryParse(vm.Date, out _))
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

            date = DateOnly.FromDateTime(DateTime.Now);
        }
        else if (DateOnly.FromDateTime(dateTime) < DateOnly.Parse("01.01.2022"))
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
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
    private protected static async Task<Reports> CheckRepsAndRepPresence(string formNum, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
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

        return selectedReports!;
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
    /// <param name="zeroFormsDtoList">Список DTO нулевых операций (не приёма-передача и не инвентаризация).</param>
    /// <returns>Словарь из уникальных учётных единиц и списков операций с ними.</returns>
    private protected static async Task<Dictionary<UniqueUnitDto, List<ShortFormDTO>>> GetDictionary_UniqueUnitsWithOperations(
        string formNum, List<ShortFormDTO> inventoryFormsDtoList, List<ShortFormDTO> plusMinusFormsDtoList, 
        List<ShortFormDTO> rechargeFormsDtoList, List<ShortFormDTO>? zeroFormsDtoList = null)
    {
        var firstInventoryDate = inventoryFormsDtoList.Count == 0
            ? DateOnly.MinValue
            : inventoryFormsDtoList
                .OrderBy(x => x.OpDate)
                .Select(x => x.OpDate)
                .First();

        var firstDateInventoryList = inventoryFormsDtoList
            .Where(x => x.OpDate == firstInventoryDate);

        IEnumerable<ShortFormDTO> unionOperationList;
        if (zeroFormsDtoList is null)
        {
            unionOperationList = firstDateInventoryList
                .Union(plusMinusFormsDtoList)
                .Union(rechargeFormsDtoList);
        }
        else
        {
            unionOperationList = inventoryFormsDtoList
                .Union(plusMinusFormsDtoList)
                .Union(rechargeFormsDtoList)
                .Union(zeroFormsDtoList);
        }

        var snkGroupKeyComparer = new SnkGroupKeyComparer();

        var groupedOperationListDictionary = unionOperationList
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ThenBy(x => x.NumberInOrder)
            .GroupBy(
                x => (x.PasNum, x.FacNum, x.Radionuclids, x.Type),
                (key, items) => new
                {
                    Key = key,
                    DateGroups = items
                        .GroupBy(x => x.OpDate)
                        .ToDictionary(
                            g => g.Key,
                            g => g.ToList()
                        )
                },
                snkGroupKeyComparer
            )
            .OrderBy(x => x.Key.PasNum)
            .ThenBy(x => x.Key.FacNum)
            .ToDictionary(x => x.Key, x => x.DateGroups);

        var comparer = new CustomSnkEqualityComparer();
        var numberComparer = new CustomSnkNumberEqualityComparer();
        var radsComparer = new CustomSnkRadionuclidsEqualityComparer();
        Dictionary<UniqueUnitDto, List<ShortFormDTO>> uniqueUnitWithAllOrderedOperationDictionary = [];
        var j = 0;
        foreach (var (unit, formsByDateDictionary) in groupedOperationListDictionary)
                     //.Where(x => x.Key.PasNum is "1231"))
        {
            j++;
            var currentPackNumber = "";
            var currentQuantity = 0;

            var inStock = formsByDateDictionary.Values
                .SelectMany(x => x)
                .Any(x => x.OpCode is "10" && x.OpDate == firstInventoryDate);

            if (inStock)
            {
                var inventoryForm = formsByDateDictionary.Values
                    .SelectMany(x => x)
                    .First(x => x.OpCode is "10" && x.OpDate == firstInventoryDate);

                currentPackNumber = inventoryForm.PackNumber;
                currentQuantity = inventoryForm.Quantity;
            }

            foreach (var (_, formsList) in formsByDateDictionary)
            {
                List<ShortFormDTO> newOperationOrderList = [];

                var editedFormsList = formsList.ToList();

                //Если есть операции инвентаризации
                if (formsList.Any(x => x.OpCode is "10"))
                {
                    //Если это первая операция с учётной единицей, то операции инвентаризации идут в начале
                    if (newOperationOrderList.Count == 0)
                    {
                        editedFormsList = editedFormsList
                            .OrderBy(x => x.OpCode is not "10")
                            .ToList();
                    }
                    //Если это не первая операция с учётной единицей, то операции инвентаризации идут в конце
                    else
                    {
                        editedFormsList = editedFormsList
                            .OrderBy(x => x.OpCode is "10")
                            .ToList();
                    }
                }

                var isPaired = true;
                    //IsPairedList(editedFormsList, inStock, currentPackNumber, formNum);

                for (var i = 0; i < editedFormsList.Count; i++)
                {
                    var form = editedFormsList[i];

                    var subsequentElementsList = editedFormsList
                        .Where((_, index) => index > i)
                        .ToList();

                    #region OneOperationPerDay

                    //Если в этот день только одна операция, то добавляем без изменений и переходим к следующему дню.
                    if (editedFormsList.Count is 1)
                    {
                        newOperationOrderList.Add(form);
                        currentPackNumber = form.PackNumber;
                        continue;
                    }

                    #endregion

                    #region Recharge
                    
                    //Если перезарядка
                    if (form.OpCode is "53" or "54")
                    {
                        //Если это первая операция с данным источником вообще,
                        //находим операции инвентаризации/получения в этот день и ставим перезарядку после этих операций.

                        var hasInventory = firstInventoryDate != DateOnly.MinValue 
                                           && formsByDateDictionary.Values
                                               .SelectMany(x => x)
                                               .Any(x => x.OpCode is "10" && x.OpDate == firstInventoryDate);

                        if (newOperationOrderList.Count == 0
                            && hasInventory 
                                ? form.OpDate == firstInventoryDate
                                : !inStock 
                                  && subsequentElementsList
                                      .Any(x => x.OpCode is "10" 
                                                || GetPlusOperationsArray(formNum).Contains(x.OpCode)))
                        {
                            var countInventoryAndPlusOperation =
                                subsequentElementsList.Count(x =>
                                    x.OpCode is "10" || GetPlusOperationsArray(formNum).Contains(x.OpCode));

                            editedFormsList.RemoveAt(i);
                            editedFormsList.Insert(i + countInventoryAndPlusOperation, form);
                            i--;
                        }

                        //Если в этот день есть необработанные операции не перезарядки с текущим номером упаковки,
                        //то помещаем операцию перезарядки после этих операций
                        else if (subsequentElementsList
                                 .Any(x => 
                                     (GetMinusOperationsArray(formNum).Contains(x.OpCode) 
                                      && numberComparer.Equals(x.PackNumber, currentPackNumber)) 
                                     || (!inStock 
                                         && GetPlusOperationsArray(formNum).Contains(x.OpCode)
                                         && !numberComparer.Equals(x.PackNumber, currentPackNumber)))
                                 && !isPaired)
                        {
                            var countOperationWithSamePackNumber = subsequentElementsList
                                .Count(x => 
                                    (GetMinusOperationsArray(formNum).Contains(x.OpCode) 
                                     && numberComparer.Equals(x.PackNumber, currentPackNumber)) 
                                    || (!inStock 
                                        && GetPlusOperationsArray(formNum).Contains(x.OpCode)
                                        && !numberComparer.Equals(x.PackNumber, currentPackNumber)));

                            editedFormsList.RemoveAt(i);
                            editedFormsList.Insert(i + countOperationWithSamePackNumber, form);
                            i--;
                        }
                        else
                        {
                            newOperationOrderList.Add(form);
                            currentPackNumber = form.PackNumber;
                        }
                    }

                    #endregion

                    #region Plus
                    
                    //Если операция получения
                    else if (GetPlusOperationsArray(formNum).Contains(form.OpCode))
                    {
                        //Если нет в наличии или (нет других операций с тем же номером упаковки или операций перезарядки)
                        if (!inStock
                            || subsequentElementsList.All(x => 
                                GetPlusOperationsArray(formNum).Contains(x.OpCode)))
                        {
                            newOperationOrderList.Add(form);
                            inStock = true;
                            currentPackNumber = form.PackNumber;
                        }
                        //Перемещаем эту операцию получения в конец списка
                        else
                        {
                            editedFormsList.RemoveAt(i);
                            editedFormsList.Add(form);
                            i--;
                        }
                    }

                    #endregion

                    #region Minus
                    
                    //Если операция передачи
                    else if (GetMinusOperationsArray(formNum).Contains(form.OpCode))
                    {
                        //(Если в наличии и номер упаковки совпадает)
                        //или (нет других операций с тем же номером упаковки или операций перезарядки)
                        if ((inStock && numberComparer.Equals(currentPackNumber, form.PackNumber))
                            || subsequentElementsList.All(x =>
                                GetMinusOperationsArray(formNum).Contains(x.OpCode))
                            )
                        {
                            newOperationOrderList.Add(form);
                            inStock = false;
                        }
                        //Перемещаем эту операцию передачи в конец списка
                        else
                        {
                            editedFormsList.RemoveAt(i);
                            editedFormsList.Add(form);
                            i--;
                        }
                    }

                    #endregion

                    #region Zero

                    //Если нулевая операция
                    else
                    {
                        newOperationOrderList.Add(form);
                    }

                    #endregion
                }

                var uniqueDto = new UniqueUnitDto(unit.FacNum, unit.PasNum, unit.Radionuclids, 
                    unit.Type, currentQuantity, currentPackNumber);

                if (!uniqueUnitWithAllOrderedOperationDictionary.Keys.Any(x =>
                        numberComparer.Equals(x.PasNum, uniqueDto.PasNum)
                        && numberComparer.Equals(x.FacNum, uniqueDto.FacNum)
                        && radsComparer.Equals(x.Radionuclids, uniqueDto.Radionuclids)
                        && comparer.Equals(x.Type, uniqueDto.Type)))
                {
                    uniqueUnitWithAllOrderedOperationDictionary.Add(uniqueDto, newOperationOrderList);
                }
                else
                {
                    var uniqUnit = uniqueUnitWithAllOrderedOperationDictionary.Keys.First(x =>
                        numberComparer.Equals(x.PasNum, uniqueDto.PasNum)
                        && numberComparer.Equals(x.FacNum, uniqueDto.FacNum)
                        && radsComparer.Equals(x.Radionuclids, uniqueDto.Radionuclids)
                        && comparer.Equals(x.Type, uniqueDto.Type));

                    uniqueUnitWithAllOrderedOperationDictionary[uniqUnit].AddRange(newOperationOrderList);
                }
            }
        }

        var orderedOperationList = uniqueUnitWithAllOrderedOperationDictionary
            .Select(x => x.Value);

        //var groupedOperationList = await GetGroupedOperationList(orderedOperationList);

        Dictionary<UniqueUnitDto, List<ShortFormDTO>> uniqueUnitWithAllOperationDictionary = [];
        foreach (var group in orderedOperationList)
        {
            foreach (var form in group)
            {
                var dto = new UniqueUnitDto(form.FacNum, form.PasNum, form.Radionuclids, form.Type, form.Quantity, form.PackNumber);

                var filteredDictionary = uniqueUnitWithAllOperationDictionary
                    .Where(keyValuePair =>
                        numberComparer.Equals(keyValuePair.Key.PasNum, form.PasNum)
                        && numberComparer.Equals(keyValuePair.Key.FacNum, form.FacNum)
                        && radsComparer.Equals(keyValuePair.Key.Radionuclids, form.Radionuclids)
                        && comparer.Equals(keyValuePair.Key.Type, form.Type)
                        && (numberComparer.Equals(keyValuePair.Key.PackNumber, form.PackNumber)
                            || form.OpCode is "53" or "54"
                            || keyValuePair.Value.All(x => x.OpCode is "53" or "54"))
                        && (formNum is "1.3"
                            || SerialNumbersIsEmpty(keyValuePair.Key.PasNum, keyValuePair.Key.FacNum)
                            || keyValuePair.Key.Quantity == form.Quantity))
                    .ToDictionary();

                // Если запись в словаре отсутствует, то добавляем новую и переходим к следующей форме.
                if (filteredDictionary.Count == 0)
                {
                    uniqueUnitWithAllOperationDictionary.Add(dto, [form]);
                    continue;
                }

                // Если операция приема/передачи/инвентаризации/нулевая и есть совпадение с имеющейся,
                // то добавляем операцию к уже имеющейся в словаре.
                if (form.OpCode is not "53" and not "54")
                {
                    filteredDictionary.First().Value.Add(form);

                    var lastOpDate = filteredDictionary
                        .SelectMany(x => x.Value)
                        .OrderByDescending(y => y.OpDate)
                        .First().OpDate;

                    //Если в последнюю дату несколько операций - берём за последнюю не минусовую.
                    ShortFormDTO? lastForm;
                    if (filteredDictionary
                            .SelectMany(x => x.Value)
                            .Where(x => x.OpCode != "10")
                            .Count(x => x.OpDate == lastOpDate) > 1)
                    {
                        lastForm = filteredDictionary
                            .SelectMany(x => x.Value)
                            .Where(x => x.OpCode != "10" && !GetMinusOperationsArray(formNum).Contains(x.OpCode))
                            .OrderByDescending(y => y.OpDate)
                            .ThenByDescending(x => x.RepDto.StartPeriod)
                            .ThenByDescending(x => x.RepDto.EndPeriod)
                            .ThenByDescending(x => x.NumberInOrder)
                            .FirstOrDefault();
                    }
                    else
                    {
                        lastForm = filteredDictionary
                            .SelectMany(x => x.Value)
                            .OrderByDescending(y => y.OpDate)
                            .FirstOrDefault();
                    }

                    if (lastForm is not null)
                    {
                        var pairWithLastOpDate = filteredDictionary
                            .First(x => x.Value.Contains(lastForm));

                        uniqueUnitWithAllOperationDictionary.Remove(pairWithLastOpDate.Key);
                        uniqueUnitWithAllOperationDictionary.Add(dto, pairWithLastOpDate.Value);
                    }
                }

                // Если операция перезарядки, то суммируем количество, если серийные номера пусты и заменяем запись в словаре
                else
                {
                    var lastOpDate = filteredDictionary
                        .SelectMany(x => x.Value)
                        .OrderByDescending(y => y.OpDate)
                        .First().OpDate;

                    //Если в последнюю дату несколько операций - берём за последнюю не минусовую.
                    ShortFormDTO? lastForm;
                    if (filteredDictionary
                            .SelectMany(x => x.Value)
                            .Where(x => x.OpCode != "10")
                            .Count(x => x.OpDate == lastOpDate) > 1)
                    {
                        lastForm = filteredDictionary
                            .SelectMany(x => x.Value)
                            .Where(x => x.OpCode != "10" && !GetMinusOperationsArray(formNum).Contains(x.OpCode))
                            .OrderByDescending(y => y.OpDate)
                            .ThenByDescending(x => x.RepDto.StartPeriod)
                            .ThenByDescending(x => x.RepDto.EndPeriod)
                            .ThenByDescending(x => x.NumberInOrder)
                            .FirstOrDefault();
                    }
                    else
                    {
                        lastForm = filteredDictionary
                            .SelectMany(x => x.Value)
                            .OrderByDescending(y => y.OpDate)
                            .FirstOrDefault();
                    }

                    if (lastForm is not null)
                    {
                        var pairWithLastOpDate = filteredDictionary
                            .First(x => x.Value.Contains(lastForm));

                        if (formNum is "1.3" || SerialNumbersIsEmpty(pairWithLastOpDate.Key.PasNum, pairWithLastOpDate.Key.FacNum))
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
        }

        uniqueUnitWithAllOperationDictionary = uniqueUnitWithAllOperationDictionary
            .OrderBy(x => x.Key.PasNum)
            .ThenBy(x => x.Key.FacNum)
            .ToDictionary();


        return await Task.FromResult(uniqueUnitWithAllOperationDictionary);
    }

    private static bool IsPairedList(List<ShortFormDTO> editedFormsList, bool inStock, string currentPackNumber, string formNum)
    {
        var numberComparer = new CustomSnkNumberEqualityComparer();
        var plusOperations = GetPlusOperationsArray(formNum);
        var minusOperations = GetMinusOperationsArray(formNum);
        var rechargeOperations = new[] { "53", "54" };

        // Filter out zero operations (like 64) and categorize the rest
        var plusOps = editedFormsList
            .Where(x => plusOperations.Contains(x.OpCode))
            .Select(x => new { Op = x, IsMatched = false })
            .ToList();

        var minusOps = editedFormsList
            .Where(x => minusOperations.Contains(x.OpCode))
            .Select(x => new { Op = x, IsMatched = false })
            .ToList();

        var rechargeOps = editedFormsList
            .Where(x => rechargeOperations.Contains(x.OpCode))
            .Select(x => new { Op = x, IsMatched = false })
            .ToList();

        // If unit is in stock, add a virtual plus operation
        if (inStock && !string.IsNullOrEmpty(currentPackNumber))
        {
            plusOps.Insert(0, new
            {
                Op = new ShortFormDTO
                {
                    OpCode = plusOperations.First(),
                    PackNumber = currentPackNumber
                },
                IsMatched = false
            });
        }

        // First pass: match plus and minus operations with the same PackNumber
        for (var i = 0; i < plusOps.Count; i++)
        {
            if (plusOps[i].IsMatched) continue;

            var plusOp = plusOps[i];
            var matchingMinusIndex = minusOps.FindIndex(m =>
                !m.IsMatched && numberComparer.Equals(m.Op.PackNumber, plusOp.Op.PackNumber));

            if (matchingMinusIndex < 0) continue;

            plusOps[i] = plusOp with { IsMatched = true };
            minusOps[matchingMinusIndex] = new { minusOps[matchingMinusIndex].Op, IsMatched = true };
        }

        // Second pass: try to match remaining plus operations with any minus operation through any recharge
        for (var i = 0; i < plusOps.Count; i++)
        {
            if (plusOps[i].IsMatched) continue;

            var plusOp = plusOps[i];
            var foundMatch = false;

            // Try to find any recharge that can connect this plus to any minus
            for (var j = 0; j < rechargeOps.Count; j++)
            {
                if (rechargeOps[j].IsMatched) continue;

                // If recharge's PackNumber matches plus operation's PackNumber
                if (!numberComparer.Equals(rechargeOps[j].Op.PackNumber, plusOp.Op.PackNumber)) continue;

                // Look for any minus operation that can be connected through this recharge
                var matchingMinusIndex = minusOps.FindIndex(m => !m.IsMatched);

                if (matchingMinusIndex < 0) continue;

                // Found a match through recharge
                plusOps[i] = plusOp with { IsMatched = true };
                minusOps[matchingMinusIndex] = new { minusOps[matchingMinusIndex].Op, IsMatched = true };
                rechargeOps[j] = new { rechargeOps[j].Op, IsMatched = true };
                foundMatch = true;
                break;
            }

            // If no match found through any recharge, the sheet is not paired
            if (!foundMatch)
            {
                return false;
            }
        }

        // The sheet is paired if all plus operations are matched
        // There might be extra minus operations (which is allowed)
        return plusOps.All(x => x.IsMatched);
    }

    #endregion

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
    private static Task<int> SumQuantityForEmptySerialNums(KeyValuePair<UniqueUnitDto, List<ShortFormDTO>> pairWithLastOpDate, 
        string formNum)
    {
        var quantity = pairWithLastOpDate.Value
            .FirstOrDefault(x => x.OpCode == "10")
            ?.Quantity ?? 0; ;
        foreach (var formDto in pairWithLastOpDate.Value)
        {
            if (GetPlusOperationsArray(formNum).Contains(formDto.OpCode))
            {
                quantity += formDto.Quantity;
            }
            else if (GetMinusOperationsArray(formNum).Contains(formDto.OpCode))
            {
                quantity -= formDto.Quantity;
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
    /// <param name="formNum">Номер формы.</param>
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
    private protected static async Task<List<ShortFormDTO>> GetPlusMinusFormsDtoList(DBModel db, List<int> reportIds,
        string formNum, DateOnly firstSnkDate, DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto? snkParams = null)
    {
        var plusOperationArray = GetPlusOperationsArray(formNum);
        var minusOperationArray = GetMinusOperationsArray(formNum);

        var plusMinusOperationDtoList = formNum switch
        {
            #region 1.1

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

            #endregion

            #region 1.3

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
                    Quantity = 1,
                    Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams == null || snkParams.CheckType
                        ? form.Type_DB
                        : string.Empty
                })
                .ToListAsync(cts.Token),

            #endregion

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        var plusMinusOperationDtoListWithDateOnly = plusMinusOperationDtoList
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

        var summedPlusMinusOperationDtoList = await GetSummedPlusMinusDtoList(plusMinusOperationDtoListWithDateOnly, formNum);

        return summedPlusMinusOperationDtoList;
    }

    #region GetSummedInventoryDtoList

    /// <summary>
    /// Суммирует операции инвентаризации для первой даты по количеству и возвращает список DTO.
    /// </summary>
    /// <param name="plusMinusDtoList">Список DTO операций приёма передачи.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <returns>Список DTO операций инвентаризации, просуммированный по количеству для первой даты.</returns>
    private static Task<List<ShortFormDTO>> GetSummedPlusMinusDtoList(List<ShortFormDTO> plusMinusDtoList, string formNum)
    {
        List<ShortFormDTO> newPlusMinusDtoList = [];

        var comparer = new CustomSnkEqualityComparer();
        var radsComparer = new CustomSnkRadionuclidsEqualityComparer();
        foreach (var form in plusMinusDtoList)
        {
            var matchingForm = newPlusMinusDtoList.FirstOrDefault(x =>
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
                    if (GetPlusOperationsArray(formNum).Contains(form.OpCode))
                    {
                        matchingForm.Quantity += form.Quantity;
                    }
                    else if (GetMinusOperationsArray(formNum).Contains(form.OpCode))
                    {
                        matchingForm.Quantity -= form.Quantity;
                    }
                }
                else
                {
                    newPlusMinusDtoList.Add(form);
                }
            }
            else
            {
                newPlusMinusDtoList.Add(form);
            }
        }
        return Task.FromResult(newPlusMinusDtoList);
    }

    #endregion

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
            #region 1.1
            
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

            #endregion

            #region 1.3
            
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
                   Quantity = 1,
                   Radionuclids = snkParams == null || snkParams.CheckRadionuclids
                       ? form.Radionuclids_DB
                       : string.Empty,
                   Type = snkParams == null || snkParams.CheckType
                       ? form.Type_DB
                       : string.Empty
               })
               .ToListAsync(cts.Token), 
            
            #endregion

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
            #region 1.3 || SerialNumEmpty

            if (formNum is "1.3" || SerialNumbersIsEmpty(unit.PasNum, unit.FacNum))
            {
                var quantity = operations
                    .FirstOrDefault(x => x.OpCode == "10" && x.OpDate == firstInventoryDate)
                    ?.Quantity ?? 0;

                var inStockOnFirstInventoryDate = operations.Any(x => x.OpCode == "10" && x.OpDate == firstInventoryDate);
                var operationsWithoutDuplicates = await GetOperationsWithoutDuplicates(operations, formNum);
                
                foreach (var operation in operationsWithoutDuplicates)
                {
                    //Складываем количество, за исключением случая, если получение идёт в дату первичной инвентаризации.
                    if (plusOperationArray.Contains(operation.OpCode) && (operation.OpDate != firstInventoryDate || !inStockOnFirstInventoryDate))
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
                        .OrderBy(x => x.OpDate)
                        .LastOrDefault();

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

    #region GetOperationsWithoutDuplicates

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

    #endregion

    #region GetOperationsWithoutMutuallyCompensating

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

        var operationsGroupedByDate = operationList
            .OrderBy(x => x.OpDate)
            .Where(x =>
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
                    && ((currentFormIsMinus && plusOperationsArray.Contains(x.OpCode))
                        || (currentFormIsPlus && minusOperationsArray.Contains(x.OpCode))));

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

    #endregion

    #region SerialNumbersIsEmpty

    private protected static bool SerialNumbersIsEmpty(string? pasNum, string? facNum)
    {
        var num1 = (pasNum ?? string.Empty)
            .ToLower()
            .Replace(" ", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("/", "")
            .Replace("\\", "");
        num1 = DashesRegex().Replace(num1, "");

        var num2 = (facNum ?? string.Empty)
            .ToLower()
            .Replace(" ", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("/", "")
            .Replace("\\", "");
        num2 = DashesRegex().Replace(num2, "");
        
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
        return SpecialSymbolsRegex()
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

    #endregion

    #region DTO

    private protected abstract class SnkFormDTO(string facNum, string pasNum, int quantity, string radionuclids, string type, string activity,
        string creatorOKPO, string creationDate, string packNumber)
    {
        public readonly string PasNum = pasNum;

        public readonly string Type = type;

        public readonly string Radionuclids = radionuclids;

        public readonly string FacNum = facNum;

        public readonly int Quantity = quantity;

        public readonly string Activity = activity;

        public readonly string CreatorOKPO = creatorOKPO;

        public readonly string CreationDate = creationDate;

        public readonly string PackNumber = packNumber;
    }

    private protected class SnkForm11DTO(string facNum, string pasNum, int quantity, string radionuclids, string type, string activity,
        string creatorOKPO, string creationDate, short? category, float? signedServicePeriod, string packNumber)
        : SnkFormDTO(facNum, pasNum, quantity, radionuclids, type, activity, creatorOKPO, creationDate, packNumber)
    {
        public readonly short? Category = category;

        public readonly float? SignedServicePeriod = signedServicePeriod;
    }

    private protected class SnkForm13DTO(string facNum, string pasNum, int quantity, string radionuclids, string type, string activity,
        string creatorOKPO, string creationDate, short? aggregateState, string packNumber)
        : SnkFormDTO(facNum, pasNum, quantity, radionuclids, type, activity, creatorOKPO, creationDate, packNumber)
    {
        public readonly short? AggregateState = aggregateState;
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

    #region Regex

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    private static partial Regex DashesRegex();

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    private static partial Regex SpecialSymbolsRegex();

    #endregion
}