using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views;

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