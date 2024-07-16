using Client_App.ViewModels;
using Models.Forms.Form1;
using System;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.PassportFill
{
    internal class PassportFillAllSyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : PassportFillBaseCommand(changeOrCreateViewModel)
    {
        public override async Task AsyncExecute(object? parameter)
        {
            collection = Storage.Rows17.ToList<Form17>();
            applicableOperationCodes = ["11","55"];
            msgTitle = "Паспорта для всех упаковок";
            msgQuestionOverride = "Сформировать паспорта для всех упаковок?   ";
            string suffix0 = applicableOperationCodes.Length == 1 ? "ом" : "ами";
            errOpsNotfound = $"Не удалось найти ни одной операции" +
                                 $"{Environment.NewLine}с код{suffix0} {string.Join(", ", applicableOperationCodes)}." +
                                 $"{Environment.NewLine}Формирование отменено.";
            await PassportFillAction();
        }
    }
}
