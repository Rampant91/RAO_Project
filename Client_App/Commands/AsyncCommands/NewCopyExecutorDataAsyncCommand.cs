using Client_App.Resources;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Models.Collections;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;

namespace Client_App.Commands.AsyncCommands;


public class NewCopyExecutorDataAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;
    private Reports Storages => Storage.Reports;
    private string FormType => formVM.FormType;
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var comparator = new CustomStringDateComparer(StringComparer.CurrentCulture);
        var lastReportWithExecutor = Storages.Report_Collection
            .Where(rep => rep.FormNum_DB == FormType
                          && !rep.Equals(Storage)
                          && (rep.FIOexecutor_DB is not (null or "" or "-")
                              || rep.ExecEmail_DB is not (null or "" or "-")
                              || rep.ExecPhone_DB is not (null or "" or "-")
                              || rep.GradeExecutor_DB is not (null or "" or "-")))
            .MaxBy(rep => rep.EndPeriod_DB, comparator);
        if (lastReportWithExecutor is null)
        {
            #region ShowMessageMissingExecutorData

            var orgName = "данной организации";
            var lastReport = Storages.Report_Collection
                .Where(rep => rep.FormNum_DB.Equals(FormType) && !rep.Equals(Storage))
                .MaxBy(rep => rep.EndPeriod_DB, comparator);
            if (FormType.ToCharArray()[0] == '1')
            {
                if (!string.IsNullOrEmpty(Storages.Master_DB.Rows10[1].ShortJurLico_DB) && Storages.Master_DB.Rows10[1].ShortJurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows10[1].ShortJurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows10[1].JurLico_DB) && Storages.Master_DB.Rows10[1].JurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows10[1].JurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows10[0].ShortJurLico_DB) && Storages.Master_DB.Rows10[0].ShortJurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows10[0].ShortJurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows10[0].JurLico_DB) && Storages.Master_DB.Rows10[0].JurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows10[0].JurLico_DB;
            }
            if (FormType.ToCharArray()[0] == '2')
            {
                if (!string.IsNullOrEmpty(Storages.Master_DB.Rows20[1].ShortJurLico_DB) && Storages.Master_DB.Rows20[1].ShortJurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows20[1].ShortJurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows20[1].JurLico_DB) && Storages.Master_DB.Rows20[1].JurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows20[1].JurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows20[0].ShortJurLico_DB) && Storages.Master_DB.Rows20[0].ShortJurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows20[0].ShortJurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows20[0].JurLico_DB) && Storages.Master_DB.Rows20[0].JurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows20[0].JurLico_DB;
            }
            var msg = lastReport is null
                ? $"У {orgName}" + Environment.NewLine + $"отсутствуют другие формы {FormType}"
                : $"У {orgName}" + Environment.NewLine + $"в формах {FormType} не заполнены данные исполнителя";
            
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Уведомление",
                    ContentMessage = msg
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }
        Storage.FIOexecutor.Value = lastReportWithExecutor.FIOexecutor_DB;
        Storage.ExecEmail.Value = lastReportWithExecutor.ExecEmail_DB;
        Storage.ExecPhone.Value = lastReportWithExecutor.ExecPhone_DB;
        Storage.GradeExecutor.Value = lastReportWithExecutor.GradeExecutor_DB;
    }
}