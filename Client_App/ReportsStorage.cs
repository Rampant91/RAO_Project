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
    public static EssenceMethods.APIFactory<Report> Api => new();
    public static EssenceMethods.APIFactory<Reports> ApiReports => new();

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

    public static async Task<Report> GetReportAsync(int id, ChangeOrCreateVM? viewModel = null)
    {
        Report? newRep;
        var db = StaticConfiguration.DBModel;
        var reps = LocalReports.Reports_Collection
            .FirstOrDefault(reports => reports.Report_Collection
                .Any(report => report.Id == Convert.ToInt32(id)));  //организация в локальном хранилище
        var checkedRep = db.Set<Report>().Local.FirstOrDefault(entry => entry.Id.Equals(id));   //отчет в локальном хранилище
        if (checkedRep != null && (checkedRep.Rows.ToList<Form>().Any(form => form == null)
                                   || checkedRep.Rows.Count != await GetReportRowsCount(checkedRep))) //если в отчете изменилось количество форм
        {
            newRep = await Api.GetAsync(Convert.ToInt32(id));   //загружаем отчет из БД
            db.Entry(checkedRep).State = EntityState.Detached; //убираем отчет из локального хранилища из отслеживания
            db.Set<Report>().Attach(newRep); //добавляем новый отчет в отслеживание
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
        return newRep;
    }

    #endregion

    #region GetReportRowsCount

    public static async Task<int> GetReportRowsCount(Report rep)
    {
        while (StaticConfiguration.IsFileLocked(null)) Thread.Sleep(50);
        await using var db = new DBModel(StaticConfiguration.DBPath);
        return rep.FormNum_DB switch
        {
            "1.1" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows11)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows11)
                .CountAsync(),
            "1.2" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows12)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows12)
                .CountAsync(),
            "1.3" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows13)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows13)
                .CountAsync(),
            "1.4" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows14)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows14)
                .CountAsync(),
            "1.5" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows15)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows15)
                .CountAsync(),
            "1.6" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows16)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows16)
                .CountAsync(),
            "1.7" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows17)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows17)
                .CountAsync(),
            "1.8" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows18)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows18)
                .CountAsync(),
            "1.9" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows19)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows19)
                .CountAsync(),
            "2.1" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows21)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows21)
                .CountAsync(),
            "2.2" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows22)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows22)
                .CountAsync(),
            "2.3" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows23)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows23)
                .CountAsync(),
            "2.4" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows24)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows24)
                .CountAsync(),
            "2.5" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows25)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows25)
                .CountAsync(),
            "2.6" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows26)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows26)
                .CountAsync(),
            "2.7" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows27)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows27)
                .CountAsync(),
            "2.8" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows28)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows28)
                .CountAsync(),
            "2.9" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows29)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows29)
                .CountAsync(),
            "2.10" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows210)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows210)
                .CountAsync(),
            "2.11" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows211)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows211)
                .CountAsync(),
            "2.12" => await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Rows212)
                .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id)
                .SelectMany(x => x.Rows212)
                .CountAsync(),
            _ => 0
        };
    }

    #endregion
    
    //#region InventoryCheck

    //public static string InventoryCheck(Report? rep)
    //{
    //    if (rep is null)
    //    {
    //        return "";
    //    }
    //    var formsIds = StaticConfiguration.DBModel.ReportCollectionDbSet
    //        .AsNoTracking()
    //        .AsSplitQuery()
    //        .AsQueryable()
    //        .Where(x => x.Id == rep.Id)
    //        .Include(x => x.Rows11)
    //        .First(x => x.Id == rep.Id).Rows11
    //        .Select(x => x.Id);
    //    var countCode10 = StaticConfiguration.DBModel.form_11
    //        .AsNoTracking()
    //        .AsSplitQuery()
    //        .AsQueryable()
    //        .Where(x => x.Id >= formsIds.Min() && x.Id <= formsIds.Max())
    //        .Count(x => x.OperationCode_DB == "10");
    //    return countCode10 == rep.Rows.Count && rep.Rows.Count > 0
    //        ? " (ИНВ)"
    //        : countCode10 > 0
    //            ? " (инв)"
    //            : "";
    //}

    //#endregion
}