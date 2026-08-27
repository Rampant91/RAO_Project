using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views.Controls;

/// <summary>Полупрозрачный индикатор поверх DataGrid при фоновой загрузке страницы.</summary>
public partial class DataGridLoadingOverlay : UserControl
{
    public DataGridLoadingOverlay()
    {
        InitializeComponent();
        PropertyChanged += OnOverlayVisibilityChanged;
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void OnOverlayVisibilityChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != IsVisibleProperty)
            return;

        var marquee = this.FindControl<IndeterminateMarqueeBar>("Marquee");
        if (e.NewValue is true)
            marquee?.Start();
        else
            marquee?.Stop();
    }
}
