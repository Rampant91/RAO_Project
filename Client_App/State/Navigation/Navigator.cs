using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Client_App.Commands.SyncCommands;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.State.Navigation;

public class Navigator : INotifyPropertyChanged, INavigator
{
    #region INotifyPropertyChanged

    protected void OnPropertyChanged(string prop)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region CurrentVM

    private BaseVM _currentVM;
    public BaseVM CurrentVM
    {
        get => _currentVM;
        set
        {
            _currentVM = value;
            OnPropertyChanged(nameof(CurrentVM));
        }
    }

    #endregion

    #region FormType

    private ViewType _FormType = ViewType.Oper;
    public ViewType FormType
    {
        get => _FormType;
        set
        {
            _FormType = value;
            //new UpdateCurrentVMCommand();
            //OnPropertyChanged(nameof(IsForm1));
        }
    }

    #endregion

    #region ReportStorage

    private List<Report> _reportStorage;
    public List<Report>? ReportStorage
    {
        get => _reportStorage;
        set
        {
            if (value != null && value != _reportStorage)
            {
                _reportStorage = value;
                OnPropertyChanged(nameof(ReportStorage));
            }
        }
    }

    #endregion

    public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentVMCommand(this, ((OperReportsVM)_currentVM).MainWindow2VM);
}