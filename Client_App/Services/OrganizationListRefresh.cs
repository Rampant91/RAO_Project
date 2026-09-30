using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Collections;

namespace Client_App.Services;

/// <summary>
/// Обновление списка организаций после add/delete/import (warm-cache + OrgKeys).
/// </summary>
public static class OrganizationListRefresh
{
    public static void AfterOrgStructureChanged(MainWindowVM? mainWindowVM)
    {
        Forms1WarmCache.Instance.InvalidateOrgPages();
        MainWindowListQuery.InvalidateAllOrgKeysCaches();
        mainWindowVM?.UpdateOrgsPageInfo();
        mainWindowVM?.UpdateTotalReportCount();
        mainWindowVM?.UpdateTotalReportsCount();
    }

    public static bool Form10HasAnyFilledTitleFields(Report? storage)
    {
        if (storage is null)
            return false;

        if (!string.IsNullOrWhiteSpace(storage.RegNoRep?.Value))
            return true;

        foreach (var key in storage.Rows10)
        {
            if (key is not Form10 row)
                continue;
            if (Row10HasContent(row))
                return true;
        }

        return false;
    }

    public static bool Form20HasAnyFilledTitleFields(Report? storage)
    {
        if (storage is null)
            return false;

        if (!string.IsNullOrWhiteSpace(storage.RegNoRep?.Value))
            return true;

        foreach (var key in storage.Rows20)
        {
            if (key is not Form20 row)
                continue;
            if (Row20HasContent(row))
                return true;
        }

        return false;
    }

    private static bool Row10HasContent(Form10 row) =>
        !string.IsNullOrWhiteSpace(row.OrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.SubjectRF_DB)
        || !string.IsNullOrWhiteSpace(row.JurLico_DB)
        || !string.IsNullOrWhiteSpace(row.ShortJurLico_DB)
        || !string.IsNullOrWhiteSpace(row.JurLicoAddress_DB)
        || !string.IsNullOrWhiteSpace(row.JurLicoFactAddress_DB)
        || !string.IsNullOrWhiteSpace(row.GradeFIO_DB)
        || !string.IsNullOrWhiteSpace(row.Telephone_DB)
        || !string.IsNullOrWhiteSpace(row.Fax_DB)
        || !string.IsNullOrWhiteSpace(row.Email_DB)
        || !string.IsNullOrWhiteSpace(row.Okpo_DB)
        || !string.IsNullOrWhiteSpace(row.Okved_DB)
        || !string.IsNullOrWhiteSpace(row.Okogu_DB)
        || !string.IsNullOrWhiteSpace(row.Oktmo_DB)
        || !string.IsNullOrWhiteSpace(row.Inn_DB)
        || !string.IsNullOrWhiteSpace(row.Kpp_DB)
        || !string.IsNullOrWhiteSpace(row.Okopf_DB)
        || !string.IsNullOrWhiteSpace(row.Okfs_DB)
        || !string.IsNullOrWhiteSpace(row.RegNo_DB);

    private static bool Row20HasContent(Form20 row) =>
        !string.IsNullOrWhiteSpace(row.OrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.SubjectRF_DB)
        || !string.IsNullOrWhiteSpace(row.JurLico_DB)
        || !string.IsNullOrWhiteSpace(row.ShortJurLico_DB)
        || !string.IsNullOrWhiteSpace(row.JurLicoAddress_DB)
        || !string.IsNullOrWhiteSpace(row.JurLicoFactAddress_DB)
        || !string.IsNullOrWhiteSpace(row.GradeFIO_DB)
        || !string.IsNullOrWhiteSpace(row.Telephone_DB)
        || !string.IsNullOrWhiteSpace(row.Fax_DB)
        || !string.IsNullOrWhiteSpace(row.Email_DB)
        || !string.IsNullOrWhiteSpace(row.Okpo_DB)
        || !string.IsNullOrWhiteSpace(row.Okved_DB)
        || !string.IsNullOrWhiteSpace(row.Okogu_DB)
        || !string.IsNullOrWhiteSpace(row.Oktmo_DB)
        || !string.IsNullOrWhiteSpace(row.Inn_DB)
        || !string.IsNullOrWhiteSpace(row.Kpp_DB)
        || !string.IsNullOrWhiteSpace(row.Okopf_DB)
        || !string.IsNullOrWhiteSpace(row.Okfs_DB)
        || !string.IsNullOrWhiteSpace(row.RegNo_DB);
}
