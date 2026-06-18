using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Behaviors;

/// <summary>
/// Изменение ширины колонок <see cref="DataGrid"/> перетаскиванием границ кастомной шапки.
/// <para>
/// Вешается на <see cref="Grid"/> шапки (на Form_11 — отдельные экземпляры для scroll / fixed / npp).
/// Ширина пишется в DataGrid.Columns[i].Width; синхронизация шапки — через
/// <see cref="TableHeaderDataGridSync.SyncLiveColumnWidth"/>.
/// </para>
/// </summary>
public class TableHeaderColumnResizeBehavior : Behavior<Grid>
{
    #region Constants

    /// <summary>Ширина зоны захвата по горизонтали: ±3 px от линии (≈6 px всего).</summary>
    private const double HorizontalResizeHitThreshold = 6.0;

    /// <summary>Допуск по вертикали при hit-test границ в многоуровневой шапке.</summary>
    private const double VerticalResizeHitThreshold = 5.0;

    /// <summary>Минимальное изменение ширины (px) для записи в DataGrid и sync шапки.</summary>
    private const double LiveResizeWidthThreshold = 1.0;

    #endregion

    #region Attached properties

    public static readonly AttachedProperty<DataGrid?> SourceDataGridProperty =
        AvaloniaProperty.RegisterAttached<TableHeaderColumnResizeBehavior, Grid, DataGrid?>("SourceDataGrid");

    /// <summary>DataGrid, колонки которого меняются при drag.</summary>
    public DataGrid? SourceDataGrid
    {
        get => GetValue(SourceDataGridProperty);
        set => SetValue(SourceDataGridProperty, value);
    }

    public static readonly AttachedProperty<int> StartColumnIndexProperty =
        AvaloniaProperty.RegisterAttached<TableHeaderColumnResizeBehavior, Grid, int>("StartColumnIndex", 0);

    /// <summary>
    /// Индекс первой колонки DataGrid, отображаемой в этом Grid шапки
    /// (0 — № п/п, 1 — первая скроллируемая на Form_11 и т.д.).
    /// </summary>
    public int StartColumnIndex
    {
        get => GetValue(StartColumnIndexProperty);
        set => SetValue(StartColumnIndexProperty, value);
    }

    public static readonly AttachedProperty<int> ColumnCountProperty =
        AvaloniaProperty.RegisterAttached<TableHeaderColumnResizeBehavior, Grid, int>("ColumnCount", 0);

    /// <summary>
    /// Число колонок шапки в этом Grid. 0 — все колонки от <see cref="StartColumnIndex"/> до конца.
    /// </summary>
    public int ColumnCount
    {
        get => GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    public static readonly AttachedProperty<bool> IncludeTrailingBoundaryProperty =
        AvaloniaProperty.RegisterAttached<TableHeaderColumnResizeBehavior, Grid, bool>("IncludeTrailingBoundary", false);

    /// <summary>
    /// Разрешить resize по правой границе последней колонки секции
    /// (нужно для единственной колонки «№ п/п» в nppScrollableHeader).
    /// </summary>
    public bool IncludeTrailingBoundary
    {
        get => GetValue(IncludeTrailingBoundaryProperty);
        set => SetValue(IncludeTrailingBoundaryProperty, value);
    }

    public static readonly AttachedProperty<bool> IncludeLeadingBoundaryProperty =
        AvaloniaProperty.RegisterAttached<TableHeaderColumnResizeBehavior, Grid, bool>("IncludeLeadingBoundary", false);

    /// <summary>
    /// Разрешить resize по левой границе первой колонки секции
    /// (граница с предыдущей frozen-колонкой, напр. № п/п | код).
    /// </summary>
    public bool IncludeLeadingBoundary
    {
        get => GetValue(IncludeLeadingBoundaryProperty);
        set => SetValue(IncludeLeadingBoundaryProperty, value);
    }

    #endregion

    #region Fields

    private Border? _hitOverlay;
    private int _draggingDataGridColumnIndex = -1;
    private double _dragStartX;
    private double _dragStartWidth;
    private IPointer? _capturedPointer;

    private double[]? _cachedColumnOffsets;
    private List<(int Row, double Top, double Bottom)>? _cachedRowBands;
    private int _cachedOffsetsColumnCount = -1;
    private int _cachedRowBandsVisibleCount = -1;

    #endregion

    #region Behavior lifecycle

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null) return;

        AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;
        AssociatedObject.DetachedFromVisualTree += OnDetachedFromVisualTree;
        AssociatedObject.LayoutUpdated += OnHeaderLayoutUpdated;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;
            AssociatedObject.DetachedFromVisualTree -= OnDetachedFromVisualTree;
            AssociatedObject.LayoutUpdated -= OnHeaderLayoutUpdated;
        }

        RemoveHitOverlay();
        FinishResize();
        base.OnDetaching();
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        SourceDataGrid ??= AssociatedObject?.GetVisualAncestors().OfType<DataGrid>().FirstOrDefault();
        EnsureHitOverlay();
    }

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        => RemoveHitOverlay();

    private void OnHeaderLayoutUpdated(object? sender, EventArgs e)
        => InvalidateHitTestCache();

    #endregion

    #region Hit-overlay

    /// <summary>
    /// Прозрачный слой поверх ячеек шапки для pointer events без перехвата кликов по контенту.
    /// </summary>
    private void EnsureHitOverlay()
    {
        if (AssociatedObject is null || _hitOverlay is not null) return;

        var rowSpan = Math.Max(1, AssociatedObject.RowDefinitions.Count);
        var colSpan = Math.Max(1, AssociatedObject.ColumnDefinitions.Count);

        _hitOverlay = new Border
        {
            Background = Brushes.Transparent,
            IsHitTestVisible = true,
            ZIndex = 1000,
            // Не наследовать Grid.tableHeader Border (1px) — иначе при frozen=2 видна
            // вертикальная линия на стыке «код | дата» через «Сведения об операции».
            BorderThickness = new Thickness(0),
            Margin = new Thickness(0)
        };

        Grid.SetRow(_hitOverlay, 0);
        Grid.SetColumn(_hitOverlay, 0);
        Grid.SetRowSpan(_hitOverlay, rowSpan);
        Grid.SetColumnSpan(_hitOverlay, colSpan);

        _hitOverlay.PointerMoved += OnPointerMoved;
        _hitOverlay.PointerPressed += OnPointerPressed;
        _hitOverlay.PointerReleased += OnPointerReleased;
        _hitOverlay.PointerCaptureLost += OnPointerCaptureLost;

        AssociatedObject.Children.Add(_hitOverlay);
    }

    private void RemoveHitOverlay()
    {
        if (_hitOverlay is null || AssociatedObject is null) return;

        _hitOverlay.PointerMoved -= OnPointerMoved;
        _hitOverlay.PointerPressed -= OnPointerPressed;
        _hitOverlay.PointerReleased -= OnPointerReleased;
        _hitOverlay.PointerCaptureLost -= OnPointerCaptureLost;

        AssociatedObject.Children.Remove(_hitOverlay);
        _hitOverlay = null;
    }

    #endregion

    #region Pointer handling

    /// <summary>
    /// Система координат для delta при drag: DataGrid, а не локальный Grid шапки
    /// (шапка сдвигается при frozen/scroll, иначе курсор «упирается»).
    /// </summary>
    private Visual? DragReferenceVisual =>
        SourceDataGrid as Visual ?? AssociatedObject?.GetVisualRoot() as Visual;

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (AssociatedObject is null || _hitOverlay is null) return;

        var point = e.GetPosition(AssociatedObject);

        if (_draggingDataGridColumnIndex >= 0 && SourceDataGrid is not null)
        {
            ApplyLiveResize(e);
            return;
        }

        var columnIndex = FindResizeTargetColumnIndex(point);
        _hitOverlay.Cursor = columnIndex >= 0
            ? new Cursor(StandardCursorType.SizeWestEast)
            : null;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (AssociatedObject is null || SourceDataGrid is null || _hitOverlay is null) return;
        if (!e.GetCurrentPoint(_hitOverlay).Properties.IsLeftButtonPressed) return;

        var reference = DragReferenceVisual;
        if (reference is null) return;

        var point = e.GetPosition(AssociatedObject);
        var columnIndex = FindResizeTargetColumnIndex(point);
        if (columnIndex < 0 || columnIndex >= SourceDataGrid.Columns.Count) return;

        _draggingDataGridColumnIndex = columnIndex;
        _dragStartX = e.GetPosition(reference).X;
        _dragStartWidth = SourceDataGrid.Columns[columnIndex].Width.DisplayValue;
        _capturedPointer = e.Pointer;
        _capturedPointer.Capture(_hitOverlay);
        TableHeaderDataGridSync.BeginLiveColumnResize(SourceDataGrid, columnIndex);
        e.Handled = true;
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        => FinishResize();

    private void OnPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
        => FinishResize();

    private void ApplyLiveResize(PointerEventArgs e)
    {
        if (SourceDataGrid is null) return;

        var reference = DragReferenceVisual;
        if (reference is null) return;

        var delta = e.GetPosition(reference).X - _dragStartX;
        var targetWidth = ClampToColumnLimits(SourceDataGrid, _dragStartWidth + delta);
        var currentWidth = SourceDataGrid.Columns[_draggingDataGridColumnIndex].Width.DisplayValue;

        if (Math.Abs(targetWidth - currentWidth) < LiveResizeWidthThreshold)
            return;

        SourceDataGrid.Columns[_draggingDataGridColumnIndex].Width = new DataGridLength(targetWidth);
        TableHeaderDataGridSync.SyncLiveColumnWidth(SourceDataGrid, _draggingDataGridColumnIndex, targetWidth);
    }

    private void FinishResize()
    {
        if (_draggingDataGridColumnIndex < 0) return;

        _capturedPointer?.Capture(null);
        _draggingDataGridColumnIndex = -1;
        _capturedPointer = null;

        if (SourceDataGrid is not null)
            TableHeaderDataGridSync.EndLiveColumnResize(SourceDataGrid);
    }

    #endregion

    #region Hit-test

    /// <summary>
    /// Индекс колонки DataGrid для resize по позиции курсора, или -1.
    /// </summary>
    private int FindResizeTargetColumnIndex(Point pointer)
    {
        if (AssociatedObject is null || SourceDataGrid is null) return -1;

        var start = Math.Max(0, StartColumnIndex);
        var end = ColumnCount > 0
            ? Math.Min(start + ColumnCount, SourceDataGrid.Columns.Count)
            : SourceDataGrid.Columns.Count;

        var visibleCount = end - start;
        if (visibleCount <= 0) return -1;

        var bestTop = double.NegativeInfinity;
        var bestDistance = double.MaxValue;
        var bestColumnIndex = -1;

        ConsiderBorderBoundaries(pointer.X, pointer.Y, start, visibleCount, ref bestDistance, ref bestTop, ref bestColumnIndex);
        ConsiderColumnGridBoundaries(pointer.X, pointer.Y, start, visibleCount, ref bestDistance, ref bestTop, ref bestColumnIndex);

        return bestColumnIndex;
    }

    private IEnumerable<Border> GetHeaderBorders()
    {
        if (AssociatedObject is null) yield break;

        foreach (var border in AssociatedObject.Children.OfType<Border>())
        {
            if (ReferenceEquals(border, _hitOverlay)) continue;
            yield return border;
        }
    }

    /// <summary>
    /// Границы из <see cref="Border"/> row 0 (групповые заголовки с ColumnSpan).
    /// Row 1 обрабатывается через <see cref="ConsiderColumnGridBoundaries"/>.
    /// </summary>
    private void ConsiderBorderBoundaries(
        double pointerX,
        double pointerY,
        int start,
        int visibleCount,
        ref double bestDistance,
        ref double bestTop,
        ref int bestColumnIndex)
    {
        foreach (var border in GetHeaderBorders())
        {
            if (!border.IsVisible) continue;

            var row = Grid.GetRow(border);
            if (row != 0) continue;
            if (border.Child is null) continue;

            var col = Grid.GetColumn(border);
            var colSpan = Math.Max(1, Grid.GetColumnSpan(border));
            if (col < 0 || col >= visibleCount) continue;

            var leftOffset = col;
            var rightOffset = Math.Min(col + colSpan, visibleCount);
            if (leftOffset >= rightOffset) continue;

            var origin = border.TranslatePoint(default, AssociatedObject);
            if (origin is null) continue;

            var leftX = origin.Value.X;
            var rightX = leftX + border.Bounds.Width;
            var top = origin.Value.Y;
            var bottom = top + border.Bounds.Height;

            if (pointerY < top - VerticalResizeHitThreshold || pointerY > bottom + VerticalResizeHitThreshold)
                continue;

            if (border.BorderThickness.Right > 0)
            {
                ConsiderCandidate(
                    Math.Abs(pointerX - rightX),
                    top,
                    start + rightOffset - 1,
                    ref bestDistance,
                    ref bestTop,
                    ref bestColumnIndex);
            }

            if (border.BorderThickness.Left > 0 && (leftOffset > 0 || IncludeLeadingBoundary))
            {
                var columnIndex = leftOffset > 0
                    ? start + leftOffset - 1
                    : start - 1;

                if (columnIndex >= 0)
                {
                    ConsiderCandidate(
                        Math.Abs(pointerX - leftX),
                        top,
                        columnIndex,
                        ref bestDistance,
                        ref bestTop,
                        ref bestColumnIndex);
                }
            }
        }
    }

    /// <summary>
    /// Вертикальные границы по <see cref="Grid.ColumnDefinitions"/> (row 1 — подзаголовки с номерами колонок).
    /// </summary>
    private void ConsiderColumnGridBoundaries(
        double pointerX,
        double pointerY,
        int start,
        int visibleCount,
        ref double bestDistance,
        ref double bestTop,
        ref int bestColumnIndex)
    {
        if (AssociatedObject is null) return;

        var columnCount = Math.Min(visibleCount, AssociatedObject.ColumnDefinitions.Count);
        if (columnCount <= 0) return;

        var offsets = GetColumnOffsets(columnCount);
        var rowBands = GetRowBands(visibleCount);

        foreach (var (row, top, bottom) in rowBands)
        {
            if (pointerY < top - VerticalResizeHitThreshold || pointerY > bottom + VerticalResizeHitThreshold)
                continue;

            if (row == 1)
            {
                for (var localIndex = 0; localIndex < columnCount - 1; localIndex++)
                {
                    ConsiderCandidate(
                        Math.Abs(pointerX - offsets[localIndex + 1]),
                        top,
                        start + localIndex,
                        ref bestDistance,
                        ref bestTop,
                        ref bestColumnIndex);
                }
            }
        }

        if (!TryGetRowBand(rowBands, 1, out var rowTop, out var rowBottom)
            && !TryGetRowBand(rowBands, 0, out rowTop, out rowBottom))
            return;

        if (pointerY < rowTop - VerticalResizeHitThreshold || pointerY > rowBottom + VerticalResizeHitThreshold)
            return;

        if (IncludeLeadingBoundary && start > 0)
        {
            ConsiderCandidate(
                Math.Abs(pointerX - offsets[0]),
                rowTop,
                start - 1,
                ref bestDistance,
                ref bestTop,
                ref bestColumnIndex);
        }

        if (IncludeTrailingBoundary)
        {
            ConsiderCandidate(
                Math.Abs(pointerX - offsets[columnCount]),
                rowTop,
                start + columnCount - 1,
                ref bestDistance,
                ref bestTop,
                ref bestColumnIndex);
        }
    }

    /// <summary>
    /// Выбор ближайшей границы; при равной дистанции приоритет у более нижней строки шапки.
    /// </summary>
    private static void ConsiderCandidate(
        double distance,
        double top,
        int columnIndex,
        ref double bestDistance,
        ref double bestTop,
        ref int bestColumnIndex)
    {
        if (distance > HorizontalResizeHitThreshold) return;
        if (columnIndex < 0) return;

        if (distance < bestDistance - 0.05
            || (Math.Abs(distance - bestDistance) < 0.05 && top > bestTop))
        {
            bestDistance = distance;
            bestTop = top;
            bestColumnIndex = columnIndex;
        }
    }

    #endregion

    #region Hit-test geometry cache

    private void InvalidateHitTestCache()
    {
        _cachedColumnOffsets = null;
        _cachedRowBands = null;
        _cachedOffsetsColumnCount = -1;
        _cachedRowBandsVisibleCount = -1;
    }

    private double[] GetColumnOffsets(int columnCount)
    {
        if (_cachedColumnOffsets is not null
            && _cachedOffsetsColumnCount == columnCount
            && _cachedColumnOffsets.Length == columnCount + 1)
            return _cachedColumnOffsets;

        _cachedColumnOffsets = BuildColumnOffsets(columnCount);
        _cachedOffsetsColumnCount = columnCount;
        return _cachedColumnOffsets;
    }

    private List<(int Row, double Top, double Bottom)> GetRowBands(int visibleCount)
    {
        if (_cachedRowBands is not null && _cachedRowBandsVisibleCount == visibleCount)
            return _cachedRowBands;

        _cachedRowBands = BuildRowBands(visibleCount);
        _cachedRowBandsVisibleCount = visibleCount;
        return _cachedRowBands;
    }

    private List<(int Row, double Top, double Bottom)> BuildRowBands(int visibleCount)
    {
        var bands = new Dictionary<int, (double Top, double Bottom)>();

        foreach (var border in GetHeaderBorders())
        {
            if (!border.IsVisible) continue;

            var row = Grid.GetRow(border);
            if (row == 0 && border.Child is null) continue;

            var col = Grid.GetColumn(border);
            if (col < 0 || col >= visibleCount) continue;

            var origin = border.TranslatePoint(default, AssociatedObject);
            if (origin is null) continue;

            var top = origin.Value.Y;
            var bottom = top + border.Bounds.Height;

            if (!bands.TryGetValue(row, out var band))
                bands[row] = (top, bottom);
            else
                bands[row] = (Math.Min(band.Top, top), Math.Max(band.Bottom, bottom));
        }

        return bands.Select(kv => (kv.Key, kv.Value.Top, kv.Value.Bottom)).ToList();
    }

    private static bool TryGetRowBand(
        List<(int Row, double Top, double Bottom)> rowBands,
        int row,
        out double top,
        out double bottom)
    {
        foreach (var band in rowBands)
        {
            if (band.Row != row) continue;
            top = band.Top;
            bottom = band.Bottom;
            return true;
        }

        top = bottom = 0;
        return false;
    }

    private double[] BuildColumnOffsets(int columnCount)
    {
        var offsets = new double[columnCount + 1];
        if (AssociatedObject is null) return offsets;

        for (var i = 0; i < columnCount; i++)
            offsets[i + 1] = offsets[i] + AssociatedObject.ColumnDefinitions[i].ActualWidth;

        return offsets;
    }

    #endregion

    #region Utilities

    private static double ClampToColumnLimits(DataGrid dataGrid, double width)
    {
        var min = dataGrid.MinColumnWidth;
        var max = dataGrid.MaxColumnWidth;

        if (double.IsNaN(min) || min <= 0) min = 20;
        if (double.IsNaN(max) || max <= 0 || double.IsPositiveInfinity(max)) max = double.MaxValue;

        return Math.Clamp(width, min, max);
    }

    #endregion
}
