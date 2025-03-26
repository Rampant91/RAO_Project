using Avalonia.Markup.Xaml;
using Client_App.ViewModels;

namespace Client_App.Views;

public class Calculator : BaseWindow<CalculatorVM>
{
    public Calculator()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}