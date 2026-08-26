using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;

namespace Client_App.Behaviors.WindowSizing;

/// <summary>
/// Расчёт экрана и центральной позиции окна (Windows + Linux/Wayland fallback).
/// </summary>
internal static class WindowCenterPlacement
{
    internal static Screen? ResolveOwnerScreen(Window window, Window? ownerWindow)
    {
        var screens = window.Screens ?? ownerWindow?.Screens;
        if (screens == null)
        {
            return null;
        }

        Screen? targetScreen = null;

        if (ownerWindow?.PlatformImpl != null)
        {
            try
            {
                targetScreen = screens.ScreenFromWindow(ownerWindow.PlatformImpl);
            }
            catch
            {
                // Linux/Wayland: ScreenFromWindow может быть недоступен до первого кадра.
            }
        }

        if (targetScreen == null && window.PlatformImpl != null)
        {
            try
            {
                targetScreen = screens.ScreenFromWindow(window.PlatformImpl);
            }
            catch
            {
                // ignore
            }
        }

        if (targetScreen == null)
        {
            try
            {
                var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                if (mainWindow?.PlatformImpl != null)
                {
                    targetScreen = screens.ScreenFromWindow(mainWindow.PlatformImpl);
                }
            }
            catch
            {
                // ignore
            }
        }

        return targetScreen ?? screens.Primary;
    }

    internal static bool TryCenterOnScreen(Window window, Window? ownerWindow)
    {
        try
        {
            if (window.WindowState is WindowState.Maximized or WindowState.FullScreen)
            {
                return false;
            }

            var targetScreen = ResolveOwnerScreen(window, ownerWindow);
            if (targetScreen == null)
            {
                return false;
            }

            var scale = 1.0;
            try
            {
                scale = targetScreen.PixelDensity;
                if (scale <= 0)
                {
                    scale = 1.0;
                }
            }
            catch
            {
                // Linux: PixelDensity иногда недоступен.
            }

            var windowWidth = window.Width > 0 && !double.IsNaN(window.Width) ? window.Width : window.MinWidth;
            var windowHeight = window.Height > 0 && !double.IsNaN(window.Height) ? window.Height : window.MinHeight;

            if (windowWidth <= 0 || windowHeight <= 0)
            {
                return false;
            }

            var workingArea = targetScreen.WorkingArea;
            var centerX = workingArea.X + (workingArea.Width - windowWidth * scale) / 2;
            var centerY = workingArea.Y + (workingArea.Height - windowHeight * scale) / 2;

            window.Position = new PixelPoint((int)Math.Round(centerX), (int)Math.Round(centerY));
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal static bool IsLikelyUnpositioned(PixelPoint position) =>
        position.X <= 1 && position.Y <= 1;
}
