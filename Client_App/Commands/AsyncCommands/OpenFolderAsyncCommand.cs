using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MsBox.Avalonia.Dto;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands;

public class OpenFolderAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var folderPath = ResolveFolderPath(parameter);
        if (string.IsNullOrEmpty(folderPath))
            return;

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

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Открытие папки",
                    ContentHeader = "Ошибка",
                    ContentMessage = "При попытке открыть папку возникла ошибка.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion
        }
    }

    private static string? ResolveFolderPath(object? parameter) => parameter switch
    {
        "app" => AppContext.BaseDirectory.TrimEnd(Path.AltDirectorySeparatorChar).TrimEnd(Path.DirectorySeparatorChar),
        "excel" =>
#if DEBUG
            Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Excel"),
#else
            Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Excel"),
#endif
        "rao" => BaseVM.GetRaoDirectoryPath(),
        string path when Directory.Exists(path) => path,
        _ => null
    };
}
