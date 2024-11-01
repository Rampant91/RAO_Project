using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Client_App.ViewModels;
using Client_App.Interfaces.Logger;
using Avalonia.Platform;

namespace Client_App.Views;

public abstract class BaseWindow<T> : ReactiveWindow<BaseVM>
{
    public override async void Show()
    {
        base.Show();
        await Task.Delay(1);
        try
        {
            SetWindowStartupLocationWorkaroundForLinux();
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
    }

    private void SetWindowStartupLocationWorkaroundForLinux()
    {
        if(OperatingSystem.IsWindows()) return;
        var scale = 1.0;
        scale = Owner!.DesktopScaling;
        var windowBase = Owner?.PlatformImpl;
        //if (windowBase != null) 
        //{
        //    scale = windowBase.DesktopScaling;
        //}
        var rect = new PixelRect(PixelPoint.Origin, PixelSize.FromSize(ClientSize, scale));
        if (WindowStartupLocation == WindowStartupLocation.CenterScreen)// && Name != "MainWindow") 
        {
            var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow;
            var screens = mainWindow.Screens;
            var screen = screens.ScreenFromWindow(mainWindow);
            //var screen = Name == "MainWindow" ? Screens.Primary : Screens.ScreenFromPoint(windowBase?.Position ?? Position);
            if (screen == null) return;
            Position = screen.WorkingArea.CenterRect(rect).Position;
            //ServiceExtension.LoggerManager.Warning($"{Environment.NewLine}Rect: x: {rect.X}, y: {rect.Y}, w: {rect.Width}, h: {rect.Height}" +
            //                                       $"{Environment.NewLine}Screen: x: {screen.WorkingArea.X}, y: {screen.WorkingArea.Y}, w: {screen.WorkingArea.Width}, h: {screen.WorkingArea.Height}" +
            //                                       $"{Environment.NewLine}Position: {Position.X}_{Position.Y}");
        }
        else 
        {
            if(windowBase == null || WindowStartupLocation != WindowStartupLocation.CenterOwner) return;
            //Position = new PixelRect(Owner.Screens.Screen.WorkingArea.CenterRect(rect).Position, PixelSize.FromSize(Owner.ClientSize, scale))
            //    .CenterRect(rect).Position;
        }
    }
}