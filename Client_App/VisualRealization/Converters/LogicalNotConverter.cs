using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Client_App.VisualRealization.Converters;
public class LogicalNotConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b ? !b : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b ? !b : value;
    }
}