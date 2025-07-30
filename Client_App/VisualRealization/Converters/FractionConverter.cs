using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.VisualRealization.Converters
{
    public class FractionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && parameter is string paramString)
            {
                if (int.TryParse(paramString, out int denominator) && denominator > 0)
                {
                    // Рассчитываем 1/20 от ширины окна, вычитая отступы
                    double effectiveWidth = width - 100; // 20px с каждой стороны
                    return effectiveWidth / denominator; // Минимальная ширина 50px
                }
            }
            return double.NaN;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
