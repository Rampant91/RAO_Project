using Client_App.ViewModels;
using Models.Collections;
using Models.Forms.Form1;
using Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.PassportFill
{
    internal class PassportFillSyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : PassportFillBaseCommand(changeOrCreateViewModel)
    {
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is not IKeyCollection collection_base) return;
            collection = collection_base.ToList<Form17>();
            applicableOperationCodes = ["11", "18", "55", "97"];
            string suffix0 = applicableOperationCodes.Length == 1 ? "ом" : "ами";
            msgTitle = "Паспорт для упаковки";
            msgQuestionOverride = null;
            errOpsNotfound =
                            $"Среди выделенных операций нет операций" +
                            $"{Environment.NewLine}с код{suffix0} {string.Join(", ", applicableOperationCodes)}." +
                            $"{Environment.NewLine}Операция отменена.";
            await PassportFillAction();
        }
    }
}
