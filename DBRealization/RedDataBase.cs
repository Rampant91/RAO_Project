using System;

namespace DBRealization
{
    public class SQLconsts
    {
        public SQLconsts()
        {

        }
        public const string strNotNullDeclaration = " varchar(255) not null, ";
        public const string intNotNullDeclaration = " int not null, ";
        public const string shortNotNullDeclaration = " smallint not null, ";
        public const string dateNotNullDeclaration = " datetimeoffset not null, ";
        public const string doubleNotNullDeclaration = " float(53) not null, ";
    }
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
