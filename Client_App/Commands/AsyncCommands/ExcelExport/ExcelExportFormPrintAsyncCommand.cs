using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.ViewModels;
using Models.Collections;
using Models.Interfaces;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Выбранная форма -> Выгрузка Excel -> Для печати
public class ExcelExportFormPrintAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> forms) return;
        var cts = new CancellationTokenSource();
        ExportType = "Для печати";

        var exportForm = (Report)forms.First();
        exportForm = await ReportsStorage.GetReportAsync(exportForm.Id);
        var orgWithExportForm = ReportsStorage.LocalReports.Reports_Collection
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

        
        var appFolderPath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "Excel", $"{formNum}.xlsx");

        using ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(appFolderPath));
        await exportForm.SortAsync();
        var worksheetTitle = excelPackage.Workbook.Worksheets[$"{formNum.Split('.')[0]}.0"];
        var worksheetMain = excelPackage.Workbook.Worksheets[formNum];
        worksheetTitle.Cells.Style.ShrinkToFit = true;
        worksheetMain.Cells.Style.ShrinkToFit = true;

        ExcelPrintTitleExport(formNum, worksheetTitle, exportForm);
        ExcelPrintSubMainExport(formNum, worksheetMain, exportForm);
        ExcelPrintNotesExport(formNum, worksheetMain, exportForm);
        ExcelPrintRowsExport(formNum, worksheetMain, exportForm);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}