using System.Linq;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.Commands.SyncCommands;

//  Выставление порядкового номера
public class SetNumberOrderSyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;

    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    public override void Execute(object? parameter)
    {
        Storage.Sort();
        var count = 1;
        var rows = Storage.Rows
            .GetEnumerable()
            .ToList();
        foreach (var row in rows)
        {
            row.SetOrder(count);
            count++;
        }
    }
}