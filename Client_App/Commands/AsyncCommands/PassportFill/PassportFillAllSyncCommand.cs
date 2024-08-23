using Client_App.ViewModels;
using Models.Forms.Form1;
using System;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.PassportFill;

public class PassportFillAllSyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : PassportFillBaseCommand(changeOrCreateViewModel)
{
    public override async Task AsyncExecute(object? parameter)
    {
        Collection = Storage.Rows17.ToList<Form17>();
        ApplicableOperationCodes = ["11","55"];
        MsgTitle = "Паспорта для всех упаковок";
        MsgQuestionOverride = "Сформировать паспорта для всех упаковок?   ";
        var suffix0 = ApplicableOperationCodes.Length == 1 ? "ом" : "ами";
        ErrOpsNotFound = $"Не удалось найти ни одной операции" +
                         $"{Environment.NewLine}с код{suffix0} {string.Join(", ", ApplicableOperationCodes)}." +
                         $"{Environment.NewLine}Формирование отменено.";
        await PassportFillAction();
    }
}