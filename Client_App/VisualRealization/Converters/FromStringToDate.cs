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
    public class FromStringToDate:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((double)value>(365*24*60*60))
            {
                return ((double)value / (365 * 24 * 60 * 60)).ToString() + " лет.";
            }
            if ((double)value > (24 * 60 * 60)&& (double)value < (365 * 24 * 60 * 60))
            {
                return ((double)value / (24 * 60 * 60)).ToString() + " сут.";
            }
            if ((double)value > (60 * 60)&& (double)value < (24 * 60 * 60))
            {
                return ((double)value / (60 * 60)).ToString() + " час.";
            }
            if ((double)value > (60)&& (double)value < (60 * 60))
            {
                return ((double)value / (60 * 60)).ToString() + " мин.";
            }
            return ((double)value).ToString() + " сек.";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double num = System.Convert.ToDouble(((string)value).Split(' ')[0]);
            string suffix= ((string)value).Split(' ')[1];
            if (suffix=="лет.")
            {
                return (num * (365 * 24 * 60 * 60));
            }
            if (suffix == "сут.")
            {
                return (num * (24 * 60 * 60));
            }
            if (suffix == "час.")
            {
                return (num * (60 * 60));
            }
            if (suffix == "мин.")
            {
                return (num * (60));
            }
            return (num);
        }
    }
}
