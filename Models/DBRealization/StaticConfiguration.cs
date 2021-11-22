using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Models.DBRealization
{
    public class StaticConfiguration
    {
        public static string _dbPath { get; set; } = "";
        public static string DBPath
        {
            get
            {
                if(_dbPath=="")
                {
                    string system = Environment.GetFolderPath(Environment.SpecialFolder.System);
                    string path = Path.GetPathRoot(system);
                    var tmp = Path.Combine(path, "RAO");
                    var tp = Directory.GetFiles(tmp,"*.RAODB");
                    if (tp.Length > 0)
                    {
                        _dbPath = tp[0];
                    }
                    else
                    {
                        tmp = Path.Combine(tmp, "Local.raodb");
                        _dbPath = tmp;
                    }
                }
                return _dbPath;
            }
            set
            {
                _dbPath = value;
            }
        }
        public static DBModel DBModel = new DBModel(DBPath);
    }
}
