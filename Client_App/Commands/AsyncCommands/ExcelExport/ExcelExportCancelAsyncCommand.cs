using System.Threading;
using System.Threading.Tasks;
using Client_App.Views.ProgressBar;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Закрывает окно, если операция отменена.
/// </summary>
/// <param name="progressBarWindow">Окно прогрессбара.</param>
public class ExcelExportCancelAsyncCommand(AnyTaskProgressBar progressBarWindow) : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var cts = (CancellationTokenSource)parameter;
        await cts.CancelAsync();
        progressBarWindow.Close();
        cts.Token.ThrowIfCancellationRequested();
    }
}