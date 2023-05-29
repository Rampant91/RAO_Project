using System;
using System.Windows.Input;
using Client_App.State.Navigation;
using Client_App.ViewModels;

namespace Client_App.Commands.SyncCommands;

public class UpdateCurrentVMCommand : ICommand

{
    private Navigator _navigator;
    private MainWindow2VM _mainWindowVM;
    public UpdateCurrentVMCommand(Navigator navigator, MainWindow2VM mainWindowVM)
    {
        _navigator = navigator;
        _mainWindowVM = mainWindowVM;
    }

    public event EventHandler? CanExecuteChanged;
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (parameter is ViewType viewType)
        {
            switch (viewType)
            {
                //case ViewType.Annual:
                //    if (_navigator.CurrentVM is not AnnualReportsViewModel)
                //        _mainWindowViewModel.MainWindowName = "Аналитика отчетности RAODB v.1.0.1 Годовая отчетность";
                //    _navigator.CurrentViewModel = new AnnualReportsViewModel(_navigator, _mainWindowViewModel);
                //    break;
                case ViewType.Oper:
                    if (_navigator.CurrentVM is not OperReportsVM)
                        _navigator.CurrentVM = new OperReportsVM(_navigator, _mainWindowVM);
                    break;
            }
        }
    }
}