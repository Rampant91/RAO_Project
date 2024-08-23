using System;
using Client_App.Interfaces.Logger.EnumLogger;

namespace Client_App.Interfaces.Logger;

public class BaseFileLogger(string filePath) : ILogger
{
    private string FilePath { get; set; } = filePath;

    public void Debug(string msg, ErrorCodeLogger code)
    {
        var currentTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
        ServiceExtension.FileManager.WriteToConsole($"{Environment.NewLine}[{currentTime}][DEBUG][{(int)code}]{msg}{Environment.NewLine}");
    }

    public void Error(string msg, ErrorCodeLogger code)
    {
        var currentTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
        ServiceExtension.FileManager.WriteToFile($"{Environment.NewLine}[{currentTime}][ERROR][{(int)code}]{msg}{Environment.NewLine}", FilePath);
    }
    
    public void Info(string msg, ErrorCodeLogger code)
    {
        var currentTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
        ServiceExtension.FileManager.WriteToFile($"{Environment.NewLine}[{currentTime}][INFO][{(int)code}]{msg}{Environment.NewLine}", FilePath);
    }

    public void Warning(string msg, ErrorCodeLogger code)
    {
        var currentTime = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
        ServiceExtension.FileManager.WriteToFile($"{Environment.NewLine}[{currentTime}][WARNING][{(int)code}]{msg}{Environment.NewLine}", FilePath);
    }

    public void Import(string msg, ErrorCodeLogger code)
    {
        ServiceExtension.FileManager.WriteToFile($"{msg}{Environment.NewLine}", FilePath);
    }
}