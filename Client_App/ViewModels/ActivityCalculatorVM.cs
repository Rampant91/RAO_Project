using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels;

public class ActivityCalculatorVM : INotifyPropertyChanged
{
    #region Properties

    private string _selectedNuclid;
    public string SelectedNuclid
    {
        get => _selectedNuclid;
        set
        {
            if (_selectedNuclid == value) return;
            _selectedNuclid = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Constructor
    
    public ActivityCalculatorVM(string selectedNuclid)
    {
        _selectedNuclid = selectedNuclid;
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}