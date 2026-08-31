using Avalonia;
using Avalonia.ReactiveUI;
using Client_App.Properties;
using System;

namespace Client_App;

internal class Program
{
    public static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            Console.WriteLine("Unhandled exception: " + e.ExceptionObject);
        };

        Settings.Default.AppStartupParameters = string.Join(",", args);
        Settings.Default.Save();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}