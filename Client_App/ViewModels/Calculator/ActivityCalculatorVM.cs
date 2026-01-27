using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands.Calculator;
using Models.DTO;

namespace Client_App.ViewModels.Calculator;

public class ActivityCalculatorVM : BaseCalculatorVM
{
    #region Properties

    private CalculatorRadionuclidDTO _selectedDictionaryNuclid;
    public CalculatorRadionuclidDTO SelectedDictionaryNuclid
    {
        get => _selectedDictionaryNuclid;
        set
        {
            if (_selectedDictionaryNuclid == value || value is null) return;
            _selectedDictionaryNuclid = value;
            OnPropertyChanged();
            ActivityCalculation.Execute(this);
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

    private bool _isDateRangeTextVisible;
    public bool IsDateRangeTextVisible
    {
        get => _isDateRangeTextVisible;
        set
        {
            _isDateRangeTextVisible = value;
            OnPropertyChanged();
        }
    }

    public string[] TimeUnitArray { get; } = ["мин", "час", "сут", "лет"];

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

    #endregion

    #region Constructor

    public ActivityCalculatorVM() { }

    public ActivityCalculatorVM(List<CalculatorRadionuclidDTO> radionuclids)
    {
        RadionuclidDictionary = new ObservableCollection<CalculatorRadionuclidDTO>(radionuclids);
        RadionuclidsFullList = [.. RadionuclidDictionary];

        FilterCommand = new CalculatorFilterAsyncCommand(this);
        ActivityCalculation = new ActivityCalculationAsyncCommand(this);
    }

    #endregion

    #region Commands

    private ICommand ActivityCalculation { get; set; } 
    
    #endregion
}