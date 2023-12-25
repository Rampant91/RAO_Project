using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            cts.Dispose();
            return;
        }

        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        var dbReadOnlyPath = Path.Combine(BaseVM.TmpDirectory, BaseVM.DbFileName + ".RAODB");
        try
        {
            if (!StaticConfiguration.IsFileLocked(dbReadOnlyPath))
            {
                File.Delete(dbReadOnlyPath);
                File.Copy(Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB"), dbReadOnlyPath);
            }
        }
        catch
        {
            cts.Dispose();
            return;
        }

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

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        #region GetDataFormDB

        #region Tuple11

        var tuple11 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.1")
            .Include(x => x.Rows11)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows11.Count, rep.Rows11.Count(form11 => form11.OperationCode_DB == "10")))
            .ToList();

        #endregion

        #region Tuple12

        var tuple12 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.2")
            .Include(x => x.Rows12)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows12.Count, rep.Rows12.Count(form12 => form12.OperationCode_DB == "10")))
            .ToList();

        #endregion

        #region Tuple13

        var tuple13 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.3")
            .Include(x => x.Rows13)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows13.Count, rep.Rows13.Count(form13 => form13.OperationCode_DB == "10")))
            .ToList();

        #endregion

        #region Tuple14

        var tuple14 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.4")
            .Include(x => x.Rows14)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows14.Count, rep.Rows14.Count(form14 => form14.OperationCode_DB == "10")))
            .ToList();

        #endregion

        #region Tuple15

        var tuple15 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.5")
            .Include(x => x.Rows15)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows15.Count, rep.Rows15.Count(form15 => form15.OperationCode_DB == "10")))
            .ToList();

        #endregion

        #region Tuple16

        var tuple16 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.6")
            .Include(x => x.Rows16)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows16.Count, rep.Rows16.Count(form16 => form16.OperationCode_DB == "10")))
            .ToList();

        #endregion

        #region Tuple17

        var tuple17 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.7")
            .Include(x => x.Rows17)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows17.Count, rep.Rows17.Count(form17 => form17.OperationCode_DB == "10")))
            .ToList();

        #endregion

        #region Tuple18

        var tuple18 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.8")
            .Include(x => x.Rows18)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows18.Count, rep.Rows18.Count(form18 => form18.OperationCode_DB == "10")))
            .ToList();

        #endregion

        #region Tuple19

        var tuple19 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.9")
            .Include(x => x.Rows19)
            .Select(rep => new Tuple<int, int, int>(rep.Id, rep.Rows19.Count, rep.Rows19.Count(form19 => form19.OperationCode_DB == "10")))
            .ToList(); 

        #endregion

        #endregion

        foreach (var reps in repsList)
        {
            var repList = reps.Report_Collection
                .Where(x =>
                {
                    if (startDateTime == DateTime.MinValue && endDateTime == DateTime.MaxValue) return true;
                    if (!DateTime.TryParse(x.EndPeriod_DB, out var repEndDateTime)) return false;
                    return repEndDateTime >= startDateTime && repEndDateTime <= endDateTime;
                })
                .OrderBy(x => x.FormNum_DB)
                .ThenBy(x => StringReverse(x.StartPeriod_DB))
                .ToList();
            foreach(var rep in repList)
            {
                var tupleList = rep.FormNum_DB switch
                {
                    "1.1" => tuple11,
                    "1.2" => tuple12,
                    "1.3" => tuple13,
                    "1.4" => tuple14,
                    "1.5" => tuple15,
                    "1.6" => tuple16,
                    "1.7" => tuple17,
                    "1.8" => tuple18,
                    "1.9" => tuple19
                };
                var tuple = tupleList.Find(x => x.Item1 == rep.Id) ?? new Tuple<int,int,int>(rep.Id, 0, 0);
                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                Worksheet.Cells[row, 4].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, row, 4);
                Worksheet.Cells[row, 5].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, row, 5);
                Worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[row, 7].Value = tuple.Item2; 
                Worksheet.Cells[row, 8].Value = InventoryCheck(tuple.Item2, tuple.Item3).TrimStart();
                row++;
            }
        }
        if (OperatingSystem.IsWindows())
        {
            var range = Worksheet.Cells[Worksheet.Dimension.Start.Row, Worksheet.Dimension.Start.Column, 
                Worksheet.Dimension.End.Row, Worksheet.Dimension.End.Column];

            Worksheet.Tables.Add(range, "myTable");
            Worksheet.Cells.AutoFitColumns(); // Под Astra Linux эта команда крашит программу без GDI дров
        }
        Worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}