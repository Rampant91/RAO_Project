using System;
using System.Collections.Generic;
using Client_App.Helpers.MasterTitleRows;
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
        var (legal, division, warnings) = MasterTitleRowsSelector.SelectForm20(rows);
        return new FormsExcelForm20CanonicalPair(legal, division, warnings);
    }

    internal static int ScoreFullness(Form20 row) => MasterTitleRowFullness.Score(row);
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
