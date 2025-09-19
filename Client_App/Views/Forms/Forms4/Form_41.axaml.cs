using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App;

public partial class Form_41 : Window
{
    public Form_41()
    {
        InitializeComponent();
        Show();
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