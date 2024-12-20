using Avalonia.Threading;
using Client_App.Views.ProgressBar;
using System.Threading;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using System.Reflection;
using System.Collections.Generic;
using OfficeOpenXml;
using Models.Forms.Form1;
using Microsoft.EntityFrameworkCore;
using static Client_App.Resources.StaticStringMethods;
using Client_App.ViewModels.ProgressBar;
using System.Collections.Concurrent;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Выгрузка в .xlsx СНК на дату.
/// </summary>
public class ExcelExportSnkAsyncCommand : ExcelExportSnkBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var mainWindow = Desktop.MainWindow as MainWindow;

        var formNum = (parameter as string)!;

        progressBarVM.SetProgressBar(5, "Проверка наличия отчётов",
            $"Выгрузка СНК {formNum}", "СНК");
        await CheckRepsAndRepPresence(formNum, progressBar, cts);
        var selectedReports = mainWindow!.SelectedReports.First() as Reports;

        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"СНК_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения", 
            $"Выгрузка СНК {formNum} {regNum}_{okpo}", ExportType);
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(8, "Запрос даты формирования СНК");
        var endSnkDate = await AskSnkEndDate(progressBar, cts);

        progressBarVM.SetProgressBar(10, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(13, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, endSnkDate);

        progressBarVM.SetProgressBar(15, "Загрузка инвентаризационных отчётов");
        var inventoryReportDtoList = await GetInventoryReportDtoList(db, selectedReports.Id, endSnkDate, cts);

        progressBarVM.SetProgressBar(17, "Загрузка операций инвентаризации");
        var inventoryFormsDtoList = await GetInventoryFormsDtoList(db, inventoryReportDtoList, endSnkDate, cts);

        progressBarVM.SetProgressBar(20, "Загрузка операций передачи/получения");
        var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, selectedReports.Id, endSnkDate, cts);

        progressBarVM.SetProgressBar(23, "Формирование списка всех операций");
        var unionFormsDtoList = await GetUnionFormsDtoList(inventoryFormsDtoList, plusMinusFormsDtoList);

        progressBarVM.SetProgressBar(25, "Формирование списка уникальных учётных единиц");
        var uniqueAccountingUnitDtoList = await GetUniqueAccountingUnitDtoList(unionFormsDtoList);

        progressBarVM.SetProgressBar(30, "Формирование СНК");
        var unitInStockDtoList = await GetUnitInStockDtoList(inventoryFormsDtoList, plusMinusFormsDtoList, uniqueAccountingUnitDtoList, 
            progressBarVM, cts);

        progressBarVM.SetProgressBar(60, "Загрузка форм");
        var fullFormsSnkList = await GetFullFormsSnkList(db, unitInStockDtoList, progressBarVM, cts);

        progressBarVM.SetProgressBar(85, "Проверка наличия");
        await CheckPresenceInSnk(fullFormsSnkList, endSnkDate, progressBar, cts);

        progressBarVM.SetProgressBar(90, "Заполнение строчек в .xlsx");
        await FillExcel(fullFormsSnkList);

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

    #region CheckPresenceInSnk

    /// <summary>
    /// Проверяет наличие учётных единиц и при их отсутствии выводит сообщение.
    /// </summary>
    /// <param name="fullFormsSnkList">Список форм с данными отчётов и организации.</param>
    /// <param name="endSnkDate">Дата, на которую нужно сформировать СНК.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckPresenceInSnk(List<Form11> fullFormsSnkList, DateOnly endSnkDate,
        AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        if (fullFormsSnkList.Count == 0)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = $"На {endSnkDate.ToShortDateString()} учётные единицы в наличии отсутствуют.",
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
    /// Проверяет наличие выбранной организации. Проверяет наличие хотя бы одного отчёта, с выбранным номером формы.
    /// В случае отсутствия выводит соответствующее сообщение и закрывает команду.
    /// </summary>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CheckRepsAndRepPresence(string formNum, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;

        //Аннотации врут, не убирай
        if (mainWindow!.SelectedReports is null || mainWindow!.SelectedReports.First() is not Reports selectedReports)  
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
    /// <param name="date">Дата, на которую формируется СНК.</param>
    private async Task FillExcelHeaders(ExcelPackage excelPackage, DateOnly date)
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
    /// <param name="fullFormsSnkList">Список полных форм СНК.</param>
    private Task FillExcel(List<Form11> fullFormsSnkList)
    {
        var currentRow = 2;
        foreach (var form in fullFormsSnkList)
        {
            if (form.Report?.Reports == null) continue;

            Worksheet.Cells[currentRow, 1].Value = form.Report.Reports.Master_DB.OkpoRep.Value;
            Worksheet.Cells[currentRow, 2].Value = form.Report.Reports.Master_DB.ShortJurLicoRep.Value;
            Worksheet.Cells[currentRow, 3].Value = form.Report.Reports.Master_DB.RegNoRep.Value;
            Worksheet.Cells[currentRow, 4].Value = form.Report.CorrectionNumber_DB;
            Worksheet.Cells[currentRow, 5].Value = ConvertToExcelDate(form.Report.StartPeriod_DB, Worksheet, currentRow, 5);
            Worksheet.Cells[currentRow, 6].Value = ConvertToExcelDate(form.Report.EndPeriod_DB, Worksheet, currentRow, 6);
            Worksheet.Cells[currentRow, 7].Value = form.NumberInOrder_DB;
            Worksheet.Cells[currentRow, 8].Value = ConvertToExcelString(form.OperationCode_DB);
            Worksheet.Cells[currentRow, 9].Value = ConvertToExcelDate(form.OperationDate_DB, Worksheet, currentRow, 9);
            Worksheet.Cells[currentRow, 10].Value = ConvertToExcelString(form.PassportNumber_DB);
            Worksheet.Cells[currentRow, 11].Value = ConvertToExcelString(form.Type_DB);
            Worksheet.Cells[currentRow, 12].Value = ConvertToExcelString(form.Radionuclids_DB);
            Worksheet.Cells[currentRow, 13].Value = ConvertToExcelString(form.FactoryNumber_DB);
            Worksheet.Cells[currentRow, 14].Value = form.Quantity_DB is null ? "-" : form.Quantity_DB;
            Worksheet.Cells[currentRow, 15].Value = ConvertToExcelDouble(form.Activity_DB);
            Worksheet.Cells[currentRow, 16].Value = ConvertToExcelString(form.CreatorOKPO_DB);
            Worksheet.Cells[currentRow, 17].Value = ConvertToExcelDate(form.CreationDate_DB, Worksheet, currentRow, 17);
            Worksheet.Cells[currentRow, 18].Value = form.Category_DB is null ? "-" : form.Category_DB;
            Worksheet.Cells[currentRow, 19].Value = form.SignedServicePeriod_DB is null ? "-" : form.SignedServicePeriod_DB;
            Worksheet.Cells[currentRow, 20].Value = form.PropertyCode_DB is null ? "-" : form.PropertyCode_DB;
            Worksheet.Cells[currentRow, 21].Value = ConvertToExcelString(form.Owner_DB);
            Worksheet.Cells[currentRow, 22].Value = form.DocumentVid_DB is null ? "-" : form.DocumentVid_DB;
            Worksheet.Cells[currentRow, 23].Value = ConvertToExcelString(form.DocumentNumber_DB);
            Worksheet.Cells[currentRow, 24].Value = ConvertToExcelDate(form.DocumentDate_DB, Worksheet, currentRow, 24);
            Worksheet.Cells[currentRow, 25].Value = ConvertToExcelString(form.ProviderOrRecieverOKPO_DB);
            Worksheet.Cells[currentRow, 26].Value = ConvertToExcelString(form.TransporterOKPO_DB);
            Worksheet.Cells[currentRow, 27].Value = ConvertToExcelString(form.PackName_DB);
            Worksheet.Cells[currentRow, 28].Value = ConvertToExcelString(form.PackType_DB);
            Worksheet.Cells[currentRow, 29].Value = ConvertToExcelString(form.PackNumber_DB);
            currentRow++;
        }
        return Task.CompletedTask;
    }

    #endregion

    #region GetFullForm

    /// <summary>
    /// Загрузка из БД полных форм вместе с данными отчётов и организации.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="unitInStockDtoList">Список DTO учётных единиц в наличии на дату.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список форм с данными отчётов и организации.</returns>
    private static async Task<List<Form11>> GetFullFormsSnkList(DBModel db, List<ShortForm11DTO> unitInStockDtoList,
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<Form11> formsList = [];

        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;
        foreach (var unit in unitInStockDtoList)
        {
            var form = await db.form_11
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report)
                .ThenInclude(x => x.Reports)
                .ThenInclude(x => x.Master_DB)
                .ThenInclude(x => x.Rows10)
                .FirstAsync(x => x.Id == unit.Id, cts.Token);
            formsList.Add(form);

            progressBarDoubleValue += (double)30 / unitInStockDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка {currentUnitNum} формы из {unitInStockDtoList.Count}",
                "Загрузка форм");
            currentUnitNum++;
        }
        return formsList;

    }

    #endregion

    #region GetUnitInStockDtoList

    /// <summary>
    /// Формирует список DTO учётных единиц в наличии на дату.
    /// </summary>
    /// <param name="inventoryFormsDtoList">Список DTO инвентаризационных операций.</param>
    /// <param name="plusMinusFormsDtoList">Список DTO операций приёма-передачи.</param>
    /// <param name="uniqueAccountingUnitDtoList">Список DTO уникальных учётных единиц.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO учётных единиц в наличии на дату.</returns>
    private static async Task<List<ShortForm11DTO>> GetUnitInStockDtoList(List<ShortForm11DTO> inventoryFormsDtoList,
    List<ShortForm11DTO> plusMinusFormsDtoList, List<UniqueAccountingUnitDTO> uniqueAccountingUnitDtoList,
    AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<ShortForm11DTO> unitInStockList = [];

        var firstInventoryDate = inventoryFormsDtoList.Count == 0
            ? DateOnly.MinValue
            : inventoryFormsDtoList
                .OrderBy(x => x.OpDate)
                .Select(x => x.OpDate)
                .First();

        unitInStockList.AddRange(inventoryFormsDtoList
            .Where(x => x.OpDate == firstInventoryDate)
            .DistinctBy(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type));

        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentUnitNum = 1;

        var unitInStockBag = new ConcurrentBag<ShortForm11DTO>(unitInStockList);
        ParallelOptions parallelOptions = new()
        {
            CancellationToken = cts.Token,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        await Parallel.ForEachAsync(uniqueAccountingUnitDtoList, parallelOptions, (unit, token) =>
        {
            var quantity = unitInStockBag
                .FirstOrDefault(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type
                                      == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type)
                ?.Quantity ?? 0;

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
                        operationsWithCurrentUnitWithoutDuplicates.Add(lastOp);
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
                        operationsWithCurrentUnitWithoutDuplicates.Add(lastOp);
                        break;
                    }
                }
            }

            foreach (var operation in operationsWithCurrentUnitWithoutDuplicates)
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

            var lastOperationWithUnit = operationsWithCurrentUnit
                .Union(inventoryWithCurrentUnit
                    .Where(x => x.OpDate >= firstInventoryDate))
                .OrderByDescending(x => x.OpDate)
                .FirstOrDefault();

            if (lastOperationWithUnit == null) return default;

            var currentUnit = unitInStockBag
                .FirstOrDefault(x => x.FacNum + x.PackNumber + x.PasNum + x.Radionuclids + x.Type
                                     == unit.FacNum + unit.PackNumber + unit.PasNum + unit.Radionuclids + unit.Type);
            if (currentUnit != null) unitInStockBag.TryTake(out currentUnit);
            if (quantity > 0)
            {
                unitInStockBag.Add(lastOperationWithUnit);
            }
            progressBarDoubleValue += (double)30 / uniqueAccountingUnitDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {currentUnitNum} единиц из {uniqueAccountingUnitDtoList.Count}",
                "Проверка наличия");
            currentUnitNum++;
            return default;
        });
        return unitInStockBag.ToList();
    }

    #endregion
}