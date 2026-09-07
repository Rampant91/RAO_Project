using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Resources.CustomComparers.SnkComparers;
using Microsoft.EntityFrameworkCore;
using Models.Comparers.FormContent;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Normalization / keys

    private static bool SerialNumbersAreEmpty(TransferReceiveDto item) =>
        Operation41PairingKeyComparer.SerialNumbersIsEmpty(item.PasNum, item.FacNum);

    private static string NormalizeSerialNumber(string? value) =>
        Operation41PairingKeyComparer.IsEmptySerial(value) ? string.Empty : NormalizeNumber(value);

    private static string NormalizeNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "-")
        {
            return string.Empty;
        }

        var normalized = Regex.Replace(value.ToLowerInvariant(), @"[\\/:*?""<>|.,_\-;:\s+]", string.Empty)
            .TrimStart('0');
        return LookalikeCharMapper.ReplaceRuEnLookalikes(normalized, includeExtendedSnkSet: true);
    }

    /// <summary>
    /// Формат «8 цифр_5 цифр» (напр. 01234567_12345).
    /// </summary>
    private static readonly Regex OkpoEightUnderscoreFiveRegex =
        new(@"^(\d{8})_(\d{5})$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static bool TryParseOkpoEightUnderscoreFive(string? raw, out string head8, out string tail5)
    {
        head8 = string.Empty;
        tail5 = string.Empty;
        if (string.IsNullOrWhiteSpace(raw))
        {
            return false;
        }

        var match = OkpoEightUnderscoreFiveRegex.Match(raw.Trim());
        if (!match.Success)
        {
            return false;
        }

        head8 = match.Groups[1].Value;
        tail5 = match.Groups[2].Value;
        return true;
    }

    private static bool TryParseOkpoPlainEight(string? raw, out string eight)
    {
        eight = string.Empty;
        if (string.IsNullOrWhiteSpace(raw))
        {
            return false;
        }

        var trimmed = raw.Trim();
        if (trimmed.Length != 8)
        {
            return false;
        }

        for (var i = 0; i < 8; i++)
        {
            if (!char.IsDigit(trimmed[i]))
            {
                return false;
            }
        }

        eight = trimmed;
        return true;
    }

    /// <summary>
    /// Совпадение ссылки ОКПО (кол. 19 → org): полное после NormalizeNumber
    /// или «ровно 8 цифр» ↔ голова расширенного (8_5 / 14+).
    /// Два разных полных 8_5/14 с одной головой — не совпадение ссылки.
    /// </summary>
    private static bool OkpoReferencesMatch(string? claimedRaw, string? targetRaw)
    {
        var claimedNorm = NormalizeNumber(claimedRaw);
        var targetNorm = NormalizeNumber(targetRaw);
        if (claimedNorm.Length > 0 && claimedNorm == targetNorm)
        {
            return true;
        }

        return OkpoIsEightPrefixOfExtended(claimedRaw, targetRaw)
               || OkpoIsEightPrefixOfExtended(targetRaw, claimedRaw);
    }

    /// <summary>
    /// true, если у обоих значений извлекается одна и та же голова из 8 цифр
    /// (plain 8, голова 8_5 или префикс 14+). Для отбора org / индексации пула.
    /// </summary>
    private static bool OkpoSharesHead8(string? leftRaw, string? rightRaw) =>
        TryGetOkpoHead8(leftRaw, out var leftHead)
        && TryGetOkpoHead8(rightRaw, out var rightHead)
        && (leftHead == rightHead
            || string.Equals(NormalizeNumber(leftHead), NormalizeNumber(rightHead), StringComparison.Ordinal));

    /// <summary>
    /// true, если left — ровно 8 цифр, а right — расширенный ОКПО (8_5 или 14+) с той же головой.
    /// </summary>
    private static bool OkpoIsEightPrefixOfExtended(string? eightRaw, string? extendedRaw)
    {
        if (!TryParseOkpoPlainEight(eightRaw, out var eight)
            || !TryGetOkpoHead8(extendedRaw, out var head8))
        {
            return false;
        }

        // Расширенный: 8_5 или ≥9 цифр (не plain 8).
        if (TryParseOkpoPlainEight(extendedRaw, out _))
        {
            return false;
        }

        return eight == head8
               || string.Equals(NormalizeNumber(eight), NormalizeNumber(head8), StringComparison.Ordinal);
    }

    /// <summary>Claim кол. 19 ↔ ОКПО титула для отбора org: full или общие первые 8.</summary>
    private static bool OkpoClaimMatchesOrgOkpo(string? claimRaw, string? orgOkpoRaw) =>
        OkpoReferencesMatch(claimRaw, orgOkpoRaw) || OkpoSharesHead8(claimRaw, orgOkpoRaw);

    /// <summary>
    /// Голова из 8 цифр: plain 8, голова формата 8_5 или первые 8 цифр длинного номера.
    /// </summary>
    private static bool TryGetOkpoHead8(string? raw, out string head8)
    {
        head8 = string.Empty;
        if (!IsOkpoValuePresent(raw))
        {
            return false;
        }

        if (TryParseOkpoEightUnderscoreFive(raw, out var fromExtended, out _))
        {
            head8 = fromExtended;
            return true;
        }

        if (TryParseOkpoPlainEight(raw, out var eight))
        {
            head8 = eight;
            return true;
        }

        var digits = ExtractOkpoDigits(raw);
        if (digits.Length < 8)
        {
            return false;
        }

        head8 = digits[..8];
        return true;
    }

    /// <summary>Только цифры из сырого ОКПО (без нормализации lookalike / ведущих нулей).</summary>
    private static string ExtractOkpoDigits(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        var trimmed = raw.Trim();
        var buffer = new char[trimmed.Length];
        var n = 0;
        foreach (var ch in trimmed)
        {
            if (char.IsDigit(ch))
            {
                buffer[n++] = ch;
            }
        }

        return n == 0 ? string.Empty : new string(buffer, 0, n);
    }

    private static bool IsOkpoValuePresent(string? raw) =>
        !string.IsNullOrWhiteSpace(raw) && raw.Trim() != "-";

    /// <summary>
    /// Ключи для индекса/поиска пула по ОКПО: полная нормализация + голова из 8 (сырая и norm).
    /// </summary>
    private static IEnumerable<string> OkpoIndexKeys(string? okpoRaw)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);

        var norm = NormalizeNumber(okpoRaw);
        if (norm.Length > 0 && seen.Add(norm))
        {
            yield return norm;
        }

        if (!TryGetOkpoHead8(okpoRaw, out var head8))
        {
            yield break;
        }

        if (seen.Add(head8))
        {
            yield return head8;
        }

        var headNorm = NormalizeNumber(head8);
        if (headNorm.Length > 0 && seen.Add(headNorm))
        {
            yield return headNorm;
        }
    }

    /// <summary>Все ключи индекса для юрлица и обособленного подразделения.</summary>
    private static IEnumerable<string> OkpoIndexKeysForTitle(OrgTitleInfo title)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var raw in TitleOkpoValues(title))
        {
            foreach (var key in OkpoIndexKeys(raw))
            {
                if (seen.Add(key))
                {
                    yield return key;
                }
            }
        }
    }

    private static IEnumerable<string> TitleOkpoValues(OrgTitleInfo title)
    {
        if (IsOkpoValuePresent(title.LegalOkpo))
        {
            yield return title.LegalOkpo.Trim();
        }

        if (IsOkpoValuePresent(title.BranchOkpo))
        {
            yield return title.BranchOkpo.Trim();
        }

        // Display на случай старых вызовов / тестов, где Legal/Branch не заполнены.
        if (IsOkpoValuePresent(title.Okpo)
            && !string.Equals(title.Okpo.Trim(), title.LegalOkpo.Trim(), StringComparison.Ordinal)
            && !string.Equals(title.Okpo.Trim(), title.BranchOkpo.Trim(), StringComparison.Ordinal))
        {
            yield return title.Okpo.Trim();
        }
    }

    /// <summary>Claim из кол. 19 подходит к любому ОКПО титула (юрлицо / филиал).</summary>
    private static bool OkpoClaimMatchesTitle(string? claimRaw, OrgTitleInfo title)
    {
        if (!IsOkpoValuePresent(claimRaw))
        {
            return false;
        }

        foreach (var orgOkpo in TitleOkpoValues(title))
        {
            if (OkpoClaimMatchesOrgOkpo(claimRaw, orgOkpo))
            {
                return true;
            }
        }

        return false;
    }

    private static string NormalizeRads(string? value)
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

    private static string NormalizeDate(string? value) =>
        DateOnly.TryParse(value, out var date)
            ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            : NormalizeNumber(value);

    private static bool DateWithinTolerance(string? left, string? right, int days = DefaultOperationDateSearchToleranceDays)
    {
        if (DateOnly.TryParse(left, out var leftDate) && DateOnly.TryParse(right, out var rightDate))
        {
            return Math.Abs(leftDate.DayNumber - rightDate.DayNumber) <= days;
        }

        return string.Equals(NormalizeDate(left), NormalizeDate(right), StringComparison.Ordinal);
    }

    /// <summary>Точное совпадение даты операции (для пары и подсветки; ±N дней — только фильтр поиска).</summary>
    private static bool DatesEqualExact(string? left, string? right)
    {
        if (DateOnly.TryParse(left, out var leftDate) && DateOnly.TryParse(right, out var rightDate))
        {
            return leftDate == rightDate;
        }

        return string.Equals(NormalizeDate(left), NormalizeDate(right), StringComparison.Ordinal);
    }

    private static int GetQuantityForComparison(TransferReceiveDto row) =>
        row.Quantity is > 0 ? row.Quantity.Value : 1;

    /// <summary>Количество ОЗИИИ (форма 1.6, строка): «-» / «прим.» / пусто — как отсутствие числа.</summary>
    private static int? ParseQuantityOziii(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw) || raw is "-" or "прим.")
        {
            return null;
        }

        return int.TryParse(raw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
            ? value
            : null;
    }

    private static bool TryParseActivity(string? value, out double activity) =>
        FormExponentialEquality.TryParse(value, out activity);

    private static bool ActivityMatches(TransferReceiveDto left, TransferReceiveDto right, bool checkActivity)
    {
        if (!checkActivity)
        {
            return true;
        }

        return ActivityMatches(left.Activity, right.Activity, checkActivity);
    }

    private static bool ActivityMatches(string? leftRaw, string? rightRaw, bool checkActivity)
    {
        if (!checkActivity)
        {
            return true;
        }

        return NumericWithinRelativeTolerance(leftRaw, rightRaw, relativeTolerance: 0.10);
    }
    private static bool MassMatches(TransferReceiveDto left, TransferReceiveDto right, bool checkMass)
    {
        if (!checkMass)
        {
            return true;
        }

        return NumericWithinRelativeTolerance(left.Mass, right.Mass, relativeTolerance: 0.10);
    }

    /// <summary>Объём (куб. м, форма 1.4): допуск ±10%, как у массы/активности.</summary>
    private static bool VolumeMatches(TransferReceiveDto left, TransferReceiveDto right, bool checkVolume)
    {
        if (!checkVolume)
        {
            return true;
        }

        return NumericWithinRelativeTolerance(left.Volume, right.Volume, relativeTolerance: 0.10);
    }

    private static bool NumericWithinRelativeTolerance(string? leftRaw, string? rightRaw, double relativeTolerance) =>
        SoftSimilarityCore.NumericMatchesWithTolerance(
            leftRaw,
            rightRaw,
            relativeTolerance,
            (left, right) => string.Equals(
                NormalizeNumber(left),
                NormalizeNumber(right),
                StringComparison.Ordinal));

    private static bool IsStatusRaoExemptOpCode(string? opCode) =>
        opCode is "28" or "38";

    private static bool SubsidyMatches(TransferReceiveDto left, TransferReceiveDto right, bool checkSubsidy)
    {
        if (!checkSubsidy)
        {
            return true;
        }

        if (SoftSimilarityCore.IsSubsidyAbsentOrZero(left.Subsidy)
            && SoftSimilarityCore.IsSubsidyAbsentOrZero(right.Subsidy))
        {
            return true;
        }

        var leftNorm = NormalizeNumber(left.Subsidy);
        var rightNorm = NormalizeNumber(right.Subsidy);
        if (string.Equals(leftNorm, rightNorm, StringComparison.Ordinal))
        {
            return true;
        }

        return SoftSimilarityCore.SubsidyMatches(left.Subsidy, right.Subsidy);
    }

    private static bool FcpNumberMatches(TransferReceiveDto left, TransferReceiveDto right, bool checkFcpNumber)
    {
        if (!checkFcpNumber)
        {
            return true;
        }

        return string.Equals(
            NormalizeSerialNumber(left.FcpNumber),
            NormalizeSerialNumber(right.FcpNumber),
            StringComparison.Ordinal);
    }

    /// <summary>
    /// Ключ учётной единицы с учётом включённых параметров.
    /// ОКПО поставщика/получателя в ключ не входит (сравнивается отдельно как обратная ссылка).
    /// Активность — отдельно с допуском ±10%.
    /// </summary>
    private static string BuildPairingKey(
        TransferReceiveDto row,
        TransferReceiveFormParams options,
        bool includeSerial,
        bool includeQuantity)
    {
        var parts = new List<string>(10);
        if (includeSerial && options.CheckPassportNumber) parts.Add(NormalizeSerialNumber(row.PasNum));
        if (options.CheckType) parts.Add(NormalizeNumber(row.Type));
        if (options.CheckRadionuclids) parts.Add(NormalizeRads(row.Radionuclids));
        if (includeSerial && options.CheckFactoryNumber) parts.Add(NormalizeSerialNumber(row.FacNum));
        if (options.CheckCodeRao) parts.Add(NormalizeNumber(row.CodeRao));
        if (options.CheckPackType) parts.Add(NormalizeNumber(row.PackType));
        if (options.CheckPackName) parts.Add(NormalizeNumber(row.PackName));
        if (options.CheckPackNumber) parts.Add(NormalizeNumber(row.PackNumber));
        if (options.CheckStatusRao && !IsStatusRaoExemptOpCode(row.OpCode))
        {
            parts.Add(NormalizeNumber(row.StatusRao));
        }

        if (options.CheckFcpNumber)
        {
            parts.Add(NormalizeSerialNumber(row.FcpNumber));
        }

        if (options.CheckCreatorOkpo) parts.Add(NormalizeNumber(row.CreatorOkpo));
        if (options.CheckCreationDate) parts.Add(NormalizeDate(row.CreationDate));
        if (includeQuantity && options.CheckQuantity)
        {
            parts.Add(GetQuantityForComparison(row).ToString(CultureInfo.InvariantCulture));
        }

        if (options.CheckAggregateState)
        {
            parts.Add(row.AggregateState?.ToString(CultureInfo.InvariantCulture) ?? string.Empty);
        }

        if (options.CheckSort)
        {
            parts.Add(row.Sort?.ToString(CultureInfo.InvariantCulture) ?? string.Empty);
        }

        if (options.CheckActivityMeasurementDate)
        {
            parts.Add(NormalizeDate(row.ActivityMeasurementDate));
        }

        return string.Join('|', parts);
    }

    private static bool IsPairMatch(
        TransferReceiveDto source,
        TransferReceiveDto candidate,
        TransferReceiveFormParams options,
        string ourOkpoRaw,
        bool includeSerial,
        string? precomputedSourceKey = null,
        string? precomputedCandidateKey = null)
    {
        if (options.CheckOperationCode && !OpCodesArePaired(source.OpCode, candidate.OpCode))
        {
            return false;
        }

        if (options.CheckOperationDate && !DatesEqualExact(source.OpDate, candidate.OpDate))
        {
            return false;
        }

        var includeQuantity = includeSerial && options.CheckQuantity;
        var sourceKey = precomputedSourceKey
                        ?? BuildPairingKey(source, options, includeSerial, includeQuantity);
        var candidateKey = precomputedCandidateKey
                           ?? BuildPairingKey(candidate, options, includeSerial, includeQuantity);
        if (!string.Equals(sourceKey, candidateKey, StringComparison.Ordinal))
        {
            return false;
        }

        if (!ActivityMatches(source, candidate, options.CheckActivity))
        {
            return false;
        }

        if (!MassMatches(source, candidate, options.CheckMass))
        {
            return false;
        }

        if (!VolumeMatches(source, candidate, options.CheckVolume))
        {
            return false;
        }

        if (!ActivityMatches(
                source.TritiumActivity, candidate.TritiumActivity, options.CheckTritiumActivity))
        {
            return false;
        }

        if (!ActivityMatches(
                source.BetaGammaActivity, candidate.BetaGammaActivity, options.CheckBetaGammaActivity))
        {
            return false;
        }

        if (!ActivityMatches(
                source.AlphaActivity, candidate.AlphaActivity, options.CheckAlphaActivity))
        {
            return false;
        }

        if (!ActivityMatches(
                source.TransuraniumActivity, candidate.TransuraniumActivity, options.CheckTransuraniumActivity))
        {
            return false;
        }

        if (!SubsidyMatches(source, candidate, options.CheckSubsidy))
        {
            return false;
        }

        if (!FcpNumberMatches(source, candidate, options.CheckFcpNumber))
        {
            return false;
        }

        if (options.CheckProviderOrRecieverOkpo)
        {
            // Кол.19 контрагента → на нас: display / юрлицо / филиал (full или общие первые 8).
            if (!OkpoReferencesMatchAny(
                    candidate.ProviderOrRecieverOkpo,
                    ourOkpoRaw,
                    source.OrgOkpo,
                    source.OrgLegalOkpo,
                    source.OrgBranchOkpo))
            {
                return false;
            }
        }

        return true;
    }

    private static bool OkpoReferencesMatchAny(string? claimedRaw, params string?[] targets)
    {
        foreach (var target in targets)
        {
            if (OkpoReferencesMatch(claimedRaw, target))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Load DTO

    /// <summary>
    /// Коды передачи/приёма форм 1.1–1.4.
    /// Без 26/36 — эта пара ожидается только на формах 1.5–1.8.
    /// </summary>
    private static readonly string[] TransferReceiveOpCodes =
    [
        "21", "22", "25", "27", "28", "29",
        "31", "32", "35", "37", "38", "39"
    ];

    /// <summary>Коды передачи/приёма формы 1.5 (включая 26↔36).</summary>
    private static readonly string[] TransferReceiveOpCodesForm15 =
    [
        "21", "22", "25", "26", "27", "28", "29",
        "31", "32", "35", "36", "37", "38", "39"
    ];

    /// <summary>Диспетчер bulk-загрузки по форме.</summary>
    private static Task<List<TransferReceiveDto>> LoadAllFormOpsAsync(
        TransferReceiveFormId formId,
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken) =>
        formId switch
        {
            TransferReceiveFormId.Form11 => LoadAllForm11TransferReceiveAsync(db, orgTitles, cancellationToken),
            TransferReceiveFormId.Form12 => LoadAllForm12TransferReceiveAsync(db, orgTitles, cancellationToken),
            TransferReceiveFormId.Form13 => LoadAllForm13TransferReceiveAsync(db, orgTitles, cancellationToken),
            TransferReceiveFormId.Form14 => LoadAllForm14TransferReceiveAsync(db, orgTitles, cancellationToken),
            TransferReceiveFormId.Form15 => LoadAllForm15TransferReceiveAsync(db, orgTitles, cancellationToken),
            TransferReceiveFormId.Form16 => LoadAllForm16TransferReceiveAsync(db, orgTitles, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет bulk-загрузчика для формы.")
        };

    /// <summary>Диспетчер точечной загрузки своих ops.</summary>
    private static Task<List<TransferReceiveDto>> LoadFormOpsForRepsAsync(
        TransferReceiveFormId formId,
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken) =>
        formId switch
        {
            TransferReceiveFormId.Form11 => LoadForm11TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            TransferReceiveFormId.Form12 => LoadForm12TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            TransferReceiveFormId.Form13 => LoadForm13TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            TransferReceiveFormId.Form14 => LoadForm14TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            TransferReceiveFormId.Form15 => LoadForm15TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            TransferReceiveFormId.Form16 => LoadForm16TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет загрузчика для формы.")
        };

    /// <summary>Диспетчер загрузки ops контрагентов (все операции org, без фильтра по кол.19).</summary>
    private static Task<List<TransferReceiveDto>> LoadFormOpsForRepsIdsAsync(
        TransferReceiveFormId formId,
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "контрагентов") =>
        formId switch
        {
            TransferReceiveFormId.Form11 => LoadForm11TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, chunkSize, progressEntityLabel),
            TransferReceiveFormId.Form12 => LoadForm12TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, chunkSize, progressEntityLabel),
            TransferReceiveFormId.Form13 => LoadForm13TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, chunkSize, progressEntityLabel),
            TransferReceiveFormId.Form14 => LoadForm14TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, chunkSize, progressEntityLabel),
            TransferReceiveFormId.Form15 => LoadForm15TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, chunkSize, progressEntityLabel),
            TransferReceiveFormId.Form16 => LoadForm16TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, chunkSize, progressEntityLabel),
            _ => throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет загрузчика для формы.")
        };

    /// <summary>
    /// Whole-DB: один проход по строкам формы keyset-пагинацией (Id &gt; lastId Take N).
    /// Титулы подставляются позже; прогресс — после каждой страницы.
    /// </summary>
    private static Task<List<TransferReceiveDto>> LoadFormOpsPagedWholeDbAsync(
        TransferReceiveFormId formId,
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum) =>
        formId switch
        {
            TransferReceiveFormId.Form11 => LoadForm11TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            TransferReceiveFormId.Form12 => LoadForm12TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            TransferReceiveFormId.Form13 => LoadForm13TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            TransferReceiveFormId.Form14 => LoadForm14TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            TransferReceiveFormId.Form15 => LoadForm15TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            TransferReceiveFormId.Form16 => LoadForm16TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            _ => throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет paged-загрузчика для формы.")
        };

    private static async Task<List<TransferReceiveDto>> LoadForm11TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodes;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 5000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_11
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new
                {
                    form.Id,
                    ReportId = form.ReportId ?? 0,
                    RepsId = form.Report!.Reports.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    FacNum = form.FactoryNumber_DB,
                    Type = form.Type_DB,
                    Radionuclids = form.Radionuclids_DB,
                    PackNumber = form.PackNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    Quantity = form.Quantity_DB,
                    Activity = form.Activity_DB,
                    CreatorOkpo = form.CreatorOKPO_DB,
                    CreationDate = form.CreationDate_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(new TransferReceiveDto
                {
                    Id = row.Id,
                    RepsId = row.RepsId,
                    ReportId = row.ReportId,
                    NumberInOrder = row.NumberInOrder,
                    OpCode = row.OpCode ?? string.Empty,
                    OpDate = row.OpDate ?? string.Empty,
                    PasNum = row.PasNum ?? string.Empty,
                    FacNum = row.FacNum ?? string.Empty,
                    Type = row.Type ?? string.Empty,
                    Radionuclids = row.Radionuclids ?? string.Empty,
                    PackNumber = row.PackNumber ?? string.Empty,
                    ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                    Quantity = row.Quantity,
                    Activity = row.Activity ?? string.Empty,
                    CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                    CreationDate = row.CreationDate ?? string.Empty,
                    StartPeriod = row.StartPeriod ?? string.Empty,
                    EndPeriod = row.EndPeriod ?? string.Empty,
                    IsTransfer = IsTransferCodeForm11(row.OpCode)
                });
            }

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    private static async Task<List<TransferReceiveDto>> LoadForm12TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodes;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 5000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_12
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new
                {
                    form.Id,
                    ReportId = form.ReportId ?? 0,
                    RepsId = form.Report!.Reports.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    FacNum = form.FactoryNumber_DB,
                    Name = form.NameIOU_DB,
                    Mass = form.Mass_DB,
                    PackType = form.PackType_DB,
                    PackNumber = form.PackNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    CreatorOkpo = form.CreatorOKPO_DB,
                    CreationDate = form.CreationDate_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(new TransferReceiveDto
                {
                    Id = row.Id,
                    RepsId = row.RepsId,
                    ReportId = row.ReportId,
                    NumberInOrder = row.NumberInOrder,
                    OpCode = row.OpCode ?? string.Empty,
                    OpDate = row.OpDate ?? string.Empty,
                    PasNum = row.PasNum ?? string.Empty,
                    FacNum = row.FacNum ?? string.Empty,
                    Type = row.Name ?? string.Empty,
                    PackType = row.PackType ?? string.Empty,
                    PackNumber = row.PackNumber ?? string.Empty,
                    ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                    Quantity = 1,
                    Mass = row.Mass ?? string.Empty,
                    CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                    CreationDate = row.CreationDate ?? string.Empty,
                    StartPeriod = row.StartPeriod ?? string.Empty,
                    EndPeriod = row.EndPeriod ?? string.Empty,
                    IsTransfer = IsTransferCodeForm11(row.OpCode)
                });
            }

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    private static async Task<List<TransferReceiveDto>> LoadForm13TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodes;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 5000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_13
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new
                {
                    form.Id,
                    ReportId = form.ReportId ?? 0,
                    RepsId = form.Report!.Reports.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    FacNum = form.FactoryNumber_DB,
                    Type = form.Type_DB,
                    Radionuclids = form.Radionuclids_DB,
                    PackNumber = form.PackNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    Activity = form.Activity_DB,
                    CreatorOkpo = form.CreatorOKPO_DB,
                    CreationDate = form.CreationDate_DB,
                    AggregateState = form.AggregateState_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(new TransferReceiveDto
                {
                    Id = row.Id,
                    RepsId = row.RepsId,
                    ReportId = row.ReportId,
                    NumberInOrder = row.NumberInOrder,
                    OpCode = row.OpCode ?? string.Empty,
                    OpDate = row.OpDate ?? string.Empty,
                    PasNum = row.PasNum ?? string.Empty,
                    FacNum = row.FacNum ?? string.Empty,
                    Type = row.Type ?? string.Empty,
                    Radionuclids = row.Radionuclids ?? string.Empty,
                    PackNumber = row.PackNumber ?? string.Empty,
                    ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                    Quantity = 1,
                    AggregateState = row.AggregateState,
                    Activity = row.Activity ?? string.Empty,
                    CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                    CreationDate = row.CreationDate ?? string.Empty,
                    StartPeriod = row.StartPeriod ?? string.Empty,
                    EndPeriod = row.EndPeriod ?? string.Empty,
                    IsTransfer = IsTransferCodeForm11(row.OpCode)
                });
            }

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    private static async Task<List<TransferReceiveDto>> LoadForm14TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodes;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 5000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_14
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new
                {
                    form.Id,
                    ReportId = form.ReportId ?? 0,
                    RepsId = form.Report!.Reports.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    Name = form.Name_DB,
                    Sort = form.Sort_DB,
                    Radionuclids = form.Radionuclids_DB,
                    Activity = form.Activity_DB,
                    ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                    Volume = form.Volume_DB,
                    Mass = form.Mass_DB,
                    AggregateState = form.AggregateState_DB,
                    PackNumber = form.PackNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(MapForm14Row(
                    row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                    string.Empty, string.Empty, string.Empty,
                    row.OpCode, row.OpDate, row.PasNum, row.Name, row.Sort, row.Radionuclids,
                    row.Activity, row.ActivityMeasurementDate, row.Volume, row.Mass, row.AggregateState,
                    row.PackNumber, row.ProviderOrRecieverOkpo, row.StartPeriod, row.EndPeriod));
            }

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    private static async Task<List<TransferReceiveDto>> LoadForm15TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodesForm15;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 5000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_15
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new
                {
                    form.Id,
                    ReportId = form.ReportId ?? 0,
                    RepsId = form.Report!.Reports.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    FacNum = form.FactoryNumber_DB,
                    Type = form.Type_DB,
                    Radionuclids = form.Radionuclids_DB,
                    StatusRao = form.StatusRAO_DB,
                    PackName = form.PackName_DB,
                    PackType = form.PackType_DB,
                    PackNumber = form.PackNumber_DB,
                    Subsidy = form.Subsidy_DB,
                    FcpNumber = form.FcpNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    Quantity = form.Quantity_DB,
                    Activity = form.Activity_DB,
                    CreationDate = form.CreationDate_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(MapForm15Row(
                    row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                    string.Empty, string.Empty, string.Empty,
                    row.OpCode, row.OpDate, row.PasNum, row.FacNum, row.Type, row.Radionuclids,
                    row.StatusRao, row.PackName, row.PackType, row.PackNumber, row.Subsidy, row.FcpNumber,
                    row.ProviderOrRecieverOkpo, row.Quantity, row.Activity, row.CreationDate,
                    row.StartPeriod, row.EndPeriod));
            }

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Сырые варианты нашего ОКПО для SQL IN: как в титуле, нормализованный, с ведущими нулями.
    /// Не покрывает lookalike-буквы — для ОКПО обычно цифры.
    /// </summary>
    private static List<string> BuildOurOkpoSqlMatchVariants(string ourOkpoRaw)
    {
        var result = new HashSet<string>(StringComparer.Ordinal);
        void Add(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "-")
            {
                return;
            }

            result.Add(value.Trim());
        }

        void AddWithLeadingZeroPads(string? value)
        {
            Add(value);
            var norm = NormalizeNumber(value);
            Add(norm);
            if (norm.Length is > 0 and <= 12)
            {
                for (var pad = 1; pad <= 4; pad++)
                {
                    result.Add(norm.PadLeft(norm.Length + pad, '0'));
                }
            }
        }

        AddWithLeadingZeroPads(ourOkpoRaw);
        // Контрагент может указать только первые 8 цифр (голова 8_5 / префикс 14+).
        if (TryGetOkpoHead8(ourOkpoRaw, out var head8))
        {
            AddWithLeadingZeroPads(head8);
        }

        return result.ToList();
    }

    /// <summary>
    /// Сырые варианты claim кол. 19 для SQL IN: full + head8 + pads (без lookalike).
    /// </summary>
    private static List<string> BuildClaimOkpoSqlMatchVariants(IReadOnlyList<string> rawOkpos)
    {
        var result = new HashSet<string>(StringComparer.Ordinal);
        foreach (var raw in rawOkpos)
        {
            foreach (var variant in BuildOurOkpoSqlMatchVariants(raw))
            {
                result.Add(variant);
            }
        }

        return result.ToList();
    }

    private static bool CounterpartProviderPointsToUs(string? providerRaw, string? ourOkpoRaw) =>
        !string.IsNullOrWhiteSpace(ourOkpoRaw) && OkpoReferencesMatch(providerRaw, ourOkpoRaw);

    /// <summary>
    /// Один запрос: все операции приёма/передачи формы 1.1 по БД (RepsId из навигации).
    /// </summary>
    private static async Task<List<TransferReceiveDto>> LoadAllForm11TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_11
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                RepsId = form.Report!.Reports.Id,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                Quantity = form.Quantity_DB,
                Activity = form.Activity_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                StartPeriod = form.Report.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
            result.Add(new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = row.RepsId,
                ReportId = row.ReportId,
                NumberInOrder = row.NumberInOrder,
                OrgOkpo = title?.Okpo ?? string.Empty,
                OrgLegalOkpo = title?.LegalOkpo ?? string.Empty,
                OrgBranchOkpo = title?.BranchOkpo ?? string.Empty,
                OrgRegNo = title?.RegNo ?? string.Empty,
                OrgShortName = title?.ShortName ?? string.Empty,
                OpCode = row.OpCode ?? string.Empty,
                OpDate = row.OpDate ?? string.Empty,
                PasNum = row.PasNum ?? string.Empty,
                FacNum = row.FacNum ?? string.Empty,
                Type = row.Type ?? string.Empty,
                Radionuclids = row.Radionuclids ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = row.Quantity,
                Activity = row.Activity ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            });
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>
    /// Один запрос: все операции приёма/передачи формы 1.2 по БД.
    /// NameIOU → Type, Mass → Mass, PackType → PackType; количество всегда 1.
    /// </summary>
    private static async Task<List<TransferReceiveDto>> LoadAllForm12TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_12
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                RepsId = form.Report!.Reports.Id,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Name = form.NameIOU_DB,
                Mass = form.Mass_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                StartPeriod = form.Report.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
            result.Add(new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = row.RepsId,
                ReportId = row.ReportId,
                NumberInOrder = row.NumberInOrder,
                OrgOkpo = title?.Okpo ?? string.Empty,
                OrgLegalOkpo = title?.LegalOkpo ?? string.Empty,
                OrgBranchOkpo = title?.BranchOkpo ?? string.Empty,
                OrgRegNo = title?.RegNo ?? string.Empty,
                OrgShortName = title?.ShortName ?? string.Empty,
                OpCode = row.OpCode ?? string.Empty,
                OpDate = row.OpDate ?? string.Empty,
                PasNum = row.PasNum ?? string.Empty,
                FacNum = row.FacNum ?? string.Empty,
                Type = row.Name ?? string.Empty,
                PackType = row.PackType ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = 1,
                Mass = row.Mass ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            });
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>
    /// Один запрос: все операции приёма/передачи формы 1.3 по БД.
    /// </summary>
    private static async Task<List<TransferReceiveDto>> LoadAllForm13TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_13
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                RepsId = form.Report!.Reports.Id,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                Activity = form.Activity_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                AggregateState = form.AggregateState_DB,
                StartPeriod = form.Report.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
            result.Add(new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = row.RepsId,
                ReportId = row.ReportId,
                NumberInOrder = row.NumberInOrder,
                OrgOkpo = title?.Okpo ?? string.Empty,
                OrgLegalOkpo = title?.LegalOkpo ?? string.Empty,
                OrgBranchOkpo = title?.BranchOkpo ?? string.Empty,
                OrgRegNo = title?.RegNo ?? string.Empty,
                OrgShortName = title?.ShortName ?? string.Empty,
                OpCode = row.OpCode ?? string.Empty,
                OpDate = row.OpDate ?? string.Empty,
                PasNum = row.PasNum ?? string.Empty,
                FacNum = row.FacNum ?? string.Empty,
                Type = row.Type ?? string.Empty,
                Radionuclids = row.Radionuclids ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = 1,
                AggregateState = row.AggregateState,
                Activity = row.Activity ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            });
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>Один запрос: все операции приёма/передачи формы 1.4 по БД.</summary>
    private static async Task<List<TransferReceiveDto>> LoadAllForm14TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_14
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                RepsId = form.Report!.Reports.Id,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                Name = form.Name_DB,
                Sort = form.Sort_DB,
                Radionuclids = form.Radionuclids_DB,
                Activity = form.Activity_DB,
                ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                Volume = form.Volume_DB,
                Mass = form.Mass_DB,
                AggregateState = form.AggregateState_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                StartPeriod = form.Report.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
            result.Add(WithOrgTitleOkpos(MapForm14Row(
                row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                title?.Okpo ?? string.Empty, title?.RegNo ?? string.Empty, title?.ShortName ?? string.Empty,
                row.OpCode, row.OpDate, row.PasNum, row.Name, row.Sort, row.Radionuclids,
                row.Activity, row.ActivityMeasurementDate, row.Volume, row.Mass, row.AggregateState,
                row.PackNumber, row.ProviderOrRecieverOkpo, row.StartPeriod, row.EndPeriod), title));
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>
    /// Whole-DB ОКПО-карта: сначала все загруженные титулы (юрлицо + филиал);
    /// <see cref="LoadRepsIdsByOkpoAsync"/> — только для claim кол. 19 без пересечения ключей.
    /// </summary>
    private static async Task<Dictionary<string, List<int>>> BuildOkpoAliasMapForWholeDbAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        IReadOnlyList<TransferReceiveDto> allOps,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null)
    {
        var byNorm = SeedOkpoAliasMapFromTitles(orgTitles);

        var counterpartRawOkpos = allOps
            .Select(op => op.ProviderOrRecieverOkpo?.Trim() ?? string.Empty)
            .Where(IsOkpoValuePresent)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var missingRaw = counterpartRawOkpos
            .Where(raw => !OkpoIndexKeys(raw).Any(byNorm.ContainsKey))
            .ToList();

        if (missingRaw.Count == 0)
        {
            progress?.Status($"ОКПО — все {counterpartRawOkpos.Count} закрыты титулами");
            return byNorm;
        }

        progress?.Status(
            $"ОКПО — догрузка хвоста {missingRaw.Count} из {counterpartRawOkpos.Count}…");
        var (extra, _) = await LoadRepsIdsByOkpoAsync(db, missingRaw, cancellationToken, progress);
        foreach (var (norm, repsIds) in extra)
        {
            if (!byNorm.TryGetValue(norm, out var list))
            {
                byNorm[norm] = repsIds.ToList();
                continue;
            }

            foreach (var repsId in repsIds)
            {
                if (!list.Contains(repsId))
                {
                    list.Add(repsId);
                }
            }
        }

        return byNorm;
    }

    /// <summary>ОКПО титула (юрлицо + филиал) → RepsId; ключи full + голова из 8.</summary>
    private static Dictionary<string, List<int>> SeedOkpoAliasMapFromTitles(
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles)
    {
        var byNorm = new Dictionary<string, List<int>>(StringComparer.Ordinal);
        foreach (var (repsId, title) in orgTitles)
        {
            IndexRepsUnderOkpoKeys(byNorm, repsId, OkpoIndexKeysForTitle(title));
        }

        return byNorm;
    }

    private static void IndexRepsUnderOkpoKeys(
        Dictionary<string, List<int>> byNorm,
        int repsId,
        IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            if (key.Length == 0)
            {
                continue;
            }

            if (!byNorm.TryGetValue(key, out var list))
            {
                list = [];
                byNorm[key] = list;
            }

            if (!list.Contains(repsId))
            {
                list.Add(repsId);
            }
        }
    }

    private static async Task<List<TransferReceiveDto>> LoadForm11TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        // Запрос через form_11 (без SelectMany по коллекциям) — Firebird не поддерживает APPLY.
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_11
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                Quantity = form.Quantity_DB,
                Activity = form.Activity_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                StartPeriod = form.Report!.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = repsId,
                ReportId = row.ReportId,
                NumberInOrder = row.NumberInOrder,
                OrgOkpo = orgOkpo,
                OpCode = row.OpCode ?? string.Empty,
                OpDate = row.OpDate ?? string.Empty,
                PasNum = row.PasNum ?? string.Empty,
                FacNum = row.FacNum ?? string.Empty,
                Type = row.Type ?? string.Empty,
                Radionuclids = row.Radionuclids ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = row.Quantity,
                Activity = row.Activity ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            })
            .Where(row => IsTransferOrReceiveCodeForm11(row.OpCode))
            .OrderBy(row => row.Id)
            .ToList();
    }

    /// <summary>
    /// Операции приёма/передачи формы 1.1 для набора организаций (без повторной загрузки «своей» org —
    /// её ops передаются отдельно и мержатся в пул в оркестрации).
    /// </summary>
    private static async Task<List<TransferReceiveDto>> LoadForm11TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodes;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_11
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            var pageSize = SelectedOrgOpsPageSize > 0 ? SelectedOrgOpsPageSize : 2500;
            var lastRowId = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rows = await query
                    .Where(form => form.Id > lastRowId)
                    .OrderBy(form => form.Id)
                    .Take(pageSize)
                    .Select(form => new
                    {
                        form.Id,
                        ReportId = form.ReportId ?? 0,
                        RepsId = form.Report!.Reports.Id,
                        NumberInOrder = form.NumberInOrder_DB,
                        OpCode = form.OperationCode_DB,
                        OpDate = form.OperationDate_DB,
                        PasNum = form.PassportNumber_DB,
                        FacNum = form.FactoryNumber_DB,
                        Type = form.Type_DB,
                        Radionuclids = form.Radionuclids_DB,
                        PackNumber = form.PackNumber_DB,
                        ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                        Quantity = form.Quantity_DB,
                        Activity = form.Activity_DB,
                        CreatorOkpo = form.CreatorOKPO_DB,
                        CreationDate = form.CreationDate_DB,
                        StartPeriod = form.Report.StartPeriod_DB,
                        EndPeriod = form.Report.EndPeriod_DB
                    })
                    .ToListAsync(cancellationToken);

                if (rows.Count == 0)
                {
                    break;
                }

                foreach (var row in rows)
                {
                    if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                    {
                        continue;
                    }

                    orgTitlesByRepsId.TryGetValue(row.RepsId, out var title);
                    result.Add(new TransferReceiveDto
                    {
                        Id = row.Id,
                        RepsId = row.RepsId,
                        ReportId = row.ReportId,
                        NumberInOrder = row.NumberInOrder,
                        OrgOkpo = title?.Okpo ?? string.Empty,
                        OrgLegalOkpo = title?.LegalOkpo ?? string.Empty,
                        OrgBranchOkpo = title?.BranchOkpo ?? string.Empty,
                        OrgRegNo = title?.RegNo ?? string.Empty,
                        OrgShortName = title?.ShortName ?? string.Empty,
                        OpCode = row.OpCode ?? string.Empty,
                        OpDate = row.OpDate ?? string.Empty,
                        PasNum = row.PasNum ?? string.Empty,
                        FacNum = row.FacNum ?? string.Empty,
                        Type = row.Type ?? string.Empty,
                        Radionuclids = row.Radionuclids ?? string.Empty,
                        PackNumber = row.PackNumber ?? string.Empty,
                        ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                        Quantity = row.Quantity,
                        Activity = row.Activity ?? string.Empty,
                        CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                        CreationDate = row.CreationDate ?? string.Empty,
                        StartPeriod = row.StartPeriod ?? string.Empty,
                        EndPeriod = row.EndPeriod ?? string.Empty,
                        IsTransfer = IsTransferCodeForm11(row.OpCode)
                    });
                }

                lastRowId = rows[^1].Id;
                if (rows.Count < pageSize)
                {
                    break;
                }
            }

            orgsDone += idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm12TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_12
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Name = form.NameIOU_DB,
                Mass = form.Mass_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                StartPeriod = form.Report!.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = repsId,
                ReportId = row.ReportId,
                NumberInOrder = row.NumberInOrder,
                OrgOkpo = orgOkpo,
                OpCode = row.OpCode ?? string.Empty,
                OpDate = row.OpDate ?? string.Empty,
                PasNum = row.PasNum ?? string.Empty,
                FacNum = row.FacNum ?? string.Empty,
                Type = row.Name ?? string.Empty,
                PackType = row.PackType ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = 1,
                Mass = row.Mass ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            })
            .Where(row => IsTransferOrReceiveCodeForm11(row.OpCode))
            .OrderBy(row => row.Id)
            .ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm12TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "1.2 контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodes;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_12
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            var pageSize = SelectedOrgOpsPageSize > 0 ? SelectedOrgOpsPageSize : 2500;
            var lastRowId = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rows = await query
                    .Where(form => form.Id > lastRowId)
                    .OrderBy(form => form.Id)
                    .Take(pageSize)
                    .Select(form => new
                    {
                        form.Id,
                        ReportId = form.ReportId ?? 0,
                        RepsId = form.Report!.Reports.Id,
                        NumberInOrder = form.NumberInOrder_DB,
                        OpCode = form.OperationCode_DB,
                        OpDate = form.OperationDate_DB,
                        PasNum = form.PassportNumber_DB,
                        FacNum = form.FactoryNumber_DB,
                        Name = form.NameIOU_DB,
                        Mass = form.Mass_DB,
                        PackType = form.PackType_DB,
                        PackNumber = form.PackNumber_DB,
                        ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                        CreatorOkpo = form.CreatorOKPO_DB,
                        CreationDate = form.CreationDate_DB,
                        StartPeriod = form.Report.StartPeriod_DB,
                        EndPeriod = form.Report.EndPeriod_DB
                    })
                    .ToListAsync(cancellationToken);

                if (rows.Count == 0)
                {
                    break;
                }

                foreach (var row in rows)
                {
                    if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                    {
                        continue;
                    }

                    orgTitlesByRepsId.TryGetValue(row.RepsId, out var title);
                    result.Add(new TransferReceiveDto
                    {
                        Id = row.Id,
                        RepsId = row.RepsId,
                        ReportId = row.ReportId,
                        NumberInOrder = row.NumberInOrder,
                        OrgOkpo = title?.Okpo ?? string.Empty,
                        OrgLegalOkpo = title?.LegalOkpo ?? string.Empty,
                        OrgBranchOkpo = title?.BranchOkpo ?? string.Empty,
                        OrgRegNo = title?.RegNo ?? string.Empty,
                        OrgShortName = title?.ShortName ?? string.Empty,
                        OpCode = row.OpCode ?? string.Empty,
                        OpDate = row.OpDate ?? string.Empty,
                        PasNum = row.PasNum ?? string.Empty,
                        FacNum = row.FacNum ?? string.Empty,
                        Type = row.Name ?? string.Empty,
                        PackType = row.PackType ?? string.Empty,
                        PackNumber = row.PackNumber ?? string.Empty,
                        ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                        Quantity = 1,
                        Mass = row.Mass ?? string.Empty,
                        CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                        CreationDate = row.CreationDate ?? string.Empty,
                        StartPeriod = row.StartPeriod ?? string.Empty,
                        EndPeriod = row.EndPeriod ?? string.Empty,
                        IsTransfer = IsTransferCodeForm11(row.OpCode)
                    });
                }

                lastRowId = rows[^1].Id;
                if (rows.Count < pageSize)
                {
                    break;
                }
            }

            orgsDone += idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm13TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_13
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                Activity = form.Activity_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                AggregateState = form.AggregateState_DB,
                StartPeriod = form.Report!.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = repsId,
                ReportId = row.ReportId,
                NumberInOrder = row.NumberInOrder,
                OrgOkpo = orgOkpo,
                OpCode = row.OpCode ?? string.Empty,
                OpDate = row.OpDate ?? string.Empty,
                PasNum = row.PasNum ?? string.Empty,
                FacNum = row.FacNum ?? string.Empty,
                Type = row.Type ?? string.Empty,
                Radionuclids = row.Radionuclids ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = 1,
                AggregateState = row.AggregateState,
                Activity = row.Activity ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            })
            .Where(row => IsTransferOrReceiveCodeForm11(row.OpCode))
            .OrderBy(row => row.Id)
            .ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm13TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "1.3 контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodes;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_13
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            var pageSize = SelectedOrgOpsPageSize > 0 ? SelectedOrgOpsPageSize : 2500;
            var lastRowId = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rows = await query
                    .Where(form => form.Id > lastRowId)
                    .OrderBy(form => form.Id)
                    .Take(pageSize)
                    .Select(form => new
                    {
                        form.Id,
                        ReportId = form.ReportId ?? 0,
                        RepsId = form.Report!.Reports.Id,
                        NumberInOrder = form.NumberInOrder_DB,
                        OpCode = form.OperationCode_DB,
                        OpDate = form.OperationDate_DB,
                        PasNum = form.PassportNumber_DB,
                        FacNum = form.FactoryNumber_DB,
                        Type = form.Type_DB,
                        Radionuclids = form.Radionuclids_DB,
                        PackNumber = form.PackNumber_DB,
                        ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                        Activity = form.Activity_DB,
                        CreatorOkpo = form.CreatorOKPO_DB,
                        CreationDate = form.CreationDate_DB,
                        AggregateState = form.AggregateState_DB,
                        StartPeriod = form.Report.StartPeriod_DB,
                        EndPeriod = form.Report.EndPeriod_DB
                    })
                    .ToListAsync(cancellationToken);

                if (rows.Count == 0)
                {
                    break;
                }

                foreach (var row in rows)
                {
                    if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                    {
                        continue;
                    }

                    orgTitlesByRepsId.TryGetValue(row.RepsId, out var title);
                    result.Add(new TransferReceiveDto
                    {
                        Id = row.Id,
                        RepsId = row.RepsId,
                        ReportId = row.ReportId,
                        NumberInOrder = row.NumberInOrder,
                        OrgOkpo = title?.Okpo ?? string.Empty,
                        OrgLegalOkpo = title?.LegalOkpo ?? string.Empty,
                        OrgBranchOkpo = title?.BranchOkpo ?? string.Empty,
                        OrgRegNo = title?.RegNo ?? string.Empty,
                        OrgShortName = title?.ShortName ?? string.Empty,
                        OpCode = row.OpCode ?? string.Empty,
                        OpDate = row.OpDate ?? string.Empty,
                        PasNum = row.PasNum ?? string.Empty,
                        FacNum = row.FacNum ?? string.Empty,
                        Type = row.Type ?? string.Empty,
                        Radionuclids = row.Radionuclids ?? string.Empty,
                        PackNumber = row.PackNumber ?? string.Empty,
                        ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                        Quantity = 1,
                        AggregateState = row.AggregateState,
                        Activity = row.Activity ?? string.Empty,
                        CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                        CreationDate = row.CreationDate ?? string.Empty,
                        StartPeriod = row.StartPeriod ?? string.Empty,
                        EndPeriod = row.EndPeriod ?? string.Empty,
                        IsTransfer = IsTransferCodeForm11(row.OpCode)
                    });
                }

                lastRowId = rows[^1].Id;
                if (rows.Count < pageSize)
                {
                    break;
                }
            }

            orgsDone += idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm14TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_14
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                Name = form.Name_DB,
                Sort = form.Sort_DB,
                Radionuclids = form.Radionuclids_DB,
                Activity = form.Activity_DB,
                ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                Volume = form.Volume_DB,
                Mass = form.Mass_DB,
                AggregateState = form.AggregateState_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                StartPeriod = form.Report!.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        return rows
            .Where(row => IsTransferOrReceiveCodeForm11(row.OpCode))
            .Select(row => MapForm14Row(
                row.Id, repsId, row.ReportId, row.NumberInOrder,
                orgOkpo, string.Empty, string.Empty,
                row.OpCode, row.OpDate, row.PasNum, row.Name, row.Sort, row.Radionuclids,
                row.Activity, row.ActivityMeasurementDate, row.Volume, row.Mass, row.AggregateState,
                row.PackNumber, row.ProviderOrRecieverOkpo, row.StartPeriod, row.EndPeriod))
            .OrderBy(row => row.Id)
            .ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm14TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "1.4 контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodes;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_14
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            var pageSize = SelectedOrgOpsPageSize > 0 ? SelectedOrgOpsPageSize : 2500;
            var lastRowId = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rows = await query
                    .Where(form => form.Id > lastRowId)
                    .OrderBy(form => form.Id)
                    .Take(pageSize)
                    .Select(form => new
                    {
                        form.Id,
                        ReportId = form.ReportId ?? 0,
                        RepsId = form.Report!.Reports.Id,
                        NumberInOrder = form.NumberInOrder_DB,
                        OpCode = form.OperationCode_DB,
                        OpDate = form.OperationDate_DB,
                        PasNum = form.PassportNumber_DB,
                        Name = form.Name_DB,
                        Sort = form.Sort_DB,
                        Radionuclids = form.Radionuclids_DB,
                        Activity = form.Activity_DB,
                        ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                        Volume = form.Volume_DB,
                        Mass = form.Mass_DB,
                        AggregateState = form.AggregateState_DB,
                        PackNumber = form.PackNumber_DB,
                        ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                        StartPeriod = form.Report.StartPeriod_DB,
                        EndPeriod = form.Report.EndPeriod_DB
                    })
                    .ToListAsync(cancellationToken);

                if (rows.Count == 0)
                {
                    break;
                }

                foreach (var row in rows)
                {
                    if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                    {
                        continue;
                    }

                    orgTitlesByRepsId.TryGetValue(row.RepsId, out var title);
                    result.Add(WithOrgTitleOkpos(MapForm14Row(
                        row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                        title?.Okpo ?? string.Empty, title?.RegNo ?? string.Empty, title?.ShortName ?? string.Empty,
                        row.OpCode, row.OpDate, row.PasNum, row.Name, row.Sort, row.Radionuclids,
                        row.Activity, row.ActivityMeasurementDate, row.Volume, row.Mass, row.AggregateState,
                        row.PackNumber, row.ProviderOrRecieverOkpo, row.StartPeriod, row.EndPeriod), title));
                }

                lastRowId = rows[^1].Id;
                if (rows.Count < pageSize)
                {
                    break;
                }
            }

            orgsDone += idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>Дописывает Legal/Branch с титула (Map* уже поставил display Okpo).</summary>
    private static TransferReceiveDto WithOrgTitleOkpos(TransferReceiveDto dto, OrgTitleInfo? title)
    {
        if (title is null)
        {
            if (string.IsNullOrEmpty(dto.OrgLegalOkpo))
            {
                dto.OrgLegalOkpo = dto.OrgOkpo;
            }

            return dto;
        }

        dto.OrgOkpo = title.Okpo;
        dto.OrgLegalOkpo = title.LegalOkpo;
        dto.OrgBranchOkpo = title.BranchOkpo;
        dto.OrgRegNo = title.RegNo;
        dto.OrgShortName = title.ShortName;
        return dto;
    }

    private static TransferReceiveDto MapForm14Row(
        int id,
        int repsId,
        int reportId,
        int numberInOrder,
        string orgOkpo,
        string orgRegNo,
        string orgShortName,
        string? opCode,
        string? opDate,
        string? pasNum,
        string? name,
        byte? sort,
        string? radionuclids,
        string? activity,
        string? activityMeasurementDate,
        string? volume,
        string? mass,
        byte? aggregateState,
        string? packNumber,
        string? providerOrRecieverOkpo,
        string? startPeriod,
        string? endPeriod) =>
        new()
        {
            Id = id,
            RepsId = repsId,
            ReportId = reportId,
            NumberInOrder = numberInOrder,
            OrgOkpo = orgOkpo,
            OrgLegalOkpo = orgOkpo,
            OrgBranchOkpo = string.Empty,
            OrgRegNo = orgRegNo,
            OrgShortName = orgShortName,
            OpCode = opCode ?? string.Empty,
            OpDate = opDate ?? string.Empty,
            PasNum = pasNum ?? string.Empty,
            Type = name ?? string.Empty,
            Sort = sort,
            Radionuclids = radionuclids ?? string.Empty,
            Activity = activity ?? string.Empty,
            ActivityMeasurementDate = activityMeasurementDate ?? string.Empty,
            Volume = volume ?? string.Empty,
            Mass = mass ?? string.Empty,
            AggregateState = aggregateState,
            PackNumber = packNumber ?? string.Empty,
            ProviderOrRecieverOkpo = providerOrRecieverOkpo ?? string.Empty,
            Quantity = 1,
            StartPeriod = startPeriod ?? string.Empty,
            EndPeriod = endPeriod ?? string.Empty,
            IsTransfer = IsTransferCodeForm11(opCode)
        };

    /// <summary>
    /// Один запрос: все операции приёма/передачи формы 1.5 по БД (без ОКПО изготовителя).
    /// </summary>
    private static TransferReceiveDto MapForm15Row(
        int id,
        int repsId,
        int reportId,
        int numberInOrder,
        string orgOkpo,
        string orgRegNo,
        string orgShortName,
        string? opCode,
        string? opDate,
        string? pasNum,
        string? facNum,
        string? type,
        string? radionuclids,
        string? statusRao,
        string? packName,
        string? packType,
        string? packNumber,
        string? subsidy,
        string? fcpNumber,
        string? providerOrRecieverOkpo,
        int? quantity,
        string? activity,
        string? creationDate,
        string? startPeriod,
        string? endPeriod) =>
        new()
        {
            Id = id,
            RepsId = repsId,
            ReportId = reportId,
            NumberInOrder = numberInOrder,
            OrgOkpo = orgOkpo,
            OrgLegalOkpo = orgOkpo,
            OrgBranchOkpo = string.Empty,
            OrgRegNo = orgRegNo,
            OrgShortName = orgShortName,
            OpCode = opCode ?? string.Empty,
            OpDate = opDate ?? string.Empty,
            PasNum = pasNum ?? string.Empty,
            FacNum = facNum ?? string.Empty,
            Type = type ?? string.Empty,
            Radionuclids = radionuclids ?? string.Empty,
            StatusRao = statusRao ?? string.Empty,
            PackName = packName ?? string.Empty,
            PackType = packType ?? string.Empty,
            PackNumber = packNumber ?? string.Empty,
            Subsidy = subsidy ?? string.Empty,
            FcpNumber = fcpNumber ?? string.Empty,
            ProviderOrRecieverOkpo = providerOrRecieverOkpo ?? string.Empty,
            Quantity = quantity,
            Activity = activity ?? string.Empty,
            CreationDate = creationDate ?? string.Empty,
            StartPeriod = startPeriod ?? string.Empty,
            EndPeriod = endPeriod ?? string.Empty,
            IsTransfer = IsTransferCodeForm11(opCode)
        };

    private static async Task<List<TransferReceiveDto>> LoadAllForm15TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodesForm15;
        var rows = await db.form_15
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                RepsId = form.Report!.Reports.Id,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                StatusRao = form.StatusRAO_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB,
                Subsidy = form.Subsidy_DB,
                FcpNumber = form.FcpNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                Quantity = form.Quantity_DB,
                Activity = form.Activity_DB,
                CreationDate = form.CreationDate_DB,
                StartPeriod = form.Report.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
            result.Add(WithOrgTitleOkpos(MapForm15Row(
                row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                title?.Okpo ?? string.Empty, title?.RegNo ?? string.Empty, title?.ShortName ?? string.Empty,
                row.OpCode, row.OpDate, row.PasNum, row.FacNum, row.Type, row.Radionuclids,
                row.StatusRao, row.PackName, row.PackType, row.PackNumber, row.Subsidy, row.FcpNumber,
                row.ProviderOrRecieverOkpo, row.Quantity, row.Activity, row.CreationDate,
                row.StartPeriod, row.EndPeriod), title));
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm15TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodesForm15;
        var rows = await db.form_15
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                StatusRao = form.StatusRAO_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB,
                Subsidy = form.Subsidy_DB,
                FcpNumber = form.FcpNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                Quantity = form.Quantity_DB,
                Activity = form.Activity_DB,
                CreationDate = form.CreationDate_DB,
                StartPeriod = form.Report!.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        return rows
            .Where(row => IsTransferOrReceiveCodeForm11(row.OpCode))
            .Select(row => MapForm15Row(
                row.Id, repsId, row.ReportId, row.NumberInOrder,
                orgOkpo, string.Empty, string.Empty,
                row.OpCode, row.OpDate, row.PasNum, row.FacNum, row.Type, row.Radionuclids,
                row.StatusRao, row.PackName, row.PackType, row.PackNumber, row.Subsidy, row.FcpNumber,
                row.ProviderOrRecieverOkpo, row.Quantity, row.Activity, row.CreationDate,
                row.StartPeriod, row.EndPeriod))
            .OrderBy(row => row.Id)
            .ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm15TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "1.5 контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodesForm15;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_15
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            var pageSize = SelectedOrgOpsPageSize > 0 ? SelectedOrgOpsPageSize : 2500;
            var lastRowId = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rows = await query
                    .Where(form => form.Id > lastRowId)
                    .OrderBy(form => form.Id)
                    .Take(pageSize)
                    .Select(form => new
                    {
                        form.Id,
                        ReportId = form.ReportId ?? 0,
                        RepsId = form.Report!.Reports.Id,
                        NumberInOrder = form.NumberInOrder_DB,
                        OpCode = form.OperationCode_DB,
                        OpDate = form.OperationDate_DB,
                        PasNum = form.PassportNumber_DB,
                        FacNum = form.FactoryNumber_DB,
                        Type = form.Type_DB,
                        Radionuclids = form.Radionuclids_DB,
                        StatusRao = form.StatusRAO_DB,
                        PackName = form.PackName_DB,
                        PackType = form.PackType_DB,
                        PackNumber = form.PackNumber_DB,
                        Subsidy = form.Subsidy_DB,
                        FcpNumber = form.FcpNumber_DB,
                        ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                        Quantity = form.Quantity_DB,
                        Activity = form.Activity_DB,
                        CreationDate = form.CreationDate_DB,
                        StartPeriod = form.Report.StartPeriod_DB,
                        EndPeriod = form.Report.EndPeriod_DB
                    })
                    .ToListAsync(cancellationToken);

                if (rows.Count == 0)
                {
                    break;
                }

                foreach (var row in rows)
                {
                    if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                    {
                        continue;
                    }

                    orgTitlesByRepsId.TryGetValue(row.RepsId, out var title);
                    result.Add(WithOrgTitleOkpos(MapForm15Row(
                        row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                        title?.Okpo ?? string.Empty, title?.RegNo ?? string.Empty, title?.ShortName ?? string.Empty,
                        row.OpCode, row.OpDate, row.PasNum, row.FacNum, row.Type, row.Radionuclids,
                        row.StatusRao, row.PackName, row.PackType, row.PackNumber, row.Subsidy, row.FcpNumber,
                        row.ProviderOrRecieverOkpo, row.Quantity, row.Activity, row.CreationDate,
                        row.StartPeriod, row.EndPeriod), title));
                }

                lastRowId = rows[^1].Id;
                if (rows.Count < pageSize)
                {
                    break;
                }
            }

            orgsDone += idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static TransferReceiveDto MapForm16Row(
        int id,
        int repsId,
        int reportId,
        int numberInOrder,
        string orgOkpo,
        string orgRegNo,
        string orgShortName,
        string? opCode,
        string? opDate,
        string? codeRao,
        string? statusRao,
        string? volume,
        string? mass,
        string? quantityOziii,
        string? radionuclids,
        string? tritiumActivity,
        string? betaGammaActivity,
        string? alphaActivity,
        string? transuraniumActivity,
        string? activityMeasurementDate,
        string? providerOrRecieverOkpo,
        string? packType,
        string? packNumber,
        string? subsidy,
        string? fcpNumber,
        string? startPeriod,
        string? endPeriod) =>
        new()
        {
            Id = id,
            RepsId = repsId,
            ReportId = reportId,
            NumberInOrder = numberInOrder,
            OrgOkpo = orgOkpo,
            OrgLegalOkpo = orgOkpo,
            OrgBranchOkpo = string.Empty,
            OrgRegNo = orgRegNo,
            OrgShortName = orgShortName,
            OpCode = opCode ?? string.Empty,
            OpDate = opDate ?? string.Empty,
            CodeRao = codeRao ?? string.Empty,
            StatusRao = statusRao ?? string.Empty,
            Volume = volume ?? string.Empty,
            Mass = mass ?? string.Empty,
            Quantity = ParseQuantityOziii(quantityOziii),
            Radionuclids = radionuclids ?? string.Empty,
            TritiumActivity = tritiumActivity ?? string.Empty,
            BetaGammaActivity = betaGammaActivity ?? string.Empty,
            AlphaActivity = alphaActivity ?? string.Empty,
            TransuraniumActivity = transuraniumActivity ?? string.Empty,
            ActivityMeasurementDate = activityMeasurementDate ?? string.Empty,
            ProviderOrRecieverOkpo = providerOrRecieverOkpo ?? string.Empty,
            PackType = packType ?? string.Empty,
            PackNumber = packNumber ?? string.Empty,
            Subsidy = subsidy ?? string.Empty,
            FcpNumber = fcpNumber ?? string.Empty,
            StartPeriod = startPeriod ?? string.Empty,
            EndPeriod = endPeriod ?? string.Empty,
            IsTransfer = IsTransferCodeForm11(opCode)
        };

    private static async Task<List<TransferReceiveDto>> LoadAllForm16TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodesForm15;
        var rows = await db.form_16
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                RepsId = form.Report!.Reports.Id,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                CodeRao = form.CodeRAO_DB,
                StatusRao = form.StatusRAO_DB,
                Volume = form.Volume_DB,
                Mass = form.Mass_DB,
                QuantityOziii = form.QuantityOZIII_DB,
                Radionuclids = form.MainRadionuclids_DB,
                TritiumActivity = form.TritiumActivity_DB,
                BetaGammaActivity = form.BetaGammaActivity_DB,
                AlphaActivity = form.AlphaActivity_DB,
                TransuraniumActivity = form.TransuraniumActivity_DB,
                ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB,
                Subsidy = form.Subsidy_DB,
                FcpNumber = form.FcpNumber_DB,
                StartPeriod = form.Report.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
            result.Add(WithOrgTitleOkpos(MapForm16Row(
                row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                title?.Okpo ?? string.Empty, title?.RegNo ?? string.Empty, title?.ShortName ?? string.Empty,
                row.OpCode, row.OpDate, row.CodeRao, row.StatusRao, row.Volume, row.Mass, row.QuantityOziii,
                row.Radionuclids, row.TritiumActivity, row.BetaGammaActivity, row.AlphaActivity,
                row.TransuraniumActivity, row.ActivityMeasurementDate, row.ProviderOrRecieverOkpo,
                row.PackType, row.PackNumber, row.Subsidy, row.FcpNumber,
                row.StartPeriod, row.EndPeriod), title));
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm16TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodesForm15;
        var rows = await db.form_16
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                CodeRao = form.CodeRAO_DB,
                StatusRao = form.StatusRAO_DB,
                Volume = form.Volume_DB,
                Mass = form.Mass_DB,
                QuantityOziii = form.QuantityOZIII_DB,
                Radionuclids = form.MainRadionuclids_DB,
                TritiumActivity = form.TritiumActivity_DB,
                BetaGammaActivity = form.BetaGammaActivity_DB,
                AlphaActivity = form.AlphaActivity_DB,
                TransuraniumActivity = form.TransuraniumActivity_DB,
                ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB,
                Subsidy = form.Subsidy_DB,
                FcpNumber = form.FcpNumber_DB,
                StartPeriod = form.Report!.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        return rows
            .Where(row => IsTransferOrReceiveCodeForm11(row.OpCode))
            .Select(row => MapForm16Row(
                row.Id, repsId, row.ReportId, row.NumberInOrder,
                orgOkpo, string.Empty, string.Empty,
                row.OpCode, row.OpDate, row.CodeRao, row.StatusRao, row.Volume, row.Mass, row.QuantityOziii,
                row.Radionuclids, row.TritiumActivity, row.BetaGammaActivity, row.AlphaActivity,
                row.TransuraniumActivity, row.ActivityMeasurementDate, row.ProviderOrRecieverOkpo,
                row.PackType, row.PackNumber, row.Subsidy, row.FcpNumber,
                row.StartPeriod, row.EndPeriod))
            .OrderBy(row => row.Id)
            .ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm16TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "1.6 контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodesForm15;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_16
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            var pageSize = SelectedOrgOpsPageSize > 0 ? SelectedOrgOpsPageSize : 2500;
            var lastRowId = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rows = await query
                    .Where(form => form.Id > lastRowId)
                    .OrderBy(form => form.Id)
                    .Take(pageSize)
                    .Select(form => new
                    {
                        form.Id,
                        ReportId = form.ReportId ?? 0,
                        RepsId = form.Report!.Reports.Id,
                        NumberInOrder = form.NumberInOrder_DB,
                        OpCode = form.OperationCode_DB,
                        OpDate = form.OperationDate_DB,
                        CodeRao = form.CodeRAO_DB,
                        StatusRao = form.StatusRAO_DB,
                        Volume = form.Volume_DB,
                        Mass = form.Mass_DB,
                        QuantityOziii = form.QuantityOZIII_DB,
                        Radionuclids = form.MainRadionuclids_DB,
                        TritiumActivity = form.TritiumActivity_DB,
                        BetaGammaActivity = form.BetaGammaActivity_DB,
                        AlphaActivity = form.AlphaActivity_DB,
                        TransuraniumActivity = form.TransuraniumActivity_DB,
                        ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                        ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                        PackType = form.PackType_DB,
                        PackNumber = form.PackNumber_DB,
                        Subsidy = form.Subsidy_DB,
                        FcpNumber = form.FcpNumber_DB,
                        StartPeriod = form.Report.StartPeriod_DB,
                        EndPeriod = form.Report.EndPeriod_DB
                    })
                    .ToListAsync(cancellationToken);

                if (rows.Count == 0)
                {
                    break;
                }

                foreach (var row in rows)
                {
                    if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                    {
                        continue;
                    }

                    orgTitlesByRepsId.TryGetValue(row.RepsId, out var title);
                    result.Add(WithOrgTitleOkpos(MapForm16Row(
                        row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                        title?.Okpo ?? string.Empty, title?.RegNo ?? string.Empty, title?.ShortName ?? string.Empty,
                        row.OpCode, row.OpDate, row.CodeRao, row.StatusRao, row.Volume, row.Mass, row.QuantityOziii,
                        row.Radionuclids, row.TritiumActivity, row.BetaGammaActivity, row.AlphaActivity,
                        row.TransuraniumActivity, row.ActivityMeasurementDate, row.ProviderOrRecieverOkpo,
                        row.PackType, row.PackNumber, row.Subsidy, row.FcpNumber,
                        row.StartPeriod, row.EndPeriod), title));
                }

                lastRowId = rows[^1].Id;
                if (rows.Count < pageSize)
                {
                    break;
                }
            }

            orgsDone += idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm16TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodesForm15;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 5000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_16
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new
                {
                    form.Id,
                    ReportId = form.ReportId ?? 0,
                    RepsId = form.Report!.Reports.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    CodeRao = form.CodeRAO_DB,
                    StatusRao = form.StatusRAO_DB,
                    Volume = form.Volume_DB,
                    Mass = form.Mass_DB,
                    QuantityOziii = form.QuantityOZIII_DB,
                    Radionuclids = form.MainRadionuclids_DB,
                    TritiumActivity = form.TritiumActivity_DB,
                    BetaGammaActivity = form.BetaGammaActivity_DB,
                    AlphaActivity = form.AlphaActivity_DB,
                    TransuraniumActivity = form.TransuraniumActivity_DB,
                    ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    PackType = form.PackType_DB,
                    PackNumber = form.PackNumber_DB,
                    Subsidy = form.Subsidy_DB,
                    FcpNumber = form.FcpNumber_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(MapForm16Row(
                    row.Id, row.RepsId, row.ReportId, row.NumberInOrder,
                    string.Empty, string.Empty, string.Empty,
                    row.OpCode, row.OpDate, row.CodeRao, row.StatusRao, row.Volume, row.Mass, row.QuantityOziii,
                    row.Radionuclids, row.TritiumActivity, row.BetaGammaActivity, row.AlphaActivity,
                    row.TransuraniumActivity, row.ActivityMeasurementDate, row.ProviderOrRecieverOkpo,
                    row.PackType, row.PackNumber, row.Subsidy, row.FcpNumber,
                    row.StartPeriod, row.EndPeriod));
            }

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Карта ключ ОКПО → Id организаций + титулы (рег / display ОКПО / юрлицо / филиал).
    /// 1) быстрый Okpo_DB IN (claim + head8 + pads);
    /// 2) при хвосте — титулы активных org, матч full∨head8 по Rows10[0] и Rows10[1].
    /// </summary>
    private static async Task<(Dictionary<string, List<int>> ByNormOkpo, Dictionary<int, OrgTitleInfo> OrgTitles)>
        LoadRepsIdsByOkpoAsync(
            DBModel db,
            IReadOnlyList<string> rawOkpos,
            CancellationToken cancellationToken,
            ProgressReporter? progress = null)
    {
        var byNorm = new Dictionary<string, List<int>>(StringComparer.Ordinal);
        var orgTitles = new Dictionary<int, OrgTitleInfo>();
        var claims = rawOkpos
            .Where(IsOkpoValuePresent)
            .Select(okpo => okpo.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();
        if (claims.Count == 0)
        {
            return (byNorm, orgTitles);
        }

        var neededKeys = new HashSet<string>(StringComparer.Ordinal);
        foreach (var claim in claims)
        {
            foreach (var key in OkpoIndexKeys(claim))
            {
                neededKeys.Add(key);
            }
        }

        if (neededKeys.Count == 0)
        {
            return (byNorm, orgTitles);
        }

        // Фаза 1: Okpo_DB IN (сырые claim + головы + pads).
        var sqlVariants = BuildClaimOkpoSqlMatchVariants(claims);
        var exactMasterIds = new HashSet<int>();
        var okpoDone = 0;
        var okpoTotal = Math.Max(1, sqlVariants.Count);

        foreach (var okpoChunk in ChunkStrings(sqlVariants))
        {
            var hits = await db.form_10
                .AsNoTracking()
                .Where(f => f.ReportId != null
                            && f.Okpo_DB != null
                            && okpoChunk.Contains(f.Okpo_DB))
                .Select(f => f.ReportId!.Value)
                .Distinct()
                .ToListAsync(cancellationToken);

            foreach (var masterId in hits)
            {
                exactMasterIds.Add(masterId);
            }

            okpoDone += okpoChunk.Count;
            progress?.Report(
                okpoDone,
                okpoTotal,
                $"поиск контрагентов по ОКПО: {okpoDone} из {okpoTotal}");
        }

        Dictionary<int, int> masterToRepsId = new();
        List<(int MasterReportId, int NumberInOrder, string RegNo, string Okpo, string ShortName)> titleRows = [];

        if (exactMasterIds.Count > 0)
        {
            progress?.Status("загрузка титулов найденных контрагентов…");
            masterToRepsId = await LoadMasterReportIdToRepsIdMapAsync(
                db, exactMasterIds.ToList(), formNum: "1.0", cancellationToken);
            titleRows = await LoadForm10TitleRowsForMastersAsync(
                db, masterToRepsId.Keys.ToList(), cancellationToken, progress);
            RegisterTitleMatches(claims, neededKeys, masterToRepsId, titleRows, byNorm, orgTitles);
        }

        var missingClaims = claims
            .Where(claim => !OkpoIndexKeys(claim).Any(byNorm.ContainsKey))
            .ToList();

        // Фаза 2: незакрытые claim — все активные org, матч Legal∨Branch / full∨head8.
        if (missingClaims.Count > 0)
        {
            progress?.Status(
                $"догрузка титулов по нормализации ОКПО (осталось {missingClaims.Count} из {claims.Count})…");

            var orgRows = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.DBObservableId != null
                               && reps.Master_DB != null
                               && reps.Master_DB.FormNum_DB == "1.0")
                .Select(reps => new { reps.Id, MasterReportId = reps.Master_DBId })
                .Where(x => x.MasterReportId != null)
                .ToListAsync(cancellationToken);

            masterToRepsId = new Dictionary<int, int>(orgRows.Count);
            foreach (var org in orgRows)
            {
                masterToRepsId[org.MasterReportId!.Value] = org.Id;
            }

            progress?.Status("чтение form_10 для сопоставления ОКПО…");
            titleRows = (await db.form_10
                    .AsNoTracking()
                    .Where(f => f.ReportId != null)
                    .Select(f => new
                    {
                        MasterReportId = f.ReportId!.Value,
                        f.NumberInOrder_DB,
                        f.RegNo_DB,
                        f.Okpo_DB,
                        f.ShortJurLico_DB
                    })
                    .ToListAsync(cancellationToken))
                .Select(f => (
                    f.MasterReportId,
                    f.NumberInOrder_DB,
                    f.RegNo_DB ?? string.Empty,
                    f.Okpo_DB ?? string.Empty,
                    f.ShortJurLico_DB ?? string.Empty))
                .ToList();

            RegisterTitleMatches(missingClaims, neededKeys, masterToRepsId, titleRows, byNorm, orgTitles);
        }

        var neededRepsIds = byNorm.Values.SelectMany(ids => ids).ToHashSet();
        var missingTitleReps = neededRepsIds.Where(id => !orgTitles.ContainsKey(id)).ToList();
        if (missingTitleReps.Count > 0)
        {
            progress?.Status($"догрузка недостающих титулов: {missingTitleReps.Count} орг.…");
            await LoadOrgTitlesForRepsIdsAsync(db, missingTitleReps, orgTitles, cancellationToken);
        }

        progress?.Report(
            neededRepsIds.Count,
            neededRepsIds.Count,
            $"контрагенты по ОКПО: найдено {neededRepsIds.Count} орг. из {claims.Count} ОКПО");

        return (byNorm, orgTitles);
    }

    private static async Task<List<(int MasterReportId, int NumberInOrder, string RegNo, string Okpo, string ShortName)>>
        LoadForm10TitleRowsForMastersAsync(
            DBModel db,
            IReadOnlyList<int> masterIds,
            CancellationToken cancellationToken,
            ProgressReporter? progress = null)
    {
        var titleRows =
            new List<(int MasterReportId, int NumberInOrder, string RegNo, string Okpo, string ShortName)>();
        if (masterIds.Count == 0)
        {
            return titleRows;
        }

        var mastersDone = 0;
        foreach (var idChunk in ChunkIds(masterIds))
        {
            var batch = await db.form_10
                .AsNoTracking()
                .Where(f => f.ReportId != null && idChunk.Contains(f.ReportId.Value))
                .Select(f => new
                {
                    MasterReportId = f.ReportId!.Value,
                    f.NumberInOrder_DB,
                    f.RegNo_DB,
                    f.Okpo_DB,
                    f.ShortJurLico_DB
                })
                .ToListAsync(cancellationToken);
            titleRows.AddRange(batch.Select(f => (
                f.MasterReportId,
                f.NumberInOrder_DB,
                f.RegNo_DB ?? string.Empty,
                f.Okpo_DB ?? string.Empty,
                f.ShortJurLico_DB ?? string.Empty)));

            mastersDone += idChunk.Count;
            progress?.Report(
                mastersDone,
                masterIds.Count,
                $"загрузка титулов контрагентов: {mastersDone} из {masterIds.Count}");
        }

        return titleRows;
    }

    /// <summary>
    /// Для каждого Master строит титул (Legal+Branch) и при матче с claim
    /// индексирует RepsId под всеми ключами claim и титула из neededKeys.
    /// </summary>
    private static void RegisterTitleMatches(
        IReadOnlyList<string> claims,
        HashSet<string> neededKeys,
        IReadOnlyDictionary<int, int> masterToRepsId,
        IReadOnlyList<(int MasterReportId, int NumberInOrder, string RegNo, string Okpo, string ShortName)> titleRows,
        Dictionary<string, List<int>> byNorm,
        Dictionary<int, OrgTitleInfo> orgTitles)
    {
        var titlesByMaster = titleRows
            .GroupBy(t => t.MasterReportId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.NumberInOrder).ToList());

        foreach (var (masterId, repsId) in masterToRepsId)
        {
            if (!titlesByMaster.TryGetValue(masterId, out var rows) || rows.Count == 0)
            {
                continue;
            }

            var title = ResolveOrgTitle(rows);
            orgTitles.TryAdd(repsId, title);

            var matched = false;
            foreach (var claim in claims)
            {
                if (!OkpoClaimMatchesTitle(claim, title))
                {
                    continue;
                }

                matched = true;
                IndexRepsUnderOkpoKeys(byNorm, repsId, OkpoIndexKeys(claim));
            }

            if (matched)
            {
                IndexRepsUnderOkpoKeys(
                    byNorm,
                    repsId,
                    OkpoIndexKeysForTitle(title).Where(neededKeys.Contains));
            }
        }
    }

    private static OrgTitleInfo ResolveOrgTitle(
        IReadOnlyList<(int MasterReportId, int NumberInOrder, string RegNo, string Okpo, string ShortName)> rows)
    {
        var head = rows[0];
        var legalOkpoRaw = head.Okpo?.Trim() ?? string.Empty;
        var legalOkpo = NormalizeOkpoField(legalOkpoRaw);
        var legalReg = head.RegNo.Trim();
        var legalName = head.ShortName.Trim();

        var branchOkpo = string.Empty;
        var branchOkpoRaw = string.Empty;
        var branchReg = string.Empty;
        var branchName = string.Empty;
        var useBranchForDisplay = false;

        if (rows.Count > 1)
        {
            var branch = rows[1];
            branchOkpoRaw = branch.Okpo?.Trim() ?? string.Empty;
            branchOkpo = NormalizeOkpoField(branchOkpoRaw);
            branchReg = branch.RegNo.Trim();
            branchName = branch.ShortName.Trim();
            // Как OkpoRep/RegNoRep: филиал для display, если ОКПО не пуст и (есть рег.№ или ОКПО «-»).
            useBranchForDisplay = branchOkpoRaw.Length > 0
                                  && (branchReg.Length > 0 || branchOkpoRaw == "-");
        }

        if (useBranchForDisplay)
        {
            return new OrgTitleInfo(
                RegNo: branchReg,
                Okpo: branchOkpoRaw == "-" ? string.Empty : branchOkpoRaw,
                ShortName: branchName,
                LegalOkpo: legalOkpo,
                BranchOkpo: branchOkpo);
        }

        return new OrgTitleInfo(
            RegNo: legalReg,
            Okpo: legalOkpoRaw == "-" ? string.Empty : legalOkpoRaw,
            ShortName: legalName,
            LegalOkpo: legalOkpo,
            BranchOkpo: branchOkpo);
    }

    private static string NormalizeOkpoField(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        var trimmed = raw.Trim();
        return trimmed == "-" ? string.Empty : trimmed;
    }

    private static async Task LoadOrgTitlesForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        Dictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        foreach (var idChunk in ChunkIds(repsIds))
        {
            var batch = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => idChunk.Contains(reps.Id) && reps.Master_DBId != null)
                .Select(reps => new { reps.Id, MasterId = reps.Master_DBId!.Value })
                .ToListAsync(cancellationToken);

            var masterIds = batch.Select(b => b.MasterId).Distinct().ToList();
            if (masterIds.Count == 0)
            {
                continue;
            }

            var titles = await db.form_10
                .AsNoTracking()
                .Where(f => f.ReportId != null && masterIds.Contains(f.ReportId.Value))
                .Select(f => new
                {
                    MasterId = f.ReportId!.Value,
                    f.NumberInOrder_DB,
                    f.RegNo_DB,
                    f.Okpo_DB,
                    f.ShortJurLico_DB
                })
                .ToListAsync(cancellationToken);

            var byMaster = titles
                .GroupBy(t => t.MasterId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(x => x.NumberInOrder_DB)
                        .Select(x => (
                            x.MasterId,
                            x.NumberInOrder_DB,
                            x.RegNo_DB ?? string.Empty,
                            x.Okpo_DB ?? string.Empty,
                            x.ShortJurLico_DB ?? string.Empty))
                        .ToList());

            foreach (var org in batch)
            {
                if (orgTitles.ContainsKey(org.Id))
                {
                    continue;
                }

                if (!byMaster.TryGetValue(org.MasterId, out var rows) || rows.Count == 0)
                {
                    continue;
                }

                orgTitles[org.Id] = ResolveOrgTitle(rows);
            }
        }
    }

    private static async Task<Dictionary<int, int>> LoadMasterReportIdToRepsIdMapAsync(
        DBModel db,
        IReadOnlyList<int> masterReportIds,
        string formNum,
        CancellationToken cancellationToken)
    {
        var map = new Dictionary<int, int>();
        if (masterReportIds.Count == 0)
        {
            return map;
        }

        foreach (var idChunk in ChunkIds(masterReportIds))
        {
            var batch = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.DBObservableId != null
                               && reps.Master_DBId != null
                               && reps.Master_DB != null
                               && reps.Master_DB.FormNum_DB == formNum
                               && idChunk.Contains(reps.Master_DBId.Value))
                .Select(reps => new { MasterReportId = reps.Master_DBId!.Value, reps.Id })
                .ToListAsync(cancellationToken);

            foreach (var row in batch)
            {
                map[row.MasterReportId] = row.Id;
            }
        }

        return map;
    }

    private static IEnumerable<List<string>> ChunkStrings(IReadOnlyList<string> values)
    {
        for (var offset = 0; offset < values.Count; offset += FirebirdInListMaxCount)
        {
            yield return values.Skip(offset).Take(FirebirdInListMaxCount).ToList();
        }
    }

    /// <summary>
    /// Титул организации: display для Excel; Legal/Branch — для поиска пары по кол. 19.
    /// </summary>
    private sealed record OrgTitleInfo(
        string RegNo,
        string Okpo,
        string ShortName,
        string LegalOkpo = "",
        string BranchOkpo = "");

    #endregion

    #region Chunk helpers

    private static IEnumerable<List<int>> ChunkIds(IReadOnlyList<int> ids, int chunkSize = FirebirdInListMaxCount)
    {
        var size = chunkSize > 0 ? chunkSize : FirebirdInListMaxCount;
        for (var offset = 0; offset < ids.Count; offset += size)
        {
            yield return ids.Skip(offset).Take(size).ToList();
        }
    }

    #endregion

    #region DTO

    /// <summary>Узкий DTO операции приёма/передачи для сопоставления и выгрузки.</summary>
    private sealed class TransferReceiveDto
    {
        public int Id { get; init; }
        public int RepsId { get; init; }
        public int ReportId { get; init; }
        public int NumberInOrder { get; init; }
        public string OrgOkpo { get; set; } = string.Empty;
        public string OrgLegalOkpo { get; set; } = string.Empty;
        public string OrgBranchOkpo { get; set; } = string.Empty;
        public string OrgRegNo { get; set; } = string.Empty;
        public string OrgShortName { get; set; } = string.Empty;
        public string StartPeriod { get; set; } = string.Empty;
        public string EndPeriod { get; set; } = string.Empty;
        public string OpCode { get; init; } = string.Empty;
        public string OpDate { get; init; } = string.Empty;
        public string PasNum { get; init; } = string.Empty;
        public string FacNum { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Radionuclids { get; init; } = string.Empty;
        public string PackType { get; init; } = string.Empty;
        public string PackNumber { get; init; } = string.Empty;
        public string StatusRao { get; init; } = string.Empty;
        public string PackName { get; init; } = string.Empty;
        public string Subsidy { get; init; } = string.Empty;
        public string FcpNumber { get; init; } = string.Empty;
        public string ProviderOrRecieverOkpo { get; init; } = string.Empty;
        public string Activity { get; init; } = string.Empty;
        public string TritiumActivity { get; init; } = string.Empty;
        public string BetaGammaActivity { get; init; } = string.Empty;
        public string AlphaActivity { get; init; } = string.Empty;
        public string TransuraniumActivity { get; init; } = string.Empty;
        public string CodeRao { get; init; } = string.Empty;
        /// <summary>Масса, кг (1.2 — обедн. U; 1.4 — общая).</summary>
        public string Mass { get; init; } = string.Empty;
        /// <summary>Объём, куб. м (форма 1.4).</summary>
        public string Volume { get; init; } = string.Empty;
        /// <summary>Дата измерения активности (форма 1.4).</summary>
        public string ActivityMeasurementDate { get; init; } = string.Empty;
        /// <summary>Вид / Sort (форма 1.4).</summary>
        public byte? Sort { get; init; }
        public string CreatorOkpo { get; init; } = string.Empty;
        public string CreationDate { get; init; } = string.Empty;
        public int? Quantity { get; init; }
        public byte? AggregateState { get; set; }
        public bool IsTransfer { get; init; }
    }

    #endregion
}
