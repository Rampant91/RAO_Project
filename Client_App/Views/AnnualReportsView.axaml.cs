using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views;
public partial class AnnualReportsView : UserControl
{
    public AnnualReportsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}