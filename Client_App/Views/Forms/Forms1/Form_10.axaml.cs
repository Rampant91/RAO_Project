using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Forms.Forms1;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_10 : BaseWindow<Form_10VM>
{
    private Form_10VM _vm = null!;

    public Form_10() { }

    public Form_10(Form_10VM vm)
    {
        AvaloniaXamlLoader.Load(this);
        _vm = vm;
    }
}