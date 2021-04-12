using System;
using System.Collections.Generic;
using System.Text;

namespace Models.AccessInterface.Report
{
    public class REDDatabase : IDataAccess
    {
        public int ReportID{get;set;}

        public REDDatabase(int ReportID)
        {
            this.ReportID = ReportID;
        }

        public object Get (string ParamName)
        {
            object obj = null;

            return obj;
        }
        public void Set(string ParamName, object value)
        {

        }
    }
}
