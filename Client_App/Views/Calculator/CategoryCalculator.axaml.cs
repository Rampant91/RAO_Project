using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Calculator;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Models.DTO;

namespace Client_App.Views.Calculator;

public class CategoryCalculator : BaseWindow<CategoryCalculatorVM>
{
    private readonly CategoryCalculatorVM _vm = null!;

    #region Constructor

    public CategoryCalculator()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public CategoryCalculator(CategoryCalculatorVM vm)
    {
        AvaloniaXamlLoader.Load(this);
        _vm = vm;
    }

    #endregion

    #region Events

    private void OnAddButtonClicked(object? sender, RoutedEventArgs e)
    {
        var categoryCalculatorVM = (DataContext as CategoryCalculatorVM)!;
        if (categoryCalculatorVM.SelectedDictionaryNuclid is not null 
            && !categoryCalculatorVM.SelectedRadionuclids.Contains(categoryCalculatorVM.SelectedDictionaryNuclid))
        {
            categoryCalculatorVM.SelectedRadionuclids?.Add(categoryCalculatorVM.SelectedDictionaryNuclid);

            categoryCalculatorVM.SelectedRadionuclids 
                = new ObservableCollection<CalculatorRadionuclidDTO>(categoryCalculatorVM.SelectedRadionuclids
                    .OrderBy(x => x.Name));
        }
    }

    private void OnCalculateButtonClicked(object? sender, RoutedEventArgs e)
    {
        _vm.CategoryCalculation.Execute(null);
    }

    private void OnClearButtonClicked(object? sender, RoutedEventArgs e)
    {
        var categoryCalculatorVM = (DataContext as CategoryCalculatorVM)!;

        if (categoryCalculatorVM.RadionuclidDictionary != null)
        {
            foreach (var nuclid in categoryCalculatorVM.SelectedRadionuclids)
            {
                categoryCalculatorVM.RadionuclidDictionary
                    .First(x => x.Name == nuclid.Name)
                    .Activity = string.Empty;
            }
        }
        categoryCalculatorVM.SelectedRadionuclids.Clear();
        categoryCalculatorVM.SelectedNuclid = null;
    }

    private void OnCloseButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnRemoveButtonClicked(object? sender, RoutedEventArgs e)
    {
        var categoryCalculatorVM = (DataContext as CategoryCalculatorVM)!;
        if (categoryCalculatorVM.SelectedNuclid is not null && categoryCalculatorVM.RadionuclidDictionary is not null)
        {
            categoryCalculatorVM.RadionuclidDictionary
                .First(x => x.Name == categoryCalculatorVM.SelectedNuclid.Name)
                .Activity = string.Empty;
            categoryCalculatorVM.SelectedRadionuclids.Remove(categoryCalculatorVM.SelectedNuclid);
            
            if (categoryCalculatorVM.SelectedRadionuclids.Count == 0) categoryCalculatorVM.SelectedNuclid = null;
        }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}