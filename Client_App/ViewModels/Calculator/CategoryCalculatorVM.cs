using Client_App.Commands.AsyncCommands.Calculator;
using Models.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
}