using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client_App.Resources.CustomConverters;

/// <summary>Возвращает true, если value == null.</summary>
public class NullToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
