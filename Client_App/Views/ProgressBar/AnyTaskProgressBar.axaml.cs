using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.ViewModels.ProgressBar;
using static Avalonia.Controls.WindowState;

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

        var showOwner = owner;
        if (showOwner == null && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            showOwner = desktop.MainWindow;
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

    private bool _mouseDownForWindowMoving;
    private Point _dragStart;

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_mouseDownForWindowMoving)
        {
            return;
        }

        var currentPosition = e.GetPosition(this);
        var startPosition = _dragStart;
        Position = new PixelPoint(
            Position.X + (int)(currentPosition.X - startPosition.X),
            Position.Y + (int)(currentPosition.Y - startPosition.Y));
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (WindowState is Maximized or FullScreen)
        {
            return;
        }

        _mouseDownForWindowMoving = true;
        _dragStart = e.GetPosition(this);
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _mouseDownForWindowMoving = false;
    }

    #endregion

    /// <summary>
    /// Крестик, Escape и кнопка «Отмена» должны останавливать команду, а не только прятать окно.
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

    public async Task BringToForegroundAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (!IsVisible)
            {
                return;
            }

            Topmost = true;
            Activate();

            // После модального диалога (выбор папки и т.п.) WM часто поднимает owner диалога — возвращаем Z-order.
            if (OperatingSystem.IsWindows())
            {
                Topmost = false;
                Topmost = true;
            }
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
