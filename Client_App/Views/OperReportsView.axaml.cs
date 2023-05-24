using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views;
public partial class OperReportsView : UserControl
{
    public OperReportsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}