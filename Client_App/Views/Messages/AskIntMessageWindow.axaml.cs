using System.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;
namespace Client_App;

public partial class AskIntMessageWindow : Window
{
    public AskIntMessageWindow(AskIntMessageVM vm)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = vm;
    }
    public AskIntMessageWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new AskIntMessageVM();
    }
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        int result = ((AskIntMessageVM)DataContext).Result;
        // Return the integer result from ViewModel
        Close(result);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        // Return a cancellation indicator (could use null or sentinel value)
        Close(null);
    }




}