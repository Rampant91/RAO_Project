using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Client_App.VisualRealization.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateTime)
            {
                return dateTime.ToString("dd.MM.yyyy");
            }
            return "";

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateOnly.TryParse(
                    dateString,
                    out DateOnly result))
                {
                    return result;
                }
            }
            return DateOnly.MinValue;
        }
    }
}
