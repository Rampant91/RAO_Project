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

    /// <summary>
    /// Grid для "№ п/п" внутри скроллируемой Panel — виден только при FrozenColumnCount=0.
    /// Ширина задаётся поведением по DataGrid col 0; трансформ X = -scrollOffset,
    /// чтобы элемент скроллировался вместе с данными (без дополнительного смещения).
    /// </summary>
    public static readonly StyledProperty<Grid?> NppScrollableHeaderGridProperty =
        AvaloniaProperty.Register<FrozenHeaderScrollSyncBehavior, Grid?>(nameof(NppScrollableHeaderGrid));

    public Grid? NppScrollableHeaderGrid
    {
        get => GetValue(NppScrollableHeaderGridProperty);
        set => SetValue(NppScrollableHeaderGridProperty, value);
    }

    private ScrollBar? _hScrollBar;
    private TranslateTransform? _transform;
    private TranslateTransform? _nppTransform;
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
        ApplyTransform(total, scrollOffset);
        UpdateFixedGroupHeaderBorder(scrollOffset);

        Debug.WriteLine($"[FrozenHeaderScrollSync] transform={-total:F1} (scroll={scrollOffset:F1} extra={extraOffset:F1})");
    }

    private void ApplyTransform(double totalOffset, double scrollOffset)
    {
        // Основной скроллируемый Grid (StartColumnIndex=1, col 0 = "код")
        var innerGrid = AssociatedObject?.Children.OfType<Grid>().FirstOrDefault();
        if (innerGrid is null) return;

        if (_transform is null)
        {
            _transform = new TranslateTransform(0, 0);
            innerGrid.RenderTransform = _transform;
        }
        _transform.X = -totalOffset;

        // Grid "№ п/п" виден только при FrozenCount=0; скроллируется без extraOffset
        if (NppScrollableHeaderGrid is not null)
        {
            if (_nppTransform is null)
            {
                _nppTransform = new TranslateTransform(0, 0);
                NppScrollableHeaderGrid.RenderTransform = _nppTransform;
            }
            _nppTransform.X = -scrollOffset;

            // Ширина синхронизируется с DataGrid col 0 ("№пп")
            if (SourceDataGrid?.Columns.Count > 0)
            {
                var w = SourceDataGrid.Columns[0].Width.DisplayValue;
                if (w > 0)
                    NppScrollableHeaderGrid.Width = w;
            }
        }
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
    /// Вычисляет дополнительное смещение скроллируемой шапки (сверх scrollOffset):
    ///
    /// • FrozenCount = 0: фиксированный оверлей скрыт → Panel занимает всю ширину с x=0.
    ///   Скроллируемая шапка (StartColumnIndex=1) покрывает DataGrid col 1+.
    ///   Чтобы col 0 ("код") выровнялся по DataGrid col 1 даже без оверлея,
    ///   шапку нужно сдвинуть ВПРАВО на ширину DataGrid col 0 ("№пп").
    ///   Возвращаем отрицательное значение → transform = -(scroll + extra) > 0 → сдвиг вправо.
    ///   Над "№пп" в многоуровневой шапке останется пустое место (фон) — приемлемый компромисс.
    ///
    /// • FrozenCount = 1: только "№пп" заморожен → никакого дополнительного сдвига не нужно.
    ///
    /// • FrozenCount >= 2: прячем col 1..FrozenCount-1, сдвигая шапку влево на их суммарную ширину.
    /// </summary>
    private double ComputeExtraFrozenOffset()
    {
        if (SourceDataGrid is null) return 0;

        var frozenCount = SourceDataGrid.FrozenColumnCount;

        if (frozenCount == 0)
        {
            // Сдвигаем вправо на ширину DataGrid col 0 ("№пп"), чтобы col 0 скроллируемой
            // шапки ("код") оказался точно над DataGrid col 1.
            var nppWidth = SourceDataGrid.Columns.Count > 0
                ? SourceDataGrid.Columns[0].Width.DisplayValue
                : 0;
            return nppWidth > 0 ? -nppWidth : 0;
        }

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
