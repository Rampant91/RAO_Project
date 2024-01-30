using Client_App.ViewModels;
using Models.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.CheckForm;

namespace Client_App.Commands.AsyncCommands.CheckForm;

//  Бесполезная команда, ничего не делает, активируется при нажатии кнопки "Проверить"
internal class CheckFormAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _changeOrCreateViewModel;
    private Report rep;

    public CheckFormAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _changeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    public override async Task<List<CheckError>> AsyncExecute(object? parameter)
    {
        rep = _changeOrCreateViewModel.Storage;
        List<CheckError>? result = new();
        switch (rep.FormNum_DB)
        {
            case "1.1":
                result.AddRange(CheckF11.Check_Total(rep));
                break;
        }
        return result;
    }
}