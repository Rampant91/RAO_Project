using System.Collections;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Forms;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранный комментарий
internal class DeleteNoteAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;
    private Report Storage => _ChangeOrCreateViewModel.Storage;

    public DeleteNoteAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        var param = (IEnumerable)parameter;
        #region MessageDeleteNote
        var answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                },
                ContentTitle = "Выгрузка в Excel",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить комментарий?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow);
        #endregion
        if (answer is "Да")
        {
            foreach (Note item in param)
            {
                if (item == null) continue;
                foreach (var key in Storage.Notes)
                {
                    var it = (Note)key;
                    if (it.Order > item.Order)
                    {
                        it.Order -= 1;
                    }
                }
                foreach (Note nt in param)
                {
                    Storage.Notes.Remove(nt);
                }
            }
            await Storage.SortAsync();
        }
    }
}
