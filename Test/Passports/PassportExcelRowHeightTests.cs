using Client_App.Commands.AsyncCommands.ExcelExport.Passports;
using SkiaSharp;
using Xunit;

namespace Test.Passports;

/// <summary>
/// Фиксирует алгоритм word-wrap высоты строк паспорта (не менять без сверки с печатной формой).
/// </summary>
public sealed class PassportExcelRowHeightTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Calculate_EmptyOrWhitespace_ReturnsZero(string? text)
    {
        using var paint = CreatePaint();
        Assert.Equal(0, PassportExcelRowHeight.Calculate(text, paint, columnWidthPixels: 100));
    }

    [Fact]
    public void Calculate_SingleShortWord_IsOneLine()
    {
        using var paint = CreatePaint();
        var lineHeight = ExpectedLineHeight(paint);

        var height = PassportExcelRowHeight.Calculate("АБВ", paint, columnWidthPixels: 500);

        Assert.Equal(lineHeight, height, precision: 5);
    }

    [Fact]
    public void Calculate_ExplicitNewline_IsTwoLines()
    {
        using var paint = CreatePaint();
        var lineHeight = ExpectedLineHeight(paint);

        var height = PassportExcelRowHeight.Calculate("строка1\nстрока2", paint, columnWidthPixels: 500);

        Assert.Equal(2 * lineHeight, height, precision: 5);
    }

    [Fact]
    public void Calculate_LongPhrase_WrapsToMultipleLines()
    {
        using var paint = CreatePaint();
        var lineHeight = ExpectedLineHeight(paint);
        const string text = "очень длинная фраза которая не помещается в узкую колонку целиком";

        var height = PassportExcelRowHeight.Calculate(text, paint, columnWidthPixels: 40);

        Assert.True(height > lineHeight + 0.01, $"expected wrap, got height={height}, line={lineHeight}");
    }

    [Fact]
    public void PaintCache_ReusesSameInstance_ForSameFont()
    {
        using var cache = new ExcelFontPaintCache();
        using var package = new OfficeOpenXml.ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("t");
        ws.Cells[1, 1].Value = "a";
        ws.Cells[1, 1].Style.Font.Name = "Arial";
        ws.Cells[1, 1].Style.Font.Size = 11;
        ws.Cells[2, 1].Value = "b";
        ws.Cells[2, 1].Style.Font.Name = "Arial";
        ws.Cells[2, 1].Style.Font.Size = 11;

        var first = cache.Get(ws.Cells[1, 1].Style.Font);
        var second = cache.Get(ws.Cells[2, 1].Style.Font);

        Assert.Same(first, second);
    }

    private static SKPaint CreatePaint()
    {
        var typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
        return new SKPaint
        {
            Typeface = typeface,
            TextSize = 11,
            IsAntialias = true,
        };
    }

    private static double ExpectedLineHeight(SKPaint paint) =>
        1.15 * (paint.FontMetrics.Descent - paint.FontMetrics.Ascent);
}
