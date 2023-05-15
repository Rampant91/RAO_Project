using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels;
using Models.Forms.Form1;
using Client_App.Resources;

namespace Client_App.Commands.AsyncCommands.Excel;

//  Excel -> Список форм 1
public class ExcelExportListOfForms1AsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список форм 1";
        var findRep = 0;

        #region ReportsCountCheck

        foreach (var key in MainWindowVM.LocalReports.Reports_Collection)
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

            await MessageBox.Avalonia.MessageBoxManager
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
                .ShowDialog(Desktop.MainWindow);

            #endregion

            return;
        }

        #endregion
        
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
        foreach (var key in MainWindowVM.LocalReports.Reports_Collection)
        {
            var item = (Reports)key;
            if (item.Master_DB.FormNum_DB.Split('.')[0] == "1")
            {
                lst.Add(item);
            }
        }

        var row = 2;
        foreach (var reps in lst.OrderBy(x => x.Master_DB.RegNoRep.Value).ToList())
        {
            foreach (var rep in reps.Report_Collection
                         .OrderBy(x => x.FormNum_DB)
                         .ThenBy(x => StaticStringMethods.StringReverse(x.StartPeriod_DB))
                         .ToList())
            {
                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                Worksheet.Cells[row, 4].Value = rep.StartPeriod_DB;
                Worksheet.Cells[row, 5].Value = rep.EndPeriod_DB;
                Worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[row, 7].Value = rep.Rows.Count;
                Worksheet.Cells[row, 8].Value = InventoryCheck(rep).TrimStart();
                row++;
            }
        }

        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns(); // Под Astra Linux эта команда крашит программу без GDI дров
        }
        Worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}