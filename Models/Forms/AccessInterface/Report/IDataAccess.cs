using System;
using System.Collections.Generic;
using System.Text;

namespace Models.AccessInterface.Report
{
    public interface IDataAccess
    {
        int ReportID { get; set; }
        object Get(string ParamName);
        void Set(string ParamName, object value);
    }
}
