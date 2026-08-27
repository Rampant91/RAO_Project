using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.StoragePoints;

namespace Client_App;

public partial class StoragePointsMenuWindow : Window
{
    public StoragePointsMenuWindow()
    {
        InitializeComponent();

    }
    public StoragePointsMenuWindow(StoragePointsMenuWindowVM vm)
    {
        DataContext = vm;
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    #if DEBUG
        this.AttachDevTools();
    #endif
        WindowState = WindowState.Maximized;
    }
}