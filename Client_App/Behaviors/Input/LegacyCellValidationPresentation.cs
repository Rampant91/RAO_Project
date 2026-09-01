using System;
using System.Collections;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Client_App.Controls.DataGrid;

namespace Client_App.Behaviors.Input;

/// <summary>
/// Legacy DataGrid: красная подсветка на всю ячейку (рамка Cell), а не только на TextBox.
/// Иконка ошибки остаётся внутри поля; фон рамки перекрывает выделение до снятия ошибки.
/// </summary>
public static class LegacyCellValidationPresentation
{
    private static readonly SolidColorBrush ErrorBackground =
        new(Color.Parse("#FFFFCDD2"));

    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>("IsEnabled", typeof(LegacyCellValidationPresentation));

    private static readonly AttachedProperty<IDisposable?> HasErrorsSubscriptionProperty =
        AvaloniaProperty.RegisterAttached<Control, IDisposable?>("HasErrorsSubscription", typeof(LegacyCellValidationPresentation));

    private static readonly AttachedProperty<IDisposable?> ErrorsSubscriptionProperty =
        AvaloniaProperty.RegisterAttached<Control, IDisposable?>("ErrorsSubscription", typeof(LegacyCellValidationPresentation));

    static LegacyCellValidationPresentation()
    {
        IsEnabledProperty.Changed.AddClassHandler<Control>(OnIsEnabledChanged);
    }

    public static bool GetIsEnabled(Control element) => element.GetValue(IsEnabledProperty);

    public static void SetIsEnabled(Control element, bool value) => element.SetValue(IsEnabledProperty, value);

    private static void OnIsEnabledChanged(Control control, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is not true)
        {
            Detach(control);
            return;
        }

        control.AttachedToVisualTree -= OnAttachedToVisualTree;
        control.DetachedFromVisualTree -= OnDetachedFromVisualTree;
        control.AttachedToVisualTree += OnAttachedToVisualTree;
        control.DetachedFromVisualTree += OnDetachedFromVisualTree;

        if (control.IsAttachedToVisualTree())
        {
            Attach(control);
        }
    }

    private static void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is Control control)
        {
            Attach(control);
        }
    }

    private static void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is Control control)
        {
            DetachSubscriptions(control);
        }
    }

    private static void Detach(Control control)
    {
        control.AttachedToVisualTree -= OnAttachedToVisualTree;
        control.DetachedFromVisualTree -= OnDetachedFromVisualTree;
        DetachSubscriptions(control);
        RestoreCellBackground(control);
    }

    private static void Attach(Control control)
    {
        if (control is not TextBox)
        {
            return;
        }

        Dispatcher.UIThread.Post(() => UpdateCellBackground(control), DispatcherPriority.Loaded);
        DetachSubscriptions(control);

        control.SetValue(HasErrorsSubscriptionProperty, control
            .GetObservable(DataValidationErrors.HasErrorsProperty)
            .Subscribe(_ => Dispatcher.UIThread.Post(() => UpdateCellBackground(control), DispatcherPriority.Background)));

        control.SetValue(ErrorsSubscriptionProperty, control
            .GetObservable(DataValidationErrors.ErrorsProperty)
            .Subscribe(_ => Dispatcher.UIThread.Post(() => UpdateCellBackground(control), DispatcherPriority.Background)));
    }

    private static void DetachSubscriptions(Control control)
    {
        control.GetValue(HasErrorsSubscriptionProperty)?.Dispose();
        control.GetValue(ErrorsSubscriptionProperty)?.Dispose();
        control.SetValue(HasErrorsSubscriptionProperty, null);
        control.SetValue(ErrorsSubscriptionProperty, null);
    }

    private static void UpdateCellBackground(Control control)
    {
        var border = FindCellBorder(control);
        if (border is null)
        {
            return;
        }

        if (HasValidationErrors(control))
        {
            border.Background = ErrorBackground;
            return;
        }

        border.ClearValue(Border.BackgroundProperty);
    }

    private static void RestoreCellBackground(Control control)
    {
        var border = FindCellBorder(control);
        border?.ClearValue(Border.BackgroundProperty);
    }

    private static bool HasValidationErrors(Control control)
    {
        if (control.GetValue(DataValidationErrors.HasErrorsProperty) is true)
        {
            return true;
        }

        var errors = control.GetValue(DataValidationErrors.ErrorsProperty) as IEnumerable;
        return errors?.Cast<object?>().Any(static e => e is not null) == true;
    }

    private static Border? FindCellBorder(Control control)
    {
        var cell = control.GetVisualAncestors().OfType<Cell>().FirstOrDefault();
        return cell?.Content as Border;
    }
}
