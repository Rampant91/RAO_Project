using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Models.Collections;
using Models.Forms;
using Models.Interfaces;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить N примечаний в форму.
/// </summary>
/// <param name="formVM">ViewModel отчёта.</param>
public class NewAddNotesAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.CurrentReport;

    public override async Task AsyncExecute(object? parameter)
    {
        // Если не получили окно напрямую, попробуем найти активное окно
        var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);

        if (owner == null) return;
        var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите количество новых примечаний"));

        int? rowCount = await dialog.ShowDialog<int?>(owner);

        if (rowCount > 0)
        {
            var r = GetNumberInOrder(Storage.Notes);
            var lst = new List<Note?>();
            for (var i = 0; i < rowCount; i++, r++)
            {
                var note = new Note { Order = r };
                Storage.Notes.Add(note);
            }
        }
    }

    /// <summary>
    /// Получить порядковый номер
    /// </summary>
    /// <param name="lst">Список элементов</param>
    /// <returns>Порядковый номер</returns>
    private static int GetNumberInOrder(IKeyCollection lst)
    {
        var maxNum = 0;
        foreach (var item in lst)
        {
            var frm = (INumberInOrder)item;
            if (frm.Order >= maxNum)
            {
                maxNum++;
            }
        }
        return maxNum + 1;
    }
}