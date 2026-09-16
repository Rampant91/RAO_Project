using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.RaodbExport;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Дополнительно → конвертер аналитической выгрузки всех форм в отдельные .RAODB.
/// Форма определяется по листу/заголовкам; активную БД не изменяет.
/// Поддерживается разбор форм 1.1–1.9 и 2.1–2.12.
/// </summary>
public class ConvertFormsExcelToRaodbAsyncCommand : ExportRaodbBaseAsyncCommand
{
    private const string DialogTitle = "Конвертер выгрузки всех форм → .RAODB";

    #region AsyncExecute

    public override async Task AsyncExecute(object? parameter)
    {
        if (!await ShowIntroMessageAsync())
        {
            return;
        }

        var excelPath = await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var openDialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "Выберите файл аналитической выгрузки форм (.xlsx)",
                Filters =
                [
                    new FileDialogFilter { Name = "Excel", Extensions = ["xlsx", "XLSX"] }
                ]
            };
            var files = await openDialog.ShowAsync(Desktop.MainWindow);
            return files is { Length: > 0 } ? files[0] : null;
        });
        if (string.IsNullOrWhiteSpace(excelPath)) return;

        FormsExcelWorkbookDetectResult detection;
        List<(string FormNum, string SheetName, string? NotesName, DetectionSource Source)> detectedSummary;
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(excelPath));
            detection = FormsExcelWorkbookDetector.Detect(package.Workbook);
            detectedSummary = detection.Sheets
                .Select(s => (
                    s.Spec.FormNum,
                    s.ReportsSheet.Name,
                    s.NotesSheet?.Name,
                    s.Source))
                .ToList();
        }
        catch (Exception ex)
        {
            ServiceExtension.LoggerManager.Error($"ConvertFormsExcelToRaodb: чтение .xlsx: {ex}");
            await ShowErrorAsync("Не удалось прочитать файл .xlsx.", ex.Message);
            return;
        }

        if (detectedSummary.Count == 0)
        {
            var details = detection.Notes.Count > 0
                ? string.Join(Environment.NewLine, detection.Notes.Take(15))
                : "Ожидаются листы «Отчеты X.Y» с заголовками, как в аналитической выгрузке.";
            await ShowErrorAsync("В файле не распознано ни одной поддерживаемой формы.", details);
            return;
        }

        var confirmMessage = BuildConfirmMessage(detectedSummary, detection.Notes);
        var confirmed = await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var answer = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Продолжить" },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ],
                    CanResize = true,
                    ContentTitle = DialogTitle,
                    ContentHeader = "Распознанные формы",
                    ContentMessage = confirmMessage,
                    MinWidth = 420,
                    MinHeight = 160,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowDialog(Desktop.MainWindow);
            return answer == "Продолжить";
        });
        if (!confirmed) return;

        if (StaticConfiguration.DBModel is null)
        {
            await ShowErrorAsync(
                "Не открыта база данных.",
                "Для заполнения формы 1.0 / 2.0 откройте базу данных с нужными организациями и повторите.");
            return;
        }

        var folderPath = await Dispatcher.UIThread.InvokeAsync(async () =>
            await new OpenFolderDialog { Title = "Папка для сохранения .RAODB" }
                .ShowAsync(Desktop.MainWindow));
        if (string.IsNullOrEmpty(folderPath)) return;

        var cts = new CancellationTokenSource();
        await Dispatcher.UIThread.InvokeAsync(() => ProgressBar = new AnyTaskProgressBar(cts));
        var progressBar = ProgressBar;
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        progressBarVM.ExportType = "Конвертация_Excel_в_RAODB";
        progressBarVM.ExportName = "Выгрузка форм → .RAODB";
        progressBarVM.ValueBar = 5;
        progressBarVM.LoadStatus = "5% (Разбор Excel)";

        var exportedCount = 0;
        var totalGroups = 0;
        var errors = new List<string>();
        errors.AddRange(detection.Notes);

        try
        {
            await Task.Run(async () =>
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(new FileInfo(excelPath));
                // Повторный детект на живом package (листы из первого Detect нельзя использовать после Dispose).
                var liveDetection = FormsExcelWorkbookDetector.Detect(package.Workbook);
                errors.Clear();
                errors.AddRange(liveDetection.Notes);

                progressBarVM.SetProgressBar(20, "Группировка строк");
                progressBarVM.SetProgressBar(30, "Загрузка организаций из текущей БД");

                var db = StaticConfiguration.DBModel
                    ?? throw new InvalidOperationException("Активная база данных не открыта.");

                FormsExcelOrgLookup? form10Lookup = null;
                FormsExcelOrgLookup? form20Lookup = null;
                if (liveDetection.Sheets.Any(s => s.Spec.Family == FormsExcelFormFamily.Form1))
                {
                    var organizations = await FormsExcelOrgResolver.LoadForm10OrganizationsAsync(db, cts.Token);
                    form10Lookup = FormsExcelOrgResolver.BuildForm10Lookup(organizations);
                }

                if (liveDetection.Sheets.Any(s => s.Spec.Family == FormsExcelFormFamily.Form2))
                {
                    var organizations = await FormsExcelOrgResolver.LoadForm20OrganizationsAsync(db, cts.Token);
                    form20Lookup = FormsExcelOrgResolver.BuildForm20Lookup(organizations);
                }

                progressBarVM.SetProgressBar(35, "Разбор листов Excel");

                var form1Jobs = new List<(FormsExcelDetectedSheet Sheet, FormsExcelForm1ReportGroup Group)>();
                var form2Jobs = new List<(FormsExcelDetectedSheet Sheet, FormsExcelForm2ReportGroup Group)>();

                foreach (var sheet in liveDetection.Sheets)
                {
                    cts.Token.ThrowIfCancellationRequested();

                    try
                    {
                        if (sheet.Spec.Family == FormsExcelFormFamily.Form1)
                        {
                            var parseResult = FormsExcelExportParserForm1.ParseDetectedSheet(sheet);
                            errors.AddRange(parseResult.Warnings.Select(w => $"[{sheet.Spec.FormNum}] {w}"));
                            foreach (var group in parseResult.Groups)
                            {
                                form1Jobs.Add((sheet, group));
                            }
                        }
                        else if (sheet.Spec.Family == FormsExcelFormFamily.Form2)
                        {
                            var parseResult = FormsExcelExportParserForm2.ParseDetectedSheet(sheet);
                            errors.AddRange(parseResult.Warnings.Select(w => $"[{sheet.Spec.FormNum}] {w}"));
                            foreach (var group in parseResult.Groups)
                            {
                                form2Jobs.Add((sheet, group));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var sheetName = sheet.ReportsSheet.Name;
                        errors.Add($"[{sheet.Spec.FormNum}] ошибка разбора листа «{sheetName}»: {ex.Message}");
                        ServiceExtension.LoggerManager.Error(
                            $"ConvertFormsExcelToRaodb: разбор «{sheetName}»: {ex}");
                    }
                }

                totalGroups = form1Jobs.Count + form2Jobs.Count;
                progressBarVM.SetProgressBar(40, $"Найдено отчётов: {totalGroups}");

                var processedReports = 0;
                foreach (var (sheet, group) in form1Jobs)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    processedReports++;
                    UpdateExportProgress(progressBarVM, processedReports, totalGroups, sheet.Spec.FormNum, group.Key.ToString());
                    if (await TryWriteForm1ReportAsync(group, form10Lookup, folderPath, errors, cts.Token))
                    {
                        exportedCount++;
                    }
                }

                foreach (var (sheet, group) in form2Jobs)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    processedReports++;
                    UpdateExportProgress(progressBarVM, processedReports, totalGroups, sheet.Spec.FormNum, group.Key.ToString());
                    if (await TryWriteForm2ReportAsync(group, form20Lookup, folderPath, errors, cts.Token))
                    {
                        exportedCount++;
                    }
                }

                progressBarVM.SetProgressBar(95, "Завершение");
            }, cts.Token);

            await progressBar.CloseAsync();
            await ShowSummaryAsync(exportedCount, totalGroups, errors);
        }
        catch (OperationCanceledException)
        {
            await progressBar.CloseAsync();
        }
        catch (Exception ex)
        {
            ServiceExtension.LoggerManager.Error($"ConvertFormsExcelToRaodb: {ex}");
            await progressBar.CloseAsync();
            await ShowErrorAsync("Ошибка конвертации.", ex.Message);
        }
    }

    #endregion

    #region Report export

    private static async Task<bool> TryWriteForm1ReportAsync(
        FormsExcelForm1ReportGroup group,
        FormsExcelOrgLookup? lookup,
        string folderPath,
        List<string> errors,
        CancellationToken ct)
    {
        if (lookup is null)
        {
            errors.Add("Словарь организаций Form 1.0 не загружен.");
            return false;
        }

        var status = lookup.TryResolve(group.Key.RegNo, group.Key.Okpo, out var organization);
        if (status == FormsExcelOrgResolveStatus.NotFound)
        {
            errors.Add($"{group.FormNum} {group.Key}: организация не найдена в текущей БД.");
            return false;
        }

        if (status == FormsExcelOrgResolveStatus.Ambiguous)
        {
            errors.Add($"{group.FormNum} {group.Key}: несколько организаций с одной парой рег.№/ОКПО.");
            return false;
        }

        try
        {
            var clonedMaster = FormsExcelMasterClone.CloneForm10Master(
                organization!.Master_DB,
                out var masterWarnings);
            foreach (var warning in masterWarnings)
            {
                errors.Add($"{group.Key}: {warning}");
            }

            var orgWithReport = FormsExcelRaodbWriter.BuildOrganizationWithReport(clonedMaster, group);
            var report = orgWithReport.Report_Collection.First();
            var fullPath = FormsExcelRaodbWriter.InsertIndexInFilePath(
                Path.Combine(folderPath, $"{FormsExcelRaodbWriter.BuildFileName(orgWithReport, report)}.RAODB"));
            await FormsExcelRaodbWriter.WriteRaodbAsync(orgWithReport, fullPath, ct);
            return true;
        }
        catch (Exception ex)
        {
            errors.Add($"{group.FormNum} {group.Key}: {ex.Message}");
            ServiceExtension.LoggerManager.Error(
                $"ConvertFormsExcelToRaodb: {group.FormNum} {group.Key}: {ex}");
            return false;
        }
    }

    private static async Task<bool> TryWriteForm2ReportAsync(
        FormsExcelForm2ReportGroup group,
        FormsExcelOrgLookup? lookup,
        string folderPath,
        List<string> errors,
        CancellationToken ct)
    {
        if (lookup is null)
        {
            errors.Add("Словарь организаций Form 2.0 не загружен.");
            return false;
        }

        var status = lookup.TryResolve(group.Key.RegNo, group.Key.Okpo, out var organization);
        if (status == FormsExcelOrgResolveStatus.NotFound)
        {
            errors.Add($"{group.FormNum} {group.Key}: организация не найдена в текущей БД.");
            return false;
        }

        if (status == FormsExcelOrgResolveStatus.Ambiguous)
        {
            errors.Add($"{group.FormNum} {group.Key}: несколько организаций с одной парой рег.№/ОКПО.");
            return false;
        }

        try
        {
            var clonedMaster = FormsExcelMasterClone.CloneForm20Master(
                organization!.Master_DB,
                out var masterWarnings);
            foreach (var warning in masterWarnings)
            {
                errors.Add($"{group.Key}: {warning}");
            }

            var orgWithReport = FormsExcelRaodbWriter.BuildOrganizationWithReport(clonedMaster, group);
            var report = orgWithReport.Report_Collection.First();
            var fullPath = FormsExcelRaodbWriter.InsertIndexInFilePath(
                Path.Combine(folderPath, $"{FormsExcelRaodbWriter.BuildFileName(orgWithReport, report)}.RAODB"));
            await FormsExcelRaodbWriter.WriteRaodbAsync(orgWithReport, fullPath, ct);
            return true;
        }
        catch (Exception ex)
        {
            errors.Add($"{group.FormNum} {group.Key}: {ex.Message}");
            ServiceExtension.LoggerManager.Error(
                $"ConvertFormsExcelToRaodb: {group.FormNum} {group.Key}: {ex}");
            return false;
        }
    }

    #endregion

    #region Dialogs

    private static async Task<bool> ShowIntroMessageAsync()
    {
        var message =
            "Выберите файл аналитической выгрузки всех форм определённого номера (1.X или 2.X). " +
            "Программа преобразует табличные данные из Excel в отдельные файлы отчётов (.RAODB)." +
            Environment.NewLine + Environment.NewLine +
            "Для заполнения формы 1.0 (или 2.0) в каждом файле организация должна присутствовать " +
            "в текущей открытой базе данных." +
            Environment.NewLine + Environment.NewLine +
            "Данные исполнителя в создаваемых файлах отчётов отсутствуют — " +
            "при необходимости их нужно заполнить отдельно.";

        var answer = await Dispatcher.UIThread.InvokeAsync(async () =>
            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Продолжить", IsDefault = true },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ],
                    CanResize = true,
                    ContentTitle = DialogTitle,
                    ContentHeader = "Информация",
                    ContentMessage = message,
                    MinWidth = 420,
                    MinHeight = 180,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowDialog(Desktop.MainWindow));

        return answer == "Продолжить";
    }

    private static string BuildConfirmMessage(
        IReadOnlyList<(string FormNum, string SheetName, string? NotesName, DetectionSource Source)> sheets,
        IReadOnlyList<string> detectNotes)
    {
        var lines = sheets.Select(s =>
        {
            var via = s.Source == DetectionSource.SheetName ? "по имени листа" : "по заголовкам";
            var notes = s.NotesName is null ? "без примечаний" : $"примечания: «{s.NotesName}»";
            return $"• Форма {s.FormNum} — лист «{s.SheetName}» ({via}, {notes})";
        });
        var message = "Будут обработаны:" + Environment.NewLine + string.Join(Environment.NewLine, lines);
        if (detectNotes.Count > 0)
        {
            message += Environment.NewLine + Environment.NewLine + "Замечания при распознавании:" +
                       Environment.NewLine + string.Join(Environment.NewLine, detectNotes.Take(10));
        }

        return message;
    }

    private static async Task ShowErrorAsync(string header, string? details)
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = DialogTitle,
                ContentHeader = header,
                ContentMessage = details ?? "",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));
    }

    private static async Task ShowSummaryAsync(int exported, int totalGroups, List<string> errors)
    {
        var message = $"Создано файлов .RAODB: {exported} из {totalGroups} отчётов.";
        if (errors.Count > 0)
        {
            var preview = string.Join(Environment.NewLine, errors.Take(30));
            if (errors.Count > 30)
            {
                preview += $"{Environment.NewLine}… и ещё {errors.Count - 30}";
            }

            message += $"{Environment.NewLine}{Environment.NewLine}Замечания / ошибки ({errors.Count}):" +
                       $"{Environment.NewLine}{preview}";
        }

        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = DialogTitle,
                ContentHeader = exported > 0 ? "Готово" : "Не создано ни одного файла",
                ContentMessage = message,
                MinWidth = 450,
                MinHeight = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));
    }

    #endregion

    #region Export progress

    private static void UpdateExportProgress(
        AnyTaskProgressBarVM progressBarVM,
        int processedReports,
        int totalReports,
        string formNum,
        string reportKey)
    {
        var progress = 40 + (int)(55.0 * processedReports / Math.Max(1, totalReports));
        progressBarVM.SetProgressBar(
            progress,
            $"Форма {formNum}: отчёт {processedReports} из {totalReports}{Environment.NewLine}{reportKey}");
    }

    #endregion
}
