using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client_App.VisualRealization.Converters;

/// <summary>
/// Конвертер для byte? с поддержкой прочерка "-".
/// null в БД отображается как "-", "-" от пользователя сохраняется как null.
/// Используется для форм 1.7 и 1.8 где прочерк допустим.
/// </summary>
public class ByteNullableOrDashToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte byteValue)
        {
            return byteValue.ToString();
        }
        // null отображаем как пустую строку
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            // Пустая строка или прочерк = null в БД
            if (string.IsNullOrWhiteSpace(stringValue) || stringValue == "-")
            {
                return null;
            }
            if (byte.TryParse(stringValue, out var byteValue))
            {
                return byteValue;
            }
        }
        return null;
    }
}
