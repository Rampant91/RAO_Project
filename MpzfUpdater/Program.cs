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
    private const string LastErrorFileName = "last-error.txt";
    private const string UpdaterLogFileName = "updater.log";
    private const string KeyAssemblyName = "Client_App.dll";
    private const string KeyExeName = "Client_App.exe";

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
        string? appDir = null;
        string? metaDir = null;
        try
        {
            appDir = GetArg(args, "--app-dir") ?? throw new ArgumentException("Нужен --app-dir");
            var exeName = GetArg(args, "--exe-name") ?? "Client_App.exe";
            var pidText = GetArg(args, "--pid");
            if (int.TryParse(pidText, out var pid))
            {
                WaitForProcessExit(pid, TimeSpan.FromMinutes(2));
            }

            Thread.Sleep(500);

            metaDir = Path.Combine(appDir, UpdateFolderName);
            Directory.CreateDirectory(metaDir);
            Log(metaDir, "Старт MpzfUpdater");

            var pendingPath = Path.Combine(metaDir, PendingFileName);
            if (!File.Exists(pendingPath))
            {
                throw new InvalidOperationException("pending.json не найден.");
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

            ClearLastError(metaDir);
            Log(metaDir, mode == "rollback" ? "Откат завершён успешно" : "Обновление завершено успешно");
            StartApp(appDir, exeName, metaDir);
            return 0;
        }
        catch (Exception ex)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(metaDir))
                {
                    WriteLastError(metaDir, ex);
                    Log(metaDir, "Ошибка: " + ex);
                }
            }
            catch
            {
                // ignore
            }

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

        EnsureKeyBinaryInDirectory(staging, "staging");

        if (Directory.Exists(previous))
        {
            Directory.Delete(previous, recursive: true);
        }

        Directory.CreateDirectory(previous);
        CopyAppFiles(appDir, previous);
        Log(metaDir, "Резервная копия previous создана");

        var appMutated = false;
        try
        {
            // Чистая замена (как rollback), локальный Client_App*.config сохраняем.
            DeleteAppFiles(appDir, preserveLocalAppConfig: true);
            appMutated = true;
            CopyDirectory(staging, appDir, preserveLocalAppConfig: true);
            EnsureKeyBinaryMatchesStaging(appDir, staging);
            Directory.Delete(staging, recursive: true);
            DeleteLegacyRootUpdaterFiles(appDir);
        }
        catch (Exception ex)
        {
            Log(metaDir, "Сбой применения, откат из previous: " + ex.Message);
            if (appMutated)
            {
                try
                {
                    RestoreFromPrevious(appDir, previous);
                    Log(metaDir, "Восстановление из previous выполнено");
                }
                catch (Exception restoreEx)
                {
                    throw new InvalidOperationException(
                        "Не удалось применить обновление и восстановить предыдущую версию. " +
                        ex.Message + " | Restore: " + restoreEx.Message,
                        ex);
                }
            }

            throw;
        }

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

        DeleteAppFiles(appDir, preserveLocalAppConfig: false);
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

    private static void RestoreFromPrevious(string appDir, string previous)
    {
        if (!Directory.Exists(previous) || !Directory.EnumerateFileSystemEntries(previous).Any())
        {
            throw new InvalidOperationException("Папка previous пуста, восстановить нельзя.");
        }

        DeleteAppFiles(appDir, preserveLocalAppConfig: false);
        CopyDirectory(previous, appDir, preserveLocalAppConfig: false);
        DeleteLegacyRootUpdaterFiles(appDir);
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

            CopyFileOverwrite(file, Path.Combine(destDir, name));
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

    private static void DeleteAppFiles(string appDir, bool preserveLocalAppConfig)
    {
        foreach (var file in Directory.GetFiles(appDir))
        {
            var name = Path.GetFileName(file);
            if (preserveLocalAppConfig && IsLocalAppConfigFile(name))
            {
                continue;
            }

            // Config восстановится из previous при откате; Logs не трогаем.
            if (name.StartsWith("MpzfUpdater", StringComparison.OrdinalIgnoreCase))
            {
                TryDeleteFile(file);
                continue;
            }

            TryDeleteFile(file);
        }

        foreach (var dir in Directory.GetDirectories(appDir))
        {
            var name = Path.GetFileName(dir);
            if (ShouldSkipDirectory(name))
            {
                continue;
            }

            TryDeleteDirectory(dir);
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

            CopyFileOverwrite(file, dest);
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

    private static void CopyFileOverwrite(string source, string dest)
    {
        if (File.Exists(dest))
        {
            File.SetAttributes(dest, FileAttributes.Normal);
        }

        File.Copy(source, dest, overwrite: true);
        File.SetAttributes(dest, FileAttributes.Normal);
    }

    private static void TryDeleteFile(string path)
    {
        File.SetAttributes(path, FileAttributes.Normal);
        File.Delete(path);
    }

    private static void TryDeleteDirectory(string path)
    {
        foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
        {
            try
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }
            catch
            {
                // ignore — Directory.Delete всё равно покажет ошибку при блокировке
            }
        }

        Directory.Delete(path, recursive: true);
    }

    private static void EnsureKeyBinaryInDirectory(string directory, string label)
    {
        var dll = Path.Combine(directory, KeyAssemblyName);
        var exe = Path.Combine(directory, KeyExeName);
        if (!File.Exists(dll) && !File.Exists(exe))
        {
            throw new InvalidOperationException(
                $"В {label} нет {KeyAssemblyName} / {KeyExeName}.");
        }
    }

    private static void EnsureKeyBinaryMatchesStaging(string appDir, string staging)
    {
        var stagingKey = Path.Combine(staging, KeyAssemblyName);
        var appKey = Path.Combine(appDir, KeyAssemblyName);
        if (!File.Exists(stagingKey))
        {
            stagingKey = Path.Combine(staging, KeyExeName);
            appKey = Path.Combine(appDir, KeyExeName);
        }

        if (!File.Exists(stagingKey))
        {
            throw new InvalidOperationException("В staging нет ключевого бинарника для проверки.");
        }

        if (!File.Exists(appKey))
        {
            throw new InvalidOperationException($"После копирования отсутствует {Path.GetFileName(appKey)}.");
        }

        var stagingInfo = new FileInfo(stagingKey);
        var appInfo = new FileInfo(appKey);
        if (stagingInfo.Length != appInfo.Length)
        {
            throw new InvalidOperationException(
                $"Размер {Path.GetFileName(appKey)} после копирования не совпал со staging " +
                $"({appInfo.Length} ≠ {stagingInfo.Length}).");
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

    private static void WriteLastError(string metaDir, Exception ex)
    {
        Directory.CreateDirectory(metaDir);
        var path = Path.Combine(metaDir, LastErrorFileName);
        File.WriteAllText(
            path,
            $"[{DateTime.Now:O}]{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex}");
    }

    private static void ClearLastError(string metaDir)
    {
        var path = Path.Combine(metaDir, LastErrorFileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static void Log(string metaDir, string message)
    {
        try
        {
            Directory.CreateDirectory(metaDir);
            File.AppendAllText(
                Path.Combine(metaDir, UpdaterLogFileName),
                $"[{DateTime.Now:O}] {message}{Environment.NewLine}");
        }
        catch
        {
            // ignore
        }
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
