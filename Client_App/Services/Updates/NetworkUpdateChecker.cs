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

    public bool IsUpdateAvailable(NetworkReleaseInfo remote, LocalUpdateState local)
    {
        if (string.IsNullOrWhiteSpace(local.InstalledReleaseId))
        {
            return true;
        }

        if (!string.IsNullOrWhiteSpace(remote.MajorVersion))
        {
            var sameMajor = string.Equals(
                remote.MajorVersion,
                local.InstalledMajorVersion,
                StringComparison.OrdinalIgnoreCase);
            var sameRelease = string.Equals(
                remote.ReleaseId,
                local.InstalledReleaseId,
                StringComparison.OrdinalIgnoreCase);
            return !sameMajor || !sameRelease;
        }

        return !string.Equals(remote.ReleaseId, local.InstalledReleaseId, StringComparison.OrdinalIgnoreCase);
    }
}
