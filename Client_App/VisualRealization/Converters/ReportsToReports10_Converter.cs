using Avalonia.Collections;
using Avalonia.Data.Converters;
using Models.Collections;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using Models.DataAccess;
using System.Linq;
namespace Client_App.Converters
{
    public class ReportsToReports10_Converter : IValueConverter
    {
        public object Convert(object Value, Type tp, object Param, CultureInfo info)
        {
            if (Value != null)
            {
                IKeyCollection rps_coll = (IKeyCollection)Value;
                try
                {
                    if (rps_coll.Count != 0)
                    {
                        return new ObservableCollectionWithItemPropertyChanged<IKey>(rps_coll.GetEnumerable().Where(x => ((Reports)x).Master_DB.FormNum_DB == "1.0"));
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
            return Value;
        }
    }
}
