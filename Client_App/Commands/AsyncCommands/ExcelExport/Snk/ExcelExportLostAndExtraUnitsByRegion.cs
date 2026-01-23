using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources.CustomComparers.SnkComparers;
using Client_App.ViewModels.Messages;
using Client_App.Views;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

public class ExcelExportLostAndExtraUnitsByRegionAsyncCommand : ExcelExportSnkBaseAsyncCommand
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
            $"Проблемные_источники_по_региону_по_форме_{formNum}", ExportType);

        progressBarVM.SetProgressBar(6, "Запрос параметров СНК и региона");
        var (endSnkDate, snkParams, region) = await AskSnkParamsAndRegion(progressBar, cts);
        ExportType = $"Проблемные_источники_по_региону_{region!}_по_форме_{formNum}";

        progressBarVM.SetProgressBar(8, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(17, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        await FillExcelHeaders(formNum, excelPackage);

        progressBarVM.SetProgressBar(20, "Получение списка организаций");
        var repsDtoList = await GetReportsListByRegion(db, region!, formNum, cts, progressBar);

        var comparer = new SnkParamsComparer();
        var lostUnitsCurrentRow = 2;
        var extraUnitsCurrentRow = 2;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var currentRepsNum = 0;
        foreach (var repsDto in repsDtoList)
        {
            currentRepsNum++;

            var inventoryReportDtoList = await GetInventoryReportDtoList(db, repsDto.Id, formNum, endSnkDate, cts);
            var (firstSnkDate, inventoryFormsDtoList, _) = await GetInventoryFormsDtoList(db, inventoryReportDtoList, formNum, endSnkDate, cts, snkParams);

            if (inventoryFormsDtoList.Count is 0) continue;
            var lastInventoryDate = inventoryFormsDtoList.Max(x => x.OpDate);
            var lastInventoryFormsDtoList = inventoryFormsDtoList
                .Where(x => x.OpDate == lastInventoryDate)
                .OrderBy(x => x.PasNum)
                .ToList();

            var reportIds = await GetReportIds(db, repsDto.Id, formNum, cts);
            var plusMinusFormsDtoList = await GetPlusMinusFormsDtoList(db, reportIds, formNum, firstSnkDate, lastInventoryDate, cts, snkParams);
            var rechargeFormsDtoList = await GetRechargeFormsDtoList(db, repsDto.Id, formNum, firstSnkDate, lastInventoryDate, cts, snkParams);

            var uniqueUnitWithAllOperationDictionary = await GetDictionary_UniqueUnitsWithOperations(formNum, inventoryFormsDtoList, plusMinusFormsDtoList, rechargeFormsDtoList);
            var unitInStockDtoList = await GetUnitInStockDtoList(uniqueUnitWithAllOperationDictionary, formNum, firstSnkDate, progressBarVM);

            var lostUnits = unitInStockDtoList
                .Except(lastInventoryFormsDtoList, comparer)
                .ToList();
            var extraUnits = lastInventoryFormsDtoList
                .Except(unitInStockDtoList, comparer)
                .ToList();

            (lostUnitsCurrentRow, extraUnitsCurrentRow) = await FillExcel(excelPackage, repsDto, lostUnits, extraUnits, formNum, lastInventoryDate, lostUnitsCurrentRow, extraUnitsCurrentRow);

            progressBarDoubleValue += (double)75 / repsDtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {currentRepsNum} из {repsDtoList.Count} СНК организаций",
                $"Проблемные_источники_по_региону_{region}_по_форме_{formNum}");
        }

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

    #region FillExcelHeaders

    /// <summary>
    /// Заполняет заголовки Excel пакета.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="excelPackage">Excel пакет.</param>
    private static async Task FillExcelHeaders(string formNum, ExcelPackage excelPackage)
    {
        var lostUnitsWorksheet = excelPackage.Workbook.Worksheets.Add("Пропавшие источники");
        var extraUnitsWorksheet = excelPackage.Workbook.Worksheets.Add("Лишние источники");

        lostUnitsWorksheet.Cells[1, 1].Value = "№ п/п";
        lostUnitsWorksheet.Cells[1, 2].Value = "Рег.№";
        lostUnitsWorksheet.Cells[1, 3].Value = "Наименование";
        lostUnitsWorksheet.Cells[1, 4].Value = "ОКПО";
        lostUnitsWorksheet.Cells[1, 5].Value = "Номер паспорта (сертификата)";
        lostUnitsWorksheet.Cells[1, 6].Value = "тип";
        lostUnitsWorksheet.Cells[1, 7].Value = "радионуклиды";
        lostUnitsWorksheet.Cells[1, 8].Value = "номер";

        extraUnitsWorksheet.Cells[1, 1].Value = "№ п/п";
        extraUnitsWorksheet.Cells[1, 2].Value = "Рег.№";
        extraUnitsWorksheet.Cells[1, 3].Value = "Наименование";
        extraUnitsWorksheet.Cells[1, 4].Value = "ОКПО";
        extraUnitsWorksheet.Cells[1, 5].Value = "Номер паспорта (сертификата)";
        extraUnitsWorksheet.Cells[1, 6].Value = "тип";
        extraUnitsWorksheet.Cells[1, 7].Value = "радионуклиды";
        extraUnitsWorksheet.Cells[1, 8].Value = "номер";

        switch (formNum)
        {
            case "1.1":
                {
                    #region Headers

                    lostUnitsWorksheet.Cells[1, 9].Value = "количество, шт.";
                    lostUnitsWorksheet.Cells[1, 10].Value = "Номер УКТ";
                    lostUnitsWorksheet.Cells[1, 11].Value = "Дата последней инвентаризации";

                    extraUnitsWorksheet.Cells[1, 9].Value = "количество, шт.";
                    extraUnitsWorksheet.Cells[1, 10].Value = "Номер УКТ";
                    extraUnitsWorksheet.Cells[1, 11].Value = "Дата последней инвентаризации";

                    #endregion

                    break;
                }
            case "1.3":
                {
                    #region Headers

                    lostUnitsWorksheet.Cells[1, 9].Value = "Номер УКТ";
                    lostUnitsWorksheet.Cells[1, 10].Value = "Дата последней инвентаризации";

                    extraUnitsWorksheet.Cells[1, 9].Value = "Номер УКТ";
                    extraUnitsWorksheet.Cells[1, 10].Value = "Дата последней инвентаризации";

                    #endregion

                    break;
                }
        }

        await AutoFitColumns(lostUnitsWorksheet);
        await AutoFitColumns(extraUnitsWorksheet);
    }

    #region AutoFitColumns

    /// <summary>
    /// Для текущей страницы Excel пакета подбирает ширину колонок и замораживает первую строчку.
    /// </summary>
    private static Task AutoFitColumns(ExcelWorksheet worksheet)
    {
        for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            if (OperatingSystem.IsWindows()) worksheet.Column(col).AutoFit();
        }
        worksheet.View.FreezePanes(2, 1);
        return Task.CompletedTask;
    }

    #endregion

    #endregion

    #region FillExcel

    /// <summary>
    /// Заполняет строчки Excel пакета.
    /// </summary>
    private static Task<(int, int)> FillExcel(ExcelPackage excelPackage, ShortReportsDto reps, List<ShortFormDTO> lostUnits,
        List<ShortFormDTO> extraUnits, string formNum, DateOnly lastInventoryDate,
        int lostUnitsCurrentRow, int extraUnitsCurrentRow)
    {
        var lostUnitsWorksheet = excelPackage.Workbook.Worksheets[0];
        foreach (var dto in lostUnits)
        {
            lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 1].Value = lostUnitsCurrentRow - 1;
            lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 2].Value = ConvertToExcelString(reps.RegNum);
            lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 3].Value = ConvertToExcelString(reps.ShortName);
            lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 4].Value = ConvertToExcelString(reps.Okpo);
            lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 5].Value = ConvertToExcelString(dto.PasNum);
            lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 6].Value = ConvertToExcelString(dto.Type);
            lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 7].Value = ConvertToExcelString(dto.Radionuclids);
            lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 8].Value = ConvertToExcelString(dto.FacNum);

            switch (formNum)
            {
                case "1.1":
                    {
                        #region Headers

                        lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 9].Value = dto.Quantity;
                        lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 10].Value = dto.PackNumber;
                        lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 11].Value = ConvertToExcelDate(lastInventoryDate.ToShortDateString(), lostUnitsWorksheet, lostUnitsCurrentRow, 11);

                        #endregion

                        break;
                    }
                case "1.3":
                    {
                        #region Headers

                        lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 9].Value = dto.PackNumber;
                        lostUnitsWorksheet.Cells[lostUnitsCurrentRow, 10].Value = ConvertToExcelDate(lastInventoryDate.ToShortDateString(), lostUnitsWorksheet, lostUnitsCurrentRow, 10);

                        #endregion

                        break;
                    }
            }

            lostUnitsCurrentRow++;
        }

        var extraUnitsWorksheet = excelPackage.Workbook.Worksheets[1];
        foreach (var dto in extraUnits)
        {
            extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 1].Value = extraUnitsCurrentRow - 1;
            extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 2].Value = ConvertToExcelString(reps.RegNum);
            extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 3].Value = ConvertToExcelString(reps.ShortName);
            extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 4].Value = ConvertToExcelString(reps.Okpo);
            extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 5].Value = ConvertToExcelString(dto.PasNum);
            extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 6].Value = ConvertToExcelString(dto.Type);
            extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 7].Value = ConvertToExcelString(dto.Radionuclids);
            extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 8].Value = ConvertToExcelString(dto.FacNum);

            switch (formNum)
            {
                case "1.1":
                    {
                        #region Headers

                        extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 9].Value = dto.Quantity;
                        extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 10].Value = dto.PackNumber;
                        extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 11].Value = ConvertToExcelDate(lastInventoryDate.ToShortDateString(), extraUnitsWorksheet, extraUnitsCurrentRow, 11);

                        #endregion

                        break;
                    }
                case "1.3":
                    {
                        #region Headers

                        extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 9].Value = dto.PackNumber;
                        extraUnitsWorksheet.Cells[extraUnitsCurrentRow, 10].Value = ConvertToExcelDate(lastInventoryDate.ToShortDateString(), extraUnitsWorksheet, extraUnitsCurrentRow, 10);

                        #endregion

                        break;
                    }
            }

            extraUnitsCurrentRow++;
        }

        return Task.FromResult((lostUnitsCurrentRow, extraUnitsCurrentRow));
    }

    #endregion

    #region GetReportsListByRegion

    private static async Task<List<ShortReportsDto>> GetReportsListByRegion(DBModel db, string region, string formNum,
    CancellationTokenSource cts, AnyTaskProgressBar? progressBar)
    {
        var query11 = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
            .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows11)
            .Include(x => x.Report_Collection)
            .Where(reps =>
                reps.DBObservable != null
                && reps.Master_DB != null
                && reps.Report_Collection
                    .Any(rep => rep.FormNum_DB == formNum
                                && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
            )
            .Select(x => new ShortReportsDto
            {
                Id = x.Id,
                Okpo = x.Master_DB.OkpoRep.Value,
                ShortName = x.Master_DB.ShortJurLicoRep.Value,
                RegNum = x.Master_DB.RegNoRep.Value
            });

        var query13 = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
            .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows13)
            .Include(x => x.Report_Collection)
            .Where(reps =>
                reps.DBObservable != null
                && reps.Master_DB != null
                && reps.Report_Collection
                    .Any(rep => rep.FormNum_DB == formNum
                                && rep.Rows13.Any(form => form.OperationCode_DB == "10"))
            )
            .Select(x => new ShortReportsDto
            {
                Id = x.Id,
                Okpo = x.Master_DB.OkpoRep.Value,
                ShortName = x.Master_DB.ShortJurLicoRep.Value,
                RegNum = x.Master_DB.RegNoRep.Value
            });

        var repsDtoList = new List<ShortReportsDto>();

        if (formNum is "1.1")
        {
            repsDtoList = (await query11.ToListAsync(cts.Token))
                .Where(x => x.RegNum.StartsWith(region))
                .ToList();
        } else if (formNum is "1.3")
        {
            repsDtoList = (await query13.ToListAsync(cts.Token))
                .Where(x => x.RegNum.StartsWith(region))
                .ToList();
        }

        if (repsDtoList.Count == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = @"Не удалось совершить выгрузку, поскольку в БД отсутствуют организации "
                                     + $"с указанным регионом, имеющие отчёты по форме {formNum} и хотя бы одну "
                                     + "операцию инвентаризации.",
                    MinWidth = 400,
                    MaxWidth = 600,
                    MinHeight = 150,
                    MaxHeight = 300,
                    CanResize = true,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        return repsDtoList;
    }

    #endregion

    #region AskSnkParamsAndRegion

    private static async Task<(DateOnly endSnkDate, SnkParamsDto? snkParams, string? region)> AskSnkParamsAndRegion(
    AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var (endSnkDate, snkParams, region) = await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var vm = new GetSnkParamsVM
            {
                CommandName = "Параметры СНК и региона"
            };
            var window = new GetSnkParams
            {
                DataContext = vm
            };
            await window.ShowDialog(Desktop.MainWindow);

            if (cts.IsCancellationRequested) return (DateOnly.MinValue, null, null);
            var snkParams = new SnkParamsDto
            (
                vm.CheckPasNum,
                vm.CheckType,
                vm.CheckRadionuclids,
                vm.CheckFacNum,
                vm.CheckPackNumber
            );
            return (DateOnly.Parse(vm.Date), snkParams, vm.Region);
        });

        if (string.IsNullOrEmpty(region) || !int.TryParse(region, out _) || region.Length != 2)
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await ShowErrorMessage("Ошибка", "Неверный формат региона. Должно быть 2 цифры (00-99).");
            });

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        return (endSnkDate, snkParams, region);

    }

    #endregion

    #region ShowErrorMessage

    private static async Task ShowErrorMessage(string title, string message)
    {
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = title,
                ContentMessage = message,
                Icon = MessageBox.Avalonia.Enums.Icon.Error,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            });
        await messageBoxStandardWindow.Show();
    }

    #endregion

    #region ShortReportsDTO

    public class ShortReportsDto
    {
        public int Id { get; set; }

        //public int CountUnits { get; set; }

        //public string FormNum { get; set; } = "";

        //public DateOnly LastInventoryDate { get; set; }

        public string Okpo { get; set; }

        public string ShortName { get; set; }

        public string RegNum { get; set; }
    }

    #endregion
}