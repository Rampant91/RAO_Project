using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Client_App.Resources.CustomConverters;

/// <summary>
/// CalendarDatePicker &lt;-&gt; date string without rewriting partial typed input.
/// Only a complete dd.MM.yyyy (or culture short date of length 10) updates the picker.
/// </summary>
public class ExactDateStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string dateString || string.IsNullOrWhiteSpace(dateString))
            return null;

        if (DateTime.TryParseExact(
                dateString,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var exact))
            return exact;

        if (dateString.Length == 10
            && DateTime.TryParse(dateString, culture, DateTimeStyles.None, out var parsed))
            return parsed;

        return BindingOperations.DoNothing;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
            return dateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

        return BindingOperations.DoNothing;
    }
}
