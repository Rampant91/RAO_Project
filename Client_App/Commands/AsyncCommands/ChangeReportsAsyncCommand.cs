using Client_App.ViewModels;
using Client_App.Views;
using Models.Collections;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Изменить Формы организации (1.0 и 2.0).
/// </summary>
public class ChangeReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is ObservableCollectionWithItemPropertyChanged<IKey> param && param.First() is { } obj)
        {
            var t = Desktop.MainWindow as MainWindow;
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
            var rep = (Reports)obj;
            var frm = new ChangeOrCreateVM(rep.Master.FormNum.Value, rep.Master);
            await MainWindowVM.ShowDialog.Handle(frm);

            //Local_Reports.Reports_Collection.Sorted = false;
            //await Local_Reports.Reports_Collection.QuickSortAsync();
            t.SelectedReports = tmp;
        }
    }
}