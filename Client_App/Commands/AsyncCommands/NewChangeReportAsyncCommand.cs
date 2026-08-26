using Client_App.Services;
using Client_App.ViewModels.MainWindowTabs;
using Models.Collections;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Открыть окно редактирования выбранного отчета.
/// </summary>
public class NewChangeReportAsyncCommand : BaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public NewChangeReportAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
    {
        _formsTabControlVM = formsTabControlVM;

        formsTabControlVM.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReport))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _formsTabControlVM.SelectedReport is not null;

    /// <summary>
    /// Используется при вызове из других команд (fire-and-forget ShowDialog внутри).
    /// </summary>
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter != null)
            Execute(parameter);
        await Task.CompletedTask;
    }

    public override async void Execute(object? parameter)
    {
        if (parameter is not Report report)
            return;

        var owner = Desktop.MainWindow as Views.MainWindow;
        await FormReportWindowOpener.OpenAsync(report, owner);
    }
}
