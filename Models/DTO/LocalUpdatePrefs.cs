using System;

namespace Models.DTO;

/// <summary>
/// Флаги проверки обновлений для конкретной локальной копии программы
/// (.mpzf-update/prefs.json). Не общие для пользователя в LocalAppData.
/// </summary>
public class LocalUpdatePrefs
{
    /// <summary>
    /// Время последней автопроверки обновлений.
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
}
