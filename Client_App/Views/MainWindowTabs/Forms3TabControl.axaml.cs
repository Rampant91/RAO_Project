using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views.MainWindowTabs;

public partial class Forms3TabControl : UserControl
{
    public Forms3TabControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}