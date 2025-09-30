using System;
using System.Globalization;
using System.Collections.Generic;
using Avalonia.Data.Converters;

namespace Client_App.VisualRealization.Converters;

public class IsOperation41AndSingleSelectionConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Count < 2)
            return false;

        var onlyOneSelected = values[0] is bool b && b;
        if (!onlyOneSelected)
            return false;

        var selectedForm = values[1];
        if (selectedForm is null)
            return false;

        string? opCode = TryGetOperationCode(selectedForm);
        return string.Equals(opCode, "41", StringComparison.Ordinal);
    }

    private static string? TryGetOperationCode(object form)
    {
        // Try property OperationCode.Value
        var type = form.GetType();
        var opCodeProp = type.GetProperty("OperationCode");
        if (opCodeProp != null)
        {
            var opObj = opCodeProp.GetValue(form);
            if (opObj != null)
            {
                var valProp = opObj.GetType().GetProperty("Value");
                var val = valProp?.GetValue(opObj)?.ToString();
                if (!string.IsNullOrWhiteSpace(val))
                    return val;
            }
        }

        // Fallback to OperationCode_DB
        var opDbProp = type.GetProperty("OperationCode_DB");
        var dbVal = opDbProp?.GetValue(form)?.ToString();
        if (!string.IsNullOrWhiteSpace(dbVal))
            return dbVal;

        return null;
    }
}