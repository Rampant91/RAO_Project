using Models.Collections;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.DBAPIFactory;
using Client_App.ViewModels;
using Models.DBRealization;

namespace Client_App;

public static class ReportsStorage
{
    private static EssenceMethods.APIFactory<Report> api => new();
    public static CancellationTokenSource _cancellationTokenSource = new();
    public static CancellationToken cancellationToken = _cancellationTokenSource.Token;

    //private static DBObservable _localReports = new();
    //public static DBObservable LocalReports
    //{
    //    get => _localReports;
    //    set
    //    {
    //        if (_localReports != value && value != null)
    //        {
    //            _localReports = value;
    //        }
    //    }
    //}

    #region GetReport

    public static async Task GetReport(int id, ChangeOrCreateVM? viewModel)
    {
        Report? newRep;
        var reps = MainWindowVM.LocalReports.Reports_Collection
            .FirstOrDefault(reports => reports.Report_Collection
                .Any(report => report.Id == Convert.ToInt32(id)));
        var checkedRep = reps.Report_Collection.FirstOrDefault(x => x.Id == Convert.ToInt32(id));
        if (checkedRep.Rows == null || checkedRep.Rows.Count == 0)
        {
            //var api = new EssenceMethods.APIFactory<Report>();
            newRep = await api.GetAsync(Convert.ToInt32(id));
            var forms = newRep.Rows11;
            //await using var db = new DBModel(StaticConfiguration.DBPath);

            var oldRep = reps.Report_Collection.First(report => report.Id == newRep.Id);
            oldRep.Rows.Remove(forms);
            oldRep.Rows.Add(forms);

            //reps.Report_Collection.Remove(checkedRep);
            //reps.Report_Collection.Add(rep);
        }
        else
            newRep = checkedRep;

        if (newRep != null && viewModel != null)
            viewModel.Storage = newRep;

    }

    #endregion
}