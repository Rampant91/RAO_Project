using Client_App.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using MessageBox.Avalonia.DTO;
using Models.CheckForm;

namespace Client_App.Commands.SyncCommands.CheckForm;

//  Проверяет открытую форму, открывает окно с отчетом об ошибках, активируется при нажатии кнопки "Проверить"
public class CheckFormSyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    public override async Task AsyncExecute(object? parameter)
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
            case "1.5":
                result.AddRange(CheckF15.Check_Total(reps, rep));
                break;
            case "1.6":
                result.AddRange(CheckF16.Check_Total(reps, rep));
                break;
            //case "1.7":
            //    result.AddRange(CheckF17.Check_Total(reps, rep));
            //    break;
            case "1.8":
                result.AddRange(CheckF18.Check_Total(reps, rep));
                break;
            //case "1.9":
            //    result.AddRange(CheckF19.Check_Total(reps, rep));
            //    break;
        }

        if (result.Count == 0)
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {rep.FormNum_DB}",
                    ContentHeader = "Уведомление",
                    ContentMessage = "По результатам проверки формы, ошибок не выявлено.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion
        }
        else
        {
            _ = new Views.CheckForm(changeOrCreateViewModel, result);
        }
    }
}