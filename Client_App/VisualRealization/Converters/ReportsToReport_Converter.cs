using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Models.Collections;
using Models.Interfaces;

namespace Client_App.VisualRealization.Converters;

public class ReportsToReport_Converter : IValueConverter
{
    private Reports last_item { get; set; }

    public object Convert(object value, Type tp, object param, CultureInfo info)
    {
        if (value != null)
        {
            var rps_coll = (IKeyCollection)value;
            try
            {
                if (rps_coll.Count != 0)
                {
                    var rps = (Reports)rps_coll.Get<Reports>(0);
                    last_item = rps;
                    if (rps != null)
                    {
                        return rps.Report_Collection;
                    }
                }
                else
                {
                    return new ObservableCollectionWithItemPropertyChanged<IKey>();
                }
            }
            catch
            {

            }
        }
        return null;
    }

    public object ConvertBack(object value, Type tp, object param, CultureInfo info)
    {
        if (last_item != null)
        {
            return last_item;
        }
        return null;
    }
}