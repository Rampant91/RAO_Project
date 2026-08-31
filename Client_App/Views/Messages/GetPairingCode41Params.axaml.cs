using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public partial class GetPairingCode41Params : BaseWindow<GetPairingCode41ParamsVM>
{
    public GetPairingCode41ParamsVM Vm = null!;

    public GetPairingCode41Params()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new GetPairingCode41ParamsVM();
        Vm = (DataContext as GetPairingCode41ParamsVM)!;
    }

    private void OnOkButtonClicked(object? sender, RoutedEventArgs e)
    {
        Vm.Ok = true;
        Close();
    }

    private void OnCancelButtonClicked(object? sender, RoutedEventArgs e)
    {
        Vm.Ok = false;
        Close();
    }
}
