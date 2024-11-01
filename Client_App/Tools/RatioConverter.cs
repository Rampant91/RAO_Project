using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Client_App.Interfaces.Logger;
using Client_App.Tools.ConverterType;
using Size = System.Drawing.Size;

namespace Client_App.Tools;

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
                var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow;
                var screens = mainWindow.Screens;
                var mainScreen = screens.ScreenFromWindow(mainWindow);
                var scale = mainScreen.Scaling;
                var height = mainScreen.WorkingArea.Height * par / scale;
                var width = mainScreen.WorkingArea.Width * par / scale;
                //var scale = DisplayTools.GetScalingFactorOnWindows();
                //var height = System.Convert.ToInt32(DisplayTools.GetDisplaySizeOnWindows().Height * par / scale);
                //var width = System.Convert.ToInt32(DisplayTools.GetDisplaySizeOnWindows().Width * par / scale);
                return isHeight
                    ? height
                    : width;
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

        return isHeight ? 600 : 800;

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

internal static partial class DisplayTools
{
    [LibraryImport("gdi32.dll")]
    private static partial int GetDeviceCaps(IntPtr hdc, int nIndex);

    private enum DeviceCap
    {
        DesktopVerticalRes = 117,
        DesktopHorizonRes = 118
    }

    public static Size GetDisplaySizeOnWindows()
    {
        var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow;
        var screens = mainWindow.Screens;
        var mainScreen = screens.ScreenFromWindow(mainWindow);
        return new Size(mainScreen.WorkingArea.Width, mainScreen.WorkingArea.Height);
        ServiceExtension.LoggerManager.Warning($"{mainScreen.WorkingArea.Width}_{mainScreen.WorkingArea.Height}");
        if (!OperatingSystem.IsWindows()) return new Size(600, 800);
        using var g = Graphics.FromHwnd(IntPtr.Zero);
        var desktop = g.GetHdc();

        var physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DesktopVerticalRes);
        var physicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.DesktopHorizonRes);

        return new Size(physicalScreenWidth, physicalScreenHeight);
    }

    public static Size GetDisplaySizeOnLinux()
    {
        // Use xrandr to get size of screen located at offset (0,0).
        var p = new System.Diagnostics.Process();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.FileName = "xrandr";
        p.Start();
        var output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        var match = DisplayRegex().Match(output);
        var w = match.Groups[1].Value;
        var h = match.Groups[2].Value;
        var r = new Size
        {
            Width = int.Parse(w),
            Height = int.Parse(h)
        };
        return r;
    }

    public static float GetScalingFactorOnWindows()
    {
        if (!OperatingSystem.IsWindows()) return 1;
        using var g = Graphics.FromHwnd(IntPtr.Zero);
        var desktop = g.GetHdc();
        var logpixelsy = GetDeviceCaps(desktop, 90);
        g.ReleaseHdc(desktop);

        var dpiScalingFactor = (float)logpixelsy / 96;

        return dpiScalingFactor;
    }

    [GeneratedRegex(@"(\d+)x(\d+)\+0\+0")]
    private static partial Regex DisplayRegex();
}