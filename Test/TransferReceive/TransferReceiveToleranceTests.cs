using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using Xunit;

namespace Test.TransferReceive;

/// <summary>
/// G2 — допуски: активность ±10%; дата — точное совпадение vs окно поиска ±15.
/// </summary>
public class TransferReceiveToleranceTests
{
    [Theory]
    [InlineData("100", "100")]
    [InlineData("100", "110")]
    [InlineData("100", "90")]
    [InlineData("1.0e+6", "1.05e+6")]
    [InlineData("1,5", "1.5")]
    public void ActivityMatches_WithinTenPercent(string left, string right)
    {
        Assert.True(TransferReceiveScenarioRunner.ActivityMatches(left, right));
    }

    [Theory]
    [InlineData("100", "112")]
    [InlineData("100", "89")]
    [InlineData("1.0e+6", "1.2e+6")]
    public void ActivityMatches_OutsideTenPercent(string left, string right)
    {
        Assert.False(TransferReceiveScenarioRunner.ActivityMatches(left, right));
    }

    [Fact]
    public void DatesEqualExact_RequiresSameDay()
    {
        Assert.True(TransferReceiveScenarioRunner.DatesEqualExact("2024-06-15", "2024-06-15"));
        Assert.False(TransferReceiveScenarioRunner.DatesEqualExact("2024-06-15", "2024-06-16"));
    }

    [Fact]
    public void DateWithinTolerance_AllowsFifteenDays()
    {
        Assert.True(TransferReceiveScenarioRunner.DateWithinTolerance("2024-06-15", "2024-06-30"));
        Assert.False(TransferReceiveScenarioRunner.DateWithinTolerance("2024-06-15", "2024-07-01"));
    }

    [Theory]
    [InlineData("21", "31")]
    [InlineData("22", "32")]
    [InlineData("25", "37")]
    [InlineData("27", "35")]
    [InlineData("28", "38")]
    [InlineData("29", "39")]
    public void OpCodesArePaired_KnownPairs(string left, string right)
    {
        Assert.True(TransferReceiveScenarioRunner.OpCodesArePaired(left, right));
        Assert.True(TransferReceiveScenarioRunner.OpCodesArePaired(right, left));
    }

    [Theory]
    [InlineData("21", "32")]
    [InlineData("21", "21")]
    public void OpCodesArePaired_RejectsWrongPairs(string left, string right)
    {
        Assert.False(TransferReceiveScenarioRunner.OpCodesArePaired(left, right));
    }
}
