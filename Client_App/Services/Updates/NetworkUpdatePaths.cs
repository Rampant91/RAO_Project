using System;
using System.IO;
using System.Linq;
using Client_App.Properties;
using Models.DTO;

namespace Client_App.Services.Updates;

/// <summary>
/// Пути для сетевых обновлений отдела и локального кэша.
/// </summary>
public static class NetworkUpdatePaths
{
    public const string UpdateFolderName = ".mpzf-update";
    public const string StateFileName = "state.json";
    public const string PrefsFileName = "prefs.json";
    public const string StagingFolderName = "staging";
    public const string PreviousFolderName = "previous";
    public const string PendingFileName = "pending.json";
    public const string LatestFileName = "latest.json";
    public const string RootOverrideFileName = "mpzf-update-root.txt";
    /// <summary>
    /// Корень выкладок: подпапки {majorVersion}/{buildId}/win-x64/.
    /// </summary>
    public const string DefaultNetworkRoot = @"Y:\АЧ 2021\Программа\Исходные";
    public const string UpdaterExeName = "MpzfUpdater.exe";
    public const string AppExeName = "Client_App.exe";
    /// <summary>
    /// Подпапка внутри data для MpzfUpdater (не в корне программы).
    /// </summary>
    public const string UpdaterFolderName = "Updater";
    public const string DataFolderName = "data";
    /// <summary>
    /// Папка Windows-сборки на шаре (фиксированно для отдела).
    /// </summary>
    public const string WinPublishFolderName = "win-x64";

    /// <summary>
    /// Каталог, из которого запущено приложение.
    /// </summary>
    public static string AppDirectory =>
        AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    public static string UpdateMetaDirectory => Path.Combine(AppDirectory, UpdateFolderName);

    public static string StateFilePath => Path.Combine(UpdateMetaDirectory, StateFileName);

    public static string PrefsFilePath => Path.Combine(UpdateMetaDirectory, PrefsFileName);

    public static string StagingDirectory => Path.Combine(UpdateMetaDirectory, StagingFolderName);

    public static string PreviousDirectory => Path.Combine(UpdateMetaDirectory, PreviousFolderName);

    public static string PendingFilePath => Path.Combine(UpdateMetaDirectory, PendingFileName);

    public static string UpdaterDirectory =>
        Path.Combine(AppDirectory, DataFolderName, UpdaterFolderName);

    public static string UpdaterExePath =>
        Path.Combine(UpdaterDirectory, UpdaterExeName);

    /// <summary>
    /// Корень сетевых (или локальных тестовых) обновлений.
    /// Приоритет: mpzf-update-root.txt рядом с exe → настройка → значение по умолчанию.
    /// </summary>
    public static string ResolveNetworkRoot()
    {
        var overrideFile = Path.Combine(AppDirectory, RootOverrideFileName);
        if (File.Exists(overrideFile))
        {
            var line = File.ReadAllLines(overrideFile)
                .Select(x => x.Trim())
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x) && !x.StartsWith('#'));
            if (!string.IsNullOrWhiteSpace(line))
            {
                return Environment.ExpandEnvironmentVariables(line);
            }
        }

        var fromSettings = Settings.Default.NetworkUpdateRootPath?.Trim();
        if (!string.IsNullOrWhiteSpace(fromSettings))
        {
            return Environment.ExpandEnvironmentVariables(fromSettings);
        }

        return DefaultNetworkRoot;
    }

    public static string GetLatestJsonPath(string networkRoot) =>
        Path.Combine(networkRoot, LatestFileName);

    /// <summary>
    /// Папка с Client_App.exe на шаре для указанного релиза.
    /// </summary>
    public static string GetReleaseDirectory(string networkRoot, NetworkReleaseInfo release)
    {
        var relative = release.RelativePath?.Replace('/', Path.DirectorySeparatorChar).Trim() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(relative))
        {
            return Path.GetFullPath(Path.Combine(networkRoot, relative));
        }

        if (!string.IsNullOrWhiteSpace(release.MajorVersion) && !string.IsNullOrWhiteSpace(release.ReleaseId))
        {
            relative = Path.Combine(
                release.MajorVersion.Trim(),
                release.ReleaseId.Trim(),
                WinPublishFolderName);
            return Path.GetFullPath(Path.Combine(networkRoot, relative));
        }

        relative = Path.Combine("releases", release.ReleaseId);
        return Path.GetFullPath(Path.Combine(networkRoot, relative));
    }

    /// <summary>
    /// Доступен ли корень сетевых обновлений (диск подключён, папка существует).
    /// </summary>
    public static bool IsNetworkRootAccessible(out string networkRoot, out string? failureReason)
    {
        networkRoot = ResolveNetworkRoot();
        try
        {
            if (string.IsNullOrWhiteSpace(networkRoot))
            {
                failureReason = "Путь к сетевому каталогу обновлений не задан.";
                return false;
            }

            var pathRoot = Path.GetPathRoot(networkRoot);
            if (!string.IsNullOrEmpty(pathRoot) && pathRoot.StartsWith(@"\\", StringComparison.Ordinal))
            {
                if (!Directory.Exists(pathRoot))
                {
                    failureReason = $"Сетевой ресурс недоступен: {networkRoot}";
                    return false;
                }
            }
            else if (!string.IsNullOrEmpty(pathRoot) && pathRoot.Length >= 2 && pathRoot[1] == ':')
            {
                var drive = DriveInfo.GetDrives()
                    .FirstOrDefault(d => string.Equals(d.Name, pathRoot, StringComparison.OrdinalIgnoreCase));
                if (drive is not { IsReady: true })
                {
                    failureReason =
                        $"Диск {pathRoot.TrimEnd(Path.DirectorySeparatorChar)} недоступен. " +
                        "Проверка и установка обновлений из сетевой папки невозможны.";
                    return false;
                }
            }

            if (!Directory.Exists(networkRoot))
            {
                failureReason = $"Каталог обновлений не найден: {networkRoot}";
                return false;
            }

            failureReason = null;
            return true;
        }
        catch (Exception ex)
        {
            failureReason = ex.Message;
            return false;
        }
    }

    public static bool IsNetworkRootAccessible() =>
        IsNetworkRootAccessible(out _, out _);

    /// <summary>
    /// Если exe лежит в {root}/{majorVersion}/{releaseId}/win-x64 — вернуть major и releaseId.
    /// </summary>
    public static bool TryParseReleaseFromAppDirectory(
        string? networkRoot,
        out string majorVersion,
        out string releaseId)
    {
        majorVersion = string.Empty;
        releaseId = string.Empty;
        try
        {
            networkRoot ??= ResolveNetworkRoot();
            if (!IsRunningFromNetworkDistribution(networkRoot))
            {
                return false;
            }

            var appDir = Path.GetFullPath(AppDirectory)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (!string.Equals(Path.GetFileName(appDir), WinPublishFolderName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var releaseDir = Path.GetDirectoryName(appDir);
            var majorDir = string.IsNullOrEmpty(releaseDir) ? null : Path.GetDirectoryName(releaseDir);
            if (string.IsNullOrEmpty(releaseDir) || string.IsNullOrEmpty(majorDir))
            {
                return false;
            }

            releaseId = Path.GetFileName(releaseDir) ?? string.Empty;
            majorVersion = Path.GetFileName(majorDir) ?? string.Empty;
            return !string.IsNullOrWhiteSpace(majorVersion) && !string.IsNullOrWhiteSpace(releaseId);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Программа запущена из каталога дистрибутивов на шаре (под Исходные\…).
    /// В этом случае нельзя применять автообновление поверх сетевой папки.
    /// </summary>
    public static bool IsRunningFromNetworkDistribution(string? networkRoot = null)
    {
        try
        {
            networkRoot ??= ResolveNetworkRoot();
            if (string.IsNullOrWhiteSpace(networkRoot) || !Directory.Exists(networkRoot))
            {
                return false;
            }

            var appDir = Path.GetFullPath(AppDirectory)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var root = Path.GetFullPath(networkRoot)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            return appDir.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(appDir, root, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}
