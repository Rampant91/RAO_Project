using System;

namespace Models.DTO;

/// <summary>
/// Описание выкладки МПЗФ на сетевой шаре (latest.json / manifest.json).
/// </summary>
public class NetworkReleaseInfo
{
    /// <summary>
    /// Уникальный идентификатор выкладки (не обязан совпадать с AssemblyVersion).
    /// </summary>
    public string ReleaseId { get; set; } = string.Empty;

    /// <summary>
    /// Отображаемое имя для пользователя.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Версия сборки (справочно).
    /// </summary>
    public string AssemblyVersion { get; set; } = string.Empty;

    /// <summary>
    /// Дата публикации выкладки.
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// Папка основной подверсии на шаре (например 1.3.0), меняется редко.
    /// </summary>
    public string MajorVersion { get; set; } = string.Empty;

    /// <summary>
    /// Путь к папке релиза относительно корня обновлений.
    /// Если пусто — собирается как {MajorVersion}/{ReleaseId}/win-x64.
    /// </summary>
    public string RelativePath { get; set; } = string.Empty;

    /// <summary>
    /// Краткое описание изменений.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Если true — нельзя пропустить релиз из диалога.
    /// </summary>
    public bool Required { get; set; }
}
