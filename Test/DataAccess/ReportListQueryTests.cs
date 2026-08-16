using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class ReportListQueryTests
{
    private static readonly ReportListItem[] Sample =
    [
        new() { ReportId = 1, ReportsId = 100, FormNum = "1.2", StartPeriod = "01.01.2024", EndPeriod = "31.03.2024", CorrectionNumber = 0 },
        new() { ReportId = 2, ReportsId = 100, FormNum = "1.1", StartPeriod = "01.01.2024", EndPeriod = "31.03.2024", CorrectionNumber = 0 },
        new() { ReportId = 3, ReportsId = 100, FormNum = "1.1", StartPeriod = "01.04.2024", EndPeriod = "30.06.2024", CorrectionNumber = 1 },
        new() { ReportId = 4, ReportsId = 200, FormNum = "1.1", StartPeriod = "01.01.2024", EndPeriod = "31.03.2024", CorrectionNumber = 0 },
        new() { ReportId = 5, ReportsId = 100, FormNum = "1.3", StartPeriod = "01.01.2023", EndPeriod = "31.12.2023", CorrectionNumber = 0 },
    ];

    [Fact]
    public void GetPage_ScopesByOrgAndOptionalFormFilter()
    {
        var page = ReportListQuery.GetPage(Sample, reportsId: 100, formNumWhiteList: "1.1", page: 1, pageSize: 10);

        Assert.Equal(2, page.TotalCount);
        Assert.All(page.Items, x => Assert.Equal(100, x.ReportsId));
        Assert.All(page.Items, x => Assert.Equal("1.1", x.FormNum));
    }

    [Fact]
    public void Order_PutsFormNumThenNewerPeriodsFirst()
    {
        var ordered = ReportListQuery.Order(ReportListQuery.Filter(Sample, 100, null)).ToList();

        Assert.Equal(new[] { 3, 2, 1, 5 }, ordered.Select(x => x.ReportId));
    }

    [Fact]
    public void GetPage_SecondPage_SkipsFirstChunk()
    {
        var page = ReportListQuery.GetPage(Sample, 100, null, page: 2, pageSize: 2);

        Assert.Equal(4, page.TotalCount);
        Assert.Equal(2, page.Items.Count);
        Assert.Equal(new[] { 1, 5 }, page.Items.Select(x => x.ReportId));
    }
}
