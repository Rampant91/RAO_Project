using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Client_App.VisualRealization.Converters;

public class StringEqualsToColorConverter : IValueConverter, IMultiValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return Brushes.WhiteSmoke;

        string? firstString = value.ToString();
        string? secondString = parameter.ToString();

        bool areEqual = string.Equals(firstString, secondString);

        return areEqual ? Brushes.LightGreen : Brushes.WhiteSmoke;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is null || values.Count < 2)
            return Brushes.WhiteSmoke;

        if (values[0] == AvaloniaProperty.UnsetValue || values[1] == AvaloniaProperty.UnsetValue)
            return Brushes.WhiteSmoke;

        string? firstString = values[0]?.ToString();
        string? secondString = values[1]?.ToString();

        if (firstString == null || secondString == null)
            return Brushes.WhiteSmoke;

        bool areEqual = string.Equals(firstString, secondString);

        return areEqual ? Brushes.LightGreen : Brushes.WhiteSmoke;
    }
}