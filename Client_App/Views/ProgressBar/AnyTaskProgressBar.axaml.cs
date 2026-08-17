using System;
using System.ComponentModel;
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

    public AnyTaskProgressBar()
    {

    }
    public AnyTaskProgressBar(CancellationTokenSource cts, Window? owner = null)
    {
        InitializeComponent();
//#if DEBUG
//        this.AttachDevTools();
//#endif
        _cancellationTokenSource = cts;
        var vm = new AnyTaskProgressBarVM(this, cts, new BackgroundLoader());
        DataContext = vm;
        AnyTaskProgressBarVM = (DataContext as AnyTaskProgressBarVM)!;

        if (owner == null && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (AnyTaskProgressBarVM.IsShowDialog) ShowDialog(desktop.MainWindow);
            else Show(desktop.MainWindow);
        }
        else if (owner != null)
        {
            if (AnyTaskProgressBarVM.IsShowDialog) ShowDialog(owner);
            else Show(owner);
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    #region Events

    private bool _mouseDownForWindowMoving = false;
    private PointerPoint? _originalPoint;

    #region OnPointerMoved

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_mouseDownForWindowMoving) return;

        var currentPoint = e.GetCurrentPoint(this);
        Position = new PixelPoint(Position.X + (int)(currentPoint.Position.X - _originalPoint!.Position.X),
            Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
    }

    #endregion

    #region OnPointerPressed
    
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (WindowState is Maximized or FullScreen) return;

        _mouseDownForWindowMoving = true;
        _originalPoint = e.GetCurrentPoint(this);
    }

    #endregion

    #region OnPointerReleased
    
    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _mouseDownForWindowMoving = false;
    }
    
    #endregion

    #endregion

    /// <summary>
    /// Крестик, Escape и кнопка «Отмена» должны останавливать команду, а не только прятать окно.
    /// </summary>
    protected override void OnClosing(CancelEventArgs e)
    {
        TryCancelLinkedCommand();
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

    public async Task CloseAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(Close);
    }
}