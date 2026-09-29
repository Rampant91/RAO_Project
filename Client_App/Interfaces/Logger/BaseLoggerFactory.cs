using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private const string CrashLogFileName = "Crash.log";
    private const string ImportLogFileName = "Import.log";

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

    public event Action<(string msg, ErrorCodeLogger code)> NewLog = _ => { };

    public void AddLogger(ILogger logger)
    {
        lock (_lock)
        {
            if (logger is BaseFileLogger fileLogger)
            {
                // Один логгер на имя файла — повторный CreateFile не плодит дубликаты.
                var existing = Loggers.OfType<BaseFileLogger>()
                    .FirstOrDefault(l =>
                        string.Equals(l.LogFileName, fileLogger.LogFileName, StringComparison.OrdinalIgnoreCase));
                if (existing is not null)
                    return;
            }

            if (!Loggers.Contains(logger))
                Loggers.Add(logger);
        }
    }

    public void RemoveLogger(ILogger logger)
    {
        lock (_lock)
        {
            if (Loggers.Contains(logger))
                Loggers.Remove(logger);
        }
    }

    public void CreateFile(string path)
    {
        AddLogger(new BaseFileLogger(path));
    }

    public void Debug(string msg,
        ErrorCodeLogger code = ErrorCodeLogger.Application,
        [CallerMemberName] string origin = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0,
        bool isIncludeOriginDetails = true)
    {
        if (isIncludeOriginDetails)
            msg = FormatOrigin(msg, origin, filePath, lineNumber);
        ForEachLogger(log => log.Debug(msg, code));
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
            msg = FormatOrigin(msg, origin, filePath, lineNumber);

        var crashLogger = FindFileLogger(CrashLogFileName);
        if (crashLogger is null)
        {
            // Гарантия записи даже если CreateFile ещё не вызывали.
            crashLogger = new BaseFileLogger(CrashLogFileName);
            AddLogger(crashLogger);
        }

        try
        {
            crashLogger.Error(msg, code);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logger Error failed: {ex.Message}");
        }

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
            msg = FormatOrigin(msg, origin, filePath, lineNumber);
        ForEachLogger(log => log.Info(msg, code));
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
            msg = FormatOrigin(msg, origin, filePath, lineNumber);
        ForEachLogger(log => log.Warning(msg, code));
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

        var importLogger = FindFileLogger(ImportLogFileName);
        if (importLogger is null)
        {
            importLogger = new BaseFileLogger(ImportLogFileName);
            AddLogger(importLogger);
        }

        importLogger.Import($"{Environment.NewLine}{msg}", code);
        NewLog.Invoke((msg, code));
    }

    private static string FormatOrigin(string msg, string origin, string filePath, int lineNumber) =>
        $"[{Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)}" +
        $".{Path.GetFileNameWithoutExtension(filePath)}.{origin} - " +
        $"Line {lineNumber}] -" +
        $"Message: {msg}";

    private ILogger? FindFileLogger(string logFileName)
    {
        lock (_lock)
        {
            return Loggers.OfType<BaseFileLogger>()
                .FirstOrDefault(l =>
                    string.Equals(l.LogFileName, logFileName, StringComparison.OrdinalIgnoreCase));
        }
    }

    private void ForEachLogger(Action<ILogger> action)
    {
        ILogger[] snapshot;
        lock (_lock)
        {
            snapshot = Loggers.ToArray();
        }

        foreach (var log in snapshot)
            action(log);
    }
}
