using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors
{
    public class DataGridDoubleClickBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped += OnDataGridDoubleTapped;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped -= OnDataGridDoubleTapped;
            }

            base.OnDetaching();
        }
        private void OnDataGridDoubleTapped(object sender, RoutedEventArgs e)
        {
            if (e is TappedEventArgs tappedEvent)
            {

                // Получаем DataGrid
                var dataGrid = (DataGrid)sender;

                // Получаем позицию клика относительно DataGrid
                var point = tappedEvent.GetPosition(dataGrid);

                // Находим визуальный элемент в точке клика
                var visual = dataGrid.GetVisualAt(point);

                // Ищем TextBox в визуальном дереве
                var textBox = FindVisualParent<TextBox>((Visual)visual);

                if (textBox != null)
                {
                    // Активируем TextBox
                    textBox.IsHitTestVisible = true;
                    textBox.Focus();

                    // Подписываемся на событие потери фокуса
                    textBox.LostFocus += OnTextBoxLostFocus;
                }
            }
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
    }
}
