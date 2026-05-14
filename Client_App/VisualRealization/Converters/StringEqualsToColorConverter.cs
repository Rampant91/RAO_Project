using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Client_App.VisualRealization.Converters;

public class StringEqualsToColorConverter : IValueConverter
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
}