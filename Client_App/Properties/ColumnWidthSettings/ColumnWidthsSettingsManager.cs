using Client_App.Properties.UnifiedConfig;
using System.Collections.Generic;
using System.Diagnostics;

namespace Client_App.Properties.ColumnWidthSettings;

/// <summary>
/// Менеджер настроек ширины колонок
/// Теперь использует единый файл конфигурации unifiedConfig.json
/// </summary>
public static class ColumnSettingsManager
{
    /// <summary>
    /// Загружает настройки ширины колонок для указанной формы
    /// </summary>
    /// <param name="formNum">Номер формы (например, "1.1", "1.2", ...)</param>
    /// <returns>Список ширин колонок</returns>
    public static List<double> LoadSettings(string formNum)
    {
        try
        {
            return UnifiedConfigManager.LoadColumnWidths(formNum);
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine($"Failed to load column widths for form {formNum}: {ex.Message}");
            return new List<double>();
        }
    }

    /// <summary>
    /// Сохраняет настройки ширины колонок для указанной формы
    /// </summary>
    /// <param name="formSettings">Список ширин колонок</param>
    /// <param name="formNum">Номер формы</param>
    public static void SaveSettings(List<double> formSettings, string formNum)
    {
        try
        {
            UnifiedConfigManager.SaveColumnWidths(formSettings, formNum);
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine($"Failed to save column widths for form {formNum}: {ex.Message}");
        }
    }

    /// <summary>
    /// Загружает все настройки ширины колонок
    /// </summary>
    /// <returns>Словарь настроек для всех форм</returns>
    public static Dictionary<string, List<double>> LoadAllSettings()
    {
        try
        {
            return UnifiedConfigManager.LoadAllColumnWidths();
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine($"Failed to load all column widths: {ex.Message}");
            return new Dictionary<string, List<double>>();
        }
    }

    /// <summary>
    /// Сохраняет все настройки ширины колонок
    /// </summary>
    /// <param name="settings">Словарь настроек для всех форм</param>
    public static void SaveAllSettings(Dictionary<string, List<double>> settings)
    {
        try
        {
            UnifiedConfigManager.SaveAllColumnWidths(settings);
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine($"Failed to save all column widths: {ex.Message}");
        }
    }
}