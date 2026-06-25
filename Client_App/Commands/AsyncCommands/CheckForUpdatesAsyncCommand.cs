using System;
using System.Threading.Tasks;
using Client_App.Services;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Ручная проверка обновлений из меню «Сервис».
/// </summary>
public class CheckForUpdatesAsyncCommand : BaseAsyncCommand
{
    private readonly UpdateService _updateService;
    private readonly Func<bool> _isDeveloperMode;

    public CheckForUpdatesAsyncCommand(UpdateService updateService, Func<bool> isDeveloperMode)
    {
        _updateService = updateService;
        _isDeveloperMode = isDeveloperMode;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        await Task.Run(async () =>
            await _updateService.ManualCheckAndNotifyAsync(_isDeveloperMode()).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}
