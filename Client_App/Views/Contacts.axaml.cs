using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.ViewModels;

namespace Client_App.Views;

public partial class Contacts : BaseWindow<BaseVM>
{
    public Contacts()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new BaseVM();
    }

    private void OnCopyLinkClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem { Tag: string text } || string.IsNullOrWhiteSpace(text))
            return;

        // Defer until after the context menu closes — otherwise Windows clipboard write can be dropped.
        Dispatcher.UIThread.Post(async () =>
        {
            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            if (clipboard is null)
                return;

            await clipboard.SetTextAsync(text);
        });
    }
}
