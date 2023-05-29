using Client_App.State.Navigation;

namespace Client_App.ViewModels;

public class AnnualReportsVM : BaseVM
{
    public MainWindow2VM MainWindow2VM;
    public AnnualReportsVM(Navigator navigator, MainWindow2VM mainWindow2VM)
    {
        MainWindow2VM = mainWindow2VM;
    }
}