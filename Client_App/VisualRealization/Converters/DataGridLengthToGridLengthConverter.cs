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
                // Преобразование типов ширины
                return dataGridLength.UnitType switch
                {
                    DataGridLengthUnitType.Pixel => new GridLength(dataGridLength.Value),
                    DataGridLengthUnitType.Auto => new GridLength(dataGridLength.DisplayValue),
                    DataGridLengthUnitType.SizeToHeader => new GridLength(dataGridLength.DisplayValue),
                    DataGridLengthUnitType.SizeToCells => new GridLength(dataGridLength.Value),
                    DataGridLengthUnitType.Star => new GridLength(dataGridLength.Value, GridUnitType.Star),
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
