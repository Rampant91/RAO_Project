using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;

namespace Client_App.Behaviors;

/// <summary>
/// Ширины колонок кастомной шапки относительно DataGrid.
/// Со скроллом (видим PART_HorizontalScrollbar): полный DisplayValue.
/// Без скролла: DisplayValue − 1px/scale на колонку (как 1.3.0.9).
/// </summary>
internal static class TableHeaderColumnWidth
{
    internal const string HScrollBarName = "PART_HorizontalScrollbar";
    public static double GetPixelDensityScale(Visual? visual)
    {
        if (visual is null) return 1.0;

        var window = visual.GetVisualAncestors().OfType<Window>().FirstOrDefault();
        if (window?.Screens is null)
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                window = desktop.MainWindow;
        }

        if (window?.Screens is null) return 1.0;

        try
        {
            var screen = window.Screens.ScreenFromWindow(window.PlatformImpl) ?? window.Screens.Primary;
            var scale = screen?.PixelDensity ?? 1.0;
            return scale > 0 ? scale : 1.0;
        }
        catch
        {
            return 1.0;
        }
    }

    public static double BorderCompensationPerColumn(Visual? visual)
        => 1.0 / GetPixelDensityScale(visual);

    /// <summary>
    /// Режим «нужен горизонтальный скролл» — только когда полоса DataGrid реально отображается (Auto).
    /// Не смотрим Extent/Viewport и Maximum: при «влезает» Maximum может быть &gt; 0 без полосы,
    /// и шапка ошибочно берёт полный DisplayValue (данные уезжают влево от шапки).
    /// </summary>
    public static bool HasHorizontalScroll(DataGrid? dataGrid)
    {
        if (dataGrid is null) return false;

        var scrollBar = dataGrid.GetVisualDescendants()
            .OfType<ScrollBar>()
            .FirstOrDefault(s => s.Name == HScrollBarName);

        // Шаблон DataGrid может ещё не быть в визуальном дереве при открытии окна.
        return scrollBar is { IsVisible: true };
    }

    /// <summary>
    /// DisplayValue колонки DataGrid → ширина ColumnDefinition шапки.
    /// </summary>
    public static double FromDataGridDisplayWidth(double displayValue, DataGrid? dataGrid, int _)
    {
        if (displayValue <= 0) return displayValue;

        if (HasHorizontalScroll(dataGrid))
            return displayValue;

        return displayValue - BorderCompensationPerColumn(dataGrid);
    }
}

public class ColumnWidthSyncBehavior : Behavior<Grid>
{
    private readonly Dictionary<int, IDisposable> _subscriptions = new();
    private IDisposable? _layoutSubscription;
    private IDisposable? _scrollStateSubscription;
    private IDisposable? _startIndexSub;
    private IDisposable? _columnCountSub;
    private bool? _lastHasHorizontalScroll;

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
            .Subscribe(_ => OnDataGridLayoutMetricsChanged());

        SubscribeToScrollStateChanges(SourceDataGrid);
    }

    private void SubscribeToScrollStateChanges(DataGrid dataGrid)
    {
        _scrollStateSubscription?.Dispose();

        var scrollBar = dataGrid.GetVisualDescendants()
            .OfType<ScrollBar>()
            .FirstOrDefault(s => s.Name == TableHeaderColumnWidth.HScrollBarName);

        if (scrollBar is null) return;

        _scrollStateSubscription = Observable.Merge(
                scrollBar.GetObservable(RangeBase.MaximumProperty).Select(_ => (object?)null),
                scrollBar.GetObservable(Visual.IsVisibleProperty).Select(_ => (object?)null))
            .Subscribe(_ => OnDataGridLayoutMetricsChanged());
    }

    private void OnDataGridLayoutMetricsChanged()
    {
        if (SourceDataGrid is null) return;

        var hasScroll = TableHeaderColumnWidth.HasHorizontalScroll(SourceDataGrid);
        if (_lastHasHorizontalScroll == hasScroll)
        {
            UpdateWidths();
            return;
        }

        _lastHasHorizontalScroll = hasScroll;
        UpdateWidths();
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

        var displayWidth = SourceDataGrid.Columns[dataGridColumnIndex].Width.DisplayValue;

        if (displayWidth > 0)
        {
            var headerWidth = TableHeaderColumnWidth.FromDataGridDisplayWidth(
                displayWidth, SourceDataGrid, dataGridColumnIndex);
            AssociatedObject!.ColumnDefinitions[gridColumnIndex].Width = new GridLength(headerWidth);
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
        _scrollStateSubscription?.Dispose();
        _startIndexSub?.Dispose();
        _columnCountSub?.Dispose();
        AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;

        base.OnDetaching();
    }
}