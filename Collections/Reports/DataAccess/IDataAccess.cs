using System;
using System.Collections.Generic;
using System.Text;

namespace Collections.Reports_Collection
{
    public interface IDataAccess
    {
        string DBPath { get; set; }

        int ReportsID { get; }

        List<object[]> Get(string ParamName);
        void Set(string ParamName, object obj);
    }
}
