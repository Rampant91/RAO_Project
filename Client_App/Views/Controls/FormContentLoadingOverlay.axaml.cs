using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Client_App.Views.Controls;

/// <summary>
/// Индикатор загрузки окна формы: карточка по центру без затемнения окна.
/// </summary>
public partial class FormContentLoadingOverlay : UserControl
{
    public FormContentLoadingOverlay()
    {
        InitializeComponent();
        PropertyChanged += OnOverlayVisibilityChanged;
        AttachedToVisualTree += (_, _) =>
        {
            if (IsVisible)
                StartMarquee();
        };
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void OnOverlayVisibilityChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != IsVisibleProperty)
            return;

        var marquee = this.FindControl<IndeterminateMarqueeBar>("Marquee");
        if (e.NewValue is true)
            StartMarquee();
        else
            StopMarquee();
    }

    private void StartMarquee()
    {
        this.FindControl<IndeterminateMarqueeBar>("Marquee")?.Start();
    }

    private void StopMarquee()
    {
        this.FindControl<IndeterminateMarqueeBar>("Marquee")?.Stop();
    }
}
