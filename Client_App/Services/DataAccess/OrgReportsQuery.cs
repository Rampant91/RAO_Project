using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Запросы отчётов организации по Id из БД (не из SelectedReports.Report_Collection).
/// Используется выгрузками Excel/RAODB и проверками наличия форм.
/// </summary>
public static class OrgReportsQuery
{
    /// <summary>
    /// Есть ли у организации хотя бы один отчёт (не master title-only в Report_Collection —
    /// любые записи в ReportCollection, привязанные к org).
    /// </summary>
    public static Task<bool> HasAnyReportsAsync(DBModel db, int reportsId, CancellationToken ct = default) =>
        db.ReportCollectionDbSet
            .AsNoTracking()
            .AnyAsync(r => r.Reports != null && r.Reports.Id == reportsId, ct);

    public static Task<bool> HasFormNumAsync(DBModel db, int reportsId, string formNum, CancellationToken ct = default) =>
        db.ReportCollectionDbSet
            .AsNoTracking()
            .AnyAsync(r => r.Reports != null && r.Reports.Id == reportsId && r.FormNum_DB == formNum, ct);

    public static Task<int> CountOrganizationsAsync(DBModel db, CancellationToken ct = default) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(x => x.DBObservableId != null)
            .CountAsync(ct);

    /// <summary>
    /// Id отчётов организации в стабильном порядке (форма, периоды).
    /// </summary>
    public static async Task<int[]> GetReportIdsAsync(DBModel db, int reportsId, CancellationToken ct = default)
    {
        var stubs = await db.ReportCollectionDbSet
            .AsNoTracking()
            .Where(r => r.Reports != null && r.Reports.Id == reportsId)
            .Select(r => new { r.Id, r.FormNum_DB, r.StartPeriod_DB })
            .ToListAsync(ct);

        return stubs
            .OrderBy(x => x.FormNum_DB)
            .ThenBy(x => x.StartPeriod_DB)
            .Select(x => x.Id)
            .ToArray();
    }

    /// <summary>
    /// Загружает организацию со всеми оболочками отчётов (без строк форм).
    /// </summary>
    public static async Task<Reports?> LoadOrgWithReportShellsAsync(
        DBModel db, int reportsId, CancellationToken ct = default)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows40)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows50)
            .Include(x => x.Report_Collection)
            .FirstOrDefaultAsync(x => x.Id == reportsId, ct);
    }

    /// <summary>
    /// Загружает организацию с оболочками отчётов только нужной формы.
    /// </summary>
    public static async Task<Reports?> LoadOrgWithReportShellsForFormAsync(
        DBModel db, int reportsId, string formNum, CancellationToken ct = default)
    {
        var isForm1 = formNum.StartsWith('1');
        IQueryable<Reports> query = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Id == reportsId);

        query = isForm1
            ? query.Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            : query.Include(x => x.Master_DB).ThenInclude(x => x.Rows20);

        query = query.Include(x => x.Report_Collection.Where(rep => rep.FormNum_DB == formNum));

        return await query.FirstOrDefaultAsync(ct);
    }
}
