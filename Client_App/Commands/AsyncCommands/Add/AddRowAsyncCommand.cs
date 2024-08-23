using Client_App.ViewModels;
using Models.Collections;
using Models.Forms;
using Models.Interfaces;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.Save;

namespace Client_App.Commands.AsyncCommands.Add;

// Добавить строку в форму
public class AddRowAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;
    private string FormType => changeOrCreateViewModel.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        var frm = FormCreator.Create(FormType);
        frm.NumberInOrder_DB = GetNumberInOrder(Storage[Storage.FormNum_DB]);
        var formContainRowAtStart = Storage.Rows.Count > 0;
        Storage[Storage.FormNum_DB].Add(frm);
        await Storage.SortAsync();
        if (!formContainRowAtStart)
        {
            await new SaveReportAsyncCommand(changeOrCreateViewModel).AsyncExecute(null);
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