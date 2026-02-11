using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public class GetRegionAndFormNums : BaseWindow<GetRegionAndFormNumsVM>
{
    public GetRegionAndFormNumsVM _vm = null!;

    public GetRegionAndFormNums()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new GetRegionAndFormNumsVM();
        _vm = (DataContext as GetRegionAndFormNumsVM)!;
    }

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

    #region FormsCheckBoxes
    
    private void AllFormsCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckAllForms = _vm.CheckForm11 = _vm.CheckForm13 = true;
        }
    }

    private void AllFormsCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckAllForms = _vm.CheckForm11 = _vm.CheckForm13 = false;
        }
    }

    private void AnyFormCheckBox_Clicked(object sender, RoutedEventArgs e)
    {
        var allCheckBox = (sender as Control).FindNameScope().Find("AllForms") as CheckBox;
        allCheckBox!.IsThreeState = true;
        _vm.CheckAllForms = _vm switch
        {
            { CheckForm11: true, CheckForm13: true } => true,
            { CheckForm11: false, CheckForm13: false } => false,
            _ => allCheckBox.IsChecked = null
        };
    }

    #endregion

    #region SnkParamsCheckBoxes

    private void AllSnkParamsCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckAllSnkParams = _vm.CheckPasNum = _vm.CheckType = _vm.CheckRadionuclids = _vm.CheckFacNum = _vm.CheckPackNumber = true;
        }
    }

    private void AllSnkParamsCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckPasNum = _vm.CheckType = _vm.CheckRadionuclids = _vm.CheckFacNum = _vm.CheckPackNumber = false;
        }
    }

    private void AnySnkParamsCheckBox_Clicked(object sender, RoutedEventArgs e)
    {
        var allCheckBox = (sender as Control).FindNameScope().Find("AllSnkParams") as CheckBox;
        allCheckBox!.IsThreeState = true;
        _vm.CheckAllSnkParams = _vm switch
        {
            { CheckPasNum: true, CheckType: true, CheckRadionuclids: true, CheckFacNum: true, CheckPackNumber: true } => true,
            { CheckPasNum: false, CheckType: false, CheckRadionuclids: false, CheckFacNum: false, CheckPackNumber: false } => false,
            _ => allCheckBox.IsChecked = null
        };
    }

    #endregion

    #endregion
}