using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

internal interface ICompareReportSource
{
    string FormatName { get; }
    bool CanRead(string path);
    Task<FormPrintRaodbIndex.LoadResult> LoadAsync(string path, string sourceLabel, CancellationToken cancellationToken);
}

internal sealed class RaodbCompareSource : ICompareReportSource
{
    public string FormatName => "RAODB";

    public bool CanRead(string path)
    {
        var ext = Path.GetExtension(path);
        return ext.Equals(".raodb", StringComparison.OrdinalIgnoreCase);
    }

    public Task<FormPrintRaodbIndex.LoadResult> LoadAsync(
        string path,
        string sourceLabel,
        CancellationToken cancellationToken) =>
        FormPrintRaodbIndex.LoadAsync(path, cancellationToken, sourceLabel);
}

internal static class CompareReportSources
{
    private static readonly ICompareReportSource[] Sources =
    [
        new RaodbCompareSource()
    ];

    public static ICompareReportSource? ForPath(string path) =>
        Sources.FirstOrDefault(s => s.CanRead(path));
}

internal static class FormPrintCompareCatalog
{
    public static FormPrintRaodbIndex.LoadResult Merge(IReadOnlyList<FormPrintRaodbIndex.LoadResult> parts)
    {
        var reports = parts.SelectMany(p => p.Reports).ToList();
        var deduped = FormPrintRaodbIndex.DeduplicateByKeyKeepMaxCorrection(reports);
        var orgs = CollectOrgKeys(deduped);

        return new FormPrintRaodbIndex.LoadResult
        {
            Reports = deduped,
            RegNo = orgs.Count == 1 ? orgs[0].RegNo : "",
            Okpo = orgs.Count == 1 ? orgs[0].Okpo : ""
        };
    }

    /// <summary>
    /// Уникальные (RegNo, Okpo) из отчётов файлов — ключи для узкой загрузки исходной БД.
    /// </summary>
    public static IReadOnlyList<(string RegNo, string Okpo)> CollectOrgKeys(
        IEnumerable<CompareReportDto> reports) =>
        reports
            .Select(r => (r.RegNo, r.Okpo))
            .Distinct(OrgPairComparer.Instance)
            .ToList();

    /// <summary>
    /// Периоды из файлов сверки по организации — для загрузки только нужных отчётов из БД.
    /// </summary>
    public static IReadOnlyDictionary<string, IReadOnlyList<ComparePeriodHint>> BuildPeriodHintsByOrg(
        IEnumerable<CompareReportDto> reports)
    {
        var map = new Dictionary<string, List<ComparePeriodHint>>(StringComparer.Ordinal);
        foreach (var report in reports)
        {
            var key = $"{ReportMatchKey.NormalizeOrg(report.RegNo)}|{ReportMatchKey.NormalizeOrg(report.Okpo)}";
            if (!map.TryGetValue(key, out var list))
            {
                list = [];
                map[key] = list;
            }

            var hint = new ComparePeriodHint(
                report.FormNum,
                report.PeriodKey,
                report.StartPeriod,
                report.EndPeriod,
                report.Year);
            if (list.Any(h =>
                    string.Equals(h.FormNum, hint.FormNum, StringComparison.Ordinal)
                    && string.Equals(h.PeriodKey, hint.PeriodKey, StringComparison.Ordinal)))
            {
                continue;
            }

            list.Add(hint);
        }

        return map.ToDictionary(
            kv => kv.Key,
            kv => (IReadOnlyList<ComparePeriodHint>)kv.Value,
            StringComparer.Ordinal);
    }

    internal sealed class OrgPairComparer : IEqualityComparer<(string RegNo, string Okpo)>
    {
        public static readonly OrgPairComparer Instance = new();

        public bool Equals((string RegNo, string Okpo) x, (string RegNo, string Okpo) y) =>
            string.Equals(ReportMatchKey.NormalizeOrg(x.RegNo), ReportMatchKey.NormalizeOrg(y.RegNo), StringComparison.Ordinal)
            && string.Equals(ReportMatchKey.NormalizeOrg(x.Okpo), ReportMatchKey.NormalizeOrg(y.Okpo), StringComparison.Ordinal);

        public int GetHashCode((string RegNo, string Okpo) obj) =>
            HashCode.Combine(ReportMatchKey.NormalizeOrg(obj.RegNo), ReportMatchKey.NormalizeOrg(obj.Okpo));
    }
}
