using System;
using System.IO;
using System.Linq;
using Models.DTO;
using Newtonsoft.Json;

namespace Client_App.Services.Updates;

/// <summary>
/// Хранение локального state.json и проверка наличия previous.
/// </summary>
public class LocalUpdateStateStore
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        Formatting = Formatting.Indented
    };

    public LocalUpdateState Load()
    {
        try
        {
            if (!File.Exists(NetworkUpdatePaths.StateFilePath))
            {
                return new LocalUpdateState();
            }

            var json = File.ReadAllText(NetworkUpdatePaths.StateFilePath);
            var state = JsonConvert.DeserializeObject<LocalUpdateState>(json) ?? new LocalUpdateState();
            if (NormalizeLegacyPlaceholders(state))
            {
                Save(state);
            }

            return state;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load update state: {ex.Message}");
            return new LocalUpdateState();
        }
    }

    public void Save(LocalUpdateState state)
    {
        Directory.CreateDirectory(NetworkUpdatePaths.UpdateMetaDirectory);
        var json = JsonConvert.SerializeObject(state, JsonSettings);
        File.WriteAllText(NetworkUpdatePaths.StateFilePath, json);
    }

    /// <summary>
    /// Если state ещё не заполнен, а локальные файлы совпадают с релизом на шаре —
    /// фиксируем версию без диалога обновления (первый запуск после введения учёта).
    /// </summary>
    public LocalUpdateState LoadAndBootstrapIfMatchesRelease(NetworkReleaseInfo release, string networkRoot)
    {
        var state = Load();
        if (!string.IsNullOrWhiteSpace(state.InstalledReleaseId)
            && !NetworkUpdateLabels.IsLegacyPlaceholder(state.InstalledReleaseId))
        {
            return state;
        }

        if (!LooksIdenticalToRelease(release, networkRoot))
        {
            return state;
        }

        state.InstalledReleaseId = release.ReleaseId?.Trim() ?? string.Empty;
        state.InstalledMajorVersion = release.MajorVersion?.Trim() ?? string.Empty;
        state.InstalledDisplayName = NetworkUpdateLabels.FormatVersion(
            state.InstalledMajorVersion,
            state.InstalledReleaseId);
        if (NetworkUpdateLabels.IsLegacyPlaceholder(state.PreviousReleaseId))
        {
            state.PreviousReleaseId = string.Empty;
            state.PreviousMajorVersion = string.Empty;
            if (string.IsNullOrWhiteSpace(state.PreviousDisplayName)
                || NetworkUpdateLabels.IsLegacyPlaceholder(state.PreviousDisplayName))
            {
                state.PreviousDisplayName = NetworkUpdateLabels.PreUpdateInstallLabel;
            }
        }

        Save(state);
        return state;
    }

    public bool HasPreviousBackup()
    {
        try
        {
            return Directory.Exists(NetworkUpdatePaths.PreviousDirectory)
                   && Directory.EnumerateFileSystemEntries(NetworkUpdatePaths.PreviousDirectory).Any();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Совпадение основного бинарника локальной копии с релизом на шаре (размер + время записи).
    /// </summary>
    public static bool LooksIdenticalToRelease(NetworkReleaseInfo release, string networkRoot)
    {
        try
        {
            var releaseDir = NetworkUpdatePaths.GetReleaseDirectory(networkRoot, release);
            var localDll = Path.Combine(NetworkUpdatePaths.AppDirectory, "Client_App.dll");
            var remoteDll = Path.Combine(releaseDir, "Client_App.dll");
            if (!File.Exists(localDll) || !File.Exists(remoteDll))
            {
                localDll = Path.Combine(NetworkUpdatePaths.AppDirectory, NetworkUpdatePaths.AppExeName);
                remoteDll = Path.Combine(releaseDir, NetworkUpdatePaths.AppExeName);
            }

            if (!File.Exists(localDll) || !File.Exists(remoteDll))
            {
                return false;
            }

            var local = new FileInfo(localDll);
            var remote = new FileInfo(remoteDll);
            return local.Length == remote.Length
                   && local.LastWriteTimeUtc == remote.LastWriteTimeUtc;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LooksIdenticalToRelease failed: {ex.Message}");
            return false;
        }
    }

    private static bool NormalizeLegacyPlaceholders(LocalUpdateState state)
    {
        var changed = false;

        if (NetworkUpdateLabels.IsLegacyPlaceholder(state.InstalledReleaseId))
        {
            state.InstalledReleaseId = string.Empty;
            state.InstalledMajorVersion = string.Empty;
            if (string.IsNullOrWhiteSpace(state.InstalledDisplayName)
                || NetworkUpdateLabels.IsLegacyPlaceholder(state.InstalledDisplayName))
            {
                state.InstalledDisplayName = NetworkUpdateLabels.PreUpdateInstallLabel;
            }

            changed = true;
        }

        if (NetworkUpdateLabels.IsLegacyPlaceholder(state.PreviousReleaseId))
        {
            state.PreviousReleaseId = string.Empty;
            state.PreviousMajorVersion = string.Empty;
            if (string.IsNullOrWhiteSpace(state.PreviousDisplayName)
                || NetworkUpdateLabels.IsLegacyPlaceholder(state.PreviousDisplayName))
            {
                state.PreviousDisplayName = NetworkUpdateLabels.PreUpdateInstallLabel;
            }

            changed = true;
        }

        return changed;
    }
}
