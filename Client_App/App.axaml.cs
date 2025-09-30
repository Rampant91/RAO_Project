using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client_App.Properties;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private static Mutex? _instanceCheckMutex;
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (!InstanceCheck())
            {
                // Start the message box without waiting for it
                _ = ShowErrorMessageAndExit();
                return;
            }

            desktop.MainWindow = new OnStartProgressBar();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async Task ShowErrorMessageAndExit()
    {
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

        Environment.Exit(0);
    }

    private static bool InstanceCheck()
    {
        if (!Settings.Default.OnlyOneAppInstanceAllowed) return true;
        _instanceCheckMutex = new Mutex(true, "<Client_App>", out var isNew);
        if (!isNew) 
            _instanceCheckMutex.Dispose();
        return isNew;
    }
}