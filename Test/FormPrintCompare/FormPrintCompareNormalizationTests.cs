using Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare.Testing;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Xunit;

namespace Test.FormPrintCompare;

/// <summary>Нормализация ключей и lookalike для fingerprint / period.</summary>
public class FormPrintCompareNormalizationTests
{
    [Theory]
    [InlineData("СОС", "COC")]
    [InlineData("  PAS-01  ", "PAS01")]
    [InlineData("007", "7")]
    [InlineData("DOC_1", "DOC1")]
    public void NormalizeId_TreatsLookalikeAndNoiseAsEqual(string left, string right)
    {
        Assert.Equal(
            FormPrintCompareTestAccess.NormalizeId(left),
            FormPrintCompareTestAccess.NormalizeId(right));
    }

    [Theory]
    [InlineData("-")]
    [InlineData("")]
    [InlineData("б.н.")]
    [InlineData("без номера")]
    public void NormalizeId_EmptySerialMarkersBecomeEmpty(string value)
    {
        Assert.Equal(string.Empty, FormPrintCompareTestAccess.NormalizeId(value));
    }

    [Theory]
    [InlineData("Co-60, Cs-137", "Cs-137; Co-60")]
    [InlineData("co60", "Co-60")]
    public void NormalizeRads_OrderAndNoiseDoNotMatter(string left, string right)
    {
        Assert.Equal(
            FormPrintCompareTestAccess.NormalizeRads(left),
            FormPrintCompareTestAccess.NormalizeRads(right));
    }

    [Fact]
    public void BuildPeriodKey_Form1_NormalizesSlashDates()
    {
        var key = FormPrintCompareTestAccess.BuildPeriodKey(
            "1.1", "01/01/2024", "31/12/2024", null);
        Assert.Equal("01.01.2024|31.12.2024", key);
    }

    [Fact]
    public void BuildPeriodKey_Form2_UsesYearDigitsOnly()
    {
        var key = FormPrintCompareTestAccess.BuildPeriodKey("2.12", null, null, "2024 г.");
        Assert.Equal("2024", key);
    }

    [Fact]
    public void Activity_WithinTenPercent_IsExact()
    {
        var level = FormPrintCompareTestAccess.CompareCellLevel(
            "1.1",
            FormPrintCompareTestAccess.ActivityColumnIndex,
            "1.0e+3",
            "1.05e+3");
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Activity_SameValueDifferentNotation_IsExact()
    {
        var level = FormPrintCompareTestAccess.CompareCellLevel(
            "1.1",
            FormPrintCompareTestAccess.ActivityColumnIndex,
            "2e+14",
            "2,00e+14");
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Activity_LargeDifference_IsMismatch()
    {
        var level = FormPrintCompareTestAccess.CompareCellLevel(
            "1.1",
            FormPrintCompareTestAccess.ActivityColumnIndex,
            "1.0e+3",
            "2.0e+3");
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Passport_Lookalike_IsExact()
    {
        var level = FormPrintCompareTestAccess.CompareCellLevel(
            "1.1",
            FormPrintCompareTestAccess.PassportColumnIndex,
            "СОС-1",
            "COC-1");
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void FormatCell_RevealsZeroWidthSpaceDifference()
    {
        var left = "74041/24/0124";
        var right = "74041/24/0124\u200B";
        var formattedLeft = FormPrintCompareTestAccess.FormatCellForExport(
            left, right, FieldMatchLevel.Near);
        var formattedRight = FormPrintCompareTestAccess.FormatCellForExport(
            right, left, FieldMatchLevel.Near);

        Assert.Equal(left, formattedLeft);
        Assert.Contains("ZWSP", formattedRight);
        Assert.NotEqual(formattedLeft, formattedRight);
    }
}
