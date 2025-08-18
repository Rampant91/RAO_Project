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
    /// <summary>
    /// Этот Behavior используется для привязки к DataGrid нового параметра, хранящего все выделенные ячейки
    /// </summary>
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
        private bool _isUpdating;

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

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == SelectedItemsProperty)
            {
                if (AssociatedObject == null || SelectedItems == null || _isUpdating)
                    return;

                _isUpdating = true;

                try
                {
                    // Обновляем выделение в DataGrid при изменении SelectedItems
                    AssociatedObject.SelectedItems.Clear();
                    foreach (var item in SelectedItems)
                    {
                        AssociatedObject.SelectedItems.Add(item);
                    }
                }
                finally
                {
                    _isUpdating = false;
                }
            }
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (AssociatedObject == null || SelectedItems == null || _isUpdating)
                return;

            _isUpdating = true;

            try
            {
                // Обновляем SelectedItems при изменении выделения в DataGrid
                SelectedItems.Clear();
                foreach (var item in AssociatedObject.SelectedItems)
                {
                    if (item is Form12 form)
                    {
                        SelectedItems.Add(form);
                    }
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }
    }
}
