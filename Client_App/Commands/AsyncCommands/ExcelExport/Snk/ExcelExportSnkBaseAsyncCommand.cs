using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources.CustomComparers.SnkComparers;
using Client_App.ViewModels.Messages;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.Comparers.FormContent;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SnkRadionuclidsEqualityComparer = Client_App.Resources.CustomComparers.SnkComparers.SnkRadionuclidsEqualityComparer;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Базовый класс выгрузки СНК в Excel.
/// </summary>
public abstract partial class ExcelExportSnkBaseAsyncCommand : ExcelBaseAsyncCommand
{
    private sealed class SnkGroupKeyComparer : IEqualityComparer<(string PasNum, string FacNum, string Radionuclids, string Type)>
    {
        private readonly SnkNumberEqualityComparer _numberComparer = new();
        private readonly SnkRadionuclidsEqualityComparer _radsComparer = new();
        private readonly SnkNumberEqualityComparer _stringComparer = new();

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

    private sealed class SnkGroupKeyComparerWithPackNumber : IEqualityComparer<(string PasNum, string FacNum, string Radionuclids, string Type, string PackNumber)>
    {
        private readonly SnkNumberEqualityComparer _numberComparer = new();
        private readonly SnkRadionuclidsEqualityComparer _radsComparer = new();
        private readonly SnkNumberEqualityComparer _stringComparer = new();

        public bool Equals((string PasNum, string FacNum, string Radionuclids, string Type, string PackNumber) x,
            (string PasNum, string FacNum, string Radionuclids, string Type, string PackNumber) y)
        {
            return _numberComparer.Equals(x.PasNum, y.PasNum)
                   && _numberComparer.Equals(x.FacNum, y.FacNum)
                   && _radsComparer.Equals(x.Radionuclids, y.Radionuclids)
                   && _stringComparer.Equals(x.Type, y.Type)
                   && _stringComparer.Equals(x.PackNumber, y.PackNumber);
        }

        public int GetHashCode((string PasNum, string FacNum, string Radionuclids, string Type, string PackNumber) obj)
        {
            return HashCode.Combine(
                _numberComparer.GetHashCode(obj.PasNum),
                _numberComparer.GetHashCode(obj.FacNum),
                _radsComparer.GetHashCode(obj.Radionuclids),
                _stringComparer.GetHashCode(obj.Type),
                _stringComparer.GetHashCode(obj.PackNumber));

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

    #region MoveTracker

    // Вспомогательный класс для отслеживания перемещений
    private class MoveTracker
    {
        private readonly Dictionary<int, int> _moveCounts = [];
        private const int MaxMovesPerOperation = 10;

        public bool CanMove(ShortFormDTO form, int moveDistance)
        {
            if (moveDistance <= 0) return false;

            if (!_moveCounts.TryGetValue(form.Id, out var value))
            {
                _moveCounts[form.Id] = 1;
                return true;
            }

            _moveCounts[form.Id] = ++value;
            return value <= MaxMovesPerOperation;
        }
    }

    // Метод для безопасного перемещения операции
    private static bool TryMoveOperation(
        ShortFormDTO form,
        int currentIndex,
        List<ShortFormDTO> sourceList,
        List<ShortFormDTO> targetList,
        MoveTracker moveTracker,
        int newPosition,
        Action onMoveSuccess = null,
        Action onMoveFailure = null)
    {
        if (moveTracker.CanMove(form, Math.Abs(newPosition - currentIndex)))
        {
            sourceList.RemoveAt(currentIndex);

            if (newPosition >= sourceList.Count)
            {
                sourceList.Add(form);
            }
            else
            {
                sourceList.Insert(newPosition, form);
            }

            onMoveSuccess?.Invoke();
            return true;
        }

        else
        {
            sourceList.RemoveAt(currentIndex);
            onMoveFailure?.Invoke();
            return false;
        }
    }

    // Метод для безопасного перемещения в конец списка
    private static bool TryMoveToEnd(
        ShortFormDTO form,
        int currentIndex,
        List<ShortFormDTO> sourceList,
        List<ShortFormDTO> targetList,
        MoveTracker moveTracker,
        Action<ShortFormDTO> onStateUpdate)
    {
        return TryMoveOperation(
            form,
            currentIndex,
            sourceList,
            targetList,
            moveTracker,
            sourceList.Count,
            onMoveSuccess: () => { },
            onMoveFailure: () => onStateUpdate(form)
        );
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
                ExcelExportLostAndExtraUnitsByRegionAsyncCommand => "Проблемные источники по региону",
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
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentMessage = "Не удалось распознать введённую дату, " +
                                     $"{Environment.NewLine}выгрузка будет выполнена на текущую системную дату.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
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
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentMessage = "Выгрузка не выполнена, поскольку введена дата ранее вступления в силу приказа.",
                    MinWidth = 400,
                    MinHeight = 115,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
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
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбран ни один из параметров, для определения учётной единицы.",
                    MinWidth = 400,
                    MinHeight = 115,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
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
    /// <param name="selectedReports">Выбранная организация.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private protected static async Task CheckRepsAndRepPresence(string formNum, Reports? selectedReports, 
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        if (selectedReports is null)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
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
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Не удалось совершить выгрузку СНК," +
                                     $"{Environment.NewLine}поскольку у выбранной организации "
                                     + $"отсутствуют отчёты по форме {formNum}.",
                    MinWidth = 400,
                    MinHeight = 100,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
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
                            g => g.ToList())
                },
                snkGroupKeyComparer
            )
            .OrderBy(x => x.Key.PasNum)
            .ThenBy(x => x.Key.FacNum)
            .ToDictionary(x => x.Key, x => x.DateGroups);

        var comparer = new SnkNumberEqualityComparer();
        var numberComparer = new SnkNumberEqualityComparer();
        var radsComparer = new SnkRadionuclidsEqualityComparer();
        Dictionary<UniqueUnitDto, List<ShortFormDTO>> uniqueUnitWithAllOrderedOperationDictionary = [];
        var j = 0;
        foreach (var (unit, formsByDateDictionary) in groupedOperationListDictionary)
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

            var isFirstDateForUnit = true;

            foreach (var (date, formsList) in formsByDateDictionary)
            {
                List<ShortFormDTO> newOperationOrderList = [];

                var editedFormsList = formsList.ToList();

                //Если есть операции инвентаризации
                if (formsList.Any(x => x.OpCode is "10"))
                {
                    //Если это первая операция с учётной единицей, то операции инвентаризации идут в начале
                    if (isFirstDateForUnit)
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

                var moveTracker = new MoveTracker();
                var reorderGuard = 0;
                var reorderGuardLimit = Math.Max(editedFormsList.Count * (editedFormsList.Count + 1), 1);

                for (var i = 0; i < editedFormsList.Count && reorderGuard++ < reorderGuardLimit; i++)
                {
                    var form = editedFormsList[i];

                    var subsequentElementsList = editedFormsList
                        .Where((_, index) => index > i)
                        .ToList();

                    #region OneOperationPerDay

                    //Если в этот день только одна операция, то добавляем без изменений и переходим к следующему дню.
                    if (editedFormsList.Count is 1)
                    {
                        AddOperation(form, newOperationOrderList, formNum, ref inStock, out currentPackNumber);
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

                        if (newOperationOrderList.Count == 0 && hasInventory 
                                ? form.OpDate == firstInventoryDate
                                : !inStock 
                                  && subsequentElementsList.Any(x => 
                                      x.OpCode is "10" || GetPlusOperationsArray(formNum).Contains(x.OpCode)))
                        {
                            var countInventoryAndPlusOperation = subsequentElementsList.Count(x => 
                                x.OpCode is "10" || GetPlusOperationsArray(formNum).Contains(x.OpCode));

                            if (TryMoveOperation(form, i, editedFormsList, newOperationOrderList,
                                    moveTracker, i + countInventoryAndPlusOperation,
                                    onMoveFailure: () =>
                                        AddOperation(form, newOperationOrderList, formNum, ref inStock, out currentPackNumber)))
                            {
                                i--;
                            }
                        }

                        //Если в этот день есть необработанные операции не перезарядки с текущим номером упаковки,
                        //то помещаем операцию перезарядки после этих операций
                        else if (subsequentElementsList
                                     .Any(x => 
                                         (GetMinusOperationsArray(formNum).Contains(x.OpCode) 
                                          && numberComparer.Equals(x.PackNumber, currentPackNumber)) 
                                         || (!inStock && GetPlusOperationsArray(formNum).Contains(x.OpCode) 
                                                      && !numberComparer.Equals(x.PackNumber, currentPackNumber))) 
                                 && !isPaired)
                        {
                            var countOperationWithSamePackNumber = subsequentElementsList
                                .Count(x => 
                                    (GetMinusOperationsArray(formNum).Contains(x.OpCode) 
                                     && numberComparer.Equals(x.PackNumber, currentPackNumber)) 
                                    || (!inStock && GetPlusOperationsArray(formNum).Contains(x.OpCode) 
                                                 && !numberComparer.Equals(x.PackNumber, currentPackNumber)));

                            if (TryMoveOperation(form, i, editedFormsList, newOperationOrderList,
                                    moveTracker, i + countOperationWithSamePackNumber,
                                    onMoveFailure: () =>
                                        AddOperation(form, newOperationOrderList, formNum, ref inStock, out currentPackNumber)))
                            {
                                i--;
                            }
                        }
                        else
                        {
                            AddOperation(form, newOperationOrderList, formNum, ref inStock, out currentPackNumber);
                        }
                    }

                    #endregion

                    #region Plus
                    
                    //Если операция получения
                    else if (GetPlusOperationsArray(formNum).Contains(form.OpCode))
                    {
                        //Если нет в наличии или (нет других операций с тем же номером упаковки или операций перезарядки)
                        if (!inStock || subsequentElementsList.All(x => 
                                GetPlusOperationsArray(formNum).Contains(x.OpCode)))
                        {
                            AddOperation(form, newOperationOrderList, formNum, ref inStock, out currentPackNumber);
                        }
                        //Перемещаем эту операцию получения в конец списка
                        else
                        {
                            if (TryMoveToEnd(form, i, editedFormsList, newOperationOrderList, moveTracker,
                                    (f) =>
                                    {
                                        AddOperation(f, newOperationOrderList, formNum, ref inStock, out currentPackNumber);
                                    }))
                            {
                                i--;
                            }
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
                                GetMinusOperationsArray(formNum).Contains(x.OpCode)))
                        {
                            AddOperation(form, newOperationOrderList, formNum, ref inStock, out currentPackNumber);
                        }
                        //Перемещаем эту операцию передачи в конец списка
                        else
                        {
                            if (TryMoveToEnd(form, i, editedFormsList, newOperationOrderList, moveTracker,
                                    (f) =>
                                    {
                                        AddOperation(f, newOperationOrderList, formNum, ref inStock, out currentPackNumber);
                                    }))
                            {
                                i--;
                            }
                        }
                    }

                    #endregion

                    #region Zero

                    //Если нулевая операция
                    else
                    {
                        AddOperation(form, newOperationOrderList, formNum, ref inStock, out currentPackNumber);
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

                isFirstDateForUnit = false;
            }
        }

        var orderedOperationList = uniqueUnitWithAllOrderedOperationDictionary
            .Select(x => x.Value);

        //var groupedOperationList = await GetGroupedOperationList(orderedOperationList);

        Dictionary<UniqueUnitDto, List<ShortFormDTO>> uniqueUnitWithAllOperationDictionary = [];
        foreach (var group in orderedOperationList)
        {
            if (group.Count == 0) continue;

            // Для серийной единицы (1.1 с непустыми зав.№) УКТ — полноценный идентификатор,
            // но он может меняться перезарядкой (53/54), причём в строке указывается только новый
            // УКТ. Поэтому разносим операции серийника по экземплярам так, что переход между УКТ
            // допускается ТОЛЬКО через перезарядку. Это разделяет параллельные единицы с одинаковым
            // серийником, но разным УКТ, и одновременно не рвёт цепочку перезарядок одной единицы.
            // Алгоритм порядко-независим относительно того, идёт ли op.10 раньше связывающей перезарядки.
            var sampleForm = group[0];
            if (formNum is not "1.3" && !SerialNumbersIsEmpty(sampleForm.PasNum, sampleForm.FacNum))
            {
                foreach (var (instanceKey, instanceOps) in SplitSerialGroupIntoInstances(group, formNum))
                {
                    uniqueUnitWithAllOperationDictionary.Add(instanceKey, instanceOps);
                }
                continue;
            }

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

    /// <summary>
    /// Экземпляр серийной учётной единицы при разнесении операций по УКТ.
    /// </summary>
    private sealed class SerialUnitInstance
    {
        public string CurrentPackNumber { get; set; } = "";
        public List<(int Index, ShortFormDTO Form)> Operations { get; } = [];
    }

    /// <summary>
    /// Разносит операции одного серийника (1.1 с непустыми зав.№) по отдельным учётным единицам.
    /// Переход между УКТ допускается только через операцию перезарядки (53/54): обычная операция
    /// (приём/передача/инвентаризация/нулевая) присоединяется к экземпляру с тем же УКТ, а если
    /// такого нет — образует новый экземпляр (параллельная единица). Перезарядка меняет УКТ
    /// последнего активного экземпляра с иным УКТ.
    /// <para>
    /// После разнесения экземпляры с одинаковым итоговым УКТ объединяются: две записи с совпадающими
    /// серийником и УКТ — это одна и та же учётная единица (все идентифицирующие параметры равны).
    /// Это «склеивает» цепочку перезарядки, если op.10 с новым УКТ оказался в списке раньше
    /// связывающей перезарядки, но НЕ объединяет параллельные единицы с разным итоговым УКТ.
    /// </para>
    /// </summary>
    /// <param name="serialOperations">Операции одного серийника (упорядоченные в проходе 1).</param>
    /// <param name="formNum">Номер формы.</param>
    /// <returns>Пары «ключ учётной единицы — список её операций» (порядок операций сохранён).</returns>
    private static List<(UniqueUnitDto Key, List<ShortFormDTO> Operations)> SplitSerialGroupIntoInstances(
        List<ShortFormDTO> serialOperations, string formNum)
    {
        var numberComparer = new SnkNumberEqualityComparer();
        var plusOperationsArray = GetPlusOperationsArray(formNum);
        var minusOperationsArray = GetMinusOperationsArray(formNum);
        List<SerialUnitInstance> instances = [];

        // Назначение операций по экземплярам делаем порядко-независимым: внутри одной даты
        // обрабатываем операции в канонической последовательности «приём → перезарядка → передача →
        // инвентаризация». Так приём успевает создать экземпляр до связывающей перезарядки, даже если
        // в исходных данных строки идут в произвольном порядке. Исходный порядок (для хранимого
        // списка операций) сохраняется через индекс и восстанавливается ниже.
        int CategoryPriority(ShortFormDTO f) =>
            f.OpCode switch
            {
                "53" or "54" => 1,
                "10" => 3,
                _ when plusOperationsArray.Contains(f.OpCode) => 0,
                _ when minusOperationsArray.Contains(f.OpCode) => 2,
                _ => 4
            };

        var assignmentOrder = serialOperations
            .Select((form, index) => (form, index))
            .OrderBy(x => x.form.OpDate)
            .ThenBy(x => CategoryPriority(x.form))
            .ThenBy(x => x.index)
            .ToList();

        foreach (var (form, index) in assignmentOrder)
        {
            if (form.OpCode is "53" or "54")
            {
                // Перезарядка переводит экземпляр на новый УКТ. Применяем к последнему экземпляру
                // с иным УКТ; если такого нет — к экземпляру с тем же УКТ; иначе создаём новый.
                var target = instances.LastOrDefault(x => !numberComparer.Equals(x.CurrentPackNumber, form.PackNumber))
                             ?? instances.LastOrDefault(x => numberComparer.Equals(x.CurrentPackNumber, form.PackNumber));

                if (target is null)
                {
                    var created = new SerialUnitInstance { CurrentPackNumber = form.PackNumber };
                    created.Operations.Add((index, form));
                    instances.Add(created);
                }
                else
                {
                    target.Operations.Add((index, form));
                    target.CurrentPackNumber = form.PackNumber;
                }
            }
            else
            {
                // Обычная операция присоединяется к экземпляру с совпадающим УКТ; если такого нет —
                // это другая параллельная единица (тот же серийник, другой УКТ).
                var target = instances.LastOrDefault(x => numberComparer.Equals(x.CurrentPackNumber, form.PackNumber));

                if (target is null)
                {
                    var created = new SerialUnitInstance { CurrentPackNumber = form.PackNumber };
                    created.Operations.Add((index, form));
                    instances.Add(created);
                }
                else
                {
                    target.Operations.Add((index, form));
                }
            }
        }

        // Объединяем экземпляры с одинаковым итоговым УКТ (одна и та же учётная единица).
        List<(UniqueUnitDto, List<ShortFormDTO>)> result = [];
        foreach (var samePackGroup in instances.GroupBy(x => x.CurrentPackNumber, numberComparer))
        {
            var mergedOperations = samePackGroup
                .SelectMany(x => x.Operations)
                .OrderBy(x => x.Index)
                .Select(x => x.Form)
                .ToList();

            var representative = SelectStockRepresentativeOperation(mergedOperations, formNum)
                                 ?? mergedOperations[^1];

            var key = new UniqueUnitDto(
                representative.FacNum, representative.PasNum, representative.Radionuclids,
                representative.Type, representative.Quantity, samePackGroup.Key);

            result.Add((key, mergedOperations));
        }

        return result;
    }

    // Вспомогательный метод для добавления операции
    private static void AddOperation(ShortFormDTO form, List<ShortFormDTO> newOperationOrderList, string formNum,
        ref bool inStock, out string currentPackNumber)
    {
        newOperationOrderList.Add(form);
        currentPackNumber = form.PackNumber;

        if (GetPlusOperationsArray(formNum).Contains(form.OpCode))
            inStock = true;
        if (GetMinusOperationsArray(formNum).Contains(form.OpCode))
            inStock = false;
    }

    private static bool IsPairedList(List<ShortFormDTO> editedFormsList, bool inStock, string currentPackNumber, string formNum)
    {
        var numberComparer = new SnkNumberEqualityComparer();
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
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.</param>
    /// <returns>Список DTO операций инвентаризации, отсортированный по датам, с фильтром по дате от 01.01.2022 до введённой пользователем даты.</returns>
    private protected static async Task<(DateOnly, List<ShortFormDTO>, List<ShortFormDTO>)> GetInventoryFormsDtoList(DBModel db, 
        List<ShortReportDTO> inventoryReportDtoList, string formNum, DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto snkParams)
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
                                PasNum = snkParams.CheckPasNum
                                    ? form11.PassportNumber_DB
                                    : string.Empty,
                                Type = snkParams.CheckType
                                    ? form11.Type_DB
                                    : string.Empty,
                                Radionuclids = snkParams.CheckRadionuclids
                                    ? form11.Radionuclids_DB
                                    : string.Empty,
                                FacNum = snkParams.CheckFacNum
                                    ? form11.FactoryNumber_DB
                                    : string.Empty,
                                Quantity = form11.Quantity_DB,
                                PackNumber = snkParams.CheckPackNumber
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
                                PasNum = snkParams.CheckPasNum
                                    ? form13.PassportNumber_DB
                                    : string.Empty,
                                Type = snkParams.CheckType
                                    ? form13.Type_DB
                                    : string.Empty,
                                Radionuclids = snkParams.CheckRadionuclids
                                    ? form13.Radionuclids_DB
                                    : string.Empty,
                                FacNum = snkParams.CheckFacNum
                                    ? form13.FactoryNumber_DB
                                    : string.Empty,
                                PackNumber = snkParams.CheckPackNumber
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
    private protected static Task<(List<ShortFormDTO>, List<ShortFormDTO>)> GetSummedInventoryDtoList(List<ShortFormDTO> inventoryFormsDtoList, string formNum)
    {
        List<ShortFormDTO> newInventoryFormsDtoList = [];
        List<ShortFormDTO> inventoryDuplicateErrors = [];

        var comparer = new SnkNumberEqualityComparer();
        var radsComparer = new SnkRadionuclidsEqualityComparer();
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
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.</param>
    /// <returns>Список DTO форм с операциями приёма передачи, отсортированный по датам.</returns>
    private protected static async Task<List<ShortFormDTO>> GetPlusMinusFormsDtoList(DBModel db, List<int> reportIds,
        string formNum, DateOnly firstSnkDate, DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto snkParams)
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
                    FacNum = snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Quantity = form.Quantity_DB,
                    Radionuclids = snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams.CheckType
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
                    FacNum = snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Quantity = 1,
                    Radionuclids = snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams.CheckType
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

    #region GetSummedPlusMinusDtoList

    /// <summary>
    /// Суммирует операции приёма-передачи по количеству и возвращает список DTO.
    /// </summary>
    /// <param name="plusMinusDtoList">Список DTO операций приёма передачи.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <returns>Список DTO операций приёма-передачи, просуммированный по количеству для первой даты.</returns>
    private static Task<List<ShortFormDTO>> GetSummedPlusMinusDtoList(List<ShortFormDTO> plusMinusDtoList,
        string formNum)
    {
        List<ShortFormDTO> newPlusMinusDtoList = [];

        var snkGroupKeyComparer = new SnkGroupKeyComparerWithPackNumber();

        var groupedOperationListDictionary = plusMinusDtoList
            .OrderBy(x => x.OpDate)
            .ThenBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ThenBy(x => x.NumberInOrder)
            .GroupBy(
                x => (x.PasNum, x.FacNum, x.Radionuclids, x.Type, x.PackNumber),
                (key, items) => new
                {
                    Key = key,
                    DateGroups = items
                        .GroupBy(x => x.OpDate)
                        .ToDictionary(
                            g => g.Key,
                            g => g.ToList())
                },
                snkGroupKeyComparer
            )
            .OrderBy(x => x.Key.PasNum)
            .ThenBy(x => x.Key.FacNum)
            .ToDictionary(x => x.Key, x => x.DateGroups);

        foreach (var (unit, dictionary) in groupedOperationListDictionary) //по учётной единице
        {
            foreach (var (_, operations) in dictionary) //операции за каждую дату 
            {
                var quantity = 0;

                switch (formNum)
                {
                    case "1.1" when SerialNumbersIsEmpty(unit.PasNum, unit.FacNum):
                    case "1.3":
                    {
                        foreach (var operation in operations)
                        {
                            if (GetPlusOperationsArray(formNum).Contains(operation.OpCode))
                            {
                                quantity += operation.Quantity;
                            }
                            else if (GetMinusOperationsArray(formNum).Contains(operation.OpCode))
                            {
                                quantity -= operation.Quantity;
                            }
                        }

                        switch (quantity)
                        {
                            case < 0:
                            {
                                quantity = Math.Abs(quantity);
                                var lastMinusOperation = operations.Last(x => 
                                    GetMinusOperationsArray(formNum).Contains(x.OpCode));
                                lastMinusOperation.Quantity = quantity;
                                newPlusMinusDtoList.Add(lastMinusOperation);
                                break;
                            }
                            case > 0:
                            {
                                var lastPlusOperation = operations.Last(x => 
                                    GetPlusOperationsArray(formNum).Contains(x.OpCode));
                                lastPlusOperation.Quantity = quantity;
                                newPlusMinusDtoList.Add(lastPlusOperation);
                                break;
                            }
                            default: continue;
                        }

                        break;
                    }
                    case "1.1" when !SerialNumbersIsEmpty(unit.PasNum, unit.FacNum):
                    {
                        foreach (var operation in operations)
                        {
                            if (GetPlusOperationsArray(formNum).Contains(operation.OpCode))
                            {
                                quantity += 1;
                            }
                            else if (GetMinusOperationsArray(formNum).Contains(operation.OpCode))
                            {
                                quantity -= 1;
                            }

                            switch (quantity)
                            {
                                case < 0:
                                {
                                    var lastMinusOperation = operations.Last(x =>
                                        GetMinusOperationsArray(formNum).Contains(x.OpCode));
                                    newPlusMinusDtoList.Add(lastMinusOperation);
                                    break;
                                }
                                case > 0:
                                {
                                    var lastPlusOperation = operations.Last(x => 
                                        GetPlusOperationsArray(formNum).Contains(x.OpCode));
                                    newPlusMinusDtoList.Add(lastPlusOperation);
                                    break;
                                }
                                case 0:
                                {
                                    if (newPlusMinusDtoList.Count > 0)
                                    {
                                        newPlusMinusDtoList.RemoveAt(newPlusMinusDtoList.Count - 1);
                                    }
                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
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
    /// <param name="snkParams">DTO состоящий из bool флагов, показывающих, по каким параметрам необходимо выполнять поиск учётной единицы.</param>
    /// <returns>Список DTO форм с операциями перезарядки, отсортированный по датам.</returns>
    private protected static async Task<List<ShortFormDTO>> GetRechargeFormsDtoList(DBModel db, int repsId, string formNum, 
        DateOnly firstSnkDate, DateOnly endSnkDate, CancellationTokenSource cts, SnkParamsDto snkParams)
    {
        if (!snkParams.CheckPackNumber) return [];

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
                    FacNum = snkParams.CheckFacNum
                        ? form.FactoryNumber_DB
                        : string.Empty,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PackNumber = snkParams.CheckPackNumber
                        ? form.PackNumber_DB
                        : string.Empty,
                    PasNum = snkParams.CheckPasNum
                        ? form.PassportNumber_DB
                        : string.Empty,
                    Quantity = form.Quantity_DB,
                    Radionuclids = snkParams.CheckRadionuclids
                        ? form.Radionuclids_DB
                        : string.Empty,
                    Type = snkParams.CheckType
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
                   FacNum = snkParams.CheckFacNum
                       ? form.FactoryNumber_DB
                       : string.Empty,
                   OpCode = form.OperationCode_DB,
                   OpDate = form.OperationDate_DB,
                   PackNumber = snkParams.CheckPackNumber
                       ? form.PackNumber_DB
                       : string.Empty,
                   PasNum = snkParams.CheckPasNum
                       ? form.PassportNumber_DB
                       : string.Empty,
                   Quantity = 1,
                   Radionuclids = snkParams.CheckRadionuclids
                       ? form.Radionuclids_DB
                       : string.Empty,
                   Type = snkParams.CheckType
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
    /// Выбирает представительную операцию для строки СНК: при нескольких операциях в последний день
    /// предпочитает последнюю не минусовую (op.10 не учитывается при выборе).
    /// </summary>
    private protected static ShortFormDTO? SelectStockRepresentativeOperation(
        IReadOnlyList<ShortFormDTO> operations, string formNum)
    {
        if (operations.Count == 0)
        {
            return null;
        }

        var minusOperationArray = GetMinusOperationsArray(formNum);
        var ordered = operations.OrderBy(x => x.OpDate).ToList();
        var lastDate = ordered[^1].OpDate;
        var onLastDate = ordered
            .Where(x => x.OpDate == lastDate && x.OpCode != "10")
            .ToList();

        if (onLastDate.Count > 1)
        {
            return onLastDate
                .Where(x => !minusOperationArray.Contains(x.OpCode))
                .OrderByDescending(x => x.RepDto.StartPeriod)
                .ThenByDescending(x => x.NumberInOrder)
                .FirstOrDefault()
                   ?? ordered.LastOrDefault();
        }

        return ordered.LastOrDefault();
    }

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
        var unitInStockList = await ComputeStockAsOfDate(
            uniqueUnitWithAllOperationDictionary, formNum, firstInventoryDate, DateOnly.MaxValue);

        progressBarVM.SetProgressBar(
            (int)Math.Floor((double)progressBarVM.ValueBar + 10),
            $"Проверено {unitInStockList.Count} единиц",
            "Проверка наличия");

        return unitInStockList;
    }

    /// <summary>
    /// Расчёт СНК (наличия) на указанную дату по словарю операций. Общий для выгрузки СНК
    /// и проверки инвентаризаций, чтобы обе функции давали идентичный результат.
    /// </summary>
    /// <param name="uniqueUnitWithAllOperationDictionary">Словарь из уникальной учётной единицы и списка всех операций с ней.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="firstInventoryDate">Дата первой инвентаризации.</param>
    /// <param name="asOfDate">Дата, на которую формируется наличие (учитываются операции с OpDate ≤ asOfDate).</param>
    /// <returns>Список DTO учётных единиц в наличии (СНК) на дату.</returns>
    private protected static async Task<List<ShortFormDTO>> ComputeStockAsOfDate(
        Dictionary<UniqueUnitDto, List<ShortFormDTO>> uniqueUnitWithAllOperationDictionary, string formNum,
        DateOnly firstInventoryDate, DateOnly asOfDate)
    {
        var plusOperationArray = GetPlusOperationsArray(formNum);
        var minusOperationArray = GetMinusOperationsArray(formNum);

        List<ShortFormDTO> unitInStockList = [];
        var comparer = new SnkNumberEqualityComparer();
        var radsComparer = new SnkRadionuclidsEqualityComparer();
        foreach (var (unit, allUnitOperations) in uniqueUnitWithAllOperationDictionary)
        {
            var operations = allUnitOperations
                .Where(x => x.OpDate <= asOfDate)
                .ToList();

            if (operations.Count == 0) continue;

            #region 1.3 || (1.1 && SerialNumEmpty)

            if (formNum is "1.3" || SerialNumbersIsEmpty(unit.PasNum, unit.FacNum))
            {
                // Для 1.3 и для 1.1 с пустыми зав./паспорт на первую дату инвентаризации
                // количество должно задаваться суммой всех строк op.10 на эту дату.
                // Иначе сценарии "N строк по 1" и "1 строка с N" дают разный результат.
                var quantity = operations
                    .Where(x => x.OpCode == "10" && x.OpDate == firstInventoryDate)
                    .Sum(x => x.Quantity);

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
                    var stockUnit = lastOperationWithUnit.Clone();
                    stockUnit.Quantity = quantity;
                    unitInStockList.Add(stockUnit);
                }
            }

            #endregion

            #region 1.1 && SerialNumNotEmpty
            
            else
            {
                // Засев «открывающего» наличия: единица учтена в первой инвентаризации (op.10 в дату
                // первой инвентаризации). Но если в эту же дату есть приходная операция, единица не была
                // в наличии ДО первой инвентаризации — она поступила в этот день, и её наличие полностью
                // определяется её +/- операциями (иначе взаимокомпенсация прихода с передачей скрыла бы
                // передачу, и единица ошибочно осталась бы в наличии).
                var inStock = operations.Any(x => x.OpCode == "10" && x.OpDate == firstInventoryDate)
                              && !operations.Any(x => plusOperationArray.Contains(x.OpCode) && x.OpDate == firstInventoryDate);

                var currentOperationsWithoutMutuallyExclusive = await GetOperationsWithoutMutuallyCompensating(operations, formNum);
                foreach (var form in currentOperationsWithoutMutuallyExclusive)
                {
                    if (plusOperationArray.Contains(form.OpCode)) inStock = true;
                    else if (minusOperationArray.Contains(form.OpCode)) inStock = false;
                }
                if (inStock)
                {
                    var lastOperationWithUnit = SelectStockRepresentativeOperation(
                        currentOperationsWithoutMutuallyExclusive, formNum);

                    if (lastOperationWithUnit != null)
                    {
                        // УКТ в строке СНК должен соответствовать состоянию на asOfDate: берём УКТ из
                        // последней перезарядки (53/54) с датой ≤ asOfDate. Это нужно, когда в последний
                        // день несколько операций и представителем выбирается приём со старым УКТ (52),
                        // хотя в тот же день была перезарядка (52-1). Если перезарядок ещё не было —
                        // оставляем УКТ выбранной операции (исходный УКТ до перезарядки).
                        var stockUnit = lastOperationWithUnit.Clone();
                        var currentPackNumber = operations
                            .Where(x => x.OpCode is "53" or "54")
                            .OrderByDescending(x => x.OpDate)
                            .ThenByDescending(x => x.RepDto.StartPeriod)
                            .ThenByDescending(x => x.NumberInOrder)
                            .FirstOrDefault()?.PackNumber;

                        if (currentPackNumber != null)
                        {
                            stockUnit.PackNumber = currentPackNumber;
                        }

                        unitInStockList.Add(stockUnit);
                    }
                }

            }

            #endregion
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
    private protected static Task<List<ShortFormDTO>> GetOperationsWithoutDuplicates(List<ShortFormDTO> operationList, string formNum)
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
                        var lastOp = group.Last(x => plusOperationsArray.Contains(x.OpCode)).Clone();
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
                        var lastOp = group.Last(x => minusOperationsArray.Contains(x.OpCode)).Clone();
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

        List<ShortFormDTO> operationWithoutMutuallyExclusive = [];
        foreach (var group in operationList
                     .Select((form, index) => new { form, index })
                     .GroupBy(x => x.form.OpDate)
                     .OrderBy(x => x.Key))
        {
            var orderedForms = group.OrderBy(x => x.index).Select(x => x.form).ToList();
            var pendingNet = 0;
            ShortFormDTO? pendingPlusTemplate = null;
            ShortFormDTO? pendingMinusTemplate = null;

            void FlushPendingNet()
            {
                switch (pendingNet)
                {
                    case > 0 when pendingPlusTemplate is not null:
                        {
                            var plus = pendingPlusTemplate.Clone();
                            plus.Quantity = pendingNet;
                            operationWithoutMutuallyExclusive.Add(plus);
                            break;
                        }
                    case < 0 when pendingMinusTemplate is not null:
                        {
                            var minus = pendingMinusTemplate.Clone();
                            minus.Quantity = int.Abs(pendingNet);
                            operationWithoutMutuallyExclusive.Add(minus);
                            break;
                        }
                }

                pendingNet = 0;
                pendingPlusTemplate = null;
                pendingMinusTemplate = null;
            }

            foreach (var form in orderedForms)
            {
                if (plusOperationsArray.Contains(form.OpCode))
                {
                    pendingNet += form.Quantity;
                    pendingPlusTemplate = form;
                }
                else if (minusOperationsArray.Contains(form.OpCode))
                {
                    pendingNet -= form.Quantity;
                    pendingMinusTemplate = form;
                }
                else
                {
                    FlushPendingNet();
                    operationWithoutMutuallyExclusive.Add(form);
                }
            }

            FlushPendingNet();
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
            "нд",
            "нетданных",
            AutoReplaceSimilarChars("бн"),
            AutoReplaceSimilarChars("без номера"),
            AutoReplaceSimilarChars("нд"),
            AutoReplaceSimilarChars("нет данных"),
            AutoReplaceSimilarChars("прим"),
            AutoReplaceSimilarChars("примечание"),
        ];
        return validStrings.Contains(num1) && validStrings.Contains(num2);
    }

    #region AutoReplaceSimilarChars

    private static string AutoReplaceSimilarChars(string? str)
    {
        var cleaned = SpecialSymbolsRegex()
            .Replace(str ?? string.Empty, "")
            .ToLower();
        return LookalikeCharMapper.ReplaceRuEnLookalikes(cleaned);
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

    public class ShortFormDTO
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

        public Status Status { get; set; }

        /// <summary>
        /// Создаёт поверхностную копию DTO. Используется в расчётах СНК, чтобы агрегация количества
        /// (свёртка +/- операций за день) не мутировала исходные объекты в общем словаре операций:
        /// <see cref="ComputeStockAsOfDate"/> вызывается многократно для разных дат по одному словарю.
        /// </summary>
        public ShortFormDTO Clone() => new()
        {
            Id = Id,
            NumberInOrder = NumberInOrder,
            RepDto = RepDto,
            OpCode = OpCode,
            OpDate = OpDate,
            PasNum = PasNum,
            Type = Type,
            Radionuclids = Radionuclids,
            FacNum = FacNum,
            Quantity = Quantity,
            PackNumber = PackNumber,
            Status = Status
        };
    }

    public enum Status
    {
        None = 0,
        Lost,
        Extra
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

    public class ShortReportDTO(int id, DateOnly startPeriod, DateOnly endPeriod)
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