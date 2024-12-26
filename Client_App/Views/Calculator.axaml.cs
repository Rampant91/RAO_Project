using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Client_App.Views;

namespace Client_App;

public partial class Calculator : BaseWindow<CalculatorVM>
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