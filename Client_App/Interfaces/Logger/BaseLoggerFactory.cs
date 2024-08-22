using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Client_App.Interfaces.Logger.EnumLogger;
using Models.DTO;

namespace Client_App.Interfaces.Logger;

public interface ILogFactory
{
    public void CreateFile(string path);
    event Action<(string msg, ErrorCodeLogger code)> NewLog;
    public bool IncludeOriginalDetails { get; set; }
    public void AddLogger(ILogger innerLogger);
    public void RemoveLogger(ILogger innerLogger);
    public void Import(LoggerImportDTO dto, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true);
    public void Info(string msg, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true);
    public void Debug(string msg, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0,
        bool isIncludeOriginDetails = true);
    public void Warning(string msg, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true);
    public void Error(string msg, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true);
}

public class BaseLoggerFactory : ILogFactory
{
    private static readonly List<ILogger> Loggers = [];
    private readonly object _lock = new();
    public bool IncludeOriginalDetails { get; set; }
    public BaseLoggerFactory(ILogger[]? loggers = null)
    {
        if (loggers == null) return;
        foreach (var log in loggers)
        {
            AddLogger(log);
        }
    }

    public event Action<(string msg, ErrorCodeLogger code)> NewLog = (details) => { };

    public void AddLogger(ILogger logger)
    {
        lock (_lock)
        {
            if (!Loggers.Contains(logger))
            {
                Loggers.Add(logger);
            }
        }
    }
    public void RemoveLogger(ILogger logger)
    {
        lock (_lock)
        {
            if (Loggers.Contains(logger))
            {
                Loggers.Remove(logger);
            }
        }
    }

    public void CreateFile(string path)
    {
        _ = new BaseLoggerFactory([new BaseFileLogger(path)]);
    }

    public void Debug(string msg, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true)
        {
            if (isIncludeOriginDetails)
                msg = $"[{Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)}" +
                      $".{Path.GetFileNameWithoutExtension(filePath)}.{origin} - " +
                    $"Line {lineNumber}] -" +
                    $"Message: {msg}";
            Loggers.ForEach(log => log.Debug(msg, code));
            NewLog.Invoke((msg, code));
        }

    public void Error(string msg, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true)
    {
        if (isIncludeOriginDetails)
            msg = $"[{Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)}" +
                  $".{Path.GetFileNameWithoutExtension(filePath)}.{origin} - " +
                $"Line {lineNumber}] -" +
                $"Message: {msg}";
        Loggers[1].Error(msg, code);
        //Loggers.ForEach(log => log.Error(msg, code));
        NewLog.Invoke((msg, code));
    }

    public void Info(string msg, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true)
    {
        if (isIncludeOriginDetails)
            msg = $"[{Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)}" +
                  $".{Path.GetFileNameWithoutExtension(filePath)}.{origin} - " +
                $"Line {lineNumber}] -" +
                $"Message: {msg}";
        Loggers.ForEach(log => log.Info(msg, code));
        NewLog.Invoke((msg, code));
    }

    public void Warning(string msg, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true)
    {
        if (isIncludeOriginDetails)
            msg = $"[{Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)}" +
                  $".{Path.GetFileNameWithoutExtension(filePath)}.{origin} - " +
                $"Line {lineNumber}] -" +
                $"Message: {msg}";
        Loggers.ForEach(log => log.Warning(msg, code));
        NewLog.Invoke((msg, code));
    }

    public void Import(LoggerImportDTO dto, 
        ErrorCodeLogger code = ErrorCodeLogger.Application, 
        [CallerMemberName] string origin = "", 
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0, 
        bool isIncludeOriginDetails = true)
    {
        var msg = dto.OperationDate +
                  $"\t{dto.CurrentLogLine}" +
                  $"\t{dto.RegNum}" +
                  $"\t{dto.Okpo}" +
                  $"\t{dto.FormNum}" +
                  $"\t{dto.CorNum}" +
                  $"\t{dto.FormCount} зап." +
                  $"\t{dto.Act}" +
                  $"\t{dto.PeriodOrYear}" +
                  $"\t{dto.ShortName}" +
                  $"\t{dto.SourceFileFullPath}";
        Loggers[0].Import($"{Environment.NewLine}{msg}", code);
        //Loggers.ForEach(log => log.Import(msg, code));
        NewLog.Invoke((msg, code));
    }
}