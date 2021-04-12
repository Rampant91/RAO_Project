using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using System.Collections.Concurrent;

namespace Models.AccessInterface.Row
{
    public interface IDataAccess
    {
        public int RowID { get; set; }
        ObservableConcurrentDictionary<string, Func<IControl>> VisualRealizations { get; set; }
        public Func<IControl> GetVisualRealization(string ParamName)
        {
            return VisualRealizations[ParamName];
        }
        public object Get(string ParamName);
        public void Set(string ParamName, object value);
    }
}
