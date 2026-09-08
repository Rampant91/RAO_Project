using System;
using System.Linq;
using System.IO;
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
    /// Автоматическая проверка при запуске.
    /// Сетевой канал: каждый старт читает latest.json — новый releaseId показывается сразу.
    /// Сайт: HTTP не чаще раза в сутки (иначе старт ждёт сеть до 5 с).
    /// «Напомнить позже» откладывает только уже предложенный релиз/версию ~на сутки.
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

            // Updater можно подтянуть при каждом старте (дёшево, без диалога).
            if (isNoraoMode)
            {
                TrySyncUpdaterFromLatestQuietly();
            }

#if DEBUG
            if (!DebugAutoUpdateCheckEnabled)
            {
                return;
            }

            _prefsStore.ResetCheckThrottleForDebug();
#endif

            if (isNoraoMode)
            {
                await CheckAndNotifyNetworkAsync(isManual: false).ConfigureAwait(false);
            }
            else
            {
                if (!ShouldCheckWebsiteForUpdates())
                {
                    return;
                }

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

    // Время прошлого предложения — до сдвига суточного HTTP-лимита.
    var previousPromptAt = _prefsStore.GetLastUpdateCheck();
    _prefsStore.MarkUpdateCheckCompleted();

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
      if (!isManual
          && UpdateAutoPromptPolicy.ShouldSuppressAutoPrompt(
              updateInfo.Version.ToString(),
              _prefsStore.GetLastNotifiedWebsiteVersion(),
              previousPromptAt,
              DateTime.Now))
      {
        return;
      }

      // Notify/throttle только по «Напомнить позже» (см. UpdateNotificationVM).
      await ShowUpdateNotificationDialog(updateInfo).ConfigureAwait(false);
    }
    else if (isManual)
    {
      await ShowUpToDateDialog(currentVersion).ConfigureAwait(false);
    }
  }

  private async Task CheckAndNotifyNetworkAsync(bool isManual)
  {
    await TryShowPendingUpdaterFailureAsync().ConfigureAwait(false);

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

    if (!_networkChecker.IsUpdateAvailable(release, localState, _networkRoot))
    {
      if (isManual)
      {
        await ShowNetworkUpToDateDialog(release, localState).ConfigureAwait(false);
      }

      return;
    }

    if (!isManual
        && UpdateAutoPromptPolicy.ShouldSuppressAutoPrompt(
            release.ReleaseId,
            _prefsStore.GetLastNotifiedReleaseId(),
            _prefsStore.GetLastUpdateCheck(),
            DateTime.Now))
    {
      return;
    }

    // Notify/throttle только по «Напомнить позже» (см. NetworkUpdateNotificationVM).
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

  /// <summary>
  /// Показать ошибку прошлого прогона MpzfUpdater (если есть last-error.txt) и снять файл.
  /// </summary>
  private async Task TryShowPendingUpdaterFailureAsync()
  {
    try
    {
      if (!File.Exists(NetworkUpdatePaths.LastErrorFilePath))
      {
        return;
      }

      var text = File.ReadAllText(NetworkUpdatePaths.LastErrorFilePath).Trim();
      try
      {
        File.Delete(NetworkUpdatePaths.LastErrorFilePath);
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine($"Failed to clear last-error.txt: {ex.Message}");
      }

      if (string.IsNullOrWhiteSpace(text))
      {
        text = "Предыдущая попытка обновления завершилась с ошибкой.";
      }

      // Показываем короткое начало — полный стек в updater.log рядом с установкой.
      var firstLine = text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
        .FirstOrDefault() ?? text;
      if (firstLine.Length > 300)
      {
        firstLine = firstLine[..300] + "…";
      }

      await ShowNetworkCheckUnavailableDialog(
          "Не удалось применить обновление при прошлом запуске.\n\n" +
          firstLine + "\n\n" +
          "Подробности: файл .mpzf-update\\updater.log рядом с программой.\n" +
          "Можно повторить обновление через «Сервис → Проверить обновления».")
        .ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"TryShowPendingUpdaterFailureAsync: {ex.Message}");
    }
  }

  /// <summary>
  /// Сайт: не чаще одного HTTP-запроса в сутки при автопроверке.
  /// </summary>
  private bool ShouldCheckWebsiteForUpdates()
  {
    var lastCheck = _prefsStore.GetLastUpdateCheck();
    return (DateTime.Now - lastCheck).TotalDays >= 1;
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
