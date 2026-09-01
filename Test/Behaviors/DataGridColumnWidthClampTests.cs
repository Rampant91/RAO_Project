using Client_App.Behaviors.DataGrid;
using Xunit;

namespace Test.Behaviors;

public class DataGridColumnWidthClampTests
{
    [Fact]
    public void Clamp_ReplacesNonFiniteMax_WithFallback()
    {
        var result = DataGridColumnWidthClamp.Clamp(80, 20, double.PositiveInfinity, 30, double.PositiveInfinity);

        Assert.True(double.IsFinite(result));
        Assert.Equal(80, result);
    }

    [Fact]
    public void Clamp_RejectsInfiniteWidth()
    {
        var result = DataGridColumnWidthClamp.Clamp(double.PositiveInfinity, 0, 0, 0, 0);

        Assert.True(double.IsFinite(result));
        Assert.Equal(DataGridColumnWidthClamp.FallbackMin, result);
    }

    [Fact]
    public void Clamp_UsesColumnLimits_WhenFinite()
    {
        var result = DataGridColumnWidthClamp.Clamp(400, 112, 200, 48, 320);

        Assert.Equal(200, result);
    }

    [Fact]
    public void Clamp_UsesGridMax_WhenColumnMaxIsInfinity()
    {
        var result = DataGridColumnWidthClamp.Clamp(900, 0, double.PositiveInfinity, 48, 320);

        Assert.Equal(320, result);
    }

    [Fact]
    public void Clamp_NeverReturnsNaNOrInfinity()
    {
        double[] samples =
        [
            double.NaN,
            double.PositiveInfinity,
            double.NegativeInfinity,
            -10,
            0
        ];

        foreach (var width in samples)
        {
            var result = DataGridColumnWidthClamp.Clamp(width, double.PositiveInfinity, double.NaN, double.NegativeInfinity, 0);
            Assert.True(double.IsFinite(result));
            Assert.True(result > 0);
        }
    }

    [Fact]
    public void SanitizeSavedWidths_ReplacesInvalidValuesWithZero()
    {
        var result = DataGridColumnWidthClamp.SanitizeSavedWidths(
        [
            53,
            double.PositiveInfinity,
            double.NaN,
            -4,
            0
        ]);

        Assert.Equal([53d, 0d, 0d, 0d, 0d], result);
    }
}
