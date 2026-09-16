using System;
using System.Collections.Generic;
using System.Linq;
using Models.Forms.Form2;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Нормализация строк Form 2.0 до канонической пары Ord=1 / Ord=2.
/// Та же проблема дублей NumberInOrder, что у Form 1.0 (см. <see cref="FormsExcelForm10Pair"/>).
/// </summary>
public static class FormsExcelForm20Pair
{
    public static FormsExcelForm20CanonicalPair Select(IEnumerable<Form20>? rows)
    {
        var list = rows?.Where(static r => r is not null).ToList() ?? [];
        var warnings = new List<string>();
        var legal = PickBest(list, order: 1, warnings);
        var division = PickBest(list, order: 2, warnings);
        return new FormsExcelForm20CanonicalPair(legal, division, warnings);
    }

    private static Form20? PickBest(List<Form20> rows, int order, List<string> warnings)
    {
        var candidates = rows.Where(r => r.NumberInOrder_DB == order).ToList();
        if (candidates.Count == 0)
        {
            return null;
        }

        var ranked = candidates
            .Select(r => (Row: r, Score: ScoreFullness(r)))
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Row.Id)
            .ToList();

        if (ranked.Count(x => x.Score > 0) > 1)
        {
            warnings.Add(
                $"Form 2.0: несколько заполненных строк с NumberInOrder={order}, " +
                $"выбрана Id={ranked[0].Row.Id}.");
        }

        return ranked[0].Row;
    }

    internal static int ScoreFullness(Form20 row)
    {
        var fields = new[]
        {
            row.RegNo_DB, row.OrganUprav_DB, row.SubjectRF_DB, row.JurLico_DB, row.ShortJurLico_DB,
            row.JurLicoAddress_DB, row.JurLicoFactAddress_DB, row.GradeFIO_DB, row.Telephone_DB,
            row.Fax_DB, row.Email_DB, row.Okpo_DB, row.Okved_DB, row.Okogu_DB, row.Oktmo_DB,
            row.Inn_DB, row.Kpp_DB, row.Okopf_DB, row.Okfs_DB
        };

        var score = 0;
        foreach (var field in fields)
        {
            if (!string.IsNullOrWhiteSpace(field) && field.Trim() is not "-")
            {
                score++;
            }
        }

        return score;
    }
}

public readonly struct FormsExcelForm20CanonicalPair
{
    public FormsExcelForm20CanonicalPair(
        Form20? legalEntity,
        Form20? separateDivision,
        IReadOnlyList<string> warnings)
    {
        LegalEntity = legalEntity;
        SeparateDivision = separateDivision;
        Warnings = warnings ?? Array.Empty<string>();
    }

    public Form20? LegalEntity { get; }
    public Form20? SeparateDivision { get; }
    public IReadOnlyList<string> Warnings { get; }
}
