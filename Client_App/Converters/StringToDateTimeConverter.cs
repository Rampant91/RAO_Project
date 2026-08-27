using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client_App.Converters
{
    public class StringToDateTimeConverter : IValueConverter
    {
        public static StringToDateTimeConverter Instance { get; } = new StringToDateTimeConverter();
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                {
                    return dateTime;
                }
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd.MM.yyyy");
            }
            return null;
        }
    }
}
