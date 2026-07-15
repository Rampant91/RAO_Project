using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;
/// <summary>
/// ViewModel окна выбора файла .RAODB при запуске, если в папке RAO найдено несколько баз данных.
/// </summary>
public sealed class MultipleRaodbFilesMessageWindowVM : INotifyPropertyChanged
{
    /// <summary>Текст сообщения над списком файлов.</summary>
    public const string messageText =
        "В папке RAO обнаружено более одного файла базы данных .RAODB." +
        "\nРекомендуется хранить в этой папке не более одного файла базы данных. " +
        "\nДля этого откройте папку RAO, переместите лишние файлы в другое место и заново откройте программу." +
        "\nЕсли вы не хотите перемещать файлы и уверены в том, какой файл БД нужно открыть, то выберите его из списка:";

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

    /// <summary>
    /// Перечитывает файлы .RAODB из папки RAO и обновляет таблицу.
    /// </summary>
    public void RefreshFilesFromRaoDirectory()
    {
        var items = BaseVM.GetRaodbFiles(new DirectoryInfo(BaseVM.RaoDirectory))
            .OrderByDescending(f => f.LastWriteTime)
            .Select(f => new RaodbFileListItem(f))
            .ToList();

        Files.Clear();
        foreach (var item in items)
            Files.Add(item);

        SelectedFile = null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
