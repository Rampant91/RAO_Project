using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Поиск организации по рег.№/ОКПО (логика как GetReports11/21FromLocalEqual),
/// без зависимости от preload form_10 в LocalReports.
/// Возвращает сущность из LocalReports (tracked), с догруженными Rows10/20.
/// </summary>
public static class OrgMatchQuery
{
    private sealed class TitleRow
    {
        public string Okpo { get; init; } = "";
        public string RegNo { get; init; } = "";
    }

    private sealed class OrgTitleCandidate
    {
        public int ReportsId { get; init; }
        public TitleRow Row0 { get; init; } = new();
        public TitleRow Row1 { get; init; } = new();
    }

    public static Reports? FindForm10Equal(Reports impReps)
    {
        if (impReps.Master_DB?.FormNum_DB is not "1.0")
            return null;
        if (impReps.Master_DB.Rows10.Count < 2)
            return null;

        try
        {
            var imp0 = ToTitle(impReps.Master.Rows10[0].Okpo_DB, impReps.Master.Rows10[0].RegNo_DB);
            var imp1 = ToTitle(impReps.Master.Rows10[1].Okpo_DB, impReps.Master.Rows10[1].RegNo_DB);
            var id = FindForm10Id(StaticConfiguration.DBModel, imp0, imp1);
            return id is null ? null : ResolveLocalOrg(id.Value);
        }
        catch
        {
            return null;
        }
    }

    public static Reports? FindForm20Equal(Reports impReps)
    {
        if (impReps.Master_DB?.FormNum_DB is not "2.0")
            return null;
        if (impReps.Master_DB.Rows20.Count < 2)
            return null;

        try
        {
            var imp0 = ToTitle(impReps.Master.Rows20[0].Okpo_DB, impReps.Master.Rows20[0].RegNo_DB);
            var imp1 = ToTitle(impReps.Master.Rows20[1].Okpo_DB, impReps.Master.Rows20[1].RegNo_DB);
            var id = FindForm20Id(StaticConfiguration.DBModel, imp0, imp1);
            return id is null ? null : ResolveLocalOrg(id.Value);
        }
        catch
        {
            return null;
        }
    }

    public static Reports? FindForm40Equal(Reports impReps)
    {
        if (impReps.Master_DB?.FormNum_DB is not "4.0")
            return null;
        try
        {
            EnsureTitleRowsLoaded(impReps);
            var code = impReps.Master_DB.Rows40.Count > 0
                ? impReps.Master_DB.Rows40[0].CodeSubjectRF_DB
                : null;
            return FindForm40ByCode(code);
        }
        catch
        {
            return null;
        }
    }

    public static Reports? FindForm50Equal(Reports impReps)
    {
        if (impReps.Master_DB?.FormNum_DB is not "5.0")
            return null;
        try
        {
            EnsureTitleRowsLoaded(impReps);
            var name = impReps.Master_DB.Rows50.Count > 0
                ? impReps.Master_DB.Rows50[0].Name_DB
                : null;
            return FindForm50ByName(name);
        }
        catch
        {
            return null;
        }
    }

    public static Reports? FindForm40ByCode(string? codeSubjectRf)
    {
        if (string.IsNullOrWhiteSpace(codeSubjectRf))
            return null;
        try
        {
            var id = StaticConfiguration.DBModel.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == "4.0")
                .Select(x => new
                {
                    x.Id,
                    Code = x.Master_DB.Rows40
                        .OrderBy(r => r.NumberInOrder_DB)
                        .Select(r => r.CodeSubjectRF_DB)
                        .FirstOrDefault()
                })
                .AsEnumerable()
                .FirstOrDefault(x => x.Code == codeSubjectRf)
                ?.Id;
            return id is null ? null : ResolveLocalOrg(id.Value);
        }
        catch
        {
            return null;
        }
    }

    public static Reports? FindForm50ByName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;
        try
        {
            var id = StaticConfiguration.DBModel.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == "5.0")
                .Select(x => new
                {
                    x.Id,
                    Name = x.Master_DB.Rows50
                        .OrderBy(r => r.NumberInOrder_DB)
                        .Select(r => r.Name_DB)
                        .FirstOrDefault()
                })
                .AsEnumerable()
                .FirstOrDefault(x => x.Name == name)
                ?.Id;
            return id is null ? null : ResolveLocalOrg(id.Value);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Matching для Excel-листа 1.0 / 2.0 по ячейкам OKPO/RegNo.</summary>
    public static Reports? FindByExcelTitleFields(
        string formNum, string? excelOkpo0, string? excelOkpo1, string? excelRegNo)
    {
        // В Excel RegNo одно поле (F6) — сравнивается и с row0, и с row1 как в старом GetBaseReps.
        var imp0 = ToTitle(excelOkpo0, excelRegNo);
        var imp1 = ToTitle(excelOkpo1, excelRegNo);

        return formNum switch
        {
            "1.0" => FindForm10Id(StaticConfiguration.DBModel, imp0, imp1) is { } id10
                ? ResolveLocalOrg(id10)
                : null,
            "2.0" => FindForm20Id(StaticConfiguration.DBModel, imp0, imp1) is { } id20
                ? ResolveLocalOrg(id20)
                : null,
            _ => null
        };
    }

    /// <summary>Догружает Rows10/20 в tracked Master организации, если их ещё нет в памяти.</summary>
    public static void EnsureTitleRowsLoaded(Reports reps)
    {
        var master = reps.Master_DB;
        if (master == null) return;
        EnsureMasterReportTitleRows(master);
    }

    /// <summary>Догружает строки титула на Report 1.0/2.0 (для Excel-заголовка и импорта).</summary>
    public static void EnsureMasterReportTitleRows(Report master)
    {
        if (master == null) return;
        var db = StaticConfiguration.DBModel;

        switch (master.FormNum_DB)
        {
            case "1.0" when master.Rows10.Count < 2:
                if (!TryEntryLoad(db, master, m => m.Rows10))
                {
                    var ordered = db.form_10.AsNoTracking()
                        .Where(f => f.ReportId == master.Id)
                        .OrderBy(f => f.NumberInOrder_DB)
                        .ToList();
                    master.Rows10.Clear();
                    foreach (var r in ordered)
                        master.Rows10.Add(r);
                }
                break;
            case "2.0" when master.Rows20.Count < 2:
                if (!TryEntryLoad(db, master, m => m.Rows20))
                {
                    var ordered = db.form_20.AsNoTracking()
                        .Where(f => f.ReportId == master.Id)
                        .OrderBy(f => f.NumberInOrder_DB)
                        .ToList();
                    master.Rows20.Clear();
                    foreach (var r in ordered)
                        master.Rows20.Add(r);
                }
                break;
            case "4.0" when master.Rows40.Count < 1:
                TryEntryLoad(db, master, m => m.Rows40);
                break;
            case "5.0" when master.Rows50.Count < 1:
                TryEntryLoad(db, master, m => m.Rows50);
                break;
        }
    }

    private static bool TryEntryLoad<T>(
        DBModel db,
        Report master,
        System.Linq.Expressions.Expression<Func<Report, IEnumerable<T>>> nav)
        where T : class
    {
        try
        {
            var entry = db.Entry(master);
            if (entry.State == EntityState.Detached)
                return false;
            entry.Collection(nav).Load();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static int? FindForm10Id(DBModel db, TitleRow imp0, TitleRow imp1)
    {
        var candidates = LoadForm10Candidates(db);
        return MatchTitle(candidates, imp0, imp1);
    }

    private static int? FindForm20Id(DBModel db, TitleRow imp0, TitleRow imp1)
    {
        var candidates = LoadForm20Candidates(db);
        return MatchTitle(candidates, imp0, imp1);
    }

    private static List<OrgTitleCandidate> LoadForm10Candidates(DBModel db) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == "1.0")
            .Select(x => new
            {
                x.Id,
                Rows = x.Master_DB.Rows10
                    .OrderBy(r => r.NumberInOrder_DB)
                    .Select(r => new { r.Okpo_DB, r.RegNo_DB })
                    .ToList()
            })
            .AsEnumerable()
            .Select(x => ToCandidate(x.Id, x.Rows.Select(r => (r.Okpo_DB, r.RegNo_DB)).ToList()))
            .ToList();

    private static List<OrgTitleCandidate> LoadForm20Candidates(DBModel db) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == "2.0")
            .Select(x => new
            {
                x.Id,
                Rows = x.Master_DB.Rows20
                    .OrderBy(r => r.NumberInOrder_DB)
                    .Select(r => new { r.Okpo_DB, r.RegNo_DB })
                    .ToList()
            })
            .AsEnumerable()
            .Select(x => ToCandidate(x.Id, x.Rows.Select(r => (r.Okpo_DB, r.RegNo_DB)).ToList()))
            .ToList();

    private static OrgTitleCandidate ToCandidate(int id, List<(string Okpo, string RegNo)> rows)
    {
        var r0 = rows.ElementAtOrDefault(0);
        var r1 = rows.ElementAtOrDefault(1);
        return new OrgTitleCandidate
        {
            ReportsId = id,
            Row0 = ToTitle(r0.Okpo, r0.RegNo),
            Row1 = ToTitle(r1.Okpo, r1.RegNo)
        };
    }

    /// <summary>Те же ветки, что в GetReports11FromLocalEqual / GetBaseReps.</summary>
    private static int? MatchTitle(IReadOnlyList<OrgTitleCandidate> candidates, TitleRow imp0, TitleRow imp1)
    {
        var primary = candidates.FirstOrDefault(t =>
            // обособленные пусты и в базе и в импорте — головное
            (imp0.Okpo == t.Row0.Okpo
             && imp0.RegNo == t.Row0.RegNo
             && imp1.Okpo == ""
             && t.Row1.Okpo == "")
            // обособленные пусты, в базе пуст рег№ юрлица — рег№ обособленного
            || (imp0.Okpo == t.Row0.Okpo
                && imp0.RegNo == t.Row1.RegNo
                && imp1.Okpo == ""
                && t.Row1.Okpo == "")
            // обособленные не пусты
            || (imp1.Okpo == t.Row1.Okpo
                && imp1.RegNo == t.Row1.RegNo
                && imp1.Okpo != "")
            // обособленные не пусты, в базе пуст рег№ обособленного — рег№ юрлица
            || (imp1.Okpo == t.Row1.Okpo
                && imp1.RegNo == t.Row0.RegNo
                && imp1.Okpo != ""
                && t.Row1.RegNo == ""));

        if (primary != null)
            return primary.ReportsId;

        // сбитый ОКПО: юрлицо ↔ обособленное
        var crossed = candidates.FirstOrDefault(t =>
            (imp1.Okpo != ""
             && t.Row1.Okpo == ""
             && imp1.Okpo == t.Row0.Okpo
             && imp1.RegNo == t.Row0.RegNo)
            || (imp1.Okpo == ""
                && t.Row1.Okpo != ""
                && imp0.Okpo == t.Row1.Okpo
                && imp0.RegNo == t.Row1.RegNo));

        return crossed?.ReportsId;
    }

    private static TitleRow ToTitle(string? okpo, string? regNo) =>
        new() { Okpo = okpo ?? "", RegNo = regNo ?? "" };

    private static Reports? ResolveLocalOrg(int reportsId)
    {
        var local = ReportsStorage.LocalReports?.Reports_Collection
            .FirstOrDefault(r => r.Id == reportsId);

        if (local != null)
        {
            EnsureTitleRowsLoaded(local);
            return local;
        }

        // Org есть в БД, но ещё не в Local (редко) — грузим через API.
        var fromApi = ReportsStorage.ApiReports.GetAsync(reportsId).GetAwaiter().GetResult();
        if (fromApi != null)
            EnsureTitleRowsLoaded(fromApi);
        return fromApi;
    }
}
