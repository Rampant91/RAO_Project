using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Client_App.ViewModels;

namespace Client_App.Properties.UnifiedConfig;

/// <summary>
/// Единый менеджер конфигурации приложения.
/// Объединяет настройки ширины колонок и количества строк в один файл.
/// </summary>
public static class UnifiedConfigManager
{
    private const string UnifiedConfigFileName = "Config.json";
    private const string OldColumnWidthsFileName = "columnWidthsSettings.json";
    
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Разрешает больше символов без экранирования
    };

    #region Paths
    private static string GetUnifiedConfigPath()
    {
        return Path.Combine(BaseVM.ConfigDirectory, UnifiedConfigFileName);
    }

    private static string GetOldColumnWidthsPath()
    {
        return Path.Combine(BaseVM.ConfigDirectory, OldColumnWidthsFileName);
    }
    #endregion

    #region Load/Save
    /// <summary>
    /// Загружает полную конфигурацию приложения с миграцией старых настроек
    /// </summary>
    public static UnifiedConfig LoadConfig()
    {
        var configPath = GetUnifiedConfigPath();
        
        // Выполняем миграцию при необходимости
        MaybeMigrateOldSettings();
        
        return LoadConfigWithoutMigration();
    }

    /// <summary>
    /// Загружает конфигурацию без выполнения миграции
    /// </summary>
    private static UnifiedConfig LoadConfigWithoutMigration()
    {
        var configPath = GetUnifiedConfigPath();
        
        if (!File.Exists(configPath))
        {
            return new UnifiedConfig(); // Возвращаем конфигурацию по умолчанию
        }

        try
        {
            var json = File.ReadAllText(configPath);
            
            if (string.IsNullOrWhiteSpace(json))
            {
                return new UnifiedConfig();
            }

            // Проверяем, есть ли комментарии в файле
            if (json.Contains("_comment"))
            {
                return LoadConfigWithComments(json);
            }
            else
            {
                return JsonSerializer.Deserialize<UnifiedConfig>(json, Options) ?? new UnifiedConfig();
            }
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            return new UnifiedConfig();
        }
    }

    /// <summary>
    /// Загружает конфигурацию из JSON с комментариями
    /// </summary>
    private static UnifiedConfig LoadConfigWithComments(string json)
    {
        try
        {
            var configDict = JsonSerializer.Deserialize<Dictionary<string, object>>(json, Options);
            if (configDict == null)
                return new UnifiedConfig();

            var config = new UnifiedConfig();
            
            // Загружаем основные параметры, игнорируя комментарии
            if (configDict.TryGetValue("version", out var versionValue))
                config.Version = versionValue?.ToString() ?? "1.0";
            
            if (configDict.TryGetValue("lastModified", out var lastModifiedValue))
            {
                if (DateTime.TryParse(lastModifiedValue?.ToString(), out var lastModified))
                    config.LastModified = lastModified;
            }
            
            // Загружаем настройки ширины колонок
            if (configDict.TryGetValue("columnWidths", out var columnWidthsValue))
            {
                var columnWidthsDict = JsonSerializer.Deserialize<Dictionary<string, object>>(columnWidthsValue?.ToString()!, Options);
                if (columnWidthsDict?.TryGetValue("columnWidthSettings", out var settingsValue) == true)
                {
                    var settingsDict = JsonSerializer.Deserialize<Dictionary<string, List<double>>>(settingsValue?.ToString()!, Options);
                    if (settingsDict != null)
                    {
                        var orderedSettings = new OrderedDictionary();
                        foreach (var kvp in settingsDict)
                        {
                            orderedSettings[kvp.Key] = kvp.Value;
                        }
                        config.ColumnWidths.ColumnWidthSettings = orderedSettings;
                    }
                }
            }
            
            // Загружаем настройки количества строк
            if (configDict.TryGetValue("rowCounts", out var rowCountsValue))
            {
                var rowCountsDict = JsonSerializer.Deserialize<Dictionary<string, object>>(rowCountsValue?.ToString()!, Options);
                if (rowCountsDict?.TryGetValue("rowCountSettings", out var settingsValue) == true)
                {
                    var settingsDict = JsonSerializer.Deserialize<Dictionary<string, RowCountSettings>>(settingsValue?.ToString()!, Options);
                    if (settingsDict != null)
                    {
                        config.RowCounts.RowCountSettings = settingsDict;
                    }
                }
            }
            
            // Загружаем настройки автозаполнения
            if (configDict.TryGetValue("autoReplace", out var autoReplaceValue))
            {
                var autoReplaceDict = JsonSerializer.Deserialize<Dictionary<string, object>>(autoReplaceValue?.ToString()!, Options);
                if (autoReplaceDict?.TryGetValue("isEnabled", out var isEnabledValue) == true)
                {
                    if (bool.TryParse(isEnabledValue?.ToString(), out var isEnabled))
                        config.AutoReplace.IsEnabled = isEnabled;
                }
            }
            
            return config;
        }
        catch
        {
            return new UnifiedConfig();
        }
    }

    /// <summary>
    /// Сохраняет полную конфигурацию приложения с комментариями
    /// </summary>
    public static void SaveConfig(UnifiedConfig config)
    {
        try
        {
            var configPath = GetUnifiedConfigPath();
            var directory = Path.GetDirectoryName(configPath);
            
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            // Создаем конфигурацию с комментариями
            var configWithComments = CreateConfigWithComments(config);
            var json = JsonSerializer.Serialize(configWithComments, Options);
            File.WriteAllText(configPath, json);
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            // Ошибка сохранения конфигурации
        }
    }

    /// <summary>
    /// Создает конфигурацию с добавлением комментариев
    /// </summary>
    private static Dictionary<string, object> CreateConfigWithComments(UnifiedConfig config)
    {
        var result = new Dictionary<string, object>
        {
            ["_comment"] = "Конфигурация ПО «МПЗФ». Автоматически генерируется, не редактируйте вручную основные параметры.",
            ["_comment_version"] = "Версия формата конфигурации",
            ["version"] = config.Version,
            
            ["_comment_lastModified"] = "Дата последнего изменения конфигурации",
            ["lastModified"] = config.LastModified,
            
            ["_comment_columnWidths"] = "Настройки ширины колонок для всех форм. Ключ: номер формы (1.1, 1.2, notes), Значение: список ширин колонок",
            ["columnWidths"] = new Dictionary<string, object>
            {
                ["_comment"] = "Упорядоченный словарь настроек ширины колонок",
                ["columnWidthSettings"] = ConvertOrderedDictionaryToDictionary(config.ColumnWidths.ColumnWidthSettings)
            },
            
            ["_comment_rowCounts"] = "Настройки количества строк таблиц организаций и отчётов. Ключ: префикс формы (form1, form2), Значение: настройки количества строк",
            ["rowCounts"] = new Dictionary<string, object>
            {
                ["_comment"] = "Словарь настроек количества строк для организаций и форм",
                ["rowCountSettings"] = config.RowCounts.RowCountSettings
            },
            
            ["_comment_autoReplace"] = "Настройки автозаполнения в формах",
            ["autoReplace"] = new Dictionary<string, object>
            {
                ["_comment_isEnabled"] = "Включено ли автозаполнение по умолчанию для всех форм",
                ["isEnabled"] = config.AutoReplace.IsEnabled
            }
        };
        
        return result;
    }

    /// <summary>
    /// Конвертирует OrderedDictionary в обычный Dictionary для сериализации
    /// </summary>
    private static Dictionary<string, object> ConvertOrderedDictionaryToDictionary(OrderedDictionary orderedDict)
    {
        var result = new Dictionary<string, object>();
        foreach (DictionaryEntry entry in orderedDict)
        {
            result[entry.Key.ToString()!] = entry.Value;
        }
        return result;
    }
    #endregion

    #region Migration
    /// <summary>
    /// Проверяет и выполняет миграцию старых настроек в новый формат
    /// </summary>
    private static void MaybeMigrateOldSettings()
    {
        var configPath = GetUnifiedConfigPath();
        var oldColumnWidthsPath = GetOldColumnWidthsPath();

        // Если старого файла нет, миграция не нужна
        if (!File.Exists(oldColumnWidthsPath))
        {
            return;
        }

        var config = new UnifiedConfig();
        var migrated = false;

        // Если новый файл существует, загружаем его для слияния
        if (File.Exists(configPath))
        {
            try
            {
                config = LoadConfigWithoutMigration();
            }
            catch (Exception ex)
            {
                // Если не удалось загрузить новый файл, начинаем с чистого конфига
                config = new UnifiedConfig();
            }
        }

        // Миграция настроек ширины колонок из старого файла
        if (File.Exists(oldColumnWidthsPath))
        {
            try
            {
                var json = File.ReadAllText(oldColumnWidthsPath);
                
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var oldSettings = JsonSerializer.Deserialize<Dictionary<string, List<double>>>(json, Options);
                    if (oldSettings != null)
                    {
                        // Создаем OrderedDictionary с правильным порядком
                        var orderedSettings = new OrderedDictionary();
                        var order = new[] { "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9", 
                                           "2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12", 
                                           "notes" };
                        
                        // Сначала добавляем существующие настройки из нового файла (если есть)
                        foreach (var key in order)
                        {
                            if (config.ColumnWidths.ColumnWidthSettings.Contains(key))
                            {
                                orderedSettings[key] = config.ColumnWidths.ColumnWidthSettings[key];
                            }
                        }
                        
                        // Затем добавляем/обновляем из старого файла (старые данные имеют приоритет)
                        foreach (var key in order)
                        {
                            if (oldSettings.ContainsKey(key))
                            {
                                orderedSettings[key] = oldSettings[key];
                                migrated = true;
                            }
                        }
                        
                        // Добавляем любые другие ключи из старого файла
                        foreach (var kvp in oldSettings.Where(x => !order.Contains(x.Key)))
                        {
                            orderedSettings[kvp.Key] = kvp.Value;
                            migrated = true;
                        }
                        
                        // Добавляем оставшиеся ключи из нового файла
                        foreach (DictionaryEntry entry in config.ColumnWidths.ColumnWidthSettings)
                        {
                            var key = entry.Key.ToString();
                            if (key != null && !orderedSettings.Contains(key))
                            {
                                orderedSettings[key] = entry.Value;
                            }
                        }
                        
                        config.ColumnWidths.ColumnWidthSettings = orderedSettings;
                    }
                }
            }
            catch (Exception ex)
            {
                // Ошибка миграции настроек ширины колонок
            }
        }

        // Если была миграция, сохраняем и очищаем старые файлы
        if (migrated)
        {
            SaveConfig(config);
            CleanupOldConfigFiles();
        }
    }
    #endregion

    #region Column Widths Methods (для совместимости)
    /// <summary>
    /// Загружает настройки ширины колонок для конкретной формы
    /// </summary>
    public static List<double> LoadColumnWidths(string formNum)
    {
        var config = LoadConfig();
        if (config.ColumnWidths.ColumnWidthSettings.Contains(formNum))
        {
            return config.ColumnWidths.ColumnWidthSettings[formNum] as List<double> ?? new List<double>();
        }
        return [];
    }

    /// <summary>
    /// Сохраняет настройки ширины колонок для конкретной формы
    /// </summary>
    public static void SaveColumnWidths(List<double> formSettings, string formNum)
    {
        var config = LoadConfig();
        config.ColumnWidths.ColumnWidthSettings[formNum] = formSettings;
        
        // Переупорядочиваем ключи после добавления/обновления
        ReorderColumnWidths(config);
        
        SaveConfig(config);
    }

    #endregion

    #region Row Count Methods (для совместимости)
    /// <summary>
    /// Загружает настройки количества строк для указанной формы
    /// </summary>
    public static (int orgs, int forms) LoadRowCountSettings(string formPrefix, int defaultOrgs, int defaultForms)
    {
        var config = LoadConfig();
        if (config.RowCounts.RowCountSettings.TryGetValue(formPrefix, out var settings))
        {
            return (settings.RowsCountOrgs, settings.RowsCountForms);
        }
        
        return (defaultOrgs, defaultForms);
    }

    /// <summary>
    /// Сохраняет настройки количества строк для указанной формы
    /// </summary>
    public static void SaveRowCountSettings(string formPrefix, int rowsCountOrgs, int rowsCountForms)
    {
        var config = LoadConfig();
        if (!config.RowCounts.RowCountSettings.ContainsKey(formPrefix))
        {
            config.RowCounts.RowCountSettings[formPrefix] = new RowCountSettings();
        }
        
        config.RowCounts.RowCountSettings[formPrefix].RowsCountOrgs = rowsCountOrgs;
        config.RowCounts.RowCountSettings[formPrefix].RowsCountForms = rowsCountForms;
        SaveConfig(config);
    }
    #endregion

    #region Auto Replace Methods
    /// <summary>
    /// Загружает настройку автозаполнения
    /// </summary>
    public static bool LoadAutoReplaceEnabled()
    {
        var config = LoadConfig();
        return config.AutoReplace.IsEnabled;
    }

    /// <summary>
    /// Сохраняет настройку автозаполнения
    /// </summary>
    public static void SaveAutoReplaceEnabled(bool isEnabled)
    {
        var config = LoadConfig();
        config.AutoReplace.IsEnabled = isEnabled;
        SaveConfig(config);
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Переупорядочивает ключи в ColumnWidthSettings в правильном порядке
    /// </summary>
    private static void ReorderColumnWidths(UnifiedConfig config)
    {
        var currentSettings = config.ColumnWidths.ColumnWidthSettings;
        var orderedSettings = new OrderedDictionary();
        
        // Заданный порядок ключей
        var order = new[] { "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9", 
                           "2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12", 
                           "notes" };
        
        // Добавляем в нужном порядке
        foreach (var key in order)
        {
            if (currentSettings.Contains(key))
            {
                orderedSettings[key] = currentSettings[key];
            }
        }
        
        // Добавляем любые другие ключи, которых нет в списке (в алфавитном порядке)
        var otherKeys = new List<string>();
        foreach (DictionaryEntry entry in currentSettings)
        {
            var key = entry.Key.ToString();
            if (key != null && !order.Contains(key))
            {
                otherKeys.Add(key);
            }
        }
        
        foreach (var key in otherKeys.OrderBy(x => x))
        {
            orderedSettings[key] = currentSettings[key];
        }
        
        config.ColumnWidths.ColumnWidthSettings = orderedSettings;
    }
    #endregion

    #region Cleanup Old Config Files
    /// <summary>
    /// Безопасно удаляет старые конфигурационные файлы после успешной миграции
    /// </summary>
    private static void CleanupOldConfigFiles()
    {
        try
        {
            var oldColumnWidthsPath = GetOldColumnWidthsPath();
            var configPath = GetUnifiedConfigPath();

            // Проверяем, что новый конфигурационный файл существует и не пуст
            if (!File.Exists(configPath) || new FileInfo(configPath).Length == 0)
            {
                return;
            }

            // Проверяем, что в новом файле есть данные ширины колонок
            try
            {
                var config = LoadConfigWithoutMigration();
                var hasColumnWidths = config.ColumnWidths.ColumnWidthSettings.Count > 0;
                
                if (!hasColumnWidths)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }

            // Безопасно удаляем старый файл ширины колонок
            if (File.Exists(oldColumnWidthsPath))
            {
                try
                {
                    File.Delete(oldColumnWidthsPath);
                }
                catch (Exception ex)
                {
                    // Ошибка удаления старого файла ширины колонок
                }
            }
        }
        catch (Exception ex)
        {
            // Ошибка во время очистки конфигурационных файлов
        }
    }
    #endregion
}

#region Configuration Classes
/// <summary>
/// Единая конфигурация приложения
/// </summary>
public class UnifiedConfig
{
    public string Version { get; set; } = "1.0";
    public DateTime LastModified { get; set; } = DateTime.Now;
    
    public ColumnWidthsConfig ColumnWidths { get; set; } = new();
    
    public RowCountsConfig RowCounts { get; set; } = new();
    
    public AutoReplaceConfig AutoReplace { get; set; } = new();
}

/// <summary>
/// Конфигурация ширины колонок
/// </summary>
public class ColumnWidthsConfig
{
    public OrderedDictionary ColumnWidthSettings { get; set; } = new();
}

/// <summary>
/// Конфигурация количества строк
/// </summary>
public class RowCountsConfig
{
    public Dictionary<string, RowCountSettings> RowCountSettings { get; set; } = new();
}

/// <summary>
/// Настройки количества строк для конкретной формы
/// </summary>
public class RowCountSettings
{
    public int RowsCountOrgs { get; set; } = 6;
    public int RowsCountForms { get; set; } = 8;
}

/// <summary>
/// Конфигурация автозаполнения
/// </summary>
public class AutoReplaceConfig
{
    public bool IsEnabled { get; set; } = true;
}
#endregion
