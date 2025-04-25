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
}