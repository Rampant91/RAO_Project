using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class MainWindowReleaseCleanupTests
{
    private sealed class IdItem
    {
        public int Id { get; init; }
    }

    [Fact]
    public void CollectionSync_SameIds_ReturnsFalseWithoutReplace()
    {
        var target = new ObservableCollection<IdItem> { new() { Id = 1 }, new() { Id = 2 } };
        IReadOnlyList<IdItem> source = [new() { Id = 1 }, new() { Id = 2 }];

        var changed = CollectionSync.ReplaceById(target, source, x => x.Id);

        Assert.False(changed);
        Assert.Equal(2, target.Count);
    }

    [Fact]
    public void CollectionSync_DifferentOrder_ReplacesAndReturnsTrue()
    {
        var target = new ObservableCollection<IdItem> { new() { Id = 1 }, new() { Id = 2 } };
        IReadOnlyList<IdItem> source = [new() { Id = 2 }, new() { Id = 1 }];

        var changed = CollectionSync.ReplaceById(target, source, x => x.Id);

        Assert.True(changed);
        Assert.Equal(2, target[0].Id);
        Assert.Equal(1, target[1].Id);
    }

    [Fact]
    public void FilterOrgKeysForm40_MatchesSubjectRfRegNoOrShort()
    {
        MainWindowListQuery.OrgKey[] keys =
        [
            new() { Id = 1, SubjectRf = "77", RegNo = "Moscow", ShortJurLico = "OrgA" },
            new() { Id = 2, SubjectRf = "50", RegNo = "SPB", ShortJurLico = "OrgB" },
        ];

        var filtered = MainWindowListQuery.FilterOrgKeysForm40ForTest(keys, "mos").ToList();

        Assert.Single(filtered);
        Assert.Equal(1, filtered[0].Id);
    }

    [Fact]
    public void FilterOrgKeysForm50_UsesNameWhenShortEmpty()
    {
        MainWindowListQuery.OrgKey[] keys =
        [
            new() { Id = 1, ShortJurLico = "", Name50 = "Alpha Plant" },
            new() { Id = 2, ShortJurLico = "Beta", Name50 = "Other" },
        ];

        var byName = MainWindowListQuery.FilterOrgKeysForm50ForTest(keys, "alpha").ToList();
        var byShort = MainWindowListQuery.FilterOrgKeysForm50ForTest(keys, "beta").ToList();

        Assert.Single(byName);
        Assert.Equal(1, byName[0].Id);
        Assert.Single(byShort);
        Assert.Equal(2, byShort[0].Id);
    }
}
