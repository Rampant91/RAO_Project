using System;
using System.Collections.Generic;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.IO;

namespace Models
{
    public class SQLDatabaseUsage
    {
        public string ConnectionString { get; set; }
        public SQLDatabaseUsage(string _path)
        {
            int pageSize = 4096;
            bool forcedWrites = true;
            bool overwrite = true;
            var pth = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
            var connectionString = new FbConnectionStringBuilder
            {
                Database = _path,
                ServerType = FbServerType.Embedded,
                UserID = "SYSDBA",
                Password = "masterkey",
                Role="ADMIN",
                ClientLibrary = pth + "REDDB\\fbclient.dll"
            }.ToString();
            ConnectionString = connectionString;

            var direct=Path.GetDirectoryName(_path);
            if (!Directory.Exists(direct))
            {
                Directory.CreateDirectory(direct);
            }
            if (!System.IO.File.Exists(_path))
            {
                FbConnection.CreateDatabase(connectionString, pageSize, forcedWrites, overwrite);
                Init();
            }
        }

        void Init()
        {
            using (FbConnection connection = new FbConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new FbCommand("create table reports ( reportid integer, formid int, formnum varchar(255) )", connection, transaction))
                    {
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
