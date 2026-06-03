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
/// Метрики layout для синхронизации кастомной шапки (кэш на один проход).
/// </summary>
internal readonly struct TableHeaderLayoutMetrics
{
    public bool HasHorizontalScroll { get; init; }
    public double BorderCompensation { get; init; }
    public int FrozenColumnCount { get; init; }
}

/// <summary>
/// Один LayoutUpdated на DataGrid, кэш ширин колонок, coalesce обновлений.
/// </summary>
internal static class TableHeaderDataGridSync
{
    internal const string HScrollBarName = "PART_HorizontalScrollbar";

    private sealed class State
    {
        public ScrollBar? HScrollBar;
        public bool LayoutHandlerAttached;
        public bool WidthSyncPosted;
        public double[] ColumnWidths = Array.Empty<double>();
        public bool HasHorizontalScroll;
        public double BorderCompensation = 1.0;
        public int CachedFrozenCount = -1;
        public double LastGridWidth = double.NaN;
        public EventHandler<ScrollEventArgs>? ScrollHandler;
        public IDisposable? ScrollValueSubscription;

        public readonly HashSet<ColumnWidthSyncBehavior> WidthBehaviors = new();
        public readonly HashSet<FrozenHeaderScrollSyncBehavior> ScrollBehaviors = new();
    }

    private static readonly ConditionalWeakTable<DataGrid, State> States = new();

    public static void RegisterWidthBehavior(DataGrid dataGrid, ColumnWidthSyncBehavior behavior)
    {
        var state = GetOrCreateState(dataGrid);
        state.WidthBehaviors.Add(behavior);
        EnsureLayoutSubscription(dataGrid, state);
        RequestSync(dataGrid, force: true);
    }

    public static void UnregisterWidthBehavior(DataGrid dataGrid, ColumnWidthSyncBehavior behavior)
    {
        if (!States.TryGetValue(dataGrid, out var state)) return;
        state.WidthBehaviors.Remove(behavior);
        TryDetachLayoutIfEmpty(dataGrid, state);
    }

    public static void RegisterScrollBehavior(DataGrid dataGrid, FrozenHeaderScrollSyncBehavior behavior)
    {
        var state = GetOrCreateState(dataGrid);
        state.ScrollBehaviors.Add(behavior);
        EnsureLayoutSubscription(dataGrid, state);
        TryFindScrollBar(dataGrid, state);
        RequestSync(dataGrid, force: true);
    }

    public static void UnregisterScrollBehavior(DataGrid dataGrid, FrozenHeaderScrollSyncBehavior behavior)
    {
        if (!States.TryGetValue(dataGrid, out var state)) return;
        state.ScrollBehaviors.Remove(behavior);
        TryDetachLayoutIfEmpty(dataGrid, state);
    }

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

    /// <summary>Мгновенная синхронизация скролла шапки (без coalesce).</summary>
    public static void NotifyScrollChanged(DataGrid dataGrid)
    {
        if (!States.TryGetValue(dataGrid, out var state)) return;
        TryFindScrollBar(dataGrid, state);
        NotifyScrollChanged(dataGrid, state);
    }

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

        if (!HasRelevantLayoutChange(dataGrid, state))
        {
            // Скроллбар мог появиться позже в визуальном дереве
            if (state.HScrollBar is null)
                TryFindScrollBar(dataGrid, state);
            return;
        }

        ScheduleWidthSync(dataGrid, state);
    }

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

    private static void RefreshScrollMode(DataGrid dataGrid, State state)
    {
        state.HasHorizontalScroll = state.HScrollBar?.IsVisible == true;
        state.CachedFrozenCount = dataGrid.FrozenColumnCount;
        state.BorderCompensation = 1.0 / ResolvePixelDensityScale(dataGrid);
    }

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

    private static void ScheduleWidthSync(DataGrid dataGrid, State state, bool force = false)
    {
        if (!force && state.WidthSyncPosted) return;
        state.WidthSyncPosted = true;

        // Render — успевает за resize колонки; Background давал отставание и сдвиг шапки
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

        NotifyScrollChanged(dataGrid, state);
    }

    private static void NotifyScrollChanged(DataGrid dataGrid, State state)
    {
        RefreshScrollMode(dataGrid, state);
        var metrics = BuildMetrics(state);
        var scrollOffset = state.HScrollBar?.Value ?? 0;

        foreach (var behavior in state.ScrollBehaviors.ToArray())
            behavior.SyncScroll(metrics, scrollOffset);
    }

    private static TableHeaderLayoutMetrics BuildMetrics(State state)
        => new()
        {
            HasHorizontalScroll = state.HasHorizontalScroll,
            BorderCompensation = state.BorderCompensation,
            FrozenColumnCount = state.CachedFrozenCount
        };

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

        // Thumb/drag в Avalonia не всегда даёт Scroll — Value надёжнее
        state.ScrollValueSubscription?.Dispose();
        state.ScrollValueSubscription = state.HScrollBar
            .GetObservable(RangeBase.ValueProperty)
            .Subscribe(_ => NotifyScrollChanged(dataGrid));
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
}
