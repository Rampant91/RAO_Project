using Client_App.ViewModels;
using System.Collections.Generic;
using Models.CheckForm;

namespace Client_App.Commands.SyncCommands.CheckForm;

//  Проверяет открытую форму, открывает окно с отчетом об ошибках, активируется при нажатии кнопки "Проверить"
internal class CheckFormSyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseCommand
{
    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    public override void Execute(object? parameter)
    {
        var reps = changeOrCreateViewModel.Storages;
        var rep = changeOrCreateViewModel.Storage;

        List<CheckError> result = [];
        switch (rep.FormNum_DB)
        {
            case "1.1":
                result.AddRange(CheckF11.Check_Total(reps, rep));
                break;
            case "1.2":
                result.AddRange(CheckF12.Check_Total(reps, rep));
                break;
            case "1.3":
                result.AddRange(CheckF13.Check_Total(reps, rep));
                break;
            case "1.4":
                result.AddRange(CheckF14.Check_Total(reps, rep));
                break;
            //case "1.5":
            //    result.AddRange(CheckF15.Check_Total(reps, rep));
            //    break;
            //case "1.6":
            //    result.AddRange(CheckF16.Check_Total(reps, rep));
            //    break;
            //case "1.7":
            //    result.AddRange(CheckF17.Check_Total(reps, rep));
            //    break;
            //case "1.8":
            //    result.AddRange(CheckF18.Check_Total(reps, rep));
            //    break;
            //case "1.9":
            //    result.AddRange(CheckF19.Check_Total(reps, rep));
            //    break;
            default: return;
        }
        _ = new Views.CheckForm(changeOrCreateViewModel, result);
    }
}