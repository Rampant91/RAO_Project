using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Фильтрация/сортировка/пагинация списка отчётов организации (чистая логика для тестов).
/// </summary>
public static class ReportListQuery
{
    public static IEnumerable<ReportListItem> Filter(
        IEnumerable<ReportListItem> source,
        int reportsId,
        string? formNumWhiteList)
    {
        var result = source.Where(x => x.ReportsId == reportsId);
        if (!string.IsNullOrWhiteSpace(formNumWhiteList))
            result = result.Where(x => x.FormNum == formNumWhiteList);
        return result;
    }

    public static IOrderedEnumerable<ReportListItem> Order(IEnumerable<ReportListItem> source) =>
        source
            .OrderBy(x =>
            {
                var parts = x.FormNum.Split('.');
                return parts.Length > 1 && int.TryParse(parts[1], out var n) ? n : int.MinValue;
            })
            .ThenByDescending(x => ParseDateOrMax(x.StartPeriod))
            .ThenByDescending(x => ParseDateOrMax(x.EndPeriod))
            .ThenBy(x => x.CorrectionNumber);

    public static PagedResult<ReportListItem> GetPage(
        IEnumerable<ReportListItem> source,
        int reportsId,
        string? formNumWhiteList,
        int page,
        int pageSize)
    {
        var filtered = Order(Filter(source, reportsId, formNumWhiteList)).ToList();
        return PagingHelper.Page(filtered, page, pageSize);
    }

    private static DateOnly ParseDateOrMax(string? value) =>
        value != null && DateOnly.TryParse(value, out var d) ? d : DateOnly.MaxValue;
}
