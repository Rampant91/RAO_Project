using Avalonia.Controls.ApplicationLifetimes;
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

        // Проверяем изменения и предлагаем сохранить
        var shouldContinue = await new CheckForChangesAndSaveCommand(formVM).AsyncExecute(null);
        if (!shouldContinue) return;

        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        var mainWindow = (desktop.MainWindow as MainWindow)!;
        var mainWindowVM = (mainWindow.DataContext as MainWindowVM)!;

        var window = Desktop.Windows.First(x => x.Name == formVM.FormType);

        switch(formVM.FormType[0])
        {
            case '1':
                await new NewChangeReportAsyncCommand(mainWindowVM.Forms1TabControlVM).AsyncExecute(selectedReport).ConfigureAwait(false);
                break;
            case '2':
                await new NewChangeReportAsyncCommand(mainWindowVM.Forms2TabControlVM).AsyncExecute(selectedReport).ConfigureAwait(false);
                break;
            //case '3':
            //    await new NewChangeReportAsyncCommand(mainWindowVM.Forms3TabControlVM).AsyncExecute(selectedReport).ConfigureAwait(false);
            //    break;
            case '4':
                await new NewChangeReportAsyncCommand(mainWindowVM.Forms4TabControlVM).AsyncExecute(selectedReport).ConfigureAwait(false);
                break;
            case '5':
                await new NewChangeReportAsyncCommand(mainWindowVM.Forms5TabControlVM).AsyncExecute(selectedReport).ConfigureAwait(false);
                break;
        }    
        window.Close();

    }
}