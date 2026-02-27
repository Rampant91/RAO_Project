using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace Client_App.VisualRealization.Converters
{
    public class DoubleToExponentionalFormatConvarter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double doubleValue) return "Необходимо использовать double";


            return doubleValue.ToString($"e{doubleValue.ToString().Length-1}");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string expStr
                || targetType != typeof(double)) 
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
