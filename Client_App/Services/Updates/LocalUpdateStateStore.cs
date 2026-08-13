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
            return JsonConvert.DeserializeObject<LocalUpdateState>(json) ?? new LocalUpdateState();
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
}
