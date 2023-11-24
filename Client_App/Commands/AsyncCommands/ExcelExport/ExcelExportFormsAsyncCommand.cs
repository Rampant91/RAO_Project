using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация-Формы 1.x, 2.x
public class ExcelExportFormsAsyncCommand : ExcelExportBaseAllAsyncCommand
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

        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
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

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
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
                .ShowDialog(mainWindow));

            #endregion

            return;
        }

        #endregion
        
        var selectedReports = (Reports?)mainWindow?.SelectedReports.FirstOrDefault();
        switch (forSelectedOrg)
        {
            case true when selectedReports is null:

                #region MessageExcelExportFail

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(mainWindow));

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
            cts.Dispose();
            return;
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        var dbReadOnlyPath = Path.Combine(BaseVM.TmpDirectory, BaseVM.DbFileName + ".RAODB");
        try
        {
            File.Delete(dbReadOnlyPath);
            File.Copy(Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB"), dbReadOnlyPath);
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

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        var lst = new List<Report>();

        var repsList = new List<Reports>();
        if (forSelectedOrg)
        {
            repsList.Add(selectedReports!);
        }
        else
        {
            repsList.AddRange(ReportsStorage.LocalReports.Reports_Collection);
        }

        foreach (var reps in repsList)
        {
            var repsWithRows = param switch
            {
                #region GetForms1FromDb
                
                #region 1.1

                "1.1" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 1.2

                "1.2" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows12)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 1.3

                "1.3" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows13)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 1.4

                "1.4" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows14)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 1.5

                "1.5" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows15)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 1.6

                "1.6" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows16)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 1.7

                "1.7" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows17)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 1.8

                "1.8" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows18)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 1.9

                "1.9" => dbReadOnly.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == reps.Id)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows19)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                    .First(),

                #endregion

                #endregion

                #region GetForms2FromDb

                #region 2.1

                "2.1" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows21)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 2.2

                "2.2" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows22)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 2.3

                "2.3" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows23)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 2.4

                "2.4" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows24)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 2.5

                "2.5" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows25)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 2.6

                "2.6" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows26)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 2.7

                "2.7" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows27)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 2.8

                "2.8" => dbReadOnly.ReportsCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == reps.Id)
                            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows28)
                            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                            .First(),

                #endregion

                #region 2.9

                "2.9" => dbReadOnly.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == reps.Id)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows29)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                    .First(),

                #endregion

                #region 2.10

                "2.10" => dbReadOnly.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == reps.Id)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows210)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                    .First(),

                #endregion

                #region 2.11

                "2.11" => dbReadOnly.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == reps.Id)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows211)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                    .First(),

                #endregion

                #region 2.12

                "2.12" => dbReadOnly.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(x => x.Id == reps.Id)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows212)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                    .First()

                #endregion

                #endregion
            };
            CurrentReports = repsWithRows;
            CurrentRow = Worksheet.Dimension.End.Row + 1;
            CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
            FillExportForms(param);
        }

        Worksheet.View.FreezePanes(2, 1);
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}