using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views.MainWindowTabs;

public partial class Forms2TabControl : UserControl
{
    public Forms2TabControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}