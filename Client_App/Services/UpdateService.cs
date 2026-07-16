using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Properties;
using Client_App.Views.Messages;
using MessageBox.Avalonia.DTO;
using Models.DTO;

namespace Client_App.Services;

/// <summary>
/// Сервис для управления проверками обновлений
/// </summary>
public class UpdateService
{
    private readonly UpdateChecker _updateChecker = new();

    /// <summary>
    /// Проверяет и уведомляет об обновлениях
    /// </summary>
    /// <param name="isDeveloperMode">Режим разработчика</param>
    /// <returns>Task</returns>
    public async Task CheckAndNotifyAsync(bool isDeveloperMode = false)
    {
        try
        {
            if (!ShouldCheckForUpdates())
            {
                return;
            }

            var updateInfo = await _updateChecker.CheckForUpdatesAsync().ConfigureAwait(false);
            if (updateInfo == null)
            {
                return;
            }

            MarkUpdateCheckCompleted();

            var skippedVersion = GetSkippedVersion();
            if (skippedVersion != null && updateInfo.Version <= skippedVersion)
            {
                return;
            }

            var currentVersion = UpdateChecker.GetCurrentVersion();
            if (updateInfo.Version > currentVersion)
            {
                if (isDeveloperMode)
                {
                    await ShowAutoUpdateDialog(updateInfo).ConfigureAwait(false);
                }
                else
                {
                    await ShowUpdateNotificationDialog(updateInfo).ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Update service error: {ex.Message}");
        }
    }

    /// <summary>
    /// Принудительно проверяет обновления (для ручного запуска из меню)
    /// </summary>
    /// <param name="isDeveloperMode">Режим разработчика</param>
    /// <returns>Task</returns>
    public async Task ManualCheckAndNotifyAsync(bool isDeveloperMode = false)
    {
        try
        {
            var updateInfo = await _updateChecker.CheckForUpdatesAsync().ConfigureAwait(false);
            if (updateInfo == null)
            {
                await ShowManualCheckFailedDialog().ConfigureAwait(false);
                return;
            }

            MarkUpdateCheckCompleted();

            var currentVersion = UpdateChecker.GetCurrentVersion();
            if (updateInfo.Version > currentVersion)
            {
                if (isDeveloperMode)
                {
                    await ShowAutoUpdateDialog(updateInfo).ConfigureAwait(false);
                }
                else
                {
                    await ShowUpdateNotificationDialog(updateInfo).ConfigureAwait(false);
                }
            }
            else
            {
                await ShowUpToDateDialog(currentVersion).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Manual update check error: {ex.Message}");
            await ShowManualCheckFailedDialog().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Проверяет, нужно ли выполнять проверку обновлений
    /// </summary>
    /// <returns>True если нужно проверить обновления</returns>
    private static bool ShouldCheckForUpdates()
    {
#if DEBUG
        //Проверка всегда будет выполняться в дебаге, для тестирования
        Settings.Default.LastUpdateCheck = DateTime.MinValue;
        Settings.Default.SkippedVersion = "";
        Settings.Default.Save();
#endif

        var lastCheck = Settings.Default.LastUpdateCheck;
        var now = DateTime.Now;

        // Проверяем не чаще раза в день
        return (now - lastCheck).TotalDays >= 1;
    }

    private static void MarkUpdateCheckCompleted()
    {
        Settings.Default.LastUpdateCheck = DateTime.Now;
        Settings.Default.Save();
    }

    /// <summary>
    /// Показывает диалог уведомления об обновлении
    /// </summary>
    /// <param name="updateInfo">Информация об обновлении</param>
    /// <returns>Task</returns>
    private static async Task ShowUpdateNotificationDialog(UpdateInfo updateInfo)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            try
            {
                var updateWindow = new UpdateNotificationWindow(updateInfo);
                var mainWindow = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                await updateWindow.ShowDialog(mainWindow?.MainWindow);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to show update dialog: {ex.Message}");
            }
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Показывает диалог автообновления (для отдела)
    /// </summary>
    /// <param name="updateInfo">Информация об обновлении</param>
    /// <returns>Task</returns>
    private static async Task ShowAutoUpdateDialog(UpdateInfo updateInfo)
    {
        // Здесь можно реализовать автообновление из сетевой папки
        // Пока просто показываем уведомление
        await ShowUpdateNotificationDialog(updateInfo).ConfigureAwait(false);
    }

    private static async Task ShowUpToDateDialog(Version currentVersion)
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Проверка обновлений",
                ContentMessage = $"У вас установлена последняя версия ПО «МПЗФ» — {currentVersion}.",
                MinWidth = 400,
                MinHeight = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(GetMainWindow())).ConfigureAwait(false);
    }

    private static async Task ShowManualCheckFailedDialog()
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Проверка обновлений",
                ContentMessage = "Не удалось проверить наличие обновлений. Проверьте подключение к интернету и повторите попытку позже.",
                MinWidth = 400,
                MinHeight = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(GetMainWindow())).ConfigureAwait(false);
    }

    private static Window? GetMainWindow()
    {
        return (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
    }

    /// <summary>
    /// Получает пропущенную версию
    /// </summary>
    /// <returns>Пропущенная версия или null</returns>
    private static Version? GetSkippedVersion()
    {
        var skipped = Settings.Default.SkippedVersion;
        return Version.TryParse(skipped, out var version)
            ? version
            : null;
    }

    /// <summary>
    /// Принудительно проверяет обновления (для ручного запуска)
    /// </summary>
    /// <param name="isDeveloperMode">Режим разработчика</param>
    /// <returns>Task</returns>
    public Task ForceCheckUpdatesAsync(bool isDeveloperMode) =>
        ManualCheckAndNotifyAsync(isDeveloperMode);
}
