using System;
using System.Collections.Generic;
using Avalonia.Data.Converters;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Models.Collections;

namespace Client_App.Resources.CustomConverters;

public class ExcelExportByFormCommandParameterConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (values.Count < 2)
        {
            return null;
        }

        var formNum = values[0] as string;
        var selectedReports = values[1] as Reports;

        if (string.IsNullOrWhiteSpace(formNum) || selectedReports is null)
        {
            return null;
        }

        return new ExcelExportAllFormsByFormNumberAsyncCommand.ExportByFormCommandParameter(formNum, selectedReports);
    }
}
