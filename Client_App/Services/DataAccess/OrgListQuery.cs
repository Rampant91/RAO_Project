using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Resources.CustomComparers;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Фильтрация/сортировка/пагинация списка организаций (чистая логика для тестов).
/// SQL-реализация на главном окне будет повторять те же правила.
/// </summary>
public static class OrgListQuery
{
    public static IEnumerable<OrgListItem> Filter(
        IEnumerable<OrgListItem> source,
        string masterFormNum,
        string? searchText)
    {
        var result = source.Where(x => x.FormNum == masterFormNum);
        if (string.IsNullOrWhiteSpace(searchText))
            return result;

        var search = searchText.Trim();
        return result.Where(x =>
            ContainsIgnoreCase(x.RegNo, search)
            || ContainsIgnoreCase(x.Okpo, search)
            || ContainsIgnoreCase(x.ShortJurLico, search));
    }

    public static IOrderedEnumerable<OrgListItem> Order(IEnumerable<OrgListItem> source)
    {
        var comparator = new CustomReportsComparer();
        return source
            .OrderBy(x => x.RegNo ?? string.Empty, comparator)
            .ThenBy(x => x.Okpo ?? string.Empty, comparator);
    }

    public static PagedResult<OrgListItem> GetPage(
        IEnumerable<OrgListItem> source,
        string masterFormNum,
        string? searchText,
        int page,
        int pageSize)
    {
        var filtered = Order(Filter(source, masterFormNum, searchText)).ToList();
        return PagingHelper.Page(filtered, page, pageSize);
    }

    private static bool ContainsIgnoreCase(string? value, string search) =>
        !string.IsNullOrEmpty(value)
        && value.Contains(search, StringComparison.CurrentCultureIgnoreCase);
}
