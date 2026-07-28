using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public class GetTransferReceiveParams : BaseWindow<GetTransferReceiveParamsVM>
{
    public GetTransferReceiveParamsVM Vm = null!;

    public GetTransferReceiveParams()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new GetTransferReceiveParamsVM();
        Vm = (DataContext as GetTransferReceiveParamsVM)!;
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
