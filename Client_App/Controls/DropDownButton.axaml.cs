using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Controls;

public partial class DropDownButton : UserControl
{
    public DropDownButton()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}