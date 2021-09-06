using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DBRealization
{
    public class StaticConfiguration
    {
        public static string DBPath
        {
            get
            {
                var tmp= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) ,"RAO");
                tmp = Path.Combine(tmp, "Local.raodb");
                return tmp;
            }
        }
        public static DBModel DBModel = new DBModel(DBPath);
    }
}
