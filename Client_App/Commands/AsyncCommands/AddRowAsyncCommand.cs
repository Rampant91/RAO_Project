using Client_App.ViewModels;
using Models.Collections;
using Models.Forms;
using Models.Interfaces;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

// Добавить строку в форму
internal class AddRowAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;
    private Report Storage => _ChangeOrCreateViewModel.Storage;
    private string FormType => _ChangeOrCreateViewModel.FormType;

    public AddRowAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        var frm = FormCreator.Create(FormType);
        frm.NumberInOrder_DB = GetNumberInOrder(Storage[Storage.FormNum_DB]);
        Storage[Storage.FormNum_DB].Add(frm);
        await Storage.SortAsync();
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