using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Helpers.MasterTitleRows;

/// <summary>
/// Оценка заполненности строки титула Form 1.0 / 2.0.
/// Пустые и "-" не считаются значимыми.
/// </summary>
public static class MasterTitleRowFullness
{
    public static int Score(Form10 row) => ScoreFields(
        row.RegNo_DB,
        row.OrganUprav_DB,
        row.SubjectRF_DB,
        row.JurLico_DB,
        row.ShortJurLico_DB,
        row.JurLicoAddress_DB,
        row.JurLicoFactAddress_DB,
        row.GradeFIO_DB,
        row.Telephone_DB,
        row.Fax_DB,
        row.Email_DB,
        row.Okpo_DB,
        row.Okved_DB,
        row.Okogu_DB,
        row.Oktmo_DB,
        row.Inn_DB,
        row.Kpp_DB,
        row.Okopf_DB,
        row.Okfs_DB);

    public static int Score(Form20 row) => ScoreFields(
        row.RegNo_DB,
        row.OrganUprav_DB,
        row.SubjectRF_DB,
        row.JurLico_DB,
        row.ShortJurLico_DB,
        row.JurLicoAddress_DB,
        row.JurLicoFactAddress_DB,
        row.GradeFIO_DB,
        row.Telephone_DB,
        row.Fax_DB,
        row.Email_DB,
        row.Okpo_DB,
        row.Okved_DB,
        row.Okogu_DB,
        row.Oktmo_DB,
        row.Inn_DB,
        row.Kpp_DB,
        row.Okopf_DB,
        row.Okfs_DB);

    public static int ScoreFields(params string?[] fields)
    {
        var score = 0;
        foreach (var field in fields)
        {
            if (IsSignificant(field))
            {
                score++;
            }
        }

        return score;
    }

    public static bool IsSignificant(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return value.Trim() is not "-";
    }
}
