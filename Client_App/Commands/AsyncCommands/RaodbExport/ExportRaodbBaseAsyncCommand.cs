using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Базовый класс экспорта в файл .RAODB
/// </summary>
public abstract class ExportRaodbBaseAsyncCommand : BaseAsyncCommand
{
    private protected AnyTaskProgressBar ProgressBar;

    private CancellationTokenSource Cts = new();

    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        try
        {
            await Task.Run(() => AsyncExecute(parameter), Cts.Token);
        }
        catch (OperationCanceledException)
        {
            //ignore
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
        IsExecute = false;
    }

    #region CancelCommandAndCloseProgressBarWindow

    /// <summary>
    /// Отмена исполняемой команды и закрытие окна прогрессбара.
    /// </summary>
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    private protected static async Task CancelCommandAndCloseProgressBarWindow(
        CancellationTokenSource cts,
        AnyTaskProgressBar? progressBar = null)
    {
        await cts.CancelAsync();
        if (progressBar is not null) await progressBar.CloseAsync();
        cts.Token.ThrowIfCancellationRequested();
    }

    #endregion

    /// <summary>
    /// Создание временной копии БД в папке temp
    /// </summary>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Путь к временной БД</returns>
    private protected static async Task<string> CreateTempDataBase(
        AnyTaskProgressBar? progressBar,
        CancellationTokenSource cts)
    {
        var index = 0;
        var tmpDbPath = Path.Combine(BaseVM.TmpDirectory, BaseVM.DbFileName + ".RAODB");
        while (File.Exists(tmpDbPath))
        {
            tmpDbPath = Path.Combine(BaseVM.TmpDirectory, BaseVM.DbFileName + $"_{++index}.RAODB");
        }

        try
        {
            File.Copy(Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB"), tmpDbPath);
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.System);

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в .RAODB",
                    ContentHeader = "Уведомление",
                    ContentMessage = "При создании файла временной БД возникла ошибка." +
                                     $"{Environment.NewLine}Операция выгрузки принудительно завершена.",
                    MinHeight = 150,
                    MinWidth = 250,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        return tmpDbPath;
    }
}
