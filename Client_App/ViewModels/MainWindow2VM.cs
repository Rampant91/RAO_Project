using Client_App.State.Navigation;
using System.Collections.ObjectModel;

namespace Client_App.ViewModels;

public class MainWindow2VM : BaseVM
{
    public Navigator Navigator { get; } = new();

    public MainWindow2VM()
    {
        //Navigator.CurrentVM = new OperReportsVM(Navigator, this);
        MainWindowTabs = new ObservableCollection<object>
        {
            new OperReportsVM(Navigator, this),
            new AnnualReportsVM(Navigator, this)
        };
    }

    public ObservableCollection<object> MainWindowTabs { get; }
}