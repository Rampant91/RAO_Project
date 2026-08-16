using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.AsyncCommands;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.Views;
using Client_App.Views.Forms.Forms4;
using Models.Collections;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;

namespace Client_App.Commands.AsyncCommands.SwitchReport;

public class SwitchToSelectedReportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"> Принимает SelectedReport на который нужно переключиться</param>
    /// <returns></returns>
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not Report selectedReport) return;

        // Shell из popup может не иметь Reports — без этого ChangeOrCreateVM.Storages = null.
        selectedReport.Reports ??= formVM.Reports ?? formVM.Report.Reports;

        // Проверяем изменения и предлагаем сохранить
        var shouldContinue = await new CheckForChangesAndSaveCommand(formVM).AsyncExecute(null);
        if (!shouldContinue) return;

        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        var mainWindow = (desktop.MainWindow as MainWindow)!;
        var mainWindowVM = (mainWindow.DataContext as MainWindowVM)!;

        var window = Desktop.Windows.First(x => x.Name == formVM.FormType);

        if (selectedReport.FormNum.Value == "4.1")
        {
            var form41 = window as Form_41;
            await new NewChangeReportAsyncCommand(mainWindowVM.Forms4TabControlVM).AsyncExecute(selectedReport).ConfigureAwait(false);
            form41.Close();
        }
        else if (Form2InterfaceFlags.IsEnabledFor(selectedReport.FormNum.Value))
        {
            await new NewChangeReportAsyncCommand(mainWindowVM.Forms2TabControlVM).AsyncExecute(selectedReport).ConfigureAwait(false);
            window.Close();
        }
        else
        {
            var windowParam = new FormParameter()
            {
                Parameter = selectedReport,
                Window = window
            };

            await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
        }

    }
}