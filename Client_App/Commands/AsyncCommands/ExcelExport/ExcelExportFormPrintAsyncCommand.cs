using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Выбранная форма -> Выгрузка Excel -> Для печати
public class ExcelExportFormPrintAsyncCommand : ExcelBaseAsyncCommand
{
    private AnyTaskProgressBar progressBar;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> forms) return;
        var cts = new CancellationTokenSource();
        ExportType = "Для_печати";

        await Dispatcher.UIThread.InvokeAsync(() => progressBar = new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM_DB;
        progressBarVM.ExportType = ExportType;
        progressBarVM.ExportName = "Выгрузка отчёта на печать";
        progressBarVM.ValueBar = 5;
        var loadStatus = "Выгрузка отчёта";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var exportRepWithOutRows = (Report)forms.First();
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
            return;
        }

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        var exportRep = await dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Rows11.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows12.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows13.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows14.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows15.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows16.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows17.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows18.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows19.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows21.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows22.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows23.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows24.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows25.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows26.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows27.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows28.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows29.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows210.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows211.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows212.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Notes.OrderBy(x => x.Order))
            .FirstAsync(x => x.Id == exportRepWithOutRows.Id, cancellationToken: cts.Token);


        //var exportForm = (Report)forms.First();
        //exportRep = await ReportsStorage.GetReportAsync(exportRep.Id);
        var orgWithExportRep = await dbReadOnly.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(reports => reports.Report_Collection)
            .FirstAsync(x => x.Id == exportRepWithOutRows.ReportsId, cancellationToken: cts.Token);

        var formNum = RemoveForbiddenChars(exportRep.FormNum_DB);
        if (formNum is "" || forms.Count == 0) return;

        var regNum = RemoveForbiddenChars(orgWithExportRep.Master.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(orgWithExportRep.Master.OkpoRep.Value);
        var corNum = Convert.ToString(exportRep.CorrectionNumber_DB);
        string fileName;
        switch (formNum[0])
        {
            case '1':
                var startPeriod = RemoveForbiddenChars(exportRep.StartPeriod_DB);
                var endPeriod = RemoveForbiddenChars(exportRep.EndPeriod_DB);
                fileName = $"{ExportType}_{regNum}_{okpo}_{formNum}_{startPeriod}_{endPeriod}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}";
                break;
            case '2':
                var year = RemoveForbiddenChars(exportRep.Year_DB);
                fileName = $"{ExportType}_{regNum}_{okpo}_{formNum}_{year}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}";
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
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return; 

#if DEBUG
        var appFolderPath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Excel", $"{formNum}.xlsx");
#else
        var appFolderPath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Excel", $"{formNum}.xlsx");
#endif

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(appFolderPath));
        await exportRep.SortAsync();
        var worksheetTitle = excelPackage.Workbook.Worksheets[$"{formNum.Split('.')[0]}.0"];
        var worksheetMain = excelPackage.Workbook.Worksheets[formNum];
        worksheetTitle.Cells.Style.ShrinkToFit = true;
        worksheetMain.Cells.Style.ShrinkToFit = true;

        ExcelPrintTitleExport(formNum, worksheetTitle, exportRep);
        ExcelPrintSubMainExport(formNum, worksheetMain, exportRep);
        ExcelPrintNotesExport(formNum, worksheetMain, exportRep);
        ExcelPrintRowsExport(formNum, worksheetMain, exportRep);

        loadStatus = "Сохранение";
        progressBarVM.ValueBar = 95;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts);

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }
}