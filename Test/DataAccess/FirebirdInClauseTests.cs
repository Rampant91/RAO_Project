using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class FirebirdInClauseTests
{
    [Fact]
    public void Chunk_RespectsFirebirdSafeBatchSize()
    {
        var ids = Enumerable.Range(1, 2501).ToList();
        var batches = FirebirdInClause.Chunk(ids).ToList();

        Assert.Equal(3, batches.Count);
        Assert.All(batches, b => Assert.True(b.Count <= FirebirdInClause.MaxBatchSize));
        Assert.Equal(2501, batches.Sum(b => b.Count));
        Assert.True(FirebirdInClause.MaxBatchSize <= 1500);
    }

    [Fact]
    public void SumCount_SplitsLargeIdLists()
    {
        var ids = Enumerable.Range(1, 2500).ToList();
        var batchSizes = new System.Collections.Generic.List<int>();
        var total = FirebirdInClause.SumCount(ids, batch =>
        {
            batchSizes.Add(batch.Count);
            return batch.Count;
        });

        Assert.Equal(2500, total);
        Assert.Equal(3, batchSizes.Count);
        Assert.All(batchSizes, s => Assert.True(s <= FirebirdInClause.MaxBatchSize));
    }

    [Fact]
    public void QueryAll_PreservesConcatenationOrderOfBatches()
    {
        var ids = Enumerable.Range(1, 1500).ToList();
        var result = FirebirdInClause.QueryAll(ids, batch => batch.Select(x => x * 10).ToList());
        Assert.Equal(1500, result.Count);
        Assert.Equal(10, result[0]);
        Assert.Equal(15000, result[^1]);
    }
}
