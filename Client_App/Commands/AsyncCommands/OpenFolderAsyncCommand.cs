using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Client_App.Properties;
using MessageBox.Avalonia.DTO;

namespace Client_App.Commands.AsyncCommands;

public class OpenFolderAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var folderPath = "";
        switch (parameter)
        {
            case "app":
            {
                folderPath = AppContext.BaseDirectory;

                var msg = $"{AppContext.BaseDirectory}";
                ServiceExtension.LoggerManager.Warning(msg + "- baseFolder");

                break;
            }
            case "excel":
            {
#if DEBUG
                folderPath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Excel");
#else
                folderPath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Excel");

                ServiceExtension.LoggerManager.Warning(folderPath + "- excelFolder");
#endif
                break;
            }
            case "rao":
            {
                var systemDirectory = Settings.Default.SystemFolderDefaultPath is "default"
                    ? OperatingSystem.IsWindows()
                        ? Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))!
                        : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                    : Settings.Default.SystemFolderDefaultPath;
                folderPath = Path.Combine(systemDirectory, "RAO");

                break;
            }
        }

        try
        {
            Process.Start(new ProcessStartInfo { FileName = folderPath, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);

            #region MessageFailedToOpenFolder

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Открытие папки",
                    ContentHeader = "Ошибка",
                    ContentMessage = "При попытке открыть папку возникла ошибка.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion
        }
    }
}