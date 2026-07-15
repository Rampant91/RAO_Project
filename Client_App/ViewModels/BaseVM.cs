using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Client_App.Properties;

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

    internal static string ConfigDirectory = "";

    /// <summary>
    /// Возвращает системную директорию программы (корень, в котором создаётся папка RAO).
    /// </summary>
    internal static string GetSystemDirectoryPath() =>
        Settings.Default.SystemFolderDefaultPath is "default"
            ? OperatingSystem.IsWindows()
                ? Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))!
                : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            : Settings.Default.SystemFolderDefaultPath;

    /// <summary>
    /// Возвращает путь к папке RAO (использует <see cref="RaoDirectory"/>, если она уже инициализирована).
    /// </summary>
    internal static string GetRaoDirectoryPath() =>
        !string.IsNullOrEmpty(RaoDirectory)
            ? RaoDirectory
            : Path.Combine(GetSystemDirectoryPath(), "RAO");

    /// <summary>
    /// Возвращает файлы .RAODB из указанной папки без обхода вложенных каталогов.
    /// </summary>
    /// <param name="dirInfo">Папка для поиска.</param>
    internal static IEnumerable<FileInfo> GetRaodbFiles(DirectoryInfo dirInfo) =>
        dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
            .Where(x => x.Name.EndsWith(".raodb", StringComparison.OrdinalIgnoreCase));

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