using Client_App.State.Navigation;

namespace Client_App.ViewModels;

public class OperReportsVM : BaseVM
{
    public MainWindow2VM MainWindow2VM;

    public OperReportsVM(Navigator navigator, MainWindow2VM mainWindow2VM)
    {
        MainWindow2VM = mainWindow2VM;
    }
}