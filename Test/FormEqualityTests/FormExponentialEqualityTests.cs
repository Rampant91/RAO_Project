using Models.Comparers.FormContent;
using Xunit;

namespace Test.FormEqualityTests;

public class FormExponentialEqualityTests
{
    [Theory]
    [InlineData("1e+3", "1000")] // Базовая экспоненциальная и десятичная форма
    [InlineData("1E+3", "1000")] // Латинская "E" в верхнем регистре
    [InlineData("1е+3", "1000")] // Кириллическая "е" вместо латинской "e"
    [InlineData("1,23e+10", "1.23E+10")] // Запятая и точка как разделители дробной части
    public void Equals_ReturnsTrue_WhenOnlyNumericNotationDiffers(string left, string right)
    {
        Assert.True(FormExponentialEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("1+5", "1e+5")] // Короткая форма степени через плюс
    [InlineData("1-5", "1e-5")] // Короткая форма степени через минус
    public void Equals_ReturnsTrue_WhenOnlyImplicitExponentDiffers(string left, string right)
    {
        Assert.True(FormExponentialEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("(1.5e+3)", "1,5e+3")] // Число в скобках и обычная запись
    [InlineData("(-2,5e-5)", "-0.000025")] // Отрицательное число в скобках
    [InlineData("(+2,5e-5)", "+0.000025")] // Положительное число в скобках
    public void Equals_ReturnsTrue_WhenOnlyBracketNotationDiffers(string left, string right)
    {
        Assert.True(FormExponentialEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("  1,0e+1  ", "10")] // Лишние пробелы по краям
    [InlineData("\n\t+0002,5000е+0002\t", "250")] // Переносы/табы и ведущие нули
    public void Equals_ReturnsTrue_WhenInputHasFormattingNoise(string left, string right)
    {
        Assert.True(FormExponentialEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("1e+3", "1001")] // Отличается целевая величина
    [InlineData("1e-3", "1e-2")] // Отличается порядок степени
    [InlineData("2,5e+1", "2,4e+1")] // Отличается мантисса
    public void Equals_ReturnsFalse_WhenNumericValueDiffers(string left, string right)
    {
        Assert.False(FormExponentialEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(null, null, true)] // Оба null
    [InlineData("", "   ", true)] // Обе строки пустые после trim
    [InlineData("-", "-", true)] // Оба служебный дефис
    [InlineData("прим.", "прим.", true)] // Оба служебное примечание
    [InlineData("прим.", "-", false)] // Разные служебные значения
    [InlineData(null, "1e+3", false)] // Null не равен числу
    public void Equals_HandlesNullAndServiceValues(string? left, string? right, bool expected)
    {
        Assert.Equal(expected, FormExponentialEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("1e+3", true, 1000d)] // Базовая экспоненциальная запись
    [InlineData("1е+3", true, 1000d)] // Кириллическая е
    [InlineData("1+3", true, 1000d)] // Неявная степень через плюс
    [InlineData("1-3", true, 0.001d)] // Неявная степень через минус
    [InlineData("(2,5e+2)", true, 250d)] // Скобочная запись
    [InlineData("-", false, 0d)] // Служебный дефис не парсится в число
    [InlineData("прим.", false, 0d)] // Служебное примечание не парсится в число
    [InlineData("not-a-number", false, 0d)] // Некорректный формат
    public void TryParse_ReturnsExpectedResult(string input, bool shouldParse, double expected)
    {
        var success = FormExponentialEquality.TryParse(input, out var actual);

        Assert.Equal(shouldParse, success);
        if (shouldParse)
        {
            Assert.True(FormDoubleEquality.Equals(expected, actual));
        }
    }
}
