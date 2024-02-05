using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Models.CheckForm;

namespace Client_App.Views;

public partial class CheckForm : BaseWindow<CheckFormVM>
{
    public CheckForm() { }
    public CheckForm(ChangeOrCreateVM changeOrCreateVM,List<CheckError> checkError)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new CheckFormVM(changeOrCreateVM, checkError);
        Show();
    }


}