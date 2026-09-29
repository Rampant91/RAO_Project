using System;
using System.IO;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.ViewModels;
using Xunit;

namespace Test.Logger;

/// <summary>
/// Error + Exit: запись Crash.log должна быть синхронной и не зависеть от Loggers[1].
/// </summary>
public class CrashLogWriteTests
{
    [Fact]
    public void Error_WritesCrashLogSynchronously_EvenWithoutPriorCreateFile()
    {
        var logsDir = Path.Combine(Path.GetTempPath(), "rao-crash-log-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(logsDir);
        var previous = BaseVM.LogsDirectory;
        BaseVM.LogsDirectory = logsDir;

        try
        {
            var factory = new BaseLoggerFactory();
            factory.Error("db create failed", ErrorCodeLogger.DataBase, isIncludeOriginDetails: false);

            var crashPath = Path.Combine(logsDir, "Crash.log");
            Assert.True(File.Exists(crashPath), "Crash.log must exist after Error");
            var text = File.ReadAllText(crashPath);
            Assert.Contains("db create failed", text);
            Assert.Contains("[ERROR]", text);
        }
        finally
        {
            BaseVM.LogsDirectory = previous;
            try
            {
                Directory.Delete(logsDir, recursive: true);
            }
            catch
            {
                // ignore cleanup
            }
        }
    }

    [Fact]
    public void WriteToFileSync_CreatesFileUnderLogsDirectory()
    {
        var logsDir = Path.Combine(Path.GetTempPath(), "rao-crash-log-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(logsDir);
        var previous = BaseVM.LogsDirectory;
        BaseVM.LogsDirectory = logsDir;

        try
        {
            var fm = new BaseFileManager();
            fm.WriteToFileSync("hello-sync", "Crash.log");

            var crashPath = Path.Combine(logsDir, "Crash.log");
            Assert.True(File.Exists(crashPath));
            Assert.Contains("hello-sync", File.ReadAllText(crashPath));
        }
        finally
        {
            BaseVM.LogsDirectory = previous;
            try
            {
                Directory.Delete(logsDir, recursive: true);
            }
            catch
            {
                // ignore cleanup
            }
        }
    }
}
