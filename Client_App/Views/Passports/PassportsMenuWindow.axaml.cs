using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Passports;

namespace Client_App;

public partial class PassportsMenuWindow : Window
{
    public PassportsMenuWindow()
    {
        InitializeComponent();

    }
    public PassportsMenuWindow(PassportsMenuWindowVM vm)
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