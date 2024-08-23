using Client_App.ViewModels;
using Models.Collections;
using Models.Forms;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

//  Добавить N строк в форму перед выбранной строкой
internal class AddRowsInAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;
    private string FormType => changeOrCreateViewModel.FormType;
    private Interaction<int, int> ShowDialogIn => changeOrCreateViewModel.ShowDialogIn;

    public override async Task AsyncExecute(object? parameter)
    {
        var param = (IEnumerable)parameter;
        var item = param.Cast<Form>().ToList().FirstOrDefault();
        if (item != null)
        {
            var numberCell = item.NumberInOrder_DB;
            var t2 = await ShowDialogIn.Handle(numberCell);
            if (t2 > 0)
            {
                foreach (var key in Storage[item.FormNum_DB])
                {
                    var it = (Form)key;
                    if (it.NumberInOrder_DB > numberCell - 1)
                    {
                        it.NumberInOrder.Value = it.NumberInOrder_DB + t2;
                    }
                }
                List<Form> lst = new();
                for (var i = 0; i < t2; i++)
                {
                    var frm = FormCreator.Create(FormType);
                    frm.NumberInOrder_DB = numberCell;
                    lst.Add(frm);
                    numberCell++;
                }

                Storage[Storage.FormNum_DB].AddRange(lst);
                await Storage.SortAsync();
            }
        }
    }
}