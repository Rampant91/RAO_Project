using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Client_App.Resources.CustomComparers.SnkComparers;
using Models.Comparers.FormContent;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    /// <summary>Уровень совпадения поля для подсветки closest (зелёный / жёлтый / красный).</summary>
    public enum FieldMatchLevel
    {
        Exact = 0,
        Near = 1,
        Mismatch = 2
    }

    private readonly record struct FieldSimilarity(double Score, FieldMatchLevel Level)
    {
        public static FieldSimilarity Exact => new(1.0, FieldMatchLevel.Exact);
        public static FieldSimilarity Near(double score) => new(Math.Clamp(score, 0, 0.999), FieldMatchLevel.Near);
        public static FieldSimilarity Mismatch(double score = 0) => new(Math.Clamp(score, 0, 1), FieldMatchLevel.Mismatch);
    }

    #region Soft similarity (closest only)

    private static FieldSimilarity FieldSimilarityOf(
        TransferReceiveDto source,
        TransferReceiveDto candidate,
        TransferReceiveNorm sourceNorm,
        TransferReceiveNorm candidateNorm,
        TransferReceiveField field,
        string sourceOrgOkpo) =>
        field switch
        {
            TransferReceiveField.OperationCode => SimilarityOperationCode(sourceNorm.OpCode, candidateNorm.OpCode),
            TransferReceiveField.OperationDate => SimilarityOperationDate(sourceNorm.OpDateRaw, candidateNorm.OpDateRaw),
            TransferReceiveField.PassportNumber => SimilarityPassportOrFactory(
                source.PasNum, candidate.PasNum, isFactory: false),
            TransferReceiveField.FactoryNumber => SimilarityPassportOrFactory(
                source.FacNum, candidate.FacNum, isFactory: true),
            TransferReceiveField.Type => SimilarityType(source.Type, candidate.Type),
            TransferReceiveField.Radionuclids => SimilarityRadionuclids(sourceNorm.Radionuclids, candidateNorm.Radionuclids, source.Radionuclids, candidate.Radionuclids),
            TransferReceiveField.Quantity => sourceNorm.Quantity == candidateNorm.Quantity
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0),
            TransferReceiveField.AggregateState => sourceNorm.AggregateState == candidateNorm.AggregateState
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0),
            TransferReceiveField.Activity => SimilarityActivity(sourceNorm.Activity, candidateNorm.Activity),
            TransferReceiveField.Mass => SimilarityMass(sourceNorm.Mass, candidateNorm.Mass),
            TransferReceiveField.CreatorOkpo => SimilarityOkpo(sourceNorm.CreatorOkpo, candidateNorm.CreatorOkpo),
            TransferReceiveField.CreationDate => SimilarityCreationDate(source.CreationDate, candidate.CreationDate),
            TransferReceiveField.PackType => SimilarityType(source.PackType, candidate.PackType),
            TransferReceiveField.PackNumber => SimilarityPackNumber(source.PackNumber, candidate.PackNumber),
            TransferReceiveField.ProviderOrRecieverOkpo => SimilarityProviderOkpo(
                candidateNorm.ProviderOrRecieverOkpo, sourceNorm.OrgOkpo, NormalizeNumber(sourceOrgOkpo)),
            _ => FieldSimilarity.Mismatch(0)
        };

    private static FieldSimilarity SimilarityOperationCode(string left, string right)
    {
        if (OpCodesArePaired(left, right))
        {
            return FieldSimilarity.Exact;
        }

        var leftTr = IsTransferOrReceiveCodeForm11(left);
        var rightTr = IsTransferOrReceiveCodeForm11(right);
        if (leftTr && rightTr)
        {
            // Оба из набора приём/передача, но непарные — типичная «небольшая» ошибка сторон.
            return FieldSimilarity.Near(0.72);
        }

        return FieldSimilarity.Mismatch(0.15);
    }

    private static FieldSimilarity SimilarityOperationDate(string? left, string? right)
    {
        if (DatesEqualExact(left, right))
        {
            return FieldSimilarity.Exact;
        }

        if (!DateOnly.TryParse(left, out var leftDate) || !DateOnly.TryParse(right, out var rightDate))
        {
            return string.Equals(NormalizeDate(left), NormalizeDate(right), StringComparison.Ordinal)
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0.1);
        }

        var delta = Math.Abs(leftDate.DayNumber - rightDate.DayNumber);
        if (delta > OperationDateToleranceDays)
        {
            return FieldSimilarity.Mismatch(0);
        }

        // ±1…15 дней — часто и допустимо для поиска; точное совпадение даты слабо информативно.
        var score = 0.92 - (delta - 1) * (0.42 / Math.Max(1, OperationDateToleranceDays - 1));
        return FieldSimilarity.Near(score);
    }

    private static FieldSimilarity SimilarityPassportOrFactory(string? leftRaw, string? rightRaw, bool isFactory)
    {
        var leftEmpty = Operation41PairingKeyComparer.IsEmptySerial(leftRaw);
        var rightEmpty = Operation41PairingKeyComparer.IsEmptySerial(rightRaw);
        if (leftEmpty && rightEmpty)
        {
            // Оба пустые — слегка повышает шанс, но не «точное» совпадение идентификатора.
            return FieldSimilarity.Near(0.55);
        }

        if (leftEmpty || rightEmpty)
        {
            return FieldSimilarity.Mismatch(0.05);
        }

        // Префикс рег.№/год в зав.№ — без 0→o (иначе «74041/22/0436» ломается).
        if (isFactory)
        {
            var leftForPrefix = LightNormalizeId(leftRaw!, mapDigitZeroToO: false);
            var rightForPrefix = LightNormalizeId(rightRaw!, mapDigitZeroToO: false);
            if (TryFactoryCoreMatch(leftForPrefix, rightForPrefix, out var factoryScore))
            {
                return factoryScore;
            }
        }

        // Lookalike с 0→o / З→3: «3C0», «3СО», «ЗС0» — одно и то же.
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

        if (TryYearSuffixVariantMatch(left, right, out var yearScore))
        {
            return yearScore;
        }

        return SimilarityByEditDistance(left, right);
    }

    /// <summary>
    /// «74041/22/0436» ↔ «0436»: префикс рег.№ изготовителя + год + слэш можно считать почти полным совпадением.
    /// </summary>
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

    /// <summary>Рег.№ изготовителя: [Мм|digit]+4 digits (как Form10), затем /|\ год /|\ ядро номера.</summary>
    private static string StripManufacturerFactoryPrefix(string value)
    {
        var match = ManufacturerFactoryPrefixRegex().Match(value);
        return match.Success ? match.Groups[1].Value : value;
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
            // Одинаковое ядро, отличие только в суффиксе года (или год есть только с одной стороны).
            similarity = FieldSimilarity.Near(0.9);
            return true;
        }

        // «3197/25» ↔ «3197/2» — недописана цифра года.
        if (IsPrefixWithShortTail(left, right) || IsPrefixWithShortTail(right, left))
        {
            similarity = FieldSimilarity.Near(0.88);
            return true;
        }

        if (!leftHadYear && !rightHadYear)
        {
            return false;
        }

        // Общий «/год», но разные номера («829/22» ↔ «721/22») — сравниваем ядра, не всю строку.
        // Иначе Levenshtein по полной строке занижает отличие из‑за совпавшего суффикса.
        var baseDistance = LevenshteinDistance(leftBase, rightBase);
        var baseLen = Math.Max(leftBase.Length, rightBase.Length);
        if (baseDistance == 1 && baseLen > 2)
        {
            similarity = FieldSimilarity.Near(0.8);
            return true;
        }

        similarity = FieldSimilarity.Mismatch(
            baseLen == 0 ? 0 : Math.Max(0, 1.0 - baseDistance / (double)baseLen) * 0.35);
        return true;
    }

    private static string StripTrailingYearSuffix(string value)
    {
        var match = TrailingYearSuffixRegex().Match(value);
        return match.Success ? match.Groups[1].Value : value;
    }

    private static bool IsPrefixWithShortTail(string longer, string shorter)
    {
        if (longer.Length <= shorter.Length || !longer.StartsWith(shorter, StringComparison.Ordinal))
        {
            return false;
        }

        var tail = longer[shorter.Length..];
        return tail.Length is >= 1 and <= 2 && tail.All(char.IsDigit);
    }

    private static FieldSimilarity SimilarityType(string? leftRaw, string? rightRaw)
    {
        // В типе часто путают 0 и О («6С0» ↔ «6СО-326») — для типа 0→o нужен.
        var left = LightNormalizeId(leftRaw ?? string.Empty, mapDigitZeroToO: true);
        var right = LightNormalizeId(rightRaw ?? string.Empty, mapDigitZeroToO: true);
        if (left.Length == 0 && right.Length == 0)
        {
            return FieldSimilarity.Near(0.5);
        }

        if (left.Length == 0 || right.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.1);
        }

        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        var leftCore = StripTypeOptionalTail(left);
        var rightCore = StripTypeOptionalTail(right);
        if (string.Equals(leftCore, rightCore, StringComparison.Ordinal)
            || string.Equals(leftCore, right, StringComparison.Ordinal)
            || string.Equals(rightCore, left, StringComparison.Ordinal))
        {
            return FieldSimilarity.Near(0.86);
        }

        return SimilarityByEditDistance(left, right);
    }

    /// <summary>«2П9-254»→«2П9», «1СО-326.24»→«1СО».</summary>
    private static string StripTypeOptionalTail(string value)
    {
        var match = TypeOptionalTailRegex().Match(value);
        return match.Success ? match.Groups[1].Value : value;
    }

    private static FieldSimilarity SimilarityRadionuclids(
        string leftNormJoined,
        string rightNormJoined,
        string? leftRaw,
        string? rightRaw)
    {
        if (string.Equals(leftNormJoined, rightNormJoined, StringComparison.Ordinal))
        {
            return leftNormJoined.Length == 0
                ? FieldSimilarity.Near(0.5)
                : FieldSimilarity.Exact;
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

        // Одинаковое число токенов — возможна опечатка в наименовании/номере.
        if (leftSet.Count == rightSet.Count)
        {
            var bestToken = BestTokenEditSimilarity(leftSet, rightSet);
            // «Cs-137»↔«Cs-138»: 1 символ при коротком токене → ~0.8.
            if (bestToken >= 0.8)
            {
                return FieldSimilarity.Near(0.55 + 0.3 * bestToken);
            }

            return FieldSimilarity.Mismatch(0.15 * bestToken);
        }

        // Отсутствие или лишний радионуклид — серьёзное расхождение (не жёлтый).
        _ = leftRaw;
        _ = rightRaw;
        var union = leftSet.Union(rightSet).Count();
        var jaccard = union == 0 ? 0 : intersection / (double)union;
        return FieldSimilarity.Mismatch(0.2 * jaccard);
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

    private static FieldSimilarity SimilarityActivity(string left, string right)
    {
        if (!TryParseActivity(left, out var la) || !TryParseActivity(right, out var ra))
        {
            return string.Equals(NormalizeNumber(left), NormalizeNumber(right), StringComparison.Ordinal)
                ? FieldSimilarity.Exact
                : SimilarityByEditDistance(LightNormalizeId(left), LightNormalizeId(right));
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

        // Ошибка на порядок (×10) — заметная, но не исключающая.
        var ratio = Math.Max(Math.Abs(la), 1e-300) / Math.Max(Math.Abs(ra), 1e-300);
        var log10 = Math.Abs(Math.Log10(ratio));
        if (log10 is >= 0.85 and <= 1.15)
        {
            return FieldSimilarity.Near(0.62);
        }

        if (rel <= 0.35)
        {
            return FieldSimilarity.Near(0.7 - rel);
        }

        return FieldSimilarity.Mismatch(Math.Max(0, 0.25 - rel));
    }

    /// <summary>
    /// Масса (кг): как активность (±10% Exact, порядок ×10 Near), плюс ×1000 (кг↔т) — Near.
    /// </summary>
    private static FieldSimilarity SimilarityMass(string left, string right)
    {
        if (!TryParseActivity(left, out var la) || !TryParseActivity(right, out var ra))
        {
            return string.Equals(NormalizeNumber(left), NormalizeNumber(right), StringComparison.Ordinal)
                ? FieldSimilarity.Exact
                : SimilarityByEditDistance(LightNormalizeId(left), LightNormalizeId(right));
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

        // Ошибка на порядок (×10) — как у активности.
        if (log10 is >= 0.85 and <= 1.15)
        {
            return FieldSimilarity.Near(0.62);
        }

        // Путаница кг и тонн (ровно ×1000).
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

    private static FieldSimilarity SimilarityOkpo(string leftNorm, string rightNorm)
    {
        if (leftNorm.Length == 0 && rightNorm.Length == 0)
        {
            return FieldSimilarity.Near(0.5);
        }

        if (leftNorm.Length == 0 || rightNorm.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.1);
        }

        if (string.Equals(leftNorm, rightNorm, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        return SimilarityByEditDistance(leftNorm, rightNorm);
    }

    private static FieldSimilarity SimilarityProviderOkpo(
        string candidateProviderNorm,
        string sourceOrgNorm,
        string sourceOrgOkpoNorm)
    {
        if (candidateProviderNorm.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.15);
        }

        if (candidateProviderNorm == sourceOrgNorm || candidateProviderNorm == sourceOrgOkpoNorm)
        {
            return FieldSimilarity.Exact;
        }

        var best = Math.Max(
            NormalizedEditSimilarity(candidateProviderNorm, sourceOrgNorm),
            NormalizedEditSimilarity(candidateProviderNorm, sourceOrgOkpoNorm));
        if (best >= 0.9)
        {
            return FieldSimilarity.Near(best);
        }

        // Полное несовпадение — значительная ошибка, но не абсолютный ноль.
        return FieldSimilarity.Mismatch(Math.Min(0.25, best));
    }

    private static FieldSimilarity SimilarityCreationDate(string? leftRaw, string? rightRaw)
    {
        var leftDigits = DigitsOnly(leftRaw);
        var rightDigits = DigitsOnly(rightRaw);
        if (leftDigits.Length == 0 && rightDigits.Length == 0)
        {
            return FieldSimilarity.Near(0.5);
        }

        if (leftDigits.Length == 0 || rightDigits.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.1);
        }

        if (string.Equals(leftDigits, rightDigits, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        // Сравниваем как набор символов ввода (опечатка в одной цифре важнее «близости дат»).
        return SimilarityByEditDistance(leftDigits, rightDigits);
    }

    private static FieldSimilarity SimilarityPackNumber(string? leftRaw, string? rightRaw)
    {
        var left = LightNormalizePack(leftRaw);
        var right = LightNormalizePack(rightRaw);
        if (left.Length == 0 && right.Length == 0)
        {
            return FieldSimilarity.Near(0.5);
        }

        if (left.Length == 0 || right.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.15);
        }

        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        var leftParts = ParsePackParts(leftRaw);
        var rightParts = ParsePackParts(rightRaw);
        if (leftParts.Count > 0 && rightParts.Count > 0)
        {
            if (leftParts.SetEquals(rightParts))
            {
                // «39 (38)» ↔ «38 (39)»
                return FieldSimilarity.Near(0.93);
            }

            if (leftParts.IsSubsetOf(rightParts) || rightParts.IsSubsetOf(leftParts))
            {
                // «67» ↔ «67(69)», «33(48)» ↔ «33», «33(61)» ↔ «61»
                return FieldSimilarity.Near(0.88);
            }

            if (leftParts.Intersect(rightParts).Any())
            {
                return FieldSimilarity.Near(0.7);
            }
        }

        return SimilarityByEditDistance(left, right);
    }

    private static HashSet<string> ParsePackParts(string? raw)
    {
        var result = new HashSet<string>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(raw) || raw.Trim() == "-")
        {
            return result;
        }

        foreach (Match match in PackNumberTokenRegex().Matches(raw))
        {
            result.Add(match.Value);
        }

        return result;
    }

    private static FieldSimilarity SimilarityByEditDistance(string left, string right)
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

        // Опечатка в 1 символ при длине поля > 2 — жёлтый; 2+ символа — уже значительное (красный).
        if (distance == 1 && maxLen > 2)
        {
            return FieldSimilarity.Near(0.78);
        }

        return FieldSimilarity.Mismatch(s);
    }

    private static double NormalizedEditSimilarity(string left, string right)
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

    private static int LevenshteinDistance(string left, string right)
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

    private static string LightNormalizeId(string value, bool mapDigitZeroToO = false)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmed = value.Trim().ToLowerInvariant();
        trimmed = trimmed.Replace('\\', '/').Replace('|', '/');
        // По умолчанию без 0→o: иначе ломается префикс рег.№/год в зав.№ («74041/22/0436»).
        // Для типа (и при явной необходимости) mapDigitZeroToO=true — «6С0» ↔ «6СО».
        trimmed = LookalikeCharMapper.ReplaceRuEnLookalikes(
            trimmed, includeExtendedSnkSet: true, mapDigitZeroToO: mapDigitZeroToO);
        return Regex.Replace(trimmed, @"\s+", string.Empty);
    }

    private static string LightNormalizePack(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim() == "-")
        {
            return string.Empty;
        }

        return LightNormalizeId(value);
    }

    private static string DigitsOnly(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return new string(value.Where(char.IsDigit).ToArray());
    }

    private static double GetFieldWeight(TransferReceiveField field, TransferReceiveDto source)
    {
        var serialsEmpty = SerialNumbersAreEmpty(source);
        return field switch
        {
            TransferReceiveField.PassportNumber => serialsEmpty ? 2.0 : 12.0,
            TransferReceiveField.FactoryNumber => serialsEmpty ? 2.0 : 12.0,
            TransferReceiveField.OperationCode => 1.5,
            TransferReceiveField.OperationDate => 2.0,
            TransferReceiveField.Type => 3.0,
            TransferReceiveField.Radionuclids => 4.5,
            TransferReceiveField.Activity => 2.5,
            TransferReceiveField.Mass => 2.5,
            TransferReceiveField.CreatorOkpo => 4.0,
            TransferReceiveField.CreationDate => 3.0,
            TransferReceiveField.ProviderOrRecieverOkpo => 4.0,
            // Тип УКТ на 1.2 важнее обычных полей, но слабее паспорта/зав.№.
            TransferReceiveField.PackType => 6.5,
            TransferReceiveField.PackNumber => 3.5,
            TransferReceiveField.Quantity => 2.0,
            TransferReceiveField.AggregateState => 3.0,
            _ => 1.0
        };
    }

    /// <summary>Бонус, если оба ключевых идентификатора совпали точно (и не пустые).</summary>
    private const double BothIdentifiersExactBonus = 10.0;

    [GeneratedRegex(@"^[мм\d]\d{4}/\d{2}/(.+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ManufacturerFactoryPrefixRegex();

    [GeneratedRegex(@"^(.+?)[/\-]\d{2}$")]
    private static partial Regex TrailingYearSuffixRegex();

    [GeneratedRegex(@"^([^\-]+?)(?:\-\d+(?:\.\d+)?)?$")]
    private static partial Regex TypeOptionalTailRegex();

    [GeneratedRegex(@"\d+")]
    private static partial Regex PackNumberTokenRegex();

    #endregion
}
