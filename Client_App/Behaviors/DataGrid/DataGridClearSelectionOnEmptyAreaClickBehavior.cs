using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.MainWindowTabs;

namespace Client_App.Behaviors.DataGrid;

/// <summary>
/// \u0421\u0431\u0440\u0430\u0441\u044b\u0432\u0430\u0435\u0442 \u0432\u044b\u0434\u0435\u043b\u0435\u043d\u0438\u0435 DataGrid \u043f\u0440\u0438 \u043a\u043b\u0438\u043a\u0435 \u0432\u043d\u0443\u0442\u0440\u0438 \u0433\u0440\u0438\u0434\u0430, \u043d\u043e \u043c\u0438\u043c\u043e \u0441\u0442\u0440\u043e\u043a\u0438 \u0434\u0430\u043d\u043d\u044b\u0445.
/// </summary>
public class DataGridClearSelectionOnEmptyAreaClickBehavior : Behavior<AvaloniaDataGrid>
{
    private bool _clearOnRelease;

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null) return;

        AttachEventHandlers();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        DetachEventHandlers();
    }

    private void AttachEventHandlers()
    {
        if (AssociatedObject is null) return;

        AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, OnDataGridPointerPressed);
        AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, OnDataGridPointerReleased);

        // Avalonia 11 DataGrid often marks PointerPressed as Handled — catch handled too.
        AssociatedObject.AddHandler(
            InputElement.PointerPressedEvent,
            OnDataGridPointerPressed,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
            handledEventsToo: true);

        // Selection may be applied after press; clear again on release if needed.
        AssociatedObject.AddHandler(
            InputElement.PointerReleasedEvent,
            OnDataGridPointerReleased,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
            handledEventsToo: true);
    }

    private void DetachEventHandlers()
    {
        if (AssociatedObject is null) return;

        AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, OnDataGridPointerPressed);
        AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, OnDataGridPointerReleased);
    }

    private void OnDataGridPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _clearOnRelease = false;

        if (AssociatedObject is null) return;
        if (!e.GetCurrentPoint(AssociatedObject).Properties.IsLeftButtonPressed) return;

        var source = e.Source as Visual;
        if (IsClickOnInteractiveChrome(source))
            return;

        if (IsClickOnDataRow(source, e))
            return;

        _clearOnRelease = true;
        ClearSelection();
    }

    private void OnDataGridPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!_clearOnRelease) return;
        _clearOnRelease = false;

        if (AssociatedObject is null) return;
        if (e.InitialPressMouseButton != MouseButton.Left) return;

        ClearSelection();
    }

    private void ClearSelection()
    {
        if (AssociatedObject is null) return;

        if (AssociatedObject.SelectionMode == DataGridSelectionMode.Extended
            && AssociatedObject.SelectedItems is { IsReadOnly: false } list)
        {
            list.Clear();
        }

        AssociatedObject.SelectedItem = null;
        AssociatedObject.SelectedIndex = -1;

        ClearSelectionInViewModel();
    }

    /// <summary>True when the press lands on a realized row that has an item.</summary>
    private bool IsClickOnDataRow(Visual? source, PointerEventArgs e)
    {
        for (var visual = source; visual != null; visual = visual.GetVisualParent() as Visual)
        {
            if (visual is DataGridRow row)
                return row.DataContext != null;
        }

        // Avalonia 11: empty area below rows may not report a row in Source — also test by position.
        if (AssociatedObject is null) return false;

        var posInGrid = e.GetPosition(AssociatedObject);
        foreach (var row in AssociatedObject.GetVisualDescendants().OfType<DataGridRow>())
        {
            if (row.DataContext is null) continue;

            var rowBounds = row.Bounds;
            var topLeft = row.TranslatePoint(new Point(0, 0), AssociatedObject);
            if (topLeft is null) continue;

            var hitBounds = new Rect(topLeft.Value, rowBounds.Size);
            if (hitBounds.Contains(posInGrid))
                return true;
        }

        return false;
    }

    /// <summary>Do not clear when clicking header / scrollbar / column resize thumb.</summary>
    private static bool IsClickOnInteractiveChrome(Visual? source)
    {
        while (source != null)
        {
            switch (source)
            {
                case DataGridColumnHeader:
                case ScrollBar:
                    return true;
                case Thumb thumb:
                    // Only treat resize/scrollbar thumbs as chrome, not unrelated thumbs.
                    if (thumb.FindAncestorOfType<DataGridColumnHeader>() != null
                        || thumb.FindAncestorOfType<ScrollBar>() != null)
                        return true;
                    break;
            }

            source = source.GetVisualParent() as Visual;
        }

        return false;
    }

    private void ClearSelectionInViewModel()
    {
        if (AssociatedObject?.DataContext is FormsTabControlBaseVM vm)
        {
            if (AssociatedObject.Name == "ReportsDataGrid")
                vm.SelectedReports = null;
            else if (AssociatedObject.Name == "ReportDataGrid")
                vm.SelectedReport = null;
        }
        else if (AssociatedObject?.DataContext is BaseFormVM formVm)
        {
            if (AssociatedObject.Name == "dataGrid")
            {
                formVm.SelectedForm = null;
                formVm.SelectedForms.Clear();
            }
            else if (AssociatedObject.Name == "notesDataGrid" || AssociatedObject.Name == "notesTable")
            {
                formVm.SelectedNote = null;
                formVm.SelectedNotes.Clear();
            }
        }
    }
}
