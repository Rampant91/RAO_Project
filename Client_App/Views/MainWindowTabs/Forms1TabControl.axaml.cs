using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views.MainWindowTabs;

public partial class Forms1TabControl : UserControl
{
    public Forms1TabControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}