using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Client_App.Views.Controls;

/// <summary>Полупрозрачный индикатор поверх DataGrid при фоновой загрузке страницы.</summary>
public partial class DataGridLoadingOverlay : UserControl
{
    public DataGridLoadingOverlay()
    {
        InitializeComponent();
        PropertyChanged += OnOverlayVisibilityChanged;
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (IsVisible)
            StartMarquee();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void OnOverlayVisibilityChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != IsVisibleProperty)
            return;

        if (e.NewValue is true)
            StartMarquee();
        else
            StopMarquee();
    }

    private void StartMarquee()
    {
        var marquee = this.FindControl<IndeterminateMarqueeBar>("Marquee");
        marquee?.Start();
    }

    private void StopMarquee()
    {
        var marquee = this.FindControl<IndeterminateMarqueeBar>("Marquee");
        marquee?.Stop();
    }
}
