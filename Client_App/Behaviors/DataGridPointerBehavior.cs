using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit.Utils;
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

    // Поведение отвечающее за выбор нескольких строк с зажатой мышкой
    #region DragSelection
    
    private bool _isSelecting = false;
    private object _firstSelectedItem;

    private object _lastSelectedItem;

    private void DragSelection_PointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        var point = e.PointerPressedEventArgs.GetCurrentPoint(AssociatedObject);


        if (point.Properties.IsLeftButtonPressed)
        {
            _isSelecting = true;

            if (e.PointerPressedEventArgs.KeyModifiers != KeyModifiers.Shift)
                AssociatedObject.SelectedItems.Clear();
            // Захватываем указатель для получения всех событий
            AssociatedObject.CapturePointer(e.PointerPressedEventArgs.Pointer);

            var row = GetRowAtPoint(point.Position);
            if (row != null)
            {
                var item = row.DataContext;
                if (_firstSelectedItem == null)
                    _firstSelectedItem = item;
                _lastSelectedItem = item;

                // Обычный клик - очищаем и выделяем один элемент
                AssociatedObject.SelectedItems.Add(item);
            }

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

    private void TextBoxFocus_PointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        var point = e.PointerPressedEventArgs.GetCurrentPoint(AssociatedObject);


        if (point.Properties.IsLeftButtonPressed)
        {
            // Захватываем указатель для получения всех событий
            AssociatedObject.CapturePointer(e.PointerPressedEventArgs.Pointer);

            // Находим визуальный элемент в точке клика
            var visual = AssociatedObject.GetVisualAt(point.Position);

            // Ищем TextBox в визуальном дереве
            var textBox = FindVisualParent<TextBox>((Visual)visual);

            if (textBox != null)
            {
                _isFocusing = true;
                _firstSelectedTextBox = textBox;
                _lastSelectedTextBox = textBox;
            }


        }
    }

    private void TextBoxFocus_PointerMoved(object sender, PointerEventArgs e)
    {
        if (_isFocusing)
        {
            var point = e.GetCurrentPoint(AssociatedObject);
            // Находим визуальный элемент в точке клика
            var visual = AssociatedObject.GetVisualAt(point.Position);

            // Ищем TextBox в визуальном дереве
            var textBox = FindVisualParent<TextBox>((Visual)visual);

            _lastSelectedTextBox = textBox;

        }
    }

    private void TextBoxFocus_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (_isFocusing)
        {
            if ((_firstSelectedTextBox != null)
                && (_lastSelectedTextBox != null)
                && (_firstSelectedTextBox == _lastSelectedTextBox))
            {
                _firstSelectedTextBox.LostFocus += OnTextBoxLostFocus;
                _firstSelectedTextBox.Focus();
                _firstSelectedTextBox.IsHitTestVisible = true;
            }
            _isSelecting = false;
            _firstSelectedTextBox = null;
            _lastSelectedTextBox = null;

            // Освобождаем захват указателя
            AssociatedObject.ReleasePointerCapture(e.Pointer);

        }
    }

    private void TextBoxFocus_PointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
    {
        if ((_firstSelectedTextBox != null)
                && (_lastSelectedTextBox != null)
                && (_firstSelectedTextBox == _lastSelectedTextBox))
        {
            _firstSelectedTextBox.LostFocus += OnTextBoxLostFocus;
            _firstSelectedTextBox.Focus();
            _firstSelectedTextBox.IsHitTestVisible = true;
        }

        _isSelecting = false;
        _firstSelectedTextBox = null;
        _lastSelectedTextBox = null;
    }
    private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            // Деактивируем TextBox при потере фокуса
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