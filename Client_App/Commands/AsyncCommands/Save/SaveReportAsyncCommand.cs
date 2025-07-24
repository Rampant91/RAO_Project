using System;
using Client_App.ViewModels;
using Models.Collections;
using Models.DBRealization;
using System.Threading.Tasks;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.ViewModels.Forms.Forms1;

namespace Client_App.Commands.AsyncCommands.Save;

//  Сохранить отчет
public class SaveReportAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _changeOrCreateVM;

    //private readonly Form_10VM _form10VM;

    public SaveReportAsyncCommand(ChangeOrCreateVM changeOrCreateVM)
    {
        _changeOrCreateVM = changeOrCreateVM;
    }

    //public SaveReportAsyncCommand(BaseVM formViewModel)
    //{
    //    if (formViewModel is Form_10VM form10VM)
    //    {
    //        _form10VM = form10VM;
    //    }
    //}

    private Report Storage => _changeOrCreateVM.Storage;
    private Reports Storages => _changeOrCreateVM.Storages;
    private string FormType => _changeOrCreateVM.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        if (_changeOrCreateVM.DBO != null)
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
            _changeOrCreateVM.DBO.Reports_Collection.Add(tmp);
            _changeOrCreateVM.DBO = null;
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
            Storage.ReportChangedDate = DateTime.Now;
            await dbm.SaveChangesAsync();
            _changeOrCreateVM.IsCanSaveReportEnabled = false;
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }
    }
}