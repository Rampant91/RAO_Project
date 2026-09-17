using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Controls;

namespace Client_App.Views.Controls;

public partial class PackagePassportPanelControl : UserControl
{
    PackagePassportPanelControlVM vm => DataContext as PackagePassportPanelControlVM;
    public PackagePassportPanelControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

    }

}