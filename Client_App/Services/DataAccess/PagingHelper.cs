using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Чистая постраничная нарезка для unit-тестов и in-memory сценариев.
/// SQL-пагинация для UI будет использовать те же правила page/pageSize.
/// </summary>
public static class PagingHelper
{
    /// <summary>
    /// Нормализует номер страницы и размер; page &gt;= 1, pageSize &gt;= 1.
    /// </summary>
    public static (int Page, int PageSize, int Skip) Normalize(int page, int pageSize)
    {
        var safePageSize = Math.Max(1, pageSize);
        var safePage = Math.Max(1, page);
        return (safePage, safePageSize, (safePage - 1) * safePageSize);
    }

    public static PagedResult<T> Page<T>(IEnumerable<T> source, int page, int pageSize)
    {
        var list = source as IList<T> ?? source.ToList();
        var (safePage, safePageSize, skip) = Normalize(page, pageSize);
        var items = list.Skip(skip).Take(safePageSize).ToList();
        return new PagedResult<T>
        {
            Items = items,
            TotalCount = list.Count,
            Page = safePage,
            PageSize = safePageSize
        };
    }
}
