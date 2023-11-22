using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Список форм 1
public class ExcelExportListOfForms1AsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список форм 1";
        var findRep = 0;

        #region ReportsCountCheck

        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.Split('.')[0] == "1")
                {
                    findRep += 1;
                }
            }
        }

        if (findRep == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка всех отчетов по форме 1 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствует отчетность по формам 1",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        #endregion
        
        #region MessageInputDateRange

        var res = 
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отмена", IsCancel = true }
                },
                ContentTitle = "Задать период",
                ContentMessage = "Введите период дат через дефис (прим: 01.01.2022-07.03.2023)." +
                                 $"{Environment.NewLine}Если даты незаполнены или введены некорректно," +
                                 $"{Environment.NewLine}то выгрузка будет осуществляться без фильтра по датам.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (res.Button is null or "Отмена") return;
        var startDateTime = DateTime.MinValue;
        var endDateTime = DateTime.MaxValue;
        if (res.Message != null)
        {
            if (res.Message.Contains('-') && res.Message.Length > 6)
            {
                var firstPeriodHalf = res.Message.Split('-')[0].Trim();
                var secondPeriodHalf = res.Message.Split('-')[1].Trim();
                if (DateTime.TryParse(firstPeriodHalf, out var parseStartDateTime) )
                {
                    startDateTime = parseStartDateTime;
                }
                if (DateTime.TryParse(secondPeriodHalf, out var parseEndDateTime) )
                {
                    endDateTime = parseEndDateTime;
                }
            }
        }

        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{BaseVM.Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }

        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0) return;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 1");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Рег.№";
        Worksheet.Cells[1, 2].Value = "ОКПО";
        Worksheet.Cells[1, 3].Value = "Форма";
        Worksheet.Cells[1, 4].Value = "Дата начала";
        Worksheet.Cells[1, 5].Value = "Дата конца";
        Worksheet.Cells[1, 6].Value = "Номер кор.";
        Worksheet.Cells[1, 7].Value = "Количество строк";
        Worksheet.Cells[1, 8].Value = "Инвентаризация";

        #endregion

        var lst = new List<Reports>();
        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var item = (Reports)key;
            if (item.Master_DB.FormNum_DB.Split('.')[0] == "1")
            {
                lst.Add(item);
            }
        }

        var row = 2;
        var repsList = lst
            .OrderBy(x => x.Master_DB.RegNoRep.Value)
            .ToList();

        var repListWithForms = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Rows11)
            .Select(x => new Tuple<int, int, int>(x.Id, x.Rows.Count, x.Rows11.Count(x => x.OperationCode_DB == "10")))
            .ToList();

        //repListWithForms = repListWithForms
        //    .Where(rep =>
        //    {
        //        if (startDateTime == DateTime.MinValue && endDateTime == DateTime.MaxValue) return true;
        //        if (!DateTime.TryParse(rep.EndPeriod_DB, out var repEndDateTime)) return false;
        //        return repEndDateTime >= startDateTime && repEndDateTime <= endDateTime;
        //    })
        //    .OrderBy(x => x.FormNum_DB)
        //    .ThenBy(x => StringReverse(x.StartPeriod_DB))
        //    .ToList();

        //foreach (var rep in repListWithForms)
        //{
        //    Worksheet.Cells[row, 1].Value = rep.Rows10[0].RegNo_DB;  //reps.Master.RegNoRep.Value;
        //    Worksheet.Cells[row, 2].Value = rep.Rows10[0].Okpo_DB;    //reps.Master.OkpoRep.Value;
        //    Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
        //    Worksheet.Cells[row, 4].Value = rep.StartPeriod_DB;
        //    Worksheet.Cells[row, 5].Value = rep.EndPeriod_DB;
        //    Worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
        //    Worksheet.Cells[row, 7].Value = rep.Rows.Count;
        //    Worksheet.Cells[row, 8].Value = InventoryCheck(rep).TrimStart();
        //    row++;
        //}

        //foreach (var reps in repsList)
        //{
        //    var repList = reps.Report_Collection
        //        .Where(x =>
        //        {
        //            if (startDateTime == DateTime.MinValue && endDateTime == DateTime.MaxValue) return true;
        //            if (!DateTime.TryParse(x.EndPeriod_DB, out var repEndDateTime)) return false;
        //            return repEndDateTime >= startDateTime && repEndDateTime <= endDateTime;
        //        })
        //        .OrderBy(x => x.FormNum_DB)
        //        .ThenBy(x => StringReverse(x.StartPeriod_DB))
        //        .ToList();
        //    foreach (var rep in repList)
        //    {
        //        Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
        //        Worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
        //        Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
        //        Worksheet.Cells[row, 4].Value = rep.StartPeriod_DB;
        //        Worksheet.Cells[row, 5].Value = rep.EndPeriod_DB;
        //        Worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
        //        Worksheet.Cells[row, 7].Value = ReportsStorage.GetReportRowsCount(rep);
        //        Worksheet.Cells[row, 8].Value = ReportsStorage.InventoryCheck(rep).TrimStart();
        //        row++;
        //    }
        //}
        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns(); // Под Astra Linux эта команда крашит программу без GDI дров
        }
        Worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);

        //DateTime repEndDateTime;
        //var row = 2;
        //await using var db = StaticConfiguration.DBModel;
        //try
        //{
        //    db.ReportsCollectionDbSet
        //        .AsNoTracking()
        //        .SelectMany(filterReps => filterReps.Report_Collection
        //            .Where(x =>
        //                x.FormNum_DB.StartsWith('1')
        //                && startDateTime == DateTime.MinValue
        //                && endDateTime == DateTime.MaxValue
        //                && DateTime.TryParse(x.EndPeriod_DB, out repEndDateTime)
        //                && repEndDateTime >= startDateTime
        //                && repEndDateTime <= endDateTime)
        //            .Select(rep => new
        //            {
        //                RegNoRep = filterReps.Master.RegNoRep.Value,
        //                OKPO = filterReps.Master.OkpoRep.Value,
        //                Report = rep
        //            }))
        //        .OrderBy(x => x.RegNoRep)
        //        .ThenBy(x => x.Report.FormNum_DB)
        //        .ThenBy(x => StringReverse(x.Report.StartPeriod_DB))
        //        .ToList()
        //        .ForEach(x =>
        //        {
        //            Worksheet.Cells[row, 1].Value = x.RegNoRep;
        //            Worksheet.Cells[row, 2].Value = x.OKPO;
        //            Worksheet.Cells[row, 3].Value = x.Report.FormNum_DB;
        //            Worksheet.Cells[row, 4].Value = x.Report.StartPeriod_DB;
        //            Worksheet.Cells[row, 5].Value = x.Report.EndPeriod_DB;
        //            Worksheet.Cells[row, 6].Value = x.Report.CorrectionNumber_DB;
        //            Worksheet.Cells[row, 7].Value = x.Report.Rows.Count;
        //            Worksheet.Cells[row, 8].Value = InventoryCheck(x.Report).TrimStart();
        //            row++;
        //        });
        //}
        //catch (Exception e)
        //{
        //    //ignored
        //}
    }
}