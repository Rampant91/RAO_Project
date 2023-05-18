using Client_App.ViewModels;
using Models.Collections;
using Models.Interfaces;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Models.Forms;

namespace Client_App.Commands.AsyncCommands.Add;

//  Добавить N примечаний в форму
internal class AddNotesAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;
    private Report Storage => _ChangeOrCreateViewModel.Storage;
    private Interaction<object, int> ShowDialog => _ChangeOrCreateViewModel.ShowDialog;

    public AddNotesAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

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