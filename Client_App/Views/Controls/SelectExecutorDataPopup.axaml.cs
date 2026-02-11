using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Models.DTO;

namespace Client_App.Views.Controls;

public partial class SelectExecutorDataPopup : UserControl
{
    public SelectExecutorDataPopup()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        var newSelectedExecutor = (sender as ListBox).SelectedItem as ExecutorDataDTO;
        //new SwitchToSelectedReportAsyncCommand(vm.FormVM).AsyncExecute(newSelectedExecutor);
    }
}