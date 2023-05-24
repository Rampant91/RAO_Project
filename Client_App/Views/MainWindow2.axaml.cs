using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views;

public partial class MainWindow2 : Window
{
    public MainWindow2()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}