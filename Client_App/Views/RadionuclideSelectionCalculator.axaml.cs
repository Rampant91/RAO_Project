using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views;

public class RadionuclideSelectionCalculator : UserControl
{
    public RadionuclideSelectionCalculator()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}