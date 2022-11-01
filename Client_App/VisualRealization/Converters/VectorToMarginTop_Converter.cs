using Avalonia.Data.Converters;
using Avalonia;
using System;
using System.Globalization;

namespace Client_App.Converters
{
    public class VectorToMarginTop_Converter : IValueConverter
    {
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            if (Value != null)
            {
                if (Value is Vector)
                {
                    Vector rps = (Vector)Value;
                    try
                    {
                        var ty = (int)rps.Y;
                        var lg = Thickness.Parse(("0," + ty) + ",0,0");
                        return lg;
                    }
                    catch
                    {

                    }
                }
                if (Value is double)
                {
                    try
                    {
                        int ty = System.Convert.ToInt32(Value);
                        var lg = Thickness.Parse(("0," + ty) + ",0,0");
                        return lg;
                    }
                    catch
                    {

                    }
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
}
