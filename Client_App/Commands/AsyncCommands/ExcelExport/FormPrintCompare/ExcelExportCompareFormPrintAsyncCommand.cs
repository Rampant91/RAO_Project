using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

/// <summary>
/// Сравнение отчётов исходной БД МПЗФ с выбранным .RAODB (формы 1.1, 1.3, 1.4, 2.12).
/// Только для режима разработчика (внутренний инструмент отдела).
/// </summary>
public class ExcelExportCompareFormPrintAsyncCommand : ExcelExportBaseAllAsyncCommand
{
    private const string WholeDbParameter = "All";
    private readonly MainWindowVM _mainWindowVM;

    public ExcelExportCompareFormPrintAsyncCommand(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;
        _mainWindowVM.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainWindowVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) =>
        IsWholeDbMode(parameter)
        || parameter is Reports
        || parameter is IKeyCollection
        || _mainWindowVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        var wholeDb = IsWholeDbMode(parameter);
        if (!wholeDb && !TryGetReports(parameter, out _))
        {
            if (_mainWindowVM.SelectedReports is null)
            {
                return;
            }
        }

        if (!await ConfirmIntroAsync(wholeDb))
        {
            return;
        }

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

            progressBarVM.SetProgressBar(12, "Копирование файла .RAODB для сравнения", "Сравнение", "Выгрузка в .xlsx");
            tmpEtalonPath = CopyEtalonToTemp(etalonPath);

            progressBarVM.SetProgressBar(22, "Загрузка отчётов исходной БД", "Сравнение", "Выгрузка в .xlsx");
            var source = await FormPrintRaodbIndex.LoadAsync(tmpSourcePath, cts.Token);
            if (!wholeDb)
            {
                source = FilterSourceToSelectedOrg(source, parameter);
            }

            if (source.Reports.Count == 0)
            {
                await ShowInfoAsync(
                    progressBar,
                    wholeDb
                        ? "В исходной БД нет отчётов форм 1.1, 1.3, 1.4 или 2.12 для сравнения."
                        : "У выбранной организации нет отчётов форм 1.1, 1.3, 1.4 или 2.12 для сравнения.");
                await CleanupAndClose(progressBar, tmpSourcePath, tmpEtalonPath);
                return;
            }

            progressBarVM.SetProgressBar(40, "Загрузка отчётов из файла для сравнения", "Сравнение", "Выгрузка в .xlsx");
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

    private async Task<bool> ConfirmIntroAsync(bool wholeDb)
    {
        var scope = wholeDb
            ? "всех организаций открытой базы данных МПЗФ"
            : "выбранной организации в открытой базе данных МПЗФ";

        var message =
            "Сравнение отчётов (внутренний инструмент)." + Environment.NewLine + Environment.NewLine +
            $"Что сравнивается: отчёты форм 1.1, 1.3, 1.4 и 2.12 у {scope}." + Environment.NewLine + Environment.NewLine +
            "С чем: вы выберете файл .RAODB — в нём ищутся те же формы и периоды." + Environment.NewLine + Environment.NewLine +
            "Номер корректировки в ключ сопоставления не входит (показывается в Excel)." + Environment.NewLine +
            "Результат — книга Excel со сводкой и листами отличий." + Environment.NewLine + Environment.NewLine +
            "Продолжить и выбрать файл .RAODB?";

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.YesNo,
                ContentTitle = "Сравнение отчётов",
                ContentHeader = "Что будет сделано",
                ContentMessage = message,
                MinWidth = 520,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        return answer == MessageBox.Avalonia.Enums.ButtonResult.Yes;
    }

    private FormPrintRaodbIndex.LoadResult FilterSourceToSelectedOrg(
        FormPrintRaodbIndex.LoadResult source,
        object? parameter)
    {
        if (!TryGetReports(parameter, out var selected) && _mainWindowVM.SelectedReports is null)
        {
            return source;
        }

        selected ??= _mainWindowVM.SelectedReports!;
        var master = selected.Master_DB;
        if (master is null)
        {
            return source;
        }

        // Берём рег.№/ОКПО так же, как при загрузке индекса.
        var (regNo, okpo) = ReadOrgIdentityFromMaster(master);
        var filtered = source.Reports
            .Where(r =>
                string.Equals(r.RegNo ?? "", regNo, StringComparison.OrdinalIgnoreCase)
                && string.Equals(r.Okpo ?? "", okpo, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return new FormPrintRaodbIndex.LoadResult
        {
            Reports = filtered,
            RegNo = string.IsNullOrEmpty(regNo) ? source.RegNo : regNo,
            Okpo = string.IsNullOrEmpty(okpo) ? source.Okpo : okpo
        };
    }

    private static (string RegNo, string Okpo) ReadOrgIdentityFromMaster(Report master)
    {
        // Дублирует логику FormPrintRaodbIndex.ReadOrgIdentity без доступа к internal helper.
        if (master.FormNum_DB == "1.0" && master.Rows10 is { Count: > 0 })
        {
            var row = PickOrgRow10(master.Rows10);
            return (row?.RegNo_DB ?? "", row?.Okpo_DB ?? "");
        }

        if (master.FormNum_DB == "2.0" && master.Rows20 is { Count: > 0 })
        {
            var row = PickOrgRow20(master.Rows20);
            return (row?.RegNo_DB ?? "", row?.Okpo_DB ?? "");
        }

        return ("", "");
    }

    private static Models.Forms.Form1.Form10? PickOrgRow10(IReadOnlyList<Models.Forms.Form1.Form10> rows)
    {
        if (rows.Count > 1
            && ((!string.IsNullOrEmpty(rows[1].RegNo_DB) || rows[1].Okpo_DB == "-")
                || !string.IsNullOrEmpty(rows[1].Okpo_DB)))
        {
            return rows[1];
        }

        return rows[0];
    }

    private static Models.Forms.Form2.Form20? PickOrgRow20(IReadOnlyList<Models.Forms.Form2.Form20> rows)
    {
        if (rows.Count > 1
            && ((!string.IsNullOrEmpty(rows[1].RegNo_DB) || rows[1].Okpo_DB == "-")
                || !string.IsNullOrEmpty(rows[1].Okpo_DB)))
        {
            return rows[1];
        }

        return rows[0];
    }

    private static bool IsWholeDbMode(object? parameter) =>
        parameter is string s && string.Equals(s, WholeDbParameter, StringComparison.OrdinalIgnoreCase);

    private static bool TryGetReports(object? parameter, out Reports reports)
    {
        switch (parameter)
        {
            case Reports r:
                reports = r;
                return true;
            case IKeyCollection { Count: > 0 } keys when keys.Get<Reports>(0) is { } fromKeys:
                reports = fromKeys;
                return true;
            default:
                reports = null!;
                return false;
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
                Title = "Выберите файл .RAODB для сравнения"
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
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch
        {
            // ignore cleanup errors
        }
    }

    private static async Task ShowInfoAsync(AnyTaskProgressBar? progressBar, string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Сравнение отчётов",
                ContentMessage = message,
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(progressBar ?? Desktop.MainWindow));
    }
}
