using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit.Utils;
using Client_App.Controls;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Client_App.Behaviors;
/// <summary>
/// Этот Behavior для DataGrid предназначен для объединения других Behavior-ов, использующих событие PointerMoved.
/// Это необходимо, т.к. в Avalonia 0.10.20 PointerMoved не может вызвать несколько обработчиков событий
/// </summary>
public class DataGridPointerBehavior : Behavior<DataGrid>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            // Подписываемся на события
            AssociatedObject.CellPointerPressed += DataGrid_PointerPressed;
            AssociatedObject.PointerMoved += DataGrid_PointerMoved;
            AssociatedObject.PointerReleased += DataGrid_PointerReleased;
            AssociatedObject.PointerCaptureLost += DataGrid_PointerCaptureLost;
        }
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
        // Редактор уже открыт — DataGrid не перехватывает указатель (каретка, выделение текста).
        if (IsPointerOverActiveLazyEditor(e))
            return;

        // Клик по отображению lazy-ячейки — вход обрабатывает DataGridLazyEditHost, без CapturePointer.
        if (IsPointerOnLazyEditHostDisplay(e))
        {
            if (!ShouldDeferPointerHandling(e))
                CloseEditingHostsInOtherCells(e);
            return;
        }

        if (!ShouldDeferPointerHandling(e))
            CloseEditingHostsInOtherCells(e);

        DragSelection_PointerPressed(sender, e);
        TextBoxFocus_PointerPressed(sender, e);
    }

    private bool IsPointerOverActiveLazyEditor(DataGridCellPointerPressedEventArgs e)
    {
        var hit = GetHitVisual(e);
        if (hit == null)
            return IsCellInLazyEditMode(e.Cell) || IsCellInLazyEditMode(GetCellAtPointer(e));

        if (hit.GetVisualAncestors().OfType<DataGridLazyEditHost>().Any(h => h.IsInEditMode))
            return true;

        var cell = hit.GetVisualAncestors().OfType<DataGridCell>().FirstOrDefault() ?? GetCellAtPointer(e);
        return IsCellInLazyEditMode(cell);
    }

    private bool IsPointerOnLazyEditHostDisplay(DataGridCellPointerPressedEventArgs e)
    {
        var hit = GetHitVisual(e);
        var host = hit?.GetVisualAncestors().OfType<DataGridLazyEditHost>().FirstOrDefault();
        return host is { IsInEditMode: false };
    }

    private Visual? GetHitVisual(DataGridCellPointerPressedEventArgs e)
    {
        var point = e.PointerPressedEventArgs.GetCurrentPoint(AssociatedObject);
        return AssociatedObject.GetVisualAt(point.Position) as Visual;
    }

    private void CloseEditingHostsInOtherCells(DataGridCellPointerPressedEventArgs e)
    {
        var clickedCell = GetCellAtPointer(e) ?? e.Cell;
        if (clickedCell == null)
            return;

        // Клик по ячейке, которая уже редактируется — редактор не закрываем.
        if (clickedCell.GetVisualDescendants().OfType<DataGridLazyEditHost>().Any(h => h.IsInEditMode))
            return;

        var grid = clickedCell.GetVisualAncestors().OfType<DataGrid>().FirstOrDefault()
                   ?? AssociatedObject;
        if (grid == null)
            return;

        foreach (var host in grid.GetVisualDescendants().OfType<DataGridLazyEditHost>().Where(h => h.IsInEditMode))
        {
            var hostCell = host.GetVisualAncestors().OfType<DataGridCell>().FirstOrDefault();
            if (hostCell != null && AreSameLogicalCell(hostCell, clickedCell))
                continue;

            host.ExitEditMode();
        }
    }

    private DataGridCell? GetCellAtPointer(DataGridCellPointerPressedEventArgs e)
    {
        var point = e.PointerPressedEventArgs.GetCurrentPoint(AssociatedObject);
        return (AssociatedObject.GetVisualAt(point.Position) as Visual)?
            .GetVisualAncestors()
            .OfType<DataGridCell>()
            .FirstOrDefault();
    }

    private static bool AreSameLogicalCell(DataGridCell a, DataGridCell b)
    {
        if (ReferenceEquals(a, b))
            return true;

        var rowA = a.GetVisualAncestors().OfType<DataGridRow>().FirstOrDefault();
        var rowB = b.GetVisualAncestors().OfType<DataGridRow>().FirstOrDefault();
        if (rowA == null || rowB == null || !ReferenceEquals(rowA, rowB))
            return false;

        return GetCellColumnIndex(a) == GetCellColumnIndex(b);
    }

    private static int GetCellColumnIndex(DataGridCell cell)
    {
        if (cell.Parent is not Panel panel)
            return -1;

        for (var i = 0; i < panel.Children.Count; i++)
        {
            if (ReferenceEquals(panel.Children[i], cell))
                return i;
        }

        return -1;
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

    // Поведение отвечающее за выбор нескольких строк с зажатой мышкой
    #region DragSelection
    
    private bool _isSelecting = false;
    private object _firstSelectedItem;

    private object _lastSelectedItem;

    private void DragSelection_PointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        if (ShouldDeferPointerHandling(e))
            return;

        var point = e.PointerPressedEventArgs.GetCurrentPoint(AssociatedObject);

        if (point.Properties.IsLeftButtonPressed)
        {
            _isSelecting = true;

            var row = GetRowAtPoint(point.Position);
            if (row != null)
            {
                var item = row.DataContext;
                if (_firstSelectedItem == null)
                    _firstSelectedItem = item;
                _lastSelectedItem = item;

                if (e.PointerPressedEventArgs.KeyModifiers != KeyModifiers.Shift)
                {
                    var keepSelection = AssociatedObject.SelectedItems.Count == 1
                                        && ReferenceEquals(AssociatedObject.SelectedItems[0], item);
                    if (!keepSelection)
                        AssociatedObject.SelectedItems.Clear();
                }

                if (!AssociatedObject.SelectedItems.Contains(item))
                    AssociatedObject.SelectedItems.Add(item);
            }

            AssociatedObject.CapturePointer(e.PointerPressedEventArgs.Pointer);

        }
    }

    private void DragSelection_PointerMoved(object sender, PointerEventArgs e)
    {
        if (_isSelecting)
        {
            var point = e.GetCurrentPoint(AssociatedObject);
            var row = GetRowAtPoint(point.Position);

            if (row != null)
            {
                var item = row.DataContext;
                if (item != _lastSelectedItem)
                {
                    if (_firstSelectedItem == null)
                        _firstSelectedItem = item;
                    _lastSelectedItem = item;

                    // Добавляем элемент к выделению, если его еще нет
                    if (!AssociatedObject.SelectedItems.Contains(item))
                    {
                        SelectRange();
                    }
                }
            }

        }
    }

    private void DragSelection_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (_isSelecting)
        {
            _isSelecting = false;
            _firstSelectedItem = null;
            _lastSelectedItem = null;

            // Освобождаем захват указателя
            AssociatedObject.ReleasePointerCapture(e.Pointer);

        }
    }

    private void DragSelection_PointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
    {
        _isSelecting = false;
        _firstSelectedItem = null;
        _lastSelectedItem = null;
    }

    private DataGridRow GetRowAtPoint(Point point)
    {
        // Используем InputHitTest для более точного определения элемента
        var hit = AssociatedObject.InputHitTest(point);
        if (hit == null) return null;

        // Ищем DataGridRow в визуальном дереве
        var visual = hit as Visual;
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
        var items = AssociatedObject.Items?.OfType<object>().ToList();
        if (items == null) return;


        var lastIndex = items.IndexOf(_lastSelectedItem);
        var firstIndex = items.IndexOf(_firstSelectedItem);

        if (firstIndex >= 0 && lastIndex >= 0)
        {
            var start = Math.Min(firstIndex, lastIndex);
            var end = Math.Max(firstIndex, lastIndex);

            for (int i = start; i <= end; i++)
            {
                if (!AssociatedObject.SelectedItems.Contains(items[i]))
                {
                    AssociatedObject.SelectedItems.Add(items[i]);
                }
            }
        }
    }
    #endregion


    // Поведение отвечающее за фокусировку на текстбоксе
    // Если пользователь кликнул на текстбокс и отпустил в том же месте, то программа сфокусируется на нем
    #region TextBoxFocus

    private bool _isFocusing = false;
    private TextBox? _firstSelectedTextBox;
    private TextBox? _lastSelectedTextBox;
    private DataGridLazyEditHost? _firstLazyHost;
    private DataGridLazyEditHost? _lastLazyHost;

    private void TextBoxFocus_PointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        var point = e.PointerPressedEventArgs.GetCurrentPoint(AssociatedObject);

        if (!point.Properties.IsLeftButtonPressed)
            return;

        if (ShouldDeferPointerHandling(e))
            return;

        var cell = GetCellAtPointer(e) ?? e.Cell;
        if (cell?.GetVisualDescendants().OfType<DataGridLazyEditHost>().Any() == true)
            return;

        var textBox = FindTextBoxInCell(e.Cell);
        DataGridLazyEditHost? lazyHost = null;

        if (textBox == null && lazyHost == null)
        {
            var visual = AssociatedObject.GetVisualAt(point.Position);
            if (visual is Visual hitVisual)
            {
                textBox = FindVisualParent<TextBox>(hitVisual);
                lazyHost = FindVisualParent<DataGridLazyEditHost>(hitVisual);
            }
        }

        AssociatedObject.CapturePointer(e.PointerPressedEventArgs.Pointer);

        if (textBox != null)
        {
            _isFocusing = true;
            _firstSelectedTextBox = textBox;
            _lastSelectedTextBox = textBox;
            _firstLazyHost = null;
            _lastLazyHost = null;
        }
        else if (lazyHost != null)
        {
            _isFocusing = true;
            _firstLazyHost = lazyHost;
            _lastLazyHost = lazyHost;
            _firstSelectedTextBox = null;
            _lastSelectedTextBox = null;
        }
    }

    private static bool IsCellInLazyEditMode(DataGridCell? cell) =>
        cell?.GetVisualDescendants().OfType<DataGridLazyEditHost>().Any(h => h.IsInEditMode) == true;

    /// <summary>
    /// Не перехватывать указатель DataGrid: ячейка уже в режиме редактирования или клик по кнопке/календарю/списку.
    /// </summary>
    private bool ShouldDeferPointerHandling(DataGridCellPointerPressedEventArgs e) =>
        IsPointerOverActiveLazyEditor(e) || IsInteractiveHit(GetHitVisual(e));

    private static bool IsInteractiveHit(Visual? hit)
    {
        if (hit == null)
            return false;
        return IsInteractiveEditorElement(hit);
    }

    private static bool IsInteractiveEditorElement(Visual visual)
    {
        for (var current = visual; current != null; current = current.GetVisualParent() as Visual)
        {
            switch (current)
            {
                case Button:
                case DropDownButton:
                case RadionuclidsSelector:
                case CalendarDatePicker:
                case Popup:
                    return true;
                case global::Avalonia.Controls.AutoCompleteBox autoComplete when autoComplete.IsDropDownOpen:
                    return true;
                case DataGridLazyEditHost host when host.IsInEditMode:
                    return true;
            }
        }

        return false;
    }

    private static TextBox? FindTextBoxInCell(DataGridCell? cell) =>
        cell?.GetVisualDescendants().OfType<TextBox>().FirstOrDefault(tb => tb.IsVisible);

    private static DataGridLazyEditHost? FindLazyHostInCell(DataGridCell? cell) =>
        cell?.GetVisualDescendants().OfType<DataGridLazyEditHost>()
            .FirstOrDefault(h => !h.IsInEditMode);

    private void TextBoxFocus_PointerMoved(object sender, PointerEventArgs e)
    {
        if (_isFocusing)
        {
            var point = e.GetCurrentPoint(AssociatedObject);
            var visual = AssociatedObject.GetVisualAt(point.Position);
            if (visual is not Visual hitVisual)
                return;

            _lastSelectedTextBox = FindVisualParent<TextBox>(hitVisual);
            var lazyHost = FindVisualParent<DataGridLazyEditHost>(hitVisual);
            if (lazyHost?.IsInEditMode == true)
                lazyHost = null;
            _lastLazyHost = lazyHost;
        }
    }

    private void TextBoxFocus_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (_isFocusing)
        {
            if (_firstSelectedTextBox != null
                && _lastSelectedTextBox != null
                && _firstSelectedTextBox == _lastSelectedTextBox)
            {
                ActivateTextBox(_firstSelectedTextBox, e);
            }
            else if (_firstLazyHost != null
                     && _lastLazyHost != null
                     && _firstLazyHost == _lastLazyHost)
            {
                var clickInHost = e.GetCurrentPoint(_firstLazyHost).Position;
                _firstLazyHost.EnterEditMode(clickInHost);
            }

            _isFocusing = false;
            _firstSelectedTextBox = null;
            _lastSelectedTextBox = null;
            _firstLazyHost = null;
            _lastLazyHost = null;

            AssociatedObject.ReleasePointerCapture(e.Pointer);
        }
    }

    private void TextBoxFocus_PointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
    {
        if (_firstSelectedTextBox != null
            && _lastSelectedTextBox != null
            && _firstSelectedTextBox == _lastSelectedTextBox)
        {
            _firstSelectedTextBox.IsHitTestVisible = true;
            if (!_firstSelectedTextBox.IsFocused)
            {
                _firstSelectedTextBox.LostFocus += OnTextBoxLostFocus;
                _firstSelectedTextBox.Focus();
            }
        }
        else if (_firstLazyHost != null
                 && _lastLazyHost != null
                 && _firstLazyHost == _lastLazyHost)
        {
            _firstLazyHost.EnterEditMode();
        }

        _isFocusing = false;
        _firstSelectedTextBox = null;
        _lastSelectedTextBox = null;
        _firstLazyHost = null;
        _lastLazyHost = null;
    }
    private void ActivateTextBox(TextBox textBox, PointerReleasedEventArgs e)
    {
        textBox.IsHitTestVisible = true;
        textBox.LostFocus += OnTextBoxLostFocus;
        textBox.Focus();
        var pointInTextBox = e.GetCurrentPoint(textBox).Position;
        Controls.DataGridLazyEditHost.TrySetCaretFromPoint(textBox, pointInTextBox);
    }

    private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            if (FindVisualParent<Controls.DataGridLazyEditHost>(textBox)?.IsInEditMode == true)
                return;

            textBox.IsHitTestVisible = false;
            textBox.LostFocus -= OnTextBoxLostFocus;
        }
    }
    // Вспомогательные методы для поиска в визуальном дереве
    public static T FindVisualParent<T>(Visual visual) where T : Visual
    {
        while (visual != null && !(visual is T))
        {
            visual = (Visual)visual.GetVisualParent();
        }
        return visual as T;
    }
    #endregion

}