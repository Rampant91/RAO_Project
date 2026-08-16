using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Firebird: в списке IN (...) не больше ~1500 значений (SQL error -901).
/// Держим запас до 1000, как в существующих Excel-выгрузках.
/// </summary>
public static class FirebirdInClause
{
    public const int MaxBatchSize = 1000;

    public static IEnumerable<List<T>> Chunk<T>(IReadOnlyList<T> values, int batchSize = MaxBatchSize)
    {
        var size = batchSize > 0 ? Math.Min(batchSize, MaxBatchSize) : MaxBatchSize;
        if (values.Count == 0)
            yield break;

        for (var offset = 0; offset < values.Count; offset += size)
            yield return values.Skip(offset).Take(size).ToList();
    }

    public static int SumCount(IReadOnlyList<int> ids, Func<List<int>, int> countForBatch)
    {
        if (ids.Count == 0) return 0;
        if (ids.Count <= MaxBatchSize)
            return countForBatch(ids as List<int> ?? ids.ToList());

        var total = 0;
        foreach (var batch in Chunk(ids))
            total += countForBatch(batch);
        return total;
    }

    public static List<TResult> QueryAll<TResult>(
        IReadOnlyList<int> ids, Func<List<int>, List<TResult>> queryBatch)
    {
        if (ids.Count == 0) return [];
        if (ids.Count <= MaxBatchSize)
            return queryBatch(ids as List<int> ?? ids.ToList());

        var result = new List<TResult>();
        foreach (var batch in Chunk(ids))
            result.AddRange(queryBatch(batch));
        return result;
    }
}
