using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Resources.CustomComparers.SnkComparers;
using Models.Comparers.FormContent;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

internal static partial class FormPrintCompareNormalize
{
    public static bool IsYearOnlyForm(string formNum) =>
        formNum.StartsWith("2.", StringComparison.Ordinal);

    public static string NormalizePeriodPart(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmed = value.Trim().Replace('/', '.');
        return DateOnly.TryParse(trimmed, out var date)
            ? date.ToString("dd.MM.yyyy")
            : trimmed;
    }

    public static string BuildPeriodKey(string formNum, string? start, string? end, string? year)
    {
        if (formNum.StartsWith("2.", StringComparison.Ordinal))
        {
            var digits = string.Concat((year ?? string.Empty).Where(char.IsDigit));
            return digits;
        }

        return $"{NormalizePeriodPart(start)}|{NormalizePeriodPart(end)}";
    }

    public static string BuildPeriodDisplay(string formNum, string? start, string? end, string? year)
    {
        if (formNum.StartsWith("2.", StringComparison.Ordinal))
        {
            var digits = string.Concat((year ?? string.Empty).Where(char.IsDigit));
            return string.IsNullOrEmpty(digits) ? "без_года" : digits;
        }

        var s = NormalizePeriodPart(start);
        var e = NormalizePeriodPart(end);
        if (string.IsNullOrEmpty(s) && string.IsNullOrEmpty(e))
        {
            return "без_периода";
        }

        return $"{s}-{e}";
    }

    public static string NormalizeId(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "-")
        {
            return string.Empty;
        }

        if (Operation41PairingKeyComparer.IsEmptySerial(value))
        {
            return string.Empty;
        }

        var normalized = SpecialCharsRegex().Replace(value.ToLowerInvariant(), string.Empty)
            .TrimStart('0');
        return LookalikeCharMapper.ReplaceRuEnLookalikes(normalized, includeExtendedSnkSet: true);
    }

    public static string NormalizeCode(string? value) =>
        string.IsNullOrWhiteSpace(value) || value == "-"
            ? string.Empty
            : value.Trim().ToLowerInvariant();

    public static string NormalizeRads(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var normalizedSet = value.Split([',', ';'])
            .Select(x => SnkRadionuclidsEqualityComparer.SnkRegex().Replace(x, "").ToLowerInvariant())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => LookalikeCharMapper.ReplaceRuEnLookalikes(x, includeExtendedSnkSet: true))
            .OrderBy(x => x);

        return string.Join("|", normalizedSet);
    }

    public static string BuildFingerprint(CompareColumn[] columns, string[] values)
    {
        var parts = new List<string>();
        for (var i = 0; i < columns.Length && i < values.Length; i++)
        {
            if (!columns[i].InFingerprint)
            {
                continue;
            }

            parts.Add(FingerprintPart(columns[i], values[i]));
        }

        var fp = string.Join("§", parts);
        return string.IsNullOrWhiteSpace(fp.Replace("§", string.Empty)) ? string.Empty : fp;
    }

    public static string FingerprintPart(CompareColumn column, string? value) =>
        column.Kind switch
        {
            CompareColumnKind.Id => NormalizeId(value),
            CompareColumnKind.Radionuclids => NormalizeRads(value),
            CompareColumnKind.Date => NormalizePeriodPart(value),
            CompareColumnKind.Code => NormalizeCode(value),
            CompareColumnKind.Type => SoftSimilarityCore.LightNormalizeId(value ?? string.Empty),
            _ => SoftSimilarityCore.LightNormalizeId(value ?? string.Empty)
        };

    public static FieldSimilarity CompareCell(CompareColumn column, string? left, string? right)
    {
        if (column.IsRowNumber)
        {
            if (string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return FieldSimilarity.Exact;
            }

            if (int.TryParse(left, out var l) && int.TryParse(right, out var r) && l == r)
            {
                return FieldSimilarity.Exact;
            }

            // № п/п не содержательное отличие — не Near/Mismatch для статуса строки.
            return FieldSimilarity.Exact;
        }

        if (string.Equals(left ?? string.Empty, right ?? string.Empty, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        var soft = column.Kind switch
        {
            CompareColumnKind.Id => SoftSimilarityCore.SimilarityPassportOrFactory(
                left, right, isFactory: column.IsFactoryId),
            CompareColumnKind.Date => SoftSimilarityCore.SimilarityOperationDate(
                left, right, toleranceDays: 0),
            CompareColumnKind.Numeric => SoftSimilarityCore.SimilarityNumericWithTolerance(
                left ?? string.Empty, right ?? string.Empty),
            CompareColumnKind.Quantity => CompareQuantity(left, right),
            CompareColumnKind.Type => SoftSimilarityCore.SimilarityType(left, right),
            CompareColumnKind.Radionuclids => SoftSimilarityCore.SimilarityRadionuclids(
                NormalizeRads(left), NormalizeRads(right)),
            CompareColumnKind.Code => string.Equals(NormalizeCode(left), NormalizeCode(right), StringComparison.Ordinal)
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(),
            _ => SoftSimilarityCore.SimilarityTextNormalized(
                SoftSimilarityCore.LightNormalizeId(left ?? string.Empty),
                SoftSimilarityCore.LightNormalizeId(right ?? string.Empty))
        };

        return soft;
    }

    /// <summary>
    /// Текст ячейки для выгрузки: при Near/Mismatch показывает скрытые/lookalike-отличия,
    /// если глазом значения кажутся одинаковыми.
    /// </summary>
    public static string FormatCellForExport(string? value, string? other, FieldMatchLevel level)
    {
        value ??= string.Empty;
        other ??= string.Empty;
        if (level is FieldMatchLevel.Exact)
        {
            return value;
        }

        if (string.Equals(value, other, StringComparison.Ordinal))
        {
            return value;
        }

        var revealed = RevealSpecialChars(value);
        var revealedOther = RevealSpecialChars(other);
        var hadHidden = !string.Equals(revealed, value, StringComparison.Ordinal)
                        || !string.Equals(revealedOther, other, StringComparison.Ordinal);
        var stillLooksSame = string.Equals(revealed, revealedOther, StringComparison.Ordinal)
                             || string.Equals(
                                 SoftSimilarityCore.LightNormalizeId(value, mapDigitZeroToO: false),
                                 SoftSimilarityCore.LightNormalizeId(other, mapDigitZeroToO: false),
                                 StringComparison.Ordinal);

        if (!hadHidden && !stillLooksSame)
        {
            // Отличие уже видно в исходном виде (например 0124 ↔ 124).
            return value;
        }

        if (string.Equals(revealed, revealedOther, StringComparison.Ordinal))
        {
            return $"{revealed} ⟨коды: {FormatCodePoints(value)} ≠ {FormatCodePoints(other)}⟩";
        }

        return revealed;
    }

    internal static string RevealSpecialChars(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var sb = new StringBuilder(value.Length + 8);
        foreach (var ch in value)
        {
            switch (ch)
            {
                case ' ':
                    sb.Append('·');
                    break;
                case '\t':
                    sb.Append("«TAB»");
                    break;
                case '\r':
                    sb.Append("«CR»");
                    break;
                case '\n':
                    sb.Append("«LF»");
                    break;
                case '\u00A0':
                    sb.Append("«NBSP»");
                    break;
                case '\u200B':
                    sb.Append("«ZWSP»");
                    break;
                case '\u200C':
                    sb.Append("«ZWNJ»");
                    break;
                case '\u200D':
                    sb.Append("«ZWJ»");
                    break;
                case '\uFEFF':
                    sb.Append("«BOM»");
                    break;
                case '\u2013':
                    sb.Append("«– en-dash»");
                    break;
                case '\u2014':
                    sb.Append("«— em-dash»");
                    break;
                case '\u2212':
                    sb.Append("«− minus»");
                    break;
                case '\uFF0F':
                    sb.Append("«／ fullwidth»");
                    break;
                case '\u2044':
                    sb.Append("«⁄ fraction»");
                    break;
                case '\\':
                    sb.Append("«\\»");
                    break;
                case '|':
                    sb.Append("«|»");
                    break;
                default:
                    if (IsLookalikeCyrillic(ch))
                    {
                        sb.Append(ch);
                        sb.Append("⟨кир U+");
                        sb.Append(((int)ch).ToString("X4", CultureInfo.InvariantCulture));
                        sb.Append('⟩');
                    }
                    else if (ch > 127 && !char.IsLetterOrDigit(ch))
                    {
                        sb.Append(ch);
                        sb.Append("⟨U+");
                        sb.Append(((int)ch).ToString("X4", CultureInfo.InvariantCulture));
                        sb.Append('⟩');
                    }
                    else
                    {
                        sb.Append(ch);
                    }

                    break;
            }
        }

        return sb.ToString();
    }

    /// <summary>Кириллические буквы, визуально схожие с латиницей (lookalike).</summary>
    private static bool IsLookalikeCyrillic(char ch) =>
        ch is 'а' or 'А' or 'е' or 'Е' or 'о' or 'О' or 'р' or 'Р' or 'с' or 'С'
            or 'у' or 'У' or 'х' or 'Х' or 'і' or 'І' or 'ї' or 'Ї' or 'ё' or 'Ё';

    private static string FormatCodePoints(string value)
    {
        if (value.Length == 0)
        {
            return "∅";
        }

        return string.Join(' ', value.Select(ch => $"U+{(int)ch:X4}"));
    }

    private static FieldSimilarity CompareQuantity(string? left, string? right)
    {
        if (string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return FieldSimilarity.Exact;
        }

        if (int.TryParse(left, out var l) && int.TryParse(right, out var r) && l == r)
        {
            return FieldSimilarity.Exact;
        }

        return SoftSimilarityCore.SimilarityByEditDistance(
            SoftSimilarityCore.LightNormalizeId(left ?? string.Empty),
            SoftSimilarityCore.LightNormalizeId(right ?? string.Empty));
    }

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]", RegexOptions.CultureInvariant)]
    private static partial Regex SpecialCharsRegex();
}
