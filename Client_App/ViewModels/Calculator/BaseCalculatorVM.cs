using Models.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client_App.ViewModels.Calculator;

public class BaseCalculatorVM : BaseVM, INotifyPropertyChanged
{
    #region Properties

    public List<CalculatorRadionuclidDTO>? RadionuclidsFullList;

    private ObservableCollection<CalculatorRadionuclidDTO>? _radionuclidDictionary;
    public ObservableCollection<CalculatorRadionuclidDTO>? RadionuclidDictionary
    {
        get => _radionuclidDictionary;
        set
        {
            if (_radionuclidDictionary != value && value != null)
            {
                _radionuclidDictionary = value;
            }
            OnPropertyChanged();
        }
    }

    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            OnPropertyChanged();
            FilterCommand?.Execute(null);
        }
    }

    #endregion

    #region Commands

    protected ICommand? FilterCommand { get; set; }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}