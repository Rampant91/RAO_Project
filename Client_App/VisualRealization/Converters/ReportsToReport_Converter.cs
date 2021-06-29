using Avalonia.Collections;
using Avalonia.Data.Converters;
using Collections;
using System;
using System.Collections;
using System.Globalization;

namespace Client_App.Converters
{
    public class ReportsToReport_Converter : IValueConverter
    {
        private Reports last_item { get; set; }
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            if (Value != null)
            {
                IEnumerable? rps_coll = (IEnumerable)Value;
                AvaloniaList<IChanged>? lst = new AvaloniaList<IChanged>();
                foreach (object? item in rps_coll)
                {
                    Reports? rps = (Reports)item;
                    last_item = rps;
                    foreach (Report? it in rps.Report_Collection)
                    {
                        it.IsChanged = true;
                        lst.Add(it);
                    }
                }
                return lst;
            }
            return null;
        }
        public object ConvertBack(object Value, Type tp, object Param, CultureInfo info)
        {
            if (last_item != null)
            {
                return last_item;
            }
            return null;
        }
    }
}
