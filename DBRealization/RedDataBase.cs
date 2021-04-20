using System;

namespace DBRealization
{
    public class RedDataBase:IDataAccess
    {
        public string DBPath { get; set; }
        public int ReportID { get; set; }
        public RedDataBase(string DBPath, int ReportID)
        {
            this.DBPath = DBPath;
            this.ReportID = ReportID;
        }
        public RedDataBase(string DBPath)
        {
            this.DBPath = DBPath;
        }

        public object Get(string ParamName)
        {
            return null;
        }

        public void Set(string ParamName, object obj)
        {

        }
    }
}
