using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Helpers.MasterTitleRows;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Нормализация строк Form 1.0 до канонической пары: Ord=1 (юрлицо) и Ord=2 (обособленное).
/// <para>
/// Контракт UI формы 1.0: <c>Rows10[0]</c> = юрлицо, <c>Rows10[1]</c> = обособленное.
/// В живой БД у части Master бывает 4 строки <c>form_10</c> на один ReportId.
/// Конвертер не чинит исходную БД, а отбирает по одной строке на order
/// (см. <see cref="MasterTitleRowsSelector"/> / <see cref="MasterTitleRowFullness"/>).
/// </para>
/// </summary>
public static class FormsExcelForm10Pair
{
    /// <summary>
    /// Выбирает по одной исходной строке на Ord=1 и Ord=2 (без копирования).
    /// </summary>
    public static FormsExcelForm10CanonicalPair Select(IEnumerable<Form10>? rows)
    {
        var (legal, division, warnings) = MasterTitleRowsSelector.SelectForm10(rows);
        return new FormsExcelForm10CanonicalPair(legal, division, warnings);
    }

    /// <summary>Число значимых непустых полей титула. "-" считается пустым.</summary>
    internal static int ScoreFullness(Form10 row) => MasterTitleRowFullness.Score(row);
}

/// <summary>
/// Каноническая пара строк Form 1.0 (ссылки на исходные сущности, могут быть null).
/// </summary>
public readonly struct FormsExcelForm10CanonicalPair
{
    public FormsExcelForm10CanonicalPair(
        Form10? legalEntity,
        Form10? separateDivision,
        IReadOnlyList<string> warnings)
    {
        LegalEntity = legalEntity;
        SeparateDivision = separateDivision;
        Warnings = warnings ?? Array.Empty<string>();
    }

    /// <summary>Строка юрлица (NumberInOrder = 1), если была в источнике.</summary>
    public Form10? LegalEntity { get; }

    /// <summary>Строка обособленного подразделения (NumberInOrder = 2), если была.</summary>
    public Form10? SeparateDivision { get; }

    public IReadOnlyList<string> Warnings { get; }
}
