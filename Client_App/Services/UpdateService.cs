using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Properties;
using Client_App.Services.Updates;
using Client_App.Views.Messages;
using MessageBox.Avalonia.DTO;
using Models.DTO;

namespace Client_App.Services;

/// <summary>
/// Сервис для управления проверками обновлений (сайт — внешние пользователи, шара — отдел).
/// </summary>
public class UpdateService
{
#if DEBUG
    /// <summary>
    /// Автопроверка обновлений при старте в Debug-сборке.
    /// Поставьте <c>true</c>, чтобы при запуске из студии показывался диалог обновления.
    /// Ручная проверка из меню «Сервис» работает независимо от этого флага.
    /// </summary>
    private const bool DebugAutoUpdateCheckEnabled = false;
#endif

    private readonly UpdateChecker _websiteChecker = new();
    private readonly NetworkUpdateChecker _networkChecker = new();
    private readonly LocalUpdateStateStore _stateStore = new();
    private readonly LocalUpdatePrefsStore _prefsStore = new();
    private readonly NetworkUpdateInstaller _installer = new();
    private string _networkRoot = string.Empty;

    /// <summary>
    /// Автоматическая проверка при запуске (не чаще 1 раза в день).
    /// </summary>
    public async Task CheckAndNotifyAsync(bool isNoraoMode = false)
    {
        try
        {
            // Фоновый запуск с -p/-y: без UI, диалог обновления блокировал бы автовыгрузки.
            if (IsUnattendedStartup())
            {
                return;
            }

            // Updater можно подтянуть при каждом старте (дёшево, без диалога),
            // даже если полная проверка обновлений отложена на сутки.
            if (isNoraoMode)
            {
                TrySyncUpdaterFromLatestQuietly();
            }

#if DEBUG
            if (!DebugAutoUpdateCheckEnabled)
            {
                return;
            }
#endif

            if (!ShouldCheckForUpdates())
            {
                return;
            }

            if (isNoraoMode)
            {
                await CheckAndNotifyNetworkAsync(isManual: false).ConfigureAwait(false);
            }
            else
            {
                await CheckAndNotifyWebsiteAsync(isManual: false).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Update service error: {ex.Message}");
        }
    }

    /// <summary>
    /// Запуск с ключами оперативной/годовой автовыгрузки (-p / -y).
    /// </summary>
    private static bool IsUnattendedStartup()
    {
        return Settings.Default.AppStartupParameters
            .Trim()
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Any(x => x is "-p" or "-y");
    }

    /// <summary>
    /// Ручная проверка из меню «Сервис».
    /// </summary>
    public async Task ManualCheckAndNotifyAsync(bool isNoraoMode = false)
    {
        try
        {
            if (isNoraoMode)
            {
                await CheckAndNotifyNetworkAsync(isManual: true).ConfigureAwait(false);
            }
            else
            {
                await CheckAndNotifyWebsiteAsync(isManual: true).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Manual update check error: {ex.Message}");
            if (isNoraoMode)
            {
                await ShowNetworkCheckUnavailableDialog(
                    "Не удалось выполнить проверку обновлений из сетевой папки.").ConfigureAwait(false);
            }
            else
            {
                await ShowWebsiteCheckFailedDialog().ConfigureAwait(false);
            }
        }
    }

  public bool CanRollback() =>
    !NetworkUpdatePaths.IsRunningFromNetworkDistribution() && _stateStore.HasPreviousBackup();

  public async Task RollbackToPreviousReleaseAsync()
  {
    if (NetworkUpdatePaths.IsRunningFromNetworkDistribution())
    {
      await ShowNetworkCheckUnavailableDialog(
          "Откат недоступен: программа запущена с сетевого диска.")
        .ConfigureAwait(false);
      return;
    }

    if (!CanRollback())
    {
      await ShowNetworkCheckUnavailableDialog("Нет сохранённой предыдущей версии для отката.").ConfigureAwait(false);
      return;
    }

    var confirmed = await Dispatcher.UIThread.InvokeAsync(async () =>
    {
      var result = await MessageBox.Avalonia.MessageBoxManager
        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
        {
          ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.YesNo,
          ContentTitle = "Откат версии",
          ContentMessage =
            "Программа будет перезапущена с предыдущей установленной версией. Продолжить?",
          MinWidth = 420,
          MinHeight = 140,
          WindowStartupLocation = WindowStartupLocation.CenterOwner
        })
        .ShowDialog(GetMainWindow());
      return result == MessageBox.Avalonia.Enums.ButtonResult.Yes;
    }).ConfigureAwait(false);

    if (!confirmed)
    {
      return;
    }

    try
    {
      _installer.PrepareAndApplyRollback();
    }
    catch (Exception ex)
    {
      await ShowNetworkCheckUnavailableDialog($"Не удалось выполнить откат: {ex.Message}").ConfigureAwait(false);
    }
  }

  private async Task CheckAndNotifyWebsiteAsync(bool isManual)
  {
    var updateInfo = await _websiteChecker.CheckForUpdatesAsync().ConfigureAwait(false);
    if (updateInfo == null)
    {
      if (isManual)
      {
        await ShowWebsiteCheckFailedDialog().ConfigureAwait(false);
      }

      return;
    }

    MarkUpdateCheckCompleted();

    var skippedVersion = GetSkippedWebsiteVersion();
    if (skippedVersion != null && updateInfo.Version <= skippedVersion)
    {
      if (isManual)
      {
        await ShowUpToDateDialog(UpdateChecker.GetCurrentVersion()).ConfigureAwait(false);
      }

      return;
    }

    var currentVersion = UpdateChecker.GetCurrentVersion();
    if (updateInfo.Version > currentVersion)
    {
      await ShowUpdateNotificationDialog(updateInfo).ConfigureAwait(false);
    }
    else if (isManual)
    {
      await ShowUpToDateDialog(currentVersion).ConfigureAwait(false);
    }
  }

  private async Task CheckAndNotifyNetworkAsync(bool isManual)
  {
    if (!NetworkUpdatePaths.IsNetworkRootAccessible(out _networkRoot, out var accessFailure))
    {
      if (isManual)
      {
        await ShowNetworkCheckUnavailableDialog(accessFailure ?? "Сетевой каталог обновлений недоступен.")
          .ConfigureAwait(false);
      }

      return;
    }

    if (NetworkUpdatePaths.IsRunningFromNetworkDistribution(_networkRoot))
    {
      if (isManual)
      {
        await ShowNetworkCheckUnavailableDialog(
            "Программа запущена с сетевого диска (из папки «Исходные»).\n\n" +
            "Автообновление в этом режиме отключено, чтобы не изменять дистрибутивы на шаре.\n" +
            "Скопируйте папку win-x64 на локальный диск и запускайте программу оттуда.")
          .ConfigureAwait(false);
      }

      return;
    }

    var release = _networkChecker.TryReadLatest(_networkRoot);
    if (release == null)
    {
      if (isManual)
      {
        await ShowNetworkCheckUnavailableDialog(
          $"Не найден latest.json или папка релиза в каталоге:\n{_networkRoot}").ConfigureAwait(false);
      }

      return;
    }

    // После перехода на новую Client_App (даже если updater раньше не обновлялся):
    // подтянуть MpzfUpdater с шары без полного обновления программы.
    try
    {
      var releaseDir = NetworkUpdatePaths.GetReleaseDirectory(_networkRoot, release);
      _installer.TrySyncUpdaterFromReleaseDirectory(releaseDir);
    }
    catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"Updater sync skipped: {ex.Message}");
    }

    MarkUpdateCheckCompleted();

    // Пустой state + локальные файлы уже = latest → зафиксировать версию без диалога
    var localState = _stateStore.LoadAndBootstrapIfMatchesRelease(release, _networkRoot);

    var skippedReleaseId = _prefsStore.GetSkippedReleaseId();
    if (!string.IsNullOrWhiteSpace(skippedReleaseId)
        && string.Equals(skippedReleaseId, release.ReleaseId, StringComparison.OrdinalIgnoreCase))
    {
      if (isManual)
      {
        await ShowNetworkUpToDateDialog(release, localState).ConfigureAwait(false);
      }

      return;
    }

    if (!_networkChecker.IsUpdateAvailable(release, localState))
    {
      if (isManual)
      {
        await ShowNetworkUpToDateDialog(release, localState).ConfigureAwait(false);
      }

      return;
    }

    await ShowNetworkUpdateDialog(release, localState).ConfigureAwait(false);
  }

  private async Task ShowNetworkUpdateDialog(NetworkReleaseInfo release, LocalUpdateState localState)
  {
    await Dispatcher.UIThread.InvokeAsync(async () =>
    {
      try
      {
        var window = new NetworkUpdateNotificationWindow(release, localState, ApplyNetworkUpdateAsync);
        var mainWindow = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        await window.ShowDialog(mainWindow?.MainWindow);
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine($"Failed to show network update dialog: {ex.Message}");
      }
    }).ConfigureAwait(false);
  }

  private async Task ApplyNetworkUpdateAsync(NetworkReleaseInfo release)
  {
    if (string.IsNullOrWhiteSpace(_networkRoot)
        && !NetworkUpdatePaths.IsNetworkRootAccessible(out _networkRoot, out var failure))
    {
      throw new InvalidOperationException(failure ?? "Сетевой каталог обновлений недоступен.");
    }

    await _installer.PrepareAndApplyUpdateAsync(release, _networkRoot).ConfigureAwait(false);
  }

  private bool ShouldCheckForUpdates()
  {
#if DEBUG
    if (DebugAutoUpdateCheckEnabled)
    {
        _prefsStore.ResetCheckThrottleForDebug();
    }
#endif

    var lastCheck = _prefsStore.GetLastUpdateCheck();
    return (DateTime.Now - lastCheck).TotalDays >= 1;
  }

  private void MarkUpdateCheckCompleted()
  {
    _prefsStore.MarkUpdateCheckCompleted();
  }

  /// <summary>
  /// Тихая синхронизация data\Updater с latest на шаре (без диалогов).
  /// </summary>
  private void TrySyncUpdaterFromLatestQuietly()
  {
    try
    {
      if (!NetworkUpdatePaths.IsNetworkRootAccessible(out var networkRoot, out _))
      {
        return;
      }

      if (NetworkUpdatePaths.IsRunningFromNetworkDistribution(networkRoot))
      {
        return;
      }

      var release = _networkChecker.TryReadLatest(networkRoot);
      if (release == null)
      {
        return;
      }

      var releaseDir = NetworkUpdatePaths.GetReleaseDirectory(networkRoot, release);
      _installer.TrySyncUpdaterFromReleaseDirectory(releaseDir);
    }
    catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"Quiet updater sync failed: {ex.Message}");
    }
  }

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

  private static async Task ShowNetworkUpToDateDialog(NetworkReleaseInfo release, LocalUpdateState localState)
  {
    var installed = NetworkUpdateLabels.FormatInstalled(localState);
    var remote = NetworkUpdateLabels.FormatRemote(release);

    await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
      .GetMessageBoxStandardWindow(new MessageBoxStandardParams
      {
        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
        ContentTitle = "Проверка обновлений",
        ContentMessage =
          $"На сетевой шаре актуальна версия {remote}.\n" +
          $"У вас установлена: {installed}.",
        MinWidth = 460,
        MinHeight = 140,
        WindowStartupLocation = WindowStartupLocation.CenterOwner
      })
      .ShowDialog(GetMainWindow())).ConfigureAwait(false);
  }

  private static async Task ShowWebsiteCheckFailedDialog()
  {
    await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
      .GetMessageBoxStandardWindow(new MessageBoxStandardParams
      {
        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
        ContentTitle = "Проверка обновлений",
        ContentMessage =
          "Не удалось проверить наличие обновлений. Проверьте подключение к интернету и повторите попытку позже.",
        MinWidth = 400,
        MinHeight = 120,
        WindowStartupLocation = WindowStartupLocation.CenterOwner
      })
      .ShowDialog(GetMainWindow())).ConfigureAwait(false);
  }

  private static async Task ShowNetworkCheckUnavailableDialog(string message)
  {
    await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
      .GetMessageBoxStandardWindow(new MessageBoxStandardParams
      {
        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
        ContentTitle = "Проверка обновлений",
        ContentMessage = message,
        MinWidth = 460,
        MinHeight = 140,
        WindowStartupLocation = WindowStartupLocation.CenterOwner
      })
      .ShowDialog(GetMainWindow())).ConfigureAwait(false);
  }

  private static Window? GetMainWindow() =>
    (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

  private Version? GetSkippedWebsiteVersion() =>
    _prefsStore.GetSkippedWebsiteVersion();

  public Task ForceCheckUpdatesAsync(bool isNoraoMode) =>
    ManualCheckAndNotifyAsync(isNoraoMode);
}
