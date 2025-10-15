using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client_App.Properties.ColumnWidthSettings
{
    public static class ExecutorDataManager
    {
        private static readonly string SettingsPath = "ExecutorData.json";

        // Опции для сериализации с обработкой ошибок
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        public static List<double> GetExecutorData(string formNum)
        {
            if (!File.Exists(SettingsPath))
                return new List<double>();

            try
            {
                var json = File.ReadAllText(SettingsPath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<double>();

                var settings = JsonSerializer.Deserialize<Dictionary<string, List<double>>>(json, Options);

                return settings?.GetValueOrDefault(formNum) ?? new List<double>();
            }
            catch
            {
                return new List<double>();
            }
        }

        public static void SaveExecutorData(List<double> formSettings, string formNum)
        {
            var currentSettings = GetAllExecutorData();
            currentSettings[formNum] = formSettings ?? new List<double>();
            SaveAllExecutorData(currentSettings);
        }

        private static Dictionary<string, List<double>> GetAllExecutorData()
        {
            if (!File.Exists(SettingsPath))
                return new Dictionary<string, List<double>>();

            try
            {
                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<Dictionary<string, List<double>>>(json, Options)
                       ?? new Dictionary<string, List<double>>();
            }
            catch
            {
                return new Dictionary<string, List<double>>();
            }
        }

        private static void SaveAllExecutorData(Dictionary<string, List<double>> settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, Options);
                File.WriteAllText(SettingsPath, json);
            }
            catch (System.Exception ex)
            {
                // Логирование ошибки
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }
    }
}