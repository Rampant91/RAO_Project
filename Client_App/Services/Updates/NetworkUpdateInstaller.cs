using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.DTO;
using Newtonsoft.Json;

namespace Client_App.Services.Updates;

/// <summary>
/// Подготовка staging и запуск MpzfUpdater для применения/отката.
/// </summary>
public class NetworkUpdateInstaller
{
    private static readonly string[] CopySkipDirectoryNames =
    [
        NetworkUpdatePaths.UpdateFolderName,
        ".git"
    ];

    private readonly LocalUpdateStateStore _stateStore = new();

    public async Task PrepareAndApplyUpdateAsync(
        NetworkReleaseInfo release,
        string networkRoot,
        CancellationToken cancellationToken = default)
    {
        if (NetworkUpdatePaths.IsRunningFromNetworkDistribution(networkRoot))
        {
            throw new InvalidOperationException(
                "Нельзя обновлять программу, запущенную с сетевого диска. " +
                "Скопируйте папку win-x64 локально.");
        }

        var sourceDir = NetworkUpdatePaths.GetReleaseDirectory(networkRoot, release);
        if (!Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"Папка релиза не найдена: {sourceDir}");
        }

        // Нельзя копировать «в себя» (источник и установка — одна и та же папка)
        var appDir = Path.GetFullPath(NetworkUpdatePaths.AppDirectory)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var sourceFull = Path.GetFullPath(sourceDir)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        if (string.Equals(appDir, sourceFull, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Текущая установка совпадает с папкой релиза на шаре. Обновление не требуется / невозможно.");
        }

        Directory.CreateDirectory(NetworkUpdatePaths.UpdateMetaDirectory);
        var staging = NetworkUpdatePaths.StagingDirectory;
        if (Directory.Exists(staging))
        {
            Directory.Delete(staging, recursive: true);
        }

        Directory.CreateDirectory(staging);
        await Task.Run(() => CopyDirectory(sourceDir, staging, cancellationToken), cancellationToken)
            .ConfigureAwait(false);

        var pending = new PendingUpdateAction
        {
            Mode = "update",
            ReleaseId = release.ReleaseId,
            MajorVersion = release.MajorVersion,
            DisplayName = NetworkUpdateLabels.FormatVersion(release.MajorVersion, release.ReleaseId)
        };
        WritePending(pending);
        WriteRestartArgs();
        LaunchUpdaterAndExit();
    }

    public void PrepareAndApplyRollback()
    {
        if (NetworkUpdatePaths.IsRunningFromNetworkDistribution())
        {
            throw new InvalidOperationException(
                "Нельзя выполнять откат при запуске с сетевого диска.");
        }

        if (!_stateStore.HasPreviousBackup())
        {
            throw new InvalidOperationException("Нет сохранённой предыдущей версии для отката.");
        }

        Directory.CreateDirectory(NetworkUpdatePaths.UpdateMetaDirectory);
        WritePending(new PendingUpdateAction { Mode = "rollback" });
        WriteRestartArgs();
        LaunchUpdaterAndExit();
    }

    private static void WritePending(PendingUpdateAction pending)
    {
        File.WriteAllText(
            NetworkUpdatePaths.PendingFilePath,
            JsonConvert.SerializeObject(pending, Formatting.Indented));
    }

    private static void WriteRestartArgs()
    {
        var args = Properties.Settings.Default.AppStartupParameters
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var path = Path.Combine(NetworkUpdatePaths.UpdateMetaDirectory, "restart-args.json");
        File.WriteAllText(path, JsonConvert.SerializeObject(args, Formatting.Indented));
    }

    private static void LaunchUpdaterAndExit()
    {
        var updaterInApp = NetworkUpdatePaths.UpdaterExePath;
        if (!File.Exists(updaterInApp))
        {
            throw new FileNotFoundException(
                $"Не найден {NetworkUpdatePaths.UpdaterExeName} в папке data\\Updater. " +
                "Выполните Publish Client_App или скопируйте updater в data\\Updater.",
                updaterInApp);
        }

        var tempDir = Path.Combine(Path.GetTempPath(), "MpzfUpdater_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        var updaterDir = NetworkUpdatePaths.UpdaterDirectory;
        foreach (var file in Directory.GetFiles(updaterDir, "MpzfUpdater*"))
        {
            File.Copy(file, Path.Combine(tempDir, Path.GetFileName(file)), overwrite: true);
        }

        var updaterTemp = Path.Combine(tempDir, NetworkUpdatePaths.UpdaterExeName);
        var startInfo = new ProcessStartInfo
        {
            FileName = updaterTemp,
            Arguments =
                $"--app-dir \"{NetworkUpdatePaths.AppDirectory}\" " +
                $"--pid {Environment.ProcessId} " +
                $"--exe-name \"{NetworkUpdatePaths.AppExeName}\"",
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = tempDir
        };

        if (Process.Start(startInfo) == null)
        {
            throw new InvalidOperationException("Не удалось запустить MpzfUpdater.");
        }

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
            else
            {
                Environment.Exit(0);
            }
        });
    }

    public static void CopyDirectory(string sourceDir, string destDir, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(destDir);
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var fileName = Path.GetFileName(file);
            if (fileName.StartsWith("MpzfUpdater", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            File.Copy(file, Path.Combine(destDir, fileName), overwrite: true);
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var name = Path.GetFileName(dir);
            if (CopySkipDirectoryNames.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            CopyDirectory(dir, Path.Combine(destDir, name), cancellationToken);
        }
    }
}

/// <summary>
/// Задание для MpzfUpdater (pending.json).
/// </summary>
public class PendingUpdateAction
{
    public string Mode { get; set; } = "update";
    public string ReleaseId { get; set; } = string.Empty;
    public string MajorVersion { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
