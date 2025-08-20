using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.Commands.AsyncCommands;
using Client_App.Controls.DataGrid;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using System.Text.RegularExpressions;
namespace Client_App;

public partial class Form_12 : BaseWindow<Form_12VM>
{

    //private Form_12VM _vm = null!;

    public Form_12() 
    {
        InitializeComponent();
        DataContext = new Form_12VM();
        Show();
    }

    public Form_12(Form_12VM vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        this.WindowState = WindowState.Maximized;
        
    }

    //¬ременное узкоспециализированное решение
    private void CopyExecutorData_Click(object sender, RoutedEventArgs e)
    {
        var command = new NewCopyExecutorDataAsyncCommand((Form_12VM)DataContext);
        if (command.CanExecute(null))
        {
            command.Execute(null);
        }
    }

}