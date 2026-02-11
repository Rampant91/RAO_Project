using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Models.DTO;
using Client_App.Properties;
using Client_App.Services;
using CommunityToolkit.Mvvm.Input;

namespace Client_App.ViewModels.Messages;

/// <summary>
/// ViewModel для окна уведомления об обновлении
/// </summary>
public class UpdateNotificationVM : INotifyPropertyChanged
{
    private readonly Action _closeCallback;
    
    public UpdateInfo UpdateInfo { get; }
    public string CurrentVersion { get; }

    public UpdateNotificationVM(){ }
    public UpdateNotificationVM(UpdateInfo updateInfo, Action closeCallback)
    {
        UpdateInfo = updateInfo ?? throw new ArgumentNullException(nameof(updateInfo));
        _closeCallback = closeCallback ?? throw new ArgumentNullException(nameof(closeCallback));
        CurrentVersion = UpdateChecker.GetCurrentVersion().ToString();
        
        DownloadCommand = new RelayCommand(OpenDownloadPage);
        RemindLaterCommand = new RelayCommand(CloseDialog);
        SkipVersionCommand = new RelayCommand(SkipThisVersion);
    }
    
    #region Commands
    
    /// <summary>
    /// Команда для открытия страницы загрузки
    /// </summary>
    public ICommand DownloadCommand { get; }
    
    /// <summary>
    /// Команда для закрытия диалога (напомнить позже)
    /// </summary>
    public ICommand RemindLaterCommand { get; }
    
    /// <summary>
    /// Команда для пропуска текущей версии
    /// </summary>
    public ICommand SkipVersionCommand { get; }
    
    #endregion
    
    #region Command Implementations
    
    /// <summary>
    /// Открывает страницу загрузки в браузере
    /// </summary>
    private void OpenDownloadPage()
    {
        try
        {
            var url = UpdateInfo.DownloadUrl;
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Безопасное открытие URL для Windows
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                
                // Для Windows 7 и более старых версий
                if (Environment.OSVersion.Version.Major < 10)
                {
                    psi.Verb = "open"; // Явно указываем команду "open"
                }
                
                Process.Start(psi);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Для Linux/Astra - пробуем разные браузеры
                var browsers = new[] { "xdg-open", "firefox", "chromium-browser", "google-chrome", "opera" };
                bool opened = false;
                
                foreach (var browser in browsers)
                {
                    try
                    {
                        Process.Start(browser, url);
                        opened = true;
                        break;
                    }
                    catch
                    {
                        // Пробуем следующий браузер
                        continue;
                    }
                }
                
                if (!opened)
                {
                    Debug.WriteLine("Failed to open browser on Linux - no compatible browser found");
                }
            }
            else
            {
                // Для других платформ (macOS и др.)
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to open download page: {ex.Message}");
            
            // Запасной вариант для Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    Process.Start("cmd.exe", $"/c start {UpdateInfo.DownloadUrl}");
                }
                catch
                {
                    Debug.WriteLine("Failed to open browser with fallback method");
                }
            }
        }
        finally
        {
            CloseDialog();
        }
    }
    
    /// <summary>
    /// Закрывает диалоговое окно
    /// </summary>
    private void CloseDialog()
    {
        _closeCallback?.Invoke();
    }
    
    /// <summary>
    /// Пропускает текущую версию обновления
    /// </summary>
    private void SkipThisVersion()
    {
        try
        {
            // Сохраняем версию, которую нужно пропустить
            Settings.Default.SkippedVersion = UpdateInfo.Version.ToString();
            Settings.Default.Save();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to save skipped version: {ex.Message}");
        }
        finally
        {
            CloseDialog();
        }
    }
    
    #endregion
    
    #region INotifyPropertyChanged
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    #endregion
}
