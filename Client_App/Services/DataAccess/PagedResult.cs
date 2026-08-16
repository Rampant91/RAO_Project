using System.Collections.Generic;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Результат постраничной выборки (read-model для гридов и тестов).
/// </summary>
public sealed class PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }

    public int TotalPages =>
        PageSize <= 0 || TotalCount <= 0
            ? 0
            : (TotalCount + PageSize - 1) / PageSize;
}
