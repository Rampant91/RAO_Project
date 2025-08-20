using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Calculator;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Client_App.ViewModels.Calculator;

public partial class CategoryCalculatorVM : BaseCalculatorVM
{
    #region Properties

    private ObservableCollection<CalculatorRadionuclidDTO> _selectedRadionuclids = [];
    public ObservableCollection<CalculatorRadionuclidDTO> SelectedRadionuclids
    {
        get => _selectedRadionuclids;
        set
        {
            if (_selectedRadionuclids == value) return;
            _selectedRadionuclids = value;
            OnPropertyChanged();
        }
    }

    private CalculatorRadionuclidDTO? _selectedDictionaryNuclid;
    public CalculatorRadionuclidDTO? SelectedDictionaryNuclid
    {
        get => _selectedDictionaryNuclid;
        set
        {
            if (_selectedDictionaryNuclid == value) return;
            _selectedDictionaryNuclid = value;
            OnPropertyChanged();
        }
    }

    private CalculatorRadionuclidDTO? _selectedNuclid;
    public CalculatorRadionuclidDTO? SelectedNuclid
    {
        get => _selectedNuclid;
        set
        {
            if (_selectedNuclid == value) return;
            _selectedNuclid = value;
            OnPropertyChanged();
        }
    }

    private string _activity;
    public string Activity
    {
        get => _activity;
        set
        {
            if (_activity != value)
            {
                _activity = value;
                OnPropertyChanged();
            }
        }
    }

    private string _activityToNormalizingD;
    public string ActivityToNormalizingD
    {
        get => _activityToNormalizingD;
        set
        {
            if (_activityToNormalizingD != value)
            {
                _activityToNormalizingD = value;
                OnPropertyChanged();
            }
        }
    }

    private string _category;
    public string Category
    {
        get => _category;
        set
        {
            if (_category != value)
            {
                _category = value;
                OnPropertyChanged();
            }
        }
    }

    private string _categoryText;
    public string CategoryText
    {
        get => _categoryText;
        set
        {
            if (_categoryText != value)
            {
                _categoryText = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isHeaderExpanded = true;
    public bool IsHeaderExpanded
    {
        get => _isHeaderExpanded;
        set
        {
            _isHeaderExpanded = value;
            OnPropertyChanged();
        }
    }

    private bool _isSingleActivity = true;
    public bool IsSingleActivity
    {
        get => _isSingleActivity;
        set
        {
            _isSingleActivity = value;
            OnPropertyChanged();
        }
    }

    private string _quantity = "1";
    public string Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region Constructor

    public CategoryCalculatorVM() { }

    public CategoryCalculatorVM(List<CalculatorRadionuclidDTO> radionuclids)
    {
        RadionuclidDictionary = new ObservableCollection<CalculatorRadionuclidDTO>(radionuclids);
        RadionuclidsFullList = [.. RadionuclidDictionary];

        FilterCommand = new CalculatorFilterAsyncCommand(this);
        CategoryCalculation = new CategoryCalculationAsyncCommand(this);
    }

    #endregion

    #region Commands

    public ICommand CategoryCalculation { get; set; }

    #endregion

    #region ExponentialString

    private protected static string ExponentialString(string value)
    {
        var tmp = (value ?? string.Empty)
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
            tmp = $"{doubleValue:0.######################################################e+00}";
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

    #endregion

    #region Regex

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    protected static partial Regex DashesRegex();

    #endregion
}