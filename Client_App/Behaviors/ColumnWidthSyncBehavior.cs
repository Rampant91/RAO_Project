using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;

namespace Client_App.Behaviors;

public class ColumnWidthSyncBehavior : Behavior<Grid>
{
    private DataGrid? _dataGrid;
    private readonly Dictionary<int, IDisposable> _subscriptions = new();
    private IDisposable? _layoutSubscription;

    public static readonly AttachedProperty<DataGrid?> SourceDataGridProperty =
        AvaloniaProperty.RegisterAttached<ColumnWidthSyncBehavior, Grid, DataGrid?>("SourceDataGrid");

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

        // Подписываемся на изменения коллекции колонок
        ((INotifyCollectionChanged)SourceDataGrid.Columns).CollectionChanged += OnColumnsChanged;

        // Инициализация существующих колонок
        SyncColumns();

        // Подписываемся на изменение размера DataGrid
        _layoutSubscription = SourceDataGrid.GetObservable(Visual.BoundsProperty)
            .Subscribe(_ => UpdateWidths());
    }

    private void OnColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        SyncColumns();
    }

    private void SyncColumns()
    {
        if (AssociatedObject == null || SourceDataGrid == null) return;

        // Очищаем предыдущие подписки
        ClearSubscriptions();

        // Создаем колонки по количеству в DataGrid
        AssociatedObject.ColumnDefinitions.Clear();
        for (var i = 0; i < SourceDataGrid.Columns.Count; i++)
        {
            var columnDefinition = new ColumnDefinition { Width = GridLength.Auto };
            AssociatedObject.ColumnDefinitions.Add(columnDefinition);
            // Подписываемся на изменение фактической ширины
            var subscription = Observable.FromEventPattern<EventHandler, EventArgs>(
                    handler => SourceDataGrid!.LayoutUpdated += handler,
                    handler => SourceDataGrid!.LayoutUpdated -= handler)
                .Subscribe(_ => UpdateColumnWidth(i));
            _subscriptions[i] = subscription;
        }
    }

    private void UpdateColumnWidth(int columnIndex)
    {
        if (SourceDataGrid == null ||
            columnIndex >= SourceDataGrid.Columns.Count ||
            columnIndex >= AssociatedObject?.ColumnDefinitions.Count)
            return;

        var column = SourceDataGrid.Columns[columnIndex];

        // Используем рефлексию для получения ActualWidth
        var actualWidth = column.Width.DisplayValue;

        if (actualWidth > 0)
        {
            //вычитаем 1 пиксель, иначе шапка таблицы съезжает
            AssociatedObject.ColumnDefinitions[columnIndex].Width = new GridLength(actualWidth - 1);
        }
    }

    private void UpdateWidths()
    {
        if (SourceDataGrid == null) return;

        for (var i = 0; i < SourceDataGrid.Columns.Count; i++)
        {
            UpdateColumnWidth(i);
        }
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
            ((INotifyCollectionChanged)SourceDataGrid.Columns).CollectionChanged -= OnColumnsChanged;
        }

        ClearSubscriptions();
        _layoutSubscription?.Dispose();
        AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;

        base.OnDetaching();
    }
}