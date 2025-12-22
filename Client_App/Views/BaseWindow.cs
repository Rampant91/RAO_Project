using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using System;
using System.Threading.Tasks;

namespace Client_App.Views;

public abstract class BaseWindow<T> : ReactiveWindow<BaseVM>
{
    public WindowState OwnerPrevState;

    public override async void Show()
    {
        base.Show();

        await Task.Delay(1).ContinueWith(_ => { Dispatcher.UIThread.Post(PositionWindowOnOwnerScreen); });
    }

    #region PositionWindowOnOwnerScreen

    private void PositionWindowOnOwnerScreen()
    {
        try
        {
            var appLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            var mainWindow = appLifetime?.MainWindow;

            if (mainWindow?.Screens != null)
            {
                // Get the screen where the MAIN window is located
                Screen? ownerScreen = null;

                // First try to get screen from main window
                if (mainWindow.PlatformImpl != null)
                {
                    try
                    {
                        ownerScreen = mainWindow.Screens.ScreenFromWindow(mainWindow.PlatformImpl);
                    }
                    catch
                    {
                        // Fallback for Linux if ScreenFromWindow fails
                    }
                }

                // Fallback to primary screen
                if (ownerScreen == null)
                {
                    ownerScreen = mainWindow.Screens.Primary;
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

    #endregion
}