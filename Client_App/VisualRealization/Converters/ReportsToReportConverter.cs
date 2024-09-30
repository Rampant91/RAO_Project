using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Models.Collections;
using Models.Interfaces;

namespace Client_App.VisualRealization.Converters;

public class ReportsToReportConverter : IValueConverter
{
    private Reports LastItem { get; set; }

    public object Convert(object value, Type tp, object param, CultureInfo info)
    {
        if (value == null) return null;
        var rpsColl = (IKeyCollection)value;
        try
        {
            if (rpsColl.Count != 0)
            {
                var rps = rpsColl.Get<Reports>(0);
                LastItem = rps;
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
        return null;
    }

    public object ConvertBack(object value, Type tp, object param, CultureInfo info) => LastItem ?? null;
}