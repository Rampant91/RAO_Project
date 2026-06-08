using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client_App.ViewModels.Messages;

public sealed class SkippedIdenticalReportsMessageWindowVM
{
    public const string Form1HeaderTitle = "Импорт отчётов по форме 1.x";

    public SkippedIdenticalReportsMessageWindowVM()
    {
        HeaderTitle = Form1HeaderTitle;
        ImportedReports = new ObservableCollection<SkippedIdenticalReportInfo>();
        SkippedReports = new ObservableCollection<SkippedIdenticalReportInfo>();
    }

    public SkippedIdenticalReportsMessageWindowVM(
        IReadOnlyList<SkippedIdenticalReportInfo> importedReports,
        IReadOnlyList<SkippedIdenticalReportInfo> skippedReports,
        string headerTitle = Form1HeaderTitle)
    {
        HeaderTitle = headerTitle;
        ImportedReports = new ObservableCollection<SkippedIdenticalReportInfo>(SortReports(importedReports));
        SkippedReports = new ObservableCollection<SkippedIdenticalReportInfo>(SortReports(skippedReports));
        ImportedCount = importedReports.Count;
        SkippedCount = skippedReports.Count;
    }

    public string HeaderTitle { get; }

    public ObservableCollection<SkippedIdenticalReportInfo> ImportedReports { get; }

    public ObservableCollection<SkippedIdenticalReportInfo> SkippedReports { get; }

    public int ImportedCount { get; }

    public int SkippedCount { get; }

    public bool HasImportedReports => ImportedCount > 0;

    public bool HasSkippedReports => SkippedCount > 0;

    public string ImportedSummaryMessage => GetImportedSummaryMessage(ImportedCount);

    public string SkippedSummaryMessage => GetSkippedSummaryMessage(SkippedCount);

    private static IEnumerable<SkippedIdenticalReportInfo> SortReports(IEnumerable<SkippedIdenticalReportInfo> reports) =>
        reports
            .OrderBy(r => r.RegNum)
            .ThenBy(r => r.Okpo)
            .ThenBy(r => r.FormNum)
            .ThenByDescending(r => ParsePeriod(r.StartPeriod))
            .ThenByDescending(r => ParsePeriod(r.EndPeriod));

    private static DateOnly ParsePeriod(string period) =>
        DateOnly.TryParse(period, out var date) ? date : DateOnly.MinValue;

    private static string GetImportedSummaryMessage(int count) => count switch
    {
        1 => "1 отчёт успешно импортирован:",
        var n when n % 10 is >= 2 and <= 4 && n % 100 is not (>= 12 and <= 14)
            => $"{n} отчёта успешно импортированы:",
        _ => $"{count} отчётов успешно импортированы:"
    };

    private static string GetSkippedSummaryMessage(int count) => count switch
    {
        1 => "1 отчёт уже имелся в базе в виде полной копии и не был импортирован:",
        var n when n % 10 is >= 2 and <= 4 && n % 100 is not (>= 12 and <= 14)
            => $"{n} отчёта уже имелись в базе в виде полной копии и не были импортированы:",
        _ => $"{count} отчётов уже имелись в базе в виде полной копии и не были импортированы:"
    };
}
