using System.Threading;
using System.Threading.Tasks;
using Client_App.Views.ProgressBar;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

public class ExcelExportCancelAsyncCommand(ExcelExportProgressBar window) : BaseAsyncCommand
{
    public override Task AsyncExecute(object? parameter)
    {
        if (parameter is not null)
        {
            var cts = (CancellationTokenSource)parameter;
            cts.CancelAsync();
        }
        window.Close();
        return Task.CompletedTask;
    }
}