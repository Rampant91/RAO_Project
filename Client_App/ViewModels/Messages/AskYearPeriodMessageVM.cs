using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class AskYearPeriodMessageVM : INotifyPropertyChanged
{
    #region Properties

    private string _initialYear = string.Empty;
    public string InitialYear
    {
        get => _initialYear;
        set
        {
            _initialYear = int.TryParse(value, out var intValue)
                ? intValue.ToString()
                : "0000";

            OnPropertyChanged();
        }
    }

    private string _residualYear = DateTime.Now.Year.ToString();
    public string ResidualYear
    {
        get => _residualYear;
        set
        {
            _residualYear = int.TryParse(value, out var intValue)
                ? intValue.ToString()
                : "0000";

            OnPropertyChanged();
        }
    }

    #endregion

    #region Constructor

    public AskYearPeriodMessageVM() { }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}