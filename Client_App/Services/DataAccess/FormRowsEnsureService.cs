using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Гарантирует, что у отчёта в памяти полный набор строк формы (для проверки, сортировки, fill-all).
/// </summary>
public static class FormRowsEnsureService
{
    public enum RowsLoadState
    {
        Unknown,
        PageOnly,
        AllLoaded
    }

    /// <summary>
    /// Если локальная коллекция строк пуста или короче, чем в БД — подгружает все строки формы.
    /// Не перезагружает при незакоммиченных Added/Modified/Deleted — иначе delete/paste/add
    /// «откатываются» полной подгрузкой из БД.
    /// </summary>
    public static async Task EnsureAllRowsLoadedAsync(
        Report report, CancellationToken ct = default)
    {
        if (report == null || string.IsNullOrEmpty(report.FormNum_DB))
            return;

        var db = StaticConfiguration.DBModel;
        var formNum = report.FormNum_DB;

        if (FormRowsPageLoader.HasPendingFormRowChanges(db, report.Id, formNum))
            return;

        var localCount = GetLocalRowCount(report, formNum);
        var dbCount = await CountRowsInDbAsync(db, report.Id, formNum, ct).ConfigureAwait(false);

        if (dbCount <= localCount && localCount > 0)
            return;

        await ReloadAllRowsAsync(db, report, formNum, ct).ConfigureAwait(false);
    }

    public static async Task ReloadNotesFromDbAsync(
        Report report, CancellationToken ct = default)
    {
        if (report == null || report.Id <= 0)
            return;

        var db = StaticConfiguration.DBModel;
        List<Note> notes;
        await using (var snap = new DBModel(StaticConfiguration.DBPath))
        {
            notes = await snap.ReportCollectionDbSet.AsNoTracking()
                .Where(r => r.Id == report.Id)
                .SelectMany(r => r.Notes)
                .OrderBy(n => n.Order)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        foreach (var entry in db.ChangeTracker.Entries().ToList())
        {
            if (entry.Entity is Note n && n.ReportId == report.Id)
                entry.State = EntityState.Detached;
        }

        report.Notes = new ObservableCollectionWithItemPropertyChanged<Note>(notes);
        foreach (var note in notes)
        {
            note.Report = report;
            note.ReportId = report.Id;
            if (db.Entry(note).State == EntityState.Detached)
                db.Attach(note);
        }
    }

    /// <summary>
    /// После Restore(): перечитать строки формы и примечания из БД в tracked Report
    /// (Deleted после Restore не возвращаются в коллекцию сами).
    /// Для 1.x paging строки не грузятся целиком — только примечания; страницу грузит вызывающий.
    /// </summary>
    public static async Task ForceReplaceRowsAndNotesFromDbAsync(
        Report report, CancellationToken ct = default)
    {
        if (report == null || report.Id <= 0 || string.IsNullOrEmpty(report.FormNum_DB))
            return;

        var db = StaticConfiguration.DBModel;
        var formNum = report.FormNum_DB;

        await ReloadNotesFromDbAsync(report, ct).ConfigureAwait(false);

        if (!FormRowsPageLoader.SupportsDbPaging(formNum))
            await ReloadAllRowsAsync(db, report, formNum, ct).ConfigureAwait(false);
    }

    private static int GetLocalRowCount(Report report, string formNum) => formNum switch
    {
        "1.1" => report.Rows11?.Count ?? 0,
        "1.2" => report.Rows12?.Count ?? 0,
        "1.3" => report.Rows13?.Count ?? 0,
        "1.4" => report.Rows14?.Count ?? 0,
        "1.5" => report.Rows15?.Count ?? 0,
        "1.6" => report.Rows16?.Count ?? 0,
        "1.7" => report.Rows17?.Count ?? 0,
        "1.8" => report.Rows18?.Count ?? 0,
        "1.9" => report.Rows19?.Count ?? 0,
        "2.1" => report.Rows21?.Count ?? 0,
        "2.2" => report.Rows22?.Count ?? 0,
        "2.3" => report.Rows23?.Count ?? 0,
        "2.4" => report.Rows24?.Count ?? 0,
        "2.5" => report.Rows25?.Count ?? 0,
        "2.6" => report.Rows26?.Count ?? 0,
        "2.7" => report.Rows27?.Count ?? 0,
        "2.8" => report.Rows28?.Count ?? 0,
        "2.9" => report.Rows29?.Count ?? 0,
        "2.10" => report.Rows210?.Count ?? 0,
        "2.11" => report.Rows211?.Count ?? 0,
        "2.12" => report.Rows212?.Count ?? 0,
        "4.1" => report.Rows41?.Count ?? 0,
        "5.1" => report.Rows51?.Count ?? 0,
        "5.2" => report.Rows52?.Count ?? 0,
        "5.3" => report.Rows53?.Count ?? 0,
        "5.4" => report.Rows54?.Count ?? 0,
        "5.5" => report.Rows55?.Count ?? 0,
        "5.6" => report.Rows56?.Count ?? 0,
        "5.7" => report.Rows57?.Count ?? 0,
        _ => 0
    };

    private static Task<int> CountRowsInDbAsync(
        DBModel db, int reportId, string formNum, CancellationToken ct)
    {
        var q = db.ReportCollectionDbSet.AsNoTracking().Where(r => r.Id == reportId);
        return formNum switch
        {
            "1.1" => q.SelectMany(r => r.Rows11).CountAsync(ct),
            "1.2" => q.SelectMany(r => r.Rows12).CountAsync(ct),
            "1.3" => q.SelectMany(r => r.Rows13).CountAsync(ct),
            "1.4" => q.SelectMany(r => r.Rows14).CountAsync(ct),
            "1.5" => q.SelectMany(r => r.Rows15).CountAsync(ct),
            "1.6" => q.SelectMany(r => r.Rows16).CountAsync(ct),
            "1.7" => q.SelectMany(r => r.Rows17).CountAsync(ct),
            "1.8" => q.SelectMany(r => r.Rows18).CountAsync(ct),
            "1.9" => q.SelectMany(r => r.Rows19).CountAsync(ct),
            "2.1" => q.SelectMany(r => r.Rows21).CountAsync(ct),
            "2.2" => q.SelectMany(r => r.Rows22).CountAsync(ct),
            "2.3" => q.SelectMany(r => r.Rows23).CountAsync(ct),
            "2.4" => q.SelectMany(r => r.Rows24).CountAsync(ct),
            "2.5" => q.SelectMany(r => r.Rows25).CountAsync(ct),
            "2.6" => q.SelectMany(r => r.Rows26).CountAsync(ct),
            "2.7" => q.SelectMany(r => r.Rows27).CountAsync(ct),
            "2.8" => q.SelectMany(r => r.Rows28).CountAsync(ct),
            "2.9" => q.SelectMany(r => r.Rows29).CountAsync(ct),
            "2.10" => q.SelectMany(r => r.Rows210).CountAsync(ct),
            "2.11" => q.SelectMany(r => r.Rows211).CountAsync(ct),
            "2.12" => q.SelectMany(r => r.Rows212).CountAsync(ct),
            "4.1" => q.SelectMany(r => r.Rows41).CountAsync(ct),
            "5.1" => q.SelectMany(r => r.Rows51).CountAsync(ct),
            "5.2" => q.SelectMany(r => r.Rows52).CountAsync(ct),
            "5.3" => q.SelectMany(r => r.Rows53).CountAsync(ct),
            "5.4" => q.SelectMany(r => r.Rows54).CountAsync(ct),
            "5.5" => q.SelectMany(r => r.Rows55).CountAsync(ct),
            "5.6" => q.SelectMany(r => r.Rows56).CountAsync(ct),
            "5.7" => q.SelectMany(r => r.Rows57).CountAsync(ct),
            _ => Task.FromResult(0)
        };
    }

    private static async Task ReloadAllRowsAsync(
        DBModel db, Report report, string formNum, CancellationToken ct)
    {
        var query = db.ReportCollectionDbSet.AsQueryable();
        query = formNum switch
        {
            "1.1" => query.Include(r => r.Rows11.OrderBy(f => f.NumberInOrder_DB)),
            "1.2" => query.Include(r => r.Rows12.OrderBy(f => f.NumberInOrder_DB)),
            "1.3" => query.Include(r => r.Rows13.OrderBy(f => f.NumberInOrder_DB)),
            "1.4" => query.Include(r => r.Rows14.OrderBy(f => f.NumberInOrder_DB)),
            "1.5" => query.Include(r => r.Rows15.OrderBy(f => f.NumberInOrder_DB)),
            "1.6" => query.Include(r => r.Rows16.OrderBy(f => f.NumberInOrder_DB)),
            "1.7" => query.Include(r => r.Rows17.OrderBy(f => f.NumberInOrder_DB)),
            "1.8" => query.Include(r => r.Rows18.OrderBy(f => f.NumberInOrder_DB)),
            "1.9" => query.Include(r => r.Rows19.OrderBy(f => f.NumberInOrder_DB)),
            "2.1" => query.Include(r => r.Rows21.OrderBy(f => f.NumberInOrder_DB)),
            "2.2" => query.Include(r => r.Rows22.OrderBy(f => f.NumberInOrder_DB)),
            "2.3" => query.Include(r => r.Rows23.OrderBy(f => f.NumberInOrder_DB)),
            "2.4" => query.Include(r => r.Rows24.OrderBy(f => f.NumberInOrder_DB)),
            "2.5" => query.Include(r => r.Rows25.OrderBy(f => f.NumberInOrder_DB)),
            "2.6" => query.Include(r => r.Rows26.OrderBy(f => f.NumberInOrder_DB)),
            "2.7" => query.Include(r => r.Rows27.OrderBy(f => f.NumberInOrder_DB)),
            "2.8" => query.Include(r => r.Rows28.OrderBy(f => f.NumberInOrder_DB)),
            "2.9" => query.Include(r => r.Rows29.OrderBy(f => f.NumberInOrder_DB)),
            "2.10" => query.Include(r => r.Rows210.OrderBy(f => f.NumberInOrder_DB)),
            "2.11" => query.Include(r => r.Rows211.OrderBy(f => f.NumberInOrder_DB)),
            "2.12" => query.Include(r => r.Rows212.OrderBy(f => f.NumberInOrder_DB)),
            "4.1" => query.Include(r => r.Rows41.OrderBy(f => f.NumberInOrder_DB)),
            "5.1" => query.Include(r => r.Rows51.OrderBy(f => f.NumberInOrder_DB)),
            "5.2" => query.Include(r => r.Rows52.OrderBy(f => f.NumberInOrder_DB)),
            "5.3" => query.Include(r => r.Rows53.OrderBy(f => f.NumberInOrder_DB)),
            "5.4" => query.Include(r => r.Rows54.OrderBy(f => f.NumberInOrder_DB)),
            "5.5" => query.Include(r => r.Rows55.OrderBy(f => f.NumberInOrder_DB)),
            "5.6" => query.Include(r => r.Rows56.OrderBy(f => f.NumberInOrder_DB)),
            "5.7" => query.Include(r => r.Rows57.OrderBy(f => f.NumberInOrder_DB)),
            _ => query
        };

        var dbReport = await query.FirstOrDefaultAsync(r => r.Id == report.Id, ct).ConfigureAwait(false);
        if (dbReport == null) return;

        switch (formNum)
        {
            case "1.1": report.Rows11 = dbReport.Rows11; break;
            case "1.2": report.Rows12 = dbReport.Rows12; break;
            case "1.3": report.Rows13 = dbReport.Rows13; break;
            case "1.4": report.Rows14 = dbReport.Rows14; break;
            case "1.5": report.Rows15 = dbReport.Rows15; break;
            case "1.6": report.Rows16 = dbReport.Rows16; break;
            case "1.7": report.Rows17 = dbReport.Rows17; break;
            case "1.8": report.Rows18 = dbReport.Rows18; break;
            case "1.9": report.Rows19 = dbReport.Rows19; break;
            case "2.1": report.Rows21 = dbReport.Rows21; break;
            case "2.2": report.Rows22 = dbReport.Rows22; break;
            case "2.3": report.Rows23 = dbReport.Rows23; break;
            case "2.4": report.Rows24 = dbReport.Rows24; break;
            case "2.5": report.Rows25 = dbReport.Rows25; break;
            case "2.6": report.Rows26 = dbReport.Rows26; break;
            case "2.7": report.Rows27 = dbReport.Rows27; break;
            case "2.8": report.Rows28 = dbReport.Rows28; break;
            case "2.9": report.Rows29 = dbReport.Rows29; break;
            case "2.10": report.Rows210 = dbReport.Rows210; break;
            case "2.11": report.Rows211 = dbReport.Rows211; break;
            case "2.12": report.Rows212 = dbReport.Rows212; break;
            case "4.1": report.Rows41 = dbReport.Rows41; break;
            case "5.1": report.Rows51 = dbReport.Rows51; break;
            case "5.2": report.Rows52 = dbReport.Rows52; break;
            case "5.3": report.Rows53 = dbReport.Rows53; break;
            case "5.4": report.Rows54 = dbReport.Rows54; break;
            case "5.5": report.Rows55 = dbReport.Rows55; break;
            case "5.6": report.Rows56 = dbReport.Rows56; break;
            case "5.7": report.Rows57 = dbReport.Rows57; break;
        }
    }
}
