using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Client_App.Services;

public static class FirebirdLogger
{
    private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
    private static readonly Lock _lock = new();
    private static StreamWriter _logWriter;
    private static bool _isInitialized = false;

    public static void Initialize()
    {
        try
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            var logFile = Path.Combine(LogDirectory, $"firebird_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            _logWriter = new StreamWriter(logFile, true, Encoding.UTF8)
            {
                AutoFlush = true
            };
                
            _isInitialized = true;
            Log("Firebird logger initialized");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to initialize Firebird logger: {ex.Message}");
        }
    }

    public static void Log(string message, string details = null)
    {
        if (!_isInitialized) return;

        try
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
                
            lock (_lock)
            {
                _logWriter.WriteLine(logMessage);
                if (!string.IsNullOrEmpty(details))
                {
                    _logWriter.WriteLine(details);
                    _logWriter.WriteLine();
                }
            }

            Debug.WriteLine(logMessage);
            if (!string.IsNullOrEmpty(details))
            {
                Debug.WriteLine(details);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error writing to Firebird log: {ex.Message}");
        }
    }

    public static void LogError(string message, Exception ex = null)
    {
        var errorMessage = $"ERROR: {message}";
        if (ex != null)
        {
            errorMessage += $"\nException: {ex.Message}\nStack: {ex.StackTrace}";
        }
        Log(errorMessage);
    }

    public static void Close()
    {
        try
        {
            if (_logWriter != null)
            {
                _logWriter.Flush();
                _logWriter.Close();
                _logWriter = null;
            }
            _isInitialized = false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error closing Firebird logger: {ex.Message}");
        }
    }
}