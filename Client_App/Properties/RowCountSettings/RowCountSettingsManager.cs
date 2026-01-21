using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Client_App.Properties.UnifiedConfig;

namespace Client_App.Properties.RowCountSettings;

/// <summary>
/// Менеджер настроек количества строк для DataGrid
/// Теперь использует единый файл конфигурации Config.json
/// </summary>
public static class RowCountSettingsManager
{
    #region Generic Methods
    
    /// <summary>
    /// Загружает настройки количества строк для указанной формы
    /// </summary>
    /// <param name="formPrefix">Префикс формы (например, "form1", "form2")</param>
    /// <param name="defaultOrgs">Значение по умолчанию для организаций</param>
    /// <param name="defaultForms">Значение по умолчанию для форм</param>
    /// <returns>Кортеж (orgs, forms) с настройками</returns>
    public static (int orgs, int forms) LoadSettings(string formPrefix, int defaultOrgs, int defaultForms)
    {
        try
        {
            return UnifiedConfigManager.LoadRowCountSettings(formPrefix, defaultOrgs, defaultForms);
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            Debug.WriteLine($"Failed to load settings for {formPrefix}: {ex.Message}");
            return (defaultOrgs, defaultForms);
        }
    }

    /// <summary>
    /// Сохраняет настройки количества строк для указанной формы
    /// </summary>
    /// <param name="formPrefix">Префикс формы (например, "form1", "form2")</param>
    /// <param name="rowsCountOrgs">Количество строк организаций</param>
    /// <param name="rowsCountForms">Количество строк форм</param>
    public static void SaveSettings(string formPrefix, int rowsCountOrgs, int rowsCountForms)
    {
        try
        {
            UnifiedConfigManager.SaveRowCountSettings(formPrefix, rowsCountOrgs, rowsCountForms);
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            Debug.WriteLine($"Failed to save settings for {formPrefix}: {ex.Message}");
        }
    }
    
    #endregion

    #region Form 1 Settings (for backward compatibility)
    
    [Obsolete("Use LoadSettings(\"form1\", 6, 8) instead")]
    public static (int orgs, int forms) LoadForm1Settings()
    {
        return LoadSettings("form1", 6, 8);
    }

    [Obsolete("Use SaveSettings(\"form1\", rowsCountOrgs, rowsCountForms) instead")]
    public static void SaveForm1Settings(int rowsCountOrgs, int rowsCountForms)
    {
        SaveSettings("form1", rowsCountOrgs, rowsCountForms);
    }
    
    #endregion

    #region Form 2 Settings (for backward compatibility)
    
    [Obsolete("Use LoadSettings(\"form2\", 10, 10) instead")]
    public static (int orgs, int forms) LoadForm2Settings()
    {
        return LoadSettings("form2", 10, 10);
    }

    [Obsolete("Use SaveSettings(\"form2\", rowsCountOrgs, rowsCountForms) instead")]
    public static void SaveForm2Settings(int rowsCountOrgs, int rowsCountForms)
    {
        SaveSettings("form2", rowsCountOrgs, rowsCountForms);
    }
    
    #endregion
}
