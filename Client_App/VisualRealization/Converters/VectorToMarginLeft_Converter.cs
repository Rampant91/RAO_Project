using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Client_App.VisualRealization.Converters;

public class VectorToMarginLeft_Converter : IValueConverter
{
    public object Convert(object value, Type tp, object param, CultureInfo info)
    {
        if (value != null)
        {
            var rps = (Vector)value;
            try
            {
                var lg = Thickness.Parse($"{(int)rps.X},0,0,0");
                return lg;
            }
            catch
            {

            }
        }
        return null;
    }
    public object ConvertBack(object value, Type tp, object param, CultureInfo info)
    {
        //
        return new Vector();
    }
}