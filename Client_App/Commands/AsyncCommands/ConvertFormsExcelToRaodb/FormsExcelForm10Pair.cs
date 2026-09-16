using System;
using System.Collections.Generic;
using System.Linq;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Нормализация строк Form 1.0 до канонической пары: Ord=1 (юрлицо) и Ord=2 (обособленное).
/// <para>
/// Контракт UI формы 1.0: <c>Rows10[0]</c> = юрлицо, <c>Rows10[1]</c> = обособленное.
/// В живой БД у части Master бывает 4 строки <c>form_10</c> на один ReportId
/// (две с <c>NumberInOrder=1</c>, две с <c>NumberInOrder=2</c>) — лишняя пустая пара
/// с теми же номерами порядка, но другими Id. После QuickSort по Order две строки
/// с Ord=1 оказываются в [0] и [1], и заполненные данные юрлица показываются
/// в блоке обособленного. Источник дублей — не контракт модели (никто не закладывал
/// несколько строк на один NumberInOrder); конвертер не чинит исходную БД, а
/// отбирает по одной строке на order, чтобы не размножать мусор в .RAODB.
/// </para>
/// </summary>
public static class FormsExcelForm10Pair
{
    /// <summary>
    /// Выбирает по одной исходной строке на Ord=1 и Ord=2 (без копирования).
    /// </summary>
    public static FormsExcelForm10CanonicalPair Select(IEnumerable<Form10>? rows)
    {
        var list = rows?.Where(static r => r is not null).ToList() ?? [];
        var warnings = new List<string>();
        var legal = PickBest(list, order: 1, warnings);
        var division = PickBest(list, order: 2, warnings);
        return new FormsExcelForm10CanonicalPair(legal, division, warnings);
    }

    private static Form10? PickBest(List<Form10> rows, int order, List<string> warnings)
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

        var filledCount = ranked.Count(x => x.Score > 0);
        if (filledCount > 1)
        {
            warnings.Add(
                $"Form 1.0: несколько заполненных строк с NumberInOrder={order}, " +
                $"выбрана Id={ranked[0].Row.Id}.");
        }

        return ranked[0].Row;
    }

    /// <summary>
    /// Число значимых непустых полей титула. "-" считается пустым.
    /// </summary>
    internal static int ScoreFullness(Form10 row)
    {
        var fields = new[]
        {
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
            row.Okfs_DB
        };

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

    private static bool IsSignificant(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();
        return trimmed is not "-";
    }
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
