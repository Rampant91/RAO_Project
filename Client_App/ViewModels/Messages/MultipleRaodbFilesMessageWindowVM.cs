using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

/// <summary>
/// ViewModel окна выбора файла .RAODB, если в папке RAO найдено несколько баз данных.
/// </summary>
public sealed class MultipleRaodbFilesMessageWindowVM : INotifyPropertyChanged
{
    /// <summary>Текст сообщения над списком файлов.</summary>
    public const string MessageText =
        "В папке RAO обнаружено более одного файла базы данных (.RAODB)." +
        "\nРекомендуется хранить в этой папке не более одного файла, остальные файлы .RAODB перенести в другие папки." +
        "\nВыберите файл для открытия:";

    private RaodbFileListItem? _selectedFile;

    /// <summary>Конструктор для дизайнера.</summary>
    public MultipleRaodbFilesMessageWindowVM()
    {
        Files = new ObservableCollection<RaodbFileListItem>();
    }

    /// <summary>
    /// Создаёт ViewModel со списком файлов, отсортированных по дате изменения (сначала новые).
    /// </summary>
    /// <param name="files">Файлы .RAODB из папки RAO.</param>
    public MultipleRaodbFilesMessageWindowVM(IEnumerable<FileInfo> files)
    {
        Files = new ObservableCollection<RaodbFileListItem>(
            files
                .OrderByDescending(f => f.LastWriteTime)
                .Select(f => new RaodbFileListItem(f)));
    }

    /// <summary>Файлы базы данных для отображения в таблице.</summary>
    public ObservableCollection<RaodbFileListItem> Files { get; }

    /// <summary>Выбранный пользователем файл базы данных.</summary>
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
