using System;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace RAO_Calculator.Converters
{
    public class FromStringToDouble:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)(value)).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tmp = ((string)(value)).Replace(".",",");
            try
            {
                return System.Convert.ToDouble((string)tmp);
            }
            catch
            {
                return 0;
            }
        }
    }
}
