using Avalonia;
using Avalonia.Data.Converters;
using Client_App.Services.DataAccess;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Client_App.VisualRealization.Converters;

/// <summary>
/// Активность кнопок фильтра форм (1.1…1.9). Нельзя опираться на SelectedReports.Report_Collection —
/// оболочки отчётов больше не preload'ятся; наличиеём stubs из warm-cache / БД.
/// </summary>
public class CheckReportsAnyMatchingFormNumConverter : IValueConverter, IMultiValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Reports selectedReports && parameter is string formNum)
            return HasForm(selectedReports, formNum);
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is null || values.Count < 2)
            return false;

        if (values[0] == AvaloniaProperty.UnsetValue || values[1] == AvaloniaProperty.UnsetValue)
            return false;

        if (values[0] is Reports selectedReports && values[1] is string formNum)
            return HasForm(selectedReports, formNum);

        return false;
    }

    private static bool HasForm(Reports selectedReports, string formNum)
    {
        var cached = Forms1WarmCache.Instance.TryHasFormNum(selectedReports.Id, formNum);
        if (cached.HasValue)
            return cached.Value;

        // Не грузим stubs в Convert (UI-поток): кнопки обновятся после async OnPropertyChanged(SelectedReports).
        return selectedReports.Report_Collection.Any(rep => rep.FormNum_DB == formNum);
    }
}
