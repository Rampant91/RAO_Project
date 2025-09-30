using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms2;
using Models.Collections;
using Models.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Изменить Формы организации (4.0).
/// </summary>
public class NewChangeReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = (Desktop.MainWindow as MainWindow)!;
        var mainWindowVM = (mainWindow.DataContext as MainWindowVM);
        var report = mainWindowVM.SelectedReports.Master;
        var formNum = report.FormNum.Value;

        switch (formNum)
        {
            case "1.0":
                {
                    var form10VM = new Form_10VM(formNum, report)
                    {
                        IsSeparateDivision = !string.IsNullOrWhiteSpace(report.Rows10[1].Okpo.Value)
                    };
                    var window = new Form_10(form10VM) { DataContext = form10VM };
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "2.0":
                {
                    var form20VM = new Form_20VM(formNum, report)
                    {
                        IsSeparateDivision = !string.IsNullOrWhiteSpace(report.Rows20[1].Okpo.Value)
                    };
                    var window = new Form_20(form20VM) { DataContext = form20VM };
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "4.0":
                {
                    var form40VM = new Form_40VM(formNum, report);
                    var window = new Form_40(form40VM) { DataContext = form40VM };
                    await window.ShowDialog(mainWindow);
                    break;
                }
        }

        mainWindowVM.UpdateReports();

    }
}