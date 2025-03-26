using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views;

public class RadionuclideCalculation : UserControl
{
    public RadionuclideCalculation()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}