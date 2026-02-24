using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Passports;
using Client_App.Views;

namespace Client_App;

public partial class PackagePassportWindow : BaseWindow<PackagePassportWindowVM>
{

    public PackagePassportWindowVM? VM
    {
        get
        {
            if (DataContext is PackagePassportWindowVM)
                return DataContext as PackagePassportWindowVM;
            else
                return null;
        }
    }
    public PackagePassportWindow()
    {

        InitializeComponent();
    }
    public PackagePassportWindow(PackagePassportWindowVM vm)
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