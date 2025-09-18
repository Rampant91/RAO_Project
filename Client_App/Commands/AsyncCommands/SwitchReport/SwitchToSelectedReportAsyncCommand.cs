using Client_App.Resources;
using Client_App.ViewModels.Forms;
using Models.Collections;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.SwitchReport;

//Потом нужно будет сделать универсальную SwitchingSelectedReportCommand TODO
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

        var window = Desktop.Windows.First(x => x.Name == formVM.FormType);
        var windowParam = new FormParameter()
        {
            Parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { selectedReport }),
            Window = window
        };
        await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
    }
}