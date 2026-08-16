using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class OrgListQueryTests
{
    private static readonly OrgListItem[] Sample =
    [
        new() { ReportsId = 1, MasterReportId = 10, FormNum = "1.0", RegNo = "77001", Okpo = "111", ShortJurLico = "Альфа" },
        new() { ReportsId = 2, MasterReportId = 20, FormNum = "1.0", RegNo = "77002", Okpo = "222", ShortJurLico = "Бета" },
        new() { ReportsId = 3, MasterReportId = 30, FormNum = "2.0", RegNo = "77001", Okpo = "333", ShortJurLico = "Гамма" },
        new() { ReportsId = 4, MasterReportId = 40, FormNum = "1.0", RegNo = "77010", Okpo = "444", ShortJurLico = "Альфа-2" },
    ];

    [Fact]
    public void GetPage_FiltersByMasterFormAndSearch()
    {
        var page = OrgListQuery.GetPage(Sample, "1.0", "альфа", page: 1, pageSize: 8);

        Assert.Equal(2, page.TotalCount);
        Assert.All(page.Items, x => Assert.Equal("1.0", x.FormNum));
        Assert.Contains(page.Items, x => x.ReportsId == 1);
        Assert.Contains(page.Items, x => x.ReportsId == 4);
    }

    [Fact]
    public void GetPage_RespectsPageSize()
    {
        var page = OrgListQuery.GetPage(Sample, "1.0", null, page: 1, pageSize: 2);

        Assert.Equal(3, page.TotalCount);
        Assert.Equal(2, page.Items.Count);
        Assert.Equal(2, page.TotalPages);
    }
}
