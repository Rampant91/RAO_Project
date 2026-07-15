using Models.Comparers.FormContent;
using Xunit;

namespace Test;

public class FormTextEqualityTests
{
    [Theory]
    [InlineData("ABC", "abc")] // Игнорируется регистр
    [InlineData("ПрИмЕр", "пример")] // Игнорируется регистр в кириллице
    public void Equals_ReturnsTrue_WhenOnlyCaseDiffers(string left, string right)
    {
        Assert.True(FormTextEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("СОС", "COC")] // Кириллические С/О эквивалентны латинским C/O
    [InlineData("МАРКЕР", "MAPKEP")] // Похожие буквы кириллица/латиница в одном слове
    [InlineData("РОСАТОМ", "POCATOM")] // Смешение алфавитов даёт одинаковую нормализованную строку
    [InlineData("т0м", "том")] // Ноль и буква O считаются идентичными
    public void Equals_ReturnsTrue_WhenOnlyLookalikeAlphabetCharsDiffer(string left, string right)
    {
        Assert.True(FormTextEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("A-B", "A—B")] // Разные unicode тире приводятся к '-'
    [InlineData("ООО Ромашка", "ООО\tРомашка")] // Пробел и таб как эквивалентные разделители
    public void Equals_ReturnsTrue_WhenOnlyDashSymbolDiffers(string left, string right)
    {
        Assert.True(FormTextEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("A/B:C", "ABC")] // Спецсимволы удаляются
    [InlineData("  12-34  ", "1234")] // Краевые пробелы и '-' удаляются regex-ом
    [InlineData("test_value", "testvalue")] // '_' удаляется
    [InlineData("a+b", "ab")] // '+' удаляется (входит в класс спецсимволов)
    public void Equals_ReturnsTrue_WhenOnlySpecialSymbolsDiffer(string left, string right)
    {
        Assert.True(FormTextEquality.Equals(left, right));
    }

    [Theory]
    [InlineData("abc", "abd")] // Отличается символ
    [InlineData("тест1", "тест2")] // Отличается цифра в конце
    [InlineData("мир", "mix")] // После подмены похожих символов строки остаются разными
    public void Equals_ReturnsFalse_WhenNormalizedTextDiffers(string left, string right)
    {
        Assert.False(FormTextEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(null, null, true)] // Оба null
    [InlineData("", "   ", true)] // Обе строки пустые после trim
    [InlineData(null, "", true)] // null и пустая строка нормализуются в пустую
    [InlineData(null, "abc", false)] // null не равен непустому тексту
    public void Equals_HandlesNullAndWhitespaceValues(string? left, string? right, bool expected)
    {
        Assert.Equal(expected, FormTextEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(" A/B:C ", "abc")] // trim + удаление спецсимволов + lower
    [InlineData("тЕсТ—1", "tect1")] // регистр + тире + подмена похожих символов
    [InlineData("СОС", "coc")] // подмена похожих кириллических символов
    [InlineData("т0м", "tom")] // ноль и буква o
    [InlineData(null, "")] // null нормализуется в пустую строку
    [InlineData("   ", "")] // whitespace-only в пустую строку
    public void Normalize_ReturnsExpectedCanonicalForm(string? input, string expected)
    {
        Assert.Equal(expected, FormTextEquality.Normalize(input));
    }
}
