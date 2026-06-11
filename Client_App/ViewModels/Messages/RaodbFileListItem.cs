using System;
using System.Globalization;
using System.IO;

namespace Client_App.ViewModels.Messages;

public sealed class RaodbFileListItem
{
    public RaodbFileListItem(FileInfo fileInfo)
    {
        FileInfo = fileInfo;
        FileName = fileInfo.Name;
        LastModified = fileInfo.LastWriteTime.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
        SizeMb = Math.Round(fileInfo.Length / 1024.0 / 1024.0, 2).ToString("0.##", CultureInfo.InvariantCulture);
    }

    public FileInfo FileInfo { get; }

    public string FileName { get; }

    public string LastModified { get; }

    public string SizeMb { get; }
}
