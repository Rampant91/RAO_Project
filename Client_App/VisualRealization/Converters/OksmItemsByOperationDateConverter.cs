using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Client_App.ViewModels.Forms.Forms1.Items;
using Spravochniki;

namespace Client_App.VisualRealization.Converters;

/// <summary>
/// Выпадающий список ОКСМ по дате операции строки.
/// Пустая/неразобранная дата → актуальные наименования.
/// </summary>
public sealed class OksmItemsByOperationDateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var operationDate = value as string ?? value?.ToString() ?? string.Empty;
        var map = Spravochniks.GetOksmForOperationDate(operationDate);
        return new ObservableCollection<OksmItem>(
            map.OrderBy(pair => pair.Key, StringComparer.Ordinal)
                .Select(pair => new OksmItem
                {
                    Code = pair.Key,
                    Country = pair.Value
                }));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
