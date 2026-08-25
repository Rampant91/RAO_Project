using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;
using Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare.Testing;
using Xunit;

namespace Test.FormPrintCompare;

public class FormPrintComparePairingTests
{
    private static CompareRowDto Row(int id, string passport) =>
        FormPrintCompareTestAccess.Row11(id, 0, 1, "11", "15.03.2024", passport, "ИИИ", "F1");

    [Fact]
    public void SameOrgFormPeriod_ArePairedAcrossSeparateCatalogs()
    {
        var a = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 0, "111", "okpo1", 1, Row(1, "PAS1"));
        var b = FormPrintCompareTestAccess.CreateReport11(
            "01.04.2024", "30.06.2024", 0, "111", "okpo1", 2, Row(2, "PAS2"));
        var asOne = FormPrintCompareTestAccess.Pair([a, b], [a, b]);
        var asManyFiles = FormPrintCompareTestAccess.Pair([a, b], [a, b]);

        Assert.Equal(asOne.Count, asManyFiles.Count);
        Assert.All(asOne, r => Assert.Equal(ReportCompareKind.Compared, r.Kind));
        Assert.All(asOne, r => Assert.True(r.IsIdentical));
    }

    [Fact]
    public void DifferentOrgs_SameFormAndPeriod_OnlyCompareFileRow()
    {
        var left = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.12.2024", 0, "111", "okpo1", 1, Row(1, "PAS1"));
        var right = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.12.2024", 0, "222", "okpo2", 2, Row(2, "PAS1"));

        var results = FormPrintCompareTestAccess.Pair([left], [right]);

        Assert.Single(results);
        Assert.Equal(ReportCompareKind.MissingInSourceDb, results[0].Kind);
        Assert.False(results[0].CreatesDetailSheet);
    }

    [Fact]
    public void ReportOnlyInCompareFiles_SummaryStatus_NoDetailSheet()
    {
        var right = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.12.2024", 0, Row(1, "NEW"));
        var results = FormPrintCompareTestAccess.Pair([], [right]);

        Assert.Single(results);
        Assert.Equal(ReportCompareKind.MissingInSourceDb, results[0].Kind);
        Assert.False(results[0].CreatesDetailSheet);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "a.RAODB; b.RAODB", results);
        Assert.Equal(2, package.Workbook.Worksheets.Count);
        Assert.Contains("Отсутствует в текущей БД", package.Workbook.Worksheets[0].Cells[14, 11].Text);
    }

    [Fact]
    public void InteriorPeriodOverlap_SpecialStatus_NoDetailSheet()
    {
        var left = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 0,  "111", "okpo1", 1, Row(1, "PAS1"));
        var right = FormPrintCompareTestAccess.CreateReport11(
            "01.02.2024", "30.06.2024", 0, "111", "okpo1", 2, Row(2, "PAS2"));

        Assert.True(FormPrintCompareTestAccess.PeriodsOverlap(left, right));
        var results = FormPrintCompareTestAccess.Pair([left], [right]);

        Assert.Single(results);
        Assert.Equal(ReportCompareKind.PeriodOverlap, results[0].Kind);
        Assert.False(results[0].CreatesDetailSheet);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "111", "okpo1", "compare.RAODB", results);
        Assert.Equal(2, package.Workbook.Worksheets.Count);
        Assert.Contains("пересекается", package.Workbook.Worksheets[0].Cells[14, 11].Text);
    }

    [Fact]
    public void AdjacentPeriods_AreNotOverlap()
    {
        var left = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 0, "111", "okpo1", 1, Row(1, "PAS1"));
        var right = FormPrintCompareTestAccess.CreateReport11(
            "31.03.2024", "30.06.2024", 0, "111", "okpo1", 2, Row(2, "PAS2"));

        Assert.False(FormPrintCompareTestAccess.PeriodsOverlap(left, right));
        var results = FormPrintCompareTestAccess.Pair([left], [right]);

        Assert.Single(results);
        Assert.Equal(ReportCompareKind.MissingInSourceDb, results[0].Kind);
        Assert.DoesNotContain(results, r => r.Kind == ReportCompareKind.PeriodOverlap);
    }

    [Fact]
    public void ExtraSourceReports_DoNotAppearInSummary_OnlyCompareFiles()
    {
        var sourceExtra = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2023", "31.12.2023", 0, "111", "okpo1", 1, Row(1, "OLD"));
        var sourceMatch = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.12.2024", 0, "111", "okpo1", 2, Row(2, "PAS"));
        var compareFile = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.12.2024", 0, "111", "okpo1", 3,
            "ООО Тест", "file001.RAODB", Row(3, "PAS"));

        var results = FormPrintCompareTestAccess.Pair([sourceExtra, sourceMatch], [compareFile]);

        Assert.Single(results);
        Assert.Equal(ReportCompareKind.Compared, results[0].Kind);
        Assert.True(results[0].IsIdentical);
        Assert.False(results[0].CreatesDetailSheet);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "111", "okpo1", "file001.RAODB", results);
        var summary = package.Workbook.Worksheets[0];
        Assert.Equal("111", summary.Cells[14, 1].Text);
        Assert.Equal("ООО Тест", summary.Cells[14, 2].Text);
        Assert.Equal("okpo1", summary.Cells[14, 3].Text);
        Assert.Equal("file001.RAODB", summary.Cells[14, 4].Text);
        Assert.Equal(2, package.Workbook.Worksheets.Count);
    }

    [Fact]
    public void Merge_KeepsHigherCorrectionForSameKey()
    {
        var older = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.12.2024", 1, "111", "okpo1", 1, Row(1, "OLD"));
        var newer = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.12.2024", 3, "111", "okpo1", 2, Row(2, "NEW"));

        var merged = FormPrintCompareTestAccess.DeduplicateByKeyKeepMaxCorrection([older, newer]);
        Assert.Single(merged);
        Assert.Equal((byte)3, merged[0].CorrectionNumber);
    }

    [Fact]
    public void CollectOrgKeys_DedupsByNormalizedRegNoOkpo()
    {
        var a1 = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 0, "111", "okpo1", 1, Row(1, "A"));
        var a2 = FormPrintCompareTestAccess.CreateReport11(
            "01.04.2024", "30.06.2024", 0, "111", "OKPO1", 2, Row(2, "B"));
        var b = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.12.2024", 0, "222", "okpo2", 3, Row(3, "C"));

        var keys = FormPrintCompareTestAccess.CollectOrgKeys([a1, a2, b]);
        Assert.Equal(2, keys.Count);
    }

    [Fact]
    public void PeriodHints_KeepExactAndOverlapping_SkipUnrelated()
    {
        var fromFile = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 0, "111", "okpo1", 1, Row(1, "A"));
        var hints = FormPrintCompareTestAccess.BuildPeriodHintsByOrg([fromFile]);
        Assert.True(hints.TryGetValue("111|OKPO1", out var orgHints));
        Assert.Single(orgHints!);

        Assert.True(FormPrintCompareTestAccess.IsNeededForCompareHints(
            "1.1", "01.01.2024", "31.03.2024", null, orgHints));
        Assert.True(FormPrintCompareTestAccess.IsNeededForCompareHints(
            "1.1", "01.02.2024", "30.06.2024", null, orgHints)); // пересечение
        Assert.False(FormPrintCompareTestAccess.IsNeededForCompareHints(
            "1.1", "01.07.2024", "30.09.2024", null, orgHints)); // другой квартал
        Assert.False(FormPrintCompareTestAccess.IsNeededForCompareHints(
            "1.2", "01.01.2024", "31.03.2024", null, orgHints)); // другая форма
    }

    [Fact]
    public void SupportedForms_HaveColumns()
    {
        foreach (var formNum in FormPrintCompareSchema.SupportedForms)
        {
            Assert.NotEmpty(FormPrintCompareSchema.ColumnsFor(formNum));
        }
    }
}
