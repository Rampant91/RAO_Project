using Avalonia;
using Avalonia.Controls;
using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Client_App.Controls;
using AppDropDownButton = Client_App.Controls.DropDownButton;
using System;
using System.Linq;

namespace Client_App.Behaviors.DataGrid;

/// <summary>
/// Behavior для DataGrid: выделение строк перетаскиванием и фокус TextBox
/// без перехвата кликов по кнопкам в ячейках (календарь, список, +/- и т.п.).
/// </summary>
public class DataGridPointerBehavior : Behavior<AvaloniaDataGrid>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null || AssociatedObject.Classes.Contains("notesTable"))
        {
            return;
        }

        AssociatedObject.CellPointerPressed += DataGrid_PointerPressed;
        AssociatedObject.PointerMoved += DataGrid_PointerMoved;
        AssociatedObject.PointerReleased += DataGrid_PointerReleased;
        AssociatedObject.PointerCaptureLost += DataGrid_PointerCaptureLost;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.CellPointerPressed -= DataGrid_PointerPressed;
            AssociatedObject.PointerMoved -= DataGrid_PointerMoved;
            AssociatedObject.PointerReleased -= DataGrid_PointerReleased;
            AssociatedObject.PointerCaptureLost -= DataGrid_PointerCaptureLost;
        }

        base.OnDetaching();
    }

    private void DataGrid_PointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        DragSelection_PointerPressed(sender, e);
        TextBoxFocus_PointerPressed(sender, e);
    }

    private void DataGrid_PointerMoved(object sender, PointerEventArgs e)
    {
        DragSelection_PointerMoved(sender, e);
        TextBoxFocus_PointerMoved(sender, e);
    }

    private void DataGrid_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        DragSelection_PointerReleased(sender, e);
        TextBoxFocus_PointerReleased(sender, e);
    }

    private void DataGrid_PointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
    {
        DragSelection_PointerCaptureLost(sender, e);
        TextBoxFocus_PointerCaptureLost(sender, e);
    }

    // Выделение нескольких строк с зажатой ЛКМ.
    #region DragSelection

    private bool _isSelecting;
    private object? _firstSelectedItem;
    private object? _lastSelectedItem;

    private void DragSelection_PointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        var point = e.PointerPressedEventArgs.GetCurrentPoint(AssociatedObject);
        if (!point.Properties.IsLeftButtonPressed)
            return;

        var hit = AssociatedObject?.GetVisualAt(point.Position);
        if (IsInteractiveControl(hit))
            return;

        _isSelecting = true;

        if (e.PointerPressedEventArgs.KeyModifiers != KeyModifiers.Shift)
            AssociatedObject!.SelectedItems.Clear();

        e.PointerPressedEventArgs.Pointer.Capture(AssociatedObject!);

        var row = GetRowAtPoint(point.Position);
        if (row == null)
            return;

        var item = row.DataContext;
        _firstSelectedItem ??= item;
        _lastSelectedItem = item;
        AssociatedObject!.SelectedItems.Add(item);
    }

    private void DragSelection_PointerMoved(object sender, PointerEventArgs e)
    {
        if (!_isSelecting)
            return;

        var point = e.GetCurrentPoint(AssociatedObject);
        var row = GetRowAtPoint(point.Position);
        if (row == null)
            return;

        var item = row.DataContext;
        if (item == _lastSelectedItem)
            return;

        _firstSelectedItem ??= item;
        _lastSelectedItem = item;

        if (!AssociatedObject!.SelectedItems.Contains(item))
            SelectRange();
    }

    private void DragSelection_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (!_isSelecting)
            return;

        _isSelecting = false;
        _firstSelectedItem = null;
        _lastSelectedItem = null;
        e.Pointer.Capture(null);
    }

    private void DragSelection_PointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
    {
        _isSelecting = false;
        _firstSelectedItem = null;
        _lastSelectedItem = null;
    }

    private DataGridRow? GetRowAtPoint(Point point)
    {
        var hit = AssociatedObject?.InputHitTest(point);
        if (hit is not Visual visual)
            return null;

        while (visual != null)
        {
            if (visual is DataGridRow row)
                return row;

            visual = visual.GetVisualParent() as Visual;
        }

        return null;
    }

    private void SelectRange()
    {
        var items = AssociatedObject?.ItemsSource?.OfType<object>().ToList();
        if (items == null || _firstSelectedItem == null || _lastSelectedItem == null)
            return;

        var lastIndex = items.IndexOf(_lastSelectedItem);
        var firstIndex = items.IndexOf(_firstSelectedItem);
        if (firstIndex < 0 || lastIndex < 0)
            return;

        var start = Math.Min(firstIndex, lastIndex);
        var end = Math.Max(firstIndex, lastIndex);

        for (var i = start; i <= end; i++)
        {
            if (!AssociatedObject!.SelectedItems.Contains(items[i]))
                AssociatedObject.SelectedItems.Add(items[i]);
        }
    }

    #endregion

    // Фокус на TextBox при клике в ячейке (без захвата указателя гридом).
    #region TextBoxFocus

    private bool _isFocusing;
    private TextBox? _firstSelectedTextBox;
    private TextBox? _lastSelectedTextBox;

    private void TextBoxFocus_PointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        var point = e.PointerPressedEventArgs.GetCurrentPoint(AssociatedObject);
        if (!point.Properties.IsLeftButtonPressed)
            return;

        var hit = AssociatedObject?.GetVisualAt(point.Position);
        if (IsInteractiveControl(hit) && hit is not Visual visual)
            return;

        if (hit is Visual hitVisual)
        {
            if (FindVisualParent<Button>(hitVisual) != null
                || FindVisualParent<CalendarDatePicker>(hitVisual) != null
                || FindVisualParent<RadionuclidsSelector>(hitVisual) != null
                || FindVisualParent<AppDropDownButton>(hitVisual) != null
                || FindVisualParent<AutoCompleteBox>(hitVisual) != null)
            {
                return;
            }
        }

        var textBox = hit is Visual v ? FindVisualParent<TextBox>(v) : null;
        if (textBox == null)
            return;

        _isFocusing = true;
        _firstSelectedTextBox = textBox;
        _lastSelectedTextBox = textBox;
    }

    private void TextBoxFocus_PointerMoved(object sender, PointerEventArgs e)
    {
        if (!_isFocusing)
            return;

        var point = e.GetCurrentPoint(AssociatedObject);
        var visual = AssociatedObject?.GetVisualAt(point.Position);
        _lastSelectedTextBox = visual is Visual v ? FindVisualParent<TextBox>(v) : null;
    }

    private void TextBoxFocus_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (!_isFocusing)
            return;

        if (_firstSelectedTextBox != null
            && _lastSelectedTextBox != null
            && ReferenceEquals(_firstSelectedTextBox, _lastSelectedTextBox))
        {
            _firstSelectedTextBox.Focus();
        }

        _isFocusing = false;
        _firstSelectedTextBox = null;
        _lastSelectedTextBox = null;
    }

    private void TextBoxFocus_PointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
    {
        _isFocusing = false;
        _firstSelectedTextBox = null;
        _lastSelectedTextBox = null;
    }

    #endregion

    private static bool IsInteractiveControl(object? hit)
    {
        if (hit is not Visual visual)
            return false;

        while (visual != null)
        {
            switch (visual)
            {
                case DataGridRow:
                case AvaloniaDataGrid:
                    return false;
                case Button:
                case CalendarDatePicker:
                case AutoCompleteBox:
                case RadionuclidsSelector:
                case AppDropDownButton:
                    return true;
                case TextBox:
                    return true;
            }

            visual = visual.GetVisualParent() as Visual;
        }

        return false;
    }

    public static T? FindVisualParent<T>(Visual? visual) where T : Visual
    {
        while (visual != null)
        {
            if (visual is T match)
                return match;

            visual = visual.GetVisualParent() as Visual;
        }

        return null;
    }
}
