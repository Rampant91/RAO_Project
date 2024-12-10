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
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

public class ExcelExportInventoryCheckAsyncCommand : ExcelExportSnkBaseAsyncCommand
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
        var endSnkDate = await AskSnkEndDate(progressBar, cts);

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

        var firstInventoryDate = inventoryReportDtoList
            .OrderBy(x => x.StartPeriod)
            .ThenBy(x => x.EndPeriod)
            .First()
            .StartPeriod;

        progressBarVM.SetProgressBar(15, "Формирование списка операций инвентаризации");
        var inventoryFormsDtoList = await GetInventoryFormsDtoList(db, inventoryReportDtoList, endSnkDate, cts);

        progressBarVM.SetProgressBar(16, "Получение списка дат инвентаризаций");
        var inventoryDatesList = await GetInventoryDatesList(inventoryFormsDtoList);

        progressBarVM.SetProgressBar(17, "Формирование периодов инвентаризаций");
        var inventoryDatesTupleList = await GetInventoryDatesTupleList(inventoryDatesList);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(19, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, inventoryDatesList);

        progressBarVM.SetProgressBar(20, "Формирование списка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, selectedReports.Id, endSnkDate, cts);

        progressBarVM.SetProgressBar(22, "Формирование списка всех операций");
        var unionFormsDtoList = await GetUnionFormsDtoList(inventoryFormsDtoList, plusMinusFormsDtoList, firstInventoryDate);

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

    #region AskSnkEndDate

    /// <summary>
    /// Запрос ввода даты формирования СНК.
    /// </summary>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Дата, на которую необходимо сформировать СНК.</returns>
    private static async Task<DateOnly> AskSnkEndDate(AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var date = DateOnly.MinValue;

        #region MessageInputSnkEndDate

        var result = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отмена", IsCancel = true }
                ],
                CanResize = true,
                ContentTitle = "Запрос даты",
                ContentMessage = "Введите дату окончания формирования выгрузки СНК.",
                MinWidth = 450,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (result.Button is "Отмена")
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        else if (!DateOnly.TryParse(result.Message, out date))
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
        return date;

    }

    #endregion

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
        foreach (var date in inventoryDatesList)
        {
            Worksheet = excelPackage.Workbook.Worksheets.Add($"СНК на {date.ToShortDateString()}");

            #region Headers

            Worksheet.Cells[1, 1].Value = "ОКПО";
            Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
            Worksheet.Cells[1, 3].Value = "Рег.№";
            Worksheet.Cells[1, 4].Value = "Номер корректировки";
            Worksheet.Cells[1, 5].Value = "Дата начала периода";
            Worksheet.Cells[1, 6].Value = "Дата конца периода";
            Worksheet.Cells[1, 7].Value = "№ п/п";
            Worksheet.Cells[1, 8].Value = "Код";
            Worksheet.Cells[1, 9].Value = "Дата операции";
            Worksheet.Cells[1, 10].Value = "Номер паспорта (сертификата)";
            Worksheet.Cells[1, 11].Value = "тип";
            Worksheet.Cells[1, 12].Value = "радионуклиды";
            Worksheet.Cells[1, 13].Value = "номер";
            Worksheet.Cells[1, 14].Value = "количество, шт.";
            Worksheet.Cells[1, 15].Value = "суммарная активность, Бк";
            Worksheet.Cells[1, 16].Value = "код ОКПО изготовителя";
            Worksheet.Cells[1, 17].Value = "дата выпуска";
            Worksheet.Cells[1, 18].Value = "категория";
            Worksheet.Cells[1, 19].Value = "НСС, мес";
            Worksheet.Cells[1, 20].Value = "код формы собственности";
            Worksheet.Cells[1, 21].Value = "код ОКПО правообладателя";
            Worksheet.Cells[1, 22].Value = "вид";
            Worksheet.Cells[1, 23].Value = "номер2";
            Worksheet.Cells[1, 24].Value = "дата3";
            Worksheet.Cells[1, 25].Value = "поставщика или получателя";
            Worksheet.Cells[1, 26].Value = "перевозчика";
            Worksheet.Cells[1, 27].Value = "наименование";
            Worksheet.Cells[1, 28].Value = "тип4";
            Worksheet.Cells[1, 29].Value = "номер5";

            #endregion

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
        List<ShortForm11DTO> plusMinusFormsDtoList, List<ShortForm11DTO> uniqueAccountingUnitDtoList,
        List<DateOnly> inventoryDatesList, List<(DateOnly, DateOnly)> inventoryDatesTupleList, CancellationTokenSource cts)
    {
        List<ShortForm11DTO> unitInStockList = [];

        //ЗРИ, имеются в предыдущей инвентаризации, отсутствующие в следующий и не имеющие ни одной операции передачи
        List<ShortForm11DTO> missingInventoryUnitsList = [];

        //ЗРИ, присутствующие в обеих инвентаризациях, но снятые с учёта
        List<ShortForm11DTO> mistakenlyPresentUnitsList = [];

        //ЗРИ, не указанные в предыдущей инвентаризации, отражённые в следующей инвентаризации, у которых последняя операция не на получение
        List<ShortForm11DTO> missingRegistrationUnitsList = [];

        //ЗРИ, не указанные в предыдущей инвентаризации, у которых первая операция на передачу
        List<ShortForm11DTO> incorrectlyTransferredUnitsList = [];

        //ЗРИ, не указанные в предыдущей инвентаризации, отражённые в следующей, снятые с учёта
        List<ShortForm11DTO> erroneousInventoryPreviouslyTransferredUnitsList = [];

        //ЗРИ, отсутствующие на момент следующей инвентаризации и отражённые в ней
        List<ShortForm11DTO> previouslyInventoriedReobtainedNotInventoriedUnitsList = [];

        //ЗРИ, полученные дважды подряд
        List<ShortForm11DTO> reReceivedUnitsList = [];

        //ЗРИ, отданные дважды подряд
        List<ShortForm11DTO> reTransferredUnitsList = [];

        unitInStockList.AddRange(inventoryFormsDtoList
            .Where(x => x.OpDate == inventoryDatesList[0])
            .DistinctBy(x => x.FacNum + x.Type + x.PasNum));

        foreach (var unit in uniqueAccountingUnitDtoList)
        {
            //var inStock2 = unitInStockList.Any(x => x.)
            List<ShortForm11DTO> firstInventoryUnitList = [];
            foreach (var (firstInventoryDate, secondInventoryDate) in inventoryDatesTupleList)
            {
                var isLastInventory = secondInventoryDate == inventoryDatesList[^1];

                if (firstInventoryDate == inventoryDatesList[0])
                {
                    firstInventoryUnitList
                        .AddRange(inventoryFormsDtoList
                            .Where(x => x.OpDate == firstInventoryDate)
                            .DistinctBy(x => x.FacNum + x.Type + x.PasNum));
                }

                var inventoryWithCurrentUnit = inventoryFormsDtoList
                    .Where(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum
                                && x.OpDate >= firstInventoryDate && x.OpDate <= secondInventoryDate)
                    .DistinctBy(x => x.OpDate)
                    .OrderBy(x => x.OpDate)
                    .ToList();

                var inStock = firstInventoryUnitList
                    .Any(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum);

                var operationsWithCurrentUnit = plusMinusFormsDtoList
                    .Where(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum
                                && x.OpDate >= firstInventoryDate && x.OpDate <= secondInventoryDate)
                    .OrderBy(x => x.OpDate)
                    .ToList();

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
                                .TakeLast(countStock);
                            reReceivedUnitsList.AddRange(reReceivedFormDto);
                            operationsWithCurrentUnitWithoutDuplicates
                                .Add(group
                                    .Last(x => PlusOperation.Contains(x.OpCode)));
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
                    missingInventoryUnitsList.Add(lastOperationWithUnit);
                }

                //2
                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !inStock
                    && !isLastInventory)
                {
                    mistakenlyPresentUnitsList.Add(lastOperationWithUnit);
                }

                //3
                if (!inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !isLastInventory
                    && (!hasReceiveOperations || !inStock))
                {
                    missingRegistrationUnitsList.Add(lastOperationWithUnit);
                }

                //4
                if (!inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && firstOperationIsTransfer)
                {
                    incorrectlyTransferredUnitsList.Add(lastOperationWithUnit);
                }

                //5
                if (!inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && !inStock
                    && !isLastInventory)
                {
                    erroneousInventoryPreviouslyTransferredUnitsList.Add(lastOperationWithUnit);
                }

                //6
                if (inventoryWithCurrentUnit.Exists(x => x.OpDate == firstInventoryDate)
                    && !inventoryWithCurrentUnit.Exists(x => x.OpDate == secondInventoryDate)
                    && firstOperationIsReceive
                    && !isLastInventory)
                {
                    previouslyInventoriedReobtainedNotInventoriedUnitsList.Add(lastOperationWithUnit);
                }

                unitInStockList
                    .Remove(unitInStockList
                        .First(x => x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum));
                if (inStock)
                {
                    unitInStockList.Add(lastOperationWithUnit);
                }
            }
        }

        return unitInStockList;
    }
}