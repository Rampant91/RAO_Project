using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
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

        public static readonly AttachedProperty<DataGrid?> SourceDataGridProperty =
            AvaloniaProperty.RegisterAttached<DataGridSwitchToNotesBehavior, DataGrid, DataGrid?>("SourceDataGrid");

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
            if (e.Source is not TextBox textBox) return;

            if (textBox.Text.Trim() == "прим.")
            notes.Add(new Note() 
            { 
                RowNumber_DB = (sourceDataGrid.SelectedIndex + 1).ToString(),
                GraphNumber_DB = (sourceDataGrid.CurrentColumn.DisplayIndex + 1).ToString(),
            });
        }

        private void ClearSubscriptions()
        {
            foreach (var subscription in _subscriptions.Values)
            {
                subscription.Dispose();
            }
            _subscriptions.Clear();
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
