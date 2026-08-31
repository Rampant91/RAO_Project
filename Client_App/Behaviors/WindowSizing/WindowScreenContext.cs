using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;

namespace Client_App.Behaviors.WindowSizing;

/// <summary>
/// Единый безопасный выбор экрана, плотности и clamp размера/позиции (Windows + Linux/Astra).
/// Экран берётся от owner / текущего MainWindow при каждом вызове — после перетаскивания MainWindow
/// дочерние окна открываются уже на новом мониторе.
/// </summary>
internal static class WindowScreenContext
{
    private const double MinWidthDip = 200;
    private const double MinHeightDip = 150;
    private const double WorkingAreaFillRatio = 0.95;

    internal static Screen? ResolveTargetScreen(Window window, Window? ownerWindow)
    {
        try
        {
            var screens = window.Screens ?? ownerWindow?.Screens;
            if (screens == null)
            {
                return null;
            }

            Screen? target = TryScreenFromWindow(screens, ownerWindow)
                             ?? TryScreenFromWindow(screens, window)
                             ?? TryScreenFromMainWindow(screens);

            return target ?? screens.Primary;
        }
        catch
        {
            return null;
        }
    }

    internal static double TryGetPixelDensity(Screen? screen)
    {
        if (screen == null)
        {
            return 1.0;
        }

        try
        {
            var density = screen.Scaling;
            return density > 0 ? density : 1.0;
        }
        catch
        {
            // Linux/Astra: PixelDensity иногда недоступен.
            return 1.0;
        }
    }

    internal static (double Width, double Height) ComputeSize(
        Screen screen,
        double widthRatio,
        double heightRatio)
    {
        var scale = TryGetPixelDensity(screen);
        var workingArea = screen.WorkingArea;
        var width = workingArea.Width * Math.Clamp(widthRatio, 0.05, 1.0) / scale;
        var height = workingArea.Height * Math.Clamp(heightRatio, 0.05, 1.0) / scale;
        return ClampToWorkingArea(width, height, screen);
    }

    /// <summary>
    /// Не даёт окну быть больше working area. Маленькие окна (splash 300×30) не раздувает.
    /// </summary>
    internal static (double Width, double Height) ClampToWorkingArea(
        double width,
        double height,
        Screen screen)
    {
        var scale = TryGetPixelDensity(screen);
        var workingArea = screen.WorkingArea;
        var maxWidth = Math.Max(1, workingArea.Width * WorkingAreaFillRatio / scale);
        var maxHeight = Math.Max(1, workingArea.Height * WorkingAreaFillRatio / scale);

        if (width <= 0 || double.IsNaN(width))
        {
            width = Math.Min(MinWidthDip, maxWidth);
        }

        if (height <= 0 || double.IsNaN(height))
        {
            height = Math.Min(MinHeightDip, maxHeight);
        }

        width = Math.Min(width, maxWidth);
        height = Math.Min(height, maxHeight);
        return (Math.Max(1, width), Math.Max(1, height));
    }

    /// <summary>
    /// Держит окно в пределах working area: шапка всегда на экране (Y >= WorkingArea.Y).
    /// </summary>
    internal static PixelPoint ClampPositionToWorkingArea(
        PixelPoint position,
        double windowWidthDip,
        double windowHeightDip,
        Screen screen)
    {
        var scale = TryGetPixelDensity(screen);
        var wa = screen.WorkingArea;
        var wPx = Math.Max(1, (int)Math.Round(windowWidthDip * scale));
        var hPx = Math.Max(1, (int)Math.Round(windowHeightDip * scale));

        int x;
        int y;

        if (wPx >= wa.Width)
        {
            x = wa.X;
        }
        else
        {
            x = Math.Clamp(position.X, wa.X, wa.X + wa.Width - wPx);
        }

        // Критично: верхний край (шапка) не выше working area.
        if (hPx >= wa.Height)
        {
            y = wa.Y;
        }
        else
        {
            y = Math.Clamp(position.Y, wa.Y, wa.Y + wa.Height - hPx);
        }

        return new PixelPoint(x, y);
    }

    internal static bool TryCenterOnScreen(Window window, Window? ownerWindow)
    {
        try
        {
            if (window.WindowState is WindowState.Maximized or WindowState.FullScreen)
            {
                return false;
            }

            var targetScreen = ResolveTargetScreen(window, ownerWindow);
            if (targetScreen == null)
            {
                return false;
            }

            var scale = TryGetPixelDensity(targetScreen);
            var windowWidth = window.Width > 0 && !double.IsNaN(window.Width) ? window.Width : window.MinWidth;
            var windowHeight = window.Height > 0 && !double.IsNaN(window.Height) ? window.Height : window.MinHeight;

            if (windowWidth <= 0 || windowHeight <= 0)
            {
                return false;
            }

            // Если Min* больше экрана — сначала ужимаем размер, иначе центр уедет за верхний край.
            var (safeW, safeH) = ClampToWorkingArea(windowWidth, windowHeight, targetScreen);
            if (Math.Abs(safeW - windowWidth) > 0.5 || Math.Abs(safeH - windowHeight) > 0.5)
            {
                window.Width = safeW;
                window.Height = safeH;
                windowWidth = safeW;
                windowHeight = safeH;
            }

            var workingArea = targetScreen.WorkingArea;
            var centerX = workingArea.X + (workingArea.Width - windowWidth * scale) / 2;
            var centerY = workingArea.Y + (workingArea.Height - windowHeight * scale) / 2;

            var centered = new PixelPoint((int)Math.Round(centerX), (int)Math.Round(centerY));
            window.Position = ClampPositionToWorkingArea(centered, windowWidth, windowHeight, targetScreen);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Подстраховка после Show/Opened: вернуть шапку в visible area (Linux WM / CenterScreen).
    /// </summary>
    internal static bool TryEnsureVisibleOnScreen(Window window, Window? ownerWindow)
    {
        try
        {
            if (window.WindowState is WindowState.Maximized or WindowState.FullScreen)
            {
                return false;
            }

            var targetScreen = ResolveTargetScreen(window, ownerWindow);
            if (targetScreen == null)
            {
                return false;
            }

            var windowWidth = window.Width > 0 && !double.IsNaN(window.Width) ? window.Width : window.Bounds.Width;
            var windowHeight = window.Height > 0 && !double.IsNaN(window.Height) ? window.Height : window.Bounds.Height;
            if (windowWidth <= 0 || windowHeight <= 0)
            {
                return false;
            }

            var (safeW, safeH) = ClampToWorkingArea(windowWidth, windowHeight, targetScreen);
            if (Math.Abs(safeW - windowWidth) > 0.5 || Math.Abs(safeH - windowHeight) > 0.5)
            {
                window.Width = safeW;
                window.Height = safeH;
                windowWidth = safeW;
                windowHeight = safeH;
            }

            var clamped = ClampPositionToWorkingArea(window.Position, windowWidth, windowHeight, targetScreen);
            if (clamped != window.Position)
            {
                window.Position = clamped;
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    internal static bool IsLikelyUnpositioned(PixelPoint position) =>
        position.X <= 1 && position.Y <= 1;

    private static Screen? TryScreenFromWindow(Screens screens, Window? window)
    {
        if (window == null)
        {
            return null;
        }

        try
        {
            return screens.ScreenFromWindow(window);
        }
        catch
        {
            return null;
        }
    }

    private static Screen? TryScreenFromMainWindow(Screens screens)
    {
        try
        {
            var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
                ?.MainWindow;
            return TryScreenFromWindow(screens, mainWindow);
        }
        catch
        {
            return null;
        }
    }
}
