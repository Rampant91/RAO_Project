using System.Collections.Generic;

namespace Collections.Notes_Collection
{
    public interface IDataAccess
    {
        string DBPath { get; set; }
        string PathToData { get; set; }

        int ReportsID { get; }
        int ReportID { get; }
        int NoteID { get; }

        List<object[]> Get(string ParamName);
        void Set(string ParamName, object obj);
    }
}
