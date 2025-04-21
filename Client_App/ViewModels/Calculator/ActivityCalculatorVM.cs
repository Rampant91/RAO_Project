using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands.Calculator;

namespace Client_App.ViewModels.Calculator;

public partial class ActivityCalculatorVM : BaseVM, INotifyPropertyChanged
{
    #region Properties

    public List<Radionuclid> RadionuclidsFullList;

    private ObservableCollection<Radionuclid> _radionuclids;
    public ObservableCollection<Radionuclid> Radionuclids
    {
        get => _radionuclids;
        set
        {
            if (_radionuclids != value && value != null)
            {
                _radionuclids = value;
            }
            OnPropertyChanged();
        }
    }

    private Radionuclid _selectedNuclid;
    public Radionuclid SelectedNuclid
    {
        get => _selectedNuclid;
        set
        {
            if (_selectedNuclid == value || value is null) return;
            _selectedNuclid = value;
            OnPropertyChanged();
            ActivityCalculation.Execute(this);
        }
    }

    private string _filter;
    public string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            OnPropertyChanged();
            FilterCommand?.Execute(null);
        }
    }

    private bool _isDateRange = true;
    public bool IsDateRange
    {
        get => _isDateRange;
        set
        {
            _isDateRange = value;
            OnPropertyChanged();
            ActivityCalculation.Execute(this);
        }
    }

    public static string[] TimeUnitArray { get; } = ["мин", "час", "сут", "лет"];

    private string _selectedTimeUnit = "мин";
    public string SelectedTimeUnit
    {
        get => _selectedTimeUnit;
        set
        {
            _selectedTimeUnit = value;
            OnPropertyChanged();
            ActivityCalculation.Execute(this);
        }
    }

    #endregion

    private string _initialActivity;
    public string InitialActivity
    {
        get => _initialActivity;
        set
        {
            _initialActivity = value;
            OnPropertyChanged();
            ActivityCalculation.Execute(this);
        }
    }

    private string _residualActivity;
    public string ResidualActivity
    {
        get => _residualActivity;
        set
        {
            _residualActivity = value;
            OnPropertyChanged();
        }
    }

    private string _timePeriodDouble;
    public string TimePeriodDouble
    {
        get => _timePeriodDouble;
        set
        {
            _timePeriodDouble = value;
            OnPropertyChanged();
            ActivityCalculation.Execute(this);
        }
    }

    private string _initialActivityDate;
    public string InitialActivityDate
    {
        get => _initialActivityDate;
        set
        {
            _initialActivityDate = DateTime.TryParse(value, out var dateTimeValue)
                ? dateTimeValue.ToShortDateString()
                : value;

            OnPropertyChanged();
            ActivityCalculation.Execute(this);
        }
    }

    private string _residualActivityDate = DateTime.Now.ToShortDateString();
    public string ResidualActivityDate
    {
        get => _residualActivityDate;
        set
        {
            _residualActivityDate = DateTime.TryParse(value, out var dateTimeValue) 
                ? dateTimeValue.ToShortDateString() 
                : value;
                
            OnPropertyChanged();
            ActivityCalculation.Execute(this);
        }
    }

    #region Constructor

    public ActivityCalculatorVM() { }

    public ActivityCalculatorVM(List<Radionuclid> radionuclids)
    {
        Radionuclids = new ObservableCollection<Radionuclid>(radionuclids);
        RadionuclidsFullList = [..Radionuclids];

        FilterCommand = new CalculatorFilterAsyncCommand(this);
        ActivityCalculation = new ActivityCalculationAsyncCommand(this);
    }

    #endregion

    public ICommand FilterCommand { get; set; }
    public ICommand ActivityCalculation { get; set; }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}

public class Radionuclid
{
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public required double Halflife { get; set; }
    public required string Unit { get; set; }
    public required string Code { get; set; }
}