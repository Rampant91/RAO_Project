using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Client_App.VisualRealization.Converters;

public class VectorToMarginLeft_Converter : IValueConverter
{
    public object Convert(object Value, Type tp, object Param, CultureInfo info)
    {
        if (Value != null)
        {
            var rps = (Vector)Value;
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
    public object ConvertBack(object Value, Type tp, object Param, CultureInfo info)
    {
        //
        return new Vector();
    }
}