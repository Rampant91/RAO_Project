using System;
using System.Collections.Generic;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace Models.Client_Model
{
    public interface IDataLoadEngine
    {
        object Get(int ID);
        void Set(int ID,object arg);
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
        public object Get(int ID)
        {
            return value;
        }
        public void Set(int ID,object arg)
        {
            value = arg;
        }
    }

    public class SQLite:IDataLoadEngine
    {
        public string PropName { get; set; }
        public string FormName { get; set; }
        public FbConnection connection = new FbConnection(@"Server=localhost;User=SYSDBA;Password=masterkey;Database=C:\Databases\Rao_Local.FDB");
        public SQLite(string propName,string formName)
        {
            PropName = propName;
            FormName = formName;
        }
        public object Get(int ID)
        {
            object obj = null;
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new FbCommand("select "+PropName+" from "+FormName+" where ID="+ID, connection, transaction))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var values = new object[1];
                            reader.GetValues(values);
                            obj = values[0];
                            break;
                        }
                    }
                }
            }
            connection.Close();
            return obj;
        }
        public void Set(int ID,object arg)
        {
            connection.Open();
            var obj = Get(ID);
            if (obj != null)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new FbCommand("update " + FormName + " set " + PropName + "=@value where ID=@id", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@value",arg);
                        command.Parameters.AddWithValue("@id", ID);
                        var rdr = command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new FbCommand("insert into " + FormName + "(ID," + PropName + ") where ID=" + ID, connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var values = new object[1];
                                reader.GetValues(values);
                                break;
                            }
                        }
                    }
                }
            }
            connection.Close();
        }
    }
}
