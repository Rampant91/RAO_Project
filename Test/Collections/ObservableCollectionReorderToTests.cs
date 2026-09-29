using System.Collections.Generic;
using System.Linq;
using Models.Collections;
using Xunit;

namespace Test.Collections;

public class ObservableCollectionReorderToTests
{
    [Fact]
    public void ReorderTo_PermutesInPlace_SameMembership()
    {
        var a = new Reports { Id = 1 };
        var b = new Reports { Id = 2 };
        var c = new Reports { Id = 3 };
        var col = new ObservableCollectionWithItemPropertyChanged<Reports>([a, b, c]);

        Assert.True(col.ReorderTo([c, a, b]));
        Assert.Equal(new[] { 3, 1, 2 }, col.Select(x => x.Id).ToArray());
        Assert.Same(c, col[0]);
        Assert.Same(a, col[1]);
        Assert.Same(b, col[2]);
    }

    [Fact]
    public void ReorderTo_RejectsDifferentMembership_LeavesUnchanged()
    {
        var a = new Reports { Id = 1 };
        var b = new Reports { Id = 2 };
        var stranger = new Reports { Id = 99 };
        var col = new ObservableCollectionWithItemPropertyChanged<Reports>([a, b]);

        Assert.False(col.ReorderTo([a, stranger]));
        Assert.Equal(new[] { 1, 2 }, col.Select(x => x.Id).ToArray());
    }

    [Fact]
    public void ReorderTo_NoOpWhenAlreadyOrdered()
    {
        var a = new Reports { Id = 1 };
        var b = new Reports { Id = 2 };
        var col = new ObservableCollectionWithItemPropertyChanged<Reports>([a, b]);
        var order = new List<Reports> { a, b };

        Assert.True(col.ReorderTo(order));
        Assert.Same(a, col[0]);
        Assert.Same(b, col[1]);
    }
}
