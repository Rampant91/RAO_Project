using System;
using Models.DTO;

namespace Client_App.Services.Updates;

/// <summary>
/// Текстовое представление версии для UI.
/// Формат: 1.3.0.11_test7
/// </summary>
public static class NetworkUpdateLabels
{
    /// <summary>Служебный id из старых сборок updater; в UI не показываем.</summary>
    public const string LegacyPreUpdateReleaseId = "pre-update";

    public const string UntrackedInstallLabel = "локальная установка (версия ещё не зафиксирована)";

    public const string PreUpdateInstallLabel = "локальная установка до обновления";

    public static string FormatVersion(string? majorVersion, string? releaseId)
    {
        if (IsLegacyPlaceholder(releaseId))
        {
            return PreUpdateInstallLabel;
        }

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
        if (!string.IsNullOrWhiteSpace(localState.InstalledReleaseId)
            && !IsLegacyPlaceholder(localState.InstalledReleaseId))
        {
            return FormatVersion(localState.InstalledMajorVersion, localState.InstalledReleaseId);
        }

        if (!string.IsNullOrWhiteSpace(localState.InstalledDisplayName))
        {
            var display = localState.InstalledDisplayName.Trim();
            if (!IsLegacyPlaceholder(display))
            {
                return display;
            }
        }

        if (IsLegacyPlaceholder(localState.InstalledReleaseId))
        {
            return PreUpdateInstallLabel;
        }

        return UntrackedInstallLabel;
    }

    public static string FormatRemote(NetworkReleaseInfo release) =>
        FormatVersion(release.MajorVersion, release.ReleaseId);

    /// <summary>
    /// Версия для шапки окна: текстовое имя выкладки отдела (1.3.0.11_test5),
    /// иначе номер из сборки.
    /// </summary>
    public static string FormatForWindowTitle(LocalUpdateState localState, string assemblyVersion)
    {
        var installed = FormatInstalled(localState);
        return IsTrackedInstallLabel(installed) ? installed : assemblyVersion;
    }

    public static bool IsTrackedInstallLabel(string? label) =>
        !string.IsNullOrWhiteSpace(label)
        && !string.Equals(label, UntrackedInstallLabel, StringComparison.Ordinal)
        && !string.Equals(label, PreUpdateInstallLabel, StringComparison.Ordinal);

    public static bool IsLegacyPlaceholder(string? releaseId) =>
        string.Equals(releaseId?.Trim(), LegacyPreUpdateReleaseId, StringComparison.OrdinalIgnoreCase);
}
