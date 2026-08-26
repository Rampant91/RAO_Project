using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Read-only снимок отчёта и org для проверки (без tracked/paging-сущностей формы).
/// </summary>
public static class ReportCheckSnapshotLoader
{
    public static async Task<Report?> LoadAsync(
        DBModel db,
        int reportId,
        string formNum,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Report> query = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Reports!).ThenInclude(r => r.DBObservable)
            .Include(x => x.Reports!).ThenInclude(r => r.Master_DB!).ThenInclude(m => m.Rows10)
            .Include(x => x.Reports!).ThenInclude(r => r.Master_DB!).ThenInclude(m => m.Rows20)
            .Include(x => x.Reports!).ThenInclude(r => r.Master_DB!).ThenInclude(m => m.Rows40);

        query = ApplyFormRowsInclude(query, formNum);

        query = query.Include(x => x.Notes.OrderBy(n => n.Order));

        return await query
            .Where(x => x.Reports != null && x.Reports.DBObservable != null)
            .FirstOrDefaultAsync(x => x.Id == reportId, cancellationToken);
    }

    public static int CountLoadedRows(Report rep) =>
        rep.FormNum_DB switch
        {
            "1.1" => rep.Rows11?.Count ?? 0,
            "1.2" => rep.Rows12?.Count ?? 0,
            "1.3" => rep.Rows13?.Count ?? 0,
            "1.4" => rep.Rows14?.Count ?? 0,
            "1.5" => rep.Rows15?.Count ?? 0,
            "1.6" => rep.Rows16?.Count ?? 0,
            "1.7" => rep.Rows17?.Count ?? 0,
            "1.8" => rep.Rows18?.Count ?? 0,
            "1.9" => rep.Rows19?.Count ?? 0,
            "2.1" => rep.Rows21?.Count ?? 0,
            "2.2" => rep.Rows22?.Count ?? 0,
            "2.3" => rep.Rows23?.Count ?? 0,
            "2.4" => rep.Rows24?.Count ?? 0,
            "2.5" => rep.Rows25?.Count ?? 0,
            "2.6" => rep.Rows26?.Count ?? 0,
            "2.7" => rep.Rows27?.Count ?? 0,
            "2.8" => rep.Rows28?.Count ?? 0,
            "2.9" => rep.Rows29?.Count ?? 0,
            "2.10" => rep.Rows210?.Count ?? 0,
            "2.11" => rep.Rows211?.Count ?? 0,
            "2.12" => rep.Rows212?.Count ?? 0,
            "4.1" => rep.Rows41?.Count ?? 0,
            _ => rep.Rows.Count
        };

    private static IQueryable<Report> ApplyFormRowsInclude(IQueryable<Report> query, string formNum) =>
        formNum switch
        {
            "1.1" => query.Include(x => x.Rows11.OrderBy(r => r.NumberInOrder_DB)),
            "1.2" => query.Include(x => x.Rows12.OrderBy(r => r.NumberInOrder_DB)),
            "1.3" => query.Include(x => x.Rows13.OrderBy(r => r.NumberInOrder_DB)),
            "1.4" => query.Include(x => x.Rows14.OrderBy(r => r.NumberInOrder_DB)),
            "1.5" => query.Include(x => x.Rows15.OrderBy(r => r.NumberInOrder_DB)),
            "1.6" => query.Include(x => x.Rows16.OrderBy(r => r.NumberInOrder_DB)),
            "1.7" => query.Include(x => x.Rows17.OrderBy(r => r.NumberInOrder_DB)),
            "1.8" => query.Include(x => x.Rows18.OrderBy(r => r.NumberInOrder_DB)),
            "1.9" => query.Include(x => x.Rows19.OrderBy(r => r.NumberInOrder_DB)),
            "2.1" => query.Include(x => x.Rows21.OrderBy(r => r.NumberInOrder_DB)),
            "2.2" => query.Include(x => x.Rows22.OrderBy(r => r.NumberInOrder_DB)),
            "2.3" => query.Include(x => x.Rows23.OrderBy(r => r.NumberInOrder_DB)),
            "2.4" => query.Include(x => x.Rows24.OrderBy(r => r.NumberInOrder_DB)),
            "2.5" => query.Include(x => x.Rows25.OrderBy(r => r.NumberInOrder_DB)),
            "2.6" => query.Include(x => x.Rows26.OrderBy(r => r.NumberInOrder_DB)),
            "2.7" => query.Include(x => x.Rows27.OrderBy(r => r.NumberInOrder_DB)),
            "2.8" => query.Include(x => x.Rows28.OrderBy(r => r.NumberInOrder_DB)),
            "2.9" => query.Include(x => x.Rows29.OrderBy(r => r.NumberInOrder_DB)),
            "2.10" => query.Include(x => x.Rows210.OrderBy(r => r.NumberInOrder_DB)),
            "2.11" => query.Include(x => x.Rows211.OrderBy(r => r.NumberInOrder_DB)),
            "2.12" => query.Include(x => x.Rows212.OrderBy(r => r.NumberInOrder_DB)),
            "4.1" => query.Include(x => x.Rows41.OrderBy(r => r.NumberInOrder_DB)),
            _ => query
        };
}
