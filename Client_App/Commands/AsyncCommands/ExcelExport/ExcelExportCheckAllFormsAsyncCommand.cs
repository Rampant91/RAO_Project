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
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not IKeyCollection collection) return;
        var par = collection.ToList<Reports>().First();
        var cts = new CancellationTokenSource();
        ExportType = "Проверка отчётов";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Создание временной БД",
            "Проверка отчётов на ошибки", "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Выбор папки для отчётов");
        var folderPath = await SelectFolder(progressBar, cts);

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

        progressBarVM.SetProgressBar(10, "Загрузка форм");

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

        progressBarVM.SetProgressBar(50, "Создание отчётов");

        if (reps is null) return;
        var countRep = 0;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var rep in reps.Report_Collection
                     .OrderBy(x => x.FormNum_DB)
                     .ThenBy(x => StaticStringMethods.StringDateReverse(x.StartPeriod_DB)))
        {
            progressBarVM.SetProgressBar($"Проверка формы {rep.FormNum_DB}_{rep.StartPeriod_DB}-{rep.EndPeriod_DB}");
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

            progressBarVM.SetProgressBar($"Сохранение {rep.FormNum_DB}_{rep.StartPeriod_DB}-{rep.EndPeriod_DB}");

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

            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Сохранение {rep.FormNum_DB}_{rep.StartPeriod_DB}-{rep.EndPeriod_DB}");
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

        progressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    /// <summary>
    /// Выбор папки для сохранения отчётов.
    /// </summary>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Полный путь к папке сохранения отчётов о найденных ошибках.</returns>
    private static async Task<string> SelectFolder(AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        #region MessageGetSaveReportFolderPath

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Выбрать папку для сохранения отчётов", IsDefault = true },
                            new ButtonDefinition { Name = "Отмена", IsCancel = true }
                ],
                ContentTitle = "Проверка форм",
                ContentHeader = "Уведомление",
                ContentMessage = $"Для данной организации будет выполнена проверка всех имеющихся форм отчётности." +
                                 $"{Environment.NewLine}Отчёты о найденных ошибках будут сохранены в выбранной папке в формате .xlsx.",
                MinWidth = 450,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(progressBar ?? Desktop.MainWindow));

        #endregion

        if (res is not "Выбрать папку для сохранения отчётов") await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        var folderPath = await new OpenFolderDialog().ShowAsync(progressBar ?? Desktop.MainWindow);
        if (folderPath is null) await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        return folderPath!;
    }
}