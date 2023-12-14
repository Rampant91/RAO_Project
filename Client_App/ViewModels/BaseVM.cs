using System.Diagnostics;

namespace Client_App.ViewModels;

public class BaseVM
{
    internal static string PasFolderPath = @"Y:\!!! Поручения\Паспорта ЗРИ 2022\Хранилище паспортов ЗРИ";

    internal const string Version = @"1.2.3.1";

    internal static string DbFileName = "Local_0";

    internal static string LogsDirectory = "";

    internal static string SystemDirectory = "";

    internal static string RaoDirectory = "";

    internal static string TmpDirectory = "";

    //  Запускает баш скрипт с введенной командой, не используется
    #region RunCommandInBush

    private protected static string? RunCommandInBush(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };
        process.Start();
        process.StandardInput.WriteLineAsync(command);
        return process.StandardOutput.ReadLine();
    }

    #endregion
}