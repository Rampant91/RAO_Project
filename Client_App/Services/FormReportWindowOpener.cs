using Avalonia.Controls;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.ViewModels.Forms.Forms5;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms2;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.Forms.Forms5;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Services;

/// <summary>
/// Создание и показ окон редактирования отчёта (общий путь для открытия и замены окна).
/// </summary>
public static class FormReportWindowOpener
{
    private const int DefaultPageSize = 30;

    /// <summary>Последний Count из фоновой загрузки 1.x для in-place reload.</summary>
    private static int? _lastLoadedDbTotalRows;

    /// <summary>Забрать Count, посчитанный в фоне при последнем <see cref="LoadReportDataAsync"/> (1.x).</summary>
    public static int? TakeLastLoadedDbTotalRows()
    {
        var value = _lastLoadedDbTotalRows;
        _lastLoadedDbTotalRows = null;
        return value;
    }

    /// <summary>
    /// Открыть отчёт поверх owner (обычно MainWindow). Overlay снимается при reveal окна.
    /// </summary>
    public static async Task OpenAsync(Report report, MainWindow? owner, WindowState? ownerPrevState = null)
    {
        if (await ReportExportLock.TryBlockReportAccessAsync(report.Id))
            return;

        if (owner != null)
            await owner.ShowReportOpeningOverlayAsync();
        try
        {
            var window = await CreateWindowAsync(report);
            if (window == null)
                return;

            // Full-row forms only: 1.x opens paged, fingerprint needs all rows.
            var formNum = report.FormNum.Value;
            if (formNum is not (
                    "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8" or "1.9"))
            {
                await ReportExportSnapshotService.EnsureSnapshotOnOpenAsync(report);
            }

            if (ownerPrevState.HasValue && window is IFormOwnerStateWindow formWindow)
                formWindow.OwnerPrevState = ownerPrevState.Value;

            await ShowFormDialogAsync(window, owner);
        }
        finally
        {
            // Reveal окна тоже снимает overlay; finally — страховка при ошибке до Show.
            owner?.SetReportOpeningOverlay(false);
        }
    }

    /// <summary>
    /// Создать окно с загруженными данными, без показа.
    /// </summary>
    public static async Task<Window?> CreateWindowAsync(Report report)
    {
        var numForm = report.FormNum.Value;
        var db = StaticConfiguration.DBModel;

        var queryWithNotes = db.ReportCollectionDbSet
            .AsSplitQuery()
            .AsQueryable()
            .Include(r => r.Reports)
            .Include(rep => rep.Notes);

        var queryWithOutNotes = db.ReportCollectionDbSet
            .AsSplitQuery()
            .AsQueryable()
            .Include(r => r.Reports);

        return numForm switch
        {
            "1.1" => await CreateForm1xPagedAsync(report, db, "1.1", r => new Form_11VM(r), vm => new Form_11(vm)),
            "1.2" => await CreateForm1xPagedAsync(report, db, "1.2", r => new Form_12VM(r), vm => new Form_12(vm)),
            "1.3" => await CreateForm1xPagedAsync(report, db, "1.3", r => new Form_13VM(r), vm => new Form_13(vm)),
            "1.4" => await CreateForm1xPagedAsync(report, db, "1.4", r => new Form_14VM(r), vm => new Form_14(vm)),
            "1.5" => await CreateForm1xPagedAsync(report, db, "1.5", r => new Form_15VM(r), vm => new Form_15(vm)),
            "1.6" => await CreateForm1xPagedAsync(report, db, "1.6", r => new Form_16VM(r), vm => new Form_16(vm)),
            "1.7" => await CreateForm1xPagedAsync(report, db, "1.7", r => new Form_17VM(r), vm => new Form_17(vm)),
            "1.8" => await CreateForm1xPagedAsync(report, db, "1.8", r => new Form_18VM(r), vm => new Form_18(vm)),
            "1.9" => await CreateForm1xPagedAsync(report, db, "1.9", r => new Form_19VM(r), vm => new Form_19(vm)),
            "2.1" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows21).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows21 = dbR.Rows21, r => new Form_21(new Form_21VM(r))),
            "2.2" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows22).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows22 = dbR.Rows22, r => new Form_22(new Form_22VM(r))),
            "2.3" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows23).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows23 = dbR.Rows23, r => new Form_23(new Form_23VM(r))),
            "2.4" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows24).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows24 = dbR.Rows24, r => new Form_24(new Form_24VM(r))),
            "2.5" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows25).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows25 = dbR.Rows25, r => new Form_25(new Form_25VM(r))),
            "2.6" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows26).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows26 = dbR.Rows26, r => new Form_26(new Form_26VM(r))),
            "2.7" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows27).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows27 = dbR.Rows27, r => new Form_27(new Form_27VM(r))),
            "2.8" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows28).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows28 = dbR.Rows28, r => new Form_28(new Form_28VM(r))),
            "2.9" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows29).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows29 = dbR.Rows29, r => new Form_29(new Form_29VM(r))),
            "2.10" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows210).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows210 = dbR.Rows210, r => new Form_210(new Form_210VM(r))),
            "2.11" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows211).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows211 = dbR.Rows211, r => new Form_211(new Form_211VM(r))),
            "2.12" => CreateFromRows(queryWithNotes.Include(rep => rep.Rows212).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows212 = dbR.Rows212, r => new Form_212(new Form_212VM(r))),
            "4.1" => CreateFromRows(queryWithOutNotes.Include(rep => rep.Rows41).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows41 = dbR.Rows41, r => new Form_41(new Form_41VM(r))),
            "5.1" => CreateFromRows(queryWithOutNotes.Include(rep => rep.Rows51).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows51 = dbR.Rows51, r => new Form_51(new Form_51VM(r))),
            "5.2" => CreateFromRows(queryWithOutNotes.Include(rep => rep.Rows52).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows52 = dbR.Rows52, r => new Form_52(new Form_52VM(r))),
            "5.3" => CreateFromRows(queryWithOutNotes.Include(rep => rep.Rows53).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows53 = dbR.Rows53, r => new Form_53(new Form_53VM(r))),
            "5.4" => CreateFromRows(queryWithOutNotes.Include(rep => rep.Rows54).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows54 = dbR.Rows54, r => new Form_54(new Form_54VM(r))),
            "5.5" => CreateFromRows(queryWithOutNotes.Include(rep => rep.Rows55).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows55 = dbR.Rows55, r => new Form_55(new Form_55VM(r))),
            "5.6" => CreateFromRows(queryWithOutNotes.Include(rep => rep.Rows56).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows56 = dbR.Rows56, r => new Form_56(new Form_56VM(r))),
            "5.7" => CreateFromRows(queryWithOutNotes.Include(rep => rep.Rows57).FirstOrDefault(x => x.Id == report.Id),
                report, (r, dbR) => r.Rows57 = dbR.Rows57, r => new Form_57(new Form_57VM(r))),
            _ => null
        };
    }

    /// <summary>
    /// Загрузить отчёт для in-place смены в том же окне.
    /// Тяжёлое чтение — в фоне (отдельный <see cref="DBModel"/>); apply/attach — на UI-контексте.
    /// </summary>
    public static async Task<Report?> LoadReportDataAsync(Report reportShell)
    {
        var numForm = reportShell.FormNum.Value;
        var reportId = reportShell.Id;
        var dbPath = StaticConfiguration.DBPath;
        var uiDb = StaticConfiguration.DBModel;

        if (numForm is "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8" or "1.9")
        {
            var tracked = await EnsureTrackedReportAsync(reportShell, uiDb).ConfigureAwait(true);
            if (tracked == null)
                return null;

            try
            {
                var (total, pageItems) = await MainWindowDbGate.RunAsync(dbPath, async (db, ct) =>
                {
                    var count = await FormRowsPageLoader.CountAsync(db, reportId, numForm, ct).ConfigureAwait(false);
                    var items = await FormRowsPageLoader
                        .LoadPageListAsync(db, reportId, numForm, skip: 0, take: DefaultPageSize, ct)
                        .ConfigureAwait(false);
                    return (count, items);
                }).ConfigureAwait(true);

                FormRowsPageLoader.ApplyPageToReport(uiDb, tracked, numForm, pageItems);
                tracked.FormNum_DB = numForm;
                _lastLoadedDbTotalRows = total;
                return tracked;
            }
            catch (Exception ex)
            {
                FirebirdLogger.LogError("FormReportWindowOpener.LoadReportDataAsync page failed", ex);
                return null;
            }
        }

        Report? snapshot;
        try
        {
            snapshot = await MainWindowDbGate.RunAsync(dbPath, async (db, _) =>
                await LoadFullReportSnapshotAsync(db, reportId, numForm).ConfigureAwait(false)
            ).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("FormReportWindowOpener.LoadReportDataAsync snapshot failed", ex);
            return null;
        }

        if (snapshot == null)
            return null;

        _lastLoadedDbTotalRows = null;

        var target = await EnsureTrackedReportAsync(reportShell, uiDb).ConfigureAwait(true)
                     ?? reportShell;

        DetachExistingFormRows(uiDb, target, numForm);
        target.Reports ??= snapshot.Reports;
        CopyRows(numForm, target, snapshot);
        if (snapshot.Notes != null)
            target.Notes = snapshot.Notes;
        AttachFormRows(uiDb, target, numForm);
        return target;
    }

    private static async Task<Report?> LoadFullReportSnapshotAsync(DBModel db, int reportId, string numForm)
    {
        var withNotes = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.Reports)
            .Include(rep => rep.Notes);

        var withoutNotes = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.Reports);

        return numForm switch
        {
            "2.1" => await withNotes.Include(rep => rep.Rows21).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.2" => await withNotes.Include(rep => rep.Rows22).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.3" => await withNotes.Include(rep => rep.Rows23).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.4" => await withNotes.Include(rep => rep.Rows24).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.5" => await withNotes.Include(rep => rep.Rows25).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.6" => await withNotes.Include(rep => rep.Rows26).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.7" => await withNotes.Include(rep => rep.Rows27).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.8" => await withNotes.Include(rep => rep.Rows28).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.9" => await withNotes.Include(rep => rep.Rows29).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.10" => await withNotes.Include(rep => rep.Rows210).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.11" => await withNotes.Include(rep => rep.Rows211).FirstOrDefaultAsync(x => x.Id == reportId),
            "2.12" => await withNotes.Include(rep => rep.Rows212).FirstOrDefaultAsync(x => x.Id == reportId),
            "4.1" => await withoutNotes.Include(rep => rep.Rows41).FirstOrDefaultAsync(x => x.Id == reportId),
            "5.1" => await withoutNotes.Include(rep => rep.Rows51).FirstOrDefaultAsync(x => x.Id == reportId),
            "5.2" => await withoutNotes.Include(rep => rep.Rows52).FirstOrDefaultAsync(x => x.Id == reportId),
            "5.3" => await withoutNotes.Include(rep => rep.Rows53).FirstOrDefaultAsync(x => x.Id == reportId),
            "5.4" => await withoutNotes.Include(rep => rep.Rows54).FirstOrDefaultAsync(x => x.Id == reportId),
            "5.5" => await withoutNotes.Include(rep => rep.Rows55).FirstOrDefaultAsync(x => x.Id == reportId),
            "5.6" => await withoutNotes.Include(rep => rep.Rows56).FirstOrDefaultAsync(x => x.Id == reportId),
            "5.7" => await withoutNotes.Include(rep => rep.Rows57).FirstOrDefaultAsync(x => x.Id == reportId),
            _ => null
        };
    }

    private static System.Collections.Generic.IEnumerable<Models.Forms.Form> EnumerateFormRows(Report report, string numForm) =>
        numForm switch
        {
            "2.1" => report.Rows21.Cast<Models.Forms.Form>(),
            "2.2" => report.Rows22.Cast<Models.Forms.Form>(),
            "2.3" => report.Rows23.Cast<Models.Forms.Form>(),
            "2.4" => report.Rows24.Cast<Models.Forms.Form>(),
            "2.5" => report.Rows25.Cast<Models.Forms.Form>(),
            "2.6" => report.Rows26.Cast<Models.Forms.Form>(),
            "2.7" => report.Rows27.Cast<Models.Forms.Form>(),
            "2.8" => report.Rows28.Cast<Models.Forms.Form>(),
            "2.9" => report.Rows29.Cast<Models.Forms.Form>(),
            "2.10" => report.Rows210.Cast<Models.Forms.Form>(),
            "2.11" => report.Rows211.Cast<Models.Forms.Form>(),
            "2.12" => report.Rows212.Cast<Models.Forms.Form>(),
            "4.1" => report.Rows41.Cast<Models.Forms.Form>(),
            "5.1" => report.Rows51.Cast<Models.Forms.Form>(),
            "5.2" => report.Rows52.Cast<Models.Forms.Form>(),
            "5.3" => report.Rows53.Cast<Models.Forms.Form>(),
            "5.4" => report.Rows54.Cast<Models.Forms.Form>(),
            "5.5" => report.Rows55.Cast<Models.Forms.Form>(),
            "5.6" => report.Rows56.Cast<Models.Forms.Form>(),
            "5.7" => report.Rows57.Cast<Models.Forms.Form>(),
            _ => System.Linq.Enumerable.Empty<Models.Forms.Form>()
        };

    private static void DetachExistingFormRows(DBModel db, Report report, string numForm)
    {
        foreach (var form in EnumerateFormRows(report, numForm).ToList())
        {
            var entry = db.Entry(form);
            if (entry.State != EntityState.Detached)
                entry.State = EntityState.Detached;
        }
    }

    private static void AttachFormRows(DBModel db, Report report, string numForm)
    {
        foreach (var form in EnumerateFormRows(report, numForm))
        {
            form.ReportId = report.Id;
            form.Report = report;
            var entry = db.Entry(form);
            if (entry.State != EntityState.Detached)
                continue;
            if (form.Id > 0)
                db.Attach(form);
            else
                db.Add(form);
        }
    }

    public static async Task ShowFormDialogAsync(Window window, MainWindow? owner)
    {
        if (window is IFormDialogHost host)
            await host.ShowFormDialogAsync(owner);
        else if (owner != null)
            await window.ShowDialog(owner);
    }

    private static Window? CreateFromRows(
        Report? dbReport,
        Report report,
        Action<Report, Report> assignRows,
        Func<Report, Window> createWindow)
    {
        if (dbReport == null)
            return null;

        assignRows(report, dbReport);
        report.Reports ??= dbReport.Reports;
        if (dbReport.Notes != null)
            report.Notes = dbReport.Notes;
        return createWindow(report);
    }

    private static void CopyRows(string numForm, Report target, Report source)
    {
        switch (numForm)
        {
            case "2.1": target.Rows21 = source.Rows21; break;
            case "2.2": target.Rows22 = source.Rows22; break;
            case "2.3": target.Rows23 = source.Rows23; break;
            case "2.4": target.Rows24 = source.Rows24; break;
            case "2.5": target.Rows25 = source.Rows25; break;
            case "2.6": target.Rows26 = source.Rows26; break;
            case "2.7": target.Rows27 = source.Rows27; break;
            case "2.8": target.Rows28 = source.Rows28; break;
            case "2.9": target.Rows29 = source.Rows29; break;
            case "2.10": target.Rows210 = source.Rows210; break;
            case "2.11": target.Rows211 = source.Rows211; break;
            case "2.12": target.Rows212 = source.Rows212; break;
            case "4.1": target.Rows41 = source.Rows41; break;
            case "5.1": target.Rows51 = source.Rows51; break;
            case "5.2": target.Rows52 = source.Rows52; break;
            case "5.3": target.Rows53 = source.Rows53; break;
            case "5.4": target.Rows54 = source.Rows54; break;
            case "5.5": target.Rows55 = source.Rows55; break;
            case "5.6": target.Rows56 = source.Rows56; break;
            case "5.7": target.Rows57 = source.Rows57; break;
        }
    }

    private static async Task<Window?> CreateForm1xPagedAsync<TVm>(
        Report report,
        DBModel db,
        string formNum,
        Func<Report, TVm> createVm,
        Func<TVm, Window> createWindow)
        where TVm : BaseFormVM
    {
        var tracked = await EnsureTrackedReportAsync(report, db);
        if (tracked == null)
            return null;

        var total = await FormRowsPageLoader.CountAsync(db, tracked.Id, formNum);
        await FormRowsPageLoader.LoadPageIntoReportAsync(db, tracked, formNum, skip: 0, take: DefaultPageSize);

        var vm = createVm(tracked);
        vm.UseDbPaging = true;
        vm.DbTotalRows = total;
        vm.RowCount = DefaultPageSize;
        vm.UpdateFormList();
        vm.UpdatePageInfo();
        vm.WarmVisiblePageCache();

        return createWindow(vm);
    }

    private static async Task<Report?> EnsureTrackedReportAsync(Report report, DBModel db)
    {
        var tracked = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == report.Id);
        if (tracked == null)
        {
            tracked = await db.ReportCollectionDbSet
                .AsSplitQuery()
                .Include(r => r.Reports!).ThenInclude(reps => reps.Master_DB).ThenInclude(m => m.Rows10)
                .Include(r => r.Reports!).ThenInclude(reps => reps.Master_DB).ThenInclude(m => m.Rows20)
                .Include(r => r.Notes)
                .FirstOrDefaultAsync(x => x.Id == report.Id);
        }
        else if (tracked.Reports == null || tracked.Notes == null || tracked.Notes.Count == 0)
        {
            var meta = await db.ReportCollectionDbSet
                .AsSplitQuery()
                .Include(r => r.Reports!).ThenInclude(reps => reps.Master_DB).ThenInclude(m => m.Rows10)
                .Include(r => r.Reports!).ThenInclude(reps => reps.Master_DB).ThenInclude(m => m.Rows20)
                .Include(r => r.Notes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == report.Id);
            if (meta != null)
            {
                if (tracked.Reports == null)
                    tracked.Reports = meta.Reports;
                if (meta.Notes != null)
                    tracked.Notes = meta.Notes;
            }
        }

        return tracked;
    }
}
