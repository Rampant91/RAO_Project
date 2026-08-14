using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;
using Client_App.Services.Updates;
using Models.DTO;
using System;
using System.Threading.Tasks;

namespace Client_App.Views.Messages;

public partial class NetworkUpdateNotificationWindow : Window
{
    public NetworkUpdateNotificationWindow()
    {
        InitializeComponent();
    }

    public NetworkUpdateNotificationWindow(
        NetworkReleaseInfo release,
        LocalUpdateState localState,
        Func<NetworkReleaseInfo, Task> applyUpdateAsync) : this()
    {
        DataContext = new NetworkUpdateNotificationVM(release, localState, Close, applyUpdateAsync);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
