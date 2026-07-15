using Models.Comparers.FormContent;
using Xunit;

namespace Test.FormEqualityTests;

public class FormRadionuclidsEqualityTests
{
    [Theory]
    [InlineData("cs-137; co-60", "cs-137; co-60")] // Одинаковый порядок
    [InlineData("Cs-137; Co-60", "cs137;co60")] // Регистр и пробелы не влияют
    [InlineData(null, null)] // Оба null
    [InlineData("", "   ")] // Обе строки пустые после нормализации
    public void Equals_ReturnsTrue_WhenOrderAndContentMatch(string? left, string? right)
    {
        Assert.True(FormRadionuclidsEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("cs-137; co-60", "co-60; cs-137")] // Разный порядок
    [InlineData("cs-137", "cs-137; co-60")] // Разное количество элементов
    [InlineData("cs-137; co-60", "cs-137; co-61")] // Отличается один элемент
    [InlineData(null, "cs-137")] // Null и непустая строка
    public void Equals_ReturnsFalse_WhenOrderOrContentDiffers(string? left, string? right)
    {
        Assert.False(FormRadionuclidsEquality.Equals(left, right));
    }
}
