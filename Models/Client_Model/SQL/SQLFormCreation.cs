using System;
using System.Collections.Generic;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.IO;

namespace Models
{
    public class SQLFormCreation
    {
        public static int[] GetReportIDS(string param)
        {
            SQLDatabaseUsage us = new SQLDatabaseUsage(@"C:\Databases\Local.raodb");
            List<int> obj = new List<int>();

            using (FbConnection connection = new FbConnection(us.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new FbCommand("select formid from reports where formnum=" + param, connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var values = new object[1];
                                reader.GetValues(values);
                                obj.Add(Convert.ToInt32(values[0]));

                            }
                        }
                    }
                }
            }
            return obj.ToArray();
        }
    }
}
