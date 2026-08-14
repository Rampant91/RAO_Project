using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms3;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.ViewModels.Forms.Forms5;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms2;
using Client_App.Views.Forms.Forms3;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.Forms.Forms5;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Изменить Формы организации (1.0, 2.0, 3.0, 4.0, 5.0).
/// </summary>
public class NewChangeReportsAsyncCommand : BaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public NewChangeReportsAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
    {
        _formsTabControlVM = formsTabControlVM;

        formsTabControlVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _formsTabControlVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = (Desktop.MainWindow as MainWindow)!;
        var mainWindowVM = (mainWindow.DataContext as MainWindowVM)!;

        if (mainWindowVM.SelectedReports is null) return;

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
            case "3.0":
                {
                    var form30VM = new Form_30VM(formNum, report)
                    {
                        IsSeparateDivision = !string.IsNullOrWhiteSpace(report.Rows30[1].Okpo.Value)
                    };
                    var window = new Form_30(form30VM) { DataContext = form30VM };
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
            case "5.0":
            {
                var form50VM = new Form_50VM(formNum, report);
                var window = new Form_50(form50VM) { DataContext = form50VM };
                await window.ShowDialog(mainWindow);
                break;
            }
        }

        mainWindowVM.UpdateReportsCollection();
    }
}