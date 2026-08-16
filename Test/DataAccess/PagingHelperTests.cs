using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class PagingHelperTests
{
    [Theory]
    [InlineData(1, 8, 0)]
    [InlineData(2, 8, 8)]
    [InlineData(0, 8, 0)]
    [InlineData(-1, 0, 0)]
    public void Normalize_ClampsPageAndSize(int page, int pageSize, int expectedSkip)
    {
        var (p, size, skip) = PagingHelper.Normalize(page, pageSize);
        Assert.True(p >= 1);
        Assert.True(size >= 1);
        Assert.Equal(expectedSkip, skip);
    }

    [Fact]
    public void Page_ReturnsRequestedSliceAndTotal()
    {
        var source = Enumerable.Range(1, 25);
        var page = PagingHelper.Page(source, page: 2, pageSize: 8);

        Assert.Equal(25, page.TotalCount);
        Assert.Equal(4, page.TotalPages);
        Assert.Equal(new[] { 9, 10, 11, 12, 13, 14, 15, 16 }, page.Items);
    }

    [Fact]
    public void Page_EmptySource_ReturnsEmpty()
    {
        var page = PagingHelper.Page(Enumerable.Empty<int>(), 1, 8);
        Assert.Empty(page.Items);
        Assert.Equal(0, page.TotalCount);
        Assert.Equal(0, page.TotalPages);
    }
}
