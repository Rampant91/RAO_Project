using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using Models.DTO;
using Client_App.Properties;
using Client_App.Views.Messages;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

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
        return;
        try
        {
#if DEBUG
            Settings.Default.LastUpdateCheck = DateTime.MinValue;
            Settings.Default.SkippedVersion = "";
            Settings.Default.Save();
#endif

            // Проверяем, нужно ли выполнять проверку
            if (!ShouldCheckForUpdates())
            {
                return;
            }
            
            // Обновляем время последней проверки
            Settings.Default.LastUpdateCheck = DateTime.Now;
            Settings.Default.Save();
            
            // Получаем информацию об обновлениях
            var updateInfo = await _updateChecker.CheckForUpdatesAsync();
            if (updateInfo == null)
            {
                return; // Нет обновлений или нет интернета
            }
            
            // Проверяем, не пропустил ли пользователь эту версию
            var skippedVersion = GetSkippedVersion();
            if (skippedVersion != null && updateInfo.Version <= skippedVersion)
            {
                return; // Пользователь пропустил эту версию
            }
            
            // Сравниваем версии
            var currentVersion = UpdateChecker.GetCurrentVersion();
            if (updateInfo.Version > currentVersion)
            {
                // Показываем уведомление
                if (isDeveloperMode)
                {
                    await ShowAutoUpdateDialog(updateInfo);
                }
                else
                {
                    await ShowUpdateNotificationDialog(updateInfo);
                }
            }
        }
        catch (Exception ex)
        {
            // Логируем, но не показываем ошибки пользователю
            System.Diagnostics.Debug.WriteLine($"Update service error: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Проверяет, нужно ли выполнять проверку обновлений
    /// </summary>
    /// <returns>True если нужно проверить обновления</returns>
    private bool ShouldCheckForUpdates()
    {
        var lastCheck = Settings.Default.LastUpdateCheck;
        var now = DateTime.Now;
        
        // Проверяем не чаще раза в день
        return (now - lastCheck).TotalDays >= 1;
    }
    
    /// <summary>
    /// Показывает диалог уведомления об обновлении
    /// </summary>
    /// <param name="updateInfo">Информация об обновлении</param>
    /// <returns>Task</returns>
    private async Task ShowUpdateNotificationDialog(UpdateInfo updateInfo)
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
        });
    }
    
    /// <summary>
    /// Показывает диалог автообновления (для отдела)
    /// </summary>
    /// <param name="updateInfo">Информация об обновлении</param>
    /// <returns>Task</returns>
    private async Task ShowAutoUpdateDialog(UpdateInfo updateInfo)
    {
        // Здесь можно реализовать автообновление из сетевой папки
        // Пока просто показываем уведомление
        await ShowUpdateNotificationDialog(updateInfo);
    }
    
    /// <summary>
    /// Получает пропущенную версию
    /// </summary>
    /// <returns>Пропущенная версия или null</returns>
    private Version? GetSkippedVersion()
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
    public async Task ForceCheckUpdatesAsync(bool isDeveloperMode)
    {
        // Сбрасываем время последней проверки для принудительной проверки
        Settings.Default.LastUpdateCheck = DateTime.MinValue;
        Settings.Default.Save();
        
        await CheckAndNotifyAsync(isDeveloperMode);
    }
}
