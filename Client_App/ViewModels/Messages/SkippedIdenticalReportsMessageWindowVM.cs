using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client_App.ViewModels.Messages;

public sealed class SkippedIdenticalReportsMessageWindowVM
{
    public static string GetHeaderTitle(ImportSummaryFormGroup formGroup) => formGroup switch
    {
        ImportSummaryFormGroup.Form1 => "Импорт отчётов по форме 1.x",
        ImportSummaryFormGroup.Form2 => "Импорт отчётов по форме 2.x",
        ImportSummaryFormGroup.Form3 => "Импорт отчётов по форме 3.x",
        ImportSummaryFormGroup.Form4 => "Импорт отчётов по форме 4.x",
        ImportSummaryFormGroup.Form5 => "Импорт отчётов по форме 5.x",
        _ => "Импорт отчётов"
    };

    public SkippedIdenticalReportsMessageWindowVM()
    {
        FormGroup = ImportSummaryFormGroup.Form1;
        HeaderTitle = GetHeaderTitle(FormGroup);
        ImportedReports = new ObservableCollection<ImportReportSummaryInfo>();
        SkippedReports = new ObservableCollection<ImportReportSummaryInfo>();
    }

    public SkippedIdenticalReportsMessageWindowVM(
        IReadOnlyList<ImportReportSummaryInfo> importedReports,
        IReadOnlyList<ImportReportSummaryInfo> skippedReports,
        ImportSummaryFormGroup formGroup)
    {
        FormGroup = formGroup;
        HeaderTitle = GetHeaderTitle(formGroup);
        ImportedReports = new ObservableCollection<ImportReportSummaryInfo>(SortReports(importedReports, formGroup));
        SkippedReports = new ObservableCollection<ImportReportSummaryInfo>(SortReports(skippedReports, formGroup));
        ImportedCount = importedReports.Count;
        SkippedCount = skippedReports.Count;
    }

    public ImportSummaryFormGroup FormGroup { get; }

    public string HeaderTitle { get; }

    public ObservableCollection<ImportReportSummaryInfo> ImportedReports { get; }

    public ObservableCollection<ImportReportSummaryInfo> SkippedReports { get; }

    public int ImportedCount { get; }

    public int SkippedCount { get; }

    public bool HasImportedReports => ImportedCount > 0;

    public bool HasSkippedReports => SkippedCount > 0;

    public string ImportedSummaryMessage => GetImportedSummaryMessage(ImportedCount);

    public string SkippedSummaryMessage => GetSkippedSummaryMessage(SkippedCount);

    private static IEnumerable<ImportReportSummaryInfo> SortReports(
        IEnumerable<ImportReportSummaryInfo> reports,
        ImportSummaryFormGroup formGroup) =>
        formGroup switch
        {
            ImportSummaryFormGroup.Form1 => reports
                .OrderBy(r => r.OrgColumn1)
                .ThenBy(r => r.OrgColumn2)
                .ThenBy(r => r.FormNum)
                .ThenByDescending(r => ParsePeriod(r.StartPeriod))
                .ThenByDescending(r => ParsePeriod(r.EndPeriod)),
            ImportSummaryFormGroup.Form2 => reports
                .OrderBy(r => r.OrgColumn1)
                .ThenBy(r => r.OrgColumn2)
                .ThenBy(r => r.FormNum)
                .ThenByDescending(r => r.Year),
            ImportSummaryFormGroup.Form3 => reports
                .OrderBy(r => r.OrgColumn1)
                .ThenBy(r => r.OrgColumn2)
                .ThenBy(r => r.FormNum)
                .ThenByDescending(r => ParsePeriod(r.StartPeriod)),
            ImportSummaryFormGroup.Form4 => reports
                .OrderBy(r => r.OrgColumn1)
                .ThenBy(r => r.FormNum)
                .ThenByDescending(r => r.Year),
            ImportSummaryFormGroup.Form5 => reports
                .OrderBy(r => r.OrgColumn1)
                .ThenBy(r => r.FormNum)
                .ThenByDescending(r => r.Year),
            _ => reports
        };

    private static DateOnly ParsePeriod(string period) =>
        DateOnly.TryParse(period, out var date) ? date : DateOnly.MinValue;

    private static int ParseYear(string year) =>
        int.TryParse(year, out var value) ? value : int.MinValue;

    private static string GetImportedSummaryMessage(int count) => count switch
    {
        1 => "1 отчёт успешно импортирован:",
        var n when n % 10 is >= 2 and <= 4 && n % 100 is not (>= 12 and <= 14)
            => $"{n} отчёта успешно импортированы:",
        _ => $"{count} отчётов успешно импортированы:"
    };

    private static string GetSkippedSummaryMessage(int count) => count switch
    {
        1 => "1 отчёт не был импортирован:",
        var n when n % 10 is >= 2 and <= 4 && n % 100 is not (>= 12 and <= 14)
            => $"{n} отчёта не были импортированы:",
        _ => $"{count} отчётов не были импортированы:"
    };
}
