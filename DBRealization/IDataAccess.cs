using System;
using System.Collections.Generic;
using System.Text;

namespace DBRealization
{
    public interface IDataAccess
    {
        string DBPath { get; set; }
        string PathToData { get; set; }
        int ReportID { get; }
        int FormID { get; }
        int RowID { get; }

        object Get(string ParamName);
        void Set(string ParamName, object obj);
    }
}
