using System;
using System.Collections.Generic;
using OfficeOpenXml.Style;
using SkiaSharp;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Passports;

/// <summary>
/// Подгонка высоты строк паспорта: измерение текста через SkiaSharp (кроссплатформенно, без Excel).
/// </summary>
internal static class PassportExcelRowHeight
{
    /// <summary>
    /// Высота текста в пунктах Excel при заданной ширине колонки в пикселях.
    /// Алгоритм word-wrap должен совпадать с прежним CalculateRowHeight.
    /// </summary>
    public static double Calculate(string? text, SKPaint paint, double columnWidthPixels)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return 0;
        }

        double lineHeight = 1.15 * (paint.FontMetrics.Descent - paint.FontMetrics.Ascent);

        text = text.Replace("\r", "");
        text = text.Replace("\n", " \n ");
        string[] words = text.Split(' ');
        var lines = new List<string>();
        string currentLine = "";

        foreach (var word in words)
        {
            if (word is "\n")
            {
                lines.Add(currentLine);
                currentLine = "";
                continue;
            }

            if (string.IsNullOrWhiteSpace(word))
            {
                continue;
            }

            string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
            if (paint.MeasureText(testLine) / 0.75 <= columnWidthPixels)
            {
                currentLine = testLine;
            }
            else
            {
                if (!string.IsNullOrEmpty(currentLine))
                {
                    lines.Add(currentLine);
                }

                currentLine = word;
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }

        return lines.Count * lineHeight;
    }
}

/// <summary>
/// Кеш SKPaint по параметрам шрифта ячейки — без пересоздания typeface на каждую ячейку.
/// </summary>
internal sealed class ExcelFontPaintCache : IDisposable
{
    private readonly Dictionary<FontKey, SKPaint> _paints = new();

    public SKPaint Get(ExcelFont excelFont)
    {
        var key = new FontKey(
            excelFont.Name ?? "",
            excelFont.Bold,
            excelFont.Italic,
            (float)excelFont.Size);

        if (_paints.TryGetValue(key, out var existing))
        {
            return existing;
        }

        var typeface = SKTypeface.FromFamilyName(
            key.Name,
            key.Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
            SKFontStyleWidth.Normal,
            key.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright);

        var paint = new SKPaint
        {
            Typeface = typeface,
            TextSize = key.Size,
            IsAntialias = true,
        };
        _paints[key] = paint;
        return paint;
    }

    public void Dispose()
    {
        foreach (var paint in _paints.Values)
        {
            paint.Typeface?.Dispose();
            paint.Dispose();
        }

        _paints.Clear();
    }

    private readonly record struct FontKey(string Name, bool Bold, bool Italic, float Size);
}
