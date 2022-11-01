namespace Models.DBRealization
{
    public class DBModel : DataContext
    {
        public DBModel(string Path = "") : base(Path)
        {

        }
    }
}
