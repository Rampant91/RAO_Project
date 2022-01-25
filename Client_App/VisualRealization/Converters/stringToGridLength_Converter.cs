using Avalonia.Collections;
using Avalonia.Data.Converters;
using Models.Collections;
using Avalonia.Controls;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using Models.DataAccess;

namespace Client_App.Converters
{
    public class stringToGridLength_Converter : IValueConverter
    {
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            if (Value != null)
            {
                string rps = (string)Value;
                try
                {
                    var lg = GridLength.Parse(rps.Replace(",","."));
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
            GridLength rps = (GridLength)Value;
            return rps.Value + "*";
        }
    }
}
