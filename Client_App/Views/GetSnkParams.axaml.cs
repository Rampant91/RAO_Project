using Avalonia.Controls;
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

    #region CancelButtonClick

    private void CancelButtonClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    #endregion

    private void AllCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckAll = _vm.CheckPasNum = _vm.CheckType = _vm.CheckRadionuclids = _vm.CheckFacNum = _vm.CheckPackNumber = true;
        }
    }

    private void AllCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckPasNum = _vm.CheckType = _vm.CheckRadionuclids = _vm.CheckFacNum = _vm.CheckPackNumber = false;
        }
    }

    private void AnyCheckBox_Clicked(object sender, RoutedEventArgs e)
    {
        var allCheckBox = (sender as Control).FindNameScope().Find("All") as CheckBox;
        allCheckBox!.IsThreeState = true;
        _vm.CheckAll = _vm switch
        {
            { CheckPasNum: true, CheckType: true, CheckRadionuclids: true, CheckFacNum: true, CheckPackNumber: true } => true,
            { CheckPasNum: false, CheckType: false, CheckRadionuclids: false, CheckFacNum: false, CheckPackNumber: false } => false,
            _ => allCheckBox.IsChecked = null
        };
    }

    #endregion
}