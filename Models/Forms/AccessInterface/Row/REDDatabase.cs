using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using Avalonia.Controls;

namespace Models.AccessInterface.Row
{
    public class REDDatabase : IDataAccess
    {
        public int RowID { get; set; }
        public ObservableConcurrentDictionary<string, Func<IControl>> VisualRealizations { get; set; } = new ObservableConcurrentDictionary<string, Func<IControl>>();

        public REDDatabase(int RowID)
        {
            this.RowID = RowID;
        }

        public object Get (string ParamName)
        {
            object obj = null;

            return obj;
        }
        public void Set(string ParamName, object value)
        {

        }
    }
}
