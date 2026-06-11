using System.Collections.Generic;
using System.IO;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public partial class MultipleRaodbFilesMessageWindow : BaseWindow<BaseVM>
{
    public MultipleRaodbFilesMessageWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new MultipleRaodbFilesMessageWindowVM();
    }

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
