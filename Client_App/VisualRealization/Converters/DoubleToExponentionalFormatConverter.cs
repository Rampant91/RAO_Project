using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.VisualRealization.Converters
{
    public class DoubleToExponentionalFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double doubleValue || doubleValue == 0)
                return 0.ToString();

            var signsCount = doubleValue.ToString().Length;
            var multipleOf10 = 10;
            while(doubleValue % multipleOf10 == 0)
            {
                signsCount--;
                multipleOf10 *= 10;
            }

            var length = int.Min(6, signsCount-1); // Максимум 6 знаков после запятой

            return doubleValue.ToString($"e{length}");//
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string expStr
                || (targetType != typeof(double)
                && targetType != (typeof(double?))))
                return 0.0;

            expStr = expStr.Replace(" ", "");  //удаляем все пробелы
            expStr = expStr.Replace('.', ','); //точка на запятую
            expStr = expStr.Replace('е', 'e'); //меняем кириллицу на латиницу
            expStr = expStr.Replace('Е', 'E'); //меняем кириллицу на латиницу

            if (double.TryParse(expStr,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                new CultureInfo("ru-RU", useUserOverride: false),
                out var result))
            {
                return result;
            }
            else
                return 0.0;

        }

    }
}
