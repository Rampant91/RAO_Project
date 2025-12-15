using System;
using System.IO;
using System.Threading.Tasks;
using Client_App.ViewModels;

namespace Client_App.Interfaces.Logger;

public interface IFileManager
{
    public Task WriteToFile(string msg, string path, bool append = true);
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

    public async Task WriteToFile(string msg, string path, bool append = true)
    {
        path = NormalizePath(path);
        path = Path.Combine(BaseVM.LogsDirectory, path);
        await Awaiter.Async(path, async() => 
        {
            await Task.Run(() =>
            {
                using TextWriter fileStream = new StreamWriter(File.Open(path, append?FileMode.Append:FileMode.Create));
                fileStream.Write(msg);
            });
        });
    }

    public async Task WriteToConsole(string msg)
    {
        await Task.Run(() =>
        {
            Console.WriteLine(msg);
        });
    }
}