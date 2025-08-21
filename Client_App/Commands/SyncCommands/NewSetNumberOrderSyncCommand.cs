using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Models.Collections;
using System.Linq;

namespace Client_App.Commands.SyncCommands;

//  Выставление порядкового номера
public class NewSetNumberOrderSyncCommand(BaseFormVM formVM) : BaseCommand
{
    private Report Storage => formVM.CurrentReport;

    public override bool CanExecute(object? parameter) => true;

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