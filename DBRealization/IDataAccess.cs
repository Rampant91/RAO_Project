using System;
using System.Collections.Generic;
using System.Text;

namespace DBRealization
{
    public interface IDataAccess
    {
        string DBPath { get; set; }
        string PathToData { get; set; }
        int ReportsID { get; }
        int ReportID { get; }
        int RowID { get; }

        object Get(string ParamName);
        void Set(string ParamName, object obj);
    }
}
