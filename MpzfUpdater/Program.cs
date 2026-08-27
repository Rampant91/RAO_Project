using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace MpzfUpdater;

internal static class Program
{
    private const string UpdateFolderName = ".mpzf-update";
    private const string StateFileName = "state.json";
    private const string StagingFolderName = "staging";
    private const string PreviousFolderName = "previous";
    private const string PendingFileName = "pending.json";
    private const string RestartArgsFileName = "restart-args.json";

    /// <summary>
    /// Папки, которые не удаляем / не затираем из дистрибутива / не тащим в previous.
    /// </summary>
    private static readonly string[] SkipDirectoryNames =
    [
        UpdateFolderName,
        ".git",
        "Logs",
        "logs"
    ];

    private static int Main(string[] args)
    {
        try
        {
            var appDir = GetArg(args, "--app-dir") ?? throw new ArgumentException("Нужен --app-dir");
            var exeName = GetArg(args, "--exe-name") ?? "Client_App.exe";
            var pidText = GetArg(args, "--pid");
            if (int.TryParse(pidText, out var pid))
            {
                WaitForProcessExit(pid, TimeSpan.FromMinutes(2));
            }

            Thread.Sleep(500);

            var metaDir = Path.Combine(appDir, UpdateFolderName);
            var pendingPath = Path.Combine(metaDir, PendingFileName);
            if (!File.Exists(pendingPath))
            {
                Console.Error.WriteLine("pending.json не найден.");
                return 1;
            }

            var pending = JsonSerializer.Deserialize<PendingAction>(File.ReadAllText(pendingPath))
                          ?? throw new InvalidOperationException("Не удалось прочитать pending.json");

            var mode = pending.Mode?.Trim().ToLowerInvariant() ?? "update";
            if (mode == "rollback")
            {
                ApplyRollback(appDir, metaDir);
            }
            else
            {
                ApplyUpdate(appDir, metaDir, pending);
            }

            if (File.Exists(pendingPath))
            {
                File.Delete(pendingPath);
            }

            StartApp(appDir, exeName, metaDir);
            return 0;
        }
        catch (Exception ex)
        {
            try
            {
                var logDir = Path.Combine(Path.GetTempPath(), "MpzfUpdater");
                Directory.CreateDirectory(logDir);
                File.AppendAllText(
                    Path.Combine(logDir, "error.log"),
                    $"[{DateTime.Now:O}] {ex}{Environment.NewLine}");
            }
            catch
            {
                // ignore
            }

            Console.Error.WriteLine(ex);
            return 1;
        }
    }

    private static void ApplyUpdate(string appDir, string metaDir, PendingAction pending)
    {
        var staging = Path.Combine(metaDir, StagingFolderName);
        var previous = Path.Combine(metaDir, PreviousFolderName);
        if (!Directory.Exists(staging) || !Directory.EnumerateFileSystemEntries(staging).Any())
        {
            throw new InvalidOperationException("Папка staging пуста или отсутствует.");
        }

        if (Directory.Exists(previous))
        {
            Directory.Delete(previous, recursive: true);
        }

        Directory.CreateDirectory(previous);
        CopyAppFiles(appDir, previous);

        // Новая версия поверх, но локальный Client_App*.config и Logs не затираем
        CopyDirectory(staging, appDir, preserveLocalAppConfig: true);
        Directory.Delete(staging, recursive: true);
        DeleteLegacyRootUpdaterFiles(appDir);

        var state = LoadState(metaDir);
        // Пустой PreviousReleaseId = «до первого учёта версий»; в UI показываем DisplayName.
        state.PreviousReleaseId = state.InstalledReleaseId ?? string.Empty;
        state.PreviousMajorVersion = state.InstalledMajorVersion ?? string.Empty;
        if (string.IsNullOrWhiteSpace(state.InstalledReleaseId)
            || string.Equals(state.InstalledReleaseId.Trim(), "pre-update", StringComparison.OrdinalIgnoreCase))
        {
            state.PreviousReleaseId = string.Empty;
            state.PreviousMajorVersion = string.Empty;
            state.PreviousDisplayName = string.IsNullOrWhiteSpace(state.InstalledDisplayName)
                ? "локальная установка до обновления"
                : state.InstalledDisplayName;
        }
        else
        {
            state.PreviousDisplayName = string.IsNullOrWhiteSpace(state.InstalledDisplayName)
                ? state.InstalledReleaseId
                : state.InstalledDisplayName;
        }

        state.InstalledReleaseId = pending.ReleaseId ?? string.Empty;
        state.InstalledMajorVersion = pending.MajorVersion ?? string.Empty;
        state.InstalledDisplayName = pending.DisplayName ?? pending.ReleaseId ?? string.Empty;
        SaveState(metaDir, state);
    }

    private static void ApplyRollback(string appDir, string metaDir)
    {
        var previous = Path.Combine(metaDir, PreviousFolderName);
        if (!Directory.Exists(previous) || !Directory.EnumerateFileSystemEntries(previous).Any())
        {
            throw new InvalidOperationException("Нет previous для отката.");
        }

        var swap = Path.Combine(metaDir, "rollback-swap");
        if (Directory.Exists(swap))
        {
            Directory.Delete(swap, recursive: true);
        }

        Directory.CreateDirectory(swap);
        CopyAppFiles(appDir, swap);

        DeleteAppFiles(appDir);
        CopyDirectory(previous, appDir, preserveLocalAppConfig: false);
        DeleteLegacyRootUpdaterFiles(appDir);

        Directory.Delete(previous, recursive: true);
        Directory.CreateDirectory(previous);
        CopyDirectory(swap, previous, preserveLocalAppConfig: false);
        Directory.Delete(swap, recursive: true);

        var state = LoadState(metaDir);
        (state.InstalledReleaseId, state.PreviousReleaseId) =
            (state.PreviousReleaseId, state.InstalledReleaseId);
        (state.InstalledMajorVersion, state.PreviousMajorVersion) =
            (state.PreviousMajorVersion, state.InstalledMajorVersion);
        (state.InstalledDisplayName, state.PreviousDisplayName) =
            (state.PreviousDisplayName, state.InstalledDisplayName);
        SaveState(metaDir, state);
    }

    private static void DeleteLegacyRootUpdaterFiles(string appDir)
    {
        foreach (var file in Directory.GetFiles(appDir, "MpzfUpdater*"))
        {
            try
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            catch
            {
                // ignore
            }
        }
    }

    private static bool ShouldSkipDirectory(string name) =>
        SkipDirectoryNames.Any(s => string.Equals(s, name, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Локальный config рядом с exe (ручные правки пользователя).
    /// </summary>
    private static bool IsLocalAppConfigFile(string fileName) =>
        string.Equals(fileName, "Client_App.dll.config", StringComparison.OrdinalIgnoreCase)
        || string.Equals(fileName, "Client_App.exe.config", StringComparison.OrdinalIgnoreCase);

    private static void CopyAppFiles(string appDir, string destDir)
    {
        Directory.CreateDirectory(destDir);
        foreach (var file in Directory.GetFiles(appDir))
        {
            var name = Path.GetFileName(file);
            if (name.StartsWith("MpzfUpdater", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            File.Copy(file, Path.Combine(destDir, name), overwrite: true);
        }

        foreach (var dir in Directory.GetDirectories(appDir))
        {
            var name = Path.GetFileName(dir);
            if (ShouldSkipDirectory(name))
            {
                continue;
            }

            CopyDirectory(dir, Path.Combine(destDir, name), preserveLocalAppConfig: false);
        }
    }

    private static void DeleteAppFiles(string appDir)
    {
        foreach (var file in Directory.GetFiles(appDir))
        {
            var name = Path.GetFileName(file);
            // Config восстановится из previous; пока удаляем, чтобы откат был полным по файлам приложения.
            // Logs не трогаем (их нет в корне как файлов обычно).
            if (name.StartsWith("MpzfUpdater", StringComparison.OrdinalIgnoreCase))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
                continue;
            }

            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (var dir in Directory.GetDirectories(appDir))
        {
            var name = Path.GetFileName(dir);
            if (ShouldSkipDirectory(name))
            {
                continue;
            }

            Directory.Delete(dir, recursive: true);
        }
    }

    private static void CopyDirectory(string sourceDir, string destDir, bool preserveLocalAppConfig)
    {
        Directory.CreateDirectory(destDir);
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var name = Path.GetFileName(file);
            var dest = Path.Combine(destDir, name);
            if (preserveLocalAppConfig && IsLocalAppConfigFile(name) && File.Exists(dest))
            {
                continue;
            }

            File.Copy(file, dest, overwrite: true);
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            var name = Path.GetFileName(dir);
            if (ShouldSkipDirectory(name))
            {
                continue;
            }

            CopyDirectory(dir, Path.Combine(destDir, name), preserveLocalAppConfig);
        }
    }

    private static LocalState LoadState(string metaDir)
    {
        var path = Path.Combine(metaDir, StateFileName);
        if (!File.Exists(path))
        {
            return new LocalState();
        }

        return JsonSerializer.Deserialize<LocalState>(File.ReadAllText(path)) ?? new LocalState();
    }

    private static void SaveState(string metaDir, LocalState state)
    {
        Directory.CreateDirectory(metaDir);
        var path = Path.Combine(metaDir, StateFileName);
        File.WriteAllText(path, JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static void StartApp(string appDir, string exeName, string metaDir)
    {
        var exePath = Path.Combine(appDir, exeName);
        if (!File.Exists(exePath))
        {
            exePath = Directory.GetFiles(appDir, "*.exe")
                .FirstOrDefault(f => !Path.GetFileName(f).StartsWith("MpzfUpdater", StringComparison.OrdinalIgnoreCase))
                ?? exePath;
        }

        var restartArgsPath = Path.Combine(metaDir, RestartArgsFileName);
        var arguments = string.Empty;
        if (File.Exists(restartArgsPath))
        {
            var list = JsonSerializer.Deserialize<string[]>(File.ReadAllText(restartArgsPath)) ?? [];
            arguments = string.Join(' ', list.Select(QuoteArg));
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = arguments,
            WorkingDirectory = appDir,
            UseShellExecute = true
        });
    }

    private static string QuoteArg(string arg) =>
        arg.Contains(' ') || arg.Contains('"')
            ? $"\"{arg.Replace("\"", "\\\"")}\""
            : arg;

    private static void WaitForProcessExit(int pid, TimeSpan timeout)
    {
        try
        {
            using var process = Process.GetProcessById(pid);
            if (!process.WaitForExit((int)timeout.TotalMilliseconds))
            {
                throw new TimeoutException($"Процесс {pid} не завершился вовремя.");
            }
        }
        catch (ArgumentException)
        {
            // Уже завершён
        }
    }

    private static string? GetArg(string[] args, string name)
    {
        for (var i = 0; i < args.Length; i++)
        {
            if (!string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (i + 1 < args.Length)
            {
                return args[i + 1];
            }
        }

        return null;
    }

    private sealed class PendingAction
    {
        public string? Mode { get; set; }
        public string? ReleaseId { get; set; }
        public string? MajorVersion { get; set; }
        public string? DisplayName { get; set; }
    }

    private sealed class LocalState
    {
        public string InstalledReleaseId { get; set; } = string.Empty;
        public string InstalledMajorVersion { get; set; } = string.Empty;
        public string InstalledDisplayName { get; set; } = string.Empty;
        public string PreviousReleaseId { get; set; } = string.Empty;
        public string PreviousMajorVersion { get; set; } = string.Empty;
        public string PreviousDisplayName { get; set; } = string.Empty;
    }
}
