using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Client_App.ViewModels;
using Models.CheckForm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Client_App.Views;

public partial class CheckForm : BaseWindow<CheckFormVM>
{
    #region Constructor

    public CheckForm() 
    {
        AvaloniaXamlLoader.Load(this);
    }

    public CheckForm(ChangeOrCreateVM changeOrCreateVM, List<CheckError> checkError)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new CheckFormVM(changeOrCreateVM, checkError);

        var dataGrid = this.Get<DataGrid>("CheckErrorsDataGrid");
        dataGrid.LoadingRow += DataGrid_LoadingRow;

        Show();
        
        // Add delay to let Avalonia finish positioning, then center on owner screen
        Task.Delay(1).ContinueWith(_ => {
            Dispatcher.UIThread.Post(PositionWindowOnOwnerScreen);
        });
    }

    #endregion

    #region Window Positioning

    private void PositionWindowOnOwnerScreen()
    {
        try
        {
            var appLifetime = Application.Current?.ApplicationLifetime as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime;
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
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Устанавливает цвет строки в зависимости от того, является ли ошибка критической.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        if (sender is DataGrid && e.Row is not null)
        {
            e.Row.Background = e.Row.DataContext switch
            {
                CheckError { IsCritical: true } => SolidColorBrush.Parse("#EADBDB"),

                CheckError { IsCritical: false } => e.Row.GetIndex() % 2 == 0
                    ? Brushes.LightBlue
                    : Brushes.White,

                _ => e.Row.Background
            };
        }
    }

    #endregion
}