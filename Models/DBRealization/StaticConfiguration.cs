using System.IO;

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

    public static bool IsFileLocked()
    {
        try
        {
            using var stream = new FileInfo(_dbPath).Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
        }
        catch (IOException)
        {
            return true;
        }
        return false;
    }
}