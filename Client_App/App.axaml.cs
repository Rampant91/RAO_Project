using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client_App.Properties;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Client_App.Services;
using System;
using System.Threading;

namespace Client_App;

public class App : Application
{
    private static Mutex? _instanceCheckMutex;

    #region Initialize
    
    public override void Initialize()
    {
        // Инициализация логгера Firebird
        FirebirdLogger.Initialize();
        FirebirdLogger.Log("Application Initialize started");

        AvaloniaXamlLoader.Load(this);

        FirebirdLogger.Log("Application Initialize finished");
    }

    #endregion

    #region OnFrameworkInitializationCompleted
    
    public override async void OnFrameworkInitializationCompleted()
    {
        try
        {
            FirebirdLogger.Log("OnFrameworkInitializationCompleted started");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (!InstanceCheck())
                {
                    FirebirdLogger.Log("Another instance detected, showing message and exiting");

                    #region MessageAppAlreadyOpen

                    await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Запуск программы",
                            ContentHeader = "Ошибка",
                            ContentMessage = "Программа уже запущена ранее и не может быть открыта повторно.",
                            Icon = Icon.Error,
                            MinWidth = 400,
                            MinHeight = 120,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        })
                        .Show();

                    #endregion

                    Environment.Exit(0);
                    return;
                }

                FirebirdLogger.Log("Creating main window OnStartProgressBar");
                desktop.MainWindow = new OnStartProgressBar();
                FirebirdLogger.Log("Main window created");
            }

            base.OnFrameworkInitializationCompleted();
            FirebirdLogger.Log("OnFrameworkInitializationCompleted finished");
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("Error in OnFrameworkInitializationCompleted", ex);
            throw;
        }
    }

    #endregion

    #region InstanceCheck

    private static bool InstanceCheck()
    {
        try
        {
            if (!Settings.Default.OnlyOneAppInstanceAllowed)
            {
                FirebirdLogger.Log("InstanceCheck: multiple instances allowed by settings");
                return true;
            }

            _instanceCheckMutex = new Mutex(true, "<Client_App>", out var isNew);
            if (!isNew)
            {
                FirebirdLogger.Log("InstanceCheck: another instance already running");
                _instanceCheckMutex.Dispose();
            }
            else
            {
                FirebirdLogger.Log("InstanceCheck: this is the first instance");
            }

            return isNew;
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("Error in InstanceCheck", ex);
            return false;
        }
    }

    #endregion
}