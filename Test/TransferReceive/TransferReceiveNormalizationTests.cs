using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using Xunit;

namespace Test.TransferReceive;

/// <summary>
/// G1 — нормализация текстовых/датовых полей и радионуклидов.
/// </summary>
public class TransferReceiveNormalizationTests
{
    [Theory]
    [InlineData("АБК", "абк")]
    [InlineData("  DOC-1  ", "DOC-1")]
    [InlineData("DOC_1", "DOC1")]
    [InlineData("007", "7")]
    [InlineData("СОС", "COC")]
    public void NormalizeNumber_TreatsEquivalentValuesAsEqual(string left, string right)
    {
        Assert.Equal(
            TransferReceiveScenarioRunner.NormalizeNumber(left),
            TransferReceiveScenarioRunner.NormalizeNumber(right));
    }

    [Theory]
    [InlineData("abc", "abd")]
    [InlineData("DOC-1", "DOC-2")]
    public void NormalizeNumber_KeepsDifferentValuesDistinct(string left, string right)
    {
        Assert.NotEqual(
            TransferReceiveScenarioRunner.NormalizeNumber(left),
            TransferReceiveScenarioRunner.NormalizeNumber(right));
    }

    [Theory]
    [InlineData("-")]
    [InlineData("")]
    [InlineData("   ")]
    public void NormalizeNumber_TreatsPlaceholdersAsEmpty(string value)
    {
        Assert.Equal(string.Empty, TransferReceiveScenarioRunner.NormalizeNumber(value));
    }

    [Theory]
    [InlineData("б.н.")]
    [InlineData("бн")]
    [InlineData("Б.Н.")]
    [InlineData("без номера")]
    [InlineData("-")]
    public void SerialNumbersAreEmpty_RecognizesPlaceholders(string marker)
    {
        Assert.True(TransferReceiveScenarioRunner.SerialNumbersAreEmpty(marker, marker));
    }

    [Theory]
    [InlineData("Cs-137, Co-60", "Co-60,Cs-137")]
    [InlineData("Cs-137;Co-60", "Co-60, Cs-137")]
    public void NormalizeRads_IgnoresOrderAndSeparators(string left, string right)
    {
        Assert.Equal(
            TransferReceiveScenarioRunner.NormalizeRads(left),
            TransferReceiveScenarioRunner.NormalizeRads(right));
    }

    [Theory]
    [InlineData("15.06.2024", "2024-06-15")]
    [InlineData("2024-06-15", "2024-06-15")]
    public void NormalizeDate_ParsesEquivalentDates(string left, string right)
    {
        Assert.Equal(
            TransferReceiveScenarioRunner.NormalizeDate(left),
            TransferReceiveScenarioRunner.NormalizeDate(right));
    }
}
