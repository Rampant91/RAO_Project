using MsBox.Avalonia;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.Commands.AsyncCommands;
using Client_App.ViewModels;
using Client_App.ViewModels.Messages;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace Client_App.Views.Messages;

/// <summary>
/// Окно выбора файла .RAODB при запуске, если в папке RAO находится более одного файла базы данных.
/// </summary>
public partial class MultipleRaodbFilesMessageWindow : BaseWindow<BaseVM>
{
    private readonly OpenFolderAsyncCommand _openFolderAsyncCommand = new();

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

    private async void OnOpenClicked(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedFile?.FileInfo is not { } fileInfo)
            return;

        if (File.Exists(fileInfo.FullName))
        {
            Close(fileInfo);
            return;
        }

        await ShowFileNotFoundMessageAsync();
        ViewModel.RefreshFilesFromRaoDirectory();
    }

    private async Task ShowFileNotFoundMessageAsync() =>
        await MessageBoxManager
            .GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                CanResize = true,
                ContentTitle = "Выбор файла базы данных",
                ContentHeader = "Уведомление",
                ContentMessage = "Выбранный файл не найден (возможно, был переименован, перемещён или удалён). " +
                                 "Список файлов обновлён, заново выберите файл.",
                MinWidth = 250,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            }).ShowWindowDialogAsync(this);

    private async void OnOpenRaoFolderClicked(object? sender, RoutedEventArgs e) =>
        await _openFolderAsyncCommand.AsyncExecute(BaseVM.RaoDirectory);

    private void OnExitClicked(object? sender, RoutedEventArgs e) => Close(null);
}
