using Client_App.ViewModels;
using Client_App.Views;
using Models.Collections;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Models.Interfaces;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Resources;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Создать и открыть новое окно формы организации (1.0 и 2.0).
/// </summary>
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

            var comparator = new CustomReportsComparer();
            var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
            ReportsStorage.LocalReports.Reports_Collection.Clear();
            ReportsStorage.LocalReports.Reports_Collection
                .AddRange(tmpReportsList
                    .OrderBy(x => x.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(x => x.Master_DB.OkpoRep.Value, comparator));

            await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();
        }
    }
}