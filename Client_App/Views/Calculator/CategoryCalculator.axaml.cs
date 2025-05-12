using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Calculator;

namespace Client_App.Views.Calculator;

public class CategoryCalculator : BaseWindow<CategoryCalculatorVM>
{

    #region Constructor

    public CategoryCalculator()
    {
        AvaloniaXamlLoader.Load(this);
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
            categoryCalculatorVM.CategoryCalculation.Execute(null);
        }
    }

    private void OnClearButtonClicked(object? sender, RoutedEventArgs e)
    {
        var categoryCalculatorVM = (DataContext as CategoryCalculatorVM)!;
        categoryCalculatorVM.SelectedRadionuclids.Clear();
        categoryCalculatorVM.SelectedNuclid = null;
        categoryCalculatorVM.CategoryCalculation.Execute(null);
    }

    private void OnRemoveButtonClicked(object? sender, RoutedEventArgs e)
    {
        var categoryCalculatorVM = (DataContext as CategoryCalculatorVM)!;
        if (categoryCalculatorVM.SelectedNuclid is not null)
        {
            categoryCalculatorVM.SelectedRadionuclids.Remove(categoryCalculatorVM.SelectedNuclid);
            if (categoryCalculatorVM.SelectedRadionuclids.Count == 0) categoryCalculatorVM.SelectedNuclid = null;
            categoryCalculatorVM.CategoryCalculation.Execute(null);
        }
    }
    
    #endregion
}