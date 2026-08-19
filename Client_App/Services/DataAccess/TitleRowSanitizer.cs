using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Санитизация строк титулов 1.0/2.0 при старте (null → "", trim, удаление \r/\n).
/// </summary>
public static class TitleRowSanitizer
{
    /// <summary>
    /// Возвращает true, если в БД были записаны изменения.
    /// </summary>
    public static async Task<bool> CleanUpAsync(string dbPath, CancellationToken cancellationToken = default)
    {
        await using var db = new DBModel(dbPath);

        var masterIds10 = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(r => r.DBObservableId != null && r.Master_DBId != null && r.Master_DB.FormNum_DB == "1.0")
            .Select(r => r.Master_DBId!.Value)
            .Distinct()
            .ToListAsync(cancellationToken);

        var masterIds20 = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(r => r.DBObservableId != null && r.Master_DBId != null && r.Master_DB.FormNum_DB == "2.0")
            .Select(r => r.Master_DBId!.Value)
            .Distinct()
            .ToListAsync(cancellationToken);

        var anyChanged = false;
        if (masterIds10.Count > 0)
            anyChanged |= await SanitizeForm10Async(db, masterIds10, cancellationToken);
        if (masterIds20.Count > 0)
            anyChanged |= await SanitizeForm20Async(db, masterIds20, cancellationToken);

        if (anyChanged)
            await db.SaveChangesAsync(cancellationToken);

        return anyChanged;
    }

    internal static string CustomTrimForTest(string? str) => CustomTrim(str);

    internal static bool FieldPotentiallyDirtyForTest(string? value) => FieldPotentiallyDirty(value);

    private static async Task<bool> SanitizeForm10Async(
        DBModel db, IReadOnlyList<int> masterIds, CancellationToken cancellationToken)
    {
        var anyChanged = false;
        foreach (var batch in FirebirdInClause.Chunk(masterIds))
        {
            var snapshots = await db.form_10
                .AsNoTracking()
                .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                .Where(f =>
                    f.RegNo_DB == null || f.RegNo_DB.Contains("\r") || f.RegNo_DB.Contains("\n") || f.RegNo_DB != f.RegNo_DB.Trim()
                    || f.OrganUprav_DB == null || f.OrganUprav_DB.Contains("\r") || f.OrganUprav_DB.Contains("\n") || f.OrganUprav_DB != f.OrganUprav_DB.Trim()
                    || f.SubjectRF_DB == null || f.SubjectRF_DB.Contains("\r") || f.SubjectRF_DB.Contains("\n") || f.SubjectRF_DB != f.SubjectRF_DB.Trim()
                    || f.JurLico_DB == null || f.JurLico_DB.Contains("\r") || f.JurLico_DB.Contains("\n") || f.JurLico_DB != f.JurLico_DB.Trim()
                    || f.ShortJurLico_DB == null || f.ShortJurLico_DB.Contains("\r") || f.ShortJurLico_DB.Contains("\n") || f.ShortJurLico_DB != f.ShortJurLico_DB.Trim()
                    || f.JurLicoAddress_DB == null || f.JurLicoAddress_DB.Contains("\r") || f.JurLicoAddress_DB.Contains("\n") || f.JurLicoAddress_DB != f.JurLicoAddress_DB.Trim()
                    || f.JurLicoFactAddress_DB == null || f.JurLicoFactAddress_DB.Contains("\r") || f.JurLicoFactAddress_DB.Contains("\n") || f.JurLicoFactAddress_DB != f.JurLicoFactAddress_DB.Trim()
                    || f.GradeFIO_DB == null || f.GradeFIO_DB.Contains("\r") || f.GradeFIO_DB.Contains("\n") || f.GradeFIO_DB != f.GradeFIO_DB.Trim()
                    || f.Telephone_DB == null || f.Telephone_DB.Contains("\r") || f.Telephone_DB.Contains("\n") || f.Telephone_DB != f.Telephone_DB.Trim()
                    || f.Fax_DB == null || f.Fax_DB.Contains("\r") || f.Fax_DB.Contains("\n") || f.Fax_DB != f.Fax_DB.Trim()
                    || f.Email_DB == null || f.Email_DB.Contains("\r") || f.Email_DB.Contains("\n") || f.Email_DB != f.Email_DB.Trim()
                    || f.Okpo_DB == null || f.Okpo_DB.Contains("\r") || f.Okpo_DB.Contains("\n") || f.Okpo_DB != f.Okpo_DB.Trim()
                    || f.Okved_DB == null || f.Okved_DB.Contains("\r") || f.Okved_DB.Contains("\n") || f.Okved_DB != f.Okved_DB.Trim()
                    || f.Okogu_DB == null || f.Okogu_DB.Contains("\r") || f.Okogu_DB.Contains("\n") || f.Okogu_DB != f.Okogu_DB.Trim()
                    || f.Oktmo_DB == null || f.Oktmo_DB.Contains("\r") || f.Oktmo_DB.Contains("\n") || f.Oktmo_DB != f.Oktmo_DB.Trim()
                    || f.Inn_DB == null || f.Inn_DB.Contains("\r") || f.Inn_DB.Contains("\n") || f.Inn_DB != f.Inn_DB.Trim()
                    || f.Kpp_DB == null || f.Kpp_DB.Contains("\r") || f.Kpp_DB.Contains("\n") || f.Kpp_DB != f.Kpp_DB.Trim()
                    || f.Okopf_DB == null || f.Okopf_DB.Contains("\r") || f.Okopf_DB.Contains("\n") || f.Okopf_DB != f.Okopf_DB.Trim()
                    || f.Okfs_DB == null || f.Okfs_DB.Contains("\r") || f.Okfs_DB.Contains("\n") || f.Okfs_DB != f.Okfs_DB.Trim())
                .Select(f => new TitleRowSnapshot(
                    f.Id,
                    f.RegNo_DB,
                    f.OrganUprav_DB,
                    f.SubjectRF_DB,
                    f.JurLico_DB,
                    f.ShortJurLico_DB,
                    f.JurLicoAddress_DB,
                    f.JurLicoFactAddress_DB,
                    f.GradeFIO_DB,
                    f.Telephone_DB,
                    f.Fax_DB,
                    f.Email_DB,
                    f.Okpo_DB,
                    f.Okved_DB,
                    f.Okogu_DB,
                    f.Oktmo_DB,
                    f.Inn_DB,
                    f.Kpp_DB,
                    f.Okopf_DB,
                    f.Okfs_DB))
                .ToListAsync(cancellationToken);

            foreach (var snapshot in snapshots)
            {
                if (!TrySanitizeSnapshot(snapshot, out var sanitized))
                    continue;

                anyChanged = true;
                AttachForm10Updates(db, snapshot, sanitized);
            }
        }

        return anyChanged;
    }

    private static async Task<bool> SanitizeForm20Async(
        DBModel db, IReadOnlyList<int> masterIds, CancellationToken cancellationToken)
    {
        var anyChanged = false;
        foreach (var batch in FirebirdInClause.Chunk(masterIds))
        {
            var snapshots = await db.form_20
                .AsNoTracking()
                .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                .Where(f =>
                    f.RegNo_DB == null || f.RegNo_DB.Contains("\r") || f.RegNo_DB.Contains("\n") || f.RegNo_DB != f.RegNo_DB.Trim()
                    || f.OrganUprav_DB == null || f.OrganUprav_DB.Contains("\r") || f.OrganUprav_DB.Contains("\n") || f.OrganUprav_DB != f.OrganUprav_DB.Trim()
                    || f.SubjectRF_DB == null || f.SubjectRF_DB.Contains("\r") || f.SubjectRF_DB.Contains("\n") || f.SubjectRF_DB != f.SubjectRF_DB.Trim()
                    || f.JurLico_DB == null || f.JurLico_DB.Contains("\r") || f.JurLico_DB.Contains("\n") || f.JurLico_DB != f.JurLico_DB.Trim()
                    || f.ShortJurLico_DB == null || f.ShortJurLico_DB.Contains("\r") || f.ShortJurLico_DB.Contains("\n") || f.ShortJurLico_DB != f.ShortJurLico_DB.Trim()
                    || f.JurLicoAddress_DB == null || f.JurLicoAddress_DB.Contains("\r") || f.JurLicoAddress_DB.Contains("\n") || f.JurLicoAddress_DB != f.JurLicoAddress_DB.Trim()
                    || f.JurLicoFactAddress_DB == null || f.JurLicoFactAddress_DB.Contains("\r") || f.JurLicoFactAddress_DB.Contains("\n") || f.JurLicoFactAddress_DB != f.JurLicoFactAddress_DB.Trim()
                    || f.GradeFIO_DB == null || f.GradeFIO_DB.Contains("\r") || f.GradeFIO_DB.Contains("\n") || f.GradeFIO_DB != f.GradeFIO_DB.Trim()
                    || f.Telephone_DB == null || f.Telephone_DB.Contains("\r") || f.Telephone_DB.Contains("\n") || f.Telephone_DB != f.Telephone_DB.Trim()
                    || f.Fax_DB == null || f.Fax_DB.Contains("\r") || f.Fax_DB.Contains("\n") || f.Fax_DB != f.Fax_DB.Trim()
                    || f.Email_DB == null || f.Email_DB.Contains("\r") || f.Email_DB.Contains("\n") || f.Email_DB != f.Email_DB.Trim()
                    || f.Okpo_DB == null || f.Okpo_DB.Contains("\r") || f.Okpo_DB.Contains("\n") || f.Okpo_DB != f.Okpo_DB.Trim()
                    || f.Okved_DB == null || f.Okved_DB.Contains("\r") || f.Okved_DB.Contains("\n") || f.Okved_DB != f.Okved_DB.Trim()
                    || f.Okogu_DB == null || f.Okogu_DB.Contains("\r") || f.Okogu_DB.Contains("\n") || f.Okogu_DB != f.Okogu_DB.Trim()
                    || f.Oktmo_DB == null || f.Oktmo_DB.Contains("\r") || f.Oktmo_DB.Contains("\n") || f.Oktmo_DB != f.Oktmo_DB.Trim()
                    || f.Inn_DB == null || f.Inn_DB.Contains("\r") || f.Inn_DB.Contains("\n") || f.Inn_DB != f.Inn_DB.Trim()
                    || f.Kpp_DB == null || f.Kpp_DB.Contains("\r") || f.Kpp_DB.Contains("\n") || f.Kpp_DB != f.Kpp_DB.Trim()
                    || f.Okopf_DB == null || f.Okopf_DB.Contains("\r") || f.Okopf_DB.Contains("\n") || f.Okopf_DB != f.Okopf_DB.Trim()
                    || f.Okfs_DB == null || f.Okfs_DB.Contains("\r") || f.Okfs_DB.Contains("\n") || f.Okfs_DB != f.Okfs_DB.Trim())
                .Select(f => new TitleRowSnapshot(
                    f.Id,
                    f.RegNo_DB,
                    f.OrganUprav_DB,
                    f.SubjectRF_DB,
                    f.JurLico_DB,
                    f.ShortJurLico_DB,
                    f.JurLicoAddress_DB,
                    f.JurLicoFactAddress_DB,
                    f.GradeFIO_DB,
                    f.Telephone_DB,
                    f.Fax_DB,
                    f.Email_DB,
                    f.Okpo_DB,
                    f.Okved_DB,
                    f.Okogu_DB,
                    f.Oktmo_DB,
                    f.Inn_DB,
                    f.Kpp_DB,
                    f.Okopf_DB,
                    f.Okfs_DB))
                .ToListAsync(cancellationToken);

            foreach (var snapshot in snapshots)
            {
                if (!TrySanitizeSnapshot(snapshot, out var sanitized))
                    continue;

                anyChanged = true;
                AttachForm20Updates(db, snapshot, sanitized);
            }
        }

        return anyChanged;
    }

    private static bool TrySanitizeSnapshot(TitleRowSnapshot snapshot, out TitleRowSnapshot sanitized)
    {
        sanitized = snapshot with
        {
            RegNo_DB = CustomTrim(snapshot.RegNo_DB),
            OrganUprav_DB = CustomTrim(snapshot.OrganUprav_DB),
            SubjectRF_DB = CustomTrim(snapshot.SubjectRF_DB),
            JurLico_DB = CustomTrim(snapshot.JurLico_DB),
            ShortJurLico_DB = CustomTrim(snapshot.ShortJurLico_DB),
            JurLicoAddress_DB = CustomTrim(snapshot.JurLicoAddress_DB),
            JurLicoFactAddress_DB = CustomTrim(snapshot.JurLicoFactAddress_DB),
            GradeFIO_DB = CustomTrim(snapshot.GradeFIO_DB),
            Telephone_DB = CustomTrim(snapshot.Telephone_DB),
            Fax_DB = CustomTrim(snapshot.Fax_DB),
            Email_DB = CustomTrim(snapshot.Email_DB),
            Okpo_DB = CustomTrim(snapshot.Okpo_DB),
            Okved_DB = CustomTrim(snapshot.Okved_DB),
            Okogu_DB = CustomTrim(snapshot.Okogu_DB),
            Oktmo_DB = CustomTrim(snapshot.Oktmo_DB),
            Inn_DB = CustomTrim(snapshot.Inn_DB),
            Kpp_DB = CustomTrim(snapshot.Kpp_DB),
            Okopf_DB = CustomTrim(snapshot.Okopf_DB),
            Okfs_DB = CustomTrim(snapshot.Okfs_DB)
        };

        return !snapshot.Equals(sanitized);
    }

    private static void AttachForm10Updates(DBModel db, TitleRowSnapshot before, TitleRowSnapshot after)
    {
        var entity = new Form10 { Id = after.Id };
        db.form_10.Attach(entity);
        var entry = db.Entry(entity);
        SetModifiedIfChanged(entry, e => e.RegNo_DB, before.RegNo_DB, after.RegNo_DB);
        SetModifiedIfChanged(entry, e => e.OrganUprav_DB, before.OrganUprav_DB, after.OrganUprav_DB);
        SetModifiedIfChanged(entry, e => e.SubjectRF_DB, before.SubjectRF_DB, after.SubjectRF_DB);
        SetModifiedIfChanged(entry, e => e.JurLico_DB, before.JurLico_DB, after.JurLico_DB);
        SetModifiedIfChanged(entry, e => e.ShortJurLico_DB, before.ShortJurLico_DB, after.ShortJurLico_DB);
        SetModifiedIfChanged(entry, e => e.JurLicoAddress_DB, before.JurLicoAddress_DB, after.JurLicoAddress_DB);
        SetModifiedIfChanged(entry, e => e.JurLicoFactAddress_DB, before.JurLicoFactAddress_DB, after.JurLicoFactAddress_DB);
        SetModifiedIfChanged(entry, e => e.GradeFIO_DB, before.GradeFIO_DB, after.GradeFIO_DB);
        SetModifiedIfChanged(entry, e => e.Telephone_DB, before.Telephone_DB, after.Telephone_DB);
        SetModifiedIfChanged(entry, e => e.Fax_DB, before.Fax_DB, after.Fax_DB);
        SetModifiedIfChanged(entry, e => e.Email_DB, before.Email_DB, after.Email_DB);
        SetModifiedIfChanged(entry, e => e.Okpo_DB, before.Okpo_DB, after.Okpo_DB);
        SetModifiedIfChanged(entry, e => e.Okved_DB, before.Okved_DB, after.Okved_DB);
        SetModifiedIfChanged(entry, e => e.Okogu_DB, before.Okogu_DB, after.Okogu_DB);
        SetModifiedIfChanged(entry, e => e.Oktmo_DB, before.Oktmo_DB, after.Oktmo_DB);
        SetModifiedIfChanged(entry, e => e.Inn_DB, before.Inn_DB, after.Inn_DB);
        SetModifiedIfChanged(entry, e => e.Kpp_DB, before.Kpp_DB, after.Kpp_DB);
        SetModifiedIfChanged(entry, e => e.Okopf_DB, before.Okopf_DB, after.Okopf_DB);
        SetModifiedIfChanged(entry, e => e.Okfs_DB, before.Okfs_DB, after.Okfs_DB);
    }

    private static void AttachForm20Updates(DBModel db, TitleRowSnapshot before, TitleRowSnapshot after)
    {
        var entity = new Form20 { Id = after.Id };
        db.form_20.Attach(entity);
        var entry = db.Entry(entity);
        SetModifiedIfChanged(entry, e => e.RegNo_DB, before.RegNo_DB, after.RegNo_DB);
        SetModifiedIfChanged(entry, e => e.OrganUprav_DB, before.OrganUprav_DB, after.OrganUprav_DB);
        SetModifiedIfChanged(entry, e => e.SubjectRF_DB, before.SubjectRF_DB, after.SubjectRF_DB);
        SetModifiedIfChanged(entry, e => e.JurLico_DB, before.JurLico_DB, after.JurLico_DB);
        SetModifiedIfChanged(entry, e => e.ShortJurLico_DB, before.ShortJurLico_DB, after.ShortJurLico_DB);
        SetModifiedIfChanged(entry, e => e.JurLicoAddress_DB, before.JurLicoAddress_DB, after.JurLicoAddress_DB);
        SetModifiedIfChanged(entry, e => e.JurLicoFactAddress_DB, before.JurLicoFactAddress_DB, after.JurLicoFactAddress_DB);
        SetModifiedIfChanged(entry, e => e.GradeFIO_DB, before.GradeFIO_DB, after.GradeFIO_DB);
        SetModifiedIfChanged(entry, e => e.Telephone_DB, before.Telephone_DB, after.Telephone_DB);
        SetModifiedIfChanged(entry, e => e.Fax_DB, before.Fax_DB, after.Fax_DB);
        SetModifiedIfChanged(entry, e => e.Email_DB, before.Email_DB, after.Email_DB);
        SetModifiedIfChanged(entry, e => e.Okpo_DB, before.Okpo_DB, after.Okpo_DB);
        SetModifiedIfChanged(entry, e => e.Okved_DB, before.Okved_DB, after.Okved_DB);
        SetModifiedIfChanged(entry, e => e.Okogu_DB, before.Okogu_DB, after.Okogu_DB);
        SetModifiedIfChanged(entry, e => e.Oktmo_DB, before.Oktmo_DB, after.Oktmo_DB);
        SetModifiedIfChanged(entry, e => e.Inn_DB, before.Inn_DB, after.Inn_DB);
        SetModifiedIfChanged(entry, e => e.Kpp_DB, before.Kpp_DB, after.Kpp_DB);
        SetModifiedIfChanged(entry, e => e.Okopf_DB, before.Okopf_DB, after.Okopf_DB);
        SetModifiedIfChanged(entry, e => e.Okfs_DB, before.Okfs_DB, after.Okfs_DB);
    }

    private static void SetModifiedIfChanged<T>(
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> entry,
        System.Linq.Expressions.Expression<Func<T, string>> property,
        string? before,
        string? after)
        where T : class
    {
        if (string.Equals(before, after, StringComparison.Ordinal))
            return;

        entry.Property(property).CurrentValue = after;
        entry.Property(property).IsModified = true;
    }

    private static bool FieldPotentiallyDirty(string? value)
    {
        if (value == null)
            return true;
        if (value.Contains('\r') || value.Contains('\n'))
            return true;
        return value.AsSpan().Trim().Length != value.Length;
    }

    private static string CustomTrim(string? str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        var span = str.AsSpan().Trim();
        var buffer = new char[span.Length];
        var bufferIndex = 0;

        foreach (var currentChar in span)
        {
            if (currentChar != '\r' && currentChar != '\n')
                buffer[bufferIndex++] = currentChar;
        }

        return new string(buffer, 0, bufferIndex);
    }

    private sealed record TitleRowSnapshot(
        int Id,
        string? RegNo_DB,
        string? OrganUprav_DB,
        string? SubjectRF_DB,
        string? JurLico_DB,
        string? ShortJurLico_DB,
        string? JurLicoAddress_DB,
        string? JurLicoFactAddress_DB,
        string? GradeFIO_DB,
        string? Telephone_DB,
        string? Fax_DB,
        string? Email_DB,
        string? Okpo_DB,
        string? Okved_DB,
        string? Okogu_DB,
        string? Oktmo_DB,
        string? Inn_DB,
        string? Kpp_DB,
        string? Okopf_DB,
        string? Okfs_DB);
}
