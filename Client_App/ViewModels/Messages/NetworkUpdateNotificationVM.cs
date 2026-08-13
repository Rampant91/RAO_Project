using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Client_App.Properties;
using Client_App.Services.Updates;
using CommunityToolkit.Mvvm.Input;
using Models.DTO;

namespace Client_App.ViewModels.Messages;

/// <summary>
/// Диалог сетевого обновления для отдела.
/// </summary>
public class NetworkUpdateNotificationVM : INotifyPropertyChanged
{
    private readonly Action _closeCallback;
    private readonly Func<NetworkReleaseInfo, Task>? _applyUpdateAsync;
    private bool _isBusy;
    private string _statusText = string.Empty;

    public NetworkReleaseInfo Release { get; }
    public string VersionLabel { get; }
    public string CurrentReleaseLabel { get; }
    public string PublishedAtText { get; }
    public bool CanSkip { get; }

    public NetworkUpdateNotificationVM()
    {
        Release = new NetworkReleaseInfo();
        VersionLabel = string.Empty;
        CurrentReleaseLabel = string.Empty;
        PublishedAtText = string.Empty;
        CanSkip = true;
        UpdateNowCommand = new AsyncRelayCommand(UpdateNowAsync, () => !IsBusy);
        RemindLaterCommand = new RelayCommand(CloseDialog, () => !IsBusy);
        SkipReleaseCommand = new RelayCommand(SkipRelease, () => !IsBusy && CanSkip);
    }

    public NetworkUpdateNotificationVM(
        NetworkReleaseInfo release,
        LocalUpdateState localState,
        Action closeCallback,
        Func<NetworkReleaseInfo, Task> applyUpdateAsync)
    {
        Release = release ?? throw new ArgumentNullException(nameof(release));
        _closeCallback = closeCallback ?? throw new ArgumentNullException(nameof(closeCallback));
        _applyUpdateAsync = applyUpdateAsync ?? throw new ArgumentNullException(nameof(applyUpdateAsync));
        CanSkip = !release.Required;

        VersionLabel = NetworkUpdateLabels.FormatRemote(release);
        CurrentReleaseLabel = NetworkUpdateLabels.FormatInstalled(localState);

        PublishedAtText = release.PublishedAt?.ToString("dd.MM.yyyy HH:mm") ?? "—";

        UpdateNowCommand = new AsyncRelayCommand(UpdateNowAsync, () => !IsBusy);
        RemindLaterCommand = new RelayCommand(CloseDialog, () => !IsBusy);
        SkipReleaseCommand = new RelayCommand(SkipRelease, () => !IsBusy && CanSkip);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (_isBusy == value)
            {
                return;
            }

            _isBusy = value;
            OnPropertyChanged();
            (UpdateNowCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (RemindLaterCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (SkipReleaseCommand as IRelayCommand)?.NotifyCanExecuteChanged();
        }
    }

    public string StatusText
    {
        get => _statusText;
        private set
        {
            if (_statusText == value)
            {
                return;
            }

            _statusText = value;
            OnPropertyChanged();
        }
    }

    public ICommand UpdateNowCommand { get; }
    public ICommand RemindLaterCommand { get; }
    public ICommand SkipReleaseCommand { get; }

    private async Task UpdateNowAsync()
    {
        if (_applyUpdateAsync == null)
        {
            return;
        }

        try
        {
            IsBusy = true;
            StatusText = "Копирование файлов обновления…";
            await _applyUpdateAsync(Release).ConfigureAwait(true);
            StatusText = "Перезапуск программы…";
        }
        catch (Exception ex)
        {
            IsBusy = false;
            StatusText = $"Ошибка: {ex.Message}";
        }
    }

    private void CloseDialog() => _closeCallback?.Invoke();

    private void SkipRelease()
    {
        try
        {
            Settings.Default.SkippedReleaseId = Release.ReleaseId;
            Settings.Default.Save();
        }
        catch
        {
            // ignore
        }
        finally
        {
            CloseDialog();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
