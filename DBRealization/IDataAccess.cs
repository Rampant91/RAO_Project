using System;
using System.Collections.Generic;
using System.Text;

namespace DBRealization
{
    public interface IDataAccess
    {
        string DBPath { get; set; }
        public int ReportID { get; set; }
        object Get(string ParamName);
        void Set(string ParamName, object obj);
    }
}
