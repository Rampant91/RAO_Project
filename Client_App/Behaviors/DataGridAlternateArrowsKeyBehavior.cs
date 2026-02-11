using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System.Linq;
using Avalonia.VisualTree;
using Avalonia.Threading;

namespace Client_App.Behaviors
{
    public class DataGridAlternateArrowsKeyBehavior : Behavior<DataGrid>
    {
        public Control? cell { get; set; } = null;
        public static readonly StyledProperty<bool> IsEditingProperty =
            AvaloniaProperty.Register<DataGridAlternateArrowsKeyBehavior, bool>(nameof(IsEditing));

        public bool IsEditing
        {
            get => GetValue(IsEditingProperty);
            set => SetValue(IsEditingProperty, value);
        }

        private bool AlternateArrowKeysControl = false;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AddHandler(
                InputElement.KeyUpEvent,
                DataGrid_OnKeyUp,
                RoutingStrategies.Tunnel);

            AssociatedObject.AddHandler(
                InputElement.KeyDownEvent,
                DataGrid_OnKeyDown,
                RoutingStrategies.Tunnel);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(InputElement.KeyUpEvent, DataGrid_OnKeyUp);
            AssociatedObject.RemoveHandler(InputElement.KeyDownEvent, DataGrid_OnKeyDown);
            base.OnDetaching();
        }

        private void DataGrid_OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
                AlternateArrowKeysControl = !AlternateArrowKeysControl;

            if (IsEditing
                && (AlternateArrowKeysControl ^ (e.KeyModifiers == KeyModifiers.Alt))
                && ((e.Key == Key.Up) || (e.Key == Key.Down) || (e.Key == Key.Left) || (e.Key == Key.Right)))
            {
                switch (e.Key)
                {
                    case Key.Up:
                        MoveFocus(-1, 0);
                        break;
                    case Key.Down:
                        MoveFocus(1, 0);
                        break;
                    case Key.Left:
                        MoveFocus(0, -1);
                        break;
                    case Key.Right:
                        MoveFocus(0, 1);
                        break;
                }
                e.Handled = true;
            }
        }

        private void DataGrid_OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (IsEditing
                && (AlternateArrowKeysControl ^ (e.KeyModifiers == KeyModifiers.Alt))
                && ((e.Key == Key.Up) || (e.Key == Key.Down) || (e.Key == Key.Left) || (e.Key == Key.Right)))
            {
                e.Handled = true;
            }
        }

        private void MoveFocus(int rowDelta, int columnDelta)
        {
            var currentCell = GetCurrentCell();
            if (currentCell == null) return;

            var dataGrid = AssociatedObject;
            var currentRow = currentCell.FindAncestorOfType<DataGridRow>();

            if (currentRow == null) return;

            // Получаем индекс текущей строки
            var rowIndex = GetRowIndex(currentRow);
            if (rowIndex == -1) return;

            // Получаем индекс текущей колонки
            var columnIndex = GetColumnIndex(currentCell);
            if (columnIndex == -1) return;

            // Вычисляем новые индексы
            var newRowIndex = rowIndex + rowDelta;
            var newColumnIndex = columnIndex + columnDelta;

            // Проверяем границы
            var itemsCount = dataGrid.Items?.Cast<object>().Count() ?? 0;
            if (newRowIndex < 0 || newRowIndex >= itemsCount) return;
            if (newColumnIndex < 0 || newColumnIndex >= dataGrid.Columns.Count) return;

            // Получаем элемент данных для новой строки
            var newItem = dataGrid.Items.Cast<object>().ElementAt(newRowIndex);

            // Получаем новую колонку
            var newColumn = dataGrid.Columns[newColumnIndex];

            // Прокручиваем к нужной строке
            dataGrid.ScrollIntoView(newItem, newColumn);

            // Ждем немного, чтобы визуальное дерево обновилось после прокрутки
            Dispatcher.UIThread.Post(() => {
                // Получаем новую строку после прокрутки
                var newRow = GetRowByIndex(dataGrid, newRowIndex);
                if (newRow == null) return;

                // Ищем ячейку в новой позиции
                var newCell = GetCellByColumn(newRow, newColumn);
                if (newCell != null)
                {
                    // Устанавливаем фокус на новую ячейку
                    newCell.Focus();

                    // Если в ячейке есть TextBox, фокусируемся на нем и выделяем весь текст
                    var textBox = newCell.FindDescendantOfType<TextBox>();
                    if (textBox != null)
                    {
                        AssociatedObject.SelectedIndex = newRowIndex;
                        AssociatedObject.CurrentColumn = newColumn;
                        textBox.Focus();
                        Dispatcher.UIThread.Post(() =>
                        {
                            textBox.SelectAll();
                        }, DispatcherPriority.Background);

                    }
                }
            }, DispatcherPriority.Background);
        }

        private DataGridCell? GetCurrentCell()
        {
            // Получаем текущий фокусный элемент
            var focusedElement = FocusManager.Instance?.Current;

            // Ищем DataGridCell среди предков или самого элемента
            var cell = focusedElement as DataGridCell ??
                       focusedElement?.FindAncestorOfType<DataGridCell>();

            return cell;
        }

        private int GetRowIndex(DataGridRow row)
        {
            var dataGrid = AssociatedObject;
            var items = dataGrid.Items?.Cast<object>().ToList();
            if (items == null) return -1;

            return items.IndexOf(row.DataContext);
        }

        private int GetColumnIndex(DataGridCell cell)
        {
            var dataGrid = AssociatedObject;

            // Получаем все ячейки в строке
            var row = cell.FindAncestorOfType<DataGridRow>();
            if (row == null) return -1;

            var cells = row.GetVisualDescendants().OfType<DataGridCell>().ToList();
            return cells.IndexOf(cell);
        }

        private DataGridRow? GetRowByIndex(DataGrid dataGrid, int index)
        {
            // Ищем все строки в DataGrid
            var rows = dataGrid.GetVisualDescendants().OfType<DataGridRow>().ToList();

            foreach (var row in rows)
            {
                if (GetRowIndex(row) == index)
                    return row;
            }

            return null;
        }

        private DataGridCell? GetCellByColumn(DataGridRow row, DataGridColumn column)
        {
            // Получаем все ячейки в строке
            var cells = row.GetVisualDescendants().OfType<DataGridCell>().ToList();

            // Получаем индекс колонки
            var columnIndex = AssociatedObject.Columns.IndexOf(column);
            if (columnIndex >= 0 && columnIndex < cells.Count)
            {
                return cells[columnIndex];
            }

            return null;
        }
    }
}