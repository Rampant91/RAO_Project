using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Models.Collections;
using Models.Interfaces;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

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
            var mainWindow = Desktop.MainWindow as MainWindow;
            var report = (Reports)obj;
            var form10VM = new Form_10VM(report.DBObservable);
            var window = new Form_10(form10VM) { DataContext = form10VM };
            await window.ShowDialog(mainWindow);
           
        }
    }
}