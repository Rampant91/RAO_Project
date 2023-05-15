using Models.Collections;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using Models.Interfaces;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.Excel;

//  Выбранная форма -> Выгрузка Excel -> Для анализа
public class ExcelExportFormAnalysisAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        ServiceExtension.LoggerManager.Import("tatata555");
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> forms) return;
        var cts = new CancellationTokenSource();
        ExportType = "Для анализа";

        var exportForm = (Report)forms.First();
        var orgWithExportForm = MainWindowVM.LocalReports.Reports_Collection
            .FirstOrDefault(t => t.Report_Collection.Contains(exportForm));
        var formNum = RemoveForbiddenChars(exportForm.FormNum_DB);
        if (formNum is "" || forms.Count == 0 || orgWithExportForm is null) return;
        
        var regNum = RemoveForbiddenChars(orgWithExportForm.Master.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(orgWithExportForm.Master.OkpoRep.Value);
        var corNum = Convert.ToString(exportForm.CorrectionNumber_DB);
        
        string fileName;
        switch (formNum[0])
        {
            case '1':
                var startPeriod = RemoveForbiddenChars(exportForm.StartPeriod_DB);
                var endPeriod = RemoveForbiddenChars(exportForm.EndPeriod_DB);
                fileName = $"{ExportType}_{regNum}_{okpo}_{formNum}_{startPeriod}_{endPeriod}_{corNum}_{BaseVM.Version}";
                break;
            case '2':
                var year = RemoveForbiddenChars(exportForm.Year_DB);
                fileName = $"{ExportType}_{regNum}_{okpo}_{formNum}_{year}_{corNum}_{BaseVM.Version}";
                break;
            default:
                return;
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
        if (forms?.Count == 0) return;
        Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}");
        WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}");
        int masterHeaderLength;
        if (formNum.Split('.')[0] == "1")
        {
            Form10.ExcelHeader(Worksheet, 1, 1);
            masterHeaderLength = Form10.ExcelHeader(WorksheetPrim, 1, 1);
        }
        else
        {
            Form20.ExcelHeader(Worksheet, 1, 1);
            masterHeaderLength = Form20.ExcelHeader(WorksheetPrim, 1, 1);
        }

        var tmpLength = Report.ExcelHeader(Worksheet, formNum, 1, masterHeaderLength + 1);
        Report.ExcelHeader(WorksheetPrim, formNum, 1, masterHeaderLength + 1);
        masterHeaderLength += tmpLength;

        #region ExcelHeaders

        switch (formNum)
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

        var exportFormList = new List<Report> { exportForm };
        
        ExcelExportRows(formNum, 2, masterHeaderLength, Worksheet, exportFormList);
        if (formNum is "2.2")
        {
            for (var col = Worksheet.Dimension.Start.Column; col <= Worksheet.Dimension.End.Column; col++)
            {
                if (Worksheet.Cells[1, col].Text != "№ п/п") continue;
                using var excelRange =
                    Worksheet.Cells[2, 1, Worksheet.Dimension.End.Row, Worksheet.Dimension.End.Column];
                excelRange.Sort(col - 1);
                break;
            }
        }
        ExcelExportNotes(formNum, 2, masterHeaderLength, WorksheetPrim, exportFormList);
        Worksheet.View.FreezePanes(2, 1);
        WorksheetPrim.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}