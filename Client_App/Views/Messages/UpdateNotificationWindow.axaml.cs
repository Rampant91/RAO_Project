using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Models.DTO;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

/// <summary>
/// Окно уведомления об обновлении программы
/// </summary>
public partial class UpdateNotificationWindow : Window
{
    public UpdateNotificationWindow()
    {
        InitializeComponent();
    }
    
    public UpdateNotificationWindow(UpdateInfo updateInfo) : this()
    {
        var viewModel = new UpdateNotificationVM(updateInfo, Close);
        DataContext = viewModel;
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}