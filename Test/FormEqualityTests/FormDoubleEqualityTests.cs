using Models.Comparers.FormContent;
using Xunit;

namespace Test.FormEqualityTests;

public class FormDoubleEqualityTests
{
    [Theory]
    [InlineData(1.0, 1.0 + 1e-10)] // В пределах абсолютного допуска по умолчанию
    [InlineData(1_000_000.0, 1_000_001.0)] // В пределах относительного допуска по умолчанию
    [InlineData(0.0, 1e-10)] // Нулевое и почти нулевое значение
    public void Equals_Double_ReturnsTrue_WhenDifferenceWithinTolerance(double left, double right)
    {
        Assert.True(FormDoubleEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(1.0, 1.001)] // Разница больше относительного допуска
    [InlineData(0.0, 1e-6)] // Разница больше абсолютного допуска
    public void Equals_Double_ReturnsFalse_WhenDifferenceExceedsTolerance(double left, double right)
    {
        Assert.False(FormDoubleEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(double.NaN, double.NaN, true)] // Оба NaN считаются равными
    [InlineData(double.NaN, 1.0, false)] // NaN и число не равны
    [InlineData(1.0, double.NaN, false)] // Число и NaN не равны
    [InlineData(double.PositiveInfinity, double.PositiveInfinity, true)] // Одинаковые бесконечности равны
    [InlineData(double.NegativeInfinity, double.PositiveInfinity, false)] // Разные бесконечности не равны
    public void Equals_Double_HandlesNaNAndInfinity(double left, double right, bool expected)
    {
        Assert.Equal(expected, FormDoubleEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(1f, 1.000009f)] // В пределах допуска float по умолчанию
    [InlineData(-100f, -100.000009f)] // В пределах допуска для отрицательных чисел
    public void Equals_Float_ReturnsTrue_WhenDifferenceWithinAbsoluteTolerance(float left, float right)
    {
        Assert.True(FormDoubleEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(1f, 1.00002f)] // Выше float-допуска по умолчанию
    [InlineData(-10f, -10.00002f)] // Выше float-допуска для отрицательных чисел
    public void Equals_Float_ReturnsFalse_WhenDifferenceExceedsAbsoluteTolerance(float left, float right)
    {
        Assert.False(FormDoubleEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(float.NaN, float.NaN, true)] // Оба NaN считаются равными
    [InlineData(float.NaN, 1f, false)] // NaN и число не равны
    [InlineData(1f, float.NaN, false)] // Число и NaN не равны
    [InlineData(float.PositiveInfinity, float.PositiveInfinity, true)] // Одинаковые бесконечности равны
    [InlineData(float.NegativeInfinity, float.PositiveInfinity, false)] // Разные бесконечности не равны
    public void Equals_Float_HandlesNaNAndInfinity(float left, float right, bool expected)
    {
        Assert.Equal(expected, FormDoubleEquality.Equals(left, right));
    }

    [Theory]
    [InlineData(null, null, true)] // Оба nullable не имеют значения
    [InlineData(null, 1f, false)] // Null и число не равны
    [InlineData(1f, null, false)] // Число и null не равны
    [InlineData(1f, 1.000009f, true)] // Nullable float в пределах допуска
    [InlineData(1f, 1.00002f, false)] // Nullable float за пределом допуска
    public void Equals_NullableFloat_HandlesNullsAndTolerance(float? left, float? right, bool expected)
    {
        Assert.Equal(expected, FormDoubleEquality.Equals(left, right));
    }

    [Fact]
    public void Equals_Double_UsesCustomTolerances()
    {
        const double left = 1.0;
        const double right = 1.1;

        Assert.False(FormDoubleEquality.Equals(left, right));
        Assert.True(FormDoubleEquality.Equals(left, right, absoluteTolerance: 0.2, relativeTolerance: 0.2));
    }
}
