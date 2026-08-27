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

    public void ResetCheckThrottleForDebug()
    {
        var prefs = Load();
        prefs.LastUpdateCheck = DateTime.MinValue;
        prefs.SkippedReleaseId = string.Empty;
        prefs.SkippedVersion = string.Empty;
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
