using System;
using System.IO;
using Models.DTO;
using Newtonsoft.Json;

namespace Client_App.Services.Updates;

/// <summary>
/// prefs.json рядом с установкой: LastUpdateCheck / пропуски версий.
/// Отдельный файл от state.json, чтобы MpzfUpdater не затирал флаги.
/// </summary>
public class LocalUpdatePrefsStore
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        Formatting = Formatting.Indented
    };

    public LocalUpdatePrefs Load()
    {
        try
        {
            if (!File.Exists(NetworkUpdatePaths.PrefsFilePath))
            {
                return new LocalUpdatePrefs();
            }

            var json = File.ReadAllText(NetworkUpdatePaths.PrefsFilePath);
            return JsonConvert.DeserializeObject<LocalUpdatePrefs>(json) ?? new LocalUpdatePrefs();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load update prefs: {ex.Message}");
            return new LocalUpdatePrefs();
        }
    }

    public void Save(LocalUpdatePrefs prefs)
    {
        Directory.CreateDirectory(NetworkUpdatePaths.UpdateMetaDirectory);
        var json = JsonConvert.SerializeObject(prefs, JsonSettings);
        File.WriteAllText(NetworkUpdatePaths.PrefsFilePath, json);
    }

    public DateTime GetLastUpdateCheck() =>
        Load().LastUpdateCheck ?? DateTime.MinValue;

    public void MarkUpdateCheckCompleted()
    {
        var prefs = Load();
        prefs.LastUpdateCheck = DateTime.Now;
        Save(prefs);
    }

    /// <summary>
    /// Зафиксировать, что пользователю уже предлагали этот сетевой релиз (автодиалог / «напомнить позже»).
    /// </summary>
    public void MarkNetworkReleaseNotified(string releaseId)
    {
        var prefs = Load();
        prefs.LastNotifiedReleaseId = releaseId ?? string.Empty;
        prefs.LastUpdateCheck = DateTime.Now;
        Save(prefs);
    }

    /// <summary>
    /// Зафиксировать, что пользователю уже предлагали эту версию с сайта.
    /// </summary>
    public void MarkWebsiteVersionNotified(string version)
    {
        var prefs = Load();
        prefs.LastNotifiedVersion = version ?? string.Empty;
        prefs.LastUpdateCheck = DateTime.Now;
        Save(prefs);
    }

    public void ResetCheckThrottleForDebug()
    {
        var prefs = Load();
        prefs.LastUpdateCheck = DateTime.MinValue;
        prefs.SkippedReleaseId = string.Empty;
        prefs.SkippedVersion = string.Empty;
        prefs.LastNotifiedReleaseId = string.Empty;
        prefs.LastNotifiedVersion = string.Empty;
        Save(prefs);
    }

    public string? GetSkippedReleaseId()
    {
        var skipped = Load().SkippedReleaseId?.Trim();
        return string.IsNullOrWhiteSpace(skipped) ? null : skipped;
    }

    public void SetSkippedReleaseId(string releaseId)
    {
        var prefs = Load();
        prefs.SkippedReleaseId = releaseId ?? string.Empty;
        Save(prefs);
    }

    public string? GetLastNotifiedReleaseId()
    {
        var notified = Load().LastNotifiedReleaseId?.Trim();
        return string.IsNullOrWhiteSpace(notified) ? null : notified;
    }

    public string? GetLastNotifiedWebsiteVersion()
    {
        var notified = Load().LastNotifiedVersion?.Trim();
        return string.IsNullOrWhiteSpace(notified) ? null : notified;
    }

    public Version? GetSkippedWebsiteVersion()
    {
        var skipped = Load().SkippedVersion;
        return Version.TryParse(skipped, out var version) ? version : null;
    }

    public void SetSkippedWebsiteVersion(string version)
    {
        var prefs = Load();
        prefs.SkippedVersion = version ?? string.Empty;
        Save(prefs);
    }
}
