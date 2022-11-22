using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Models.Collections;
using Models.Interfaces;

namespace Client_App.VisualRealization.Converters;

public class ReportsToReport_Converter : IValueConverter
{
    private Reports last_item { get; set; }
    public object Convert(object Value, Type tp, object Param, CultureInfo info)
    {
        if (Value != null)
        {
            var rps_coll = (IKeyCollection)Value;
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
    public object ConvertBack(object Value, Type tp, object Param, CultureInfo info)
    {
        if (last_item != null)
        {
            return last_item;
        }
        return null;
    }
}