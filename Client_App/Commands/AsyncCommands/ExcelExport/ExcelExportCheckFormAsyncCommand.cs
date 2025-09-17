using Avalonia.Styling;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using OfficeOpenXml.Style;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Сохранить список ошибок при проверке отчёта в .xlsx.
/// </summary>
public class ExcelExportCheckFormAsyncCommand : ExcelBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not (CheckFormVM or NewCheckFormVM)) return;
        
        dynamic checkFormVM;
        if (parameter is CheckFormVM vm) checkFormVM = vm;
        else checkFormVM = (NewCheckFormVM)parameter;

        var cts = new CancellationTokenSource();
        ExportType = "Список_ошибок";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения", "Выгрузка списка ошибок", ExportType);
        var fileName = $"{checkFormVM.TitleName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);
        Worksheet = excelPackage.Workbook.Worksheets.Add("Проверка формы");

        progressBarVM.SetProgressBar(15, "Выгрузка ошибок");
        await FillExcel(checkFormVM, progressBarVM);

        progressBarVM.SetProgressBar(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

        progressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    /// <summary>
    /// Заполняет Excel пакет данными.
    /// </summary>
    /// <param name="checkFormVM">ViewModel окна проверки отчёта.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <returns>Успешно выполненная Task.</returns>
    private Task FillExcel(CheckFormVM checkFormVM, AnyTaskProgressBarVM progressBarVM)
    {
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

            if (error.IsCritical)
            {
                Worksheet.Cells[currentRow, 1].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
                Worksheet.Cells[currentRow, 2].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
                Worksheet.Cells[currentRow, 3].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
                Worksheet.Cells[currentRow, 4].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
                Worksheet.Cells[currentRow, 5].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
            }

            currentRow++;

            progressBarDoubleValue += (double)90 / checkFormVM.CheckError.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(double.Clamp(progressBarDoubleValue,0,100)), "Выгрузка ошибок");
        }

        #endregion

        #region FormatCells

        if (OperatingSystem.IsWindows())
        {
            for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
            {
                Worksheet.Column(col).AutoFit();
                for (var row = 2; row < currentRow; row++)
                {
                    Worksheet.Cells[row, col].Style.WrapText = true;
                    Worksheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Worksheet.Cells[row, col].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                }
            }
            Worksheet.Column(2).Width = 2 * Worksheet.Column(1).Width;
            Worksheet.Column(3).Width = 2 * Worksheet.Column(1).Width;
            Worksheet.Column(4).Width = 11 * Worksheet.Column(1).Width;
            Worksheet.Column(5).Width = 11 * Worksheet.Column(1).Width;
        }

        #endregion

        Worksheet.View.FreezePanes(2, 1);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Заполняет Excel пакет данными.
    /// </summary>
    /// <param name="checkFormVM">ViewModel окна проверки отчёта.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <returns>Успешно выполненная Task.</returns>
    private Task FillExcel(NewCheckFormVM checkFormVM, AnyTaskProgressBarVM progressBarVM)
    {
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

            if (error.IsCritical)
            {
                Worksheet.Cells[currentRow, 1].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
                Worksheet.Cells[currentRow, 2].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
                Worksheet.Cells[currentRow, 3].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
                Worksheet.Cells[currentRow, 4].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
                Worksheet.Cells[currentRow, 5].Style.Fill.SetBackground(System.Drawing.Color.RosyBrown, ExcelFillStyle.LightGray);
            }

            currentRow++;

            progressBarDoubleValue += (double)90 / checkFormVM.CheckError.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(double.Clamp(progressBarDoubleValue, 0, 100)), "Выгрузка ошибок");
        }

        #endregion

        #region FormatCells

        if (OperatingSystem.IsWindows())
        {
            for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
            {
                Worksheet.Column(col).AutoFit();
                for (var row = 2; row < currentRow; row++)
                {
                    Worksheet.Cells[row, col].Style.WrapText = true;
                    Worksheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Worksheet.Cells[row, col].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                }
            }
            Worksheet.Column(2).Width = 2 * Worksheet.Column(1).Width;
            Worksheet.Column(3).Width = 2 * Worksheet.Column(1).Width;
            Worksheet.Column(4).Width = 11 * Worksheet.Column(1).Width;
            Worksheet.Column(5).Width = 11 * Worksheet.Column(1).Width;
        }

        #endregion

        Worksheet.View.FreezePanes(2, 1);
        return Task.CompletedTask;
    }
}