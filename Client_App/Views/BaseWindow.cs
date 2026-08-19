using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Client_App.Behaviors.WindowSizing;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Views;

public interface IFormDialogHost
{
    Task ShowFormDialogAsync(Window? owner);
}

public abstract class BaseWindow<T> : ReactiveWindow<BaseVM>, IFormDialogHost where T : class
{
    public T? VM => DataContext as T;
    public WindowState OwnerPrevState;

    protected virtual bool IsFullScreenWindow => false;

    /// <summary>
    /// Задаёт финальный размер/состояние до показа окна, чтобы избежать «прыжка» layout.
    /// </summary>
    public void PrepareBeforeShow(Window? owner = null)
    {
        if (owner != null)
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

        if (IsFullScreenWindow)
        {
            WindowState = WindowState.Maximized;
            return;
        }

        if (WindowState is WindowState.Maximized or WindowState.FullScreen)
            return;

        // WindowScreenSizeBehavior (Form_10/Form_20 и др.) уже мог выставить размер при загрузке XAML.
        if (Width > 0 && !double.IsNaN(Width) && Height > 0 && !double.IsNaN(Height))
            return;

        // Пересчёт через behavior: учитывает WorkingArea, PixelDensity и Linux-fallback.
        WindowScreenSizeBehavior.RefreshWindowSize(this);

        if (Width > 0 && !double.IsNaN(Width) && Height > 0 && !double.IsNaN(Height))
            return;

        if (Width <= 0 || double.IsNaN(Width))
            Width = MinWidth > 0 ? MinWidth : 800;

        if (Height <= 0 || double.IsNaN(Height))
            Height = MinHeight > 0 ? MinHeight : 600;
    }

    public async Task ShowFormDialogAsync(Window? owner)
    {
        PrepareBeforeShow(owner);
        AttachFullscreenFallback();
        AttachRevealOnOpen();
        if (owner is MainWindow mainWindow)
            mainWindow.SetReportOpeningOverlay(false);

        if (owner != null)
            await ShowDialog(owner);
    }

    private void AttachRevealOnOpen()
    {
        Opacity = 0;
        Opened += OnRevealAfterOpen;
    }

    /// <summary>
    /// На части Linux/Wayland WM Maximized до Show() не применяется — дублируем в Opened (как было раньше).
    /// </summary>
    private void AttachFullscreenFallback()
    {
        if (!IsFullScreenWindow)
            return;

        Opened += OnEnsureMaximizedOnOpen;
    }

    private void OnEnsureMaximizedOnOpen(object? sender, EventArgs e)
    {
        Opened -= OnEnsureMaximizedOnOpen;
        if (WindowState != WindowState.Maximized)
            WindowState = WindowState.Maximized;
    }

    private void OnRevealAfterOpen(object? sender, EventArgs e)
    {
        Opened -= OnRevealAfterOpen;
        Opacity = 1;
    }

    public override async void Show()
    {
        PrepareBeforeShow();
        AttachFullscreenFallback();
        AttachRevealOnOpen();
        base.Show();
        await Task.Delay(1).ContinueWith(_ =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (!IsFullScreenWindow)
                    PositionWindowOnOwnerScreen();
            });
        });
    }

    #region PositionWindowOnOwnerScreen

    protected virtual void PositionWindowOnOwnerScreen()
    {
        try
        {
            if (WindowState is WindowState.Maximized or WindowState.FullScreen) return;

            var ownerWindow = GetOwnerWindow();

            if (ownerWindow?.Screens == null) return;

            // Get the screen where the OWNER window is located
            Screen? ownerScreen = null;

            // First try to get screen from owner window
            if (ownerWindow.PlatformImpl != null)
            {
                try
                {
                    ownerScreen = ownerWindow.Screens.ScreenFromWindow(ownerWindow.PlatformImpl);
                }
                catch
                {
                    // Fallback for Linux if ScreenFromWindow fails
                }
            }

            // Fallback to primary screen
            if (ownerScreen == null)
            {
                ownerScreen = ownerWindow.Screens.Primary;
            }

            if (ownerScreen != null)
            {
                var scale = 1.0;
                try
                {
                    scale = ownerScreen.PixelDensity;
                    if (scale <= 0) scale = 1.0;
                }
                catch
                {
                    // Fallback for Linux if PixelDensity access fails
                }

                var windowWidth = Width;
                var windowHeight = Height;

                // Calculate center position in physical pixels
                var centerX = ownerScreen.WorkingArea.X + (ownerScreen.WorkingArea.Width - windowWidth * scale) / 2;
                var centerY = ownerScreen.WorkingArea.Y + (ownerScreen.WorkingArea.Height - windowHeight * scale) / 2;

                Position = new PixelPoint((int)Math.Round(centerX), (int)Math.Round(centerY));
            }
        }
        catch (Exception ex)
        {
            // Fallback to default behavior if positioning fails
            // Let the window use default positioning (will be centered on primary screen)
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
    }

    private Window? GetOwnerWindow()
    {
        // Try to find the most recent active window that could be the owner
        var appLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var windows = appLifetime?.Windows;

        if (windows == null) return null;

        // Find windows that are not this window and have been activated recently
        var candidateWindows = new List<Window>();
        foreach (var window in windows)
        {
            if (window != this && window.WindowState != WindowState.Minimized)
            {
                candidateWindows.Add(window);
            }
        }

        // If we have candidate windows, return last not MainWindow, if we have only MainWindow, return it
        if (candidateWindows.Count > 0)
        {
            var notMainWindowCandidates = candidateWindows
                .Where(x => x.Name != "MainWindow")
                .ToList();

            return notMainWindowCandidates.Count > 0
                ? notMainWindowCandidates.LastOrDefault()
                : candidateWindows.LastOrDefault();
        }
            
        // Fallback: if all windows are minimized, return the main window
        var mainWindow = appLifetime.MainWindow;
        if (mainWindow != null && mainWindow != this)
        {
            return mainWindow;
        }

        return null;
    }

    #endregion
}