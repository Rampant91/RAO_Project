using Client_App.Behaviors;
using Xunit;

namespace Test.Behaviors;

public class CalculatorInputPatternsTests
{
    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("1.")]
    [InlineData("1,2")]
    [InlineData("1.2e")]
    [InlineData("1.2e-")]
    [InlineData("1.2e-3")]
    [InlineData("1.2E+03")]
    [InlineData("+5")]
    [InlineData("1\u0435-2")]
    public void PositiveExponential_AllowsValidPartialAndComplete(string text)
    {
        Assert.Matches(CalculatorInputPatterns.PositiveExponential, text);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("1b")]
    [InlineData("-1")]
    [InlineData("1e1e2")]
    [InlineData("1..2")]
    public void PositiveExponential_RejectsNoise(string text)
    {
        Assert.DoesNotMatch(CalculatorInputPatterns.PositiveExponential, text);
    }

    [Theory]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("12")]
    [InlineData("1.")]
    [InlineData("1,5")]
    [InlineData(".5")]
    public void PositiveDecimal_AllowsValidPartialAndComplete(string text)
    {
        Assert.Matches(CalculatorInputPatterns.PositiveDecimal, text);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("-1")]
    [InlineData("1e3")]
    [InlineData("1.2.3")]
    public void PositiveDecimal_RejectsNoise(string text)
    {
        Assert.DoesNotMatch(CalculatorInputPatterns.PositiveDecimal, text);
    }

    [Theory]
    [InlineData("")]
    [InlineData("09.09.2026")]
    [InlineData("1")]
    [InlineData("..")]
    public void DateDigitsAndDots_AllowsDigitsAndDots(string text)
    {
        Assert.Matches(CalculatorInputPatterns.DateDigitsAndDots, text);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("09-09-2026")]
    [InlineData("09/09/2026")]
    public void DateDigitsAndDots_RejectsOtherChars(string text)
    {
        Assert.DoesNotMatch(CalculatorInputPatterns.DateDigitsAndDots, text);
    }
}
