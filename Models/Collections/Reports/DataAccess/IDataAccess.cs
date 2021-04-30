using System.Collections.Generic;

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
