using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;

namespace Client_App.Views;

public class GetSnkParams : BaseWindow<GetSnkParamsVM>
{
    public GetSnkParamsVM _vm = null!;

    #region InitializeComponent

    public GetSnkParams()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new GetSnkParamsVM();
        _vm = (DataContext as GetSnkParamsVM)!;
    }

    #endregion

    #region Buttons

    #region OkButtonClick

    private void OkButtonClick(object? sender, RoutedEventArgs e)
    {
        _vm.Ok = true;
        Close();
    }

    #endregion

    private void checkBox_Checked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckPasNum = _vm.CheckType = _vm.CheckRadionuclids = _vm.CheckFacNum = _vm.CheckPackNumber = true;
        }
    }

    private void checkBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckPasNum = _vm.CheckType = _vm.CheckRadionuclids = _vm.CheckFacNum = _vm.CheckPackNumber = false;
        }
    }

    #endregion
}