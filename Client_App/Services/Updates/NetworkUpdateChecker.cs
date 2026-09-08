using System;
using System.IO;
using Models.DTO;
using Newtonsoft.Json;

namespace Client_App.Services.Updates;

/// <summary>
/// Чтение latest.json с сетевой (или локальной тестовой) шары.
/// </summary>
public class NetworkUpdateChecker
{
    public NetworkReleaseInfo? TryReadLatest(string? networkRoot = null)
    {
        try
        {
            networkRoot ??= NetworkUpdatePaths.ResolveNetworkRoot();
            if (!Directory.Exists(networkRoot))
            {
                return null;
            }

            var latestPath = NetworkUpdatePaths.GetLatestJsonPath(networkRoot);
            if (!File.Exists(latestPath))
            {
                return null;
            }

            var json = File.ReadAllText(latestPath);
            var release = JsonConvert.DeserializeObject<NetworkReleaseInfo>(json);
            if (release == null || string.IsNullOrWhiteSpace(release.ReleaseId))
            {
                return null;
            }

            var releaseDir = NetworkUpdatePaths.GetReleaseDirectory(networkRoot, release);
            if (!Directory.Exists(releaseDir))
            {
                System.Diagnostics.Debug.WriteLine($"Network release folder missing: {releaseDir}");
                return null;
            }

            var exePath = Path.Combine(releaseDir, NetworkUpdatePaths.AppExeName);
            if (!File.Exists(exePath))
            {
                System.Diagnostics.Debug.WriteLine($"Client_App.exe not found in: {releaseDir}");
                return null;
            }

            if (string.IsNullOrWhiteSpace(release.DisplayName))
            {
                release.DisplayName = NetworkUpdateLabels.FormatVersion(release.MajorVersion, release.ReleaseId);
            }

            return release;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Network update check failed: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Нужно ли предлагать обновление / repair.
    /// Если метки в state совпали с latest, но локальные файлы не совпадают с релизом на шаре —
    /// считаем обновление нужным (частичный apply / рассинхрон).
    /// </summary>
    /// <param name="localAppDirectory">Для тестов; по умолчанию — каталог текущей установки.</param>
    public bool IsUpdateAvailable(
        NetworkReleaseInfo remote,
        LocalUpdateState local,
        string? networkRoot = null,
        string? localAppDirectory = null)
    {
        // Нет учёта версии (или старый служебный id) — предлагаем обновление,
        // если bootstrap не зафиксировал совпадение с latest.
        if (string.IsNullOrWhiteSpace(local.InstalledReleaseId)
            || NetworkUpdateLabels.IsLegacyPlaceholder(local.InstalledReleaseId))
        {
            return true;
        }

        if (!IdsMatch(remote, local))
        {
            return true;
        }

        if (!string.IsNullOrWhiteSpace(networkRoot)
            && !LocalUpdateStateStore.LooksIdenticalToRelease(remote, networkRoot, localAppDirectory))
        {
            return true;
        }

        return false;
    }

    private static bool IdsMatch(NetworkReleaseInfo remote, LocalUpdateState local)
    {
        var sameRelease = string.Equals(
            remote.ReleaseId,
            local.InstalledReleaseId,
            StringComparison.OrdinalIgnoreCase);

        if (!sameRelease)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(remote.MajorVersion))
        {
            return true;
        }

        return string.Equals(
            remote.MajorVersion,
            local.InstalledMajorVersion,
            StringComparison.OrdinalIgnoreCase);
    }
}
