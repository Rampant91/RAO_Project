using Client_App.ViewModels;
using Models.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Models.Forms;
using Models.Interfaces;
using ReactiveUI;

namespace Client_App.Commands.AsyncCommands.Add;

// Добавить N строк в форму
internal class AddRowsAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;
    private Report Storage => _ChangeOrCreateViewModel.Storage;
    private string FormType => _ChangeOrCreateViewModel.FormType;
    private Interaction<object, int> ShowDialog => _ChangeOrCreateViewModel.ShowDialog;

    public AddRowsAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        var t = await ShowDialog.Handle(Desktop.MainWindow);
        if (t > 0)
        {
            var number = GetNumberInOrder(Storage.Rows);
            var lst = new List<Form?>();
            for (var i = 0; i < t; i++)
            {
                var frm = FormCreator.Create(FormType);
                frm.NumberInOrder_DB = number;
                lst.Add(frm);
                number++;
            }

            Storage.Rows.AddRange(lst);
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