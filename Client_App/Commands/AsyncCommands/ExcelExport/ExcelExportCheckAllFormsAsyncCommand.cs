using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Commands.SyncCommands.CheckForm;
using System;
using Client_App.ViewModels;
using System.Threading;
using OfficeOpenXml;
using System.IO;
using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using System.Diagnostics;
using Client_App.Interfaces.Logger;
using Client_App.Resources;
using Client_App.Views.ProgressBar;
using Models.CheckForm;
using System.Collections.Generic;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Проверяет все формы организации из главного окна и сохраняет в .xlsx
/// </summary>
public class ExcelExportCheckAllFormsAsyncCommand : ExcelBaseAsyncCommand
{
    private AnyTaskProgressBar progressBar;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not IKeyCollection collection) return;
        var cts = new CancellationTokenSource();
        ExportType = "Проверка форм";
        var par = collection.ToList<Reports>().First();
        await using var db = new DBModel(StaticConfiguration.DBPath);
        var folderPath = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (folderPath is null) return;

        await Dispatcher.UIThread.InvokeAsync(() => progressBar = new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM_DB;
        progressBarVM.ExportType = ExportType;
        progressBarVM.ExportName = "Проверка всех форм";
        progressBarVM.ValueBar = 5;
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
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Warning(msg);
            return;
        }

        loadStatus = "Загрузка форм";
        progressBarVM.ValueBar = 10;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #region GetReportsFormDB

        var reps = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsQueryable()
            .AsSplitQuery()
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows12)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows13)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows14)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows15)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows16)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows17)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows18)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows19)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows21)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows22)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows23)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows24)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows25)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows26)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows27)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows28)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows29)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows210)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows211)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows212)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
            .FirstOrDefaultAsync(x => x.Id == par.Id, cts.Token);

        #endregion

        loadStatus = "Создание отчётов";
        progressBarVM.ValueBar = 50;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        if (reps is null) return;
        var countRep = 0;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var rep in reps.Report_Collection
                     .OrderBy(x => x.FormNum_DB)
                     .ThenBy(x => StaticStringMethods.StringDateReverse(x.StartPeriod_DB)))
        {
            loadStatus = $"Проверка формы {rep.FormNum_DB}_{rep.StartPeriod_DB}-{rep.EndPeriod_DB}";
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
            List<CheckError> errorList = [];
            try
            {
                errorList = rep.FormNum_DB switch
                {
                    "1.1" => CheckF11.Check_Total(rep.Reports, rep),
                    "1.2" => CheckF12.Check_Total(rep.Reports, rep),
                    "1.3" => CheckF13.Check_Total(rep.Reports, rep),
                    "1.4" => CheckF14.Check_Total(rep.Reports, rep),
                    "1.5" => CheckF15.Check_Total(rep.Reports, rep),
                    "1.6" => CheckF16.Check_Total(rep.Reports, rep),
                    "1.7" => CheckF17.Check_Total(rep.Reports, rep),
                    "1.8" => CheckF18.Check_Total(rep.Reports, rep),
                    _ => throw new Exception()
                };
            }
            catch (Exception ex)
            {
                var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                          $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                ServiceExtension.LoggerManager.Warning(msg);

                #region MessageCheckFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                        ContentHeader = "Уведомление",
                        ContentMessage = $"В ходе выполнения проверки формы {rep.FormNum_DB}_{rep.StartPeriod_DB}-{rep.EndPeriod_DB} " +
                                         $"возникла непредвиденная ошибка.",
                        MinWidth = 400,
                        MinHeight = 170,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .Show(Desktop.MainWindow));

                #endregion

                continue;
            }

            loadStatus = $"Сохранение {rep.FormNum_DB}_{rep.StartPeriod_DB}-{rep.EndPeriod_DB}";
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            var checkFormVM = new CheckFormVM(new ChangeOrCreateVM(rep.FormNum_DB, rep), errorList);
            var fileName = checkFormVM.TitleName;
            var fullPath = Path.Combine(folderPath, fileName) + ".xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage excelPackage = new(new FileInfo(fullPath));
            excelPackage.Workbook.Properties.Author = "RAO_APP";
            excelPackage.Workbook.Properties.Title = "ReportCheck";
            excelPackage.Workbook.Properties.Created = DateTime.Now;
            Worksheet = excelPackage.Workbook.Worksheets.Add($"Проверка формы {rep.FormNum_DB}");

            #region FillHeaders

            Worksheet.Cells[1, 1].Value = "№ п/п";
            Worksheet.Cells[1, 2].Value = "Стр.";
            Worksheet.Cells[1, 3].Value = "Графа";
            Worksheet.Cells[1, 4].Value = "Значение";
            Worksheet.Cells[1, 5].Value = "Сообщение";

            #endregion

            #region FillData

            var currentRow = 2;
            foreach (var error in checkFormVM.CheckError)
            {
                Worksheet.Cells[currentRow, 1].Value = error.Index;
                Worksheet.Cells[currentRow, 2].Value = error.Row;
                Worksheet.Cells[currentRow, 3].Value = error.Column;
                Worksheet.Cells[currentRow, 4].Value = error.Value;
                Worksheet.Cells[currentRow, 5].Value = error.Message;
                currentRow++;
            }

            #endregion

            for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
            {
                if (OperatingSystem.IsWindows()) Worksheet.Column(col).AutoFit();
            }
            Worksheet.View.FreezePanes(2, 1);

            await excelPackage.SaveAsync(cancellationToken: cts.Token);
            countRep++;
            progressBarDoubleValue += (double)50 / reps.Report_Collection.Count;
            progressBarVM.ValueBar = (int)Math.Floor(progressBarDoubleValue);
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
        }

        #region MessageCheckComplete

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Открыть папку с выгрузкой" }
                ],
                ContentTitle = "Проверка форм",
                ContentHeader = "Уведомление",
                ContentMessage = $"Проверка форм организации {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value} завершена." +
                                 $"{Environment.NewLine}Проверено {countRep} из {reps.Report_Collection.Count} отчётов.",
                MinWidth = 400,
                MinHeight = 170,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is "Открыть папку с выгрузкой")
        {
            Process.Start("explorer", folderPath);
        }

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }
}