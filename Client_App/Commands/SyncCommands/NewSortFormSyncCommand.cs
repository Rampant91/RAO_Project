using Client_App.ViewModels.Forms;
using Models.Collections;
using Models.Forms;
using System.Linq;
using Models.Interfaces;

namespace Client_App.Commands.SyncCommands;

public class NewSortFormSyncCommand(BaseFormVM formVM) : BaseCommand
{
    private Report Storage => formVM.Report;

    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        var formNum = Storage.FormNum_DB;
        var enumerable = Storage[formNum].GetEnumerable();
        if (enumerable is null) return;
        var formArray = enumerable as IKey[] ?? enumerable.ToArray();
        if (formArray.Length == 0) return;

        var minItem = formArray.Min(x => x.Order);

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
    }
}