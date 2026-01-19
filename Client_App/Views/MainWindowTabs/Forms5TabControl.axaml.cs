using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views;

namespace Client_App.Views.MainWindowTabs;

public partial class Forms5TabControl : UserControl
{
    public Forms5TabControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}