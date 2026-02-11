using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Client_App.Interfaces.Logger;
using Client_App.Resources.ConverterType;

namespace Client_App.Resources;

// get display resolution
public partial class RatioConverter : MarkupExtension, IValueConverter
{
    private static RatioConverter? _instance;

    public RatioConverter() { }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    { // do not let the culture default to local to prevent variable outcome re decimal syntax
        var t = value as Type;
        var isHeight = t == typeof(Height);
        try
        {
            var par = float.TryParse(parameter?.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var floatPar)
                ? floatPar
                : 1;

            //if (OperatingSystem.IsWindows())
            {
                var appLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                
                // Try to get the target window from binding context
                Window? targetWindow = null;
                
                // Check if we can get the window from the serviceProvider (if available)
                if (value is IServiceProvider serviceProvider)
                {
                    var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
                    targetWindow = rootProvider?.RootObject as Window;
                }
                
                // Fallback: try to get the active window or main window
                if (targetWindow == null)
                {
                    var mainWindow = appLifetime?.MainWindow;
                    var screens = mainWindow?.Screens;
                    
                    // If we have screens available, try to get the screen where the cursor/current window is
                    if (screens != null)
                    {
                        // In Avalonia 0.10.22, we can't get cursor position, so use primary screen as fallback
                        var targetScreen = screens.Primary;
                        
                        // If cursor screen detection fails, fallback to main window screen
                        if (targetScreen == null && mainWindow != null)
                        {
                            targetScreen = screens.ScreenFromWindow(mainWindow.PlatformImpl);
                        }
                        
                        if (targetScreen != null)
                        {
                            var scale = targetScreen.PixelDensity;
                            var height = targetScreen.WorkingArea.Height * par / scale;
                            var width = targetScreen.WorkingArea.Width * par / scale;
                            
                            return isHeight 
                                ? height
                                : width;
                        }
                    }
                }
                
                // Final fallback to original logic
                var mainWindowFallback = appLifetime?.MainWindow;
                if (mainWindowFallback?.Screens != null)
                {
                    var mainScreen = mainWindowFallback.Screens.ScreenFromWindow(mainWindowFallback.PlatformImpl);
                    var scale = mainScreen.PixelDensity;
                    var height = mainScreen.WorkingArea.Height * par / scale;
                    var width = mainScreen.WorkingArea.Width * par / scale;
                    
                    return isHeight 
                        ? height
                        : width;
                }
                
                // Hardcoded fallback
                return isHeight 
                    ? 600 * par
                    : 800 * par;
            }
            //if (OperatingSystem.IsLinux())
            //{
            //    return isHeight
            //        ? System.Convert.ToInt32(DisplayTools.GetDisplaySizeOnLinux().Height * par)
            //        : System.Convert.ToInt32(DisplayTools.GetDisplaySizeOnLinux().Width * par);
            //}
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }

        return isHeight 
            ? 600 
            : 800;

        //var size = System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter,CultureInfo.InvariantCulture);
        //return size.ToString( "G0", CultureInfo.InvariantCulture );
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    { // read only converter...
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _instance ??= new RatioConverter();
    }
}