using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;
using System;

namespace Client_App.Views.Messages;

public partial class AskDatePeriodMessageWindow : BaseWindow<AskDatePeriodMessageVM>
{
    private readonly AskDatePeriodMessageVM _askDatePeriodMessageVM;

    public AskDatePeriodMessageWindow()
    {
        _askDatePeriodMessageVM = new AskDatePeriodMessageVM();
        DataContext = _askDatePeriodMessageVM;
        AvaloniaXamlLoader.Load(this);
    }

    private void OnCancelButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(("Отмена", DateOnly.MinValue, DateOnly.MaxValue));
    }

    private void OnOkButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (!DateOnly.TryParse(_askDatePeriodMessageVM.InitialDate, out var initialDateOnly)) initialDateOnly = DateOnly.MinValue;
        if (!DateOnly.TryParse(_askDatePeriodMessageVM.ResidualDate, out var residualDateOnly)) residualDateOnly = DateOnly.MaxValue;

        Close(("Ок", initialDateOnly, residualDateOnly));
    }
}