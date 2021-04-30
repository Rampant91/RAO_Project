using System.Collections.Generic;

namespace Collections.Rows_Collection
{
    public interface IDataAccess
    {
        string DBPath { get; set; }
        string PathToData { get; set; }

        int ReportsID { get; }
        int ReportID { get; }
        int RowID { get; }
        int RowType { get; }

        List<object[]> Get(string ParamName);
        void Set(string ParamName, object obj);
    }
}
