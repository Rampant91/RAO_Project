using Client_App.ViewModels;
using System.Linq;
using Models.Collections;

namespace Client_App.Commands.SyncCommands;

//  Выставление порядкового номера
internal class SetNumberOrderSyncCommand : BaseCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;
    private Report Storage => _ChangeOrCreateViewModel.Storage;

    public SetNumberOrderSyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    public override void Execute(object? parameter)
    {
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