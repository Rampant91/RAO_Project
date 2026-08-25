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
using Models.Interfaces;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

/// <summary>
/// Сравнение отчётов исходной БД МПЗФ с выбранными .RAODB (формы 1 и 2).
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

        var etalonPaths = await AskCompareRaodbPathsAsync();
        if (etalonPaths.Count == 0)
        {
            return;
        }

        var cts = new CancellationTokenSource();
        ExportType = "Сверка_отчётов_с_БД";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        string? tmpSourcePath = null;
        var tmpEtalonPaths = new List<string>();

        try
        {
            progressBarVM.SetProgressBar(5, "Копирование файлов .RAODB для сверки", "Сверка", "Выгрузка в .xlsx");
            var filesCount = etalonPaths.Count;
            for (var copyIndex = 0; copyIndex < etalonPaths.Count; copyIndex++)
            {
                if (filesCount > 1)
                {
                    progressBarVM.SetProgressBar(
                        5,
                        $"Копирование файлов .RAODB для сверки: {copyIndex + 1} из {filesCount}",
                        "Сверка",
                        "Выгрузка в .xlsx");
                }

                tmpEtalonPaths.Add(CopyEtalonToTemp(etalonPaths[copyIndex]));
            }

            // File-first: сначала файлы сверки, затем только нужные организации из БД.
            progressBarVM.SetProgressBar(8, "Загрузка отчётов из файлов для сверки", "Сверка", "Выгрузка в .xlsx");
            var etalonParts = new List<FormPrintRaodbIndex.LoadResult>(tmpEtalonPaths.Count);
            const int filesProgressStart = 8;
            const int filesProgressSpan = 27;
            var lastFilesPercent = filesProgressStart;
            for (var i = 0; i < tmpEtalonPaths.Count; i++)
            {
                cts.Token.ThrowIfCancellationRequested();

                // Абсолютные start/span для LoadAsync; в UI пересчитываем в монотонный % по индексу файла.
                var start = filesProgressStart + (int)Math.Floor(filesProgressSpan * (double)i / Math.Max(1, filesCount));
                var end = filesProgressStart
                          + (int)Math.Floor(filesProgressSpan * (i + 1.0) / Math.Max(1, filesCount));
                var span = Math.Max(1, end - start);
                var filePrefix = filesCount > 1 ? $"Файл {i + 1} из {filesCount}. " : "";

                Action<int, string> fileProgress = (percent, text) =>
                {
                    var local = span <= 1
                        ? 1.0
                        : Math.Clamp((percent - start) / (double)span, 0.0, 1.0);
                    var mapped = (int)Math.Floor(
                        filesProgressStart + filesProgressSpan * (i + local) / Math.Max(1, filesCount));
                    mapped = Math.Min(filesProgressStart + filesProgressSpan, Math.Max(lastFilesPercent, mapped));
                    lastFilesPercent = mapped;
                    progressBarVM.SetProgressBar(mapped, filePrefix + text, "Сверка", "Выгрузка в .xlsx");
                };

                etalonParts.Add(await FormPrintRaodbIndex.LoadAsync(
                    tmpEtalonPaths[i],
                    cts.Token,
                    Path.GetFileName(etalonPaths[i]),
                    fileProgress,
                    progressStartPercent: start,
                    progressSpanPercent: span));
            }

            var etalon = FormPrintCompareCatalog.Merge(etalonParts);
            if (!wholeDb)
            {
                var selectedIdentity = TryGetSelectedOrgIdentity(parameter);
                if (selectedIdentity is { } sel)
                {
                    etalon = FilterReportsToOrg(etalon, sel.RegNo, sel.Okpo);
                }
            }

            var orgKeys = FormPrintCompareCatalog.CollectOrgKeys(etalon.Reports);
            var periodHintsByOrg = FormPrintCompareCatalog.BuildPeriodHintsByOrg(etalon.Reports);
            progressBarVM.SetProgressBar(
                36,
                $"Организаций в файлах сверки: {orgKeys.Count}",
                "Сверка",
                "Выгрузка в .xlsx");

            progressBarVM.SetProgressBar(38, "Создание временной копии исходной БД", "Сверка", "Выгрузка в .xlsx");
            tmpSourcePath = await CreateTempDataBase(progressBar, cts);
            if (string.IsNullOrEmpty(tmpSourcePath) || cts.IsCancellationRequested)
            {
                return;
            }

            progressBarVM.SetProgressBar(
                48,
                "Загрузка отчётов исходной БД (организации и периоды из файлов)",
                "Сверка",
                "Выгрузка в .xlsx");
            Action<int, string> sourceProgress = (percent, text) =>
                progressBarVM.SetProgressBar(percent, text, "Сверка", "Выгрузка в .xlsx");

            var source = await FormPrintRaodbIndex.LoadAsync(
                tmpSourcePath,
                cts.Token,
                "исходная_БД",
                sourceProgress,
                progressStartPercent: 48,
                progressSpanPercent: 27,
                orgFilter: orgKeys,
                periodHintsByOrg: periodHintsByOrg);

            if (!wholeDb)
            {
                source = FilterSourceToSelectedOrg(source, parameter);
            }

            if (source.Reports.Count == 0 && etalon.Reports.Count == 0)
            {
                await ShowInfoAsync(
                    progressBar,
                    wholeDb
                        ? "В исходной БД и в выбранных файлах нет отчётов форм 1 и 2 для сравнения."
                        : "У выбранной организации нет отчётов форм 1 и 2 для сравнения (ни в БД, ни в файлах).");
                await CleanupAndClose(progressBar, tmpSourcePath, tmpEtalonPaths);
                return;
            }

            progressBarVM.SetProgressBar(76, "Сверка отчётов с БД", "Сверка", "Выгрузка в .xlsx");
            Action<int, string> pairingProgress = (percent, text) =>
                progressBarVM.SetProgressBar(percent, text, "Сверка", "Выгрузка в .xlsx");
            var results = FormPrintComparePairing.Pair(
                source.Reports,
                etalon.Reports,
                pairingProgress,
                progressStartPercent: 76,
                progressSpanPercent: 6);

            var exportRegNo = !string.IsNullOrWhiteSpace(etalon.RegNo)
                ? etalon.RegNo
                : source.RegNo;
            var exportOkpo = !string.IsNullOrWhiteSpace(etalon.Okpo)
                ? etalon.Okpo
                : source.Okpo;
            var regNo = RemoveForbiddenChars(
                string.IsNullOrWhiteSpace(exportRegNo) ? "без_рег" : exportRegNo);
            var okpo = RemoveForbiddenChars(
                string.IsNullOrWhiteSpace(exportOkpo) ? "без_окпо" : exportOkpo);
            var fileName = $"{ExportType}_{regNo}_{okpo}_{DateTime.Now:yyyyMMdd_HHmmss}";

            progressBarVM.SetProgressBar(82, "Запрос пути сохранения", "Сверка", "Выгрузка в .xlsx");
            var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);
            if (string.IsNullOrEmpty(fullPath))
            {
                TryDelete(tmpSourcePath);
                TryDeleteAll(tmpEtalonPaths);
                return;
            }

            progressBarVM.SetProgressBar(88, "Инициализация Excel пакета", "Сверка", "Выгрузка в .xlsx");
            using var excelPackage = await InitializeExcelPackage(fullPath);
            var sourceFileName = string.IsNullOrWhiteSpace(StaticConfiguration.DBPath)
                ? "исходная_БД.RAODB"
                : Path.GetFileName(StaticConfiguration.DBPath);
            FormPrintCompareExcel.FillWorkbook(
                excelPackage,
                sourceFileName,
                FormatCompareFileNames(etalonPaths),
                results,
                (percent, text) => progressBarVM.SetProgressBar(percent, text, "Сверка", "Выгрузка в .xlsx"),
                progressStartPercent: 88,
                progressSpanPercent: 7);

            progressBarVM.SetProgressBar(95, "Сохранение", "Сверка", "Выгрузка в .xlsx");
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);
            await CleanupAndClose(progressBar, tmpSourcePath, tmpEtalonPaths);
        }
        catch (OperationCanceledException)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            TryDelete(tmpSourcePath);
            TryDeleteAll(tmpEtalonPaths);
        }
        catch (Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Сверка отчётов с БД",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar!);
            TryDelete(tmpSourcePath);
            TryDeleteAll(tmpEtalonPaths);
        }
    }

    private FormPrintRaodbIndex.LoadResult FilterSourceToSelectedOrg(
        FormPrintRaodbIndex.LoadResult source,
        object? parameter)
    {
        var selectedIdentity = TryGetSelectedOrgIdentity(parameter);
        if (selectedIdentity is null)
        {
            return source;
        }

        var (regNo, okpo) = selectedIdentity.Value;
        var filtered = source.Reports
            .Where(r =>
                string.Equals(
                    ReportMatchKey.NormalizeOrg(r.RegNo),
                    ReportMatchKey.NormalizeOrg(regNo),
                    StringComparison.Ordinal)
                && string.Equals(
                    ReportMatchKey.NormalizeOrg(r.Okpo),
                    ReportMatchKey.NormalizeOrg(okpo),
                    StringComparison.Ordinal))
            .ToList();

        return new FormPrintRaodbIndex.LoadResult
        {
            Reports = filtered,
            RegNo = string.IsNullOrEmpty(regNo) ? source.RegNo : regNo,
            Okpo = string.IsNullOrEmpty(okpo) ? source.Okpo : okpo
        };
    }

    private (string RegNo, string Okpo)? TryGetSelectedOrgIdentity(object? parameter)
    {
        if (!TryGetReports(parameter, out var selected) && _mainWindowVM.SelectedReports is null)
        {
            return null;
        }

        selected ??= _mainWindowVM.SelectedReports!;
        var master = selected.Master_DB;
        if (master is null)
        {
            return null;
        }

        return ReadOrgIdentityFromMaster(master);
    }

    private static FormPrintRaodbIndex.LoadResult FilterReportsToOrg(
        FormPrintRaodbIndex.LoadResult source,
        string regNo,
        string okpo)
    {
        var filtered = source.Reports
            .Where(r =>
                string.Equals(
                    ReportMatchKey.NormalizeOrg(r.RegNo),
                    ReportMatchKey.NormalizeOrg(regNo),
                    StringComparison.Ordinal)
                && string.Equals(
                    ReportMatchKey.NormalizeOrg(r.Okpo),
                    ReportMatchKey.NormalizeOrg(okpo),
                    StringComparison.Ordinal))
            .ToList();

        return new FormPrintRaodbIndex.LoadResult
        {
            Reports = filtered,
            RegNo = regNo,
            Okpo = okpo
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
        IEnumerable<string> tmpEtalonPaths)
    {
        TryDelete(tmpSourcePath);
        TryDeleteAll(tmpEtalonPaths);
        progressBar.AnyTaskProgressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    private static async Task<IReadOnlyList<string>> AskCompareRaodbPathsAsync()
    {
        string[]? files = null;
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var dial = new OpenFileDialog
            {
                AllowMultiple = true,
                Title = "Выберите один или несколько файлов .RAODB для сравнения"
            };
            dial.Filters.Add(new FileDialogFilter
            {
                Name = "RAODB",
                Extensions = { "raodb", "RAODB" }
            });
            files = await dial.ShowAsync(Desktop.MainWindow);
        });

        if (files is not { Length: > 0 })
        {
            return [];
        }

        var readable = files.Where(p => CompareReportSources.ForPath(p) is not null).ToList();
        return readable;
    }

    private static string FormatCompareFileNames(IReadOnlyList<string> paths)
    {
        var names = paths.Select(Path.GetFileName).Where(n => !string.IsNullOrEmpty(n)).Cast<string>().ToList();
        if (names.Count == 0)
        {
            return "";
        }

        if (names.Count <= 8)
        {
            return string.Join("; ", names);
        }

        return $"{names.Count} файлов: {string.Join("; ", names.Take(5))}; …";
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

    private static void TryDeleteAll(IEnumerable<string> paths)
    {
        foreach (var path in paths)
        {
            TryDelete(path);
        }
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
                ContentTitle = "Сверка отчётов с БД",
                ContentMessage = message,
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(progressBar ?? Desktop.MainWindow));
    }
}
