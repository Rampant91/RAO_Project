using System.Linq;
using Client_App.ViewModels;
using Models.Collections;
using Models.DBRealization;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Save;

//  Сохранить отчет
internal class SaveReportAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;
    private Report Storage => _ChangeOrCreateViewModel.Storage;
    private Reports Storages => _ChangeOrCreateViewModel.Storages;
    private string FormType => _ChangeOrCreateViewModel.FormType;

    public SaveReportAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        if (_ChangeOrCreateViewModel.DBO != null)
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
            _ChangeOrCreateViewModel.DBO.Reports_Collection.Add(tmp);
            _ChangeOrCreateViewModel.DBO = null;
        }
        else if (Storages != null && FormType is not ("1.0" or "2.0") && Storages.Report_Collection.All(rep => rep.Id != Storage.Id))
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
            dbm.SaveChanges();  // Была багуля, при async версии метода не сохраняет БД с 1 раза, приходилось вызывать метод 2 раза.
            _ChangeOrCreateViewModel.IsCanSaveReportEnabled = false;
        }
        catch
        {
            // ignored
        }
    }
}