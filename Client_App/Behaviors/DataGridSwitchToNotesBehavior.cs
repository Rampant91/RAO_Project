using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class DataGridSwitchToNotesBehavior : Behavior<DataGrid>
    {
        private DataGrid? _dataGrid;
        private readonly Dictionary<int, IDisposable> _subscriptions = new();
        private IDisposable? _layoutSubscription;

        public static readonly StyledProperty<DataGrid?> SourceDataGridProperty =
             AvaloniaProperty.Register<DataGridSwitchToNotesBehavior, DataGrid?>(
                 nameof(SourceDataGrid));

        public DataGrid? SourceDataGrid
        {
            get => GetValue(SourceDataGridProperty);
            set => SetValue(SourceDataGridProperty, value);
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;
        }

        private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            // Используем поиск через визуальное дерево
            SourceDataGrid ??= AssociatedObject.GetVisualAncestors()
                .OfType<DataGrid>()
                .FirstOrDefault();

            if (SourceDataGrid != null)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            if (AssociatedObject == null || SourceDataGrid == null) return;

            SourceDataGrid.LostFocus += OnLostFocusSourceGrid;
        }

        private void OnLostFocusSourceGrid(object? sender, RoutedEventArgs e)
        {
            if (sender is not DataGrid sourceDataGrid) return;

            if (AssociatedObject.Items is not ObservableCollection<Note> notes) return;

            var oldRow = GetRowByIndex(sourceDataGrid, sourceDataGrid.SelectedIndex);
            if (oldRow == null) return;

            var oldCell = GetCellByColumn(sourceDataGrid, oldRow, sourceDataGrid.CurrentColumn);
            var textBox = oldCell.FindDescendantOfType<TextBox>();

            if (textBox == null) return;

            if (string.IsNullOrEmpty(textBox.Text)) return;

            if (textBox.Text.Trim() != "прим.") return;

            if (notes.Any(note =>
            note.RowNumber_DB == (sourceDataGrid.SelectedIndex + 1).ToString()
            && note.GraphNumber_DB == (sourceDataGrid.CurrentColumn.DisplayIndex + 1).ToString())) return;

            notes.Add(new Note()
            {
                RowNumber_DB = (sourceDataGrid.SelectedIndex + 1).ToString(),
                GraphNumber_DB = (sourceDataGrid.CurrentColumn.DisplayIndex + 1).ToString(),
            });

            AssociatedObject.SelectedIndex = notes.Count - 1;
            AssociatedObject.CurrentColumn = AssociatedObject.Columns[2]; // Колонка с комментарием

            Dispatcher.UIThread.Post(() => {
                // Получаем новую строку после прокрутки
                var newRow = GetRowByIndex(AssociatedObject, AssociatedObject.SelectedIndex);
                if (newRow == null) return;

                // Ищем ячейку в новой позиции
                var newCell = GetCellByColumn(AssociatedObject, newRow, AssociatedObject.CurrentColumn);
                if (newCell != null)
                {
                    // Устанавливаем фокус на новую ячейку
                    newCell.Focus();

                    // Если в ячейке есть TextBox, фокусируемся на нем и выделяем весь текст
                    var noteTextBox = newCell.FindDescendantOfType<TextBox>();
                    if (noteTextBox != null)
                    {

                        noteTextBox.Focus();

                        Dispatcher.UIThread.Post(() =>
                        {
                            AssociatedObject.ScrollIntoView(notes.Last(), AssociatedObject.CurrentColumn);
                            noteTextBox.SelectAll();
                        }, DispatcherPriority.Background);

                        //Подключаем одноразовый обработчик события, который вернет пользователя к редактированию таблицы

                        // Сохраняем ссылку на обработчик
                        EventHandler<RoutedEventArgs> handler = null;
                        handler = (s, ev) =>
                        {
                            // Отписываемся от события
                            noteTextBox.LostFocus -= handler;

                            textBox.Focus();

                            Dispatcher.UIThread.Post(() =>
                            {
                               textBox.SelectionStart = textBox.Text.Length;
                            }, DispatcherPriority.Send);
                        };

                        noteTextBox.LostFocus += handler;
                    }
                }
            }, DispatcherPriority.Background);
        }

        private DataGridCell? GetCellByColumn(DataGrid dataGrid, DataGridRow row, DataGridColumn column)
        {
            // Получаем все ячейки в строке
            var cells = row.GetVisualDescendants().OfType<DataGridCell>().ToList();

            // Получаем индекс колонки
            var columnIndex = dataGrid.Columns.IndexOf(column);
            if (columnIndex >= 0 && columnIndex < cells.Count)
            {
                return cells[columnIndex];
            }

            return null;
        }
        private void ClearSubscriptions()
        {
            foreach (var subscription in _subscriptions.Values)
            {
                subscription.Dispose();
            }
            _subscriptions.Clear();
        }
        private DataGridRow? GetRowByIndex(DataGrid dataGrid, int index)
        {
            // Ищем все строки в DataGrid
            var rows = dataGrid.GetVisualDescendants().OfType<DataGridRow>().ToList();

            foreach (var row in rows)
            {
                if (GetRowIndex(dataGrid, row) == index)
                    return row;
            }

            return null;
        }
        private int GetRowIndex(DataGrid dataGrid, DataGridRow row)
        {
            var items = dataGrid.Items?.Cast<object>().ToList();
            if (items == null) return -1;

            return items.IndexOf(row.DataContext);
        }
        protected override void OnDetaching()
        {
            if (SourceDataGrid != null)
            {
                SourceDataGrid.LostFocus -= OnLostFocusSourceGrid;
            }

            ClearSubscriptions();
            _layoutSubscription?.Dispose();
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;

            base.OnDetaching();
        }
    }
}
