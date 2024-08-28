using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Client_App.ViewModels;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

public class ExcelExportCheckFormAsyncCommand : ExcelBaseAsyncCommand
{
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

        await ExcelSaveAndOpen(excelPackage, fullPath, openTmp, cts);
    }
}