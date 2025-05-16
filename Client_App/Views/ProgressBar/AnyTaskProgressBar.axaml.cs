using System.Threading;
using System.Threading.Tasks;
using Avalonia;
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

    public AnyTaskProgressBar()
    {

    }
    public AnyTaskProgressBar(CancellationTokenSource cts)
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        
        var vm = new AnyTaskProgressBarVM(this, cts, new BackgroundLoader());
        DataContext = vm;
        AnyTaskProgressBarVM = (DataContext as AnyTaskProgressBarVM)!;
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (AnyTaskProgressBarVM.IsShowDialog) ShowDialog(desktop.MainWindow);
            else Show(desktop.MainWindow);
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

    public async Task CloseAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(Close);
    }
}