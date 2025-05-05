using Avalonia.Data.Converters;
using System;

namespace Client_App.Resources.CustomConverters;

public class BoolInverterConverter : IValueConverter
{
    #region IValueConverter Members

    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is bool b)
        {
            return !b;
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is bool b)
        {
            return !b;
        }
        return value;
    }

    #endregion
}