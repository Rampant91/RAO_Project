using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Client_App.Resources.CustomConverters;

public class RadioButtonCheckedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString() == parameter?.ToString();
    }

    public object ConvertBack(object isChecked, Type targetType, object parameter, CultureInfo culture)
    {
        return isChecked is true ? parameter : BindingOperations.DoNothing;
    }
}