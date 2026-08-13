using Models.DTO;

namespace Client_App.Services.Updates;

/// <summary>
/// Текстовое представление версии для UI.
/// Формат: 1.3.0.11_test7
/// </summary>
public static class NetworkUpdateLabels
{
    public static string FormatVersion(string? majorVersion, string? releaseId)
    {
        if (!string.IsNullOrWhiteSpace(majorVersion) && !string.IsNullOrWhiteSpace(releaseId))
        {
            return $"{majorVersion.Trim()}.{releaseId.Trim()}";
        }

        if (!string.IsNullOrWhiteSpace(releaseId))
        {
            return releaseId.Trim();
        }

        if (!string.IsNullOrWhiteSpace(majorVersion))
        {
            return majorVersion.Trim();
        }

        return string.Empty;
    }

    public static string FormatInstalled(LocalUpdateState localState)
    {
        if (string.IsNullOrWhiteSpace(localState.InstalledReleaseId))
        {
            return "не зафиксирована (первая установка через автообновление)";
        }

        return FormatVersion(localState.InstalledMajorVersion, localState.InstalledReleaseId);
    }

    public static string FormatRemote(NetworkReleaseInfo release) =>
        FormatVersion(release.MajorVersion, release.ReleaseId);
}
