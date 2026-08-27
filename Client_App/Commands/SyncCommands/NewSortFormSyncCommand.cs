using Client_App.ViewModels.Forms;
using Models.Collections;
using Models.Forms;

namespace Client_App.Commands.SyncCommands;

public class NewSortFormSyncCommand(BaseFormVM formVM) : BaseCommand
{
    private Report Storage => formVM.Report;

    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        formVM.UpdateFormList();
        formVM.UpdatePageInfo();
    }
}
