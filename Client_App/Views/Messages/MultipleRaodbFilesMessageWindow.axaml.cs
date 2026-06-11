using System.Collections.Generic;
using System.IO;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

/// <summary>
/// Окно выбора файла .RAODB при запуске в режиме разработчика,
/// если в папке RAO находится более одного файла базы данных.
/// </summary>
public partial class MultipleRaodbFilesMessageWindow : BaseWindow<BaseVM>
{
    /// <summary>Конструктор для дизайнера.</summary>
    public MultipleRaodbFilesMessageWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new MultipleRaodbFilesMessageWindowVM();
    }

    /// <summary>
    /// Создаёт окно со списком найденных файлов .RAODB.
    /// </summary>
    /// <param name="files">Файлы .RAODB из папки RAO.</param>
    public MultipleRaodbFilesMessageWindow(IEnumerable<FileInfo> files)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new MultipleRaodbFilesMessageWindowVM(files);
    }

    private MultipleRaodbFilesMessageWindowVM ViewModel => (MultipleRaodbFilesMessageWindowVM)DataContext!;

    private void OnOpenClicked(object? sender, RoutedEventArgs e) =>
        Close(ViewModel.SelectedFile?.FileInfo);

    private void OnExitClicked(object? sender, RoutedEventArgs e) => Close(null);
}
