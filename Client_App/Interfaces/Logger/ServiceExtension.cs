using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Client_App.Interfaces.Logger;
public class ServiceExtension
{
    public static ServiceProvider ServiceProvider = new ServiceCollection()
        .AddTransient<IFileManager, BaseFileManager>()
        .AddTransient<ILogFactory, BaseLoggerFactory>()
        .BuildServiceProvider();
    public static IFileManager FileManager = ServiceProvider.GetService<IFileManager>();
    public static ILogFactory LoggerManager = ServiceProvider.GetService<ILogFactory>();
}