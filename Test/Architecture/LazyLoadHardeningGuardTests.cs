using System.IO;
using System.Linq;
using Xunit;

namespace Test.Architecture;

/// <summary>
/// Регрессии lazy-load: импорт и паспорта не должны опираться на paged Report_Collection.
/// </summary>
public class LazyLoadHardeningGuardTests
{
    private static string RepoRoot
    {
        get
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "RAO_Project.sln"))
                   && !File.Exists(Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
                dir = dir.Parent;
            Assert.NotNull(dir);
            return dir!.FullName;
        }
    }

    private static string ReadClient(params string[] relative)
    {
        var path = Path.Combine(new[] { RepoRoot, "Client_App" }.Concat(relative).ToArray());
        Assert.True(File.Exists(path), $"Missing {path}");
        return File.ReadAllText(path);
    }

    [Fact]
    public void ImportBase_LoadsOrgShellsBeforeMerge()
    {
        var src = ReadClient("Commands", "AsyncCommands", "Import", "ImportBaseAsyncCommand.cs");
        Assert.Contains("GetBaseOrgReportShellsAsync", src);
        Assert.Contains("LoadOrgWithReportShellsAsync", src);
        Assert.Contains("InvalidateMainWindowCachesAfterImport", src);
    }

    [Fact]
    public void PassportFill_EnsuresAllRowsBeforeFill()
    {
        var src = ReadClient("Commands", "AsyncCommands", "PassportFill", "PassportFillBaseCommand.cs");
        Assert.Contains("EnsureRowsAndTitleAsync", src);
        Assert.Contains("EnsureAllRowsLoadedAsync", src);
    }

    [Fact]
    public void Forms4And5_UsePersistentWarmCacheCollections()
    {
        var forms4 = ReadClient("ViewModels", "MainWindowTabs", "Forms4TabControlVM.cs");
        var forms5 = ReadClient("ViewModels", "MainWindowTabs", "Forms5TabControlVM.cs");
        Assert.Contains("Forms1WarmCache", forms4);
        Assert.Contains("Forms1WarmCache", forms5);
        Assert.Contains("UpdateReportCollection", forms4);
        Assert.Contains("UpdateReportCollection", forms5);
        Assert.DoesNotContain("HasFormNumAsync", forms4);
        Assert.DoesNotContain("HasFormNumAsync", forms5);
        Assert.DoesNotContain(".GetResult()", forms4);
        Assert.DoesNotContain(".GetResult()", forms5);
    }

    [Fact]
    public void Initialization_PrefetchesTabsAndSkipsTitlePreload()
    {
        var src = ReadClient("Commands", "AsyncCommands", "InitializationAsyncCommand.cs");
        Assert.Contains("PrefetchTabs", src);
        Assert.DoesNotContain("await dbm.form_20.LoadAsync()", src);
        Assert.DoesNotContain("await dbm.form_40.LoadAsync()", src);
        Assert.DoesNotContain("await dbm.form_50.LoadAsync()", src);
        Assert.Contains("masterIdsWithForm20", src);
        Assert.Contains("zeroOrderNotes", src);
    }
}
