using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport;
public class ExcelExportCancelAsyncCommand : BaseAsyncCommand
{
    public override Task AsyncExecute(object? parameter)
    {
        if (parameter is not null)
        {
            var cts = (CancellationTokenSource)parameter;
            cts.CancelAsync();
        }
        return Task.CompletedTask;
    }
}