using System;
using System.Collections.Generic;
using Avalonia.Data.Converters;

namespace Client_App.Resources.CustomConverters;

public class AppLaunchedAndSelectedReportsVisibleConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (values.Count < 2)
        {
            return false;
        }

        var appLaunchedAtNorao = values[0] is bool b && b;
        var hasSelectedReports = values[1] is not null;

        return appLaunchedAtNorao && hasSelectedReports;
    }
}
