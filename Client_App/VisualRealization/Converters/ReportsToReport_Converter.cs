using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Collections;
using System.Collections;
using System.Collections.Generic;

namespace Client_App.Converters
{
    public class ReportsToReport_Converter : IValueConverter
    {
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            var rps_coll = (IEnumerable)Value;
            List<Report> lst = new List<Report>();
            foreach(var item in rps_coll)
            {
                var rps = (Models.DataAccess.IDataAccess<Reports>)item;
                foreach(var it in rps.Value.Report_Collection)
                {
                    lst.Add(it);
                }
            }
            return lst;
        }
        public object ConvertBack(object Value, Type tp, object Param, CultureInfo info)
        {
            return null;
        }
    }
}
