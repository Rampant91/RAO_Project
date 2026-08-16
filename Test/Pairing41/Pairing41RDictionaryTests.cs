using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>Unit-тесты загрузки R.xlsx и распределения активности по каналам (Form13/14).</summary>
public sealed class Pairing41RDictionaryTests : IDisposable
{
    public Pairing41RDictionaryTests()
    {
        Pairing41TestAccess.ResetRDictionaryForTests();
    }

    public void Dispose()
    {
        Pairing41TestAccess.ResetRDictionaryForTests();
    }

    [Fact]
    public void TryLoad_FromMissingFile_ReturnsFalseWithMessage()
    {
        var ok = Pairing41TestAccess.TryLoadRDictionaryFromFileForTests(
            Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "_missing_R.xlsx"),
            out var error);

        Assert.False(ok);
        Assert.Contains("R.xlsx", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TryLoad_FromMinimalFixture_MapsBetaActivity()
    {
        var path = CreateMinimalRFixture("тритий-3", "т", activity: "1,0E+06");
        var ok = Pairing41TestAccess.TryLoadRDictionaryFromFileForTests(path, out var error);
        Assert.True(ok, error);

        var activities = Pairing41TestAccess.GetActivitiesForExportForTests("тритий-3", "1,0E+06");
        Assert.Equal("-", activities["alpha"]);
        Assert.Equal("-", activities["beta"]);
        Assert.NotEqual("-", activities["tritium"]);
        Assert.Equal("-", activities["transuranium"]);
    }

    [Fact]
    public void GetActivities_UnknownNuclide_ReturnsDashes()
    {
        var path = CreateMinimalRFixture("кобальт-60", "б", activity: "100");
        Assert.True(Pairing41TestAccess.TryLoadRDictionaryFromFileForTests(path, out _));

        var activities = Pairing41TestAccess.GetActivitiesForExportForTests("неизвестный-999", "100");
        Assert.Equal("-", activities["alpha"]);
        Assert.Equal("-", activities["beta"]);
        Assert.Equal("-", activities["tritium"]);
        Assert.Equal("-", activities["transuranium"]);
    }

    [Fact]
    public void GetActivities_MultiNuclideSameType_UsesFirstCode()
    {
        var path = CreateMinimalRFixtureMulti(
            ("кобальт-60", "б"),
            ("кобальт-61", "б"));
        Assert.True(Pairing41TestAccess.TryLoadRDictionaryFromFileForTests(path, out _));

        var activities = Pairing41TestAccess.GetActivitiesForExportForTests("кобальт-60;кобальт-61", "2,5E+05");
        Assert.NotEqual("-", activities["beta"]);
        Assert.Equal("-", activities["alpha"]);
    }

    [Fact]
    public void TryLoad_RepositoryRFile_WhenPresent_LoadsRows()
    {
        var repoPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "data", "Spravochniki", "R.xlsx"));
        if (!File.Exists(repoPath))
        {
            return;
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        string nuclideName;
        string code;
        using (var probe = new ExcelPackage(new FileInfo(repoPath)))
        {
            var ws = probe.Workbook.Worksheets["Лист1"];
            Assert.NotNull(ws);
            nuclideName = ws.Cells[2, 1].Text;
            code = ws.Cells[2, 8].Text;
            Assert.False(string.IsNullOrWhiteSpace(nuclideName));
        }

        var ok = Pairing41TestAccess.TryLoadRDictionaryFromFileForTests(repoPath, out var error);
        Assert.True(ok, error);

        var activities = Pairing41TestAccess.GetActivitiesForExportForTests(nuclideName, "1,0E+03");
        var channel = code switch
        {
            "а" => "alpha",
            "б" => "beta",
            "т" => "tritium",
            "у" => "transuranium",
            _ => null
        };
        if (channel is not null)
        {
            Assert.NotEqual("-", activities[channel]);
        }
    }

    private string CreateMinimalRFixture(string nuclideName, string code, string activity)
    {
        var dir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Pairing41R_" + Guid.NewGuid())).FullName;
        var path = Path.Combine(dir, "R_minimal.xlsx");
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Лист1");
        sheet.Cells[1, 1].Value = "name";
        sheet.Cells[1, 8].Value = "code";
        sheet.Cells[2, 1].Value = nuclideName;
        sheet.Cells[2, 8].Value = code;
        package.SaveAs(new FileInfo(path));
        return path;
    }

    private string CreateMinimalRFixtureMulti(params (string Name, string Code)[] rows)
    {
        var dir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Pairing41R_" + Guid.NewGuid())).FullName;
        var path = Path.Combine(dir, "R_multi.xlsx");
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Лист1");
        sheet.Cells[1, 1].Value = "name";
        sheet.Cells[1, 8].Value = "code";
        for (var i = 0; i < rows.Length; i++)
        {
            sheet.Cells[i + 2, 1].Value = rows[i].Name;
            sheet.Cells[i + 2, 8].Value = rows[i].Code;
        }

        package.SaveAs(new FileInfo(path));
        return path;
    }
}
