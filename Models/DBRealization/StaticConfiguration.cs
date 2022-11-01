namespace Models.DBRealization
{
    public class StaticConfiguration
    {
        public static string _dbPath { get; set; } = "";
        public static string DBPath
        {
            get
            {
                return _dbPath;
            }
            set
            {
                _dbPath = value;
            }
        }
        public static DBModel DBModel;
    }
}
