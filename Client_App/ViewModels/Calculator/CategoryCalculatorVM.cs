using Client_App.Commands.AsyncCommands.Calculator;
using Models.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client_App.ViewModels.Calculator;

public class CategoryCalculatorVM : BaseCalculatorVM
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
            _activity = value;
            OnPropertyChanged();
            CategoryCalculation.Execute(null);
        }
    }

    private string _category;
    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged();
        }
    }

    private string _quantity = "1";
    public string Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            OnPropertyChanged();
            CategoryCalculation.Execute(null);
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