using Client_App.ViewModels;
using Client_App.Views;
using Models.Collections;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.Save;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.Add;

//  Создать и открыть новое окно формы организации (1.0 и 2.0)
public class AddReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is string par)
        {
            var mainWindow = (Desktop.MainWindow as MainWindow)!;
            ChangeOrCreateVM frm = new(par, ReportsStorage.LocalReports);
            await new SaveReportAsyncCommand(frm).AsyncExecute(null);
            await MainWindowVM.ShowDialog.Handle(frm);
            mainWindow.SelectedReports = mainWindow.SelectedReports is null
                ? []
                : new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);
            await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();
        }
    }
}