using System;
using System.Collections.Generic;
using System.Text;

namespace DBRealization
{
    public class StaticConfiguration
    {
        public static string DBPath
        {
            get
            {
                var tmp= Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\RAO\\Local.raodb";
                return tmp;
            }
        }
        public static DBModel DBModel = new DBModel(DBPath);
    }
}
