using Avalonia.Threading;
using System;
using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;
using Client_App.ViewModels.ProgressBar;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

public class ExcelExportSnkAsyncCommand : ExcelBaseAsyncCommand
{
    #region Properties
    
    private static readonly string[] MinusOperation =
    [
        "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "47", "65", "67", "71", "72", "81", "82", "83", "84", "98"
    ];

    private static readonly string[] PlusOperation =
    [
        "11", "12", "17", "31", "32", "35", "37", "38", "39", "58", "73", "74", "75", "85", "86", "87", "88", "97"
    ];

    #endregion

    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var formNum = parameter as string;

        var mainWindow = Desktop.MainWindow as MainWindow;
        var selectedReports = mainWindow!.SelectedReports.First() as Reports;
        if (selectedReports is null)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"СНК_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(9, "Создание временной БД", $"Выгрузка СНК {formNum} {regNum}_{okpo}", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(11, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        var forms11DtoList = await GetForms11DtoList(db, selectedReports.Id, cts);

        
    }

    private static async Task<List<ShortForm11DTO>> GetForms11DtoList(DBModel db, int repsId, CancellationTokenSource cts)
    {
        var repDtoList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep =>  rep.FormNum_DB == "1.1" && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
                .Select(rep => new ShortReportDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB)))
            .ToListAsync(cts.Token);

        var firstRepWithCode10Id = repDtoList
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod, out var endDate) ? endDate : DateOnly.MaxValue)
            .First().Id;

        var inventoryList = await db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(rep => rep.Id == firstRepWithCode10Id)
            .SelectMany(x => x.Rows11
                .Where(x => x.OperationCode_DB == "10")
                .Select(form => new ShortForm11DTO(form.Id, form.FactoryNumber_DB, form.OperationCode_DB, 
                        form.PassportNumber_DB, form.Type_DB)))
            .ToListAsync(cts.Token);

        List<ShortForm11DTO> uniqueAccountingUnitList = [];
        foreach (var formDto in inventoryList
                     .Where(formDto => uniqueAccountingUnitList
                         .All(x => x.FacNum + x.Type + x.PasNum != formDto.FacNum + formDto.Type + formDto.PasNum)))
        {
            uniqueAccountingUnitList.Add(formDto);
        }
        
        var allForm11Operation = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.Id == repsId)
            .SelectMany(reps => reps.Report_Collection)
            .SelectMany(rep => rep.Rows11
                .Where(form => PlusOperation.Contains(form.OperationCode_DB) || MinusOperation.Contains(form.OperationCode_DB))
                .Select(form => new ShortForm11DTO2(form.Id, form.FactoryNumber_DB, form.OperationCode_DB, 
                    form.OperationDate_DB, form.PassportNumber_DB, form.Type_DB)))
            .ToListAsync(cts.Token);

        List<ShortForm11DTO> unitInStockList = [];
        foreach (var unit in uniqueAccountingUnitList)
        {
            var inStock = true;
            foreach (var operation in allForm11Operation
                         .Where(x => DateOnly.TryParse(x.OpDate, out _) 
                                     && x.FacNum + x.Type + x.PasNum == unit.FacNum + unit.Type + unit.PasNum)
                         .OrderBy(x => DateOnly.TryParse(x.OpDate, out var opDate) ? opDate : DateOnly.MaxValue))
            {
                if (MinusOperation.Contains(operation.OpCode)) inStock = false;
                else if (PlusOperation.Contains(operation.OpCode)) inStock = true;
            }
            if (inStock) unitInStockList.Add(unit);
        }
        return unitInStockList;
    }

    #region GetReportWithRows

    /// <summary>
    /// Получение отчёта вместе со строчками из БД.
    /// </summary>
    /// <param name="repId">Id отчёта.</param>
    /// <param name="dbReadOnly">Модель временной БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отчёт вместе со строчками.</returns>
    private static async Task<Report> GetReportWithRows(int repId, DBModel dbReadOnly, CancellationTokenSource cts)
    {
        return await dbReadOnly.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
                .Include(rep => rep.Rows11.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Notes.OrderBy(note => note.Order))
                .FirstAsync(rep => rep.Id == repId, cts.Token);
    }

    #endregion

    #region GetReportsWithRows

    /// <summary>
    /// Загружает из БД все отчёты вместе со строчками форм.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsWithOutRows">Организация вместе с коллекцией отчётов без строчек форм.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Организацию вместе с коллекцией отчётов со строчками форм.</returns>
    private static async Task<Reports> GetReportsWithRows(DBModel db, Reports repsWithOutRows, AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var repsWithRows = new Reports { Master = repsWithOutRows.Master };
        foreach (var rep in repsWithOutRows.Report_Collection
                     .OrderBy(x => x.FormNum_DB)
                     .ThenBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
                     .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue))
        {
            var repWithRows = await GetReportWithRows(rep.Id, db, cts);
            repsWithRows.Report_Collection.Add(repWithRows);
            progressBarDoubleValue += (double)50 / repsWithOutRows.Report_Collection.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загрузка отчёта {rep.FormNum_DB}_{rep.StartPeriod_DB}_{rep.EndPeriod_DB}",
                $"Загрузка отчётов {repsWithOutRows.Master_DB.RegNoRep.Value}_{repsWithOutRows.Master_DB.OkpoRep.Value}");
        }
        return repsWithRows;
    }

    #endregion

    private class ShortForm11DTO(int id, string facNum, string opCode , string pasNum, string type)
    {
        public readonly int Id = id;

        public readonly string FacNum = facNum;

        public readonly string OpCode = opCode;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;
    }

    private class ShortForm11DTO2(int id, string facNum, string opCode, string opDate, string pasNum, string type)
    {
        public readonly int Id = id;

        public readonly string FacNum = facNum;

        public readonly string OpCode = opCode;

        public readonly string OpDate = opDate;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;
    }

    private class ShortReportDTO(int id, string startPeriod, string endPeriod)
    {
        public readonly int Id = id;

        public readonly string StartPeriod = startPeriod;

        public readonly string EndPeriod = endPeriod;
    }


}