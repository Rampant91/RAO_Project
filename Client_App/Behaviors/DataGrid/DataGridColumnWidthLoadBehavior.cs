using Avalonia;
using Avalonia.Controls;
using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using Client_App.Properties.ColumnWidthSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Behaviors.DataGrid;

/// <summary>
/// Восстанавливает сохранённые ширины колонок (в пикселях). Star-колонки не трогает — грид заполняет доступную ширину.
/// При подключении вызывает <see cref="DataGridMaxColumnWidthGuard.TryApplyReportTableCap"/> как запасной вариант.
/// Основной лимит MaxColumnWidth для таблиц отчётов — атрибут на DataGrid в XAML (см. datagrid-report-table-width.mdc).
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
    private HashSet<int> _starColumnIndices = new();
    private bool _columnsSubscribed;
    private bool _isApplying;
    private bool _loaded;
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
        PreserveStarLayout || AssociatedObject.Classes.Contains("main-list-grid");

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
                if (ShouldPreserveStarLayout() && _starColumnIndices.Contains(i))
                {
                    continue;
                }

                var width = ClampWidth(columns[i], _columnWidths[i]);
                if (!TryCreatePixelWidth(width, out var pixelWidth))
                {
                    continue;
                }

                columns[i].Width = pixelWidth;
            }
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

        _columnsSubscribed = false;
    }

    private void ColumnOnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (_isApplying || e.Property != DataGridColumn.WidthProperty)
        {
            return;
        }

        ScheduleSave();
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

    private void SaveWidths()
    {
        if (AssociatedObject is null || string.IsNullOrWhiteSpace(FormNum) || _isApplying)
        {
            return;
        }

        var columns = AssociatedObject.Columns;
        _columnWidths.Clear();

        for (var i = 0; i < columns.Count; i++)
        {
            if (ShouldPreserveStarLayout() && _starColumnIndices.Contains(i))
            {
                _columnWidths.Add(0);
                continue;
            }

            var column = columns[i];
            var width = column.Width.IsAbsolute
                ? column.Width.Value
                : column.ActualWidth;

            _columnWidths.Add(ClampWidth(column, width));
        }

        ColumnSettingsManager.SaveSettings(_columnWidths, FormNum);
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
