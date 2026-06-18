using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Client_App.Behaviors;

/// <summary>
/// Снимок метрик layout DataGrid на один проход синхронизации кастомной шапки.
/// </summary>
internal readonly struct TableHeaderLayoutMetrics
{
    /// <summary>Горизонтальный скроллбар DataGrid виден (контент шире области).</summary>
    public bool HasHorizontalScroll { get; init; }

    /// <summary>1 / DPI экрана; вычитается из ширины col 0 для стыковки 1px-границ шапки.</summary>
    public double BorderCompensation { get; init; }

    /// <summary>Текущее значение <see cref="DataGrid.FrozenColumnCount"/>.</summary>
    public int FrozenColumnCount { get; init; }
}

/// <summary>
/// Координатор синхронизации между <see cref="DataGrid"/> и кастомной шапкой таблицы.
/// <para>
/// На каждый DataGrid вешается один LayoutUpdated; зарегистрированные
/// <see cref="ColumnWidthSyncBehavior"/> и <see cref="FrozenHeaderScrollSyncBehavior"/>
/// получают обновления через единый кэш и coalesce (без дублирования подписок на шапке).
/// </para>
/// <para>
/// Resize через <see cref="TableHeaderColumnResizeBehavior"/> идёт по отдельному «live»-пути:
/// синхронная точечная синхронизация шапки без очереди <see cref="DispatcherPriority.Render"/>.
/// </para>
/// </summary>
internal static class TableHeaderDataGridSync
{
    #region Константы

    /// <summary>Имя горизонтального скроллбара в визуальном дереве Avalonia DataGrid.</summary>
    internal const string HScrollBarName = "PART_HorizontalScrollbar";

    #endregion

    #region Состояние (на DataGrid)

    private sealed class State
    {
        public ScrollBar? HScrollBar;
        public bool LayoutHandlerAttached;
        public bool WidthSyncPosted;

        /// <summary>Счётчик вложенных live-resize (на случай нескольких overlay на одной форме).</summary>
        public int LiveResizeCount;

        /// <summary>Индекс колонки последнего live-resize (для отладки и расширений).</summary>
        public int LiveResizeColumnIndex = -1;

        public double[] ColumnWidths = Array.Empty<double>();
        public bool HasHorizontalScroll;
        public double BorderCompensation = 1.0;
        public double CachedPixelDensity = 1.0;
        public bool PixelDensityResolved;
        public int CachedFrozenCount = -1;
        public double LastGridWidth = double.NaN;
        public EventHandler<ScrollEventArgs>? ScrollHandler;
        public IDisposable? ScrollValueSubscription;

        public readonly HashSet<ColumnWidthSyncBehavior> WidthBehaviors = new();
        public readonly HashSet<FrozenHeaderScrollSyncBehavior> ScrollBehaviors = new();

        public bool IsLiveResizing => LiveResizeCount > 0;
    }

    private static readonly ConditionalWeakTable<DataGrid, State> States = new();

    #endregion

    #region Регистрация behaviors

    /// <summary>Подключает синхронизацию ширин колонок шапки к DataGrid.</summary>
    public static void RegisterWidthBehavior(DataGrid dataGrid, ColumnWidthSyncBehavior behavior)
    {
        var state = GetOrCreateState(dataGrid);
        state.WidthBehaviors.Add(behavior);
        EnsureLayoutSubscription(dataGrid, state);
        RequestSync(dataGrid, force: true);
    }

    /// <summary>Отключает <see cref="ColumnWidthSyncBehavior"/> и снимает подписки, если behaviors не осталось.</summary>
    public static void UnregisterWidthBehavior(DataGrid dataGrid, ColumnWidthSyncBehavior behavior)
    {
        if (!States.TryGetValue(dataGrid, out var state)) return;
        state.WidthBehaviors.Remove(behavior);
        TryDetachLayoutIfEmpty(dataGrid, state);
    }

    /// <summary>Подключает синхронизацию горизонтального скролла шапки к DataGrid.</summary>
    public static void RegisterScrollBehavior(DataGrid dataGrid, FrozenHeaderScrollSyncBehavior behavior)
    {
        var state = GetOrCreateState(dataGrid);
        state.ScrollBehaviors.Add(behavior);
        EnsureLayoutSubscription(dataGrid, state);
        TryFindScrollBar(dataGrid, state);
        RequestSync(dataGrid, force: true);
    }

    /// <summary>Отключает <see cref="FrozenHeaderScrollSyncBehavior"/>.</summary>
    public static void UnregisterScrollBehavior(DataGrid dataGrid, FrozenHeaderScrollSyncBehavior behavior)
    {
        if (!States.TryGetValue(dataGrid, out var state)) return;
        state.ScrollBehaviors.Remove(behavior);
        TryDetachLayoutIfEmpty(dataGrid, state);
    }

    #endregion

    #region Публичный API синхронизации

    /// <summary>
    /// Запланировать полную синхронизацию ширин и скролла шапки (с coalesce через Render).
    /// </summary>
    /// <param name="force">Сбросить кэш ширин и выполнить sync даже при совпадении значений.</param>
    public static void RequestSync(DataGrid dataGrid, bool force = false)
    {
        var state = GetOrCreateState(dataGrid);
        if (force)
        {
            InvalidateWidthCache(state, dataGrid.Columns.Count);
            state.WidthSyncPosted = false;
        }

        ScheduleWidthSync(dataGrid, state, force);
    }

    /// <summary>
    /// Немедленно синхронизировать только скролл шапки (без coalesce).
    /// Вызывается при прокрутке DataGrid и из <see cref="RunWidthSync"/>.
    /// </summary>
    public static void NotifyScrollChanged(DataGrid dataGrid)
    {
        if (!States.TryGetValue(dataGrid, out var state)) return;
        TryFindScrollBar(dataGrid, state);
        RefreshScrollMode(dataGrid, state);
        DispatchScrollSync(dataGrid, state);
    }

    #endregion

    #region Live-resize (TableHeaderColumnResizeBehavior)

    /// <summary>
    /// Начало drag-resize колонки через кастомную шапку.
    /// Подавляет полный sync из LayoutUpdated DataGrid на время перетаскивания.
    /// </summary>
    public static void BeginLiveColumnResize(DataGrid dataGrid, int columnIndex)
    {
        var state = GetOrCreateState(dataGrid);
        state.LiveResizeCount++;
        state.LiveResizeColumnIndex = columnIndex;
    }

    /// <summary>
    /// Конец drag-resize. Выполняет финальный <see cref="RequestSync"/> для согласованности.
    /// </summary>
    public static void EndLiveColumnResize(DataGrid dataGrid)
    {
        if (!States.TryGetValue(dataGrid, out var state)) return;

        state.LiveResizeCount = Math.Max(0, state.LiveResizeCount - 1);
        if (!state.IsLiveResizing)
        {
            state.LiveResizeColumnIndex = -1;
            RequestSync(dataGrid);
        }
    }

    /// <summary>
    /// Синхронная точечная синхронизация шапки во время drag-resize (без очереди Render).
    /// Кэш ширин обновляется здесь — при IsLiveResizing LayoutUpdated не сканирует все колонки.
    /// </summary>
    public static void SyncLiveColumnWidth(DataGrid dataGrid, int columnIndex, double width)
    {
        if (!States.TryGetValue(dataGrid, out var state)) return;

        state.LiveResizeColumnIndex = columnIndex;
        TryFindScrollBar(dataGrid, state);
        RefreshScrollMode(dataGrid, state);
        UpdateColumnWidthCache(state, dataGrid.Columns.Count, columnIndex, width);

        var metrics = BuildMetrics(state);
        foreach (var behavior in state.WidthBehaviors)
            behavior.SyncWidths(metrics, changedColumnIndex: columnIndex);

        // Scroll-sync нужен только если меняется ширина frozen/npp или сдвиг extraOffset
        if (ShouldSyncScrollDuringLiveResize(columnIndex, metrics))
            DispatchScrollSync(dataGrid, state);
    }

    /// <summary>
    /// При live-resize scroll-sync не нужен для «далёких» колонок: scrollOffset не меняется,
    /// а transform/extraOffset зависят только от col 0 и frozen-колонок.
    /// </summary>
    private static bool ShouldSyncScrollDuringLiveResize(int columnIndex, TableHeaderLayoutMetrics metrics)
    {
        if (columnIndex == 0)
            return true;

        var frozen = metrics.FrozenColumnCount;
        if (frozen == 2 && columnIndex is 1 or 2)
            return true;

        return frozen > 1 && columnIndex >= 1 && columnIndex < frozen;
    }

    #endregion

    #region LayoutUpdated и планирование sync

    private static State GetOrCreateState(DataGrid dataGrid)
        => States.GetValue(dataGrid, _ => new State());

    private static void EnsureLayoutSubscription(DataGrid dataGrid, State state)
    {
        if (state.LayoutHandlerAttached) return;
        dataGrid.LayoutUpdated += OnDataGridLayoutUpdated;
        state.LayoutHandlerAttached = true;
    }

    private static void TryDetachLayoutIfEmpty(DataGrid dataGrid, State state)
    {
        if (state.WidthBehaviors.Count > 0 || state.ScrollBehaviors.Count > 0) return;

        if (state.LayoutHandlerAttached)
        {
            dataGrid.LayoutUpdated -= OnDataGridLayoutUpdated;
            state.LayoutHandlerAttached = false;
        }

        if (state.HScrollBar is not null && state.ScrollHandler is not null)
            state.HScrollBar.Scroll -= state.ScrollHandler;

        state.ScrollValueSubscription?.Dispose();
        state.ScrollValueSubscription = null;
    }

    private static void OnDataGridLayoutUpdated(object? sender, EventArgs e)
    {
        if (sender is not DataGrid dataGrid) return;

        var state = GetOrCreateState(dataGrid);
        TryFindScrollBar(dataGrid, state);

        // Во время drag шапка уже синхронизирована через SyncLiveColumnWidth
        if (state.IsLiveResizing)
            return;

        if (!HasRelevantLayoutChange(dataGrid, state))
        {
            // Скроллбар может появиться в визуальном дереве позже первого layout
            if (state.HScrollBar is null)
                TryFindScrollBar(dataGrid, state);
            return;
        }

        ScheduleWidthSync(dataGrid, state);
    }

    /// <summary>
    /// Фильтр «шумных» LayoutUpdated: реагируем только на смену ширины грида,
    /// ширин колонок, видимости скролла или FrozenColumnCount.
    /// </summary>
    private static bool HasRelevantLayoutChange(DataGrid dataGrid, State state)
    {
        var width = dataGrid.Bounds.Width;
        if (!double.IsNaN(state.LastGridWidth) && Math.Abs(width - state.LastGridWidth) > 0.5)
        {
            state.LastGridWidth = width;
            return true;
        }

        if (double.IsNaN(state.LastGridWidth))
            state.LastGridWidth = width;

        if (ColumnWidthsChanged(dataGrid, state))
            return true;

        var hadScroll = state.HasHorizontalScroll;
        var hadFrozen = state.CachedFrozenCount;
        RefreshScrollMode(dataGrid, state);
        return hadScroll != state.HasHorizontalScroll || hadFrozen != state.CachedFrozenCount;
    }

    private static bool ColumnWidthsChanged(DataGrid dataGrid, State state)
    {
        var count = dataGrid.Columns.Count;
        if (state.ColumnWidths.Length != count)
        {
            ResizeWidthCache(state, count);
            CaptureColumnWidths(dataGrid, state);
            return true;
        }

        for (var i = 0; i < count; i++)
        {
            var w = dataGrid.Columns[i].Width.DisplayValue;
            if (Math.Abs(w - state.ColumnWidths[i]) > 0.05)
            {
                CaptureColumnWidths(dataGrid, state);
                return true;
            }
        }

        return false;
    }

    private static void ScheduleWidthSync(DataGrid dataGrid, State state, bool force = false)
    {
        if (!force && state.WidthSyncPosted) return;
        state.WidthSyncPosted = true;

        // Render — шапка успевает за layout DataGrid; Background давал визуальное отставание
        Dispatcher.UIThread.Post(() =>
        {
            state.WidthSyncPosted = false;
            if (!States.TryGetValue(dataGrid, out var current)) return;
            RunWidthSync(dataGrid, current, force);
        }, DispatcherPriority.Render);
    }

    private static void RunWidthSync(DataGrid dataGrid, State state, bool force)
    {
        TryFindScrollBar(dataGrid, state);
        RefreshScrollMode(dataGrid, state);
        CaptureColumnWidths(dataGrid, state);

        var metrics = BuildMetrics(state);
        foreach (var behavior in state.WidthBehaviors.ToArray())
            behavior.SyncWidths(metrics, force);

        DispatchScrollSync(dataGrid, state);
    }

    #endregion

    #region Синхронизация скролла

    private static void DispatchScrollSync(DataGrid dataGrid, State state)
    {
        var metrics = BuildMetrics(state);
        var scrollOffset = state.HScrollBar?.Value ?? 0;

        foreach (var behavior in state.ScrollBehaviors)
            behavior.SyncScroll(metrics, scrollOffset);
    }

    private static void TryFindScrollBar(DataGrid dataGrid, State state)
    {
        if (state.HScrollBar is not null) return;

        var scrollBar = dataGrid.GetVisualDescendants()
            .OfType<ScrollBar>()
            .FirstOrDefault(s => s.Name == HScrollBarName);

        if (scrollBar is null) return;

        state.HScrollBar = scrollBar;

        state.ScrollHandler = (_, _) => NotifyScrollChanged(dataGrid);
        state.HScrollBar.Scroll += state.ScrollHandler;

        // Thumb/drag в Avalonia не всегда даёт Scroll — подписка на Value надёжнее
        state.ScrollValueSubscription?.Dispose();
        state.ScrollValueSubscription = state.HScrollBar
            .GetObservable(RangeBase.ValueProperty)
            .Subscribe(_ => NotifyScrollChanged(dataGrid));
    }

    #endregion

    #region Метрики и кэш ширин

    private static void RefreshScrollMode(DataGrid dataGrid, State state)
    {
        state.HasHorizontalScroll = state.HScrollBar?.IsVisible == true;
        state.CachedFrozenCount = dataGrid.FrozenColumnCount;

        if (!state.PixelDensityResolved)
        {
            state.CachedPixelDensity = ResolvePixelDensityScale(dataGrid);
            state.PixelDensityResolved = true;
        }

        state.BorderCompensation = 1.0 / state.CachedPixelDensity;
    }

    private static TableHeaderLayoutMetrics BuildMetrics(State state)
        => new()
        {
            HasHorizontalScroll = state.HasHorizontalScroll,
            BorderCompensation = state.BorderCompensation,
            FrozenColumnCount = state.CachedFrozenCount
        };

    private static void CaptureColumnWidths(DataGrid dataGrid, State state)
    {
        var count = dataGrid.Columns.Count;
        ResizeWidthCache(state, count);
        for (var i = 0; i < count; i++)
            state.ColumnWidths[i] = dataGrid.Columns[i].Width.DisplayValue;
    }

    private static void ResizeWidthCache(State state, int count)
    {
        if (state.ColumnWidths.Length == count) return;
        state.ColumnWidths = new double[count];
    }

    private static void InvalidateWidthCache(State state, int count)
    {
        ResizeWidthCache(state, count);
        Array.Fill(state.ColumnWidths, double.NaN);
    }

    private static void UpdateColumnWidthCache(State state, int count, int columnIndex, double width)
    {
        ResizeWidthCache(state, count);
        if (columnIndex >= 0 && columnIndex < state.ColumnWidths.Length)
            state.ColumnWidths[columnIndex] = width;
    }

    private static double ResolvePixelDensityScale(Visual visual)
    {
        var window = visual.GetVisualAncestors().OfType<Window>().FirstOrDefault();
        if (window?.Screens is null &&
            Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            window = desktop.MainWindow;

        if (window?.Screens is null) return 1.0;

        try
        {
            var screen = window.Screens.ScreenFromWindow(window.PlatformImpl) ?? window.Screens.Primary;
            var scale = screen?.PixelDensity ?? 1.0;
            return scale > 0 ? scale : 1.0;
        }
        catch
        {
            return 1.0;
        }
    }

    #endregion
}
