using AvaloniaEdit.Utils;
using Client_App.ViewModels.Forms;
using Models.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

public class SelectAllRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private ObservableCollection<Form> FormList => formVM.FormList;
    private ObservableCollection<Form> Selected => formVM.SelectedForms;

    public override async Task AsyncExecute(object? parameter)
    {
        // Создаем новую коллекцию вместо изменения существующей
        var allRows = new ObservableCollection<Form>();

        allRows.AddRange(FormList);
        // Заменяем всю коллекцию (это вызовет PropertyChanged)
        formVM.SelectedForms = allRows;
    }
}