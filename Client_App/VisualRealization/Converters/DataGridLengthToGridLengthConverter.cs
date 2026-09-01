using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.VisualRealization.Converters
{
    public class DataGridLengthToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridLength dataGridLength)
            {
                var pixels = dataGridLength.IsAbsolute || dataGridLength.IsSizeToCells
                    ? dataGridLength.Value
                    : dataGridLength.DisplayValue;
                if (!double.IsFinite(pixels) || pixels < 0)
                {
                    return GridLength.Auto;
                }

                return dataGridLength.UnitType switch
                {
                    DataGridLengthUnitType.Pixel => new GridLength(pixels),
                    DataGridLengthUnitType.Auto => new GridLength(pixels),
                    DataGridLengthUnitType.SizeToHeader => new GridLength(pixels),
                    DataGridLengthUnitType.SizeToCells => new GridLength(pixels),
                    DataGridLengthUnitType.Star => double.IsFinite(dataGridLength.Value) && dataGridLength.Value > 0
                        ? new GridLength(dataGridLength.Value, GridUnitType.Star)
                        : GridLength.Auto,
                    _ => GridLength.Auto
                };
            }
            return GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
