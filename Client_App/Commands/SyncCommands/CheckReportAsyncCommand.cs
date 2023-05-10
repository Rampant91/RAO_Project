using Client_App.ViewModels;

namespace Client_App.Commands.SyncCommands;

//  Бесполезная команда, ничего не делает, активируется при нажатии кнопки "Проверить"
internal class CheckReportAsyncCommand : BaseCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;

    public CheckReportAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    public override void Execute(object? parameter)
    {
        _ChangeOrCreateViewModel.IsCanSaveReportEnabled = true;
    }
}