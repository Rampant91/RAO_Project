using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Media;

namespace Client_App.Resources.CustomConverters;

/// <summary>
/// Если value == null, возвращает содержимое подсказки (Border + перенос текста, фон как у «инфо»).
/// Если value не null — null, подсказка не показывается.
/// Текст задаётся через ConverterParameter (строка).
/// </summary>
public class NullToTooltipConverter : IValueConverter
{
    private const double TipMaxWidth = 400;

    /// <summary>Фон в духе SystemColors.Info (Windows) — хорошо читается на белом.</summary>
    private static readonly SolidColorBrush TipBackground =
        new(Color.FromRgb(255, 255, 225));

    private static readonly SolidColorBrush TipBorderBrush =
        new(Color.FromRgb(200, 196, 170));

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not null)
            return null;

        var text = parameter switch
        {
            null => null,
            string s when string.IsNullOrWhiteSpace(s) => null,
            string s => s,
            _ => parameter.ToString()
        };

        if (string.IsNullOrEmpty(text))
            return null;

        var textBlock = new TextBlock
        {
            Text = text,
            TextWrapping = TextWrapping.Wrap,
            MaxWidth = TipMaxWidth,
            Foreground = Brushes.Black,
            TextAlignment = TextAlignment.Left
        };

        return new Border
        {
            Background = TipBackground,
            BorderBrush = TipBorderBrush,
            BorderThickness = new Thickness(1),
            Padding = new Thickness(10, 8),
            Child = textBlock
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
