using System;
using System.Collections.Generic;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using System.Linq;

namespace Models.AccessInterface.REDDatabase_Usage
{
    public class REDDatabase_Methods
    {
        public static int GetFreeRowID(string type)
        {
            int RowID = 0;

            REDDatabase_Object us = new REDDatabase_Object(@"C:\Databases\Local.raodb");
            List<int> obj = new List<int>();

            using (FbConnection connection = new FbConnection(us.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new FbCommand("select row_id from data_rows", connection, transaction))
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
            if (obj.Count() > 0)
            {
                RowID = obj.Max();
            }
            return RowID;
        }

        public static int[] GetFormsIDS(int masterkey)
        {
            REDDatabase_Object us = new REDDatabase_Object(@"C:\Databases\Local.raodb");
            List<int> obj = new List<int>();

            using (FbConnection connection = new FbConnection(us.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new FbCommand("select report_id from reports where masterform_id=" + masterkey, connection, transaction))
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
