using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Services;

/// <summary>
/// Решение пользователя в диалоге напоминания о номере корректировки при выгрузке.
/// </summary>
public enum ReportExportSnapshotDecision
{
    RaiseAndExport,
    ExportAsIs,
    Cancel
}

/// <summary>
/// Слепок выгрузки отчёта: оценка необходимости диалога, UI-prompt, запись/импорт/догон.
/// </summary>
public static class ReportExportSnapshotService
{
    private static IClassicDesktopStyleApplicationLifetime Desktop =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

    /// <summary>
    /// Нужен ли диалог: тот же N, что при последнем слепке, и содержимое изменилось.
    /// </summary>
    public static bool Evaluate(Report report)
    {
        ArgumentNullException.ThrowIfNull(report);

        if (report.LastExportedCorrectionNumber_DB is not { } lastN)
        {
            return false;
        }

        if (report.CorrectionNumber_DB != lastN)
        {
            return false;
        }

        if (string.IsNullOrEmpty(report.LastExportedFingerprint_DB))
        {
            return false;
        }

        var current = ReportContentFingerprint.Compute(report);
        return !string.Equals(current, report.LastExportedFingerprint_DB, StringComparison.Ordinal);
    }

    /// <summary>
    /// Единый диалог для одного отчёта.
    /// </summary>
    public static Task<ReportExportSnapshotDecision> PromptAsync(Report report)
    {
        ArgumentNullException.ThrowIfNull(report);
        var n = report.CorrectionNumber_DB;
        var exportDate = string.IsNullOrWhiteSpace(report.ExportDate_DB) ? "—" : report.ExportDate_DB;
        var message =
            $"Отчёт ранее уже выгружался с номером корректировки **{n}**, " +
            $"при этом его содержимое изменилось (дата прошлой выгрузки: {exportDate})." +
            $"{Environment.NewLine}{Environment.NewLine}" +
            BuildRulesMarkdown(exportDate);

        return ShowDecisionDialogAsync(
            message,
            raiseButton: $"Повысить номер корректировки до {n + 1}, сохранить и выгрузить",
            keepButton: $"Выгрузить с номером корректировки {n}");
    }

    /// <summary>
    /// Сводный диалог для пакетной выгрузки (один раз на все отчёты, требующие предупреждения).
    /// </summary>
    public static Task<ReportExportSnapshotDecision> PromptAsync(IReadOnlyList<Report> reportsNeedingWarning)
    {
        ArgumentNullException.ThrowIfNull(reportsNeedingWarning);
        if (reportsNeedingWarning.Count == 0)
        {
            return Task.FromResult(ReportExportSnapshotDecision.ExportAsIs);
        }

        if (reportsNeedingWarning.Count == 1)
        {
            return PromptAsync(reportsNeedingWarning[0]);
        }

        var sb = new StringBuilder();
        sb.AppendLine("Следующие отчёты уже выгружались с тем же номером корректировки, но содержимое изменилось:");
        sb.AppendLine();
        foreach (var report in reportsNeedingWarning)
        {
            var exportDate = string.IsNullOrWhiteSpace(report.ExportDate_DB) ? "—" : report.ExportDate_DB;
            sb.Append("- ");
            sb.Append(DescribeReport(report));
            sb.Append(" (N=");
            sb.Append(report.CorrectionNumber_DB);
            sb.Append(", дата выгрузки: ");
            sb.Append(exportDate);
            sb.Append(')');
            sb.AppendLine();
        }

        sb.AppendLine();
        // В списке выше уже есть даты по отчётам — в правилах дату не дублируем.
        sb.Append(BuildRulesMarkdown(exportDate: null));

        return ShowDecisionDialogAsync(
            sb.ToString(),
            raiseButton: "Повысить номера корректировки, сохранить и выгрузить",
            keepButton: "Выгрузить с текущими номерами корректировки");
    }

    /// <summary>
    /// После успешного File.Copy: обновить слепок на tracked-отчёте (ExportDate уже выставляет вызывающий).
    /// </summary>
    public static void RecordSuccessfulExport(Report tracked)
    {
        ArgumentNullException.ThrowIfNull(tracked);
        tracked.LastExportedCorrectionNumber_DB = tracked.CorrectionNumber_DB;
        tracked.LastExportedFingerprint_DB = ReportContentFingerprint.Compute(tracked);
    }

    /// <summary>
    /// Записать в копию для .RAODB слепок текущей выгрузки (N + fingerprint).
    /// </summary>
    public static void WriteSnapshotOntoExportCopy(Report exportReport)
    {
        ArgumentNullException.ThrowIfNull(exportReport);
        exportReport.LastExportedCorrectionNumber_DB = exportReport.CorrectionNumber_DB;
        exportReport.LastExportedFingerprint_DB = ReportContentFingerprint.Compute(exportReport);
    }

    /// <summary>
    /// При импорте: если fingerprint из файла пуст — Compute и LastExported = N; иначе оставить поля файла.
    /// </summary>
    public static void ApplyFromImport(Report report)
    {
        ArgumentNullException.ThrowIfNull(report);

        if (!string.IsNullOrEmpty(report.LastExportedFingerprint_DB))
        {
            return;
        }

        report.LastExportedCorrectionNumber_DB = report.CorrectionNumber_DB;
        report.LastExportedFingerprint_DB = ReportContentFingerprint.Compute(report);
    }

    /// <summary>
    /// Догон слепка при открытии: fingerprint пуст и уже был ExportDate или LastExported.
    /// Черновики (нет ExportDate и нет LastExported) пропускаются.
    /// </summary>
    public static async Task EnsureSnapshotOnOpenAsync(Report report)
    {
        ArgumentNullException.ThrowIfNull(report);

        if (!string.IsNullOrEmpty(report.LastExportedFingerprint_DB))
        {
            return;
        }

        var hasExportDate = !string.IsNullOrWhiteSpace(report.ExportDate_DB);
        var hasLastExported = report.LastExportedCorrectionNumber_DB.HasValue;
        if (!hasExportDate && !hasLastExported)
        {
            return;
        }

        if (!hasLastExported)
        {
            report.LastExportedCorrectionNumber_DB = report.CorrectionNumber_DB;
        }

        report.LastExportedFingerprint_DB = ReportContentFingerprint.Compute(report);
        await StaticConfiguration.DBModel.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Блок правил для Markdown-диалога. Дата — только если одна и осмысленная;
    /// иначе формулировка «ранее выгруженный отчёт».
    /// </summary>
    private static string BuildRulesMarkdown(string? exportDate)
    {
        var exportedPhrase = string.IsNullOrWhiteSpace(exportDate) || exportDate == "—"
            ? "ранее выгруженный отчёт"
            : $"отчёт, выгруженный {exportDate},";

        return
            "**Правила изменения номера корректировки:**" + Environment.NewLine + Environment.NewLine +
            $"- **Для организации:** Если {exportedPhrase} был официально направлен в ИАЦ, " +
            $"необходимо изменить номер корректировки." +
            Environment.NewLine + Environment.NewLine +
            $"- **Для ИАЦ:** Если {exportedPhrase} был официально направлен в ЦИАЦ, " +
            "необходимо направить скорректированный отчёт " +
            "в организацию для повторной отправки в ИАЦ с изменённым номером корректировки.";
    }

    private static string DescribeReport(Report report)
    {
        if (!string.IsNullOrEmpty(report.FormNum_DB) && report.FormNum_DB.StartsWith('1'))
        {
            return $"{report.FormNum_DB} {report.StartPeriod_DB}–{report.EndPeriod_DB}";
        }

        if (!string.IsNullOrEmpty(report.FormNum_DB) &&
            (report.FormNum_DB.StartsWith('2') || report.FormNum_DB.StartsWith('4') || report.FormNum_DB.StartsWith('5')))
        {
            return $"{report.FormNum_DB} год {report.Year_DB}";
        }

        return report.FormNum_DB ?? "отчёт";
    }

    private static async Task<ReportExportSnapshotDecision> ShowDecisionDialogAsync(
        string message,
        string raiseButton,
        string keepButton)
    {
        var answer = await Dispatcher.UIThread.InvokeAsync(async () =>
            await MessageBoxManager
                .GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = raiseButton },
                        new ButtonDefinition { Name = keepButton },
                        new ButtonDefinition { Name = "\u041e\u0442\u043c\u0435\u043d\u0430", IsCancel = true }
                    ],
                    CanResize = true,
                    ContentTitle = "\u0412\u044b\u0433\u0440\u0443\u0437\u043a\u0430 \u0432 .raodb",
                    ContentHeader = "\u041d\u043e\u043c\u0435\u0440 \u043a\u043e\u0440\u0440\u0435\u043a\u0442\u0438\u0440\u043e\u0432\u043a\u0438",
                    ContentMessage = message,
                    Markdown = true,
                    MinWidth = 500,
                    MaxWidth = 1000,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true
                })
                .ShowWindowDialogAsync(Desktop.MainWindow));
        if (string.Equals(answer, raiseButton, StringComparison.Ordinal))
        {
            return ReportExportSnapshotDecision.RaiseAndExport;
        }

        if (string.Equals(answer, keepButton, StringComparison.Ordinal))
        {
            return ReportExportSnapshotDecision.ExportAsIs;
        }

        return ReportExportSnapshotDecision.Cancel;
    }
}
