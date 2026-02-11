using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public partial class AskYearPeriodMessageWindow : BaseWindow<AskYearPeriodMessageVM>
{
    private readonly AskYearPeriodMessageVM _askYearPeriodMessageVM;

    public AskYearPeriodMessageWindow()
    {
        _askYearPeriodMessageVM = new AskYearPeriodMessageVM();
        DataContext = _askYearPeriodMessageVM;
        AvaloniaXamlLoader.Load(this);
    }

    private void OnCancelButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(("Отмена", 0, 0));
    }

    private void OnOkButtonClicked(object? sender, RoutedEventArgs e)
    {
        var minYear = 0;
        var maxYear = 9999;
        if (int.TryParse(_askYearPeriodMessageVM.InitialYear, out var minYearParse) && minYearParse.ToString().Length == 4)
        {
            minYear = minYearParse;
        }
        if (int.TryParse(_askYearPeriodMessageVM.ResidualYear, out var maxYearParse) && maxYearParse.ToString().Length == 4)
        {
            maxYear = maxYearParse;
        }
        if (minYear > maxYear) Close(("Ок", 0, 9999));

        Close(("Ок", minYear, maxYear));
    }
}