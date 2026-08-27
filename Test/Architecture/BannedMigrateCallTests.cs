using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Test.Architecture;

public class BannedMigrateCallTests
{
    private static readonly Regex DirectMigratePattern = new(
        @"\.Database\s*\.\s*Migrate(Async)?\s*\(",
        RegexOptions.Compiled);

    private static readonly string[] AllowedFiles =
    {
        "DatabaseMigrationHelper.cs"
    };

    [Fact]
    public void No_Direct_Database_Migrate_Calls_Outside_Helper()
    {
        var repoRoot = FindRepoRoot();
        var csFiles = Directory.EnumerateFiles(repoRoot, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains(Path.Combine("bin", "")) && !f.Contains(Path.Combine("obj", "")))
            .Where(f => !AllowedFiles.Any(allowed => f.EndsWith(allowed)));

        var violations = csFiles
            .SelectMany(file => File.ReadAllLines(file)
                .Select((line, idx) => new { file, line, lineNum = idx + 1 })
                .Where(x => DirectMigratePattern.IsMatch(x.line)))
            .Select(x => $"{Path.GetRelativePath(repoRoot, x.file)}:{x.lineNum}  {x.line.Trim()}")
            .ToList();

        Assert.True(
            violations.Count == 0,
            "Прямой вызов Database.Migrate / Database.MigrateAsync запрещён.\n" +
            "Используйте DataContext.MigrateDatabase() / MigrateDatabaseAsync().\n\n" +
            "Нарушения:\n" + string.Join("\n", violations));
    }

    private static string FindRepoRoot()
    {
        var dir = Directory.GetCurrentDirectory();
        while (dir != null)
        {
            if (Directory.Exists(Path.Combine(dir, ".git")) ||
                File.Exists(Path.Combine(dir, "RAO_Project.sln")))
                return dir;
            dir = Directory.GetParent(dir)?.FullName;
        }
        return Directory.GetCurrentDirectory();
    }
}
