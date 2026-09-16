using System.Collections.Generic;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Глубокая копия Master Form 1.0 / 2.0 для записи в отдельный .RAODB (новые сущности, Id = 0).
/// Всегда ровно две строки титула: Ord=1 (юрлицо) и Ord=2 (обособленное).
/// </summary>
public static class FormsExcelMasterClone
{
    public static Report CloneForm10Master(Report sourceMaster) =>
        CloneForm10Master(sourceMaster, out _);

    public static Report CloneForm10Master(Report sourceMaster, out IReadOnlyList<string> warnings)
    {
        var pair = FormsExcelForm10Pair.Select(sourceMaster?.Rows10);
        warnings = pair.Warnings;

        var master = new Report { FormNum_DB = "1.0" };
        master.Rows10.Add(CloneOrEmpty10(pair.LegalEntity, numberInOrder: 1));
        master.Rows10.Add(CloneOrEmpty10(pair.SeparateDivision, numberInOrder: 2));
        return master;
    }

    public static Report CloneForm20Master(Report sourceMaster) =>
        CloneForm20Master(sourceMaster, out _);

    public static Report CloneForm20Master(Report sourceMaster, out IReadOnlyList<string> warnings)
    {
        var pair = FormsExcelForm20Pair.Select(sourceMaster?.Rows20);
        warnings = pair.Warnings;

        var master = new Report { FormNum_DB = "2.0" };
        master.Rows20.Add(CloneOrEmpty20(pair.LegalEntity, numberInOrder: 1));
        master.Rows20.Add(CloneOrEmpty20(pair.SeparateDivision, numberInOrder: 2));
        return master;
    }

    private static Form10 CloneOrEmpty10(Form10? source, int numberInOrder)
    {
        var copy = (Form10)FormCreator.Create("1.0");
        if (source is not null)
        {
            CopyForm10Fields(source, copy);
        }

        copy.NumberInOrder_DB = numberInOrder;
        return copy;
    }

    private static Form20 CloneOrEmpty20(Form20? source, int numberInOrder)
    {
        var copy = (Form20)FormCreator.Create("2.0");
        if (source is not null)
        {
            CopyForm20Fields(source, copy);
        }

        copy.NumberInOrder_DB = numberInOrder;
        return copy;
    }

    public static void CopyForm10Fields(Form10 source, Form10 target)
    {
        target.NumberInOrder_DB = source.NumberInOrder_DB;
        target.RegNo_DB = source.RegNo_DB;
        target.OrganUprav_DB = source.OrganUprav_DB;
        target.SubjectRF_DB = source.SubjectRF_DB;
        target.JurLico_DB = source.JurLico_DB;
        target.ShortJurLico_DB = source.ShortJurLico_DB;
        target.JurLicoAddress_DB = source.JurLicoAddress_DB;
        target.JurLicoFactAddress_DB = source.JurLicoFactAddress_DB;
        target.GradeFIO_DB = source.GradeFIO_DB;
        target.Telephone_DB = source.Telephone_DB;
        target.Fax_DB = source.Fax_DB;
        target.Email_DB = source.Email_DB;
        target.Okpo_DB = source.Okpo_DB;
        target.Okved_DB = source.Okved_DB;
        target.Okogu_DB = source.Okogu_DB;
        target.Oktmo_DB = source.Oktmo_DB;
        target.Inn_DB = source.Inn_DB;
        target.Kpp_DB = source.Kpp_DB;
        target.Okopf_DB = source.Okopf_DB;
        target.Okfs_DB = source.Okfs_DB;
    }

    public static void CopyForm20Fields(Form20 source, Form20 target)
    {
        target.NumberInOrder_DB = source.NumberInOrder_DB;
        target.RegNo_DB = source.RegNo_DB;
        target.OrganUprav_DB = source.OrganUprav_DB;
        target.SubjectRF_DB = source.SubjectRF_DB;
        target.JurLico_DB = source.JurLico_DB;
        target.ShortJurLico_DB = source.ShortJurLico_DB;
        target.JurLicoAddress_DB = source.JurLicoAddress_DB;
        target.JurLicoFactAddress_DB = source.JurLicoFactAddress_DB;
        target.GradeFIO_DB = source.GradeFIO_DB;
        target.Telephone_DB = source.Telephone_DB;
        target.Fax_DB = source.Fax_DB;
        target.Email_DB = source.Email_DB;
        target.Okpo_DB = source.Okpo_DB;
        target.Okved_DB = source.Okved_DB;
        target.Okogu_DB = source.Okogu_DB;
        target.Oktmo_DB = source.Oktmo_DB;
        target.Inn_DB = source.Inn_DB;
        target.Kpp_DB = source.Kpp_DB;
        target.Okopf_DB = source.Okopf_DB;
        target.Okfs_DB = source.Okfs_DB;
    }
}
