using Avalonia;
using Avalonia.Controls;
using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Client_App.Properties.ColumnWidthSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Behaviors.DataGrid;

/// <summary>
/// Сохраняет/восстанавливает ширины колонок в Config.json.
/// Для списков организаций (MainWindow.Orgs.*): Star-наименование снова Star после ресайза,
/// фиксированные (ОКПО и т.п.) — Absolute по ActualWidth. У отчётов restore не делаем;
/// средние колонки Absolute в XAML, Star только у комментария. На время drag Star
/// замораживается в Absolute — иначе каждый пиксель ресайза перемеривает все ячейки
/// (с данными это лаг; без строк измерять нечего).
/// </summary>
public class DataGridColumnWidthLoadBehavior : Behavior<AvaloniaDataGrid>
{
    public static readonly StyledProperty<string?> FormNumProperty =
        AvaloniaProperty.Register<DataGridColumnWidthLoadBehavior, string?>(nameof(FormNum));

    public static readonly StyledProperty<bool> PreserveStarLayoutProperty =
        AvaloniaProperty.Register<DataGridColumnWidthLoadBehavior, bool>(nameof(PreserveStarLayout), false);

    public string? FormNum
    {
        get => GetValue(FormNumProperty);
        set => SetValue(FormNumProperty, value);
    }

    public bool PreserveStarLayout
    {
        get => GetValue(PreserveStarLayoutProperty);
        set => SetValue(PreserveStarLayoutProperty, value);
    }

    private readonly List<double> _columnWidths = new();
    private readonly List<double> _snapshotActualWidths = new();
    private HashSet<int> _starColumnIndices = new();
    private int[] _starIndicesFrozenThisDrag = [];
    private bool _columnsSubscribed;
    private bool _isApplying;
    private bool _loaded;
    private bool _pointerPressed;
    private DispatcherTimer? _saveTimer;

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null)
        {
            return;
        }

        DataGridMaxColumnWidthGuard.TryApplyReportTableCap(AssociatedObject);
        AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;
        AssociatedObject.DetachedFromVisualTree += OnDetachedFromVisualTree;
        AssociatedObject.SizeChanged += OnSizeChanged;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        CaptureStarColumns();
        SubscribeToColumnWidthChanges();
        Dispatcher.UIThread.Post(ApplyWhenReady, DispatcherPriority.Loaded);
    }

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        SaveWidths();
        UnsubscribeFromColumnWidthChanges();
        _saveTimer?.Stop();
        _loaded = false;
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e) => ApplyWhenReady();

    private void CaptureStarColumns()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        _starColumnIndices = AssociatedObject.Columns
            .Select((column, index) => (index, column.Width.IsStar))
            .Where(pair => pair.Item2)
            .Select(pair => pair.index)
            .ToHashSet();
    }

    private bool ShouldPreserveStarLayout() =>
        DataGridColumnWidthPersistence.ShouldPreserveStarLayout(PreserveStarLayout, FormNum);

    private void ApplyWhenReady()
    {
        if (_loaded || AssociatedObject is null)
        {
            return;
        }

        var available = AssociatedObject.Bounds.Width;
        if (!double.IsFinite(available) || available <= 0)
        {
            return;
        }

        ApplySavedWidths();
        _loaded = true;
        Dispatcher.UIThread.Post(CaptureActualWidthSnapshot, DispatcherPriority.Render);
    }

    private void ApplySavedWidths()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        _columnWidths.Clear();
        _columnWidths.AddRange(ColumnSettingsManager.LoadSettings(FormNum ?? string.Empty));

        _isApplying = true;
        try
        {
            var columns = AssociatedObject.Columns;

            for (var i = 0; i < _columnWidths.Count && i < columns.Count; i++)
            {
                // Star flex columns (org name / report comments): always keep XAML Star.
                if (_starColumnIndices.Contains(i))
                {
                    continue;
                }

                var saved = _columnWidths[i];
                if (DataGridColumnWidthPersistence.ShouldKeepXamlWidth(saved))
                {
                    continue;
                }

                var width = ClampWidth(columns[i], saved);
                if (!TryCreatePixelWidth(width, out var pixelWidth))
                {
                    continue;
                }

                columns[i].Width = pixelWidth;
            }

            RestoreFlexStarColumns(columns);
        }
        finally
        {
            _isApplying = false;
        }
    }

    protected override void OnDetaching()
    {
        SaveWidths();
        UnsubscribeFromColumnWidthChanges();
        _saveTimer?.Stop();
        _saveTimer = null;
        if (AssociatedObject is not null)
        {
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;
            AssociatedObject.DetachedFromVisualTree -= OnDetachedFromVisualTree;
            AssociatedObject.SizeChanged -= OnSizeChanged;
        }

        base.OnDetaching();
    }

    private void SubscribeToColumnWidthChanges()
    {
        if (_columnsSubscribed || AssociatedObject is null)
        {
            return;
        }

        foreach (var column in AssociatedObject.Columns)
        {
            column.PropertyChanged += ColumnOnPropertyChanged;
        }

        AssociatedObject.LayoutUpdated += OnLayoutUpdated;
        AssociatedObject.AddHandler(
            InputElement.PointerPressedEvent,
            OnPointerPressed,
            RoutingStrategies.Bubble,
            handledEventsToo: true);
        AssociatedObject.AddHandler(
            InputElement.PointerReleasedEvent,
            OnPointerReleased,
            RoutingStrategies.Bubble,
            handledEventsToo: true);
        AssociatedObject.AddHandler(
            InputElement.PointerCaptureLostEvent,
            OnPointerCaptureLost,
            RoutingStrategies.Bubble,
            handledEventsToo: true);

        _columnsSubscribed = true;
    }

    private void UnsubscribeFromColumnWidthChanges()
    {
        if (!_columnsSubscribed || AssociatedObject is null)
        {
            return;
        }

        foreach (var column in AssociatedObject.Columns)
        {
            column.PropertyChanged -= ColumnOnPropertyChanged;
        }

        AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
        AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, OnPointerPressed);
        AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, OnPointerReleased);
        AssociatedObject.RemoveHandler(InputElement.PointerCaptureLostEvent, OnPointerCaptureLost);
        _pointerPressed = false;
        _starIndicesFrozenThisDrag = [];

        _columnsSubscribed = false;
    }

    private void ColumnOnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (_isApplying || e.Property != DataGridColumn.WidthProperty)
        {
            return;
        }

        // During drag WidthProperty fires often; persist after release (see OnPointerReleased).
        if (_pointerPressed)
        {
            return;
        }

        ScheduleSave();
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(AssociatedObject).Properties.IsLeftButtonPressed)
        {
            return;
        }

        // Only while dragging a column resize thumb — not on every cell click.
        if (e.Source is Thumb || (e.Source as Visual)?.FindAncestorOfType<Thumb>() is not null)
        {
            _pointerPressed = true;
            FreezeStarColumnsForDrag();
        }
    }

    private void OnPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e) =>
        EndColumnResizeDrag();

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e) =>
        EndColumnResizeDrag();

    /// <summary>
    /// Star (and leftover redistribution) remeasures every visible cell each drag pixel.
    /// With rows that is the lag; freeze Stars to Absolute for the drag, restore after.
    /// </summary>
    private void FreezeStarColumnsForDrag()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var columns = AssociatedObject.Columns;
        var frozen = new List<int>();
        _isApplying = true;
        try
        {
            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                if (!column.Width.IsStar)
                {
                    continue;
                }

                var actualWidth = column.ActualWidth;
                if (!double.IsFinite(actualWidth) || actualWidth <= 0)
                {
                    continue;
                }

                frozen.Add(i);
                column.Width = new DataGridLength(ClampWidth(column, actualWidth));
            }
        }
        finally
        {
            _isApplying = false;
        }

        _starIndicesFrozenThisDrag = frozen.ToArray();
    }

    private void RestoreStarColumnsAfterDrag()
    {
        if (AssociatedObject is null || _starIndicesFrozenThisDrag.Length == 0)
        {
            return;
        }

        var columns = AssociatedObject.Columns;
        _isApplying = true;
        try
        {
            foreach (var index in _starIndicesFrozenThisDrag)
            {
                if (index >= 0 && index < columns.Count)
                {
                    columns[index].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                }
            }
        }
        finally
        {
            _isApplying = false;
        }

        _starIndicesFrozenThisDrag = [];
    }

    private void EndColumnResizeDrag()
    {
        if (!_pointerPressed)
        {
            return;
        }

        _pointerPressed = false;

        if (_isApplying || !_loaded)
        {
            RestoreStarColumnsAfterDrag();
            return;
        }

        if (ShouldPreserveStarLayout())
        {
            CommitFixedColumnsAndRestoreStars();
            _starIndicesFrozenThisDrag = [];
        }
        else
        {
            RestoreStarColumnsAfterDrag();
            if (AssociatedObject is not null)
            {
                _isApplying = true;
                try
                {
                    RestoreFlexStarColumns(AssociatedObject.Columns);
                }
                finally
                {
                    _isApplying = false;
                }
            }
        }

        if (HasActualWidthSnapshotChanged())
        {
            ScheduleSave();
        }
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (_isApplying || !_loaded || _pointerPressed)
        {
            return;
        }

        if (HasActualWidthSnapshotChanged())
        {
            ScheduleSave();
        }
    }

    private void ScheduleSave()
    {
        _saveTimer ??= new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
        _saveTimer.Stop();
        _saveTimer.Tick -= SaveTimerOnTick;
        _saveTimer.Tick += SaveTimerOnTick;
        _saveTimer.Start();
    }

    private void SaveTimerOnTick(object? sender, EventArgs e)
    {
        _saveTimer?.Stop();
        SaveWidths();
    }

    /// <summary>
    /// Закрепляет видимую ширину фиксированных колонок и снова делает Star-колонки Star,
    /// чтобы остаток не уходил в последнюю Absolute (ОКПО).
    /// </summary>
    private void CommitFixedColumnsAndRestoreStars()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var columns = AssociatedObject.Columns;
        _isApplying = true;
        try
        {
            for (var i = 0; i < columns.Count; i++)
            {
                if (_starColumnIndices.Contains(i))
                {
                    continue;
                }

                var aw = columns[i].ActualWidth;
                if (!double.IsFinite(aw) || aw <= 0)
                {
                    continue;
                }

                columns[i].Width = new DataGridLength(ClampWidth(columns[i], aw));
            }

            RestoreFlexStarColumns(columns);
        }
        finally
        {
            _isApplying = false;
        }
    }

    private void RestoreFlexStarColumns(IList<DataGridColumn> columns)
    {
        foreach (var index in _starColumnIndices)
        {
            if (index < 0 || index >= columns.Count)
            {
                continue;
            }

            columns[index].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }
    }

    private void SaveWidths()
    {
        if (AssociatedObject is null || string.IsNullOrWhiteSpace(FormNum) || _isApplying)
        {
            return;
        }

        if (!_loaded || AssociatedObject.Bounds.Width <= 0)
        {
            return;
        }

        var columns = AssociatedObject.Columns;
        _columnWidths.Clear();

        for (var i = 0; i < columns.Count; i++)
        {
            var column = columns[i];

            // Star flex (name / comments): never persist Absolute — always takes the leftover.
            if (_starColumnIndices.Contains(i))
            {
                _columnWidths.Add(0);
                continue;
            }

            var width = DataGridColumnWidthPersistence.GetPersistableWidth(
                column.ActualWidth,
                column.Width.IsAbsolute,
                column.Width.IsStar,
                column.Width.Value,
                preserveStarLayout: false);

            if (width <= 0)
            {
                _columnWidths.Add(0);
                continue;
            }

            _columnWidths.Add(ClampWidth(column, width));
        }

        ColumnSettingsManager.SaveSettings(_columnWidths, FormNum);
        CaptureActualWidthSnapshot();
    }

    private void CaptureActualWidthSnapshot()
    {
        _snapshotActualWidths.Clear();
        if (AssociatedObject is null)
        {
            return;
        }

        foreach (var column in AssociatedObject.Columns)
        {
            _snapshotActualWidths.Add(column.ActualWidth);
        }
    }

    private bool HasActualWidthSnapshotChanged()
    {
        if (AssociatedObject is null)
        {
            return false;
        }

        var columns = AssociatedObject.Columns;
        if (_snapshotActualWidths.Count != columns.Count)
        {
            return true;
        }

        const double epsilon = 0.5;
        for (var i = 0; i < columns.Count; i++)
        {
            if (Math.Abs(columns[i].ActualWidth - _snapshotActualWidths[i]) > epsilon)
            {
                return true;
            }
        }

        return false;
    }

    private double ClampWidth(DataGridColumn column, double width) =>
        DataGridColumnWidthClamp.Clamp(
            width,
            column.MinWidth,
            column.MaxWidth,
            AssociatedObject?.MinColumnWidth ?? 0,
            DataGridColumnWidthClamp.FallbackMax);

    private static bool TryCreatePixelWidth(double width, out DataGridLength pixelWidth)
    {
        if (!double.IsFinite(width) || width <= 0)
        {
            pixelWidth = default;
            return false;
        }

        pixelWidth = new DataGridLength(width);
        return true;
    }
}
