using Client_App.ViewModels;
using Models.Forms.Form1;
using Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.PassportFill;

internal class PassportFillSyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : PassportFillBaseCommand(changeOrCreateViewModel)
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not IKeyCollection collectionBase) return;
        Collection = collectionBase.ToList<Form17>();
        ApplicableOperationCodes = ["11", "18", "55", "97"];
        var suffix0 = ApplicableOperationCodes.Length == 1 ? "ом" : "ами";
        MsgTitle = "Паспорт для упаковки";
        MsgQuestionOverride = null;
        ErrOpsNotFound = $"Среди выделенных операций нет операций" +
                         $"{Environment.NewLine}с код{suffix0} {string.Join(", ", ApplicableOperationCodes)}." +
                         $"{Environment.NewLine}Операция отменена.";
        await PassportFillAction();
    }
}