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
        public FbConnection connection = new FbConnection(@"Server=localhost;User=SYSDBA;Password=masterkey;Database=C:\Databases\Rao_Local.raodb");
        public SQLite(string propName,string formName,int ID)
        {
            PropName = propName;
            FormName = formName;
            this.ID = ID;
            if(!System.IO.File.Exists(@"C:\Databases\Rao_Local.raodb"))
            {
                System.IO.File.Create(@"C:\Databases\Rao_Local.raodb");
            }
        }
        public object Get()
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
        public void Set(object arg)
        {
            connection.Open();
            var obj = Get();
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
