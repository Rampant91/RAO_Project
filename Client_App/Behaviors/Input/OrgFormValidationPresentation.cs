using System;
using System.Collections;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Client_App.Behaviors.Input;

/// <summary>
/// Org form fields: error text in tooltip on whole control hover.
/// Icon placement stays in XAML (ContentControl of DataValidationErrors) so the field itself stays stretchable and editable.
/// </summary>
public static class OrgFormValidationPresentation
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>("IsEnabled", typeof(OrgFormValidationPresentation));

    private static readonly AttachedProperty<IDisposable?> HasErrorsSubscriptionProperty =
        AvaloniaProperty.RegisterAttached<Control, IDisposable?>("HasErrorsSubscription", typeof(OrgFormValidationPresentation));

    private static readonly AttachedProperty<IDisposable?> ErrorsSubscriptionProperty =
        AvaloniaProperty.RegisterAttached<Control, IDisposable?>("ErrorsSubscription", typeof(OrgFormValidationPresentation));

    static OrgFormValidationPresentation()
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
        ToolTip.SetTip(control, null);
    }

    private static void Attach(Control control)
    {
        if (control is not TextBox and not AutoCompleteBox)
        {
            return;
        }

        Dispatcher.UIThread.Post(() => UpdateToolTip(control), DispatcherPriority.Loaded);
        DetachSubscriptions(control);

        control.SetValue(HasErrorsSubscriptionProperty, control
            .GetObservable(DataValidationErrors.HasErrorsProperty)
            .Subscribe(_ => Dispatcher.UIThread.Post(() => UpdateToolTip(control), DispatcherPriority.Background)));

        control.SetValue(ErrorsSubscriptionProperty, control
            .GetObservable(DataValidationErrors.ErrorsProperty)
            .Subscribe(_ => Dispatcher.UIThread.Post(() => UpdateToolTip(control), DispatcherPriority.Background)));
    }

    private static void DetachSubscriptions(Control control)
    {
        control.GetValue(HasErrorsSubscriptionProperty)?.Dispose();
        control.GetValue(ErrorsSubscriptionProperty)?.Dispose();
        control.SetValue(HasErrorsSubscriptionProperty, null);
        control.SetValue(ErrorsSubscriptionProperty, null);
    }

    private static void UpdateToolTip(Control control)
    {
        var errors = control.GetValue(DataValidationErrors.ErrorsProperty) as IEnumerable;
        var messages = errors?
            .Cast<object?>()
            .Select(ToErrorMessage)
            .Where(static message => !string.IsNullOrWhiteSpace(message))
            .ToArray() ?? Array.Empty<string>();

        ToolTip.SetTip(control, messages.Length == 0 ? null : string.Join(Environment.NewLine, messages));
    }

    private static string? ToErrorMessage(object? error) =>
        error switch
        {
            null => null,
            Exception exception => exception.Message,
            _ => error.ToString()
        };
}
