using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;



namespace Client_App.VisualRealization.Converters
{
    public class IntToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue == 0 ? "-" : intValue.ToString();
            }

            // Если значение не int, возвращаем пустую строку
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (int.TryParse(stringValue, out int result))
                    return result;

                return 0;

            }

            // Невозможно преобразовать - вернуть UnsetValue для отмены изменения
            return BindingOperations.DoNothing;
        }
    }
}
