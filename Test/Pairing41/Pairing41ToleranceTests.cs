using Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41.Testing;
using Xunit;

namespace Test.Pairing41;

/// <summary>
/// G2 — допуск ±10% для числовых полей и перевод массы кг→т.
/// </summary>
public class Pairing41ToleranceTests
{
    /// <summary>Значения внутри или на границе ±10% считаются равными.</summary>
    [Theory]
    [InlineData("100", "100")]
    [InlineData("100", "109")]
    [InlineData("100", "91")]
    [InlineData("1.0e+06", "1.05e+06")]
    [InlineData("0", "0")]
    [InlineData("0.0", "0")]
    public void NumericWithTolerance_ReturnsTrue_WhenWithinOrEqual(string left, string right)
    {
        Assert.True(Pairing41ScenarioRunner.NumericWithTolerance(left, right));
    }

    [Theory]
    [InlineData("100", "112")]
    [InlineData("100", "88")]
    [InlineData("1.0e+06", "1.2e+06")]
    public void NumericWithTolerance_ReturnsFalse_WhenOutside10Percent(string left, string right)
    {
        Assert.False(Pairing41ScenarioRunner.NumericWithTolerance(left, right));
    }

    [Fact]
    public void ToMassTon_ConvertsKilogramsToTons()
    {
        var tons = Pairing41ScenarioRunner.ToMassTon("1000");
        Assert.True(Pairing41ScenarioRunner.NumericWithTolerance(tons, "1"));
    }
}
