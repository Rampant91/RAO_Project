namespace Models.DBRealization;

public static class StaticConfiguration
{
    private static string _dbPath { get; set; } = "";
    public static string DBPath
    {
        get => _dbPath;
        set => _dbPath = value;
    }
    public static DBModel DBModel;
}