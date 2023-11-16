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
        var db = StaticConfiguration.DBModel;
        var reps = LocalReports.Reports_Collection
            .FirstOrDefault(reports => reports.Report_Collection
                .Any(report => report.Id == Convert.ToInt32(id)));  //организация в локальном хранилище
        var checkedRep = db.Set<Report>().Local.FirstOrDefault(entry => entry.Id.Equals(id));   //отчет в локальном хранилище
        if (checkedRep != null && (checkedRep.Rows.ToList<Form>().Any(form => form == null) || checkedRep.Rows.Count == 0)) //если в отчете нет форм
        {
            newRep = await api.GetAsync(Convert.ToInt32(id));   //загружаем отчет из БД
            db.Entry(checkedRep).State = EntityState.Detached; //убираем отчет из локального хранилища из отслеживания
            db.Set<Report>().Attach(newRep);    //добавляем новый отчет в отслеживание
            //db.Entry(newRep).State = EntityState.Modified;  //устанавливаем флаг, что этот отчет был изменен и требует перезаписи в БД
            //await db.SaveChangesAsync();
            reps.Report_Collection.Replace(checkedRep, newRep); //заменяем отчет в локальном хранилище на тот, в котором загружены формы
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