using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public partial class AskIntMessageWindow : BaseWindow<AskIntMessageVM>
{
    public AskIntMessageWindow(AskIntMessageVM vm)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = vm;
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var textbox = this.FindControl<TextBox>("Textbox");
            textbox.Focus();
        }, DispatcherPriority.Loaded);
    }

    public AskIntMessageWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new AskIntMessageVM();

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var textbox = this.FindControl<TextBox>("Textbox");
            textbox.Focus();
        }, DispatcherPriority.Loaded);
    }
    
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        var result = ((AskIntMessageVM)DataContext).Result;
        // Return the integer result from ViewModel
        if (result == null)
            Close(0);
        else
            Close((int)result);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        // Return a cancellation indicator (could use null or sentinel value)
        Close(null);
    }
}