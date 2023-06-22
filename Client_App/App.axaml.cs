using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client_App.Views;
using System.Threading;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using MessageBox.Avalonia.Enums;

namespace Client_App;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private static Mutex? _instanceCheckMutex;
    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (!InstanceCheck())
            {
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
            }
            desktop.MainWindow = new OnStartProgressBar();
        }
        base.OnFrameworkInitializationCompleted();
    }

    private static bool InstanceCheck()
    {
        _instanceCheckMutex = new Mutex(true, "<Client_App>", out var isNew);
        if (!isNew) 
            _instanceCheckMutex.Dispose();
        return isNew;
    }
}