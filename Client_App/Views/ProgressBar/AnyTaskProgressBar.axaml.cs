using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.ViewModels.ProgressBar;

namespace Client_App.Views.ProgressBar;

public partial class AnyTaskProgressBar : BaseWindow<AnyTaskProgressBarVM>
{
    public AnyTaskProgressBarVM AnyTaskProgressBarVM { get; }
    private readonly CancellationTokenSource? _cancellationTokenSource;
    private bool _suppressCancelOnClose;

    protected override bool RevealOnOpen => false;

    public AnyTaskProgressBar()
    {
    }

    public AnyTaskProgressBar(CancellationTokenSource cts, Window? owner = null, bool isShowDialog = false)
    {
        InitializeComponent();
        _cancellationTokenSource = cts;
        var vm = new AnyTaskProgressBarVM(this, cts, new BackgroundLoader(), isShowDialog);
        DataContext = vm;
        AnyTaskProgressBarVM = (AnyTaskProgressBarVM)DataContext!;

        // ProgressBar often marks PointerPressed handled — still allow drag from chrome.
        AddHandler(PointerPressedEvent, Chrome_OnPointerPressed, RoutingStrategies.Tunnel, handledEventsToo: true);

        var showOwner = owner;
        if (showOwner == null && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            showOwner = desktop.MainWindow;
        }

        // Owner keeps us above the main window without Topmost over system/file dialogs.
        if (showOwner != null && showOwner != this)
        {
            Owner = showOwner;
        }

        if (AnyTaskProgressBarVM.IsShowDialog && showOwner != null)
        {
            PrepareBeforeShow(showOwner);
            AttachOpenedPositionFallback(showOwner);
            Opacity = 1;
            ShowDialog(showOwner);
        }
        else
        {
            ShowCentered(showOwner);
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    #region Events

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

    #endregion

    /// <summary>
    /// Крестик, Escape и закрытие окна должны останавливать команду, а не только прятать окно.
    /// </summary>
    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (!_suppressCancelOnClose)
        {
            TryCancelLinkedCommand();
        }

        base.OnClosing(e);
    }

    private void TryCancelLinkedCommand()
    {
        var cts = _cancellationTokenSource ?? AnyTaskProgressBarVM?.CancellationTokenSource;
        if (cts is null)
        {
            return;
        }

        try
        {
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
        }
        catch (ObjectDisposedException)
        {
            // Команда уже завершилась и освободила токен.
        }
    }

    /// <summary>
    /// Поднять окно над owner после модального диалога, без постоянного Topmost.
    /// </summary>
    public async Task BringToForegroundAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (!IsVisible)
            {
                return;
            }

            Topmost = false;
            Activate();
        });
    }

    /// <summary>
    /// Перед модальным диалогом на MainWindow: спрятать прогресс, чтобы он не оказался
    /// disabled/под диалогом и не мешал SaveFileDialog.
    /// </summary>
    public async Task PrepareForExternalDialogAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (!IsVisible)
            {
                return;
            }

            Topmost = false;
            Hide();
        });
    }

    /// <summary>
    /// После закрытия внешнего диалога — снова показать прогресс над главным окном.
    /// </summary>
    public async Task RestoreAfterExternalDialogAsync(Window? owner = null)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            var showOwner = owner;
            if (showOwner == null && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                showOwner = desktop.MainWindow;
            }

            if (showOwner != null && showOwner != this)
            {
                Owner = showOwner;
            }

            Topmost = false;
            if (!IsVisible)
            {
                Show();
            }

            Activate();
        });
    }

    public async Task CloseAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(Close);
    }

    /// <summary>
    /// Закрытие после успешного завершения операции — без отмены команды (иначе не покажется итоговый диалог).
    /// </summary>
    public async Task CloseCompletedAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _suppressCancelOnClose = true;
            Close();
        });
    }
}
