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
using Models.Forms.Form2;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Поиск организаций Form 1.0 / 2.0 по паре рег.№ + ОКПО.
/// </summary>
public static class FormsExcelOrgResolver
{
    public static async Task<List<Reports>> LoadForm10OrganizationsAsync(DBModel db, CancellationToken ct)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Master_DB.FormNum_DB == "1.0")
            .Include(x => x.Master_DB)
            .ThenInclude(m => m.Rows10.OrderBy(r => r.NumberInOrder_DB))
            .ToListAsync(ct);
    }

    public static async Task<List<Reports>> LoadForm20OrganizationsAsync(DBModel db, CancellationToken ct)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Master_DB.FormNum_DB == "2.0")
            .Include(x => x.Master_DB)
            .ThenInclude(m => m.Rows20.OrderBy(r => r.NumberInOrder_DB))
            .ToListAsync(ct);
    }

    public static FormsExcelOrgLookup BuildForm10Lookup(IEnumerable<Reports> organizations) =>
        BuildLookup(organizations, expectedMasterForm: "1.0", GetForm10ReportingRegNoOkpo);

    public static FormsExcelOrgLookup BuildForm20Lookup(IEnumerable<Reports> organizations) =>
        BuildLookup(organizations, expectedMasterForm: "2.0", GetForm20ReportingRegNoOkpo);

    /// <summary>Обратная совместимость с тестами Form 1.0.</summary>
    public static FormsExcelOrgLookup BuildLookup(IEnumerable<Reports> organizations) =>
        BuildForm10Lookup(organizations);

    private static FormsExcelOrgLookup BuildLookup(
        IEnumerable<Reports> organizations,
        string expectedMasterForm,
        Func<Report, (string RegNo, string Okpo)> keySelector)
    {
        var map = new Dictionary<(string RegNo, string Okpo), Reports>();
        var ambiguous = new HashSet<(string RegNo, string Okpo)>();

        foreach (var org in organizations)
        {
            if (org.Master_DB is not { } master || master.FormNum_DB != expectedMasterForm)
            {
                continue;
            }

            var (regNo, okpo) = keySelector(master);
            if (string.IsNullOrEmpty(regNo) || string.IsNullOrEmpty(okpo))
            {
                continue;
            }

            var key = (regNo, okpo);
            if (ambiguous.Contains(key))
            {
                continue;
            }

            if (!map.TryAdd(key, org))
            {
                map.Remove(key);
                ambiguous.Add(key);
            }
        }

        return new FormsExcelOrgLookup(map, ambiguous);
    }

    public static (string RegNo, string Okpo) GetForm10ReportingRegNoOkpo(Report master)
    {
        var pair = FormsExcelForm10Pair.Select(master?.Rows10);
        var row0 = pair.LegalEntity ?? CreateEmptyForm10(numberInOrder: 1);
        var row1 = pair.SeparateDivision ?? CreateEmptyForm10(numberInOrder: 2);

        var okpo = row1.Okpo_DB is "" or "-"
            ? GetForm10RowOkpo(row0)
            : GetForm10RowOkpo(row1);
        var regNo = (GetForm10RowRegNo(row1) != "" || row1.Okpo_DB == "-") && GetForm10RowOkpo(row1) != ""
            ? GetForm10RowRegNo(row1)
            : GetForm10RowRegNo(row0);
        return (regNo, okpo);
    }

    public static (string RegNo, string Okpo) GetForm20ReportingRegNoOkpo(Report master)
    {
        var pair = FormsExcelForm20Pair.Select(master?.Rows20);
        var row0 = pair.LegalEntity ?? CreateEmptyForm20(numberInOrder: 1);
        var row1 = pair.SeparateDivision ?? CreateEmptyForm20(numberInOrder: 2);

        var okpo = row1.Okpo_DB is "" or "-"
            ? GetForm20RowOkpo(row0)
            : GetForm20RowOkpo(row1);
        var regNo = (GetForm20RowRegNo(row1) != "" || row1.Okpo_DB == "-") && GetForm20RowOkpo(row1) != ""
            ? GetForm20RowRegNo(row1)
            : GetForm20RowRegNo(row0);
        return (regNo, okpo);
    }

    private static Form10 CreateEmptyForm10(int numberInOrder)
    {
        var row = (Form10)FormCreator.Create("1.0");
        row.NumberInOrder_DB = numberInOrder;
        return row;
    }

    private static Form20 CreateEmptyForm20(int numberInOrder)
    {
        var row = (Form20)FormCreator.Create("2.0");
        row.NumberInOrder_DB = numberInOrder;
        return row;
    }

    private static string GetForm10RowOkpo(Form10 row) =>
        string.IsNullOrWhiteSpace(row.Okpo_DB) ? (row.Okpo?.Value ?? "").Trim() : row.Okpo_DB.Trim();

    private static string GetForm10RowRegNo(Form10 row) =>
        string.IsNullOrWhiteSpace(row.RegNo_DB) ? (row.RegNo?.Value ?? "").Trim() : row.RegNo_DB.Trim();

    private static string GetForm20RowOkpo(Form20 row) =>
        string.IsNullOrWhiteSpace(row.Okpo_DB) ? (row.Okpo?.Value ?? "").Trim() : row.Okpo_DB.Trim();

    private static string GetForm20RowRegNo(Form20 row) =>
        string.IsNullOrWhiteSpace(row.RegNo_DB) ? (row.RegNo?.Value ?? "").Trim() : row.RegNo_DB.Trim();
}

/// <summary>
/// Словарь организаций по (RegNo, Okpo) с учётом коллизий.
/// </summary>
public sealed class FormsExcelOrgLookup
{
    private readonly Dictionary<(string RegNo, string Okpo), Reports> _map;
    private readonly HashSet<(string RegNo, string Okpo)> _ambiguous;

    public FormsExcelOrgLookup(
        Dictionary<(string RegNo, string Okpo), Reports> map,
        HashSet<(string RegNo, string Okpo)> ambiguous)
    {
        _map = map;
        _ambiguous = ambiguous;
    }

    public FormsExcelOrgResolveStatus TryResolve(string regNo, string okpo, out Reports? organization)
    {
        organization = null;
        var key = (regNo.Trim(), okpo.Trim());
        if (_ambiguous.Contains(key))
        {
            return FormsExcelOrgResolveStatus.Ambiguous;
        }

        if (_map.TryGetValue(key, out organization))
        {
            return FormsExcelOrgResolveStatus.Found;
        }

        return FormsExcelOrgResolveStatus.NotFound;
    }
}
