using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client_App.ViewModels.Messages;

public sealed class SkippedIdenticalReportsMessageWindowVM
{
    public SkippedIdenticalReportsMessageWindowVM() =>
        Reports = new ObservableCollection<SkippedIdenticalReportInfo>();

    public SkippedIdenticalReportsMessageWindowVM(IReadOnlyList<SkippedIdenticalReportInfo> reports)
    {
        Reports = new ObservableCollection<SkippedIdenticalReportInfo>(SortReports(reports));
        Count = reports.Count;
    }

    private static IEnumerable<SkippedIdenticalReportInfo> SortReports(IEnumerable<SkippedIdenticalReportInfo> reports) =>
        reports
            .OrderBy(r => r.RegNum)
            .ThenBy(r => r.Okpo)
            .ThenBy(r => r.FormNum)
            .ThenByDescending(r => ParsePeriod(r.StartPeriod))
            .ThenByDescending(r => ParsePeriod(r.EndPeriod));

    private static DateOnly ParsePeriod(string period) =>
        DateOnly.TryParse(period, out var date) ? date : DateOnly.MinValue;

    public ObservableCollection<SkippedIdenticalReportInfo> Reports { get; }

    public int Count { get; }

    public string SummaryMessage => GetSummaryMessage(Count);

    private static string GetSummaryMessage(int count) => count switch
    {
        1 => "1 отчёт уже имелся в базе в виде полной копии и не был импортирован.",
        var n when n % 10 is >= 2 and <= 4 && n % 100 is not (>= 12 and <= 14)
            => $"{n} отчёта уже имелись в базе в виде полной копии и не были импортированы.",
        _ => $"{count} отчётов уже имелись в базе в виде полной копии и не были импортированы."
    };
}
