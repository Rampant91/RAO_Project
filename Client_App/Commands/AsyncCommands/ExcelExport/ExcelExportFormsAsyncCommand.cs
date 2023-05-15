using Avalonia.Controls;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels;
using Client_App.Resources;

namespace Client_App.Commands.AsyncCommands.Excel;

//  Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация-Формы 1.x, 2.x
public class ExcelExportFormsAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;
        var cts = new CancellationTokenSource();
        var findRep = 0;
        string fileName;

        var forSelectedOrg = parameter.ToString()!.Contains("Org");
        var param = Regex.Replace(parameter.ToString()!, "[^\\d.]", "");

        #region CheckReportsCount

        foreach (var key in MainWindowVM.LocalReports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.StartsWith(param))
                {
                    findRep++;
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
                        $"Не удалось совершить выгрузку форм {param}," +
                        $"{Environment.NewLine}поскольку эти формы отсутствуют в текущей базе.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(mainWindow);

            #endregion

            return;
        }

        #endregion
        
        var selectedReports = (Reports?)mainWindow?.SelectedReports.FirstOrDefault();
        switch (forSelectedOrg)
        {
            case true when selectedReports is null:

                #region MessageExcelExportFail

                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(mainWindow);

                #endregion

                return;
            case true:
            {
                ExportType = $"Выбранная организация_Формы {param}";
                var regNum = StaticStringMethods.RemoveForbiddenChars(selectedReports.Master.RegNoRep.Value);
                var okpo = StaticStringMethods.RemoveForbiddenChars(selectedReports.Master.OkpoRep.Value);
                fileName = $"{ExportType}_{regNum}_{okpo}_{BaseVM.Version}";
                break;
            }
            default:
                ExportType = $"Формы {param}";
                fileName = $"{ExportType}_{BaseVM.DbFileName}_{BaseVM.Version}";
                break;
        }

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
        Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {param}");
        WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {param}");
        int masterHeaderLength;
        if (param.Split('.')[0] == "1")
        {
            masterHeaderLength = Form10.ExcelHeader(Worksheet, 1, 1, id: "ID") + 1;
            masterHeaderLength = Form10.ExcelHeader(WorksheetPrim, 1, 1, id: "ID") + 1;
        }
        else
        {
            masterHeaderLength = Form20.ExcelHeader(Worksheet, 1, 1, id: "ID") + 1;
            masterHeaderLength = Form20.ExcelHeader(WorksheetPrim, 1, 1, id: "ID") + 1;
        }

        var t = Report.ExcelHeader(Worksheet, param, 1, masterHeaderLength);
        Report.ExcelHeader(WorksheetPrim, param, 1, masterHeaderLength);
        masterHeaderLength += t;
        masterHeaderLength--;

        #region BindingsExcelHeaders

        switch (param)
        {
            case "1.1":
                Form11.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.2":
                Form12.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.3":
                Form13.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.4":
                Form14.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.5":
                Form15.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.6":
                Form16.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.7":
                Form17.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.8":
                Form18.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.9":
                Form19.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.1":
                Form21.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.2":
                Form22.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.3":
                Form23.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.4":
                Form24.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.5":
                Form25.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.6":
                Form26.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.7":
                Form27.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.8":
                Form28.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.9":
                Form29.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.10":
                Form210.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.11":
                Form211.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.12":
                Form212.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
        }
        Note.ExcelHeader(WorksheetPrim, 1, masterHeaderLength + 1);

        #endregion

        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns();
            WorksheetPrim.Cells.AutoFitColumns();
        }

        var lst = new List<Report>();
        if (forSelectedOrg)
        {
            var newItem = selectedReports!.Report_Collection
                .Where(x => x.FormNum_DB.Equals(param))
                .OrderBy(x => param[0] is '1' ? StaticStringMethods.StringReverse(x.StartPeriod_DB) : x.Year_DB);
            lst.AddRange(newItem);
        }
        else
        {
            foreach (var key in MainWindowVM.LocalReports.Reports_Collection)
            {
                var item = (Reports)key;
                var newItem = item.Report_Collection
                    .Where(x => x.FormNum_DB.Equals(param))
                    .OrderBy(x => param[0] is '1' ? StaticStringMethods.StringReverse(x.StartPeriod_DB) : x.Year_DB);
                lst.AddRange(newItem);
            }
        }

        //foreach (Reports item in Local_Reports.Reports_Collection)
        //{
        //    lst.AddRange(item.Report_Collection);
        //}

        ExcelExportRows(param, 2, masterHeaderLength, Worksheet, lst, true);
        ExcelExportNotes(param, 2, masterHeaderLength, WorksheetPrim, lst, true);
        Worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}