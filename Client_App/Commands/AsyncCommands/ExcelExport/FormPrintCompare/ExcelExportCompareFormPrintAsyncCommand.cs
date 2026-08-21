using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

/// <summary>
/// Сравнение отчётов исходной БД МПЗФ с выбранным эталонным .RAODB (формы 1.1, 1.3, 1.4, 2.12).
/// Оркестрация по образцу Pairing41 / TransferReceive: temp-БД, прогресс-бар, InitializeExcelPackage.
/// </summary>
public class ExcelExportCompareFormPrintAsyncCommand : ExcelExportBaseAllAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var etalonPath = await AskEtalonRaodbPathAsync();
        if (string.IsNullOrWhiteSpace(etalonPath))
        {
            return;
        }

        var cts = new CancellationTokenSource();
        ExportType = "Сравнение_отчётов";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        string? tmpSourcePath = null;
        string? tmpEtalonPath = null;

        try
        {
            progressBarVM.SetProgressBar(5, "Создание временной копии исходной БД", "Сравнение", "Выгрузка в .xlsx");
            tmpSourcePath = await CreateTempDataBase(progressBar, cts);
            if (string.IsNullOrEmpty(tmpSourcePath) || cts.IsCancellationRequested)
            {
                return;
            }

            progressBarVM.SetProgressBar(12, "Копирование эталонного .RAODB", "Сравнение", "Выгрузка в .xlsx");
            tmpEtalonPath = CopyEtalonToTemp(etalonPath);

            progressBarVM.SetProgressBar(22, "Загрузка отчётов исходной БД", "Сравнение", "Выгрузка в .xlsx");
            var source = await FormPrintRaodbIndex.LoadAsync(tmpSourcePath, cts.Token);
            if (source.Reports.Count == 0)
            {
                await ShowInfoAsync(
                    progressBar,
                    "В исходной БД нет отчётов форм 1.1, 1.3, 1.4 или 2.12 для сравнения.");
                await CleanupAndClose(progressBar, tmpSourcePath, tmpEtalonPath);
                return;
            }

            progressBarVM.SetProgressBar(40, "Загрузка отчётов эталона", "Сравнение", "Выгрузка в .xlsx");
            var etalon = await FormPrintRaodbIndex.LoadAsync(tmpEtalonPath, cts.Token);
            var etalonIndex = FormPrintRaodbIndex.ToIndex(etalon.Reports);

            progressBarVM.SetProgressBar(55, "Сравнение отчётов", "Сравнение", "Выгрузка в .xlsx");
            var results = new List<ReportCompareResult>(source.Reports.Count);
            var total = source.Reports.Count;
            for (var i = 0; i < total; i++)
            {
                cts.Token.ThrowIfCancellationRequested();
                var left = source.Reports[i];
                etalonIndex.TryGetValue(new ReportMatchKey(left.FormNum, left.PeriodKey), out var right);
                results.Add(FormPrintCompareMatcher.Compare(left, right));

                var percent = 55 + (25 * (i + 1) / Math.Max(total, 1));
                progressBarVM.SetProgressBar(
                    percent,
                    $"Сравнение отчётов ({i + 1}/{total})",
                    "Сравнение");
            }

            var regNo = RemoveForbiddenChars(
                string.IsNullOrWhiteSpace(source.RegNo) ? "без_рег" : source.RegNo);
            var okpo = RemoveForbiddenChars(
                string.IsNullOrWhiteSpace(source.Okpo) ? "без_окпо" : source.Okpo);
            var fileName = $"{ExportType}_{regNo}_{okpo}_{DateTime.Now:yyyyMMdd_HHmmss}";

            progressBarVM.SetProgressBar(82, "Запрос пути сохранения", "Сравнение", "Выгрузка в .xlsx");
            var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);
            if (string.IsNullOrEmpty(fullPath))
            {
                TryDelete(tmpSourcePath);
                TryDelete(tmpEtalonPath);
                return;
            }

            progressBarVM.SetProgressBar(88, "Инициализация Excel пакета", "Сравнение", "Выгрузка в .xlsx");
            using var excelPackage = await InitializeExcelPackage(fullPath);
            var sourceFileName = string.IsNullOrWhiteSpace(StaticConfiguration.DBPath)
                ? "исходная_БД.RAODB"
                : Path.GetFileName(StaticConfiguration.DBPath);
            FormPrintCompareExcel.FillWorkbook(
                excelPackage,
                source.RegNo,
                source.Okpo,
                sourceFileName,
                Path.GetFileName(etalonPath),
                results);

            progressBarVM.SetProgressBar(95, "Сохранение", "Сравнение", "Выгрузка в .xlsx");
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);
            await CleanupAndClose(progressBar, tmpSourcePath, tmpEtalonPath);
        }
        catch (OperationCanceledException)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            TryDelete(tmpSourcePath);
            TryDelete(tmpEtalonPath);
        }
        catch (Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Сравнение отчётов",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar!);
            TryDelete(tmpSourcePath);
            TryDelete(tmpEtalonPath);
        }
    }

    private static async Task CleanupAndClose(
        AnyTaskProgressBar progressBar,
        string? tmpSourcePath,
        string? tmpEtalonPath)
    {
        TryDelete(tmpSourcePath);
        TryDelete(tmpEtalonPath);
        progressBar.AnyTaskProgressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    private static async Task<string?> AskEtalonRaodbPathAsync()
    {
        string[]? files = null;
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var dial = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "Выберите эталонный файл .RAODB для сверки"
            };
            dial.Filters.Add(new FileDialogFilter
            {
                Name = "RAODB",
                Extensions = { "raodb", "RAODB" }
            });
            files = await dial.ShowAsync(Desktop.MainWindow);
        });

        return files is { Length: > 0 } ? files[0] : null;
    }

    private static string CopyEtalonToTemp(string etalonPath)
    {
        var index = 0;
        var tmpPath = Path.Combine(BaseVM.TmpDirectory, $"etalon_compare_{index}.RAODB");
        while (File.Exists(tmpPath))
        {
            tmpPath = Path.Combine(BaseVM.TmpDirectory, $"etalon_compare_{++index}.RAODB");
        }

        File.Copy(etalonPath, tmpPath, overwrite: true);
        return tmpPath;
    }

    private static void TryDelete(string? path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            return;
        }

        try
        {
            File.Delete(path);
        }
        catch
        {
            // ignored — как в Pairing41 CleanupAndClose
        }
    }

    private static async Task ShowInfoAsync(AnyTaskProgressBar progressBar, string message) =>
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Сравнение отчётов",
                ContentHeader = "Уведомление",
                ContentMessage = message,
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true
            })
            .ShowDialog(progressBar ?? Desktop.MainWindow));
}
