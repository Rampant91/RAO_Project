using DBRealization;
using System.Collections.Generic;

namespace Collections.Reports_Collection
{
    public class RedDataBase : IDataAccess
    {
        public string DBPath { get; set; }

        int _ReportsID = -1;
        public int ReportsID
        {
            get
            {
                return _ReportsID;
            }
            set
            {
                if (_ReportsID != value)
                {
                    _ReportsID = value;
                }
            }
        }

        public RedDataBase(string DBPath, int ReportsID)
        {
            this.DBPath = DBPath;
            this.ReportsID = ReportsID;
            RedDataBaseUse use = new RedDataBaseUse(DBPath);
        }
        public RedDataBase(string DBPath)
        {
            //Write get ID what we need by type
            this.DBPath = DBPath;
            RedDataBaseUse use = new RedDataBaseUse(DBPath);
        }

        public List<object[]> Get(string ParamName)
        {
            if (ReportsID != -1)
            {
                return Get_ByReportsID(ParamName);
            }
            else
            {
                return null;
            }
        }
        List<object[]> Get_ByReportsID(string ParamName)
        {
            RedDataBaseUse use = new RedDataBaseUse(DBPath);

            return (List<object[]>)use.DoCommand("select " + ParamName + " from reports where reports_id=" + ReportsID, (reader) =>
                   {
                       List<object[]> lst = new List<object[]>();
                       while (reader.Read())
                       {
                           var values = new object[reader.FieldCount];
                           reader.GetValues(values);
                           lst.Add(values);
                       }
                       return lst;
                   });
        }

        public void Set(string ParamName, object obj)
        {
            if (ReportsID != -1)
            {
                Set_ByReportsID(ParamName, obj);
            }
        }
        void Set_ByReportsID(string ParamName, object obj)
        {

        }
    }
}
