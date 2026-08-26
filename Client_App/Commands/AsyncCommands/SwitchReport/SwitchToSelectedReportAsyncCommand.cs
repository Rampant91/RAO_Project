using Client_App.Commands.AsyncCommands;
using Client_App.Services;
using Client_App.ViewModels.Forms;
using Models.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.SwitchReport;

/// <summary>
/// Переключиться на выбранный отчёт (↑/↓ / «Выбрать отчёт»).
/// </summary>
public class SwitchToSelectedReportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not Report selectedReport)
            return;

        // Shell из popup может не иметь Reports — без этого ChangeOrCreateVM.Storages = null.
        selectedReport.Reports ??= formVM.Reports ?? formVM.Report.Reports;

        var shouldContinue = await new CheckForChangesAndSaveCommand(formVM).AsyncExecute(null);
        if (!shouldContinue)
            return;

        var window = Desktop.Windows.First(x => x.Name == formVM.FormType);
        await FormReportWindowNavigator.SwitchAsync(window, formVM, selectedReport).ConfigureAwait(true);
    }
}
