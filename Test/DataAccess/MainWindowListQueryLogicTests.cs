using System.IO;
using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class MainWindowListQueryLogicTests
{
    [Fact]
    public void PagingHelper_UsedByMainWindow_DefaultsMatchUi()
    {
        var (_, pageSize, skip) = PagingHelper.Normalize(2, MainWindowPagingDefaults.DefaultOrgsPerPage);
        Assert.Equal(MainWindowPagingDefaults.DefaultOrgsPerPage, pageSize);
        Assert.Equal(MainWindowPagingDefaults.DefaultOrgsPerPage, skip);
    }

    [Theory]
    [InlineData(1, 10, 0)]
    [InlineData(3, 10, 20)]
    public void ReportPage_SkipMatchesFormsDefault(int page, int pageSize, int expectedSkip)
    {
        var (_, _, skip) = PagingHelper.Normalize(page, pageSize);
        Assert.Equal(expectedSkip, skip);
    }

    [Fact]
    public void OrgListQuery_StillFiltersForm10Only()
    {
        OrgListItem[] sample =
        [
            new() { ReportsId = 1, MasterReportId = 1, FormNum = "1.0", RegNo = "1", Okpo = "1", ShortJurLico = "A" },
            new() { ReportsId = 2, MasterReportId = 2, FormNum = "2.0", RegNo = "1", Okpo = "1", ShortJurLico = "B" },
        ];

        var page = OrgListQuery.GetPage(sample, "1.0", null, 1, 8);
        Assert.Single(page.Items);
        Assert.Equal(1, page.Items[0].ReportsId);
    }

    [Fact]
    public void CountAllReports_DoesNotUseUnbatchedOrgIdContains()
    {
        // Регрессия Firebird -901: Too many values in member list (>1500).
        var path = Path.Combine(FindRepoRoot(), "Client_App", "Services", "DataAccess", "MainWindowListQuery.cs");
        Assert.True(File.Exists(path), path);
        var src = File.ReadAllText(path);

        Assert.Contains("FirebirdInClause.SumCount", src);
        Assert.Contains("Master_DB.FormNum_DB == masterFormNum", src);
        Assert.Contains("UpsertOrgKeyForm10FromMaster", src);
        Assert.Contains("UpsertOrgKeyForm20FromMaster", src);
        Assert.Contains("_cachedOrgKeysForm20", src);
        Assert.Contains("InvalidateOrgKeysCacheForm20", src);
        Assert.Contains("_cachedOrgKeysForm40", src);
        Assert.Contains("_cachedOrgKeysForm50", src);
        Assert.Contains("InvalidateOrgKeysCacheForm40", src);
        Assert.Contains("InvalidateAllOrgKeysCaches", src);
        Assert.Contains("LoadOrgKeysForm40", src);
        Assert.Contains("LoadOrgKeysForm50", src);
        Assert.DoesNotContain("filteredOrgIds.Contains(r.Reports.Id)", src);
    }

    [Fact]
    public void Form10Close_DoesNotInvalidateWholeOrgKeysCache()
    {
        var path = Path.Combine(
            FindRepoRoot(), "Client_App", "Commands", "AsyncCommands", "NewChangeReportsAsyncCommand.cs");
        Assert.True(File.Exists(path), path);
        var src = File.ReadAllText(path);
        Assert.Contains("UpsertOrgKeyForm10FromMaster", src);
        Assert.Contains("RefreshOrgListAfterTitleChange", src);
        Assert.DoesNotContain("InvalidateOrgKeysCacheForm10()", src);
    }

    [Fact]
    public void Form20Close_DoesNotInvalidateWholeOrgKeysCache()
    {
        var path = Path.Combine(
            FindRepoRoot(), "Client_App", "Commands", "AsyncCommands", "NewChangeReportsAsyncCommand.cs");
        Assert.True(File.Exists(path), path);
        var src = File.ReadAllText(path);
        Assert.Contains("UpsertOrgKeyForm20FromMaster", src);
        Assert.Contains("RefreshOrgListAfterTitleChange", src);
        Assert.DoesNotContain("InvalidateOrgKeysCacheForm20()", src);
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null
               && !File.Exists(Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
            dir = dir.Parent;
        Assert.NotNull(dir);
        return dir!.FullName;
    }
}
