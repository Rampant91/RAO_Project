using Models.Collections;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.DBAPIFactory;
using Client_App.ViewModels;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms;

namespace Client_App;

public static class ReportsStorage
{
    private static EssenceMethods.APIFactory<Report> api => new();
    public static CancellationTokenSource _cancellationTokenSource = new();
    public static CancellationToken cancellationToken = _cancellationTokenSource.Token;

    private static DBObservable _localReports = new();
    public static DBObservable LocalReports
    {
        get => _localReports;
        set
        {
            if (_localReports != value && value != null)
            {
                _localReports = value;
            }
        }
    }

    #region GetReport

    public static async Task GetReport(int id, ChangeOrCreateVM? viewModel)
    {
        Report? newRep;
        var reps = LocalReports.Reports_Collection
            .FirstOrDefault(reports => reports.Report_Collection
                .Any(report => report.Id == Convert.ToInt32(id)));
        var checkedRep = reps.Report_Collection.FirstOrDefault(x => x.Id == Convert.ToInt32(id));
        if (checkedRep != null && (checkedRep.Rows.ToList<Form>().Any(form => form == null) || checkedRep.Rows.Count == 0))
        {
            //var api = new EssenceMethods.APIFactory<Report>();
            //var forms = newRep.Rows11;
            //await using var db = new DBModel(StaticConfiguration.DBPath);
            
            
            
            //oldRep.Rows.Remove(forms);
            //oldRep.Rows.Add(forms);

            
            
            //var oldRep = reps.Report_Collection.First(report => report.Id == newRep.Id);

            //StaticConfiguration.DBModel.ReportCollectionDbSet.Local.Remove(oldRep);
            //StaticConfiguration.DBModel.ReportCollectionDbSet.Local.Add(newRep);
            //StaticConfiguration.DBModel.ReportCollectionDbSet.Local.OrderBy(x => x.Id);
            
            //await StaticConfiguration.DBModel.SaveChangesAsync();

            //StaticConfiguration.DBModel.Attach(newRep);
            //StaticConfiguration.DBModel.Entry(newRep).State = EntityState.Detached;

            newRep = await api.GetAsync(Convert.ToInt32(id));

            var db = StaticConfiguration.DBModel;
            var local = db.Set<Report>().Local.FirstOrDefault(entry => entry.Id.Equals(id));
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }
            db.Entry(newRep).State = EntityState.Modified;
            await db.SaveChangesAsync();
            reps.Report_Collection.Replace(checkedRep, newRep);
        }
        else
            newRep = checkedRep;

        if (newRep != null && viewModel != null)
        {
            viewModel.Storage = newRep;
            viewModel.Storages = reps;
        }
    }

    #endregion
}