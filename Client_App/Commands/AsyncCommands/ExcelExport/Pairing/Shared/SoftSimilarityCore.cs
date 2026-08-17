using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Client_App.Resources.CustomComparers.SnkComparers;
using Models.Comparers.FormContent;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;

/// <summary>
/// Общие алгоритмы soft-similarity для closest-match (паспорт, зав.№, тип, активность и т.д.).
/// </summary>
public static partial class SoftSimilarityCore
{
    public static bool TryParseNumeric(string? value, out double result) =>
        FormExponentialEquality.TryParse(value, out result);

    /// <summary>
    /// Субсидия: прочерк/пусто/служебные маркеры или 0 %.
    /// </summary>
    public static bool IsSubsidyAbsentOrZero(string? raw)
    {
        if (Operation41PairingKeyComparer.IsEmptySerial(raw))
        {
            return true;
        }

        var trimmed = (raw ?? string.Empty).Trim();
        return TryParseSubsidyPercent(trimmed, out var value) && value == 0;
    }

    /// <summary>
    /// Числовое поле с экспоненциальным парсингом: ±10% или эквивалентность нуля и прочерка.
    /// </summary>
    public static bool NumericMatchesWithTolerance(
        string? leftRaw,
        string? rightRaw,
        double relativeTolerance = 0.10,
        Func<string?, string?, bool>? unparsableEquals = null)
    {
        if (FormExponentialEquality.IsAbsentOrZero(leftRaw)
            && FormExponentialEquality.IsAbsentOrZero(rightRaw))
        {
            return true;
        }

        if (!TryParseNumeric(leftRaw, out var leftValue) || !TryParseNumeric(rightRaw, out var rightValue))
        {
            return unparsableEquals?.Invoke(leftRaw, rightRaw)
                ?? string.Equals(leftRaw ?? string.Empty, rightRaw ?? string.Empty, StringComparison.Ordinal);
        }

        var scale = Math.Max(Math.Abs(leftValue), Math.Abs(rightValue));
        if (scale <= double.Epsilon)
        {
            return true;
        }

        return Math.Abs(leftValue - rightValue) <= scale * relativeTolerance;
    }

    /// <summary>
    /// Субсидия, %: 0 ↔ прочерк/пусто; иначе точное совпадение или |Δ| ≤ 10.
    /// </summary>
    public static bool SubsidyMatches(string? leftRaw, string? rightRaw)
    {
        if (IsSubsidyAbsentOrZero(leftRaw) && IsSubsidyAbsentOrZero(rightRaw))
        {
            return true;
        }

        var left = (leftRaw ?? string.Empty).Trim();
        var right = (rightRaw ?? string.Empty).Trim();
        if (string.Equals(left, right, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (TryParseSubsidyPercent(left, out var leftValue) && TryParseSubsidyPercent(right, out var rightValue))
        {
            return Math.Abs(leftValue - rightValue) <= 10;
        }

        return false;
    }

    public static bool DatesEqualExact(string? left, string? right)
    {
        if (DateOnly.TryParse(left, out var leftDate) && DateOnly.TryParse(right, out var rightDate))
        {
            return leftDate == rightDate;
        }

        return string.Equals(left?.Trim(), right?.Trim(), StringComparison.Ordinal);
    }

    public static FieldSimilarity SimilarityOperationDate(
        string? left,
        string? right,
        int toleranceDays,
        Func<string?, string>? normalizeUnparseable = null)
    {
        if (DatesEqualExact(left, right))
        {
            return FieldSimilarity.Exact;
        }

        if (!DateOnly.TryParse(left, out var leftDate) || !DateOnly.TryParse(right, out var rightDate))
        {
            if (normalizeUnparseable is not null)
            {
                return string.Equals(normalizeUnparseable(left), normalizeUnparseable(right), StringComparison.Ordinal)
                    ? FieldSimilarity.Exact
                    : FieldSimilarity.Mismatch(0.1);
            }

            return string.Equals(NormalizeDigitsOnly(left), NormalizeDigitsOnly(right), StringComparison.Ordinal)
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0.1);
        }

        var delta = Math.Abs(leftDate.DayNumber - rightDate.DayNumber);
        if (delta > toleranceDays)
        {
            return FieldSimilarity.Mismatch(0);
        }

        var score = 0.92 - (delta - 1) * (0.42 / Math.Max(1, toleranceDays - 1));
        return FieldSimilarity.Near(score);
    }

    /// <summary>
    /// Near для перечисления зав.№ (диапазон или список чисел)
    /// при qty = числу раскрытых номеров ↔ одиночный номер из набора (qty 1).
    /// Жёлтый, но высокий кэф — с высокой вероятностью та же партия.
    /// Тот же кэф — когда активность списка совпадает с ожидаемой суммой вкладов (при равных
    /// активностях элементов это unit×N).
    /// </summary>
    public const double FactoryNumericRangeNearScore = 0.92;

    /// <summary>
    /// Активность списка ≈ ожидаемая сумма (unit×N) в допуске ±10% — Near чуть ниже 0.92.
    /// </summary>
    public const double FactoryEnumerationActivityNearScoreWithinTolerance = 0.85;

    /// <summary>
    /// Активность одиночного элемента строго меньше суммарной у списка (правдоподобный вклад в сумму),
    /// но unit×N не бьётся — Near слабее (разнородные вклады по одной строке не восстановить).
    /// </summary>
    public const double FactoryEnumerationActivityNearScorePlausiblePart = 0.70;

    public static FieldSimilarity SimilarityPassportOrFactory(
        string? leftRaw,
        string? rightRaw,
        bool isFactory,
        int leftQuantity = 1,
        int rightQuantity = 1)
    {
        var leftEmpty = Operation41PairingKeyComparer.IsEmptySerial(leftRaw);
        var rightEmpty = Operation41PairingKeyComparer.IsEmptySerial(rightRaw);
        if (leftEmpty && rightEmpty)
        {
            return FieldSimilarity.Exact;
        }

        if (leftEmpty || rightEmpty)
        {
            return FieldSimilarity.Mismatch(0.05);
        }

        if (isFactory)
        {
            var leftForPrefix = LightNormalizeId(leftRaw!, mapDigitZeroToO: false);
            var rightForPrefix = LightNormalizeId(rightRaw!, mapDigitZeroToO: false);
            if (TryFactoryCoreMatch(leftForPrefix, rightForPrefix, out var factoryScore))
            {
                return factoryScore;
            }

            // Перечисление зав.№ (диапазон / список) + qty ↔ одиночный номер (qty 1); не для паспорта.
            if (TryFactoryNumericRangeAlias(leftRaw, rightRaw, leftQuantity, rightQuantity))
            {
                return FieldSimilarity.Near(FactoryNumericRangeNearScore);
            }
        }
        else if (MatchesPassportActNumberAlias(leftRaw, rightRaw))
        {
            // Акт с номером ↔ другой акт / голый номер того же акта — только паспорт.
            return FieldSimilarity.Near(0.95);
        }

        var left = LightNormalizeId(leftRaw!, mapDigitZeroToO: true);
        var right = LightNormalizeId(rightRaw!, mapDigitZeroToO: true);
        if (left.Length == 0 || right.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.05);
        }

        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        // Ведущие нули в числовых фрагментах — почти совпадение.
        // Сравниваем без mapDigitZeroToO, иначе нули станут «о» и потеряют смысл.
        var leftDigitsKeep = LightNormalizeId(leftRaw!, mapDigitZeroToO: false);
        var rightDigitsKeep = LightNormalizeId(rightRaw!, mapDigitZeroToO: false);
        if (EqualIgnoringLeadingZerosInDigitRuns(leftDigitsKeep, rightDigitsKeep))
        {
            return FieldSimilarity.Near(0.97);
        }

        // Суффикс «.XX» и/или ведущие нули (по отдельности или вместе).
        if (TryOptionalDotTwoDigitSerialNear(leftDigitsKeep, rightDigitsKeep, out var dotScore))
        {
            return FieldSimilarity.Near(dotScore);
        }

        // Тире в номере часто не пишут.
        if (EqualIgnoringHyphens(left, right)
            || EqualIgnoringHyphens(leftDigitsKeep, rightDigitsKeep)
            || EqualIgnoringHyphens(
                StripLeadingZerosInDigitRuns(leftDigitsKeep),
                StripLeadingZerosInDigitRuns(rightDigitsKeep)))
        {
            return FieldSimilarity.Near(0.96);
        }

        // Перестановка двух соседних символов — типичная опечатка, не «почти точное» совпадение.
        var leftZerosStripped = StripLeadingZerosInDigitRuns(leftDigitsKeep);
        var rightZerosStripped = StripLeadingZerosInDigitRuns(rightDigitsKeep);
        if (IsAdjacentCharacterTransposition(left, right)
            || IsAdjacentCharacterTransposition(leftDigitsKeep, rightDigitsKeep))
        {
            return FieldSimilarity.Near(0.78);
        }

        if (IsAdjacentCharacterTransposition(leftZerosStripped, rightZerosStripped))
        {
            // Ведущие нули + перестановка — всё ещё Near, кэф ниже.
            return FieldSimilarity.Near(CombinedSoftNearScore);
        }

        // К номеру дописана дата MM.yyyy (через пробел/спецсимвол).
        // Сравниваем без mapDigitZeroToO — иначе нули в дате станут «о».
        if (MatchesTrailingMonthYearAlias(leftDigitsKeep, rightDigitsKeep))
        {
            return FieldSimilarity.Near(0.70);
        }

        if (MatchesTrailingMonthYearAlias(leftZerosStripped, rightZerosStripped))
        {
            return FieldSimilarity.Near(CombinedSoftNearScore);
        }

        // В конце дописали метку №/No/номер и цифры.
        if (MatchesTrailingSerialLabelAlias(leftDigitsKeep, rightDigitsKeep))
        {
            return FieldSimilarity.Near(0.72);
        }

        if (MatchesTrailingSerialLabelAlias(leftZerosStripped, rightZerosStripped))
        {
            return FieldSimilarity.Near(CombinedSoftNearScore);
        }

        if (TryYearSuffixVariantMatch(left, right, out var yearScore))
        {
            return yearScore;
        }

        if (TryYearSuffixVariantMatch(leftZerosStripped, rightZerosStripped, out yearScore)
            && yearScore.Level == FieldMatchLevel.Near)
        {
            return FieldSimilarity.Near(Math.Min(yearScore.Score, CombinedSoftNearScore));
        }

        return SimilarityByEditDistance(left, right);
    }

    /// <summary>
    /// Количество: точное равенство или Near при перечислении зав.№ (1 ↔ число раскрытых номеров).
    /// </summary>
    public static FieldSimilarity SimilarityQuantityConsideringFactoryRange(
        int leftQuantity,
        int rightQuantity,
        string? leftFactoryRaw,
        string? rightFactoryRaw)
    {
        if (leftQuantity == rightQuantity)
        {
            return FieldSimilarity.Exact;
        }

        if (TryFactoryNumericRangeAlias(leftFactoryRaw, rightFactoryRaw, leftQuantity, rightQuantity))
        {
            return FieldSimilarity.Near(FactoryNumericRangeNearScore);
        }

        return FieldSimilarity.Mismatch(0);
    }

    /// <summary>
    /// Активность при перечислении зав.№: на стороне списка — сумма активностей элементов.
    /// В построчном closest видна одна одиночная строка:
    /// <list type="bullet">
    /// <item>список ≈ unit×N (равные вклады) — Near 0.92;</item>
    /// <item>то же в ±10% — Near 0.85;</item>
    /// <item>unit &lt; сумма списка (правдоподобный вклад, сумма разнородна) — Near 0.70;</item>
    /// <item>unit ≥ сумма при N&gt;1 — не специальный Near (не может быть частью суммы).</item>
    /// </list>
    /// </summary>
    public static FieldSimilarity SimilarityActivityConsideringFactoryEnumeration(
        string? leftActivity,
        string? rightActivity,
        string? leftFactoryRaw,
        string? rightFactoryRaw,
        int leftQuantity,
        int rightQuantity)
    {
        if (TryGetFactoryEnumerationMatch(
                leftFactoryRaw,
                rightFactoryRaw,
                leftQuantity,
                rightQuantity,
                out var enumerationQuantity,
                out var leftIsEnumeration)
            && TryParseNumeric(leftActivity, out var leftValue)
            && TryParseNumeric(rightActivity, out var rightValue))
        {
            var enumerationActivity = leftIsEnumeration ? leftValue : rightValue;
            var singleActivity = leftIsEnumeration ? rightValue : leftValue;
            // Ожидаемая сумма при равных вкладах каждого элемента (= сумма N одинаковых).
            var expectedSum = singleActivity * enumerationQuantity;

            var scale = Math.Max(Math.Abs(enumerationActivity), Math.Abs(expectedSum));
            if (scale <= double.Epsilon)
            {
                return FieldSimilarity.Near(FactoryNumericRangeNearScore);
            }

            var rel = Math.Abs(enumerationActivity - expectedSum) / scale;
            if (rel <= 1e-12)
            {
                return FieldSimilarity.Near(FactoryNumericRangeNearScore);
            }

            if (rel <= 0.10)
            {
                return FieldSimilarity.Near(FactoryEnumerationActivityNearScoreWithinTolerance);
            }

            // Разнородные вклады: по одной строке сумму не собрать, но элемент должен быть
            // строго меньше суммарной активности группы.
            if (enumerationQuantity > 1
                && singleActivity > 0
                && enumerationActivity > 0
                && singleActivity < enumerationActivity)
            {
                return FieldSimilarity.Near(FactoryEnumerationActivityNearScorePlausiblePart);
            }
        }

        return SimilarityNumericWithTolerance(
            leftActivity ?? string.Empty,
            rightActivity ?? string.Empty);
    }

    public static FieldSimilarity SimilarityType(string? leftRaw, string? rightRaw)
    {
        var left = LightNormalizeId(leftRaw ?? string.Empty, mapDigitZeroToO: true);
        var right = LightNormalizeId(rightRaw ?? string.Empty, mapDigitZeroToO: true);
        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        if (left.Length == 0 || right.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.1);
        }

        // Опциональный хвост «-N» / «-N.NN» снимаем без mapDigitZeroToO: иначе ноль в числе станет «о» и \d+ не матчит.
        var leftKeep = LightNormalizeId(leftRaw ?? string.Empty, mapDigitZeroToO: false);
        var rightKeep = LightNormalizeId(rightRaw ?? string.Empty, mapDigitZeroToO: false);

        // Та же строка плюс «.XX» (две цифры) или «.S» / «.SS» (короткий буквенно-цифровой хвост).
        if (MatchesOptionalDotTwoDigitSuffix(leftKeep, rightKeep)
            || MatchesOptionalDotTwoDigitSuffix(
                leftKeep.Replace('0', 'o'),
                rightKeep.Replace('0', 'o'))
            || MatchesOptionalDotShortSuffix(leftKeep, rightKeep)
            || MatchesOptionalDotShortSuffix(left, right))
        {
            return FieldSimilarity.Near(TypeOptionalDotTwoDigitNearScore);
        }

        var leftCoreKeep = StripTypeOptionalTail(leftKeep);
        var rightCoreKeep = StripTypeOptionalTail(rightKeep);
        if (TypeOptionalTailCoresMatch(leftKeep, rightKeep, leftCoreKeep, rightCoreKeep))
        {
            return FieldSimilarity.Near(0.86);
        }

        // Lookalike-путь (0≈о) для уже снятого хвоста.
        var leftCore = StripTypeOptionalTail(left);
        var rightCore = StripTypeOptionalTail(right);
        // Только «короткий ↔ короткий+хвост», не разные номера с общим префиксом после срезания.
        if (string.Equals(leftCore, right, StringComparison.Ordinal)
            || string.Equals(rightCore, left, StringComparison.Ordinal))
        {
            return FieldSimilarity.Near(0.86);
        }

        // Перестановка двух соседних символов в любом месте — типичная опечатка, не «сильное» отличие.
        if (IsAdjacentCharacterTransposition(left, right)
            || IsAdjacentCharacterTransposition(leftCore, rightCore)
            || IsAdjacentCharacterTransposition(leftCore, right)
            || IsAdjacentCharacterTransposition(left, rightCore))
        {
            return FieldSimilarity.Near(0.78);
        }

        // В одном значении текст в скобках, в другом — только содержимое скобок.
        if (MatchesParentheticalAlias(left, right)
            || MatchesParentheticalAlias(leftCore, right)
            || MatchesParentheticalAlias(left, rightCore)
            || MatchesParentheticalAlias(leftCore, rightCore))
        {
            return FieldSimilarity.Near(0.70);
        }

        // Длинная база + короткий буквенный хвост через «-»/«.»/пробел (например модификатор в конце).
        if (MatchesTypeOptionalLetterSuffix(leftKeep, rightKeep)
            || MatchesTypeOptionalLetterSuffix(left, right)
            || MatchesTypeOptionalLetterSuffix(
                LightNormalizeTypeKeepingSeparators(leftRaw ?? string.Empty, mapDigitZeroToO: false),
                LightNormalizeTypeKeepingSeparators(rightRaw ?? string.Empty, mapDigitZeroToO: false))
            || MatchesTypeOptionalLetterSuffix(
                LightNormalizeTypeKeepingSeparators(leftRaw ?? string.Empty, mapDigitZeroToO: true),
                LightNormalizeTypeKeepingSeparators(rightRaw ?? string.Empty, mapDigitZeroToO: true)))
        {
            return FieldSimilarity.Near(TypeOptionalLetterSuffixNearScore);
        }

        // Тире часто ставят или пропускают (пробел уже снят light-normalize).
        if (EqualIgnoringHyphens(left, right)
            || EqualIgnoringHyphens(leftCore, rightCore)
            || EqualIgnoringHyphens(leftCore, right)
            || EqualIgnoringHyphens(left, rightCore))
        {
            return FieldSimilarity.Near(0.96);
        }

        // Дальше сравниваем без тире: иначе «пробел↔тире» + одна опечатка дают distance≥2 и ложный Mismatch.
        // Короткие строки (1↔I и т.п.) по-прежнему режет SimilarityByEditDistance (Near только при length>2).
        var leftBare = StripHyphens(left);
        var rightBare = StripHyphens(right);
        if (leftBare.Length > 0 && rightBare.Length > 0
            && (leftBare.Length != left.Length || rightBare.Length != right.Length))
        {
            return SimilarityByEditDistance(leftBare, rightBare);
        }

        return SimilarityByEditDistance(left, right);
    }

    /// <summary>
    /// Одна сторона — другая плюс ровно две цифры после точки.
    /// Не гарантированное совпадение → умеренный Near (~0.7).
    /// </summary>
    public const double OptionalDotTwoDigitNearScore = 0.70;

    /// <summary>Два мягких отличия сразу (ведущие нули + «.XX» и т.п.) — Near ниже, но не Mismatch.</summary>
    public const double CombinedSoftNearScore = 0.65;

    /// <summary>Устаревший алиас для тестов типа; то же, что <see cref="OptionalDotTwoDigitNearScore"/>.</summary>
    public const double TypeOptionalDotTwoDigitNearScore = OptionalDotTwoDigitNearScore;

    /// <summary>
    /// Тип: длинная база + короткий буквенный хвост через разделитель — Near ниже 0.7.
    /// </summary>
    public const double TypeOptionalLetterSuffixNearScore = 0.65;

    /// <summary>Минимальная длина базы типа для смягчения буквенного хвоста (строго больше 5).</summary>
    public const int TypeOptionalLetterSuffixMinCoreLength = 6;

    /// <summary>
    /// True, если одна строка = другая + «-»/«.» + короткий буквенный хвост (1–4 знака, с буквы),
    /// база длиннее 5 символов; тире в базе игнорируются.
    /// </summary>
    public static bool MatchesTypeOptionalLetterSuffix(string left, string right)
    {
        if (left.Length == 0 || right.Length == 0 || left.Length == right.Length)
        {
            return false;
        }

        if (TryStripTypeOptionalLetterSuffix(left, out var leftCore)
            && TypeLetterSuffixCoreMatches(leftCore, right))
        {
            return true;
        }

        if (TryStripTypeOptionalLetterSuffix(right, out var rightCore)
            && TypeLetterSuffixCoreMatches(rightCore, left))
        {
            return true;
        }

        return false;
    }

    public static bool TryStripTypeOptionalLetterSuffix(string value, out string core)
    {
        core = value;
        var match = TypeOptionalLetterSuffixRegex().Match(value);
        if (!match.Success)
        {
            return false;
        }

        core = match.Groups[1].Value;
        return core.Length >= TypeOptionalLetterSuffixMinCoreLength;
    }

    private static bool TypeLetterSuffixCoreMatches(string core, string other) =>
        other.Length >= TypeOptionalLetterSuffixMinCoreLength
        && (string.Equals(core, other, StringComparison.Ordinal)
            || EqualIgnoringHyphens(core, other)
            || EqualIgnoringTypeSeparators(core, other));

    /// <summary>
    /// Совпадение без тире и точек (в типе «02-000» и «02.000» — одна запись).
    /// </summary>
    public static bool EqualIgnoringTypeSeparators(string left, string right)
    {
        var leftBare = StripTypeSeparators(left);
        var rightBare = StripTypeSeparators(right);
        if (leftBare.Length == 0 || rightBare.Length == 0)
        {
            return false;
        }

        return string.Equals(leftBare, rightBare, StringComparison.Ordinal);
    }

    private static string StripTypeSeparators(string value) =>
        StripHyphens(value).Replace(".", string.Empty, StringComparison.Ordinal);

    /// <summary>
    /// Как <see cref="LightNormalizeId"/>, но пробелы → «-», чтобы сохранить разделитель перед хвостом.
    /// </summary>
    public static string LightNormalizeTypeKeepingSeparators(string value, bool mapDigitZeroToO = false)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return LightNormalizeId(
            value.Replace(' ', '-'),
            mapDigitZeroToO);
    }

    /// <summary>
    /// True, если одна строка = другая + «.XX» (ровно две цифры).
    /// </summary>
    public static bool MatchesOptionalDotTwoDigitSuffix(string left, string right)
    {
        if (left.Length == 0 || right.Length == 0 || left.Length == right.Length)
        {
            return false;
        }

        if (left.Length > right.Length)
        {
            return left.StartsWith(right, StringComparison.Ordinal)
                && TypeOptionalDotTwoDigitSuffixRegex().IsMatch(left[right.Length..]);
        }

        return right.StartsWith(left, StringComparison.Ordinal)
            && TypeOptionalDotTwoDigitSuffixRegex().IsMatch(right[left.Length..]);
    }

    /// <summary>
    /// Тип: одна строка = другая + точка и 1–2 буквы/цифры (модификатор в конце).
    /// Короткие ядра (&lt; 3) не смягчаем — слишком легко ложное совпадение.
    /// </summary>
    public static bool MatchesOptionalDotShortSuffix(string left, string right)
    {
        if (left.Length == 0 || right.Length == 0 || left.Length == right.Length)
        {
            return false;
        }

        string longer;
        string shorter;
        if (left.Length > right.Length)
        {
            longer = left;
            shorter = right;
        }
        else
        {
            longer = right;
            shorter = left;
        }

        if (shorter.Length < 3
            || !longer.StartsWith(shorter, StringComparison.Ordinal))
        {
            return false;
        }

        return TypeOptionalDotShortSuffixRegex().IsMatch(longer[shorter.Length..]);
    }

    /// <summary>
    /// Снимает хвостовое «.XX» (ровно две цифры). False, если хвоста нет.
    /// </summary>
    public static bool TryStripOptionalDotTwoDigitSuffix(string value, out string core)
    {
        core = value;
        if (value.Length < 4
            || !TypeOptionalDotTwoDigitSuffixRegex().IsMatch(value[^3..]))
        {
            return false;
        }

        core = value[..^3];
        return core.Length > 0;
    }

    /// <summary>
    /// «.XX» у паспорта/зав.№/УКТ; при необходимости с ведущими нулями и без тире.
    /// </summary>
    public static bool TryOptionalDotTwoDigitSerialNear(string left, string right, out double score)
    {
        if (TryOptionalDotTwoDigitSerialNearCore(left, right, out score))
        {
            return true;
        }

        var leftBare = StripHyphens(left);
        var rightBare = StripHyphens(right);
        return (leftBare != left || rightBare != right)
            && TryOptionalDotTwoDigitSerialNearCore(leftBare, rightBare, out score);
    }

    private static bool TryOptionalDotTwoDigitSerialNearCore(string left, string right, out double score)
    {
        score = 0;
        if (MatchesOptionalDotTwoDigitSuffix(left, right))
        {
            score = OptionalDotTwoDigitNearScore;
            return true;
        }

        if (TryStripOptionalDotTwoDigitSuffix(left, out var leftCore)
            && EqualIgnoringLeadingZerosInDigitRuns(leftCore, right))
        {
            score = CombinedSoftNearScore;
            return true;
        }

        if (TryStripOptionalDotTwoDigitSuffix(right, out var rightCore)
            && EqualIgnoringLeadingZerosInDigitRuns(rightCore, left))
        {
            score = CombinedSoftNearScore;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Ядра после снятия «-N» / «-N.N»: только префикс к короткой форме, с 0→o.
    /// Совпадение двух срезанных ядер само по себе не считается (иначе ложный Near у разных номеров).
    /// </summary>
    private static bool TypeOptionalTailCoresMatch(
        string leftKeep,
        string rightKeep,
        string leftCoreKeep,
        string rightCoreKeep)
    {
        if (string.Equals(leftCoreKeep, rightKeep, StringComparison.Ordinal)
            || string.Equals(rightCoreKeep, leftKeep, StringComparison.Ordinal))
        {
            return true;
        }

        var leftCoreO = leftCoreKeep.Replace('0', 'o');
        var rightCoreO = rightCoreKeep.Replace('0', 'o');
        var leftO = leftKeep.Replace('0', 'o');
        var rightO = rightKeep.Replace('0', 'o');
        return string.Equals(leftCoreO, rightO, StringComparison.Ordinal)
            || string.Equals(rightCoreO, leftO, StringComparison.Ordinal);
    }

    /// <summary>
    /// True, если одна строка равна содержимому скобок другой.
    /// </summary>
    public static bool MatchesParentheticalAlias(string left, string right)
    {
        if (left.Length == 0 || right.Length == 0)
        {
            return false;
        }

        if (TryExtractLastParentheticalContent(left, out var leftInner)
            && string.Equals(leftInner, right, StringComparison.Ordinal))
        {
            return true;
        }

        if (TryExtractLastParentheticalContent(right, out var rightInner)
            && string.Equals(rightInner, left, StringComparison.Ordinal))
        {
            return true;
        }

        return false;
    }

    /// <summary>Содержимое последней пары скобок (после light-normalize пробелы уже сняты).</summary>
    public static bool TryExtractLastParentheticalContent(string value, out string content)
    {
        content = string.Empty;
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        Match? last = null;
        foreach (Match match in ParentheticalContentRegex().Matches(value))
        {
            last = match;
        }

        if (last is null || !last.Success)
        {
            return false;
        }

        content = last.Groups[1].Value;
        return content.Length > 0;
    }

    /// <summary>
    /// True, если строки одинаковой длины и отличаются только обменом двух соседних символов.
    /// </summary>
    public static bool IsAdjacentCharacterTransposition(string left, string right)
    {
        if (left.Length != right.Length || left.Length < 2)
        {
            return false;
        }

        var firstDiff = -1;
        for (var i = 0; i < left.Length; i++)
        {
            if (left[i] == right[i])
            {
                continue;
            }

            if (firstDiff < 0)
            {
                firstDiff = i;
                continue;
            }

            if (i != firstDiff + 1
                || left[firstDiff] != right[i]
                || left[i] != right[firstDiff])
            {
                return false;
            }

            for (var j = i + 1; j < left.Length; j++)
            {
                if (left[j] != right[j])
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    public static FieldSimilarity SimilarityRadionuclids(string leftNormJoined, string rightNormJoined)
    {
        if (string.Equals(leftNormJoined, rightNormJoined, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        var leftSet = leftNormJoined.Split('|', StringSplitOptions.RemoveEmptyEntries).ToHashSet(StringComparer.Ordinal);
        var rightSet = rightNormJoined.Split('|', StringSplitOptions.RemoveEmptyEntries).ToHashSet(StringComparer.Ordinal);
        if (leftSet.Count == 0 || rightSet.Count == 0)
        {
            return FieldSimilarity.Mismatch(0.1);
        }

        var intersection = leftSet.Intersect(rightSet).Count();
        if (intersection == leftSet.Count && intersection == rightSet.Count)
        {
            return FieldSimilarity.Exact;
        }

        if (leftSet.Count == rightSet.Count)
        {
            var bestToken = BestTokenEditSimilarity(leftSet, rightSet);
            if (bestToken >= 0.8)
            {
                return FieldSimilarity.Near(0.55 + 0.3 * bestToken);
            }

            return FieldSimilarity.Mismatch(0.15 * bestToken);
        }

        var union = leftSet.Union(rightSet).Count();
        var jaccard = union == 0 ? 0 : intersection / (double)union;
        return FieldSimilarity.Mismatch(0.2 * jaccard);
    }

    public static FieldSimilarity SimilarityNumericWithTolerance(
        string left,
        string right,
        Func<string, string>? normalize = null)
    {
        if (FormExponentialEquality.IsAbsentOrZero(left) && FormExponentialEquality.IsAbsentOrZero(right))
        {
            return FieldSimilarity.Exact;
        }

        var leftValue = normalize is null ? left : normalize(left);
        var rightValue = normalize is null ? right : normalize(right);

        if (FormExponentialEquality.IsAbsentOrZero(leftValue) && FormExponentialEquality.IsAbsentOrZero(rightValue))
        {
            return FieldSimilarity.Exact;
        }

        if (!TryParseNumeric(leftValue, out var la) || !TryParseNumeric(rightValue, out var ra))
        {
            return string.Equals(leftValue, rightValue, StringComparison.Ordinal)
                ? FieldSimilarity.Exact
                : SimilarityByEditDistance(LightNormalizeId(leftValue), LightNormalizeId(rightValue));
        }

        var scale = Math.Max(Math.Abs(la), Math.Abs(ra));
        if (scale <= double.Epsilon)
        {
            return FieldSimilarity.Exact;
        }

        var rel = Math.Abs(la - ra) / scale;
        if (rel <= 0.10)
        {
            return FieldSimilarity.Exact;
        }

        var ratio = Math.Max(Math.Abs(la), 1e-300) / Math.Max(Math.Abs(ra), 1e-300);
        var log10 = Math.Abs(Math.Log10(ratio));
        if (log10 is >= 0.85 and <= 1.15)
        {
            return FieldSimilarity.Near(0.62);
        }

        if (log10 is >= 2.85 and <= 3.15)
        {
            return FieldSimilarity.Near(0.68);
        }

        if (rel <= 0.35)
        {
            return FieldSimilarity.Near(0.7 - rel);
        }

        return FieldSimilarity.Mismatch(Math.Max(0, 0.25 - rel));
    }

    public static FieldSimilarity SimilarityTextNormalized(string leftNorm, string rightNorm)
    {
        if (string.Equals(leftNorm, rightNorm, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        if (leftNorm.Length == 0 || rightNorm.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.1);
        }

        return SimilarityByEditDistance(leftNorm, rightNorm);
    }

    /// <summary>
    /// Календарная дата (выпуск, документ, измерение активности и т.п.): Exact при совпадении,
    /// Near в окне ±<see cref="CalendarDateToleranceDays"/> с понижением кэфа по |Δ дней|,
    /// иначе Mismatch (та же логика, что у даты операции).
    /// </summary>
    public const int CalendarDateToleranceDays = 15;

    public static FieldSimilarity SimilarityCalendarDate(string? leftRaw, string? rightRaw) =>
        SimilarityOperationDate(leftRaw, rightRaw, CalendarDateToleranceDays);

    public static FieldSimilarity SimilarityPackNumber(string? leftRaw, string? rightRaw)
    {
        var leftEmpty = Operation41PairingKeyComparer.IsEmptySerial(leftRaw);
        var rightEmpty = Operation41PairingKeyComparer.IsEmptySerial(rightRaw);
        if (leftEmpty && rightEmpty)
        {
            return FieldSimilarity.Exact;
        }

        if (leftEmpty || rightEmpty)
        {
            return FieldSimilarity.Mismatch(0.05);
        }

        var left = LightNormalizePack(leftRaw);
        var right = LightNormalizePack(rightRaw);
        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        if (left.Length == 0 || right.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.15);
        }

        // Ведущие нули — почти совпадение (как у паспорта / зав.№).
        if (EqualIgnoringLeadingZerosInDigitRuns(left, right))
        {
            return FieldSimilarity.Near(0.97);
        }

        if (TryOptionalDotTwoDigitSerialNear(left, right, out var packDotScore))
        {
            return FieldSimilarity.Near(packDotScore);
        }

        if (EqualIgnoringHyphens(left, right)
            || EqualIgnoringHyphens(
                StripLeadingZerosInDigitRuns(left),
                StripLeadingZerosInDigitRuns(right)))
        {
            return FieldSimilarity.Near(0.96);
        }

        // Перестановка двух соседних символов — опечатка (как у типа / паспорта / зав.№).
        if (IsAdjacentCharacterTransposition(left, right))
        {
            return FieldSimilarity.Near(0.78);
        }

        // Дописанная дата MM.yyyy.
        if (MatchesTrailingMonthYearAlias(left, right))
        {
            return FieldSimilarity.Near(0.70);
        }

        var leftParts = ParsePackParts(leftRaw);
        var rightParts = ParsePackParts(rightRaw);
        if (leftParts.Count > 0 && rightParts.Count > 0)
        {
            if (leftParts.SetEquals(rightParts))
            {
                return FieldSimilarity.Exact;
            }

            if (leftParts.IsSubsetOf(rightParts) || rightParts.IsSubsetOf(leftParts))
            {
                return FieldSimilarity.Near(0.88);
            }

            if (leftParts.Intersect(rightParts).Any())
            {
                return FieldSimilarity.Near(0.7);
            }
        }

        return SimilarityByEditDistance(left, right);
    }

    /// <summary>
    /// Субсидия, %: точное совпадение — Exact; оба числа 0–100, |Δ| ≤ 10 — Near с понижением по разнице.
    /// </summary>
    public static FieldSimilarity SimilaritySubsidy(string? leftRaw, string? rightRaw)
    {
        if (IsSubsidyAbsentOrZero(leftRaw) && IsSubsidyAbsentOrZero(rightRaw))
        {
            return FieldSimilarity.Exact;
        }

        var left = (leftRaw ?? string.Empty).Trim();
        var right = (rightRaw ?? string.Empty).Trim();
        if (string.Equals(left, right, StringComparison.OrdinalIgnoreCase))
        {
            return FieldSimilarity.Exact;
        }

        if (Operation41PairingKeyComparer.IsEmptySerial(left)
            && Operation41PairingKeyComparer.IsEmptySerial(right))
        {
            return FieldSimilarity.Exact;
        }

        if (TryParseSubsidyPercent(left, out var leftValue) && TryParseSubsidyPercent(right, out var rightValue))
        {
            var diff = Math.Abs(leftValue - rightValue);
            if (diff == 0)
            {
                return FieldSimilarity.Exact;
            }

            if (diff <= 10)
            {
                return FieldSimilarity.Near(Math.Max(0.55, 0.95 - diff * 0.04));
            }

            return FieldSimilarity.Mismatch(Math.Max(0.08, 0.35 - diff * 0.015));
        }

        return SimilarityByEditDistance(LightNormalizeId(left), LightNormalizeId(right));
    }

    private static bool TryParseSubsidyPercent(string raw, out int value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(raw) || raw is "-" or "прим.")
        {
            return false;
        }

        return int.TryParse(raw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value)
               && value is >= 0 and <= 100;
    }

    public static FieldSimilarity SimilarityByEditDistance(string left, string right)
    {
        if (left.Length == 0 || right.Length == 0)
        {
            return FieldSimilarity.Mismatch(0);
        }

        var s = NormalizedEditSimilarity(left, right);
        if (s >= 0.999)
        {
            return FieldSimilarity.Exact;
        }

        var maxLen = Math.Max(left.Length, right.Length);
        var distance = LevenshteinDistance(left, right);

        if (distance == 1 && maxLen > 2)
        {
            return FieldSimilarity.Near(0.78);
        }

        return FieldSimilarity.Mismatch(s);
    }

    public static double NormalizedEditSimilarity(string left, string right)
    {
        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            return 1;
        }

        var maxLen = Math.Max(left.Length, right.Length);
        if (maxLen == 0)
        {
            return 1;
        }

        return 1.0 - LevenshteinDistance(left, right) / (double)maxLen;
    }

    public static int LevenshteinDistance(string left, string right)
    {
        var n = left.Length;
        var m = right.Length;
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        var prev = new int[m + 1];
        var curr = new int[m + 1];
        for (var j = 0; j <= m; j++)
        {
            prev[j] = j;
        }

        for (var i = 1; i <= n; i++)
        {
            curr[0] = i;
            for (var j = 1; j <= m; j++)
            {
                var cost = left[i - 1] == right[j - 1] ? 0 : 1;
                curr[j] = Math.Min(
                    Math.Min(curr[j - 1] + 1, prev[j] + 1),
                    prev[j - 1] + cost);
            }

            (prev, curr) = (curr, prev);
        }

        return prev[m];
    }

    /// <summary>
    /// True, если после снятия ведущих нулей в каждом непрерывном числовом фрагменте
    /// строки совпадают. Число из одних нулей нормализуется в «0».
    /// </summary>
    public static bool EqualIgnoringLeadingZerosInDigitRuns(string left, string right)
    {
        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            return true;
        }

        if (left.Length == 0 || right.Length == 0)
        {
            return false;
        }

        return string.Equals(
            StripLeadingZerosInDigitRuns(left),
            StripLeadingZerosInDigitRuns(right),
            StringComparison.Ordinal);
    }

    public static string StripLeadingZerosInDigitRuns(string value) =>
        PackNumberTokenRegex().Replace(value, static match =>
        {
            var digits = match.Value.TrimStart('0');
            return digits.Length == 0 ? "0" : digits;
        });

    /// <summary>
    /// Совпадение без учёта дефисов/тире. False, если после снятия тире строка пустая.
    /// </summary>
    public static bool EqualIgnoringHyphens(string left, string right)
    {
        var leftBare = StripHyphens(left);
        var rightBare = StripHyphens(right);
        if (leftBare.Length == 0 || rightBare.Length == 0)
        {
            return false;
        }

        return string.Equals(leftBare, rightBare, StringComparison.Ordinal);
    }

    private static string StripHyphens(string value) =>
        value
            .Replace("-", string.Empty, StringComparison.Ordinal)
            .Replace("\u2013", string.Empty, StringComparison.Ordinal) // en-dash
            .Replace("\u2014", string.Empty, StringComparison.Ordinal); // em-dash

    public static string LightNormalizeId(string value, bool mapDigitZeroToO = false)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmed = value.Trim().ToLowerInvariant();
        trimmed = trimmed.Replace('\\', '/').Replace('|', '/');
        trimmed = LookalikeCharMapper.ReplaceRuEnLookalikes(
            trimmed, includeExtendedSnkSet: true, mapDigitZeroToO: mapDigitZeroToO);
        return Regex.Replace(trimmed, @"\s+", string.Empty);
    }

    public static string LightNormalizePack(string? value)
    {
        if (Operation41PairingKeyComparer.IsEmptySerial(value))
        {
            return string.Empty;
        }

        return LightNormalizeId(value!);
    }

    public static string DigitsOnly(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return new string(value.Where(char.IsDigit).ToArray());
    }

    private static string NormalizeDigitsOnly(string? value) => DigitsOnly(value);

    private static bool TryFactoryCoreMatch(string left, string right, out FieldSimilarity similarity)
    {
        similarity = default;
        var leftCore = StripManufacturerFactoryPrefix(left);
        var rightCore = StripManufacturerFactoryPrefix(right);
        if (string.Equals(leftCore, rightCore, StringComparison.Ordinal)
            && (leftCore != left || rightCore != right))
        {
            similarity = FieldSimilarity.Near(0.97);
            return true;
        }

        if (string.Equals(leftCore, right, StringComparison.Ordinal) && leftCore != left)
        {
            similarity = FieldSimilarity.Near(0.97);
            return true;
        }

        if (string.Equals(rightCore, left, StringComparison.Ordinal) && rightCore != right)
        {
            similarity = FieldSimilarity.Near(0.97);
            return true;
        }

        return false;
    }

    private static string StripManufacturerFactoryPrefix(string value)
    {
        var match = ManufacturerFactoryPrefixRegex().Match(value);
        return match.Success ? match.Groups[1].Value : value;
    }

    /// <summary>
    /// True, если одна сторона — перечисление зав.№ (одиночные числа и/или «start-end» через «,»/«;»),
    /// qty = числу раскрытых номеров, другая — одиночный номер из набора с qty 1.
    /// Работает в обе стороны (список↔одиночный). Тире без совпадения qty с длиной раскрытия
    /// не считается перечислением. Чистый одиночный номер без «,»;/диапазона — не список.
    /// </summary>
    public static bool TryFactoryNumericRangeAlias(
        string? leftRaw,
        string? rightRaw,
        int leftQuantity,
        int rightQuantity) =>
        TryGetFactoryEnumerationMatch(
            leftRaw, rightRaw, leftQuantity, rightQuantity, out _, out _);

    /// <summary>
    /// Как <see cref="TryFactoryNumericRangeAlias"/>, плюс N и какая сторона — перечисление.
    /// </summary>
    public static bool TryGetFactoryEnumerationMatch(
        string? leftRaw,
        string? rightRaw,
        int leftQuantity,
        int rightQuantity,
        out int enumerationQuantity,
        out bool leftIsEnumeration)
    {
        enumerationQuantity = 0;
        leftIsEnumeration = false;

        if (leftQuantity <= 0 || rightQuantity <= 0)
        {
            return false;
        }

        var left = NormalizeFactoryRangeText(leftRaw);
        var right = NormalizeFactoryRangeText(rightRaw);
        if (left.Length == 0 || right.Length == 0)
        {
            return false;
        }

        if (MatchesFactoryNumericEnumerationPair(leftRaw, right, leftQuantity, rightQuantity))
        {
            enumerationQuantity = leftQuantity;
            leftIsEnumeration = true;
            return true;
        }

        if (MatchesFactoryNumericEnumerationPair(rightRaw, left, rightQuantity, leftQuantity))
        {
            enumerationQuantity = rightQuantity;
            leftIsEnumeration = false;
            return true;
        }

        return false;
    }

    private static string NormalizeFactoryRangeText(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        var normalized = LightNormalizeId(raw, mapDigitZeroToO: false);
        return normalized
            .Replace('\u2013', '-') // en-dash
            .Replace('\u2014', '-'); // em-dash
    }

    private static bool MatchesFactoryNumericEnumerationPair(
        string? enumerationRaw,
        string singleSide,
        int enumerationQuantity,
        int singleQuantity)
    {
        if (singleQuantity != 1)
        {
            return false;
        }

        var enumerationSide = PrepareFactoryEnumerationSide(enumerationRaw);
        if (!LooksLikeFactoryNumericEnumerationCore(enumerationSide))
        {
            return false;
        }

        if (!TryExpandFactoryNumericEnumerationNormalized(enumerationSide, out var numbers)
            || numbers.Count == 0
            || numbers.Count != enumerationQuantity)
        {
            return false;
        }

        if (!TryParsePureFactoryNumber(singleSide, out var value))
        {
            return false;
        }

        return numbers.Contains(value);
    }

    /// <summary>
    /// Снимает «номера»/«номер»/«№» до lookalike-нормализации (иначе слово «номера» искажается lookalike),
    /// только если хвост похож на перечисление.
    /// </summary>
    private static string PrepareFactoryEnumerationSide(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        var trimmed = raw.Trim().ToLowerInvariant();
        var withoutLabel = FactoryEnumerationLabelPrefixRawRegex().Replace(trimmed, string.Empty);
        var strippedNorm = NormalizeFactoryRangeText(withoutLabel);
        var fullNorm = NormalizeFactoryRangeText(raw);

        if (strippedNorm.Length > 0
            && !string.Equals(strippedNorm, fullNorm, StringComparison.Ordinal)
            && LooksLikeFactoryNumericEnumerationCore(strippedNorm))
        {
            return strippedNorm;
        }

        // Fallback: после light-normalize кириллический префикс уже в lookalike-форме.
        var strippedLookalike = FactoryEnumerationLabelPrefixLookalikeRegex().Replace(fullNorm, string.Empty);
        if (strippedLookalike.Length > 0
            && !string.Equals(strippedLookalike, fullNorm, StringComparison.Ordinal)
            && LooksLikeFactoryNumericEnumerationCore(strippedLookalike))
        {
            return strippedLookalike;
        }

        return fullNorm;
    }

    /// <summary>
    /// Список (есть «,»/«;») или один числовой диапазон «start-end» — иначе это обычный зав.№.
    /// </summary>
    private static bool LooksLikeFactoryNumericEnumerationCore(string value) =>
        value.Contains(',')
        || value.Contains(';')
        || FactoryNumericRangeRegex().IsMatch(value);

    /// <summary>
    /// Раскрывает список/диапазон зав.№ (в т.ч. с префиксом «номера»/«№») в множество чисел. False при «грязном» токене.
    /// </summary>
    public static bool TryExpandFactoryNumericEnumeration(string? raw, out HashSet<long> numbers)
    {
        numbers = new HashSet<long>();
        var normalized = PrepareFactoryEnumerationSide(raw);
        if (normalized.Length == 0)
        {
            return false;
        }

        return TryExpandFactoryNumericEnumerationNormalized(normalized, out numbers);
    }

    private static bool TryExpandFactoryNumericEnumerationNormalized(string normalized, out HashSet<long> numbers)
    {
        numbers = new HashSet<long>();
        foreach (var token in normalized.Split(FactoryEnumerationSeparators, StringSplitOptions.RemoveEmptyEntries))
        {
            if (TryParsePureFactoryNumber(token, out var single))
            {
                numbers.Add(single);
                continue;
            }

            if (!TryParseFactoryNumericRange(token, out var start, out var end) || end < start)
            {
                numbers.Clear();
                return false;
            }

            for (var n = start; n <= end; n++)
            {
                numbers.Add(n);
            }
        }

        return numbers.Count > 0;
    }

    private static readonly char[] FactoryEnumerationSeparators = [',', ';'];

    private static bool TryParseFactoryNumericRange(string value, out long start, out long end)
    {
        start = 0;
        end = 0;
        var match = FactoryNumericRangeRegex().Match(value);
        if (!match.Success)
        {
            return false;
        }

        return long.TryParse(match.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out start)
            && long.TryParse(match.Groups[2].Value, NumberStyles.None, CultureInfo.InvariantCulture, out end);
    }

    private static bool TryParsePureFactoryNumber(string value, out long number)
    {
        number = 0;
        if (value.Length == 0 || !value.All(char.IsDigit))
        {
            return false;
        }

        return long.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out number);
    }

    /// <summary>
    /// True, если одна строка — это другая плюс хвост даты «MM.yyyy»
    /// (после light-normalize пробелы уже сняты; допустимы разделители , ; / - _ перед датой).
    /// </summary>
    public static bool MatchesTrailingMonthYearAlias(string left, string right)
    {
        if (left.Length == 0 || right.Length == 0)
        {
            return false;
        }

        if (TryStripTrailingMonthYear(left, out var leftCore)
            && string.Equals(leftCore, right, StringComparison.Ordinal))
        {
            return true;
        }

        if (TryStripTrailingMonthYear(right, out var rightCore)
            && string.Equals(rightCore, left, StringComparison.Ordinal))
        {
            return true;
        }

        return false;
    }

    public static bool TryStripTrailingMonthYear(string value, out string core)
    {
        core = value;
        var match = TrailingMonthYearSuffixRegex().Match(value);
        if (!match.Success)
        {
            return false;
        }

        core = match.Groups[1].Value;
        return core.Length > 0;
    }

    /// <summary>
    /// True, если одна строка — другая плюс хвост «No/№/номер/номера» + цифры
    /// (после light-normalize пробелы сняты; кириллические метки в lookalike-форме).
    /// Только паспорт/зав.№; не трогает префикс перечисления «номера»/«№» в начале строки.
    /// </summary>
    public static bool MatchesTrailingSerialLabelAlias(string left, string right)
    {
        if (left.Length == 0 || right.Length == 0)
        {
            return false;
        }

        if (TryStripTrailingSerialLabel(left, out var leftCore)
            && string.Equals(leftCore, right, StringComparison.Ordinal))
        {
            return true;
        }

        if (TryStripTrailingSerialLabel(right, out var rightCore)
            && string.Equals(rightCore, left, StringComparison.Ordinal))
        {
            return true;
        }

        return false;
    }

    public static bool TryStripTrailingSerialLabel(string value, out string core)
    {
        core = value;
        var match = TrailingSerialLabelSuffixRegex().Match(value);
        if (!match.Success)
        {
            return false;
        }

        core = match.Groups[1].Value;
        // Короткое ядро слишком слабо (случайный короткий префикс перед меткой).
        return core.Length >= 3;
    }

    /// <summary>
    /// Паспорт: акт с номером ↔ тот же акт (кратко/развёрнуто) или голый номер того же акта.
    /// </summary>
    public static bool MatchesPassportActNumberAlias(string? leftRaw, string? rightRaw)
    {
        var leftIsAct = TryGetPassportActNumber(leftRaw, out var leftNum);
        var rightIsAct = TryGetPassportActNumber(rightRaw, out var rightNum);

        if (leftIsAct && rightIsAct)
        {
            return string.Equals(
                NormalizeActNumberDigits(leftNum),
                NormalizeActNumberDigits(rightNum),
                StringComparison.Ordinal);
        }

        // Одна сторона — «акт … №N», другая — только номер (или №/No/номер + цифры).
        if (leftIsAct && IsBarePassportActNumber(rightRaw, leftNum))
        {
            return true;
        }

        if (rightIsAct && IsBarePassportActNumber(leftRaw, rightNum))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// True, если строка — только номер акта (цифры, опционально метка №/No/номер), без слова «акт».
    /// </summary>
    public static bool IsBarePassportActNumber(string? raw, string actNumber)
    {
        if (string.IsNullOrWhiteSpace(raw)
            || Operation41PairingKeyComparer.IsEmptySerial(raw)
            || PassportActWordRegex().IsMatch(raw))
        {
            return false;
        }

        var bareMatch = PassportBareActNumberRegex().Match(raw.Trim());
        if (!bareMatch.Success)
        {
            return false;
        }

        return string.Equals(
            NormalizeActNumberDigits(bareMatch.Groups[1].Value),
            NormalizeActNumberDigits(actNumber),
            StringComparison.Ordinal);
    }

    public static bool TryGetPassportActNumber(string? raw, out string number)
    {
        number = string.Empty;
        if (string.IsNullOrWhiteSpace(raw))
        {
            return false;
        }

        var actMatch = PassportActWordRegex().Match(raw);
        if (!actMatch.Success)
        {
            return false;
        }

        // Номер ищем только после «акт», чтобы не взять чужой № из префикса.
        var afterAct = raw[actMatch.Index..];
        var numMatch = PassportActNumberLabelRegex().Match(afterAct);
        if (!numMatch.Success)
        {
            return false;
        }

        number = numMatch.Groups[1].Value;
        return number.Length > 0;
    }

    private static string NormalizeActNumberDigits(string digits)
    {
        var trimmed = digits.TrimStart('0');
        return trimmed.Length == 0 ? "0" : trimmed;
    }

    private static bool TryYearSuffixVariantMatch(string left, string right, out FieldSimilarity similarity)
    {
        similarity = default;
        var leftBase = StripTrailingYearSuffix(left);
        var rightBase = StripTrailingYearSuffix(right);
        var leftHadYear = leftBase != left;
        var rightHadYear = rightBase != right;

        if (string.Equals(leftBase, rightBase, StringComparison.Ordinal)
            && (leftHadYear || rightHadYear))
        {
            similarity = FieldSimilarity.Near(0.9);
            return true;
        }

        if (IsPrefixWithShortTail(left, right) || IsPrefixWithShortTail(right, left))
        {
            similarity = FieldSimilarity.Near(0.88);
            return true;
        }

        if (!leftHadYear && !rightHadYear)
        {
            return false;
        }

        var baseDistance = LevenshteinDistance(leftBase, rightBase);
        var baseLen = Math.Max(leftBase.Length, rightBase.Length);
        if (baseDistance == 1 && baseLen > 2)
        {
            similarity = FieldSimilarity.Near(0.8);
            return true;
        }

        // Не форсируем Mismatch: слабое совпадение ядер после снятия «года» может быть ложным
        // (короткий префикс + «-NN»), дальше сработают Near по дефису/edit.
        return false;
    }

    /// <summary>
    /// Снимает хвост «/YY» или «-YY», только если ядро достаточно длинное
    /// (короткое ядро — не год, а часть номера).
    /// </summary>
    private static string StripTrailingYearSuffix(string value)
    {
        var match = TrailingYearSuffixRegex().Match(value);
        if (!match.Success)
        {
            return value;
        }

        var core = match.Groups[1].Value;
        // Короткое ядро перед «-NN» — не год, а часть заводского/паспортного номера.
        return core.Length >= 3 ? core : value;
    }

    private static bool IsPrefixWithShortTail(string longer, string shorter)
    {
        // Короткое ядро (1–2 символа) слишком слабо: «1»↔«100» после снятия ведущих нулей.
        if (shorter.Length < 3
            || longer.Length <= shorter.Length
            || !longer.StartsWith(shorter, StringComparison.Ordinal))
        {
            return false;
        }

        var tail = longer[shorter.Length..];
        return tail.Length is >= 1 and <= 2 && tail.All(char.IsDigit);
    }

    private static string StripTypeOptionalTail(string value)
    {
        var match = TypeOptionalTailRegex().Match(value);
        return match.Success ? match.Groups[1].Value : value;
    }

    private static double BestTokenEditSimilarity(HashSet<string> left, HashSet<string> right)
    {
        var best = 0.0;
        foreach (var a in left)
        {
            foreach (var b in right)
            {
                best = Math.Max(best, NormalizedEditSimilarity(a, b));
            }
        }

        return best;
    }

    private static HashSet<string> ParsePackParts(string? raw)
    {
        var result = new HashSet<string>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(raw) || raw.Trim() == "-")
        {
            return result;
        }

        // Составной номер через тире — один токен УКТ, не список частей
        // (иначе общий короткий префикс даёт ложный Near). Списки в скобках/через пробел — отдельные токены.
        foreach (Match match in PackNumberPartTokenRegex().Matches(raw))
        {
            var token = DigitsOnly(match.Value);
            if (token.Length > 0)
            {
                result.Add(token);
            }
        }

        return result;
    }

    [GeneratedRegex(@"^[мм\d]\d{4}/\d{2}/(.+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ManufacturerFactoryPrefixRegex();

    /// <summary>Чисто числовой диапазон зав.№ «start-end» (после light-normalize и унификации тире).</summary>
    [GeneratedRegex(@"^(\d+)-(\d+)$")]
    private static partial Regex FactoryNumericRangeRegex();

    /// <summary>Префикс «номера»/«номер»/«№» до lookalike (пробелы ещё на месте).</summary>
    [GeneratedRegex(@"^(?:номера|номер|№)\s*[:.]*\s*(?=\d)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex FactoryEnumerationLabelPrefixRawRegex();

    /// <summary>
    /// Тот же префикс после light-normalize (кириллица в lookalike-форме).
    /// </summary>
    [GeneratedRegex(@"^(?:homepa|homep|№)+[:.]*(?=\d)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex FactoryEnumerationLabelPrefixLookalikeRegex();

    [GeneratedRegex(@"^(.+?)[/\-]\d{2}$")]
    private static partial Regex TrailingYearSuffixRegex();

    /// <summary>
    /// Хвост «MM.yyyy» с опциональным разделителем (пробел уже снят light-normalize).
    /// </summary>
    [GeneratedRegex(@"^(.+?)[,;/\-_]*(\d{2}\.\d{4})$")]
    private static partial Regex TrailingMonthYearSuffixRegex();

    /// <summary>
    /// Хвост «No/№/number/номер/номера» + цифры после light-normalize (пробелы сняты, lookalike).
    /// Длинные метки раньше коротких.
    /// </summary>
    [GeneratedRegex(@"^(.+)(?:homepa|homep|number|no|№)[:.]*(\d+)$", RegexOptions.CultureInvariant)]
    private static partial Regex TrailingSerialLabelSuffixRegex();

    /// <summary>Слово «акт» в паспорте-документе (до lookalike).</summary>
    [GeneratedRegex(@"\bакт\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex PassportActWordRegex();

    /// <summary>Номер акта после «акт»: № / No / номер + цифры (не дата «от DD.MM.YYYY»).</summary>
    [GeneratedRegex(@"(?:№|no\.?|номер)\s*[:.]?\s*(\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex PassportActNumberLabelRegex();

    /// <summary>Голый номер акта без слова «акт»: цифры или №/No/номер + цифры.</summary>
    [GeneratedRegex(@"^(?:№|no\.?|номер)?\s*[:.]?\s*(\d+)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex PassportBareActNumberRegex();

    [GeneratedRegex(@"^([^\-]+?)(?:\-\d+(?:\.\d+)?)?$")]
    private static partial Regex TypeOptionalTailRegex();

    /// <summary>Хвост «.XX» ровно из двух цифр (тип / номер).</summary>
    [GeneratedRegex(@"^\.\d{2}$")]
    private static partial Regex TypeOptionalDotTwoDigitSuffixRegex();

    /// <summary>Тип: хвостовой модификатор «.» + 1–2 буквы/цифры.</summary>
    [GeneratedRegex(@"^\.[A-Za-z0-9]{1,2}$", RegexOptions.CultureInvariant)]
    private static partial Regex TypeOptionalDotShortSuffixRegex();

    /// <summary>Тип: хвост «-»/«.» + 1–4 знака с буквы (модификатор вроде SFC).</summary>
    [GeneratedRegex(@"^(.+)[-.]([A-Za-z][A-Za-z0-9]{0,3})$", RegexOptions.CultureInvariant)]
    private static partial Regex TypeOptionalLetterSuffixRegex();

    [GeneratedRegex(@"\(([^()]*)\)")]
    private static partial Regex ParentheticalContentRegex();

    [GeneratedRegex(@"\d+")]
    private static partial Regex PackNumberTokenRegex();

    /// <summary>
    /// Токен номера УКТ: либо одиночное число, либо составной «N-M» целиком (тире ASCII/en/em).
    /// </summary>
    [GeneratedRegex(@"\d+(?:[-–—]\d+)?")]
    private static partial Regex PackNumberPartTokenRegex();
}
