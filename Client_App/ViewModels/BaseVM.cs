using System.Diagnostics;

namespace Client_App.ViewModels;

/// <summary>
/// Базовый класс ViewModel
/// </summary>
public class BaseVM
{
    internal static string DbFileName = "Local_0";

    internal static string LogsDirectory = "";

    internal static string SystemDirectory = "";

    internal static string RaoDirectory = "";

    internal static string ReserveDirectory = "";

    internal static string TmpDirectory = "";

    #region RunCommandInBush

    /// <summary>
    /// Запускает баш скрипт с введенной командой (не используется).
    /// </summary>
    /// <param name="command">Команда.</param>
    /// <returns></returns>
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