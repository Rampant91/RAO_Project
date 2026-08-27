using System;
using System.Threading.Tasks;
using Client_App.Services;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Откат на предыдущую выкладку (только отдел).
/// </summary>
public class RollbackPreviousReleaseAsyncCommand : BaseAsyncCommand
{
    private readonly UpdateService _updateService;

    public RollbackPreviousReleaseAsyncCommand(UpdateService updateService)
    {
        _updateService = updateService;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        await Task.Run(async () =>
            await _updateService.RollbackToPreviousReleaseAsync().ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}
