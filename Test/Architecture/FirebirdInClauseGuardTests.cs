using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Test.Architecture;

/// <summary>
/// Firebird: IN (...) ≤ ~1500. В DataAccess большие списки Id — только через FirebirdInClause / batch.
/// </summary>
public class FirebirdInClauseGuardTests
{
    private static string DataAccessDir
    {
        get
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null
                   && !File.Exists(Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
                dir = dir.Parent;
            Assert.NotNull(dir);
            return Path.Combine(dir!.FullName, "Client_App", "Services", "DataAccess");
        }
    }

    // Локальная коллекция Id в Contains без имени batch — типичный источник -901.
    private static readonly Regex RiskyContains = new(
        @"\b(?!batch)([a-zA-Z_][\w]*)Ids?\.(Contains)\(",
        RegexOptions.Compiled);

    [Fact]
    public void DataAccess_QueryableContains_MustUseBatchedLists()
    {
        Assert.True(Directory.Exists(DataAccessDir), DataAccessDir);
        var offenders = Directory.GetFiles(DataAccessDir, "*.cs")
            .SelectMany(path => File.ReadAllLines(path)
                .Select((line, i) => (path, line, num: i + 1)))
            .Where(x => RiskyContains.IsMatch(x.line)
                        && !x.line.TrimStart().StartsWith("//")
                        && !x.line.Contains("FirebirdInClause"))
            .Select(x => $"{Path.GetFileName(x.path)}:{x.num}: {x.line.Trim()}")
            .ToList();

        Assert.True(offenders.Count == 0,
            "Используйте FirebirdInClause.Chunk/SumCount/QueryAll (batch.Contains). Найдено:\n"
            + string.Join("\n", offenders));
    }

    [Fact]
    public void FirebirdInClause_MaxBatch_UnderFirebirdLimit()
    {
        Assert.True(Client_App.Services.DataAccess.FirebirdInClause.MaxBatchSize <= 1500);
        Assert.True(Client_App.Services.DataAccess.FirebirdInClause.MaxBatchSize >= 500);
    }
}
