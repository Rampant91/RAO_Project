using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client_App.VisualRealization.Converters;

public class ShortNullableToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is short shortValue)
        {
            return shortValue.ToString();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && short.TryParse(stringValue, out var shortValue))
        {
            return shortValue;
        }
        return null;
    }
}
