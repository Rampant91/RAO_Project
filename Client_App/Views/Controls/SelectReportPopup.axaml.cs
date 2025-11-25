using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.Commands.AsyncCommands.SwitchReport;
using Client_App.ViewModels.Controls;
using Models.Collections;

namespace Client_App.Views.Controls;

public partial class SelectReportPopup : UserControl
{
    SelectReportPopupVM vm => DataContext as SelectReportPopupVM;
    public SelectReportPopup()
    {
        InitializeComponent();

    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        
    }
    public void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        var newSelectedReport = (sender as ListBox).SelectedItem as Report; 
        new SwitchToSelectedReportAsyncCommand(vm.FormVM).AsyncExecute(newSelectedReport);
    }
}