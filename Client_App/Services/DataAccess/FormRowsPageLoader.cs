using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Models.Collections;

using Models.DBRealization;

using Models.Forms;

using Models.Forms.Form1;

using Models.Interfaces;



namespace Client_App.Services.DataAccess;



/// <summary>

/// Постраничная загрузка строк форм 1.x из БД (без полного Include всех строк).

/// Страницы грузятся AsNoTracking и затем Attach как Unchanged к tracked Report,

/// чтобы Clear/перелистывание не помечали строки Deleted и чтобы SaveChanges видел новые строки.

/// </summary>

public static class FormRowsPageLoader

{

    public static async Task<int> CountAsync(DBModel db, int reportId, string formNum, CancellationToken ct = default)

    {

        return formNum switch

        {

            "1.1" => await db.form_11.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            "1.2" => await db.form_12.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            "1.3" => await db.form_13.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            "1.4" => await db.form_14.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            "1.5" => await db.form_15.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            "1.6" => await db.form_16.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            "1.7" => await db.form_17.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            "1.8" => await db.form_18.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            "1.9" => await db.form_19.AsNoTracking().CountAsync(f => f.ReportId == reportId, ct),

            _ => 0

        };

    }



    public static async Task<int> GetMaxNumberInOrderAsync(

        DBModel db, int reportId, string formNum, CancellationToken ct = default)

    {

        return formNum switch

        {

            "1.1" => await db.form_11.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            "1.2" => await db.form_12.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            "1.3" => await db.form_13.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            "1.4" => await db.form_14.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            "1.5" => await db.form_15.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            "1.6" => await db.form_16.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            "1.7" => await db.form_17.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            "1.8" => await db.form_18.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            "1.9" => await db.form_19.AsNoTracking().Where(f => f.ReportId == reportId)

                .MaxAsync(f => (int?)f.NumberInOrder_DB, ct) ?? 0,

            _ => 0

        };

    }



    public static bool SupportsDbPaging(string formNum) =>

        formNum is "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8" or "1.9";



    /// <summary>

    /// Загружает страницу строк без записи в Report (для prefetch-кэша).

    /// </summary>

    public static async Task<List<Form>> LoadPageListAsync(

        DBModel db, int reportId, string formNum, int skip, int take, CancellationToken ct = default)

    {

        return formNum switch

        {

            "1.1" => (await PageAsync(db.form_11, reportId, skip, take, ct)).Cast<Form>().ToList(),

            "1.2" => (await PageAsync(db.form_12, reportId, skip, take, ct)).Cast<Form>().ToList(),

            "1.3" => (await PageAsync(db.form_13, reportId, skip, take, ct)).Cast<Form>().ToList(),

            "1.4" => (await PageAsync(db.form_14, reportId, skip, take, ct)).Cast<Form>().ToList(),

            "1.5" => (await PageAsync(db.form_15, reportId, skip, take, ct)).Cast<Form>().ToList(),

            "1.6" => (await PageAsync(db.form_16, reportId, skip, take, ct)).Cast<Form>().ToList(),

            "1.7" => (await PageAsync(db.form_17, reportId, skip, take, ct)).Cast<Form>().ToList(),

            "1.8" => (await PageAsync(db.form_18, reportId, skip, take, ct)).Cast<Form>().ToList(),

            "1.9" => (await PageAsync(db.form_19, reportId, skip, take, ct)).Cast<Form>().ToList(),

            _ => []

        };

    }



    public static void ApplyPageToReport(Report report, string formNum, IReadOnlyList<Form> items) =>

        ApplyPageToReport(StaticConfiguration.DBModel, report, formNum, items);



    public static void ApplyPageToReport(

        DBModel db, Report report, string formNum, IReadOnlyList<Form> items)

    {

        EnsureReportTracked(db, report);

        DetachCurrentFormRows(db, report, formNum);



        switch (formNum)

        {

            case "1.1":

                Replace(report.Rows11, items.Cast<Form11>().ToList());

                break;

            case "1.2":

                Replace(report.Rows12, items.Cast<Form12>().ToList());

                break;

            case "1.3":

                Replace(report.Rows13, items.Cast<Form13>().ToList());

                break;

            case "1.4":

                Replace(report.Rows14, items.Cast<Form14>().ToList());

                break;

            case "1.5":

                Replace(report.Rows15, items.Cast<Form15>().ToList());

                break;

            case "1.6":

                Replace(report.Rows16, items.Cast<Form16>().ToList());

                break;

            case "1.7":

                Replace(report.Rows17, items.Cast<Form17>().ToList());

                break;

            case "1.8":

                Replace(report.Rows18, items.Cast<Form18>().ToList());

                break;

            case "1.9":

                Replace(report.Rows19, items.Cast<Form19>().ToList());

                break;

        }



        AttachPageForms(db, report, formNum);

    }



    /// <summary>

    /// Загружает страницу строк в коллекцию отчёта, заменяя текущее содержимое.

    /// </summary>

    public static async Task LoadPageIntoReportAsync(

        DBModel db, Report report, string formNum, int skip, int take, CancellationToken ct = default)

    {

        var items = await LoadPageListAsync(db, report.Id, formNum, skip, take, ct);

        ApplyPageToReport(db, report, formNum, items);

    }



    /// <summary>

    /// Регистрирует новую строку в ChangeTracker (критично при paging / AsNoTracking-оболочках).

    /// </summary>

    public static void TrackNewFormRow(DBModel db, Report report, Form form)

    {

        EnsureReportTracked(db, report);

        form.Report = report;

        form.ReportId = report.Id;

        var entry = db.Entry(form);

        if (entry.State == EntityState.Detached)

            db.Add(form);

    }



    /// <summary>

    /// Помечает строку Deleted и убирает из коллекции (нужно при Attach-страницах paging).

    /// </summary>

    public static void TrackDeletedFormRow(DBModel db, Report report, Form form)

    {

        EnsureReportTracked(db, report);

        report.Rows.Remove(form);



        var entry = db.Entry(form);

        if (entry.State == EntityState.Detached)

        {

            if (form.Id > 0)

            {

                db.Attach(form);

                db.Remove(form);

            }

            return;

        }



        if (entry.State == EntityState.Added)

            entry.State = EntityState.Detached;

        else if (entry.State != EntityState.Deleted)

            db.Remove(form);

    }



    /// <summary>

    /// Есть ли незакоммиченные изменения строк данной формы отчёта.

    /// </summary>

    public static bool HasPendingFormRowChanges(DBModel db, int reportId, string formNum)

    {

        if (db == null || reportId <= 0 || string.IsNullOrEmpty(formNum))

            return false;



        foreach (var entry in db.ChangeTracker.Entries())

        {

            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))

                continue;

            if (entry.Entity is not Form form)

                continue;

            if (form.ReportId != reportId)

                continue;

            if (!string.IsNullOrEmpty(form.FormNum_DB) && form.FormNum_DB != formNum)

                continue;

            return true;

        }



        return false;

    }



    private static async Task<List<T>> PageAsync<T>(

        DbSet<T> set, int reportId, int skip, int take, CancellationToken ct)

        where T : Form

    {

        return await set

            .AsNoTracking()

            .Where(f => f.ReportId == reportId)

            .OrderBy(f => f.NumberInOrder_DB)

            .Skip(skip)

            .Take(take)

            .ToListAsync(ct);

    }



    private static void Replace<T>(ObservableCollectionWithItemPropertyChanged<T> target, List<T> items)

        where T : class, IKey

    {

        target.Clear();

        target.AddRange(items);

    }



    private static void EnsureReportTracked(DBModel db, Report report)

    {

        var entry = db.Entry(report);

        if (entry.State != EntityState.Detached)

            return;



        var local = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == report.Id);

        if (local != null && !ReferenceEquals(local, report))

        {

            // Уже есть другой tracked экземпляр — не Attach дубликат.

            return;

        }



        db.Attach(report);

    }



    private static void DetachCurrentFormRows(DBModel db, Report report, string formNum)

    {

        foreach (var form in EnumerateFormRows(report, formNum).ToList())

        {

            var entry = db.Entry(form);

            if (entry.State is EntityState.Added)

            {

                // Новая ещё не сохранённая — убираем из трекера, иначе Clear() + потеря ссылки.

                entry.State = EntityState.Detached;

                continue;

            }



            if (entry.State != EntityState.Detached)

                entry.State = EntityState.Detached;

        }

    }



    private static void AttachPageForms(DBModel db, Report report, string formNum)

    {

        foreach (var form in EnumerateFormRows(report, formNum))

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



    private static IEnumerable<Form> EnumerateFormRows(Report report, string formNum) =>

        formNum switch

        {

            "1.1" => report.Rows11.Cast<Form>(),

            "1.2" => report.Rows12.Cast<Form>(),

            "1.3" => report.Rows13.Cast<Form>(),

            "1.4" => report.Rows14.Cast<Form>(),

            "1.5" => report.Rows15.Cast<Form>(),

            "1.6" => report.Rows16.Cast<Form>(),

            "1.7" => report.Rows17.Cast<Form>(),

            "1.8" => report.Rows18.Cast<Form>(),

            "1.9" => report.Rows19.Cast<Form>(),

            _ => Enumerable.Empty<Form>()

        };

}


