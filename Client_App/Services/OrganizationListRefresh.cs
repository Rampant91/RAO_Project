using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms.Form4;
using Models.Forms.Form5;
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

    public static bool Form40HasAnyFilledTitleFields(Report? storage)
    {
        if (storage is null)
            return false;

        foreach (var key in storage.Rows40)
        {
            if (key is not Form40 row)
                continue;
            if (Row40HasContent(row))
                return true;
        }

        return false;
    }

    public static bool Form50HasAnyFilledTitleFields(Report? storage)
    {
        if (storage is null)
            return false;

        foreach (var key in storage.Rows50)
        {
            if (key is not Form50 row)
                continue;
            if (Row50HasContent(row))
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

    private static bool Row40HasContent(Form40 row) =>
        !string.IsNullOrWhiteSpace(row.CodeSubjectRF_DB)
        || !string.IsNullOrWhiteSpace(row.SubjectRF_DB)
        || !string.IsNullOrWhiteSpace(row.NameOrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.ShortNameOrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.AddressOrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.GradeFioDirectorOrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.GradeFioExecutorOrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.TelephoneOrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.FaxOrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.EmailOrganUprav_DB)
        || !string.IsNullOrWhiteSpace(row.NameRiac_DB)
        || !string.IsNullOrWhiteSpace(row.ShortNameRiac_DB)
        || !string.IsNullOrWhiteSpace(row.AddressRiac_DB)
        || !string.IsNullOrWhiteSpace(row.GradeFioDirectorRiac_DB)
        || !string.IsNullOrWhiteSpace(row.GradeFioExecutorRiac_DB)
        || !string.IsNullOrWhiteSpace(row.TelephoneRiac_DB)
        || !string.IsNullOrWhiteSpace(row.FaxRiac_DB)
        || !string.IsNullOrWhiteSpace(row.EmailRiac_DB);

    private static bool Row50HasContent(Form50 row) =>
        !string.IsNullOrWhiteSpace(row.ExecutiveAuthority_DB)
        || row.Rosatom_DB
        || row.MinObr_DB
        || !string.IsNullOrWhiteSpace(row.Name_DB)
        || !string.IsNullOrWhiteSpace(row.ShortName_DB)
        || !string.IsNullOrWhiteSpace(row.Address_DB)
        || !string.IsNullOrWhiteSpace(row.GradeFioDirector_DB)
        || !string.IsNullOrWhiteSpace(row.GradeFioExecutor_DB)
        || !string.IsNullOrWhiteSpace(row.Telephone_DB)
        || !string.IsNullOrWhiteSpace(row.Fax_DB)
        || !string.IsNullOrWhiteSpace(row.Email_DB);
}
