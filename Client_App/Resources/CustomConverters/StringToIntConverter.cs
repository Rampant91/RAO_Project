using Avalonia.Data.Converters;
using System;

namespace Client_App.Resources.CustomConverters;

public class StringToIntConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is uint uintValue)
        {
            return uintValue;
        }
        return (uint)1;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return value?.ToString();
    }
}