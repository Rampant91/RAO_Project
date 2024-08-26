using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Forms;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранный комментарий
internal class DeleteNoteAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is IEnumerable<IKey> enumerable)
        {
            var param = enumerable.Cast<Note>().ToArray();

            #region MessageDeleteNote

            var suffix = param.Length == 1 ? 'у' : 'и';
            var answer = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да", IsDefault = true },
                        new ButtonDefinition { Name = "Нет", IsCancel = true }
                    ],
                    ContentTitle = "Удаление",
                    CanResize = true,
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Вы действительно хотите удалить строчк{suffix}?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow);

            #endregion

            if (answer is not "Да") return;

            foreach (var item in param)
            {
                foreach (var key in Storage.Notes)
                {
                    var it = (Note)key;
                    if (it.Order > item.Order)
                    {
                        it.Order -= 1;
                    }
                }
                foreach (var nt in param)
                {
                    Storage.Notes.Remove(nt);
                }
            }
            await Storage.SortAsync();
        }
    }
}