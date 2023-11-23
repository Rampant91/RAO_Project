using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Разрывы и пересечения
public class ExcelExportIntersectionsAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Разрывы и пересечения";
        var findRep = 0;

        #region ReportsCountCheck

        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.Split('.')[0] == "1")
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
                        "Не удалось совершить выгрузку списка разрывов и пересечений дат," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствуют отчеты по форме 1",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        #endregion
        
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{BaseVM.Version}";
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
        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0) return;

        Worksheet = excelPackage.Workbook.Worksheets.Add("Разрывы и пересечения");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Рег.№";
        Worksheet.Cells[1, 2].Value = "ОКПО";
        Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        Worksheet.Cells[1, 4].Value = "Форма";
        Worksheet.Cells[1, 5].Value = "Начало прошлого периода";
        Worksheet.Cells[1, 6].Value = "Конец прошлого периода";
        Worksheet.Cells[1, 7].Value = "Начало периода";
        Worksheet.Cells[1, 8].Value = "Конец периода";
        Worksheet.Cells[1, 9].Value = "Зона разрыва";
        Worksheet.Cells[1, 10].Value = "Вид несоответствия"; 

        #endregion

        if (OperatingSystem.IsWindows()) Worksheet.Column(3).AutoFit();   // Под Astra Linux эта команда крашит программу без GDI дров      

        var listSortRep = ReportsStorage.LocalReports.Reports_Collection
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out _)
                              && DateTime.TryParse(rep.EndPeriod_DB, out _))
                .Select(rep =>
                {
                    if (true);
                    var start = DateTime.Parse(rep.StartPeriod_DB);
                    var end = DateTime.Parse(rep.EndPeriod_DB);
                    return new ReportForSort
                        {
                            RegNoRep = reps.Master_DB.RegNoRep.Value ?? "",
                            OkpoRep = reps.Master_DB.OkpoRep.Value ?? "",
                            FormNum = rep.FormNum_DB,
                            StartPeriod = start,
                            EndPeriod = end,
                            ShortYr = reps.Master_DB.ShortJurLicoRep.Value
                        };
                }))
            .OrderBy(x => x.RegNoRep)
            .ThenBy(x => x.FormNum)
            .ThenBy(x => x.StartPeriod)
            .ThenBy(x => x.EndPeriod)
            .ToList();

        var row = 2;
        for (var i = 0; i < listSortRep.Count; i++)
        {
            var rep = listSortRep[i];
            var repStart = rep.StartPeriod;
            var repEnd = rep.EndPeriod;
            var listToCompare = listSortRep
                .Skip(i + 1)
                .Where(x => x.RegNoRep == rep.RegNoRep && x.OkpoRep == rep.OkpoRep && x.FormNum == rep.FormNum)
                .ToList();
            var isNext = true;
            foreach (var repToCompare in listToCompare)
            {
                var repToCompareStart = repToCompare.StartPeriod;
                var repToCompareEnd = repToCompare.EndPeriod;
                var minEndDate = repEnd < repToCompareEnd
                    ? repEnd
                    : repToCompareEnd;

                Worksheet.Cells[row, 1].Value = rep.RegNoRep;
                Worksheet.Cells[row, 2].Value = rep.OkpoRep;
                Worksheet.Cells[row, 3].Value = rep.ShortYr;
                Worksheet.Cells[row, 4].Value = rep.FormNum;
                Worksheet.Cells[row, 5].Value = repStart.ToShortDateString();
                Worksheet.Cells[row, 6].Value = repEnd.ToShortDateString();
                Worksheet.Cells[row, 7].Value = repToCompareStart.ToShortDateString();
                Worksheet.Cells[row, 8].Value = repToCompareEnd.ToShortDateString();
                if (repStart == repToCompareStart && repEnd == repToCompareEnd)
                {
                    Worksheet.Cells[row, 9].Value = $"{repStart.ToShortDateString()}-{repEnd.ToShortDateString()}";
                    Worksheet.Cells[row, 10].Value = "совпадение";
                }
                if (repStart < repToCompareEnd && repEnd > repToCompareStart)
                {
                    Worksheet.Cells[row, 9].Value = $"{repToCompareStart.ToShortDateString()}-{minEndDate.ToShortDateString()}";
                    Worksheet.Cells[row, 10].Value = "пересечение";
                }
                else if (isNext && repEnd < repToCompareStart)
                {
                    Worksheet.Cells[row, 9].Value = $"{repEnd.ToShortDateString()}-{repToCompareStart.ToShortDateString()}";
                    Worksheet.Cells[row, 10].Value = "разрыв";
                }
                row++;
                isNext = false;
            }
        }

        for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
        {
            if (Worksheet.Cells[1, col].Value is "Сокращенное наименование") continue;

            if (OperatingSystem.IsWindows()) Worksheet.Column(col).AutoFit();
        }

        Worksheet.View.FreezePanes(2, 1);
        
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}