using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
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
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region CurrentViewModel

    private BaseVM _currentViewModel;
    public BaseVM CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
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

    public ICommand UpdateCurrentViewModelCommand => null;//new UpdateCurrentViewModelCommand(this, ((OperReportsViewModel)_currentViewModel)._mainWindowViewModel);
}