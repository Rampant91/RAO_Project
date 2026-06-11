using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public sealed class MultipleRaodbFilesMessageWindowVM : INotifyPropertyChanged
{
    public const string MessageText =
        "В папке RAO обнаружено более одного файла базы данных (.RAODB)." +
        "\nРекомендуется хранить в этой папке не более одного файла, остальные файлы .RAODB перенести в другие папки." +
        "\nВыберите файл для открытия:";

    private RaodbFileListItem? _selectedFile;

    public MultipleRaodbFilesMessageWindowVM()
    {
        Files = new ObservableCollection<RaodbFileListItem>();
    }

    public MultipleRaodbFilesMessageWindowVM(IEnumerable<FileInfo> files)
    {
        Files = new ObservableCollection<RaodbFileListItem>(
            files
                .OrderByDescending(f => f.LastWriteTime)
                .Select(f => new RaodbFileListItem(f)));
    }

    public ObservableCollection<RaodbFileListItem> Files { get; }

    public RaodbFileListItem? SelectedFile
    {
        get => _selectedFile;
        set
        {
            if (_selectedFile == value)
                return;

            _selectedFile = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
