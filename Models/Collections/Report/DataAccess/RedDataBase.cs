using System;
using System.Collections.Generic;
using DBRealization;

namespace Collections.Report_Collection
{
    public class RedDataBase:IDataAccess
    {
        public string DBPath { get; set; }

        //Иcпользуется как ReportsID|_ / ReportID|_
        public string PathToData { get; set; }
        public int ReportsID 
        { 
            get 
            {
                try
                {
                    return Convert.ToInt32(PathToData.Split('/')[0]);
                }
                catch
                {
                    return -1;
                }
            } 
        }
        public int ReportID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(PathToData.Split('/')[1]);
                }
                catch
                {
                    return -1;
                }
            }
        }

        public RedDataBase(string DBPath, string PathToData)
        {
            this.DBPath = DBPath;
            this.PathToData = PathToData;
            RedDataBaseUse use = new RedDataBaseUse(DBPath);
        }
        public RedDataBase(string DBPath, int Type)
        {
            // 1=Row, 2=Report, 3=Reports
            //Write get ID what we need by type
            this.DBPath = DBPath;
            this.PathToData = "1";
            RedDataBaseUse use = new RedDataBaseUse(DBPath);
        }

        public List<object[]> Get(string ParamName)
        {
            if(ReportsID!=-1)
            {
                return Get_ByReportsID(ParamName);
            }
            else
            {
                if (ReportID != -1)
                {
                    return Get_ByReportID(ParamName);
                }
                else
                {
                    return null;
                }
            }
        }
        List<object[]> Get_ByReportsID(string ParamName)
        {
            RedDataBaseUse use = new RedDataBaseUse(DBPath);

            return (List<object[]>)use.DoCommand("select "+ParamName+" from reports where reports_id="+ReportsID,(reader)=> 
            {
                List<object[]> lst = new List<object[]>();
                while(reader.Read())
                {
                    var values = new object[reader.FieldCount];
                    reader.GetValues(values);
                    lst.Add(values);
                }
                return lst;
            });
        }
        List<object[]> Get_ByReportID(string ParamName)
        {
            RedDataBaseUse use = new RedDataBaseUse(DBPath);

            return (List<object[]>)use.DoCommand("select " + ParamName + " from report where report_id=" + ReportID, (reader) =>
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
            if (ReportID != -1)
            {
                Set_ByReportsID(ParamName,obj);
            }
            else
            {
                if (ReportID != -1)
                {
                    Set_ByReportID(ParamName, obj);
                }
            }
        }
        void Set_ByReportsID(string ParamName,object obj)
        {

        }
        void Set_ByReportID(string ParamName, object obj)
        {

        }
    }
}
