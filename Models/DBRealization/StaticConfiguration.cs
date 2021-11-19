using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Models.DBRealization
{
    public class StaticConfiguration
    {
        public static string DBPath
        {
            get
            {
                string system = Environment.GetFolderPath(Environment.SpecialFolder.System);
                string path = Path.GetPathRoot(system);
                var tmp= Path.Combine(path ,"RAO");
                tmp = Path.Combine(tmp, "Local.raodb");
                return tmp;
            }
        }
        public static DBModel DBModel = new DBModel(DBPath);
    }
}
