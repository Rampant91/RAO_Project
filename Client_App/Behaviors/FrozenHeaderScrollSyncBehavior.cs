using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System;
using System.Diagnostics;
using System.Linq;

namespace Client_App.Behaviors;

/// <summary>
/// Синхронизирует горизонтальный скролл кастомной шапки с DataGrid.
///
/// AssociatedObject — Panel с ClipToBounds="True", содержащий Grid шапки.
/// Смещение: TranslateTransform.X = -offset на этом Grid (render-уровень, без re-layout).
/// Обнаружение: DispatcherTimer 16 мс — надёжнее любых событий в Avalonia 0.10.x.
/// Запуск: Dispatcher.Post в OnAttached — не зависит от порядка visual-tree и биндингов.
/// </summary>
public class FrozenHeaderScrollSyncBehavior : Behavior<Panel>
{
    // Имя горизонтального scrollbar в шаблоне DataGrid (из исходного кода DataGrid.cs)
    private const string HScrollBarName = "PART_HorizontalScrollbar";

    public static readonly AttachedProperty<DataGrid?> SourceDataGridProperty =
        AvaloniaProperty.RegisterAttached<FrozenHeaderScrollSyncBehavior, Panel, DataGrid?>(
            "SourceDataGrid");

    public DataGrid? SourceDataGrid
    {
        get => GetValue(SourceDataGridProperty);
        set => SetValue(SourceDataGridProperty, value);
    }

    private ScrollBar? _hScrollBar;
    private TranslateTransform? _transform;
    private DispatcherTimer? _pollTimer;
    private double _lastAppliedOffset = double.NaN;

    // ─────────────────────────────────────────────────────────────────
    //  Lifecycle
    // ─────────────────────────────────────────────────────────────────

    protected override void OnAttached()
    {
        base.OnAttached();

        // Запускаем через Post — к этому моменту XAML уже распарсен,
        // биндинги разрешены, но мы не в visual tree.
        // Post с ApplicationIdle гарантирует запуск ПОСЛЕ полного layout.
        Dispatcher.UIThread.Post(EnsureTimerStarted, DispatcherPriority.ApplicationIdle);

        if (AssociatedObject is not null)
            AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        EnsureTimerStarted();
    }

    protected override void OnDetaching()
    {
        StopTimer();
        if (AssociatedObject is not null)
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;
        base.OnDetaching();
    }

    // ─────────────────────────────────────────────────────────────────
    //  Timer
    // ─────────────────────────────────────────────────────────────────

    private void EnsureTimerStarted()
    {
        if (_pollTimer is not null) return;   // уже запущен

        // Найти DataGrid через биндинг или через visual tree
        if (SourceDataGrid is null && AssociatedObject is not null)
        {
            SourceDataGrid = AssociatedObject
                .GetVisualAncestors()
                .SelectMany(a => a.GetVisualDescendants())
                .OfType<DataGrid>()
                .FirstOrDefault(dg => dg.Name == "dataGrid");
        }

        if (SourceDataGrid is null) return;

        _pollTimer = new DispatcherTimer(DispatcherPriority.Normal)
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        _pollTimer.Tick += OnPollTick;
        _pollTimer.Start();

        Debug.WriteLine("[FrozenHeaderScrollSync] Timer started.");
    }

    private void StopTimer()
    {
        if (_pollTimer is null) return;
        _pollTimer.Stop();
        _pollTimer.Tick -= OnPollTick;
        _pollTimer = null;
    }

    // ─────────────────────────────────────────────────────────────────
    //  Poll tick
    // ─────────────────────────────────────────────────────────────────

    private void OnPollTick(object? sender, EventArgs e)
    {
        if (SourceDataGrid is null || AssociatedObject is null) return;

        // Находим скроллбар по точному имени из шаблона DataGrid
        if (_hScrollBar is null)
        {
            _hScrollBar = SourceDataGrid
                .GetVisualDescendants()
                .OfType<ScrollBar>()
                .FirstOrDefault(sb => sb.Name == HScrollBarName);

            if (_hScrollBar is null) return;

            Debug.WriteLine($"[FrozenHeaderScrollSync] ScrollBar found: {_hScrollBar.Name}");
        }

        var scrollOffset = _hScrollBar.Value;
        var extraOffset = ComputeExtraFrozenOffset();
        var totalOffset = scrollOffset + extraOffset;

        // Сравниваем СУММАРНОЕ смещение, а не только scrollbar.Value.
        // Без этого изменение ширины замороженных колонок или FrozenColumnCount
        // не приводило к пересчёту трансформа, пока скроллбар не двигался.
        if (!double.IsNaN(_lastAppliedOffset) &&
            Math.Abs(totalOffset - _lastAppliedOffset) < 0.1)
            return;

        _lastAppliedOffset = totalOffset;
        ApplyTransform(totalOffset);

        Debug.WriteLine($"[FrozenHeaderScrollSync] ApplyTransform(scroll={scrollOffset:F1} extra={extraOffset:F1} total={totalOffset:F1})");
    }

    // ─────────────────────────────────────────────────────────────────
    //  Apply
    // ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Применяет TranslateTransform.X = -totalOffset к внутреннему Grid.
    /// totalOffset уже включает scroll + extra (ширины замороженных колонок 1..N-1).
    /// </summary>
    private void ApplyTransform(double totalOffset)
    {
        var innerGrid = AssociatedObject?.Children.OfType<Grid>().FirstOrDefault();
        if (innerGrid is null)
        {
            Debug.WriteLine("[FrozenHeaderScrollSync] innerGrid not found!");
            return;
        }

        if (_transform is null)
        {
            _transform = new TranslateTransform(0, 0);
            innerGrid.RenderTransform = _transform;
        }

        _transform.X = -totalOffset;
    }

    /// <summary>
    /// Вычисляет суммарную ширину DataGrid-колонок 1..(FrozenColumnCount-1).
    /// Используется тот же вычет 1/PixelDensity что и в ColumnWidthSyncBehavior.
    /// </summary>
    private double ComputeExtraFrozenOffset()
    {
        if (SourceDataGrid is null) return 0;

        var frozenCount = SourceDataGrid.FrozenColumnCount;
        if (frozenCount <= 1) return 0;

        var scale = SourceDataGrid
            .GetVisualAncestors()
            .OfType<Window>()
            .FirstOrDefault()
            ?.Screens?.Primary?.PixelDensity ?? 1.0;

        var total = 0.0;
        for (var i = 1; i < frozenCount && i < SourceDataGrid.Columns.Count; i++)
        {
            var w = SourceDataGrid.Columns[i].Width.DisplayValue;
            if (w > 0)
                total += w - 1.0 / scale;
        }
        return total;
    }
}
