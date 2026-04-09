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
                if (DateTime.TryParse(
                    dateString,
                    out DateTime date))
                {
                    return date;
                }
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd.MM.yyyy");
            }
                return "";
        }
    }
}
