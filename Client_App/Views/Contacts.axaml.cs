using Avalonia.Markup.Xaml;
using Client_App.ViewModels;

namespace Client_App.Views;

public class Contacts : BaseWindow<BaseVM>
{
    public Contacts()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new BaseVM();
    }
}