using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Views;

public abstract class BaseWindow<T> : ReactiveWindow<BaseVM>
{
    public WindowState OwnerPrevState;

    protected virtual bool IsFullScreenWindow => false;
    public override async void Show()
    {
        base.Show();
        Opened += OnOpenedForBase;
        await Task.Delay(1).ContinueWith(_ =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (!IsFullScreenWindow)
                {
                    PositionWindowOnOwnerScreen();
                }
            });
        });
    }

    private void OnOpenedForBase(object? sender, EventArgs e)
    {
        if (IsFullScreenWindow)
        {
            // здесь единая логика разворота на весь экран (Maximized/FullScreen)
            WindowState = WindowState.Maximized; // или FullScreen
        }
        Opened -= OnOpenedForBase;
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
                // Get DPI scaling factor
                var scale = ownerScreen.PixelDensity;
                if (scale <= 0) scale = 1.0;

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