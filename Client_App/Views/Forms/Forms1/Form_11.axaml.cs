using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;

namespace Client_App;

public partial class Form_11 : BaseWindow<Form_11VM>
{
    public Form_11()
    {
        InitializeComponent();
        Show();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new Form_11VM();
    }
}