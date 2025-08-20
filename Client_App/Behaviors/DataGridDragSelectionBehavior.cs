using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit.Utils;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Client_App.Behaviors
{
    public class DataGridDragSelectionBehavior : Behavior<DataGrid>
    {
        private bool _isSelecting = false;
        private object _firstSelectedItem;

        private object _lastSelectedItem;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                // Подписываемся на события
                AssociatedObject.PointerPressed += DataGrid_PointerPressed;
                AssociatedObject.PointerMoved += DataGrid_PointerMoved;
                AssociatedObject.PointerReleased += DataGrid_PointerReleased;
                AssociatedObject.PointerCaptureLost += DataGrid_PointerCaptureLost;
                AssociatedObject.SelectionChanged += DataGrid_OnSelectionChanged;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PointerPressed -= DataGrid_PointerPressed;
                AssociatedObject.PointerMoved -= DataGrid_PointerMoved;
                AssociatedObject.PointerReleased -= DataGrid_PointerReleased;
                AssociatedObject.PointerCaptureLost -= DataGrid_PointerCaptureLost;
                AssociatedObject.SelectionChanged -= DataGrid_OnSelectionChanged;
            }

            base.OnDetaching();
        }

        private void DataGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (AssociatedObject.SelectedItems.Count != 1)
            {
                return;
            }

            _isSelecting = true;
            _firstSelectedItem = AssociatedObject.SelectedItems[0];
            _lastSelectedItem = AssociatedObject.SelectedItems[0];
        }
        private void DataGrid_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            AssociatedObject.SelectedItems.Clear();
            var point = e.GetCurrentPoint(AssociatedObject);

            // Обрабатываем только левую кнопку мыши
            if (point.Properties.IsLeftButtonPressed)
            {
                _isSelecting = true;

                // Захватываем указатель для получения всех событий
                AssociatedObject.CapturePointer(e.Pointer);

                var row = GetRowAtPoint(point.Position);
                if (row != null)
                {
                    var item = row.DataContext;
                    if (_firstSelectedItem ==null)
                        _firstSelectedItem = item;
                    _lastSelectedItem = item;

                    // Обычный клик - очищаем и выделяем один элемент
                    AssociatedObject.SelectedItems.Add(item);
                }

                e.Handled = true;
            }
        }

        private void DataGrid_PointerMoved(object sender, PointerEventArgs e)
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

                e.Handled = true;
            }
        }

        private void DataGrid_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (_isSelecting)
            {
                _isSelecting = false;
                _firstSelectedItem = null;
                _lastSelectedItem = null;

                // Освобождаем захват указателя
                AssociatedObject.ReleasePointerCapture(e.Pointer);

                e.Handled = true;
            }
        }
        
        private void DataGrid_PointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
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
    }
}
