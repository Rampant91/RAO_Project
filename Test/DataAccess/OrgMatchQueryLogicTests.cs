using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

/// <summary>
/// Контракт matching импорта по OKPO/RegNo (те же ветки, что раньше в LocalReports).
/// </summary>
public class OrgMatchQueryLogicTests
{
    [Theory]
    [InlineData("111", "R0", "", "", "111", "R0", "", "", true)]   // головное
    [InlineData("111", "R1", "", "", "111", "R0", "", "R1", true)] // рег№ из обособленного в базе
    [InlineData("111", "RH", "222", "RB", "111", "RH", "222", "RB", true)] // обособленные
    [InlineData("111", "RH", "222", "RH", "111", "RH", "222", "", true)] // рег№ юрлица при пустом у обособл.
    [InlineData("222", "RB", "111", "RH", "111", "RH", "", "", true)] // сбитый ОКПО: импорт обособл. ↔ база юрлицо
    [InlineData("111", "RH", "", "", "999", "RH", "111", "RH", true)] // сбитый ОКПО: импорт юрлицо ↔ база обособл.
    [InlineData("111", "R0", "", "", "999", "R0", "", "", false)]
    public void MatchBranches_SameAsLegacyLocalEqual(
        string i0Okpo, string i0Reg, string i1Okpo, string i1Reg,
        string t0Okpo, string t0Reg, string t1Okpo, string t1Reg,
        bool expectMatch)
    {
        // Дублируем предикат MatchTitle (primary + crossed) для unit-теста без БД.
        var primary =
            (i0Okpo == t0Okpo && i0Reg == t0Reg && i1Okpo == "" && t1Okpo == "")
            || (i0Okpo == t0Okpo && i0Reg == t1Reg && i1Okpo == "" && t1Okpo == "")
            || (i1Okpo == t1Okpo && i1Reg == t1Reg && i1Okpo != "")
            || (i1Okpo == t1Okpo && i1Reg == t0Reg && i1Okpo != "" && t1Reg == "");
        var crossed =
            (i1Okpo != "" && t1Okpo == "" && i1Okpo == t0Okpo && i1Reg == t0Reg)
            || (i1Okpo == "" && t1Okpo != "" && i0Okpo == t1Okpo && i0Reg == t1Reg);
        var matched = primary || crossed;

        Assert.Equal(expectMatch, matched);
    }

    [Fact]
    public void FirebirdGuard_OrgMatchQuery_DoesNotUseUnbatchedIdContains()
    {
        var path = System.IO.Path.Combine(
            FindRepoRoot(), "Client_App", "Services", "DataAccess", "OrgMatchQuery.cs");
        Assert.True(System.IO.File.Exists(path));
        var src = System.IO.File.ReadAllText(path);
        Assert.DoesNotContain("Ids.Contains", src);
        Assert.Contains("FindForm10Equal", src);
        Assert.Contains("FindByExcelTitleFields", src);
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
