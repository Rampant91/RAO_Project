using System;
using System.Globalization;
using System.IO;

namespace Client_App.ViewModels.Messages;

/// <summary>
/// Строка списка файлов .RAODB для окна выбора базы данных при запуске.
/// </summary>
public sealed class RaodbFileListItem
{
    public RaodbFileListItem(FileInfo fileInfo)
    {
        FileInfo = fileInfo;
        FileName = fileInfo.Name;
        LastModified = fileInfo.LastWriteTime.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
        SizeMb = Math.Round(fileInfo.Length / 1024.0 / 1024.0, 2).ToString("0.##", CultureInfo.InvariantCulture);
    }

    /// <summary>Исходный файл на диске.</summary>
    public FileInfo FileInfo { get; }

    /// <summary>Имя файла с расширением.</summary>
    public string FileName { get; }

    /// <summary>Дата последнего изменения для отображения в таблице.</summary>
    public string LastModified { get; }

    /// <summary>Размер файла в мегабайтах для отображения в таблице.</summary>
    public string SizeMb { get; }
}
