using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Client_App.Behaviors.WindowSizing;

public static class WindowScreenSizeBehavior
{
    public static readonly AttachedProperty<double> WidthRatioProperty =
        AvaloniaProperty.RegisterAttached<Window, double>(
            "WidthRatio",
            typeof(WindowScreenSizeBehavior),
            defaultValue: 0.5,
            defaultBindingMode: BindingMode.OneWay);

    public static readonly AttachedProperty<double> HeightRatioProperty =
        AvaloniaProperty.RegisterAttached<Window, double>(
            "HeightRatio",
            typeof(WindowScreenSizeBehavior),
            defaultValue: 0.5,
            defaultBindingMode: BindingMode.OneWay);

    static WindowScreenSizeBehavior()
    {
        WidthRatioProperty.Changed.AddClassHandler<Window>(OnWidthRatioChanged);
        HeightRatioProperty.Changed.AddClassHandler<Window>(OnHeightRatioChanged);
    }

    public static double GetWidthRatio(Window element) =>
        element.GetValue(WidthRatioProperty);

    public static void SetWidthRatio(Window element, double value) =>
        element.SetValue(WidthRatioProperty, value);

    public static double GetHeightRatio(Window element) =>
        element.GetValue(HeightRatioProperty);

    public static void SetHeightRatio(Window element, double value) =>
        element.SetValue(HeightRatioProperty, value);

    /// <summary>
    /// Пересчитывает Width/Height по ratio на экране owner/MainWindow (с clamp к working area).
    /// Без явно заданных ratio ничего не меняет (чтобы не раздувать окна с фиксированным размером).
    /// </summary>
    public static void RefreshWindowSize(Window window, Window? ownerWindow = null)
    {
        if (!window.IsSet(WidthRatioProperty) && !window.IsSet(HeightRatioProperty))
        {
            return;
        }

        UpdateWindowSize(window, ownerWindow);
    }

    private static void OnWidthRatioChanged(Window window, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is double and > 0)
        {
            UpdateWindowSize(window, ownerWindow: null);
        }
    }

    private static void OnHeightRatioChanged(Window window, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is double and > 0)
        {
            UpdateWindowSize(window, ownerWindow: null);
        }
    }

    private static void UpdateWindowSize(Window window, Window? ownerWindow)
    {
        try
        {
            var targetScreen = WindowScreenContext.ResolveTargetScreen(window, ownerWindow);
            if (targetScreen == null)
            {
                return;
            }

            var widthRatio = GetWidthRatio(window);
            var heightRatio = GetHeightRatio(window);
            if (widthRatio <= 0 || heightRatio <= 0)
            {
                return;
            }

            var (newWidth, newHeight) = WindowScreenContext.ComputeSize(targetScreen, widthRatio, heightRatio);

            if (window.MaxWidth > 0 && !double.IsInfinity(window.MaxWidth))
            {
                newWidth = Math.Min(newWidth, window.MaxWidth);
            }

            if (window.MaxHeight > 0 && !double.IsInfinity(window.MaxHeight))
            {
                newHeight = Math.Min(newHeight, window.MaxHeight);
            }

            // Min* уважаем только в пределах working area — иначе окно выше экрана и шапка пропадает.
            if (window.MinWidth > 0)
            {
                newWidth = Math.Max(newWidth, window.MinWidth);
            }

            if (window.MinHeight > 0)
            {
                newHeight = Math.Max(newHeight, window.MinHeight);
            }

            (newWidth, newHeight) = WindowScreenContext.ClampToWorkingArea(newWidth, newHeight, targetScreen);

            window.Width = newWidth;
            window.Height = newHeight;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"WindowScreenSizeBehavior error: {ex.Message}");
            window.Width = 800;
            window.Height = 600;
        }
    }
}
