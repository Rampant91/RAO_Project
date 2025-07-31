using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;

namespace Client_App;

public partial class Form_12 : BaseWindow<Form_12VM>
{

    //private Form_12VM _vm = null!;

    public Form_12() 
    {
        InitializeComponent();
        Show();
    }

    public Form_12(Form_12VM vm)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = vm;
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new Form_12VM();
    }
}