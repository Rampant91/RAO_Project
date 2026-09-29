using System;
using System.IO;
using System.Threading.Tasks;
using Client_App.ViewModels;

namespace Client_App.Interfaces.Logger;

public interface IFileManager
{
    public Task WriteToFile(string msg, string path, bool append = true);
    /// <summary>
    /// Синхронная запись — для Error/Exit: async WriteToFile не успевает до Environment.Exit.
    /// </summary>
    void WriteToFileSync(string msg, string path, bool append = true);
    public Task WriteToConsole(string msg);
    public string NormalizePath(string path);
    public string ResolvePath(string path);
}

public class BaseFileManager : IFileManager
{
    public string NormalizePath(string path)
    {
        return OperatingSystem.IsWindows()
            ? path.Replace("/", "\\").Trim()
            : path.Replace("\\", "/").Trim();
    }

    public string ResolvePath(string path)
    {
        return Path.GetFullPath(path);
    }

    private string ResolveLogFilePath(string path)
    {
        path = NormalizePath(path);
        var logsDir = BaseVM.LogsDirectory;
        if (string.IsNullOrWhiteSpace(logsDir))
        {
            // Fallback до ProcessRaoDirectory — хотя бы не теряем запись в CWD без имени.
            logsDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "RAO",
                "logs");
        }

        Directory.CreateDirectory(logsDir);
        return Path.Combine(logsDir, path);
    }

    public async Task WriteToFile(string msg, string path, bool append = true)
    {
        path = ResolveLogFilePath(path);
        await Awaiter.Async(path, async () =>
        {
            await Task.Run(() => WriteToFileCore(msg, path, append));
        });
    }

    public void WriteToFileSync(string msg, string path, bool append = true)
    {
        path = ResolveLogFilePath(path);
        // Тот же ключ, что у async — не перемешиваем запись в один файл.
        Awaiter.Async(path, () =>
        {
            WriteToFileCore(msg, path, append);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }

    private static void WriteToFileCore(string msg, string path, bool append)
    {
        using var writer = new StreamWriter(File.Open(path, append ? FileMode.Append : FileMode.Create));
        writer.Write(msg);
        writer.Flush();
    }

    public async Task WriteToConsole(string msg)
    {
        await Task.Run(() =>
        {
            Console.WriteLine(msg);
        });
    }
}
