using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client_App.VisualRealization.Converters;

public class ByteNullableToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte byteValue)
        {
            return byteValue.ToString();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && byte.TryParse(stringValue, out var byteValue))
        {
            return byteValue;
        }
        return null;
    }
}
