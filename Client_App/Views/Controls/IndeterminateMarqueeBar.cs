using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace Client_App.Views.Controls;

/// <summary>
/// Бегунок загрузки без ProgressBar: в Avalonia 0.10 DefaultTheme
/// IsIndeterminate часто не анимируется (статичный тёмный кусок).
/// </summary>
public sealed class IndeterminateMarqueeBar : Panel
{
    private const double DefaultTrackWidth = 220;
    private const double DefaultTrackHeight = 6;
    private const double ThumbWidth = 72;
    private const double SpeedPxPerTick = 7;

    private readonly Border _track;
    private readonly Border _thumb;
    private DispatcherTimer? _timer;
    private double _offset;
    private bool _running;
    private bool _attached;

    public static readonly StyledProperty<double> TrackWidthProperty =
        AvaloniaProperty.Register<IndeterminateMarqueeBar, double>(nameof(TrackWidth), DefaultTrackWidth);

    public static readonly StyledProperty<IBrush?> FillProperty =
        AvaloniaProperty.Register<IndeterminateMarqueeBar, IBrush?>(
            nameof(Fill),
            new SolidColorBrush(Color.Parse("#FF1976D2")));

    public static readonly StyledProperty<IBrush?> TrackBrushProperty =
        AvaloniaProperty.Register<IndeterminateMarqueeBar, IBrush?>(
            nameof(TrackBrush),
            new SolidColorBrush(Color.Parse("#FFE0E0E0")));

    public double TrackWidth
    {
        get => GetValue(TrackWidthProperty);
        set => SetValue(TrackWidthProperty, value);
    }

    public IBrush? Fill
    {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public IBrush? TrackBrush
    {
        get => GetValue(TrackBrushProperty);
        set => SetValue(TrackBrushProperty, value);
    }

    public IndeterminateMarqueeBar()
    {
        ClipToBounds = true;
        Width = DefaultTrackWidth;
        Height = DefaultTrackHeight;

        _track = new Border
        {
            Background = TrackBrush,
            CornerRadius = new CornerRadius(3),
            Width = DefaultTrackWidth,
            Height = DefaultTrackHeight,
        };

        _thumb = new Border
        {
            Background = Fill,
            CornerRadius = new CornerRadius(3),
            Width = ThumbWidth,
            Height = DefaultTrackHeight,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
        };

        Children.Add(_track);
        Children.Add(_thumb);

        PropertyChanged += OnPropertyChanged;
        AttachedToVisualTree += (_, _) => _attached = true;
        DetachedFromVisualTree += (_, _) =>
        {
            _attached = false;
            Stop();
        };
    }

    /// <summary>Запустить бегунок (вызывать при показе overlay).</summary>
    public void Start() => TryStart();

    /// <summary>Остановить бегунок.</summary>
    public void Stop()
    {
        _running = false;
        if (_timer is null)
            return;
        _timer.Tick -= OnTick;
        _timer.Stop();
        _timer = null;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var w = TrackWidth > 0 ? TrackWidth : DefaultTrackWidth;
        const double h = DefaultTrackHeight;
        _track.Width = w;
        _track.Height = h;
        _thumb.Height = h;
        Width = w;
        Height = h;
        return new Size(w, h);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var w = TrackWidth > 0 ? TrackWidth : finalSize.Width;
        const double h = DefaultTrackHeight;
        _track.Arrange(new Rect(0, 0, w, h));
        _thumb.Arrange(new Rect(_offset, 0, ThumbWidth, h));
        return new Size(w, h);
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == FillProperty)
            _thumb.Background = Fill;
        else if (e.Property == TrackBrushProperty)
            _track.Background = TrackBrush;
        else if (e.Property == TrackWidthProperty)
            InvalidateMeasure();
    }

    private void TryStart()
    {
        if (!_attached)
            return;
        if (_running)
            return;

        _running = true;
        _offset = -ThumbWidth;
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _timer.Tick += OnTick;
        _timer.Start();
        InvalidateArrange();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        var trackW = TrackWidth > 0 ? TrackWidth : Bounds.Width;
        if (trackW <= 0)
            trackW = DefaultTrackWidth;

        _offset += SpeedPxPerTick;
        if (_offset > trackW)
            _offset = -ThumbWidth;

        InvalidateArrange();
    }
}
