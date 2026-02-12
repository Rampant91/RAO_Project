using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views.MainWindowTabs;

public partial class Forms1TabControl : UserControl
{
    public Forms1TabControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void ImportRaodb_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is ViewModels.MainWindowTabs.Forms1TabControlVM vm)
        {
            vm.MainWindowVM.ImportRaodb.Execute("Selected");
        }
    }

    private void ImportExcel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is ViewModels.MainWindowTabs.Forms1TabControlVM vm)
        {
            vm.MainWindowVM.ImportExcel.Execute("Selected");
        }
    }

    private void ExportExcelAnalysis_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is ViewModels.MainWindowTabs.Forms1TabControlVM vm)
        {
            vm.MainWindowVM.ExcelExportFormAnalysis.Execute(vm.SelectedReport);
        }
    }

    private void ExportExcelPrint_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is ViewModels.MainWindowTabs.Forms1TabControlVM vm)
        {
            vm.MainWindowVM.ExcelExportFormPrint.Execute(vm.SelectedReport);
        }
    }
}