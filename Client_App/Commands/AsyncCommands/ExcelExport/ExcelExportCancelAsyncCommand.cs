using System.Threading;
using System.Threading.Tasks;
using Client_App.Views.ProgressBar;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Отменяет команду (тот же CTS, что у прогрессбара) и закрывает окно.
/// Закрытие окна крестиком отменяет команду так же, как эта кнопка.
/// </summary>
/// <param name="progressBarWindow">Окно прогрессбара.</param>
public class ExcelExportCancelAsyncCommand(AnyTaskProgressBar progressBarWindow) : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var cts = (CancellationTokenSource)parameter;
        await cts.CancelAsync();
        await progressBarWindow.CloseAsync();
        cts.Token.ThrowIfCancellationRequested();
    }
}