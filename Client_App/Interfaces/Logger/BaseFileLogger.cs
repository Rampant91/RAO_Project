using System;
using Client_App.Interfaces.Logger.EnumLogger;

namespace Client_App.Interfaces.Logger;

public class BaseFileLogger : ILogger
{
    public string FilePath { get; set; }
    public BaseFileLogger(string filePath)
    {
        FilePath = filePath;
    }

    public void Debug(string msg, ErrorCodeLogger code)
    {
        var currentTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
        ServiceExtension.FileManager.WriteToConsole($"[{currentTime}][DEBUG][{(int)code}]{msg}{Environment.NewLine}");
    }

    public void Error(string msg, ErrorCodeLogger code)
    {
        var currentTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
        ServiceExtension.FileManager.WriteToFile($"[{currentTime}][ERROR][{(int)code}]{msg}{Environment.NewLine}", FilePath, true);
    }

    public void Info(string msg, ErrorCodeLogger code)
    {
        var currentTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
        ServiceExtension.FileManager.WriteToFile($"[{currentTime}][INFO][{(int)code}]{msg}{Environment.NewLine}", FilePath, true);
    }

    public void Warning(string msg, ErrorCodeLogger code)
    {
        var currentTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
        ServiceExtension.FileManager.WriteToFile($"[{currentTime}][WARNING][{(int)code}]{msg}{Environment.NewLine}", FilePath, true);
    }

    public void Import(string msg, ErrorCodeLogger code)
    {
        ServiceExtension.FileManager.WriteToFile($"{msg}{Environment.NewLine}", FilePath, true);
    }
}