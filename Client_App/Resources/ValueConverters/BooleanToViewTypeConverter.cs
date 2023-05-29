using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Client_App.State.Navigation;

namespace Client_App.Resources.ValueConverters;

public class BooleanToViewTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
            throw new InvalidOperationException("The target must be a boolean");
        
        return (ViewType)value is ViewType.Oper && (bool)parameter
            ? ViewType.Oper
            : ViewType.Annual;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}