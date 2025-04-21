using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands.Calculator;
using ExtendedNumerics;

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
            OnPropertyChanged(nameof(ResidualActivity));
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
            OnPropertyChanged(nameof(ResidualActivity));
        }
    }

    #endregion

    private string _initialActivity;
    public string InitialActivity
    {
        get => _initialActivity;
        set
        {
            _initialActivity = ToExponentialString(value);
            OnPropertyChanged(nameof(ResidualActivity));
        }
    }

    private string _residualActivity;
    public string ResidualActivity
    {
        get
        {
            if (double.TryParse(TimePeriodDouble, out var timePeriodDoubleValue) 
                && double.TryParse(InitialActivity, out var initialActivityDoubleValue))
            {
                
                var cycles = timePeriodDoubleValue / SelectedNuclid.Halflife;
                var activity = initialActivityDoubleValue / BigDecimal.Pow(2, cycles);
                var decimalValue = GetActivityInTargetType(activity, SelectedNuclid.Unit, SelectedTimeUnit);
                return ToExponentialString(decimalValue);
            }
            return _residualActivity;
        }
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
            OnPropertyChanged(nameof(ResidualActivity));
        }
    }

    private DateOnly _initialActivityDate = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly InitialActivityDate
    {
        get => _initialActivityDate;
        set
        {
            _initialActivityDate = value;
            OnPropertyChanged();
        }
    }

    private DateOnly _residualActivityDate = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly ResidualActivityDate
    {
        get => _residualActivityDate;
        set
        {
            _residualActivityDate = value;
            OnPropertyChanged();
        }
    }

    private static BigDecimal GetActivityInTargetType(BigDecimal value, string currentUnit, string targetUnit)
    {
        switch (currentUnit)
        {
            case "мин":
            {
                switch (targetUnit)
                {
                    case "мин": return value;
                    case "час": return value / 60;
                    case "сут": return value / 60 / 24;
                    case "лет": return value / 60 / 24 / 365;
                }
                break;
            }
            case "час":
            {
                switch (targetUnit)
                {
                    case "мин": return value * 60;
                    case "час": return value;
                    case "сут": return value / 24;
                    case "лет": return value / 24 / 365;
                }
                break;
            }
            case "сут":
            {
                switch (targetUnit)
                {
                    case "мин": return value * 60 * 24;
                    case "час": return value * 24;
                    case "сут": return value;
                    case "лет": return value / 365;
                }
                break;
            }
            case "лет":
            {
                switch (targetUnit)
                {
                    case "мин": return value * 60 * 24 * 365;
                    case "час": return value * 24 * 365;
                    case "сут": return value * 365;
                    case "лет": return value;
                }
                break;
            }
        }
        return value;
    }

    #region Constructor

    public ActivityCalculatorVM() { }

    public ActivityCalculatorVM(List<Radionuclid> radionuclids)
    {
        Radionuclids = new ObservableCollection<Radionuclid>(radionuclids);
        RadionuclidsFullList = [..Radionuclids];

        FilterCommand = new CalculatorFilterAsyncCommand(this);
    }

    #endregion

    public ICommand? FilterCommand { get; set; }

    private protected static string ToExponentialString(object? value)
    {
        var tmp = (value?.ToString() ?? string.Empty)
            .Trim()
            .ToLower()
            .Replace('е', 'e');
        tmp = ReplaceDashes(tmp);
        if (tmp != "прим.")
        {
            tmp = tmp.Replace('.', ',');
        }
        if (tmp is "прим." or "-")
        {
            return tmp;
        }
        var doubleStartsWithBrackets = false;
        if (tmp.StartsWith('(') && tmp.EndsWith(')'))
        {
            doubleStartsWithBrackets = true;
            tmp = tmp
                .TrimStart('(')
                .TrimEnd(')');
        }
        var tmpNumWithoutSign = tmp.StartsWith('+') || tmp.StartsWith('-')
            ? tmp[1..]
            : tmp;
        var sign = tmp.StartsWith('-')
            ? "-"
            : string.Empty;
        if (!tmp.Contains('e')
            && tmpNumWithoutSign.Count(x => x is '+' or '-') == 1)
        {
            tmp = sign + tmpNumWithoutSign.Replace("+", "e+").Replace("-", "e-");
        }
        if (double.TryParse(tmp,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.###e+00}";
        }
        return doubleStartsWithBrackets
            ? $"({tmp})"
            : tmp;
    }

    private protected static string ReplaceDashes(string value) =>
        value switch
        {
            null => string.Empty,
            _ => DashesRegex().Replace(value, "-")
        };

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    protected static partial Regex DashesRegex();

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