using System;

namespace Models.DTO;

/// <summary>
/// Информация об обновлении программы
/// </summary>
public class UpdateInfo
{
    /// <summary>
    /// Версия обновления
    /// </summary>
    public Version Version { get; set; }

    /// <summary>
    /// Дата выпуска версии
    /// </summary>
    public string ReleaseDate { get; set; } = string.Empty;

    /// <summary>
    /// URL для скачивания
    /// </summary>
    public string DownloadUrl { get; set; } = string.Empty;

    /// <summary>
    /// Описание изменений (опционально)
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
