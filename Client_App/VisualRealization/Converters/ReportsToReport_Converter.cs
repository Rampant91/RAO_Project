using Avalonia.Collections;
using Avalonia.Data.Converters;
using Models.Collections;
using System;
using System.Collections;
using System.ComponentModel;
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
                ObservableCollectionWithItemPropertyChanged<IKey>? lst = new ObservableCollectionWithItemPropertyChanged<IKey>();
                foreach (object? item in rps_coll)
                {
                    Reports? rps = (Reports)item;
                    last_item = rps;
                    if (rps != null)
                    {
                        foreach (Report? it in rps.Report_Collection)
                        {
                            lst.Add(it);
                        }
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
