﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Все формы & Excel -> Выбранная организация -> Все формы
public class ExcelExportAllAsyncCommandAsyncCommand : ExcelExportBaseAllAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        IsSelectedOrg = parameter is "SelectedOrg";
        string fileName;
        var mainWindow = Desktop.MainWindow as MainWindow;

        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Выгрузка не выполнена, поскольку в базе отсутствуют формы отчетности.",
                    MinHeight = 150,
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(mainWindow));

            #endregion

            return;
        }
        if (IsSelectedOrg)
        {
            var selectedReports = (Reports?)mainWindow?.SelectedReports?.FirstOrDefault();
            if (selectedReports is null || selectedReports.Report_Collection.Count == 0)
            {
                #region MessageExcelExportFail

                var msg = "Выгрузка не выполнена, поскольку ";
                msg += selectedReports is null
                    ? "не выбрана организация."
                    : "у выбранной организации" +
                       $"{Environment.NewLine}отсутствуют формы отчетности.";
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentHeader = "Уведомление",
                        ContentMessage = msg,
                        MinHeight = 150,
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(mainWindow));

                #endregion

                return;
            }
            CurrentReports = selectedReports;
            ExportType = "Выбранная организация_Все формы";
            var regNum = RemoveForbiddenChars(CurrentReports.Master_DB.RegNoRep.Value);
            var okpo = RemoveForbiddenChars(CurrentReports.Master_DB.OkpoRep.Value);
            fileName = $"{ExportType}_{regNum}_{okpo}_{Assembly.GetExecutingAssembly().GetName().Version}";
        }
        else
        {
            ExportType = "Все формы";
            fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
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
            if (!StaticConfiguration.IsFileLocked(dbReadOnlyPath))
            {
                File.Delete(dbReadOnlyPath);
                File.Copy(Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB"), dbReadOnlyPath);
            }
        }
        catch
        {
            cts.Dispose();
            return;
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;

        var repsList = new List<Reports>();
        if (IsSelectedOrg)
        {
            repsList.Add(CurrentReports);
        }
        else
        {
            repsList.AddRange(ReportsStorage.LocalReports.Reports_Collection.OrderBy(x => x.Master_DB.RegNoRep.Value));
        }

        HashSet<string> formNums = [];
        foreach (var rep in repsList
                     .SelectMany(reps => reps.Report_Collection)
                     .OrderBy(x => byte.Parse(x.FormNum_DB.Split('.')[0]))
                     .ThenBy(x => byte.Parse(x.FormNum_DB.Split('.')[1]))
                     .ToList())
        {
            formNums.Add(rep.FormNum_DB);
        }
        foreach (var formNum in formNums)
        {
            Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}");
            WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}");
            FillHeaders(formNum);
        }

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        foreach (var reps in repsList)
        {
            var oldDBPath = new string(StaticConfiguration.DBPath);
            StaticConfiguration.DBPath = dbReadOnlyPath;
            var repsWithRows = await ReportsStorage.ApiReports.GetAsync(reps.Id);
            StaticConfiguration.DBPath = oldDBPath;
            foreach (var formNum in formNums)
            {
                CurrentReports = repsWithRows;
                Worksheet = excelPackage.Workbook.Worksheets[$"Отчеты {formNum}"];
                WorksheetPrim = excelPackage.Workbook.Worksheets[$"Примечания {formNum}"];
                CurrentRow = Worksheet.Dimension.End.Row + 1;
                CurrentPrimRow = WorksheetPrim.Dimension.End.Row + 1;
                FillExportForms(formNum);
            }
        }

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}