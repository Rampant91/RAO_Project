using MsBox.Avalonia;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Interfaces;
using Client_App.ViewModels.Forms;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранный комментарий (новое окно формы).
/// </summary>
public class NewDeleteNoteAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not IEnumerable<IKey> enumerable)
            return;

        var param = enumerable.Cast<Note>().ToArray();
        if (param.Length == 0)
            return;

        #region MessageDeleteNote

        var suffix = param.Length == 1 ? 'у' : 'и';
        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            }).ShowWindowDialogAsync(Desktop.MainWindow));

        #endregion

        if (answer is not "Да") return;

        var db = StaticConfiguration.DBModel;
        var targets = new List<Note>();
        foreach (var item in param)
        {
            if (item.Id > 0)
            {
                var byId = Storage.Notes.FirstOrDefault(n => n.Id == item.Id);
                targets.Add(byId ?? item);
            }
            else
                targets.Add(item);
        }

        foreach (var target in targets.Distinct())
        {
            foreach (var note in Storage.Notes.Where(n => n.Order > target.Order).ToList())
                note.Order -= 1;

            Storage.Notes.Remove(target);
            var entry = db.Entry(target);
            if (entry.State == EntityState.Added)
                entry.State = EntityState.Detached;
            else if (entry.State is not (EntityState.Detached or EntityState.Deleted))
                db.Remove(target);
        }

        await Storage.SortAsync();
        formVM.UpdateNoteList();
        formVM.IsCanSaveReportEnabled = true;
    }
}
