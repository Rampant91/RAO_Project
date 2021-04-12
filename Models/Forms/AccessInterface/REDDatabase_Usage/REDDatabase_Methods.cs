using System;
using System.Collections.Generic;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.IO;

namespace Models.AccessInterface.REDDatabase_Usage
{
    public class REDDatabase_Methods
    {
        public static int[] GetFormsIDS(string param,int masterkey)
        {
            REDDatabase_Object us = new REDDatabase_Object(@"C:\Databases\Local.raodb");
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
        public static int GetFormTypeByID(int FormID)
        {
            int i = 0;

            return i;
        }
    }
}
