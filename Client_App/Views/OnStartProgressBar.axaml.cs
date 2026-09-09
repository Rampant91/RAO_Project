using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System.Threading.Tasks;
using ReactiveUI;
using Client_App.ViewModels;
using Client_App.Interfaces.BackgroundLoader;

namespace Client_App.Views;

public partial class OnStartProgressBar : BaseWindow<OnStartProgressBarVM>
{
    /// <summary>Splash should show immediately (no fade), with correct center/Linux fallback.</summary>
    protected override bool RevealOnOpen => false;

    private bool _suppressExitOnClose;

    public OnStartProgressBar()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        // ProgressBar often marks PointerPressed handled — still allow drag from any chrome area.
        AddHandler(PointerPressedEvent, Chrome_OnPointerPressed, RoutingStrategies.Tunnel, handledEventsToo: true);

        this.WhenActivated(d =>
        {
            var vm = (OnStartProgressBarVM)ViewModel;
            d(vm!.ShowDialog.RegisterHandler(context => DoShowDialogAsync(context).GetAwaiter().GetResult()));
        });
    }

    private async Task DoShowDialogAsync(IInteractionContext<MainWindowVM, object> interaction)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(interaction.Input);
            desktop.MainWindow.Show();
            _suppressExitOnClose = true;
            Close();
        }
        interaction.SetOutput(null!);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new OnStartProgressBarVM(new BackgroundLoader());
    }

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        RequestCancelAndExit();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (!_suppressExitOnClose)
        {
            (DataContext as OnStartProgressBarVM)?.RequestCancelStartup();
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() => desktop.Shutdown(0));
            }
        }

        base.OnClosing(e);
    }

    private void RequestCancelAndExit()
    {
        (DataContext as OnStartProgressBarVM)?.RequestCancelStartup();
        Close();
    }

    private void Chrome_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Button || VisualTreeHelperIsDescendantOfButton(e.Source as Visual))
        {
            return;
        }

        if (WindowState is WindowState.Maximized or WindowState.FullScreen)
        {
            return;
        }

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private static bool VisualTreeHelperIsDescendantOfButton(Visual? visual)
    {
        while (visual is not null)
        {
            if (visual is Button)
            {
                return true;
            }

            visual = visual.GetVisualParent() as Visual;
        }

        return false;
    }
}
