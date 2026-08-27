using System.Collections.Generic;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class FormRowOrderedIdsCacheTests
{
    public FormRowOrderedIdsCacheTests()
    {
        FormRowOrderedIdsCache.Invalidate();
    }

    [Fact]
    public void Store_ThenTryGet_SameReportHits()
    {
        var ids = new List<int> { 10, 20, 30 };
        FormRowOrderedIdsCache.Store(5, "1.1", ids);

        Assert.True(FormRowOrderedIdsCache.TryGet(5, "1.1", out var cached));
        Assert.Equal(ids, cached);
    }

    [Fact]
    public void TryGet_DifferentReportOrForm_Misses()
    {
        FormRowOrderedIdsCache.Store(5, "1.1", [1, 2]);
        Assert.False(FormRowOrderedIdsCache.TryGet(6, "1.1", out _));
        Assert.False(FormRowOrderedIdsCache.TryGet(5, "1.2", out _));
    }

    [Fact]
    public void Invalidate_ClearsHit()
    {
        FormRowOrderedIdsCache.Store(5, "1.1", [1, 2]);
        FormRowOrderedIdsCache.Invalidate();
        Assert.False(FormRowOrderedIdsCache.TryGet(5, "1.1", out _));
    }

    [Fact]
    public void Store_OverwritesPreviousReport()
    {
        FormRowOrderedIdsCache.Store(5, "1.1", [1]);
        FormRowOrderedIdsCache.Store(8, "1.1", [9]);
        Assert.False(FormRowOrderedIdsCache.TryGet(5, "1.1", out _));
        Assert.True(FormRowOrderedIdsCache.TryGet(8, "1.1", out var cached));
        Assert.Equal([9], cached);
    }
}
