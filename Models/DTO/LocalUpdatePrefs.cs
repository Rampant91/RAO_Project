using System;

namespace Models.DTO;

/// <summary>
/// Флаги проверки обновлений для конкретной локальной копии программы
/// (.mpzf-update/prefs.json). Не общие для пользователя в LocalAppData.
/// </summary>
public class LocalUpdatePrefs
{
    /// <summary>
    /// Время последнего показа предложения обновить / отметки «напомнить позже» для этого ключа.
    /// </summary>
    public DateTime? LastUpdateCheck { get; set; }

    /// <summary>
    /// Пропущенный releaseId сетевой выкладки (отдел).
    /// </summary>
    public string SkippedReleaseId { get; set; } = string.Empty;

    /// <summary>
    /// Пропущенная версия с сайта (внешние пользователи).
    /// </summary>
    public string SkippedVersion { get; set; } = string.Empty;

    /// <summary>
    /// releaseId, для которого уже показывали автодиалог (в т.ч. «напомнить позже»).
    /// </summary>
    public string LastNotifiedReleaseId { get; set; } = string.Empty;

    /// <summary>
    /// Версия с сайта, для которой уже показывали автодиалог.
    /// </summary>
    public string LastNotifiedVersion { get; set; } = string.Empty;
}
