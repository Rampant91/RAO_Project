using System.Threading;
using System.Threading.Tasks;
using Client_App.Views.ProgressBar;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Закрывает окно, если операция отменена
public class ExcelExportCancelAsyncCommand(ExcelExportProgressBar window) : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not null)
        {
            var cts = (CancellationTokenSource)parameter;
            await cts.CancelAsync();
            cts.Dispose();
        }
        window.Close();
    }
}