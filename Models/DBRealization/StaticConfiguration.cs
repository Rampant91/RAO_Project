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

    /// <summary>
    /// Короткое ожидание перед операцией с файлом БД (копирование и т.п.).
    /// Не блокирует бесконечно: при активном Firebird-соединении файл часто «занят».
    /// </summary>
    public static async Task WaitForDatabaseFileAvailableAsync(
        CancellationToken cancellationToken = default,
        int maxWaitMs = 2000)
    {
        var deadline = Environment.TickCount64 + maxWaitMs;
        while (IsFileLocked(null))
        {
            if (Environment.TickCount64 >= deadline || cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await Task.Delay(50, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>Синхронный вариант <see cref="WaitForDatabaseFileAvailableAsync"/>.</summary>
    public static void WaitForDatabaseFileAvailable(int maxWaitMs = 2000)
    {
        var deadline = Environment.TickCount64 + maxWaitMs;
        while (IsFileLocked(null))
        {
            if (Environment.TickCount64 >= deadline)
            {
                return;
            }

            Thread.Sleep(50);
        }
    }

    #endregion
}