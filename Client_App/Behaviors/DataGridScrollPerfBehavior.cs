using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms.Forms1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Client_App.Behaviors;

/// <summary>
/// Временный инструмент для объективной оценки скролла DataGrid (форма 1.1).
/// Показывает число реализованных строк/контролов и время отклика на прокрутку.
/// </summary>
public class DataGridScrollPerfBehavior : Behavior<DataGrid>
{
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<DataGridScrollPerfBehavior, bool>(nameof(IsEnabled), defaultValue: false);

    public bool IsEnabled
    {
        get => GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    private ScrollViewer? _scrollViewer;
    private bool _pendingScrollMeasure;
    private readonly Stopwatch _scrollStopwatch = new();
    private readonly Queue<double> _scrollDurationsMs = new();
    private DateTime _lastTreeWalkUtc = DateTime.MinValue;
    private int _lastRowCount;
    private int _lastTextBoxCount;
    private int _lastAutoCompleteCount;
    private int _lastLazyHostCount;
    private int _lastLazyHostEditingCount;
    private DispatcherTimer? _walkTimer;

    private const int MaxSamples = 25;
    private static readonly TimeSpan TreeWalkInterval = TimeSpan.FromMilliseconds(400);

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);
        if (change.Property != IsEnabledProperty || AssociatedObject == null)
            return;

        if (IsEnabled)
        {
            TrySubscribeScroll();
            ScheduleTreeWalk(immediate: true);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject == null)
            return;

        AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;
        AssociatedObject.DetachedFromVisualTree += OnDetachedFromVisualTree;
        AssociatedObject.LayoutUpdated += OnLayoutUpdated;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;
            AssociatedObject.DetachedFromVisualTree -= OnDetachedFromVisualTree;
            AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
            UnsubscribeScroll();
        }

        _walkTimer?.Stop();
        _walkTimer = null;
        base.OnDetaching();
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (!IsEnabled)
            return;

        TrySubscribeScroll();
        ScheduleTreeWalk(immediate: true);
    }

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        UnsubscribeScroll();
        _walkTimer?.Stop();
        _walkTimer = null;
    }

    private void TrySubscribeScroll()
    {
        if (AssociatedObject == null || _scrollViewer != null)
            return;

        _scrollViewer = AssociatedObject.GetVisualDescendants().OfType<ScrollViewer>().FirstOrDefault();
        if (_scrollViewer != null)
            _scrollViewer.ScrollChanged += OnScrollChanged;
    }

    private void UnsubscribeScroll()
    {
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged -= OnScrollChanged;
            _scrollViewer = null;
        }
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (!IsEnabled || Math.Abs(e.OffsetDelta.Y) < 0.01)
            return;

        _scrollStopwatch.Restart();
        _pendingScrollMeasure = true;
        ScheduleTreeWalk(immediate: false);
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (!IsEnabled || !_pendingScrollMeasure)
            return;

        _pendingScrollMeasure = false;
        _scrollStopwatch.Stop();

        var ms = _scrollStopwatch.Elapsed.TotalMilliseconds;
        _scrollDurationsMs.Enqueue(ms);
        while (_scrollDurationsMs.Count > MaxSamples)
            _scrollDurationsMs.Dequeue();

        PublishStats(lastScrollMs: ms);
    }

    private void ScheduleTreeWalk(bool immediate)
    {
        if (!IsEnabled || AssociatedObject == null)
            return;

        if (immediate)
        {
            WalkVisualTree();
            PublishStats(lastScrollMs: null);
            return;
        }

        _walkTimer ??= new DispatcherTimer
        {
            Interval = TreeWalkInterval
        };
        _walkTimer.Tick -= OnWalkTimerTick;
        _walkTimer.Tick += OnWalkTimerTick;
        _walkTimer.Stop();
        _walkTimer.Start();
    }

    private void OnWalkTimerTick(object? sender, EventArgs e)
    {
        _walkTimer?.Stop();
        WalkVisualTree();
        PublishStats(lastScrollMs: null);
    }

    private void WalkVisualTree()
    {
        if (AssociatedObject == null)
            return;

        var now = DateTime.UtcNow;
        if (now - _lastTreeWalkUtc < TreeWalkInterval && _lastRowCount > 0)
            return;

        _lastTreeWalkUtc = now;
        _lastRowCount = AssociatedObject.GetVisualDescendants().OfType<DataGridRow>().Count();
        _lastTextBoxCount = AssociatedObject.GetVisualDescendants().OfType<TextBox>().Count();
        _lastAutoCompleteCount = AssociatedObject.GetVisualDescendants().OfType<global::Avalonia.Controls.AutoCompleteBox>().Count();

        var lazyHosts = AssociatedObject.GetVisualDescendants().OfType<Controls.DataGridLazyEditHost>().ToList();
        _lastLazyHostCount = lazyHosts.Count;
        _lastLazyHostEditingCount = lazyHosts.Count(h => h.IsInEditMode);
    }

    private void PublishStats(double? lastScrollMs)
    {
        if (AssociatedObject?.DataContext is not Form_11VM vm)
            return;

        var avg = _scrollDurationsMs.Count > 0 ? _scrollDurationsMs.Average() : 0;
        var max = _scrollDurationsMs.Count > 0 ? _scrollDurationsMs.Max() : 0;
        var scrollLine = lastScrollMs.HasValue
            ? $"scroll layout: {lastScrollMs.Value:F1} ms (avg {avg:F1}, max {max:F1}, n={_scrollDurationsMs.Count})"
            : $"scroll layout: avg {avg:F1} ms, max {max:F1} ms (n={_scrollDurationsMs.Count})";

        vm.ScrollPerfStats =
            $"{scrollLine}\n" +
            $"rows={_lastRowCount}  TextBox={_lastTextBoxCount}  AutoComplete={_lastAutoCompleteCount}\n" +
            $"LazyHost={_lastLazyHostCount}  editing={_lastLazyHostEditingCount}";
    }
}
