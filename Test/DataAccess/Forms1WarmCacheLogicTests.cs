using System.Collections.Generic;
using System.Linq;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class Forms1WarmCacheLogicTests
{
    [Fact]
    public void GetOrderedReportIds_WhiteListFiltersWithoutReload()
    {
        ReportListStub[] stubs =
        [
            new() { Id = 1, FormNum = "1.1", StartPeriod = "01.01.2020", EndPeriod = "31.12.2020", CorrectionNumber = 0 },
            new() { Id = 2, FormNum = "1.5", StartPeriod = "01.01.2021", EndPeriod = "31.12.2021", CorrectionNumber = 0 },
            new() { Id = 3, FormNum = "1.1", StartPeriod = "01.01.2022", EndPeriod = "31.12.2022", CorrectionNumber = 0 },
        ];

        var only11 = MainWindowListQuery.GetOrderedReportIds(stubs, "1.1");
        Assert.Equal(new[] { 3, 1 }, only11);

        var all = MainWindowListQuery.GetOrderedReportIds(stubs, null);
        Assert.Equal(3, all.Count);
        Assert.Equal(1, all.Count(id => id == 2));
    }

    [Fact]
    public void GetOrderedReportIds_PageSliceMatchesPagingHelper()
    {
        var stubs = Enumerable.Range(1, 25)
            .Select(i => new ReportListStub
            {
                Id = i,
                FormNum = "1.1",
                StartPeriod = $"01.01.{2000 + i}",
                EndPeriod = $"31.12.{2000 + i}",
                CorrectionNumber = 0
            })
            .ToList();

        var ordered = MainWindowListQuery.GetOrderedReportIds(stubs, "1.1");
        var (_, pageSize, skip) = PagingHelper.Normalize(2, 10);
        var pageIds = ordered.Skip(skip).Take(pageSize).ToList();
        Assert.Equal(10, pageIds.Count);
        Assert.Equal(ordered[10], pageIds[0]);
    }

    [Fact]
    public void WarmCache_InvalidateAll_IsSafeWhenEmpty()
    {
        Forms1WarmCache.Instance.InvalidateAll();
        Forms1WarmCache.Instance.InvalidateOrg(42);
    }

    [Fact]
    public void WarmCache_StaleEpoch_DoesNotReseedEmptyStubsAfterInvalidate()
    {
        var cache = Forms1WarmCache.Instance;
        cache.InvalidateAll();
        var epochBefore = cache.DataEpoch;

        var empty = new List<ReportListStub>();
        Assert.True(cache.TryCommitStubsForTests(7, empty, epochBefore));
        Assert.True(cache.TryGetCachedStubCount(7, out var emptyCount));
        Assert.Equal(0, emptyCount);

        cache.InvalidateOrg(7);
        var epochAfter = cache.DataEpoch;
        Assert.True(epochAfter > epochBefore);
        Assert.False(cache.TryGetCachedStubCount(7, out _));

        // In-flight load after delete (epochBefore) must not poison cache after invalidate.
        Assert.False(cache.TryCommitStubsForTests(7, empty, epochBefore));
        Assert.False(cache.TryGetCachedStubCount(7, out _));

        var fresh = new List<ReportListStub>
        {
            new()
            {
                Id = 100,
                FormNum = "1.1",
                StartPeriod = "01.01.2024",
                EndPeriod = "31.12.2024",
                CorrectionNumber = 0
            }
        };
        Assert.True(cache.TryCommitStubsForTests(7, fresh, epochAfter));
        Assert.True(cache.TryGetCachedStubCount(7, out var freshCount));
        Assert.Equal(1, freshCount);

        cache.InvalidateAll();
    }

    [Fact]
    public void SelectReportPopup_LoadsShellsFromDbNotOnlyLocalCollection()
    {
        var path = System.IO.Path.Combine(
            FindRepoRoot(), "Client_App", "ViewModels", "Controls", "SelectReportPopupVM.cs");
        Assert.True(System.IO.File.Exists(path), path);
        var src = System.IO.File.ReadAllText(path);
        Assert.Contains("EnsureFormShellsLoadedAsync", src);
        Assert.Contains("LoadReportStubsForForm", src);
        Assert.Contains("CreateReportShellsFromStubs", src);
        Assert.DoesNotContain("LoadReportsByIds", src);
    }

    [Fact]
    public void ImportCommands_RefreshMainWindowAfterImport_OnSuccessAndSortEarlyReturn()
    {
        var baseSrc = System.IO.File.ReadAllText(System.IO.Path.Combine(
            FindRepoRoot(), "Client_App", "Commands", "AsyncCommands", "Import", "ImportBaseAsyncCommand.cs"));
        Assert.Contains("RefreshMainWindowAfterImport", baseSrc);
        Assert.Contains("InvalidateMainWindowCachesAfterImport", baseSrc);

        foreach (var relative in new[]
                 {
                     System.IO.Path.Combine("ImportRaodbAsyncCommand.cs"),
                     System.IO.Path.Combine("ImportExcelAsyncCommand.cs"),
                     System.IO.Path.Combine("ImportJson", "ImportJsonAsyncCommand.cs")
                 })
        {
            var path = System.IO.Path.Combine(
                FindRepoRoot(), "Client_App", "Commands", "AsyncCommands", "Import", relative);
            var src = System.IO.File.ReadAllText(path);
            Assert.Contains("RefreshMainWindowAfterImport()", src);
            Assert.DoesNotContain("InvalidateMainWindowCachesAfterImport();\r\n            var mainWindowVM", src);
            Assert.DoesNotContain("InvalidateMainWindowCachesAfterImport();\n            var mainWindowVM", src);
        }
    }

    private static string FindRepoRoot()
    {
        var dir = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
        while (dir != null
               && !System.IO.File.Exists(System.IO.Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
            dir = dir.Parent;
        Assert.NotNull(dir);
        return dir!.FullName;
    }
}
