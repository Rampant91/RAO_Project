using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Calculator;

namespace Client_App.Views.Calculator;

public class ActivityCalculator : BaseWindow<ActivityCalculatorVM>
{
    #region Constructor

    public ActivityCalculator()
    {
        AvaloniaXamlLoader.Load(this);
    }

    #endregion
}