using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Forms.Form1;

namespace Client_App.Behaviors
{
    public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
    {
        public static readonly StyledProperty<IList?> SelectedItemsProperty =
            AvaloniaProperty.Register<DataGridSelectedItemsBehavior, IList?>(
                nameof(SelectedItems));

        public IList? SelectedItems
        {
            get => GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged += OnSelectionChanged;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged -= OnSelectionChanged;
            }

            base.OnDetaching();
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (AssociatedObject == null || SelectedItems == null)
                return;

            // Очищаем коллекцию в UI-потоке (если Avalonia)
            Dispatcher.UIThread.Post(() =>
            {
                SelectedItems.Clear();

                foreach (var item in AssociatedObject.SelectedItems)
                {
                    if (item is Form12 form)  // Проверяем тип
                    {
                        SelectedItems.Add(form);
                    }
                }
            });
        }
    }
}
