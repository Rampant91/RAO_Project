using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views.Controls;

/// <summary>Полупрозрачный индикатор поверх DataGrid при фоновой загрузке страницы.</summary>
public partial class DataGridLoadingOverlay : UserControl
{
    public DataGridLoadingOverlay()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
