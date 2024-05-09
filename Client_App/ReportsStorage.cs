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
                                   || checkedRep.Rows.Count != GetReportRowsCount(checkedRep))) //если в отчете изменилось количество форм
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

    public static int GetReportRowsCount(Report rep)
    {
        while (StaticConfiguration.IsFileLocked(null)) Thread.Sleep(50);
        var db = StaticConfiguration.DBModel;
        return rep.FormNum_DB switch
            {
                "1.1" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows11)
                    .SelectMany(x => x.Rows11)
                    .Count(),
                "1.2" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows12)
                    .SelectMany(x => x.Rows12)
                    .Count(),
                "1.3" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows13)
                    .SelectMany(x => x.Rows13)
                    .Count(),
                "1.4" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows14)
                    .SelectMany(x => x.Rows14)
                    .Count(),
                "1.5" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows15)
                    .SelectMany(x => x.Rows15)
                    .Count(),
                "1.6" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows16)
                    .SelectMany(x => x.Rows16)
                    .Count(),
                "1.7" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows17)
                    .SelectMany(x => x.Rows17)
                    .Count(),
                "1.8" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows18)
                    .SelectMany(x => x.Rows18)
                    .Count(),
                "1.9" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows19)
                    .SelectMany(x => x.Rows19)
                    .Count(),
                "2.1" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows21)
                    .SelectMany(x => x.Rows21)
                    .Count(),
                "2.2" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows22)
                    .SelectMany(x => x.Rows22)
                    .Count(),
                "2.3" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows23)
                    .SelectMany(x => x.Rows23)
                    .Count(),
                "2.4" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows24)
                    .SelectMany(x => x.Rows24)
                    .Count(),
                "2.5" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows25)
                    .SelectMany(x => x.Rows25)
                    .Count(),
                "2.6" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows26)
                    .SelectMany(x => x.Rows26)
                    .Count(),
                "2.7" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows27)
                    .SelectMany(x => x.Rows27)
                    .Count(),
                "2.8" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows28)
                    .SelectMany(x => x.Rows28)
                    .Count(),
                "2.9" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows29)
                    .SelectMany(x => x.Rows29)
                    .Count(),
                "2.10" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows210)
                    .SelectMany(x => x.Rows210)
                    .Count(),
                "2.11" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows211)
                    .SelectMany(x => x.Rows211)
                    .Count(),
                "2.12" => db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Id == rep.Id)
                    .Include(x => x.Rows212)
                    .SelectMany(x => x.Rows212)
                    .Count(),
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