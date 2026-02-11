using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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


    #region IsFileLocked

    public static bool IsFileLocked(string fullPath)
    {
        fullPath ??= DBPath;
        try
        {
            using var stream = new FileInfo(fullPath).Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
        }
        catch (FileNotFoundException)
        {
            return false;
        }
        catch (IOException)
        {
            return true;
        }
        return false;
    }

    #endregion
}