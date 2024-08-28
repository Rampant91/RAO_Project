using System;
using Client_App.ViewModels;
using Models.Collections;
using Models.DBRealization;
using System.Threading.Tasks;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;

namespace Client_App.Commands.AsyncCommands.Save;

//  Сохранить отчет
public class SaveReportAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;
    private Reports Storages => changeOrCreateViewModel.Storages;
    private string FormType => changeOrCreateViewModel.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        if (changeOrCreateViewModel.DBO != null)
        {
            var tmp = new Reports { Master = Storage };
            if (tmp.Master.Rows10.Count != 0)
            {
                tmp.Master.Rows10[1].OrganUprav.Value = tmp.Master.Rows10[0].OrganUprav.Value;
                tmp.Master.Rows10[1].RegNo.Value = tmp.Master.Rows10[0].RegNo.Value;
            }
            if (tmp.Master.Rows20.Count != 0)
            {
                tmp.Master.Rows20[1].OrganUprav.Value = tmp.Master.Rows20[0].OrganUprav.Value;
                tmp.Master.Rows20[1].RegNo.Value = tmp.Master.Rows20[0].RegNo.Value;
            }
            changeOrCreateViewModel.DBO.Reports_Collection.Add(tmp);
            changeOrCreateViewModel.DBO = null;
        }
        else if (Storages != null && FormType is not ("1.0" or "2.0") && !Storages.Report_Collection.Contains(Storage))
        {
            Storages.Report_Collection.Add(Storage);
        }

        if (Storages != null)
        {
            if (Storages.Master.Rows10.Count != 0)
            {
                Storages.Master.Rows10[1].OrganUprav.Value = Storages.Master.Rows10[0].OrganUprav.Value;
                Storages.Master.Rows10[1].RegNo.Value = Storages.Master.Rows10[0].RegNo.Value;
            }
            if (Storages.Master.Rows20.Count != 0)
            {
                Storages.Master.Rows20[1].OrganUprav.Value = Storages.Master.Rows20[0].OrganUprav.Value;
                Storages.Master.Rows20[1].RegNo.Value = Storages.Master.Rows20[0].RegNo.Value;
            }
            Storages.Report_Collection.Sorted = false;
            await Storages.Report_Collection.QuickSortAsync();
        }
        var dbm = StaticConfiguration.DBModel;
        try
        {
            await dbm.SaveChangesAsync();
            changeOrCreateViewModel.IsCanSaveReportEnabled = false;
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }
    }
}