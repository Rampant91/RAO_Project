using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Client_App.VisualRealization.Converters
{
    public class IfNullShowAnotherValue : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count >= 2)
            {
                var mainValue = values[0];
                var fallbackValue = values[1];

                // Проверяем, является ли основное значение null или пустой строкой
                bool isEmpty = mainValue == null ||
                              (mainValue is string str && string.IsNullOrEmpty(str));

                return isEmpty ? fallbackValue : mainValue;
            }

            return values.Count > 0 ? values[0] : null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}