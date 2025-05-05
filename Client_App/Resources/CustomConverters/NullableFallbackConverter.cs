using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Client_App.Resources.CustomConverters;

public class NullableFallbackConverter : IValueConverter
{
    public static NullableFallbackConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // return just the value. We are only interested in convert back
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Return the parameter if value was null. Parameter must be our fallback value. 
        if (value is DateTime dateTimeValue)
        {
            return DateOnly.FromDateTime(dateTimeValue);
        }
        return value ?? parameter;
    }

    public static DateOnly GetDateOnlyNow { get; } = DateOnly.FromDateTime(DateTime.Now);
    
}