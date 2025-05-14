using Avalonia.Threading;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.Interfaces.Logger;
using MessageBox.Avalonia.DTO;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Открыть файл, расположенный в папке data с программой. В качестве параметра передаётся строчка, определяющая, какой файл необходимо открыть.
/// </summary>
public class OpenFileAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        try
        {
            var filePathInDataFolder = parameter switch
            {
                "manual" => "Инструкция_МПЗФ.pdf",
                "radsDictionary" => Path.Combine("Spravochniki", "R.xlsx"),
                _ => ""
            };

#if DEBUG
            var filePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", filePathInDataFolder);
#else
            var filePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", filePathInDataFolder);
#endif

            Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);

            #region MessageFailedToOpenFile

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Открытие файла",
                    ContentHeader = "Ошибка",
                    ContentMessage = "При попытке открыть файл возникла ошибка.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion
        }
    }
}