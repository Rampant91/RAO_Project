using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Collections;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Collections;

namespace Client_App.Converters
{
    public class ReportsToReport_Converter : IValueConverter
    {
        Reports last_item { get; set; }
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            if (Value != null)
            {
                var rps_coll = (IEnumerable)Value;
                var lst = new AvaloniaList<object>();
                foreach (var item in rps_coll)
                {
                    var rps = (Reports)item;
                    last_item = rps;
                    foreach (var it in rps.Report_Collection)
                    {
                        lst.Add(it);
                    }
                }
                return lst;
            }
            return null;
        }
        public object ConvertBack(object Value, Type tp, object Param, CultureInfo info)
        {
            if(last_item!=null)
            {
                return last_item;
            }
            return null;
        }
    }
}
