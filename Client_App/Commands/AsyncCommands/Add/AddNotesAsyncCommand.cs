using Client_App.ViewModels;
using Models.Collections;
using Models.Interfaces;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Models.Forms;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить N примечаний в форму.
/// </summary>
/// <param name="changeOrCreateViewModel">ViewModel отчёта.</param>
public class AddNotesAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;
    private Interaction<object, int> ShowDialog => changeOrCreateViewModel.ShowDialog;

    public override async Task AsyncExecute(object? parameter)
    {
        var t = await ShowDialog.Handle(Desktop.MainWindow);
        if (t > 0)
        {
            var r = GetNumberInOrder(Storage.Notes);
            var lst = new List<Note?>();
            for (var i = 0; i < t; i++, r++)
            {
                var note = new Note { Order = r };
                lst.Add(note);
            }
            Storage.Notes.AddRange(lst);
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