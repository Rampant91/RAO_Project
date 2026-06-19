using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors;

/// <summary>
/// Сбрасывает выделение DataGrid при клике вне грида (в пределах окна).
/// </summary>
public class DataGridClearSelectionOnOutsideClickBehavior : Behavior<DataGrid>
{
    private Window? _parentWindow;
    private bool _isAttached;

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null) return;

        AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is not null)
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;

        DetachEventHandlers();
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _parentWindow = AssociatedObject?.GetVisualRoot() as Window;

        if (_parentWindow is not null && !_isAttached)
        {
            AttachEventHandlers();
            _isAttached = true;
        }
    }

    private void AttachEventHandlers()
    {
        if (_parentWindow is not null)
            _parentWindow.PointerPressed += OnWindowPointerPressed;
    }

    private void DetachEventHandlers()
    {
        if (_parentWindow is not null)
        {
            _parentWindow.PointerPressed -= OnWindowPointerPressed;
            _isAttached = false;
        }
    }

    private void OnWindowPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (AssociatedObject is null) return;

        var source = e.Source as Control;
        if (!IsSourceInsideDataGrid(source))
            AssociatedObject.SelectedItem = null;
    }

    private bool IsSourceInsideDataGrid(Control? source)
    {
        while (source != null)
        {
            if (source == AssociatedObject)
                return true;

            source = source.Parent as Control;
        }

        return false;
    }
}
