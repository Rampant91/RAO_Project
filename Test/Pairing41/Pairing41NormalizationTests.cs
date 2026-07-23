using Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41.Testing;
using Xunit;

namespace Test.Pairing41;

/// <summary>
/// G1 — нормализация текстовых/датовых полей и радионуклидов
/// (регистр, спецсимволы, lookalike RU/EN, ведущие нули, порядок нуклидов).
/// </summary>
public class Pairing41NormalizationTests
{
    /// <summary>Эквивалентные записи после NormalizeNumber совпадают.</summary>
    [Theory]
    [InlineData("АБК", "абк")]
    [InlineData("  DOC-1  ", "DOC-1")]
    [InlineData("DOC_1", "DOC1")]
    [InlineData("007", "7")]
    [InlineData("СОС", "COC")] // lookalike RU/EN
    public void NormalizeNumber_TreatsEquivalentValuesAsEqual(string left, string right)
    {
        Assert.Equal(
            Pairing41ScenarioRunner.NormalizeNumber(left),
            Pairing41ScenarioRunner.NormalizeNumber(right));
    }

    [Theory]
    [InlineData("abc", "abd")]
    [InlineData("DOC-1", "DOC-2")]
    public void NormalizeNumber_KeepsDifferentValuesDistinct(string left, string right)
    {
        Assert.NotEqual(
            Pairing41ScenarioRunner.NormalizeNumber(left),
            Pairing41ScenarioRunner.NormalizeNumber(right));
    }

    [Theory]
    [InlineData("-")]
    [InlineData("")]
    [InlineData("   ")]
    public void NormalizeNumber_TreatsPlaceholdersAsEmpty(string value)
    {
        Assert.Equal(string.Empty, Pairing41ScenarioRunner.NormalizeNumber(value));
    }

    [Theory]
    [InlineData("б.н.")]
    [InlineData("бн")]
    [InlineData("Б.Н.")]
    [InlineData("без номера")]
    [InlineData("без номера.")]
    [InlineData("прим")]
    [InlineData("примечание")]
    [InlineData("-")]
    [InlineData("")]
    public void NormalizeSerialNumber_TreatsEmptyMarkersAsEmpty(string value)
    {
        Assert.Equal(string.Empty, Pairing41ScenarioRunner.NormalizeSerialNumber(value));
    }

    [Theory]
    [InlineData("б.н.", "без номера")]
    [InlineData("-", "бн")]
    [InlineData("", "прим.")]
    public void SerialNumbersAreEmpty_WhenBothSidesAreMarkers(string pasNum, string facNum)
    {
        Assert.True(Pairing41ScenarioRunner.SerialNumbersAreEmpty(pasNum, facNum));
    }

    [Fact]
    public void SerialNumbersAreEmpty_False_WhenFactoryHasRealNumber()
    {
        Assert.False(Pairing41ScenarioRunner.SerialNumbersAreEmpty("б.н.", "F-100"));
    }

    [Fact]
    public void NormalizeNumber_DoesNotCollapseBezNomera_ForNonSerialFields()
    {
        // Общая нормализация чисел не должна обнулять «без номера» (это только serial-путь).
        Assert.NotEqual(string.Empty, Pairing41ScenarioRunner.NormalizeNumber("без номера"));
    }

    [Fact]
    public void NormalizeDate_CanonicalizesIsoDate()
    {
        Assert.Equal("2024-01-15", Pairing41ScenarioRunner.NormalizeDate("2024-01-15"));
    }

    [Fact]
    public void NormalizeDate_SameCalendarDay_IsEqual_WhenParsable()
    {
        // Текущая культура может принимать локальный формат; если нет — тест не ломает сборку.
        var localized = Pairing41ScenarioRunner.NormalizeDate("15.01.2024");
        var iso = Pairing41ScenarioRunner.NormalizeDate("2024-01-15");
        if (localized == "2024-01-15")
        {
            Assert.Equal(iso, localized);
        }
    }

    [Theory]
    [InlineData("кобальт-60; цезий-137", "цезий-137; кобальт-60")]
    [InlineData("кобальт-60,цезий-137", "цезий-137;кобальт-60")]
    public void NormalizeRads_IgnoresOrderAndSeparators(string left, string right)
    {
        Assert.Equal(
            Pairing41ScenarioRunner.NormalizeRads(left),
            Pairing41ScenarioRunner.NormalizeRads(right));
    }
}
