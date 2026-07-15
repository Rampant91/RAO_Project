using System;
using Models.Comparers.FormContent;
using Xunit;

namespace Test.FormEqualityTests;

public class FormDateEqualityTests
{
    [Theory]
    [InlineData("1.1.2021", "01.01.2021")] // День и месяц с/без ведущего нуля
    [InlineData("09.12.2020", "9.12.2020")] // Одинаковая дата в разных форматах
    public void Equals_ReturnsTrue_WhenOnlyDateNotationDiffers(string left, string right)
    {
        Assert.True(FormDateEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("\n 01.01.2021 \t", "01.01.2021")] // Лишние пробелы и переводы строк по краям
    [InlineData("0 1.0 1.2021", "01.01.2021")] // Внутренние пробелы в дате
    public void Equals_ReturnsTrue_WhenInputHasWhitespaceNoise(string left, string right)
    {
        Assert.True(FormDateEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("01.01.2021", "02.01.2021")] // Отличается день
    [InlineData("01.02.2021", "01.03.2021")] // Отличается месяц
    [InlineData("01.01.2021", "01.01.2022")] // Отличается год
    public void Equals_ReturnsFalse_WhenDateValueDiffers(string left, string right)
    {
        Assert.False(FormDateEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(null, null, true)] // Оба null
    [InlineData("", "   ", true)] // Обе строки пустые после trim
    [InlineData("-", "-", true)] // Оба служебный дефис
    [InlineData("прим.", "прим", true)] // Эквивалентные служебные строки после нормализации
    [InlineData(null, "01.01.2021", false)] // Null и валидная дата
    public void Equals_HandlesNullAndServiceValues(string? left, string? right, bool expected)
    {
        Assert.Equal(expected, FormDateEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("1.1.2021", true, 1, 1, 2021)] // День и месяц без ведущих нулей
    [InlineData("01.01.2021", true, 1, 1, 2021)] // Канонический формат даты
    [InlineData("\n 01.01.2021 \t", true, 1, 1, 2021)] // Пробелы и переводы строк по краям
    [InlineData("-", false, 1, 1, 1)] // Служебный дефис не парсится в дату
    [InlineData("прим.", false, 1, 1, 1)] // Служебная строка не парсится в дату
    [InlineData("32.01.2021", false, 1, 1, 1)] // Некорректный день месяца
    [InlineData("not-a-date", false, 1, 1, 1)] // Некорректный формат
    public void TryParse_ReturnsExpectedResult(string? input, bool shouldParse, int day, int month, int year)
    {
        var success = FormDateEquality.TryParse(input, out var actual);

        Assert.Equal(shouldParse, success);
        if (shouldParse)
        {
            Assert.Equal(new DateOnly(year, month, day), actual);
        }
    }
}
