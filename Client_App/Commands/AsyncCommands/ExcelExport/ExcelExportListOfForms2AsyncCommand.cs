using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Список форм 2
public class ExcelExportListOfForms2AsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список форм 2";

        #region ReportsCountCheck

        var findRep = 0;
        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.Split('.')[0] == "2")
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
                        "Не удалось совершить выгрузку списка всех отчетов по форме 2 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствуют отчеты по форме 2",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        } 

        #endregion

        #region MessageInputYearRange

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
                ContentMessage = "Введите год или период лет через дефис (прим: 2021-2023)." +
                                 $"{Environment.NewLine}Если поле незаполнено или года введены некорректно," +
                                 $"{Environment.NewLine}то выгрузка будет осуществляться без фильтра по годам.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (res.Button is null or "Отмена") return;
        var minYear = 0;
        var maxYear = 9999;
        if (res.Message != null)
        {
            if (!res.Message.Contains('-'))
            {
                if (int.TryParse(res.Message, out var parseYear) && parseYear.ToString().Length == 4)
                {
                    minYear = parseYear;
                    maxYear = parseYear;
                }
            }
            else if (res.Message.Length > 4)
            {
                var firstResHalf = res.Message.Split('-')[0].Trim();
                var secondResHalf = res.Message.Split('-')[1].Trim();
                if(int.TryParse(firstResHalf, out var minYearParse) && minYearParse.ToString().Length == 4)
                {
                    minYear = minYearParse;
                }
                if(int.TryParse(secondResHalf, out var maxYearParse) && maxYearParse.ToString().Length == 4)
                {
                    maxYear = maxYearParse;
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

        Worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 2");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Рег.№";
        Worksheet.Cells[1, 2].Value = "ОКПО";
        Worksheet.Cells[1, 3].Value = "Форма";
        Worksheet.Cells[1, 4].Value = "Отчетный год";
        Worksheet.Cells[1, 5].Value = "Номер кор.";
        Worksheet.Cells[1, 6].Value = "Количество строк"; 

        #endregion

        var lst = new List<Reports>();
        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var item = (Reports)key;
            if (item.Master_DB.FormNum_DB.Split('.')[0] == "2")
            {
                lst.Add(item);
            }
        }

        var row = 2;
        var repsList = lst
            .OrderBy(x => x.Master_DB.RegNoRep.Value)
            .ToList();

        #region GetDataFormDB

        #region Tuple11

        var tuple11 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.1")
            .Include(x => x.Rows11)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows11.Count))
            .ToList();

        #endregion

        #region Tuple12

        var tuple12 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.2")
            .Include(x => x.Rows12)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows12.Count))
            .ToList();

        #endregion

        #region Tuple13

        var tuple13 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.3")
            .Include(x => x.Rows13)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows13.Count))
            .ToList();

        #endregion

        #region Tuple14

        var tuple14 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.4")
            .Include(x => x.Rows14)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows14.Count))
            .ToList();

        #endregion

        #region Tuple15

        var tuple15 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.5")
            .Include(x => x.Rows15)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows15.Count))
            .ToList();

        #endregion

        #region Tuple16

        var tuple16 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.6")
            .Include(x => x.Rows16)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows16.Count))
            .ToList();

        #endregion

        #region Tuple17

        var tuple17 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.7")
            .Include(x => x.Rows17)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows17.Count))
            .ToList();

        #endregion

        #region Tuple18

        var tuple18 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.8")
            .Include(x => x.Rows18)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows18.Count))
            .ToList();

        #endregion

        #region Tuple19

        var tuple19 = StaticConfiguration.DBModel.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.9")
            .Include(x => x.Rows19)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows19.Count))
            .ToList(); 

        #endregion

        #endregion

        foreach (var reps in repsList)
        {
            var repList = reps.Report_Collection
                .Where(x =>
                {
                    if (minYear == 0 && maxYear == 9999) return true;
                    if (x.Year_DB?.Length != 4 || !int.TryParse(x.Year_DB, out var currentRepsYear)) return false;
                    return currentRepsYear >= minYear && currentRepsYear <= maxYear;
                })
                .OrderBy(x => x.FormNum_DB)
                .ThenBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in repList)
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
                var tuple = tupleList.Find(x => x.Item1 == rep.Id) ?? new Tuple<int,int>(rep.Id, 0);
                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                Worksheet.Cells[row, 4].Value = rep.Year_DB;
                Worksheet.Cells[row, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[row, 6].Value = tuple.Item2;
                row++;
            }
        }
        if (OperatingSystem.IsWindows()) 
        {
            Worksheet.Cells.AutoFitColumns();  // Под Astra Linux эта команда крашит программу без GDI дров
        }
        Worksheet.View.FreezePanes(2, 1);
        
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}