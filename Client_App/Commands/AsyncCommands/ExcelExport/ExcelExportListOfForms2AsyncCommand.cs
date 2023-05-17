using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
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
        foreach (var key in MainWindowVM.LocalReports.Reports_Collection)
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

            await MessageBox.Avalonia.MessageBoxManager
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
                .ShowDialog(Desktop.MainWindow);

            #endregion

            return;
        } 

        #endregion

        #region MessageInputYearRange

        var res = await MessageBox.Avalonia.MessageBoxManager
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
            .ShowDialog(Desktop.MainWindow);

        #endregion

        if (res.Button is null or "Отмена") return;
        var minYear = 0;
        var maxYear = 9999;
        if (!res.Message.Contains('-'))
        {
            if (int.TryParse(res.Message, out var parseYear) && parseYear.ToString().Length == 4)
            {
                minYear = parseYear;
                maxYear = parseYear;
            }
            else if (res.Message.Length > 1)
            {
                var firstResHalf = res.Message.Split('-')[0].Trim();
                var secondResHalf = res.Message.Split('-')[1].Trim();
                if(int.TryParse(firstResHalf, out var minYearParse) && minYearParse.ToString().Length == 4)
                {
                    minYear = minYearParse;
                }
                if(int.TryParse(firstResHalf, out var maxYearParse) && maxYearParse.ToString().Length == 4)
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
        if (MainWindowVM.LocalReports.Reports_Collection.Count == 0) return;

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
        foreach (var key in MainWindowVM.LocalReports.Reports_Collection)
        {
            var item = (Reports)key;
            if (item.Master_DB.FormNum_DB.Split('.')[0] == "2")
            {
                lst.Add(item);
            }
        }

        var row = 2;
        foreach (var reps in lst.OrderBy(x => x.Master_DB.RegNoRep.Value))
        {
            foreach (var rep in reps.Report_Collection
                         .OrderBy(x => x.FormNum_DB)
                         .ThenBy(x => x.Year_DB))
            {
                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                Worksheet.Cells[row, 4].Value = rep.Year_DB;
                Worksheet.Cells[row, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[row, 6].Value = rep.Rows.Count;
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