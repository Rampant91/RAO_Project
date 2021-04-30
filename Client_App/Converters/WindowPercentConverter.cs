using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Client_App.Converters
{
    public class WindowPercentConverter_Height : IValueConverter
    {
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            try
            {
                double val = System.Convert.ToDouble(Value);
                if (!double.IsNaN(val))
                {
                    double per = System.Convert.ToDouble(Param.ToString().Replace("%", ""));
                    return val / 100 * per;
                }
                else
                {
                    return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }
        }
        public object ConvertBack(object Value, Type tp, object Param, CultureInfo info)
        {
            try
            {
                double val = System.Convert.ToDouble(Value);
                if (!double.IsNaN(val))
                {
                    double per = System.Convert.ToDouble(Param.ToString().Replace("%", ""));
                    return val / per * 100;
                }
                else
                {
                    return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }
        }
    }
    public class WindowPercentConverter_Width : IValueConverter
    {
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            try
            {
                double val = System.Convert.ToDouble(Value);
                if (!double.IsNaN(val))
                {
                    double per = System.Convert.ToDouble(Param.ToString().Replace("%", ""));
                    return val / 100 * per;
                }
                else
                {
                    return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }
        }
        public object ConvertBack(object Value, Type tp, object Param, CultureInfo info)
        {
            try
            {
                double val = System.Convert.ToDouble(Value);
                if (!double.IsNaN(val))
                {
                    double per = System.Convert.ToDouble(Param.ToString().Replace("%", ""));
                    return val / per * 100;
                }
                else
                {
                    return double.NaN;
                }
            }
            catch
            {
                return double.NaN;
            }
        }
    }
}
