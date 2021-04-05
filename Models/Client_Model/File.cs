using System;
using System.Collections.Generic;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.IO;

namespace Models.Client_Model
{
    public interface IDataLoadEngine
    {
        object Get();
        void Set(object arg);
    }
    public class File:IDataLoadEngine
    {
        public File()
        {

        }
        public File(string value)
        {
            this.value = value;
        }

        public object value = null;
        public object Get()
        {
            return value;
        }
        public void Set(object arg)
        {
            value = arg;
        }
    }

    public class SQLite:IDataLoadEngine
    {
        public string PropName { get; set; }
        public string FormName { get; set; }
        public int ID { get; set; }
        public SQLite(string propName,string formName,int ID)
        {
            PropName = propName;
            FormName = formName;
            this.ID = ID;
        }
        public object Get()
        {
            object obj = null;
            
            return obj;
        }
        public void Set(object arg)
        {
            
        }
    }
}
