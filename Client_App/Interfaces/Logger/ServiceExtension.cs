using Microsoft.Extensions.DependencyInjection;

namespace Client_App.Interfaces.Logger;

public abstract class ServiceExtension
{
    private static readonly ServiceProvider ServiceProvider = new ServiceCollection()
        .AddTransient<IFileManager, BaseFileManager>()
        .AddTransient<ILogFactory, BaseLoggerFactory>()
        .BuildServiceProvider();

    public static readonly IFileManager FileManager = ServiceProvider.GetService<IFileManager>();

    public static readonly ILogFactory LoggerManager = ServiceProvider.GetService<ILogFactory>();
}