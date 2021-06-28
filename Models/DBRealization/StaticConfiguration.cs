using System;
using System.Collections.Generic;
using System.Text;

namespace DBRealization
{
    public class StaticConfiguration
    {
        public static string DBPath = @"C:\Databases\local.raodb";
        public static DBModel DBModel = new DBModel(DBPath);
    }
}
