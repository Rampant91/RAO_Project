using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Client_App.Behaviors
{
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

        public static double GetWidthRatio(Window element)
        {
            return element.GetValue(WidthRatioProperty);
        }

        public static void SetWidthRatio(Window element, double value)
        {
            element.SetValue(WidthRatioProperty, value);
        }

        public static double GetHeightRatio(Window element)
        {
            return element.GetValue(HeightRatioProperty);
        }

        public static void SetHeightRatio(Window element, double value)
        {
            element.SetValue(HeightRatioProperty, value);
        }

        private static void OnWidthRatioChanged(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is double ratio && ratio > 0)
            {
                UpdateWindowSize(window);
            }
        }

        private static void OnHeightRatioChanged(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is double ratio && ratio > 0)
            {
                UpdateWindowSize(window);
            }
        }

        private static void UpdateWindowSize(Window window)
        {
            try
            {
                var screens = window.Screens;
                if (screens == null) return;

                // Get the screen where the main window is located (same logic as CheckForm)
                var targetScreen = screens.Primary;
                
                // Try to get screen from main window first
                try
                {
                    var appLifetime = Application.Current?.ApplicationLifetime as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime;
                    var mainWindow = appLifetime?.MainWindow;
                    
                    if (mainWindow?.PlatformImpl != null)
                    {
                        var screenFromMainWindow = screens.ScreenFromWindow(mainWindow.PlatformImpl);
                        if (screenFromMainWindow != null)
                        {
                            targetScreen = screenFromMainWindow;
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("ScreenFromWindow for main window failed, using primary screen");
                }

                // Try to get screen from window position if window is already initialized
                try
                {
                    if (window.PlatformImpl != null)
                    {
                        var screenFromWindow = screens.ScreenFromWindow(window.PlatformImpl);
                        if (screenFromWindow != null)
                        {
                            targetScreen = screenFromWindow;
                        }
                    }
                }
                catch
                {
                    // Fallback to primary screen if ScreenFromWindow fails
                    System.Diagnostics.Debug.WriteLine("ScreenFromWindow failed, using primary screen");
                }

                // Final fallback to primary screen
                targetScreen ??= screens.Primary;

                if (targetScreen == null) return;

                var widthRatio = GetWidthRatio(window);
                var heightRatio = GetHeightRatio(window);

                // Use PixelDensity for scaling, but handle potential issues
                var scale = 1.0;
                try
                {
                    scale = targetScreen.PixelDensity;
                    if (scale <= 0) scale = 1.0; // Fallback if PixelDensity is invalid
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("PixelDensity access failed, using scale 1.0");
                }

                // Calculate new size with proper error handling
                var workingArea = targetScreen.WorkingArea;
                var newWidth = workingArea.Width * widthRatio / scale;
                var newHeight = workingArea.Height * heightRatio / scale;

                // Apply minimum size constraints to prevent invalid window sizes
                newWidth = Math.Max(newWidth, 200);
                newHeight = Math.Max(newHeight, 150);

                window.Width = newWidth;
                window.Height = newHeight;
            }
            catch (Exception ex)
            {
                // Log error if logger is available
                System.Diagnostics.Debug.WriteLine($"WindowScreenSizeBehavior error: {ex.Message}");
                
                // Fallback to hardcoded reasonable size
                window.Width = 800;
                window.Height = 600;
            }
        }
    }
}
