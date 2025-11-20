using Avalonia;
using Avalonia.ReactiveUI;
using Client_App.Properties;

namespace Client_App;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
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

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    }
}