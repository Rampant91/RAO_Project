using Client_App.ViewModels.Forms;

namespace Client_App.Commands.SyncCommands;

//  Выставление порядкового номера
public class NewSetNumberOrderSyncCommand(BaseFormVM formVM) : BaseCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        formVM.UpdateFormList();
        formVM.UpdatePageInfo();
        formVM.IsCanSaveReportEnabled = true;
    }
}
