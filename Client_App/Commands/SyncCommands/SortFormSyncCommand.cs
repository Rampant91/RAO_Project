using System.Linq;
using Client_App.ViewModels;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form2;

namespace Client_App.Commands.SyncCommands;

public class SortFormSyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;

    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        var formNum = Storage.FormNum_DB;
        var minItem = (long)(parameter ?? Storage[formNum].GetEnumerable().Min(x => x.Order));
        switch (formNum)
        {
            case "2.1":
            {
                var count = 1;
                var rows = Storage[formNum]
                    .GetEnumerable()
                    .Cast<Form21>()
                    .ToArray();
                foreach (var row in rows)
                {
                    row.NumberInOrder_DB = count;
                    count++;
                    row.NumberInOrderSum_DB = string.Empty;
                }
                break;
            }
            case "2.2":
            {
                var count = 1;
                var rows = Storage[formNum]
                    .GetEnumerable()
                    .Cast<Form22>()
                    .ToArray();
                foreach (var row in rows)
                {
                    row.NumberInOrder_DB = count;
                    count++;
                    row.NumberInOrderSum_DB = string.Empty;
                }
                break;
            }
            default:
            {
                Storage.Sort();
                var itemQ = Storage.Rows
                    .GetEnumerable()
                    .Where(x => x.Order >= minItem)
                    .Select(x => (Form)x)
                    .ToArray();
                foreach (var form in itemQ)
                {
                    form.NumberInOrder_DB = (int)minItem;
                    form.NumberInOrder.OnPropertyChanged();
                    minItem++;
                }
                break;
            }
        }
    }
}