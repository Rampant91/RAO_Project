using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class FormRowsPageLoaderTests
{
    [Theory]
    [InlineData("1.1", true)]
    [InlineData("1.9", true)]
    [InlineData("2.1", false)]
    [InlineData("4.1", false)]
    public void SupportsDbPaging_OnlyForm1x(string formNum, bool expected)
    {
        Assert.Equal(expected, FormRowsPageLoader.SupportsDbPaging(formNum));
    }

    [Fact]
    public void FormRowsQuery_PageOnly_DoesNotExposeOtherPages()
    {
        var rows = Enumerable.Range(1, 100).ToList();
        var page = FormRowsQuery.GetPage(rows, page: 2, pageSize: 30);
        Assert.Equal(30, page.Items.Count);
        Assert.Equal(31, page.Items[0]);
        Assert.Equal(60, page.Items[page.Items.Count - 1]);
    }
}
