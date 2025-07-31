using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms2;
using Models.Collections;
using Models.Forms.Form1;
using Models.Interfaces;
using System.Linq;
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
            var mainWindow = (Desktop.MainWindow as MainWindow)!;
            var mainWindowVM = (mainWindow.DataContext as MainWindowVM);
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);
            var reps = (Reports)obj;
            switch (mainWindowVM.SelectedReportTypeToString)
            {
                case "1.0":
                    {
                        var form10VM = new Form_10VM(mainWindowVM.SelectedReportTypeToString, reps.Master)
                        {
                            IsSeparateDivision = !string.IsNullOrWhiteSpace(reps.Master.Rows10[1].Okpo.Value)
                        };
                        var window = new Form_10(form10VM) { DataContext = form10VM };
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.0":
                    {
                        var form20VM = new Form_20VM(mainWindowVM.SelectedReportTypeToString, reps.Master)
                        {
                            IsSeparateDivision = !string.IsNullOrWhiteSpace(reps.Master.Rows20[1].Okpo.Value)
                        };
                        var window = new Form_20(form20VM) { DataContext = form20VM };
                        await window.ShowDialog(mainWindow);
                        break;
                    }
            }

            //Local_Reports.Reports_Collection.Sorted = false;
            //await Local_Reports.Reports_Collection.QuickSortAsync();
            mainWindow.SelectedReports = tmp;
        }
    }
}