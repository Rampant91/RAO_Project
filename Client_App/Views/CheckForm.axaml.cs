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

    private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        var rowNumber = e.Row.GetIndex() + 1;
        e.Row.Header = rowNumber.ToString();
        if (e.Row.DataContext is CheckError dataObject) dataObject.Row = rowNumber.ToString();
    }
}