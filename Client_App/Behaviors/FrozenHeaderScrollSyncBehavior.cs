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

    /// <summary>
    /// Border фиксированного заголовка "Сведения об операции" (видим только при FrozenColumnCount=2).
    /// Поведение динамически управляет правой границей:
    ///   • нет правой границы — пока колонка "дата" хотя бы частично видна в скроллируемой области
    ///     (merged-вид: фикс. ячейка и пустая скроллируемая выглядят как одна);
    ///   • полная граница — когда "дата" полностью ушла за левый край (граница отделяет
    ///     "Сведения об операции" от следующей группы заголовков).
    /// </summary>
    public static readonly StyledProperty<Border?> FixedGroupHeaderBorderProperty =
        AvaloniaProperty.Register<FrozenHeaderScrollSyncBehavior, Border?>(nameof(FixedGroupHeaderBorder));

    public Border? FixedGroupHeaderBorder
    {
        get => GetValue(FixedGroupHeaderBorderProperty);
        set => SetValue(FixedGroupHeaderBorderProperty, value);
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
        UpdateFixedGroupHeaderBorder(scrollOffset);

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
    /// Управляет правой границей фиксированного заголовка "Сведения об операции".
    /// Граница скрыта (merged-вид) пока колонка "дата" (DataGrid col 2) хотя бы частично
    /// видна в скроллируемой области. Как только "дата" уходит за левый край — граница
    /// появляется, чтобы визуально отделить фиксированную область от следующей группы.
    /// </summary>
    private void UpdateFixedGroupHeaderBorder(double scrollOffset)
    {
        if (FixedGroupHeaderBorder is null || SourceDataGrid is null) return;

        var frozenCount = SourceDataGrid.FrozenColumnCount;

        // Ширина колонки "дата" (DataGrid column index 2).
        // Пока scrollOffset < datWidth, "дата" хотя бы частично видна → merged-вид.
        var datWidth = frozenCount == 2 && SourceDataGrid.Columns.Count > 2
            ? SourceDataGrid.Columns[2].Width.DisplayValue
            : 0;

        // merged: нет правой границы (сливается со скроллируемой пустой ячейкой)
        // separated: полная граница (отделяет от следующей группы)
        var merged = frozenCount != 2 || datWidth <= 0 || scrollOffset < datWidth;

        FixedGroupHeaderBorder.BorderThickness = merged
            ? new Thickness(1, 1, 0, 1)
            : new Thickness(1);
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
