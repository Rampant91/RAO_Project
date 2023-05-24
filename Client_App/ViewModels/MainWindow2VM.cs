using Client_App.State.Navigation;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace Client_App.ViewModels;

public class MainWindow2VM : BaseVM
{
    public Navigator Navigator { get; } = new();

    public MainWindow2VM()
    {
        Navigator.CurrentViewModel = new OperReportsVM(Navigator, this);
    }
}