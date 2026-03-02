using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Client_App.VisualRealization.Converters
{
    public class StringToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateOnly.TryParse(
                    dateString,
                    out DateOnly dateOnly))
                {
                    return dateOnly;
                }
            }
            return DateOnly.MinValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
            {
                return dateOnly.ToString("dd.MM.yyyy");
            }
            return "";
        }
    }
}
