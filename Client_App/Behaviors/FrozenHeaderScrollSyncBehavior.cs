using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System;
using System.Linq;

namespace Client_App.Behaviors;

/// <summary>
/// Синхронизирует горизонтальный скролл кастомной шапки с DataGrid.
/// Обновления координируются через TableHeaderDataGridSync (без LayoutUpdated на каждый кадр).
/// </summary>
public class FrozenHeaderScrollSyncBehavior : Behavior<Panel>
{
    public static readonly AttachedProperty<DataGrid?> SourceDataGridProperty =
        AvaloniaProperty.RegisterAttached<FrozenHeaderScrollSyncBehavior, Panel, DataGrid?>(
            "SourceDataGrid");

    public DataGrid? SourceDataGrid
    {
        get => GetValue(SourceDataGridProperty);
        set => SetValue(SourceDataGridProperty, value);
    }

    public static readonly StyledProperty<Border?> FixedGroupHeaderBorderProperty =
        AvaloniaProperty.Register<FrozenHeaderScrollSyncBehavior, Border?>(nameof(FixedGroupHeaderBorder));

    public Border? FixedGroupHeaderBorder
    {
        get => GetValue(FixedGroupHeaderBorderProperty);
        set => SetValue(FixedGroupHeaderBorderProperty, value);
    }

    public static readonly StyledProperty<Grid?> NppScrollableHeaderGridProperty =
        AvaloniaProperty.Register<FrozenHeaderScrollSyncBehavior, Grid?>(nameof(NppScrollableHeaderGrid));

    public Grid? NppScrollableHeaderGrid
    {
        get => GetValue(NppScrollableHeaderGridProperty);
        set => SetValue(NppScrollableHeaderGridProperty, value);
    }

    private TranslateTransform? _transform;
    private TranslateTransform? _nppTransform;
    private double _lastAppliedTotal = double.NaN;
    private double _lastNppWidth = double.NaN;
    private double _lastFixedBorderWidth = double.NaN;
    private bool _lastHadHorizontalScroll;

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is not null)
            AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;

        Dispatcher.UIThread.Post(TrySubscribe, DispatcherPriority.ApplicationIdle);
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        => TrySubscribe();

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;

        if (SourceDataGrid is not null)
            TableHeaderDataGridSync.UnregisterScrollBehavior(SourceDataGrid, this);

        base.OnDetaching();
    }

    private void TrySubscribe()
    {
        SourceDataGrid ??= AssociatedObject?
            .GetVisualAncestors()
            .OfType<DataGrid>()
            .FirstOrDefault(dg => dg.Name == "dataGrid");

        if (SourceDataGrid is null) return;

        TableHeaderDataGridSync.RegisterScrollBehavior(SourceDataGrid, this);
    }

    /// <summary>Вызывается координатором TableHeaderDataGridSync.</summary>
    internal void SyncScroll(TableHeaderLayoutMetrics metrics, double scrollOffset)
    {
        if (_lastHadHorizontalScroll != metrics.HasHorizontalScroll)
        {
            _lastHadHorizontalScroll = metrics.HasHorizontalScroll;
            _lastAppliedTotal = double.NaN;
        }

        var extraOffset = ComputeExtraFrozenOffset(metrics);
        var total = scrollOffset + extraOffset;

        var transformChanged = double.IsNaN(_lastAppliedTotal)
                               || Math.Abs(total - _lastAppliedTotal) >= 0.1;

        if (transformChanged)
        {
            _lastAppliedTotal = total;
            ApplyTransform(total, scrollOffset, metrics);
        }

        UpdateFixedGroupHeaderBorder(metrics, scrollOffset);
    }

    private void ApplyTransform(double totalOffset, double scrollOffset, TableHeaderLayoutMetrics metrics)
    {
        var innerGrid = AssociatedObject?.Children.OfType<Grid>().FirstOrDefault();
        if (innerGrid is null) return;

        if (_transform is null)
        {
            _transform = new TranslateTransform(0, 0);
            innerGrid.RenderTransform = _transform;
        }
        _transform.X = -totalOffset;

        if (NppScrollableHeaderGrid is null || SourceDataGrid?.Columns.Count is not > 0) return;

        if (_nppTransform is null)
        {
            _nppTransform = new TranslateTransform(0, 0);
            NppScrollableHeaderGrid.RenderTransform = _nppTransform;
        }
        _nppTransform.X = -scrollOffset;

        var w = SourceDataGrid.Columns[0].Width.DisplayValue;
        if (w <= 0) return;

        var headerWidth = TableHeaderColumnWidth.FromDataGridDisplayWidth(w, metrics, 0);
        if (Math.Abs(_lastNppWidth - headerWidth) < 0.05) return;

        _lastNppWidth = headerWidth;
        NppScrollableHeaderGrid.Width = headerWidth;
    }

    private void UpdateFixedGroupHeaderBorder(TableHeaderLayoutMetrics metrics, double scrollOffset)
    {
        if (FixedGroupHeaderBorder is null || SourceDataGrid is null) return;

        if (metrics.FrozenColumnCount != 2 || SourceDataGrid.Columns.Count <= 2)
        {
            ResetFixedGroupHeaderBorderIfNeeded();
            return;
        }

        var datWidth = SourceDataGrid.Columns[2].Width.DisplayValue;
        var kodWidth = SourceDataGrid.Columns[1].Width.DisplayValue;

        if (datWidth <= 0 || kodWidth <= 0)
        {
            ResetFixedGroupHeaderBorderIfNeeded();
            return;
        }

        var visibleDat = Math.Max(0.0, datWidth - scrollOffset);
        double targetWidth;
        HorizontalAlignment alignment;

        if (visibleDat > 0.5)
        {
            targetWidth = kodWidth + 1.0 + visibleDat;
            alignment = HorizontalAlignment.Left;
        }
        else
        {
            targetWidth = double.NaN;
            alignment = HorizontalAlignment.Stretch;
        }

        if (!double.IsNaN(targetWidth) && Math.Abs(_lastFixedBorderWidth - targetWidth) < 0.05
            && FixedGroupHeaderBorder.HorizontalAlignment == alignment)
            return;

        _lastFixedBorderWidth = targetWidth;
        FixedGroupHeaderBorder.Width = targetWidth;
        FixedGroupHeaderBorder.HorizontalAlignment = alignment;
        SetUniformBorderThickness(FixedGroupHeaderBorder, 1);
    }

    private static void SetUniformBorderThickness(Border border, double thickness)
    {
        var current = border.BorderThickness;
        if (Math.Abs(current.Left - thickness) < 0.01
            && Math.Abs(current.Top - thickness) < 0.01
            && Math.Abs(current.Right - thickness) < 0.01
            && Math.Abs(current.Bottom - thickness) < 0.01)
            return;

        border.BorderThickness = new Thickness(thickness);
    }

    private void ResetFixedGroupHeaderBorderIfNeeded()
    {
        if (FixedGroupHeaderBorder is null) return;
        if (double.IsNaN(_lastFixedBorderWidth) && FixedGroupHeaderBorder.Width is double.NaN) return;

        _lastFixedBorderWidth = double.NaN;
        FixedGroupHeaderBorder.Width = double.NaN;
        FixedGroupHeaderBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
        SetUniformBorderThickness(FixedGroupHeaderBorder, 1);
    }

    private double ComputeExtraFrozenOffset(TableHeaderLayoutMetrics metrics)
    {
        if (SourceDataGrid is null) return 0;

        var frozenCount = metrics.FrozenColumnCount;

        if (frozenCount == 0)
        {
            var nppWidth = SourceDataGrid.Columns.Count > 0
                ? SourceDataGrid.Columns[0].Width.DisplayValue
                : 0;
            return nppWidth > 0
                ? -TableHeaderColumnWidth.FromDataGridDisplayWidth(nppWidth, metrics, 0)
                : 0;
        }

        if (frozenCount <= 1) return 0;

        var total = 0.0;
        for (var i = 1; i < frozenCount && i < SourceDataGrid.Columns.Count; i++)
        {
            var w = SourceDataGrid.Columns[i].Width.DisplayValue;
            if (w > 0)
                total += TableHeaderColumnWidth.FromDataGridDisplayWidth(w, metrics, i);
        }

        return total;
    }
}
