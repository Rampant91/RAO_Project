using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Passports;
using Client_App.Views;

namespace Client_App;

public partial class PackagePassportWindow : BaseWindow<PackagePassportWindowVM>
{
    private readonly PackagePassportWindowVM _vm = null!;
    public PackagePassportWindow()
    {
        InitializeComponent();

    }
    public PackagePassportWindow(PackagePassportWindowVM vm)
    {
        InitializeComponent();

        DataContext = vm;
        _vm = vm;
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