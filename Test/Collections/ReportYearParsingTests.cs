using Models.Collections;
using Xunit;

namespace Test.Collections;

public class ReportYearParsingTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("   ", null)]
    [InlineData("2024", 2024)]
    [InlineData(" 2025 ", 2025)]
    [InlineData("г.2023", 2023)]
    [InlineData("abcd", null)]
    public void ParseYearFromText_ExtractsDigitsOrNull(string? input, int? expected) =>
        Assert.Equal(expected, Report.ParseYearFromText(input));

    [Theory]
    [InlineData(2024, 2024)]
    [InlineData(2024.0, 2024)]
    [InlineData("2026", 2026)]
    [InlineData(null, null)]
    public void ParseYearFromImport_HandlesCommonImportValues(object? input, int? expected) =>
        Assert.Equal(expected, Report.ParseYearFromImport(input));
}
