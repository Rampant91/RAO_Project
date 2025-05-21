using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;
using System;

namespace Client_App.Views.Messages;

/// <summary>
/// Окно, запрашивающее у пользователя начальную и конечную даты.
/// </summary>
public partial class AskDatePeriodMessageWindow : BaseWindow<AskDatePeriodMessageVM>
{
    private readonly AskDatePeriodMessageVM _askDatePeriodMessageVM;

    #region Constructor
    
    public AskDatePeriodMessageWindow()
    {
        _askDatePeriodMessageVM = new AskDatePeriodMessageVM();
        DataContext = _askDatePeriodMessageVM;
        AvaloniaXamlLoader.Load(this);
    }

    #endregion

    #region Events

    #region OnCancelButtonClicked

    /// <summary>
    /// Ивент при нажатии кнопки "Отмена".
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    private void OnCancelButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(("Отмена", DateOnly.MinValue, DateOnly.MaxValue));
    }

    #endregion

    #region OnOkButtonClicked

    /// <summary>
    /// Ивент при нажатии кнопки "Ок". Возвращает кортеж из введённой пользователем начальной/конечной даты.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnOkButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (!DateOnly.TryParse(_askDatePeriodMessageVM.InitialDate, out var initialDateOnly)) initialDateOnly = DateOnly.MinValue;
        if (!DateOnly.TryParse(_askDatePeriodMessageVM.ResidualDate, out var residualDateOnly)) residualDateOnly = DateOnly.MaxValue;

        Close(("Ок", initialDateOnly, residualDateOnly));
    }

    #endregion 

    #endregion
}