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

    private void AllCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckAll = _vm.CheckForm11 = _vm.CheckForm13 = true;
        }
    }

    private void AllCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckAll = _vm.CheckForm11 = _vm.CheckForm13 = false;
        }
    }

    private void AnyCheckBox_Clicked(object sender, RoutedEventArgs e)
    {
        var allCheckBox = (sender as Control).FindNameScope().Find("All") as CheckBox;
        allCheckBox!.IsThreeState = true;
        _vm.CheckAll = _vm switch
        {
            { CheckForm11: true, CheckForm13: true } => true,
            { CheckForm11: false, CheckForm13: false } => false,
            _ => allCheckBox.IsChecked = null
        };
    }

    #endregion
}