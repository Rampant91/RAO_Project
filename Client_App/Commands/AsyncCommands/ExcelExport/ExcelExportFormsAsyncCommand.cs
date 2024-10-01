using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.Views;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x.
/// </summary>
public partial class ExcelExportFormsAsyncCommand : ExcelExportBaseAllAsyncCommand
{
    private AnyTaskProgressBar progressBar;

    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;
        var cts = new CancellationTokenSource();
        string fileName;
        var forSelectedOrg = parameter.ToString()!.Contains("Org");
        var selectedReports = (Reports?)mainWindow?.SelectedReports?.FirstOrDefault();
        var param = OnlyDigitsRegex().Replace(parameter.ToString(), "");

        switch (forSelectedOrg)
        {
            case true when selectedReports is null:
            {
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
            }
            case true when selectedReports.Report_Collection.All(rep => rep.FormNum_DB != param):
            case false when !ReportsStorage.LocalReports.Reports_Collection
                .Any(reps => reps.Report_Collection
                    .Any(rep => rep.FormNum_DB == param)):
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
            case true:
            {
                ExportType = $"Выбранная_организация_Формы_{param}";
                var regNum = StaticStringMethods.RemoveForbiddenChars(selectedReports.Master.RegNoRep.Value);
                var okpo = StaticStringMethods.RemoveForbiddenChars(selectedReports.Master.OkpoRep.Value);
                fileName = $"{ExportType}_{regNum}_{okpo}_{Assembly.GetExecutingAssembly().GetName().Version}";
                break;
            }
            default:
            {
                ExportType = $"Формы_{param}";
                fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
                break;
            }
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

        await Dispatcher.UIThread.InvokeAsync(() => progressBar = new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        progressBarVM.ExportType = ExportType;
        progressBarVM.ExportName = $"Выгрузка {ExportType.ToLower()}";
        progressBarVM.ValueBar = 2;
        var loadStatus = "Создание временной БД";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

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

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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

        FillHeaders(param);
        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns();
            WorksheetPrim.Cells.AutoFitColumns();
        }

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        var repsList = new List<Reports>();
        if (forSelectedOrg)
        {
            repsList.Add(selectedReports!);
        }
        else
        {
            repsList.AddRange(ReportsStorage.LocalReports.Reports_Collection
                .Where(reps => reps.Report_Collection
                    .Any(rep => rep.FormNum_DB == param)));
        }

        loadStatus = "Загрузка форм";
        progressBarVM.ValueBar = 5;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        double progressBarDoubleValue = progressBarVM.ValueBar;

        foreach (var reps in repsList)
        {
            loadStatus = $"Загрузка форм {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}";
            var repsWithRows = await GetReportsForms(param, reps, dbReadOnly, cts);
            CurrentReports = repsWithRows;
            CurrentRow = Worksheet.Dimension.End.Row + 1;
            CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
            FillExportForms(param);

            progressBarDoubleValue += (double)90 / repsList.Count;
            progressBarVM.ValueBar = (int)Math.Floor(progressBarDoubleValue);
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
        }
        Worksheet.View.FreezePanes(2, 1);

        loadStatus = "Сохранение";
        progressBarVM.ValueBar = 95;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts);

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }

    /// <summary>
    /// Загрузки из БД организации вместе со строчками для определённого номера формы. 
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="reps">Организация.</param>
    /// <param name="dbReadOnly">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Организацию вместе со строчками определённого номера формы.</returns>
    private static async Task<Reports> GetReportsForms(string formNum, Reports reps, DBModel dbReadOnly, CancellationTokenSource cts)
    {
        return formNum switch
        {
            #region GetForms1FromDb

            #region 1.1

            "1.1" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 1.2

            "1.2" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows12)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 1.3

            "1.3" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows13)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 1.4

            "1.4" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows14)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 1.5

            "1.5" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows15)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 1.6

            "1.6" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows16)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 1.7

            "1.7" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows17)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 1.8

            "1.8" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows18)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 1.9

            "1.9" => await dbReadOnly.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id == reps.Id)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Rows19)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #endregion

            #region GetForms2FromDb

            #region 2.1

            "2.1" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows21)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.2

            "2.2" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows22)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.3

            "2.3" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows23)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.4

            "2.4" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows24)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.5

            "2.5" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows25)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.6

            "2.6" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows26)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.7

            "2.7" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows27)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.8

            "2.8" => await dbReadOnly.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(x => x.Id == reps.Id)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows28)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.9

            "2.9" => await dbReadOnly.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id == reps.Id)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Rows29)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.10

            "2.10" => await dbReadOnly.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id == reps.Id)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Rows210)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.11

            "2.11" => await dbReadOnly.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id == reps.Id)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Rows211)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                .FirstAsync(cancellationToken: cts.Token),

            #endregion

            #region 2.12

            "2.12" => await dbReadOnly.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id == reps.Id)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Rows212)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                .FirstAsync(cancellationToken: cts.Token)

            #endregion

            #endregion
        };
    }

    /// <summary>
    /// Регулярка проверяющая, что строчка содержит только цифры.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"[^\d.]")]
    private static partial Regex OnlyDigitsRegex();
}