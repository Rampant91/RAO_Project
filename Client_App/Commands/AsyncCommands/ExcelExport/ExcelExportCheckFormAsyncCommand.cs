using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

public class ExcelExportCheckFormAsyncCommand : ExcelBaseAsyncCommand
{
    private ExcelExportProgressBar progressBar;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not CheckFormVM checkFormVM) return;
        var cts = new CancellationTokenSource();
        var fileName = checkFormVM.TitleName;
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
        var openTmp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        await Dispatcher.UIThread.InvokeAsync(() => progressBar = new ExcelExportProgressBar(cts));
        var progressBarVM = progressBar.ExcelExportProgressBarVM;
        progressBarVM.ExportType = ExportType;
        progressBarVM.ExportName = "Выгрузка списка ошибок";
        progressBarVM.ValueBar = 5;
        var loadStatus = "выгрузка ошибок";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "ReportCheck";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Проверка формы");

        #region FillHeaders

        Worksheet.Cells[1, 1].Value = "№ п/п";
        Worksheet.Cells[1, 2].Value = "Стр.";
        Worksheet.Cells[1, 3].Value = "Графа";
        Worksheet.Cells[1, 4].Value = "Значение";
        Worksheet.Cells[1, 5].Value = "Сообщение";

        #endregion

        #region FillData
        
        var currentRow = 2;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var error in checkFormVM.CheckError)
        {
            Worksheet.Cells[currentRow, 1].Value = error.Index;
            Worksheet.Cells[currentRow, 2].Value = error.Row;
            Worksheet.Cells[currentRow, 3].Value = error.Column;
            Worksheet.Cells[currentRow, 4].Value = error.Value;
            Worksheet.Cells[currentRow, 5].Value = error.Message;
            progressBarDoubleValue += (double)90 / checkFormVM.CheckError.Count;
            progressBarVM.ValueBar = (int)Math.Floor(progressBarDoubleValue);
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
            currentRow++;
        }

        #endregion

        for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
        {
            if (OperatingSystem.IsWindows()) Worksheet.Column(col).AutoFit();
        }
        Worksheet.View.FreezePanes(2, 1);

        loadStatus = "Сохранение";
        progressBarVM.ValueBar = 95;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await ExcelSaveAndOpen(excelPackage, fullPath, openTmp, cts);

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }
}