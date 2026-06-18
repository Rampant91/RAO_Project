using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Client_App.VisualRealization.Converters;

/// <summary>
/// ColumnSpan для «Сведения об операции» в фиксированной шапке: 2 при FrozenColumnCount >= 3, иначе 1.
/// </summary>
public class FrozenOperationHeaderColSpanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is int count && count >= 3 ? 2 : 1;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
