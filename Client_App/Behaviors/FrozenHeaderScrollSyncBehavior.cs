using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
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
    /// Управляет шириной и выравниванием фиксированного заголовка "Сведения об операции".
    ///
    /// Пока колонка "дата" (DataGrid col 2) хотя бы частично видна в скроллируемой области:
    ///   • Border расширяется на её видимую часть → текст центрируется над суммой ("код" + видимая "дата");
    ///   • граница "right" включена — правый край бордера совпадает с правым краем видимой "дата".
    ///
    /// Как только "дата" уходит за левый край скролла:
    ///   • Border возвращается к естественной ширине колонки "код";
    ///   • граница "right" тоже включена, отделяя "Сведения об операции" от следующей группы.
    /// </summary>
    private void UpdateFixedGroupHeaderBorder(double scrollOffset)
    {
        if (FixedGroupHeaderBorder is null || SourceDataGrid is null) return;

        var frozenCount = SourceDataGrid.FrozenColumnCount;

        if (frozenCount != 2 || SourceDataGrid.Columns.Count <= 2)
        {
            ResetFixedGroupHeaderBorder();
            return;
        }

        var datWidth = SourceDataGrid.Columns[2].Width.DisplayValue;
        var kodWidth = SourceDataGrid.Columns[1].Width.DisplayValue;

        if (datWidth <= 0 || kodWidth <= 0)
        {
            ResetFixedGroupHeaderBorder();
            return;
        }

        var visibleDat = Math.Max(0.0, datWidth - scrollOffset);

        if (visibleDat > 0.5)
        {
            // Merged-вид: расширяем Border вправо на видимую часть "дата".
            // +1 компенсирует Margin="-1,0,0,0" у Border: его левый край на 1px левее колонки,
            // поэтому для достижения нужного правого края нужна +1 к ширине.
            FixedGroupHeaderBorder.Width = kodWidth + 1.0 + visibleDat;
            FixedGroupHeaderBorder.HorizontalAlignment = HorizontalAlignment.Left;
        }
        else
        {
            ResetFixedGroupHeaderBorder();
        }

        FixedGroupHeaderBorder.BorderThickness = new Thickness(1);
    }

    private void ResetFixedGroupHeaderBorder()
    {
        if (FixedGroupHeaderBorder is null) return;
        FixedGroupHeaderBorder.Width = double.NaN;
        FixedGroupHeaderBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
        FixedGroupHeaderBorder.BorderThickness = new Thickness(1);
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

        var total = 0.0;
        for (var i = 1; i < frozenCount && i < SourceDataGrid.Columns.Count; i++)
        {
            var w = SourceDataGrid.Columns[i].Width.DisplayValue;
            if (w > 0)
                total += w;
        }
        return total;
    }
}
