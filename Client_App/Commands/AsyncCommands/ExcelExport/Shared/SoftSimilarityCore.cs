using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Client_App.Resources.CustomComparers.SnkComparers;
using Models.Comparers.FormContent;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Shared;

/// <summary>
/// Общие алгоритмы soft-similarity для closest-match (паспорт, зав.№, тип, активность и т.д.).
/// </summary>
public static partial class SoftSimilarityCore
{
    public static bool TryParseNumeric(string? value, out double result)
    {
        result = 0;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = value.Trim().Replace(" ", string.Empty).Replace(',', '.');
        return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
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

    public static FieldSimilarity SimilarityPassportOrFactory(string? leftRaw, string? rightRaw, bool isFactory)
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

        if (TryYearSuffixVariantMatch(left, right, out var yearScore))
        {
            return yearScore;
        }

        return SimilarityByEditDistance(left, right);
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
        var leftValue = normalize is null ? left : normalize(left);
        var rightValue = normalize is null ? right : normalize(right);

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

    public static FieldSimilarity SimilarityCreationDate(string? leftRaw, string? rightRaw)
    {
        var leftDigits = DigitsOnly(leftRaw);
        var rightDigits = DigitsOnly(rightRaw);
        if (string.Equals(leftDigits, rightDigits, StringComparison.Ordinal))
        {
            return FieldSimilarity.Exact;
        }

        if (leftDigits.Length == 0 || rightDigits.Length == 0)
        {
            return FieldSimilarity.Mismatch(0.1);
        }

        return SimilarityByEditDistance(leftDigits, rightDigits);
    }

    public static FieldSimilarity SimilarityPackNumber(string? leftRaw, string? rightRaw)
    {
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
        if (string.IsNullOrWhiteSpace(value) || value.Trim() == "-")
        {
            return string.Empty;
        }

        return LightNormalizeId(value);
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

        foreach (Match match in PackNumberTokenRegex().Matches(raw))
        {
            result.Add(match.Value);
        }

        return result;
    }

    [GeneratedRegex(@"^[мм\d]\d{4}/\d{2}/(.+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ManufacturerFactoryPrefixRegex();

    [GeneratedRegex(@"^(.+?)[/\-]\d{2}$")]
    private static partial Regex TrailingYearSuffixRegex();

    [GeneratedRegex(@"^([^\-]+?)(?:\-\d+(?:\.\d+)?)?$")]
    private static partial Regex TypeOptionalTailRegex();

    [GeneratedRegex(@"\d+")]
    private static partial Regex PackNumberTokenRegex();
}
