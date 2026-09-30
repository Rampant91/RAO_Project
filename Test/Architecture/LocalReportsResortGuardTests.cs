using System.IO;
using System.Linq;
using Xunit;

namespace Test.Architecture;

/// <summary>
/// Clear on tracked LocalReports.Reports_Collection marks orgs Deleted (EF Cascade) — wipe risk.
/// Resort must use ReorderTo / never Reports_Collection.Clear().
/// </summary>
public class LocalReportsResortGuardTests
{
    private static string ClientAppRoot
    {
        get
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null
                   && !File.Exists(Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
                dir = dir.Parent;
            Assert.NotNull(dir);
            return Path.Combine(dir!.FullName, "Client_App");
        }
    }

    private static string ModelsRoot
    {
        get
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null
                   && !File.Exists(Path.Combine(dir.FullName, "Models", "Models.csproj")))
                dir = dir.Parent;
            Assert.NotNull(dir);
            return Path.Combine(dir!.FullName, "Models");
        }
    }

    [Fact]
    public void ClientApp_DoesNotClearTrackedReportsCollection()
    {
        Assert.True(Directory.Exists(ClientAppRoot), ClientAppRoot);
        var offenders = Directory.GetFiles(ClientAppRoot, "*.cs", SearchOption.AllDirectories)
            .SelectMany(path => File.ReadAllLines(path)
                .Select((line, i) => (path, line, num: i + 1)))
            .Where(x => x.line.Contains("Reports_Collection.Clear()", System.StringComparison.Ordinal)
                        && !x.line.TrimStart().StartsWith("//"))
            .Select(x => $"{x.path}:{x.num}: {x.line.Trim()}")
            .ToList();

        Assert.True(offenders.Count == 0,
            "Use ReorderTo instead of Reports_Collection.Clear():\n" + string.Join("\n", offenders));
    }

    [Fact]
    public void ResortSites_UseReorderTo()
    {
        string[] relative =
        [
            Path.Combine("Commands", "AsyncCommands", "Add", "AddReportsAsyncCommand.cs"),
            Path.Combine("Commands", "AsyncCommands", "Save", "SaveReportsAsyncCommand.cs"),
            Path.Combine("Commands", "AsyncCommands", "InitializationAsyncCommand.cs"),
            Path.Combine("Commands", "AsyncCommands", "Import", "ImportRaodbAsyncCommand.cs"),
            Path.Combine("Commands", "AsyncCommands", "Import", "ImportExcelAsyncCommand.cs"),
            Path.Combine("Commands", "AsyncCommands", "Import", "ImportJson", "ImportJsonAsyncCommand.cs"),
        ];

        foreach (var rel in relative)
        {
            var path = Path.Combine(ClientAppRoot, rel);
            Assert.True(File.Exists(path), path);
            var src = File.ReadAllText(path);
            Assert.Contains("ReorderTo", src);
        }
    }

    [Fact]
    public void DataContext_GuardsMassReportsDelete()
    {
        var path = Path.Combine(ModelsRoot, "DBRealization", "DataContext.cs");
        Assert.True(File.Exists(path), path);
        var src = File.ReadAllText(path);
        Assert.Contains("ThrowIfUnsafeReportsMassDelete", src);
        Assert.Contains("EfReportsDeleteGuard.AllowBulkReportsDelete", src);
        Assert.Contains("override int SaveChanges()", src);
        Assert.Contains("override Task<int> SaveChangesAsync", src);
    }

    [Fact]
    public void StartupOrphanCleanup_UsesIdAndBulkGuard()
    {
        var path = Path.Combine(ClientAppRoot, "Commands", "AsyncCommands", "InitializationAsyncCommand.cs");
        Assert.True(File.Exists(path), path);
        var src = File.ReadAllText(path);
        Assert.Contains("DBObservableId == null", src);
        Assert.Contains("EfReportsDeleteGuard.AllowBulkDeletes()", src);
    }
}
