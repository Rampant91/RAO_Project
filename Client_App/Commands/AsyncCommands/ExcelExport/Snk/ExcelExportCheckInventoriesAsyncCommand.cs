using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.IO;
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
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Excel -> Проверка инвентаризаций.
/// </summary>
public class ExcelExportCheckInventoriesAsyncCommand : ExcelExportSnkBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var mainWindow = Desktop.MainWindow as MainWindow;
        var selectedReports = mainWindow!.SelectedReports.First() as Reports;

        var formNum = (parameter as string)!;
        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"СНК_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(5, "Проверка наличия отчётов",
            $"Выгрузка СНК {formNum} {regNum}_{okpo}", ExportType);
        await CheckRepsAndRepPresence(selectedReports, formNum, progressBar, cts);

        progressBarVM.SetProgressBar(6, "Запрос даты формирования СНК");
        var(endSnkDate, snkParams) = await AskSnkEndDate(progressBar, cts);

        progressBarVM.SetProgressBar(8, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(13, "Формирование списка инвентаризационных отчётов");
        var inventoryReportDtoList = await GetInventoryReportDtoList(db, selectedReports.Id, endSnkDate, cts);

        progressBarVM.SetProgressBar(14, "Проверка наличия инвентаризации");
        await CheckInventoryFormPresence(inventoryReportDtoList, formNum, progressBar, cts);

        progressBarVM.SetProgressBar(15, "Формирование списка операций инвентаризации");
        var (firstSnkDate, inventoryFormsDtoList) = await GetInventoryFormsDtoList(db, inventoryReportDtoList, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(16, "Получение списка дат инвентаризаций");
        var inventoryDatesList = await GetInventoryDatesList(inventoryFormsDtoList);

        progressBarVM.SetProgressBar(17, "Формирование периодов инвентаризаций");
        var inventoryDatesTupleList = await GetInventoryDatesTupleList(inventoryDatesList);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(19, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, inventoryDatesList);

        progressBarVM.SetProgressBar(20, "Формирование списка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, selectedReports.Id, firstSnkDate, endSnkDate, cts, snkParams);

        progressBarVM.SetProgressBar(22, "Формирование списка всех операций");
        var unionFormsDtoList = await GetUnionFormsDtoList(inventoryFormsDtoList, plusMinusFormsDtoList);

        progressBarVM.SetProgressBar(24, "Формирование списка уникальных учётных единиц");
        var uniqueAccountingUnitDtoList = await GetUniqueAccountingUnitDtoList(unionFormsDtoList);

        var forms11DtoList = await GetForms11DtoList(inventoryFormsDtoList, plusMinusFormsDtoList, uniqueAccountingUnitDtoList,
            inventoryDatesList, inventoryDatesTupleList, cts);

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

    #region CheckInventoryFormPresence

    /// <summary>
    /// Проверяет наличие у организации хотя бы одного отчёта, содержащего форму с кодом операции 10.
    /// При отсутствии таковой выводит сообщение и завершает выполнение команды.
    /// </summary>
    /// <param name="inventoryReportDtoList">Список DTO отчётов по форме 1.1, отсортированный по датам.</param>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckInventoryFormPresence(List<ShortReportDTO> inventoryReportDtoList, string formNum,
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        if (inventoryReportDtoList.Count == 0)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = $"Выгрузка не выполнена, поскольку у организации отсутствуют формы {formNum} с кодом операции 10.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion

    #region CheckRepsAndRepPresence

    /// <summary>
    /// Проверяет наличие выбранной организации, в случае если запущена команда для неё.
    /// Проверяет наличие хотя бы одного отчёта, с выбранным номером формы. В случае отсутствия выводит соответствующее сообщение и закрывает команду.
    /// </summary>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="selectedReports">Выбранная организация.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckRepsAndRepPresence(Reports? selectedReports, string formNum,
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
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
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполняет заголовки Excel пакета.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <param name="inventoryDatesList">Список дат инвентаризации вместе с текущей датой/датой введённой пользователем.</param>
    private async Task FillExcelHeaders(ExcelPackage excelPackage, List<DateOnly> inventoryDatesList)
    {
        //ws.Cells["A1:C1"].Merge = true;

        foreach (var date in inventoryDatesList)
        {
            Worksheet = excelPackage.Workbook.Worksheets.Add($"СНК на {date.ToShortDateString()}");

            Worksheet.Cells[1, 1, 1, 11].Merge = true;
            Worksheet.Cells[1, 1].Value = $"СНК на {date.ToShortDateString()}";

            Worksheet.Cells[2, 1].Value = "№ п/п";
            Worksheet.Cells[2, 2].Value = "Номер паспорта (сертификата)";
            Worksheet.Cells[2, 3].Value = "тип";
            Worksheet.Cells[2, 4].Value = "радионуклиды";
            Worksheet.Cells[2, 5].Value = "номер";
            Worksheet.Cells[2, 6].Value = "количество, шт.";
            Worksheet.Cells[2, 7].Value = "суммарная активность, Бк";
            Worksheet.Cells[2, 8].Value = "код ОКПО изготовителя";
            Worksheet.Cells[2, 9].Value = "дата выпуска";
            Worksheet.Cells[2, 10].Value = "категория";
            Worksheet.Cells[2, 11].Value = "НСС, мес";

            Worksheet.Cells[1, 12, 1, 22].Merge = true;
            Worksheet.Cells[1, 12].Value = $"Инвентаризация на {date.ToShortDateString()}";

            Worksheet.Cells[2, 12].Value = "№ п/п";
            Worksheet.Cells[2, 13].Value = "Номер паспорта (сертификата)";
            Worksheet.Cells[2, 14].Value = "тип";
            Worksheet.Cells[2, 15].Value = "радионуклиды";
            Worksheet.Cells[2, 16].Value = "номер";
            Worksheet.Cells[2, 17].Value = "количество, шт.";
            Worksheet.Cells[2, 18].Value = "суммарная активность, Бк";
            Worksheet.Cells[2, 19].Value = "код ОКПО изготовителя";
            Worksheet.Cells[2, 20].Value = "дата выпуска";
            Worksheet.Cells[2, 21].Value = "категория";
            Worksheet.Cells[2, 22].Value = "НСС, мес";

            await AutoFitColumns();

            if (date != inventoryDatesList[0])
            {
                Worksheet = excelPackage.Workbook.Worksheets.Add($"Ошибки на {date.ToShortDateString()}");

                #region Headers

                Worksheet.Cells[1, 1].Value = "Описание";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "Код";
                Worksheet.Cells[1, 10].Value = "Дата операции";
                Worksheet.Cells[1, 11].Value = "Номер паспорта (сертификата)";
                Worksheet.Cells[1, 12].Value = "тип";
                Worksheet.Cells[1, 13].Value = "радионуклиды";
                Worksheet.Cells[1, 14].Value = "номер";
                Worksheet.Cells[1, 15].Value = "количество, шт.";
                Worksheet.Cells[1, 16].Value = "суммарная активность, Бк";
                Worksheet.Cells[1, 17].Value = "код ОКПО изготовителя";
                Worksheet.Cells[1, 18].Value = "дата выпуска";
                Worksheet.Cells[1, 19].Value = "категория";
                Worksheet.Cells[1, 20].Value = "НСС, мес";
                Worksheet.Cells[1, 21].Value = "код формы собственности";
                Worksheet.Cells[1, 22].Value = "код ОКПО правообладателя";
                Worksheet.Cells[1, 23].Value = "вид";
                Worksheet.Cells[1, 24].Value = "номер2";
                Worksheet.Cells[1, 25].Value = "дата3";
                Worksheet.Cells[1, 26].Value = "поставщика или получателя";
                Worksheet.Cells[1, 27].Value = "перевозчика";
                Worksheet.Cells[1, 28].Value = "наименование";
                Worksheet.Cells[1, 29].Value = "тип4";
                Worksheet.Cells[1, 30].Value = "номер5";

                #endregion

                await AutoFitColumns();
            }
        }
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
        Worksheet.View.FreezePanes(3, 1);
        return Task.CompletedTask;
    }

    #endregion

    #endregion

    #region GetInventoryDatesList

    /// <summary>
    /// Формирование списка операций инвентаризации.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO операций инвентаризации, отсортированный по датам.</param>
    /// <returns>Список операций инвентаризации.</returns>
    private static Task<List<DateOnly>> GetInventoryDatesList(List<ShortForm11DTO> inventoryFormsDtoList)
    {
        var inventoryDates = inventoryFormsDtoList
            .Select(x => x.OpDate)
            .Distinct()
            .Order()
            .ToList();
        inventoryDates.Add(DateOnly.FromDateTime(DateTime.Now));

        return Task.FromResult(inventoryDates);
    }

    #endregion

    #region GetInventoryDatesTupleList

    /// <summary>
    /// Формирование списка кортежей периодов инвентаризаций.
    /// </summary>
    /// <param name="inventoryDates">Отсортированный по возрастанию список дат операций инвентаризации.</param>
    /// <returns>Список кортежей периодов инвентаризаций.</returns>
    private static Task<List<(DateOnly, DateOnly)>> GetInventoryDatesTupleList(List<DateOnly> inventoryDates)
    {
        List<(DateOnly, DateOnly)> inventoryDatesTupleList = [];
        for (var i = 0; i < inventoryDates.Count - 1; i++)
        {
            inventoryDatesTupleList.Add((inventoryDates[i], inventoryDates[i + 1]));
        }

        return Task.FromResult(inventoryDatesTupleList);
    }

    #endregion

    private static async Task<List<ShortForm11DTO>> GetForms11DtoList(List<ShortForm11DTO> inventoryFormsDtoList,
        List<ShortForm11DTO> plusMinusFormsDtoList, List<UniqueAccountingUnitDTO> uniqueAccountingUnitDtoList,
        List<DateOnly> inventoryDatesList, List<(DateOnly, DateOnly)> inventoryDatesTupleList, CancellationTokenSource cts)
    {
        List<ShortForm11DTO> unitInStockList = [];

        //  1. Не указаны в обеих инвентаризациях, последняя операция - постановка на учёт.
        List<ShortForm11DTO> missingInventoryReceivedUnitsList = [];

        //  2. Присутствуют в первой инвентаризации, отсутствуют в последующей, нет операций по снятию/постановке на учёт.
        List<ShortForm11DTO> missingInventoryPreviouslyRegisteredUnitsList = [];

        //  3. Присутствуют в последующей инвентаризации, последняя операция - не постановка на учёт.
        List<ShortForm11DTO> registeredWithOutReceivingUnitsList = [];

        //  4. Отсутствуют в первой инвентаризации, первая операция - снятие с учёта.
        List<ShortForm11DTO> transferredWithOutPreviosRegistrationUnitsList = [];

        //  5. Две операции снятия подряд
        List<ShortForm11DTO> reTransferredUnitsList = [];

        //  6. Две операции получения подряд
        List<ShortForm11DTO> reReceivedUnitsList = [];

        //  7. Не указаны в обеих инвентаризациях, первая операция нулевая.
        List<ShortForm11DTO> zeroOperationWithOutInventoriesUnitsList = [];

        unitInStockList.AddRange(inventoryFormsDtoList
            .Where(x => x.OpDate == inventoryDatesList[0])
            .DistinctBy(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type));


        //foreach (var unit in uniqueAccountingUnitDtoList)
        //foreach (var (firstInventoryDate, secondInventoryDate) in inventoryDatesTupleList)

        foreach (var (firstInventoryDate, secondInventoryDate) in inventoryDatesTupleList)
        {
            var isLastInventory = secondInventoryDate == inventoryDatesList[^1];

            foreach (var unit in uniqueAccountingUnitDtoList)
            {
                var inventoryWithCurrentUnit = inventoryFormsDtoList
                    .Where(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type 
                                == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type
                                && x.OpDate >= firstInventoryDate 
                                && x.OpDate <= secondInventoryDate)
                    .DistinctBy(x => x.OpDate)
                    .OrderBy(x => x.OpDate)
                    .ToList();

                var operationsWithCurrentUnit = plusMinusFormsDtoList
                    .Where(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type
                                == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type
                                && x.OpDate >= firstInventoryDate
                                && x.OpDate <= secondInventoryDate)
                    .OrderBy(x => x.OpDate)
                    .ToList();

                var inStock = unitInStockList
                    .Any(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type
                              == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type);

                if (inventoryWithCurrentUnit.Count == 0 && operationsWithCurrentUnit.Count == 0) continue;

                var hasTransferOperations = operationsWithCurrentUnit.Any(x => MinusOperation.Contains(x.OpCode));
                var hasReceiveOperations = operationsWithCurrentUnit.Any(x => PlusOperation.Contains(x.OpCode));

                var firstOperationIsTransfer = operationsWithCurrentUnit.Count > 0
                                               && MinusOperation.Contains(operationsWithCurrentUnit.First().OpCode);

                var firstOperationIsReceive = operationsWithCurrentUnit.Count > 0
                                              && PlusOperation.Contains(operationsWithCurrentUnit.First().OpCode);

                List<ShortForm11DTO> operationsWithCurrentUnitWithoutDuplicates = [];
                foreach (var group in operationsWithCurrentUnit.GroupBy(x => x.OpDate))
                {
                    var countMinus = group.Count(x => MinusOperation.Contains(x.OpCode));
                    var countPlus = group.Count(x => PlusOperation.Contains(x.OpCode));





                    var countStock = (inStock ? 1 : 0) + countPlus - countMinus;

                    switch (countStock)
                    {
                        case > 1:
                        {
                            var reReceivedFormDto = group
                                .Where(x => PlusOperation.Contains(x.OpCode))
                                .TakeLast(countStock - 1);
                            reReceivedUnitsList.AddRange(reReceivedFormDto);
                            operationsWithCurrentUnitWithoutDuplicates
                                .Add(group
                                    .First(x => PlusOperation.Contains(x.OpCode)));
                            break;
                        }
                        case 1:
                        {
                            operationsWithCurrentUnitWithoutDuplicates
                                .Add(group
                                    .Last(x => PlusOperation.Contains(x.OpCode)));
                            break;
                        }
                        case 0:
                        {
                            operationsWithCurrentUnitWithoutDuplicates
                                .Add(group
                                    .Last(x => MinusOperation.Contains(x.OpCode)));
                            break;
                        }
                        case < 0:
                        {
                            var reTransferredFormDto = group
                                .Where(x => MinusOperation.Contains(x.OpCode))
                                .TakeLast(Math.Abs(countStock));
                            reTransferredUnitsList.AddRange(reTransferredFormDto);
                            operationsWithCurrentUnitWithoutDuplicates
                                .Add(group
                                    .Last(x => MinusOperation.Contains(x.OpCode)));
                            break;
                        }
                    }
                }

                foreach (var operation in operationsWithCurrentUnitWithoutDuplicates)
                {
                    switch (inStock)
                    {
                        case true when PlusOperation.Contains(operation.OpCode):
                            {
                                reReceivedUnitsList.Add(operation);
                                break;
                            }
                        case false when PlusOperation.Contains(operation.OpCode):
                            {
                                inStock = true;
                                break;
                            }
                        case false when MinusOperation.Contains(operation.OpCode):
                            {
                                reTransferredUnitsList.Add(operation);
                                break;
                            }
                        case true when MinusOperation.Contains(operation.OpCode):
                            {
                                inStock = false;
                                break;
                            }
                    }
                }

                var lastOperationWithUnit = operationsWithCurrentUnit
                    .Union(inventoryWithCurrentUnit
                        .Where(x => x.OpDate >= firstInventoryDate || x.OpDate == secondInventoryDate))
                    .OrderByDescending(x => x.OpDate)
                    .First();

                //1
                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && !inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !hasTransferOperations
                    && !isLastInventory)
                {
                    missingInventoryReceivedUnitsList.Add(lastOperationWithUnit);
                }

                //2
                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !inStock
                    && !isLastInventory)
                {
                    missingInventoryPreviouslyRegisteredUnitsList.Add(lastOperationWithUnit);
                }

                //3
                if (!inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !isLastInventory
                    && (!hasReceiveOperations || !inStock))
                {
                    registeredWithOutReceivingUnitsList.Add(lastOperationWithUnit);
                }

                //4
                if (!inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && firstOperationIsTransfer)
                {
                    transferredWithOutPreviosRegistrationUnitsList.Add(lastOperationWithUnit);
                }

                //5
                if (!inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !inStock
                    && !isLastInventory)
                {
                    zeroOperationWithOutInventoriesUnitsList.Add(lastOperationWithUnit);
                }

                unitInStockList
                    .Remove(unitInStockList
                        .First(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type
                                    == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type));
                if (inStock)
                {
                    unitInStockList.Add(lastOperationWithUnit);
                }
            }
        }

        return unitInStockList;
    }
}