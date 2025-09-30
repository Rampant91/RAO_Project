using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System.Linq;

namespace Client_App.Behaviors;

/// <summary>
/// Behavior to expose a bindable IsEditing flag for a DataGrid based on its edit lifecycle events.
/// Works with Avalonia 0.10.x.
/// </summary>
public class DataGridEditingStateBehavior : Behavior<DataGrid>
{
    public static readonly StyledProperty<bool> IsEditingProperty =
        AvaloniaProperty.Register<DataGridEditingStateBehavior, bool>(nameof(IsEditing));

    public bool IsEditing
    {
        get => GetValue(IsEditingProperty);
        set => SetValue(IsEditingProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null) return;

        AssociatedObject.BeginningEdit += OnBeginningEdit;
        AssociatedObject.CellEditEnded += OnCellEditEnded;
        AssociatedObject.LostFocus += OnLostFocus;
        AssociatedObject.LoadingRow += OnLoadingRow; // ensure edits end update

        // Additionally, track focus on any descendant TextBox to handle TemplateColumns
        var strategies = RoutingStrategies.Tunnel | RoutingStrategies.Bubble;
        AssociatedObject.AddHandler(InputElement.GotFocusEvent, OnGotFocus, strategies);
        AssociatedObject.AddHandler(InputElement.LostFocusEvent, OnLostFocusElement, strategies);
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.BeginningEdit -= OnBeginningEdit;
            AssociatedObject.CellEditEnded -= OnCellEditEnded;
            AssociatedObject.LostFocus -= OnLostFocus;
            AssociatedObject.LoadingRow -= OnLoadingRow;
            AssociatedObject.RemoveHandler(InputElement.GotFocusEvent, OnGotFocus);
            AssociatedObject.RemoveHandler(InputElement.LostFocusEvent, OnLostFocusElement);
        }
        base.OnDetaching();
    }

    private void OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e)
    {
        IsEditing = true;
    }

    private void OnCellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
    {
        // End of a cell edit (Commit or Cancel)
        IsEditing = false;
    }

    private void OnLostFocus(object? sender, RoutedEventArgs e)
    {
        // When the grid loses focus, ensure we don't stay stuck in editing state
        IsEditing = false;
    }

    private void OnLoadingRow(object? sender, DataGridRowEventArgs e)
    {
        // As rows get (re)loaded, ensure state remains consistent
        if (AssociatedObject?.IsKeyboardFocusWithin != true)
            IsEditing = false;
    }

    private void OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (AssociatedObject is null) return;

        if (e.Source is IVisual v)
        {
            var tb = v.GetSelfAndVisualAncestors().OfType<TextBox>().FirstOrDefault();
            if (tb != null)
            {
                IsEditing = true;
            }
        }
    }

    private void OnLostFocusElement(object? sender, RoutedEventArgs e)
    {
        if (AssociatedObject == null)
            return;

        // If focus moved outside the DataGrid or no TextBox within has focus, reset
        if (!AssociatedObject.IsKeyboardFocusWithin)
        {
            IsEditing = false;
            return;
        }

        // If still inside, check if any TextBox retains focus; if not, assume edit ended
        if (e.Source is IVisual v)
        {
            // Defer to DataGrid events when possible, but ensure fallback
            // Keep true if another TextBox is focused; otherwise set false
            var focused = FocusManager.Instance?.Current;
            if (focused is IVisual focusedVisual && AssociatedObject is IVisual gridVisual)
            {
                // Keep editing true only if focus is still inside the grid and on a TextBox
                var isTextBox = focusedVisual.GetSelfAndVisualAncestors().OfType<TextBox>().Any();
                IsEditing = gridVisual.IsVisualAncestorOf(focusedVisual) && isTextBox;
            }
            else
            {
                IsEditing = false;
            }
        }
    }
}
