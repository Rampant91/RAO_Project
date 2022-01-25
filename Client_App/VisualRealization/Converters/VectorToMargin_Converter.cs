using Avalonia.Collections;
using Avalonia.Data.Converters;
using Models.Collections;
using Avalonia.Controls;
using Avalonia;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using Models.DataAccess;

namespace Client_App.Converters
{
    public class VectorToMargin_Converter : IValueConverter
    {
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            if (Value != null)
            {
                Vector rps = (Vector)Value;
                try
                {
                    
                    var lg = Thickness.Parse(((int)rps.X)+",0,0,0");
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
}
