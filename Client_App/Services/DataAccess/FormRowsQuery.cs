using System.Collections.Generic;
using System.Linq;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Постраничная выборка строк формы (чистая логика; DB Include Skip/Take — следующий шаг фазы 3).
/// </summary>
public static class FormRowsQuery
{
    public static PagedResult<T> GetPage<T>(IEnumerable<T> allRows, int page, int pageSize) =>
        PagingHelper.Page(allRows, page, pageSize);

    public static int Count<T>(IEnumerable<T> allRows) => allRows.Count();
}
