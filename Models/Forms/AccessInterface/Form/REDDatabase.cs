using System;
using System.Collections.Generic;
using System.Text;

namespace Models.AccessInterface.Form
{
    public class REDDatabase : IDataAccess
    {
        public int FormID { get; set; }

        public REDDatabase(int FormID)
        {
            this.FormID = FormID;
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
