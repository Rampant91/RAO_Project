using Avalonia;
using Avalonia.Controls;
using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;

namespace Client_App.Behaviors.TableHeader;

/// <summary>
/// Ширины кастомной шапки относительно AvaloniaDataGrid.
/// </summary>
public class TableHeaderColumnWidthSyncBehavior : Behavior<Grid>
{
    private IDisposable? _startIndexSub;
    private IDisposable? _columnCountSub;
    private double[] _lastAppliedWidths = Array.Empty<double>();
    private bool? _lastHasHorizontalScroll;
    private int _lastFrozenColumnCount = -1;

    public static readonly AttachedProperty<AvaloniaDataGrid?> SourceDataGridProperty =
        AvaloniaProperty.RegisterAttached<TableHeaderColumnWidthSyncBehavior, Grid, AvaloniaDataGrid?>("SourceDataGrid");

    public AvaloniaDataGrid? SourceDataGrid
    {
        get => GetValue(SourceDataGridProperty);
        set => SetValue(SourceDataGridProperty, value);
    }

    public static readonly AttachedProperty<int> StartColumnIndexProperty =
        AvaloniaProperty.RegisterAttached<TableHeaderColumnWidthSyncBehavior, Grid, int>("StartColumnIndex", 0);

    public int StartColumnIndex
    {
        get => GetValue(StartColumnIndexProperty);
        set => SetValue(StartColumnIndexProperty, value);
    }

    public static readonly AttachedProperty<int> ColumnCountProperty =
        AvaloniaProperty.RegisterAttached<TableHeaderColumnWidthSyncBehavior, Grid, int>("ColumnCount", 0);

    public int ColumnCount
    {
        get => GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;

        _startIndexSub = StartColumnIndexProperty.Changed
            .Where(e => ReferenceEquals(e.Sender, this))
            .Subscribe(_ =>
            {
                if (SourceDataGrid is not null)
                    TableHeaderDataGridSync.RequestSync(SourceDataGrid, force: true);
            });
        _columnCountSub = ColumnCountProperty.Changed
            .Where(e => ReferenceEquals(e.Sender, this))
            .Subscribe(_ =>
            {
                if (SourceDataGrid is not null)
                    TableHeaderDataGridSync.RequestSync(SourceDataGrid, force: true);
            });
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        SourceDataGrid ??= AssociatedObject.GetVisualAncestors()
            .OfType<AvaloniaDataGrid>()
            .FirstOrDefault();

        if (SourceDataGrid != null)
            Initialize();
    }

    private void Initialize()
    {
        if (AssociatedObject == null || SourceDataGrid == null) return;

        ((INotifyCollectionChanged)SourceDataGrid.Columns).CollectionChanged += OnColumnsChanged;

        EnsureColumnDefinitions();
        TableHeaderDataGridSync.RegisterWidthBehavior(SourceDataGrid, this);
    }

    private void OnColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        EnsureColumnDefinitions();
        if (SourceDataGrid is not null)
            TableHeaderDataGridSync.RequestSync(SourceDataGrid, force: true);
    }

    /// <summary>Вызывается координатором TableHeaderDataGridSync.</summary>
    internal void SyncWidths(TableHeaderLayoutMetrics metrics, bool force = false, int changedColumnIndex = -1)
    {
        if (AssociatedObject == null || SourceDataGrid == null) return;

        EnsureColumnDefinitions();

        var start = StartColumnIndex;
        var end = ColumnCount > 0
            ? Math.Min(start + ColumnCount, SourceDataGrid.Columns.Count)
            : SourceDataGrid.Columns.Count;

        var neededCount = end - start;
        EnsureLastAppliedCache(neededCount);

        if (force
            || _lastHasHorizontalScroll != metrics.HasHorizontalScroll
            || _lastFrozenColumnCount != metrics.FrozenColumnCount)
        {
            _lastHasHorizontalScroll = metrics.HasHorizontalScroll;
            _lastFrozenColumnCount = metrics.FrozenColumnCount;
            Array.Fill(_lastAppliedWidths, double.NaN);
        }

        for (var i = start; i < end; i++)
        {
            if (changedColumnIndex >= 0 && i != changedColumnIndex)
                continue;

            var gridIndex = i - start;
            var displayWidth = SourceDataGrid.Columns[i].Width.DisplayValue;
            if (!double.IsFinite(displayWidth) || displayWidth <= 0) continue;

            var headerWidth = TableHeaderColumnWidth.FromDataGridDisplayWidth(displayWidth, metrics, i);
            if (!double.IsFinite(headerWidth) || headerWidth <= 0) continue;

            if (!force && Math.Abs(_lastAppliedWidths[gridIndex] - headerWidth) < 0.05)
                continue;

            _lastAppliedWidths[gridIndex] = headerWidth;
            AssociatedObject.ColumnDefinitions[gridIndex].Width = new GridLength(headerWidth);
        }
    }

    private void EnsureColumnDefinitions()
    {
        if (AssociatedObject == null || SourceDataGrid == null) return;

        var start = StartColumnIndex;
        var end = ColumnCount > 0
            ? Math.Min(start + ColumnCount, SourceDataGrid.Columns.Count)
            : SourceDataGrid.Columns.Count;

        var neededCount = end - start;

        while (AssociatedObject.ColumnDefinitions.Count < neededCount)
            AssociatedObject.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        for (var i = neededCount; i < AssociatedObject.ColumnDefinitions.Count; i++)
            AssociatedObject.ColumnDefinitions[i].Width = new GridLength(0);
    }

    private void EnsureLastAppliedCache(int neededCount)
    {
        if (_lastAppliedWidths.Length == neededCount) return;
        _lastAppliedWidths = new double[neededCount];
        Array.Fill(_lastAppliedWidths, double.NaN);
    }

    protected override void OnDetaching()
    {
        if (SourceDataGrid != null)
        {
            ((INotifyCollectionChanged)SourceDataGrid.Columns).CollectionChanged -= OnColumnsChanged;
            TableHeaderDataGridSync.UnregisterWidthBehavior(SourceDataGrid, this);
        }

        _startIndexSub?.Dispose();
        _columnCountSub?.Dispose();
        AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;

        base.OnDetaching();
    }
}
