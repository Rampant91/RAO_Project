using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class AskDatePeriodMessageVM : INotifyPropertyChanged
{
    #region Properties
    
    private string _initialDate = string.Empty;
    public string InitialDate
    {
        get => _initialDate;
        set
        {
            _initialDate = DateTime.TryParse(value, out var dateTimeValue)
                ? dateTimeValue.ToShortDateString()
                : value;

            OnPropertyChanged();
        }
    }

    private string _residualDate =  string.Empty;        //DateOnly.FromDateTime(DateTime.Now).ToShortDateString();
    public string ResidualDate
    {
        get => _residualDate;
        set
        {
            _residualDate = DateTime.TryParse(value, out var dateTimeValue)
                ? dateTimeValue.ToShortDateString()
                : value;

            OnPropertyChanged();
        }
    }

    #endregion

    #region Constructor

    public AskDatePeriodMessageVM() {}

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}