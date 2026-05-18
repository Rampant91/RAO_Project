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
    private readonly Dictionary<int, IDisposable> _subscriptions = new();
    private IDisposable? _layoutSubscription;
    private IDisposable? _startIndexSub;
    private IDisposable? _columnCountSub;

    public static readonly AttachedProperty<DataGrid?> SourceDataGridProperty =
        AvaloniaProperty.RegisterAttached<ColumnWidthSyncBehavior, Grid, DataGrid?>("SourceDataGrid");

    public DataGrid? SourceDataGrid
    {
        get => GetValue(SourceDataGridProperty);
        set => SetValue(SourceDataGridProperty, value);
    }

    /// <summary>
    /// Индекс первой колонки DataGrid, начиная с которой синхронизируются ширины (по умолчанию 0 — все).
    /// </summary>
    public static readonly AttachedProperty<int> StartColumnIndexProperty =
        AvaloniaProperty.RegisterAttached<ColumnWidthSyncBehavior, Grid, int>("StartColumnIndex", 0);

    public int StartColumnIndex
    {
        get => GetValue(StartColumnIndexProperty);
        set => SetValue(StartColumnIndexProperty, value);
    }

    /// <summary>
    /// Количество синхронизируемых колонок (0 = все, начиная с StartColumnIndex).
    /// </summary>
    public static readonly AttachedProperty<int> ColumnCountProperty =
        AvaloniaProperty.RegisterAttached<ColumnWidthSyncBehavior, Grid, int>("ColumnCount", 0);

    public int ColumnCount
    {
        get => GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;

        // Реагируем на динамическое изменение StartColumnIndex / ColumnCount через привязку.
        // Property.Changed — глобальный IObservable на классе свойства, фильтруем по this.
        _startIndexSub = StartColumnIndexProperty.Changed
            .Where(e => ReferenceEquals(e.Sender, this))
            .Subscribe(_ => { if (SourceDataGrid is not null) { ClearSubscriptions(); SyncColumns(); } });
        _columnCountSub = ColumnCountProperty.Changed
            .Where(e => ReferenceEquals(e.Sender, this))
            .Subscribe(_ => { if (SourceDataGrid is not null) { ClearSubscriptions(); SyncColumns(); } });
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

        ClearSubscriptions();

        var start = StartColumnIndex;
        var end = ColumnCount > 0
            ? Math.Min(start + ColumnCount, SourceDataGrid.Columns.Count)
            : SourceDataGrid.Columns.Count;

        var neededCount = end - start;

        // Добавляем недостающие ColumnDefinitions
        while (AssociatedObject.ColumnDefinitions.Count < neededCount)
            AssociatedObject.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        // Лишние колонки НЕ удаляем — удаление ломает Grid.Column/ColumnSpan у дочерних элементов.
        // Вместо этого ставим Width=0, чтобы они не занимали места.
        for (var i = neededCount; i < AssociatedObject.ColumnDefinitions.Count; i++)
            AssociatedObject.ColumnDefinitions[i].Width = new GridLength(0);

        for (var i = start; i < end; i++)
        {
            var gridIndex = i - start;
            var dataGridIndex = i;

            AssociatedObject.ColumnDefinitions[gridIndex].Width = GridLength.Auto;

            var subscription = Observable.FromEventPattern<EventHandler, EventArgs>(
                    handler => SourceDataGrid!.LayoutUpdated += handler,
                    handler => SourceDataGrid!.LayoutUpdated -= handler)
                .Subscribe(_ => UpdateColumnWidth(gridIndex, dataGridIndex));

            _subscriptions[gridIndex] = subscription;
        }
    }

    private void UpdateColumnWidth(int gridColumnIndex, int dataGridColumnIndex)
    {
        if (SourceDataGrid == null ||
            dataGridColumnIndex >= SourceDataGrid.Columns.Count ||
            gridColumnIndex >= (AssociatedObject?.ColumnDefinitions.Count ?? 0))
            return;

        var column = SourceDataGrid.Columns[dataGridColumnIndex];

        var scale = (double)SourceDataGrid.GetVisualAncestors()
            .OfType<Window>()
            .First()
            .Screens
            .Primary
            .PixelDensity!;

        var actualWidth = column.Width.DisplayValue;

        if (actualWidth > 0)
        {
            //вычитаем 1 пиксель делённый на масштаб, иначе шапка таблицы съезжает
            AssociatedObject!.ColumnDefinitions[gridColumnIndex].Width = new GridLength(actualWidth - 1 / scale);
        }
    }

    private void UpdateWidths()
    {
        if (SourceDataGrid == null) return;

        var start = StartColumnIndex;
        var end = ColumnCount > 0
            ? Math.Min(start + ColumnCount, SourceDataGrid.Columns.Count)
            : SourceDataGrid.Columns.Count;

        for (var i = start; i < end; i++)
            UpdateColumnWidth(i - start, i);
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
        _startIndexSub?.Dispose();
        _columnCountSub?.Dispose();
        AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;

        base.OnDetaching();
    }
}