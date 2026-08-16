using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class FormRowsQueryTests
{
    [Fact]
    public void GetPage_DefaultThirty_ShowsFirstPageOnly()
    {
        var rows = Enumerable.Range(1, 75).ToList();
        var page = FormRowsQuery.GetPage(rows, page: 1, pageSize: 30);

        Assert.Equal(75, page.TotalCount);
        Assert.Equal(3, page.TotalPages);
        Assert.Equal(Enumerable.Range(1, 30), page.Items);
    }

    [Fact]
    public void GetPage_PageThree_ReturnsRemainder()
    {
        var rows = Enumerable.Range(1, 75).ToList();
        var page = FormRowsQuery.GetPage(rows, page: 3, pageSize: 30);

        Assert.Equal(Enumerable.Range(61, 15), page.Items);
    }
}
