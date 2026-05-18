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
/// Синхронизирует горизонтальный скролл кастомной шапки с DataGrid без задержки.
///
/// AssociatedObject — Panel с ClipToBounds="True", содержащий Grid шапки.
/// Смещение: TranslateTransform.X = -(scroll + extraFrozen) на внутреннем Grid.
///
/// Подписки вместо таймера:
///   • _hScrollBar.Scroll        — мгновенный отклик при прокрутке
///   • SourceDataGrid.LayoutUpdated — обновление при изменении ширин колонок
/// </summary>
public class FrozenHeaderScrollSyncBehavior : Behavior<Panel>
{
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
    private double _lastAppliedTotal = double.NaN;

    // ─────────────────────────────────────────────────────────────────
    //  Lifecycle
    // ─────────────────────────────────────────────────────────────────

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is not null)
            AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;

        // Post на ApplicationIdle — биндинг SourceDataGrid уже разрешён к этому моменту
        Dispatcher.UIThread.Post(TrySubscribe, DispatcherPriority.ApplicationIdle);
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        => TrySubscribe();

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;

        if (SourceDataGrid is not null)
            SourceDataGrid.LayoutUpdated -= OnDataGridLayoutUpdated;

        if (_hScrollBar is not null)
            _hScrollBar.Scroll -= OnScrollBarScroll;

        base.OnDetaching();
    }

    // ─────────────────────────────────────────────────────────────────
    //  Подписка
    // ─────────────────────────────────────────────────────────────────

    private void TrySubscribe()
    {
        if (SourceDataGrid is null && AssociatedObject is not null)
        {
            SourceDataGrid = AssociatedObject
                .GetVisualAncestors()
                .SelectMany(a => a.GetVisualDescendants())
                .OfType<DataGrid>()
                .FirstOrDefault(dg => dg.Name == "dataGrid");
        }

        if (SourceDataGrid is null) return;

        // LayoutUpdated покрывает: ресайз колонок, изменение FrozenColumnCount,
        // первоначальный layout и любые другие перестройки макета
        SourceDataGrid.LayoutUpdated -= OnDataGridLayoutUpdated;
        SourceDataGrid.LayoutUpdated += OnDataGridLayoutUpdated;

        // Применяем сразу
        UpdateTransform();
    }

    // ─────────────────────────────────────────────────────────────────
    //  Обработчики событий
    // ─────────────────────────────────────────────────────────────────

    private void OnDataGridLayoutUpdated(object? sender, EventArgs e)
    {
        // При первом срабатывании пробуем найти горизонтальный скроллбар
        if (_hScrollBar is null)
        {
            var sb = SourceDataGrid?
                .GetVisualDescendants()
                .OfType<ScrollBar>()
                .FirstOrDefault(s => s.Name == HScrollBarName);

            if (sb is not null)
            {
                _hScrollBar = sb;
                _hScrollBar.Scroll += OnScrollBarScroll;
                Debug.WriteLine("[FrozenHeaderScrollSync] ScrollBar subscribed.");
            }
        }

        UpdateTransform();
    }

    private void OnScrollBarScroll(object? sender, ScrollEventArgs e)
        => UpdateTransform();

    // ─────────────────────────────────────────────────────────────────
    //  Расчёт и применение
    // ─────────────────────────────────────────────────────────────────

    private void UpdateTransform()
    {
        var scrollOffset = _hScrollBar?.Value ?? 0;
        var extraOffset = ComputeExtraFrozenOffset();
        var total = scrollOffset + extraOffset;

        // Пропускаем если значение не изменилось (LayoutUpdated очень частый)
        if (!double.IsNaN(_lastAppliedTotal) &&
            Math.Abs(total - _lastAppliedTotal) < 0.1)
            return;

        _lastAppliedTotal = total;
        ApplyTransform(total);

        Debug.WriteLine($"[FrozenHeaderScrollSync] transform={-total:F1} (scroll={scrollOffset:F1} extra={extraOffset:F1})");
    }

    private void ApplyTransform(double totalOffset)
    {
        var innerGrid = AssociatedObject?.Children.OfType<Grid>().FirstOrDefault();
        if (innerGrid is null) return;

        if (_transform is null)
        {
            _transform = new TranslateTransform(0, 0);
            innerGrid.RenderTransform = _transform;
        }

        _transform.X = -totalOffset;
    }

    /// <summary>
    /// Суммарная ширина DataGrid-колонок 1..(FrozenColumnCount-1) —
    /// на столько скроллируемая шапка сдвигается дополнительно, чтобы спрятать
    /// колонки, попавшие в фиксированную область.
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
