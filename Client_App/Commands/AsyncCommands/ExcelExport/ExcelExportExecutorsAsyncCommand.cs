using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Threading;
using Client_App.Views;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Список исполнителей
public class ExcelExportExecutorsAsyncCommand : ExcelBaseAsyncCommand
{
    private Reports CurrentReports;
    private int _currentRow;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
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

        ExportType = "Список исполнителей";
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{BaseVM.Version}";

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

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;

        var repsList = new List<Reports>();
        repsList.AddRange(ReportsStorage.LocalReports.Reports_Collection
            .OrderBy(x => x.Master.RegNoRep.Value));

        HashSet<string> formNums = new();
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
            _currentRow = 2;
            Worksheet = excelPackage.Workbook.Worksheets.Add($"Форма {formNum}");
            FillExecutorsHeaders(formNum[0]);
            
            foreach (var reps in repsList)
            {
                CurrentReports = reps;
                foreach (var rep in CurrentReports.Report_Collection.OrderBy(x => StringReverse(x.StartPeriod_DB)))
                {
                    FillExecutors(rep);
                    _currentRow++;
                }
            }
        }

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #region FillExecutorsHeaders

    private void FillExecutorsHeaders(char formNum)
    {
        switch (formNum)
        {
            case '1':
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Форма";
                Worksheet.Cells[1, 5].Value = "Дата начала периода";
                Worksheet.Cells[1, 6].Value = "Дата конца периода";
                Worksheet.Cells[1, 7].Value = "Номер корректировки";
                Worksheet.Cells[1, 8].Value = "ФИО исполнителя";
                Worksheet.Cells[1, 9].Value = "Должность";
                Worksheet.Cells[1, 10].Value = "Телефон";
                Worksheet.Cells[1, 11].Value = "Электронная почта";
                break;
            case '2':
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Форма";
                Worksheet.Cells[1, 5].Value = "Отчетный год";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "ФИО исполнителя";
                Worksheet.Cells[1, 8].Value = "Должность";
                Worksheet.Cells[1, 9].Value = "Телефон";
                Worksheet.Cells[1, 10].Value = "Электронная почта";
                break;
        }
        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns(); // Под Astra Linux эта команда крашит программу без GDI дров
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region FillExecutors

    private void FillExecutors(Report rep)
    {
        switch (rep.FormNum_DB[0])
        {
            case '1':
                Worksheet.Cells[_currentRow, 1].Value = CurrentReports.Master.RegNoRep.Value;
                Worksheet.Cells[_currentRow, 2].Value = CurrentReports.Master.ShortJurLicoRep.Value;
                Worksheet.Cells[_currentRow, 3].Value = CurrentReports.Master.OkpoRep.Value;
                Worksheet.Cells[_currentRow, 4].Value = rep.FormNum_DB;
                Worksheet.Cells[_currentRow, 5].Value = rep.StartPeriod_DB;
                Worksheet.Cells[_currentRow, 6].Value = rep.EndPeriod_DB;
                Worksheet.Cells[_currentRow, 7].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[_currentRow, 8].Value = rep.FIOexecutor_DB;
                Worksheet.Cells[_currentRow, 9].Value = rep.GradeExecutor_DB;
                Worksheet.Cells[_currentRow, 10].Value = rep.ExecPhone_DB;
                Worksheet.Cells[_currentRow, 11].Value = rep.ExecEmail_DB;
                break;
            case '2':
                Worksheet.Cells[_currentRow, 1].Value = CurrentReports.Master.RegNoRep.Value;
                Worksheet.Cells[_currentRow, 2].Value = CurrentReports.Master.ShortJurLicoRep.Value;
                Worksheet.Cells[_currentRow, 3].Value = CurrentReports.Master.OkpoRep.Value;
                Worksheet.Cells[_currentRow, 4].Value = rep.FormNum_DB;
                Worksheet.Cells[_currentRow, 5].Value = rep.Year_DB;
                Worksheet.Cells[_currentRow, 6].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[_currentRow, 7].Value = rep.FIOexecutor_DB;
                Worksheet.Cells[_currentRow, 8].Value = rep.GradeExecutor_DB;
                Worksheet.Cells[_currentRow, 9].Value = rep.ExecPhone_DB;
                Worksheet.Cells[_currentRow, 10].Value = rep.ExecEmail_DB;
                break;
        }
    }

    #endregion
}