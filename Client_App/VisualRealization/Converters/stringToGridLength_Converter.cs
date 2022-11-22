using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Client_App.VisualRealization.Converters;

public class stringToGridLength_Converter : IValueConverter
{
    public object Convert(object Value, Type tp, object Param, CultureInfo info)
    {
        if (Value != null)
        {
            var rps = (string)Value;
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
        var rps = (GridLength)Value;
        return $"{rps.Value}*";
    }
}