using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Calculator;

namespace Client_App.Views.Calculator;

public class ActivityCalculator : BaseWindow<ActivityCalculatorVM>
{
    private ActivityCalculatorVM _vm = null!;

    #region Constructor

    public ActivityCalculator()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ActivityCalculator(ActivityCalculatorVM vm)
    {
        AvaloniaXamlLoader.Load(this);
        _vm = vm;
    }

    #endregion
}