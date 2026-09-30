using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors.DataGrid;

/// <summary>
/// Открывает <see cref="Control.ContextMenu"/> DataGrid по ПКМ по пустой области
/// (и по строкам, если стандартный ContextRequested не сработал).
/// В Avalonia без Background пустая область не участвует в hit-test — меню видно только на заголовках.
/// </summary>
public class DataGridEnsureContextMenuBehavior : Behavior<AvaloniaDataGrid>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null) return;

        EnsureBackgroundForHitTest();

        AssociatedObject.AddHandler(
            InputElement.PointerReleasedEvent,
            OnPointerReleased,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
            handledEventsToo: true);
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
            AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, OnPointerReleased);

        base.OnDetaching();
    }

    private void EnsureBackgroundForHitTest()
    {
        if (AssociatedObject is null) return;

        if (AssociatedObject.Background is null)
            AssociatedObject.Background = Brushes.White;
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (AssociatedObject?.ContextMenu is not { } menu) return;
        if (e.InitialPressMouseButton != MouseButton.Right) return;
        if (menu.IsOpen) return;

        var source = e.Source as Visual;
        if (IsScrollBar(source)) return;

        if (source is not null)
        {
            var ownerGrid = source.FindAncestorOfType<AvaloniaDataGrid>(includeSelf: true);
            if (ownerGrid is not null && !ReferenceEquals(ownerGrid, AssociatedObject))
                return;
        }

        // Header already opens via default path; still safe if IsOpen is false.
        menu.Open(AssociatedObject);
        e.Handled = true;
    }

    private static bool IsScrollBar(Visual? source)
    {
        while (source != null)
        {
            if (source is ScrollBar)
                return true;
            source = source.GetVisualParent() as Visual;
        }

        return false;
    }
}
