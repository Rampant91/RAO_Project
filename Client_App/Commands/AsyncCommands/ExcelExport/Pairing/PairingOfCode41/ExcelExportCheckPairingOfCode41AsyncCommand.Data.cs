using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Resources;
using Client_App.Resources.CustomComparers.SnkComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.Comparers.FormContent;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    #region Match 1.1 ↔ 1.5

    private static readonly SnkNumberEqualityComparer NumberComparer = new();

    /// <summary>
    /// Непарные операции 1.1↔1.5: ветки с серийными номерами и без (сумма количества).
    /// </summary>
    private static List<Operation41PairingDto> GetUnpairedOperations11To15(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options)
    {
        var sourceWithSerial = source.Where(item => !SerialNumbersAreEmpty(item)).ToList();
        var sourceWithoutSerial = source.Where(SerialNumbersAreEmpty).ToList();
        var referenceWithSerial = reference.Where(item => !SerialNumbersAreEmpty(item)).ToList();
        var referenceWithoutSerial = reference.Where(SerialNumbersAreEmpty).ToList();

        var unpaired = new List<Operation41PairingDto>();
        unpaired.AddRange(FindUnpairedWithSerial(sourceWithSerial, referenceWithSerial, options));
        unpaired.AddRange(FindUnpairedWithoutSerial(sourceWithoutSerial, referenceWithoutSerial, options));
        return unpaired;
    }

    private static List<Operation41PairingDto> FindUnpairedWithSerial(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options)
    {
        var referenceByKey = reference
            .GroupBy(item => BuildPairingKey(item, options, includeSerial: true, includeQuantity: options.CheckQuantity))
            .ToDictionary(group => group.Key, group => group.ToList(), StringComparer.Ordinal);

        var unpaired = new List<Operation41PairingDto>();
        foreach (var sourceItem in source)
        {
            var key = BuildPairingKey(sourceItem, options, includeSerial: true, includeQuantity: options.CheckQuantity);
            if (!referenceByKey.TryGetValue(key, out var candidates) || candidates.Count == 0)
            {
                unpaired.Add(sourceItem);
                continue;
            }

            var matchIndex = candidates.FindIndex(candidate => ActivityMatches(sourceItem, candidate, options.CheckActivity));
            if (matchIndex < 0)
            {
                unpaired.Add(sourceItem);
                continue;
            }

            candidates.RemoveAt(matchIndex);
        }

        return unpaired;
    }

    private static List<Operation41PairingDto> FindUnpairedWithoutSerial(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options)
    {
        var sourceByKey = source
            .GroupBy(item => BuildPairingKey(item, options, includeSerial: false, includeQuantity: false))
            .ToDictionary(group => group.Key, group => group.ToList(), StringComparer.Ordinal);
        var referenceByKey = reference
            .GroupBy(item => BuildPairingKey(item, options, includeSerial: false, includeQuantity: false))
            .ToDictionary(group => group.Key, group => group.ToList(), StringComparer.Ordinal);

        var unpaired = new List<Operation41PairingDto>();
        foreach (var sourceGroup in sourceByKey)
        {
            if (!referenceByKey.TryGetValue(sourceGroup.Key, out var refRows))
            {
                unpaired.AddRange(sourceGroup.Value);
                continue;
            }

            var refStates = refRows
                .Select(row => new RemainingRowState(row, GetQuantityForComparison(row, options.CheckQuantity)))
                .ToList();

            foreach (var sourceRow in sourceGroup.Value)
            {
                var remainingSourceQty = GetQuantityForComparison(sourceRow, options.CheckQuantity);
                for (var i = 0; i < refStates.Count && remainingSourceQty > 0; i++)
                {
                    if (refStates[i].RemainingQuantity <= 0)
                    {
                        continue;
                    }

                    if (!ActivityMatches(sourceRow, refStates[i].Row, options.CheckActivity))
                    {
                        continue;
                    }

                    var matchedQty = Math.Min(remainingSourceQty, refStates[i].RemainingQuantity);
                    remainingSourceQty -= matchedQty;
                    refStates[i].RemainingQuantity -= matchedQty;
                }

                if (remainingSourceQty > 0)
                {
                    unpaired.Add(sourceRow);
                }
            }
        }

        return unpaired;
    }

    private static int GetQuantityForComparison(Operation41PairingDto row, bool checkQuantity) =>
        checkQuantity ? row.Quantity is > 0 ? row.Quantity.Value : 1 : 1;

    private static bool ActivityMatches(Operation41PairingDto left, Operation41PairingDto right, bool checkActivity)
    {
        if (!checkActivity)
        {
            return true;
        }

        return NumericWithTolerance(left.Activity, right.Activity);
    }

    private static bool NumericWithTolerance(string? left, string? right) =>
        SoftSimilarityCore.NumericMatchesWithTolerance(left, right, 0.10, NumberComparer.Equals);

    private static string BuildPairingKey(
        Operation41PairingDto row,
        Pairing11To15Params options,
        bool includeSerial,
        bool includeQuantity)
    {
        var parts = new List<string>(17);
        if (options.CheckOperationCode) parts.Add(NormalizeNumber(row.OpCode));
        if (options.CheckOperationDate) parts.Add(NormalizeDate(row.OpDate));
        if (includeSerial && options.CheckPassportNumber) parts.Add(NormalizeSerialNumber(row.PasNum));
        if (options.CheckType) parts.Add(NormalizeNumber(row.Type));
        if (options.CheckRadionuclids) parts.Add(NormalizeRads(row.Radionuclids));
        if (includeSerial && options.CheckFactoryNumber) parts.Add(NormalizeSerialNumber(row.FacNum));
        if (options.CheckCreationDate) parts.Add(NormalizeDate(row.CreationDate));
        if (options.CheckDocumentVid) parts.Add(NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(row.DocumentVid)));
        if (options.CheckDocumentNumber) parts.Add(NormalizeNumber(row.DocumentNumber));
        if (options.CheckDocumentDate) parts.Add(NormalizeDate(row.DocumentDate));
        if (options.CheckProviderOrRecieverOkpo) parts.Add(NormalizeNumber(row.ProviderOrRecieverOkpo));
        if (options.CheckTransporterOkpo) parts.Add(NormalizeNumber(row.TransporterOkpo));
        if (options.CheckPackName) parts.Add(NormalizeNumber(row.PackName));
        if (options.CheckPackType) parts.Add(NormalizeNumber(row.PackType));
        if (options.CheckPackNumber) parts.Add(NormalizeNumber(row.PackNumber));
        if (includeQuantity && options.CheckQuantity) parts.Add(GetQuantityForComparison(row, true).ToString(CultureInfo.InvariantCulture));
        return string.Join('|', parts);
    }

    private static string NormalizeDate(string? value) =>
        DateOnly.TryParse(value, out var date)
            ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            : NormalizeNumber(value);

    private static bool SerialNumbersAreEmpty(Operation41PairingDto item) =>
        Operation41PairingKeyComparer.SerialNumbersIsEmpty(item.PasNum, item.FacNum);

    /// <summary>
    /// Паспорт / зав. №: пустая строка, «-», «б.н.», «без номера», «н.д.», «н/д», «нет данных» и т.п. → одна пустая каноническая форма.
    /// </summary>
    private static string NormalizeSerialNumber(string? value) =>
        Operation41PairingKeyComparer.IsEmptySerial(value) ? string.Empty : NormalizeNumber(value);

    private static string NormalizeNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "-")
        {
            return string.Empty;
        }

        // Как в SnkNumberEqualityComparer: спецсимволы, регистр, ведущие нули, схожие RU/EN буквы.
        var normalized = Regex.Replace(value.ToLowerInvariant(), @"[\\/:*?""<>|.,_\-;:\s+]", string.Empty)
            .TrimStart('0');
        return LookalikeCharMapper.ReplaceRuEnLookalikes(normalized, includeExtendedSnkSet: true);
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

    private sealed class RemainingRowState(Operation41PairingDto row, int remainingQuantity)
    {
        public Operation41PairingDto Row { get; } = row;
        public int RemainingQuantity { get; set; } = remainingQuantity;
    }

    #endregion

    #region Match 1.2–1.4 ↔ 1.6 / сторона 1.6

    /// <summary>
    /// Непарные 1.2 / 1.3 / 1.4 / 1.6 с <b>общим</b> пулом 1.6: одна строка 1.6 закрывает
    /// не больше одной строки РВ. Приоритет захвата: сначала 1.2, затем 1.3, затем 1.4.
    /// </summary>
    private static (
        List<Operation41PairingDto> Unpaired12,
        List<Operation41PairingDto> Unpaired13,
        List<Operation41PairingDto> Unpaired14,
        List<Operation41PairingDto> Unpaired16)
        GetUnpairedForms12To16Shared(
            List<Operation41PairingDto> form12,
            List<Operation41PairingDto> form13,
            List<Operation41PairingDto> form14,
            List<Operation41PairingDto> form16,
            Pairing12To16Params pairing12To16Params,
            Pairing13To16Params pairing13To16Params,
            Pairing14To16Params pairing14To16Params)
    {
        var form16Norms = CreatePairingNorms(form16);
        var used16 = new bool[form16Norms.Length];

        var unpaired12 = GetUnpairedByNormMatch(
            form12,
            form16,
            (rv, rao) => Matches12To16Norm(rv, rao, pairing12To16Params),
            bucketByOpDate: pairing12To16Params.CheckOperationDate,
            sharedReferenceUsed: used16,
            precomputedRefNorms: form16Norms);

        var unpaired13 = GetUnpairedByNormMatch(
            form13,
            form16,
            (rv, rao) => Matches13To16Norm(rv, rao, pairing13To16Params),
            bucketByOpDate: pairing13To16Params.CheckOperationDate,
            sharedReferenceUsed: used16,
            precomputedRefNorms: form16Norms);

        var unpaired14 = GetUnpairedByNormMatch(
            form14,
            form16,
            (rv, rao) => Matches14To16Norm(rv, rao, pairing14To16Params),
            bucketByOpDate: pairing14To16Params.CheckOperationDate,
            sharedReferenceUsed: used16,
            precomputedRefNorms: form16Norms);

        var unpaired16 = new List<Operation41PairingDto>();
        for (var i = 0; i < form16.Count; i++)
        {
            if (!used16[i])
            {
                unpaired16.Add(form16[i]);
            }
        }

        return (unpaired12, unpaired13, unpaired14, unpaired16);
    }

    /// <summary>Непарные 1.2→1.6 без учёта 1.3/1.4 (только для узких unit-тестов).</summary>
    private static List<Operation41PairingDto> GetUnpairedOperations12To16(
        List<Operation41PairingDto> source, List<Operation41PairingDto> reference, Pairing12To16Params options) =>
        GetUnpairedByNormMatch(
            source,
            reference,
            (rv, rao) => Matches12To16Norm(rv, rao, options),
            bucketByOpDate: options.CheckOperationDate);

    private static List<Operation41PairingDto> GetUnpairedOperations13To16(
        List<Operation41PairingDto> source, List<Operation41PairingDto> reference, Pairing13To16Params options) =>
        GetUnpairedByNormMatch(
            source,
            reference,
            (rv, rao) => Matches13To16Norm(rv, rao, options),
            bucketByOpDate: options.CheckOperationDate);

    private static List<Operation41PairingDto> GetUnpairedOperations14To16(
        List<Operation41PairingDto> source, List<Operation41PairingDto> reference, Pairing14To16Params options) =>
        GetUnpairedByNormMatch(
            source,
            reference,
            (rv, rao) => Matches14To16Norm(rv, rao, options),
            bucketByOpDate: options.CheckOperationDate);

    /// <summary>Непарные 1.6 — тот же общий claim, что и у РВ→1.6 (приоритет 12→13→14).</summary>
    private static List<Operation41PairingDto> GetUnpairedForm16(
        List<Operation41PairingDto> form16Operations,
        List<Operation41PairingDto> form12Operations,
        List<Operation41PairingDto> form13Operations,
        List<Operation41PairingDto> form14Operations,
        Pairing12To16Params pairing12To16Params,
        Pairing13To16Params pairing13To16Params,
        Pairing14To16Params pairing14To16Params) =>
        GetUnpairedForms12To16Shared(
                form12Operations,
                form13Operations,
                form14Operations,
                form16Operations,
                pairing12To16Params,
                pairing13To16Params,
                pairing14To16Params)
            .Unpaired16;

    /// <summary>
    /// Жадное сопоставление с пренормализацией, без RemoveAt и с бакетами по дате операции (если дата в ключе).
    /// <paramref name="isMatch"/>: (сторона source/RV, сторона reference/РАО) — как Matches*To16(rv, rao).
    /// <paramref name="sharedReferenceUsed"/> — общий used по пулу reference (одна 1.6 не закрывает две строки РВ).
    /// </summary>
    private static List<Operation41PairingDto> GetUnpairedByNormMatch(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Func<PairingNorm, PairingNorm, bool> isMatch,
        bool bucketByOpDate,
        bool[]? sharedReferenceUsed = null,
        PairingNorm[]? precomputedRefNorms = null)
    {
        if (source.Count == 0)
        {
            return [];
        }

        if (reference.Count == 0)
        {
            return [.. source];
        }

        var refNorms = precomputedRefNorms ?? CreatePairingNorms(reference);
        if (refNorms.Length != reference.Count)
        {
            throw new ArgumentException("precomputedRefNorms length must match reference.", nameof(precomputedRefNorms));
        }

        var used = sharedReferenceUsed ?? new bool[refNorms.Length];
        if (used.Length != refNorms.Length)
        {
            throw new ArgumentException("sharedReferenceUsed length must match reference norms.", nameof(sharedReferenceUsed));
        }

        var buckets = BuildOpDateBuckets(refNorms, bucketByOpDate);
        var unpaired = new List<Operation41PairingDto>();

        foreach (var row in source)
        {
            var rowNorm = CreatePairingNorm(row);
            if (!TryClaimMatchAsSource(rowNorm, refNorms, used, buckets, bucketByOpDate, isMatch))
            {
                unpaired.Add(row);
            }
        }

        return unpaired;
    }

    /// <summary>
    /// RV → РАО: isMatch(sourceNorm, refNorm). Помечает первого подходящего reference (в т.ч. в общем used 1.6).
    /// </summary>
    private static bool TryClaimMatchAsSource(
        PairingNorm sourceNorm,
        PairingNorm[] refNorms,
        bool[] used,
        Dictionary<string, List<int>>? buckets,
        bool bucketByOpDate,
        Func<PairingNorm, PairingNorm, bool> isMatch)
    {
        foreach (var i in CandidateIndices(sourceNorm.OpDate, refNorms.Length, buckets, bucketByOpDate))
        {
            if (used[i])
            {
                continue;
            }

            if (!isMatch(sourceNorm, refNorms[i]))
            {
                continue;
            }

            used[i] = true;
            return true;
        }

        return false;
    }

    private static Dictionary<string, List<int>>? BuildOpDateBuckets(PairingNorm[] norms, bool enabled)
    {
        if (!enabled || norms.Length == 0)
        {
            return null;
        }

        var buckets = new Dictionary<string, List<int>>(StringComparer.Ordinal);
        for (var i = 0; i < norms.Length; i++)
        {
            var key = norms[i].OpDate;
            if (!buckets.TryGetValue(key, out var list))
            {
                list = [];
                buckets[key] = list;
            }

            list.Add(i);
        }

        return buckets;
    }

    private static IEnumerable<int> CandidateIndices(
        string opDate,
        int length,
        Dictionary<string, List<int>>? buckets,
        bool bucketByOpDate)
    {
        if (!bucketByOpDate || buckets is null)
        {
            for (var i = 0; i < length; i++)
            {
                yield return i;
            }

            yield break;
        }

        if (buckets.TryGetValue(opDate, out var list))
        {
            foreach (var i in list)
            {
                yield return i;
            }
        }
    }

    private static bool Matches12To16(Operation41PairingDto rv, Operation41PairingDto rao, Pairing12To16Params options) =>
        Matches12To16Norm(CreatePairingNorm(rv), CreatePairingNorm(rao), options);

    private static bool Matches13To16(Operation41PairingDto rv, Operation41PairingDto rao, Pairing13To16Params options) =>
        Matches13To16Norm(CreatePairingNorm(rv), CreatePairingNorm(rao), options);

    private static bool Matches14To16(Operation41PairingDto rv, Operation41PairingDto rao, Pairing14To16Params options) =>
        Matches14To16Norm(CreatePairingNorm(rv), CreatePairingNorm(rao), options);

    private static bool Matches12To16Norm(PairingNorm rv, PairingNorm rao, Pairing12To16Params options) =>
        (!options.CheckOperationDate || rv.OpDate == rao.OpDate)
        && (!options.CheckMass || NumericTolerance(rv.Mass, rao.Mass))
        && (!options.CheckBetaGammaActivity || NumericTolerance(rv.BetaGammaActivity, rao.BetaGammaActivity))
        && (!options.CheckAlphaActivity || NumericTolerance(rv.AlphaActivity, rao.AlphaActivity))
        && (!options.CheckActivityMeasurementDate || rv.ActivityMeasurementDate == rao.ActivityMeasurementDate)
        && (!options.CheckDocumentVid || rv.DocumentVid == rao.DocumentVid)
        && (!options.CheckDocumentNumber || rv.DocumentNumber == rao.DocumentNumber)
        && (!options.CheckDocumentDate || rv.DocumentDate == rao.DocumentDate)
        && (!options.CheckPackName || rv.PackName == rao.PackName)
        && (!options.CheckPackType || rv.PackType == rao.PackType)
        && (!options.CheckPackNumber || rv.PackNumber == rao.PackNumber)
        && (!options.CheckCodeRao || rv.CodeRao == rao.CodeRao);

    private static bool Matches13To16Norm(PairingNorm rv, PairingNorm rao, Pairing13To16Params options) =>
        (!options.CheckOperationDate || rv.OpDate == rao.OpDate)
        && (!options.CheckMainRadionuclids || rv.MainRadionuclids == rao.MainRadionuclids)
        && (!options.CheckTritiumActivity || NumericTolerance(rv.TritiumActivity, rao.TritiumActivity))
        && (!options.CheckBetaGammaActivity || NumericTolerance(rv.BetaGammaActivity, rao.BetaGammaActivity))
        && (!options.CheckAlphaActivity || NumericTolerance(rv.AlphaActivity, rao.AlphaActivity))
        && (!options.CheckTransuraniumActivity || NumericTolerance(rv.TransuraniumActivity, rao.TransuraniumActivity))
        && (!options.CheckActivityMeasurementDate || rv.ActivityMeasurementDate == rao.ActivityMeasurementDate)
        && (!options.CheckDocumentVid || rv.DocumentVid == rao.DocumentVid)
        && (!options.CheckDocumentNumber || rv.DocumentNumber == rao.DocumentNumber)
        && (!options.CheckDocumentDate || rv.DocumentDate == rao.DocumentDate)
        && (!options.CheckPackName || rv.PackName == rao.PackName)
        && (!options.CheckPackType || rv.PackType == rao.PackType)
        && (!options.CheckPackNumber || rv.PackNumber == rao.PackNumber)
        && (!options.CheckCodeRao || rv.CodeRao == rao.CodeRao);

    private static bool Matches14To16Norm(PairingNorm rv, PairingNorm rao, Pairing14To16Params options) =>
        (!options.CheckOperationDate || rv.OpDate == rao.OpDate)
        && (!options.CheckVolume || NumericTolerance(rv.Volume, rao.Volume))
        && (!options.CheckMass || NumericTolerance(rv.Mass, rao.Mass))
        && (!options.CheckMainRadionuclids || rv.MainRadionuclids == rao.MainRadionuclids)
        && (!options.CheckTritiumActivity || NumericTolerance(rv.TritiumActivity, rao.TritiumActivity))
        && (!options.CheckBetaGammaActivity || NumericTolerance(rv.BetaGammaActivity, rao.BetaGammaActivity))
        && (!options.CheckAlphaActivity || NumericTolerance(rv.AlphaActivity, rao.AlphaActivity))
        && (!options.CheckTransuraniumActivity || NumericTolerance(rv.TransuraniumActivity, rao.TransuraniumActivity))
        && (!options.CheckActivityMeasurementDate || rv.ActivityMeasurementDate == rao.ActivityMeasurementDate)
        && (!options.CheckDocumentVid || rv.DocumentVid == rao.DocumentVid)
        && (!options.CheckDocumentNumber || rv.DocumentNumber == rao.DocumentNumber)
        && (!options.CheckDocumentDate || rv.DocumentDate == rao.DocumentDate)
        && (!options.CheckPackName || rv.PackName == rao.PackName)
        && (!options.CheckPackType || rv.PackType == rao.PackType)
        && (!options.CheckPackNumber || rv.PackNumber == rao.PackNumber)
        && (!options.CheckCodeRao || rv.CodeRao == rao.CodeRao);

    #endregion

    #region Bulk load

    /// <summary>
    /// Операции 41 одной организации (уже с заполненным <see cref="Operation41PairingDto.RepsId"/>).
    /// </summary>
    private sealed class OrgOperation41Lists
    {
        public List<Operation41PairingDto> Form11 { get; set; } = [];
        public List<Operation41PairingDto> Form12 { get; set; } = [];
        public List<Operation41PairingDto> Form13 { get; set; } = [];
        public List<Operation41PairingDto> Form14 { get; set; } = [];
        public List<Operation41PairingDto> Form15 { get; set; } = [];
        public List<Operation41PairingDto> Form16 { get; set; } = [];
    }

    /// <summary>
    /// Bulk: постраничная загрузка 6 форм (RepsId в DTO) + GroupBy org.
    /// </summary>
    private static async Task<Dictionary<int, OrgOperation41Lists>> LoadAllOperation41GroupedByRepsIdAsync(
        DBModel db,
        PairingParamsSet pairingParams,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null)
    {
        var p11 = pairingParams.Pairing11To15;
        const int stages = 7;
        const int bulkMin = 28;
        const int bulkMax = 55;

        ProgressReporter? FormProgress(int index) =>
            progress?.Nest(
                bulkMin + (bulkMax - bulkMin) * index / stages,
                bulkMin + (bulkMax - bulkMin) * (index + 1) / stages);

        var form11Progress = FormProgress(0);
        var form11 = await LoadOperation41ListAsync(db, null, "1.1", cancellationToken, p11, form11Progress);
        form11Progress?.ReportNow(1, 1, $"форма 1.1: готово — {form11.Count} строк");

        var form12Progress = FormProgress(1);
        var form12 = await LoadOperation41ListAsync(db, null, "1.2", cancellationToken, progress: form12Progress);
        form12Progress?.ReportNow(1, 1, $"форма 1.2: готово — {form12.Count} строк");

        var form13Progress = FormProgress(2);
        var form13 = await LoadOperation41ListAsync(db, null, "1.3", cancellationToken, progress: form13Progress);
        form13Progress?.ReportNow(1, 1, $"форма 1.3: готово — {form13.Count} строк");

        var form14Progress = FormProgress(3);
        var form14 = await LoadOperation41ListAsync(db, null, "1.4", cancellationToken, progress: form14Progress);
        form14Progress?.ReportNow(1, 1, $"форма 1.4: готово — {form14.Count} строк");

        var form15Progress = FormProgress(4);
        var form15 = await LoadOperation41ListAsync(db, null, "1.5", cancellationToken, p11, form15Progress);
        form15Progress?.ReportNow(1, 1, $"форма 1.5: готово — {form15.Count} строк");

        var form16Progress = FormProgress(5);
        var form16 = await LoadOperation41ListAsync(db, null, "1.6", cancellationToken, progress: form16Progress);
        form16Progress?.ReportNow(1, 1, $"форма 1.6: готово — {form16.Count} строк");

        var groupProgress = FormProgress(6);
        groupProgress?.Status("группировка по организациям…");
        var byOrg = new Dictionary<int, OrgOperation41Lists>();
        AddFormToOrgGroups(byOrg, form11, static (org, rows) => org.Form11 = rows);
        AddFormToOrgGroups(byOrg, form12, static (org, rows) => org.Form12 = rows);
        AddFormToOrgGroups(byOrg, form13, static (org, rows) => org.Form13 = rows);
        AddFormToOrgGroups(byOrg, form14, static (org, rows) => org.Form14 = rows);
        AddFormToOrgGroups(byOrg, form15, static (org, rows) => org.Form15 = rows);
        AddFormToOrgGroups(byOrg, form16, static (org, rows) => org.Form16 = rows);
        groupProgress?.ReportNow(1, 1, $"группировка: {byOrg.Count} организаций");
        return byOrg;
    }

    private static void AddFormToOrgGroups(
        Dictionary<int, OrgOperation41Lists> byOrg,
        List<Operation41PairingDto> rows,
        Action<OrgOperation41Lists, List<Operation41PairingDto>> assign)
    {
        foreach (var group in rows.GroupBy(row => row.RepsId))
        {
            if (group.Key == 0)
            {
                continue;
            }

            if (!byOrg.TryGetValue(group.Key, out var orgLists))
            {
                orgLists = new OrgOperation41Lists();
                byOrg[group.Key] = orgLists;
            }

            // GroupBy сохраняет порядок встречи; списки уже отсортированы по Id в LoadOperation41ListAsync.
            assign(orgLists, group.ToList());
        }
    }

    #endregion

    #region Organization info for export

    /// <summary>
    /// Реквизиты организации (Рег.№/ОКПО/наименование) — константны для всех строк org,
    /// заполняются после загрузки из уже материализованного <see cref="Reports"/> (RegNoRep и т.п. не транслируются в SQL).
    /// </summary>
    private static void StampOrganizationInfo(IEnumerable<Operation41PairingDto> ops, Reports org)
    {
        var regNo = org.Master_DB.RegNoRep.Value ?? string.Empty;
        var okpo = org.Master_DB.OkpoRep.Value ?? string.Empty;
        var shortName = org.Master_DB.ShortJurLicoRep.Value ?? string.Empty;
        foreach (var op in ops)
        {
            op.OrgRegNo = regNo;
            op.OrgOkpo = okpo;
            op.OrgShortName = shortName;
        }
    }

    private static IEnumerable<List<int>> ChunkIds(IReadOnlyList<int> ids)
    {
        for (var offset = 0; offset < ids.Count; offset += FirebirdInListMaxCount)
        {
            yield return ids.Skip(offset).Take(FirebirdInListMaxCount).ToList();
        }
    }

    #endregion

    #region R.xlsx / activities

    private static readonly List<Dictionary<string, string>> R = [];

    /// <summary>name → code из R.xlsx; строится при загрузке справочника.</summary>
    private static Dictionary<string, string> RCodeByName { get; set; } = new(StringComparer.Ordinal);

    /// <summary>Сбрасывает кэш R.xlsx (для unit-тестов).</summary>
    internal static void ResetRDictionaryCache()
    {
        R.Clear();
        RCodeByName = new Dictionary<string, string>(StringComparer.Ordinal);
    }

    /// <summary>Загружает справочник из указанного файла (unit-тесты / диагностика).</summary>
    internal static bool TryLoadRDictionaryFromFile(string filePath, out string errorMessage)
    {
        errorMessage = string.Empty;
        ResetRDictionaryCache();

        if (!File.Exists(filePath))
        {
            errorMessage =
                "Не удалось найти справочник радионуклидов R.xlsx (папка data\\Spravochniki)." +
                $"{Environment.NewLine}Выгрузка непарных операций 41 прервана.";
            return false;
        }

        return TryReadRDictionaryWorkbook(filePath, out errorMessage);
    }

    /// <summary>
    /// Загружает справочник радионуклидов R.xlsx. Возвращает false, если файл не найден или не удалось прочитать.
    /// </summary>
    private static bool TryLoadRDictionary(out string errorMessage)
    {
        errorMessage = string.Empty;
        if (R.Count != 0)
        {
            return true;
        }

        var filePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx");
        if (!File.Exists(filePath))
        {
            filePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", "R.xlsx");
        }

        if (!File.Exists(filePath))
        {
            errorMessage =
                "Не удалось найти справочник радионуклидов R.xlsx (папка data\\Spravochniki)." +
                $"{Environment.NewLine}Выгрузка непарных операций 41 прервана.";
            return false;
        }

        return TryReadRDictionaryWorkbook(filePath, out errorMessage);
    }

    private static bool TryReadRDictionaryWorkbook(string filePath, out string errorMessage)
    {
        errorMessage = string.Empty;
        try
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var xls = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath));
            var ws = xls.Workbook.Worksheets["Лист1"];
            if (ws is null)
            {
                errorMessage =
                    "Не удалось прочитать справочник радионуклидов R.xlsx: в файле отсутствует лист «Лист1»." +
                    $"{Environment.NewLine}Закройте файл, если он открыт в другой программе, и повторите выгрузку.";
                return false;
            }

            for (var i = 2; ws.Cells[i, 1].Text != string.Empty; i++)
            {
                R.Add(new Dictionary<string, string>
                {
                    { "name", ws.Cells[i, 1].Text },
                    { "code", ws.Cells[i, 8].Text }
                });
            }

            if (R.Count == 0)
            {
                errorMessage =
                    "Справочник радионуклидов R.xlsx пуст или не содержит данных." +
                    $"{Environment.NewLine}Выгрузка непарных операций 41 прервана.";
                return false;
            }

            RebuildRCodeByName();
            return true;
        }
        catch (IOException)
        {
            errorMessage =
                "Не удалось прочитать справочник радионуклидов R.xlsx." +
                $"{Environment.NewLine}Закройте файл, если он открыт в другой программе, и повторите выгрузку.";
            return false;
        }
        catch (Exception)
        {
            errorMessage =
                "Не удалось прочитать справочник радионуклидов R.xlsx." +
                $"{Environment.NewLine}Закройте файл, если он открыт в другой программе, и повторите выгрузку.";
            return false;
        }
    }

    private static void RebuildRCodeByName()
    {
        var map = new Dictionary<string, string>(R.Count, StringComparer.Ordinal);
        foreach (var row in R)
        {
            map[row["name"]] = row["code"];
        }

        RCodeByName = map;
    }

    private static string ToMassTon(string? mass)
    {
        var massTmp = (mass ?? "").ToLower().Replace('.', ',').Replace("(", "").Replace(")", "").Replace('е', 'e').Trim();
        return double.TryParse(massTmp, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
            new CultureInfo("ru-RU", useUserOverride: false), out var value)
            ? $"{value / 1000:0.######################################################e+00}"
            : "";
    }

    private static string ComputeFromMass(string massTon, double coef) =>
        double.TryParse(massTon.Replace('.', ','), NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent,
            new CultureInfo("ru-RU", useUserOverride: false), out var value)
            ? $"{value * coef:0.######################################################e+00}"
            : "";

    private static Dictionary<string, string> GetActivitiesForExport(string? radionuclids, string? activityRaw)
    {
        if (R.Count == 0)
        {
            throw new InvalidOperationException("Справочник радионуклидов R.xlsx не загружен.");
        }

        if (RCodeByName.Count == 0)
        {
            RebuildRCodeByName();
        }

        var nuclids = (radionuclids ?? "").Replace(" ", string.Empty).ToLower().Replace(',', ';').Split(';', StringSplitOptions.RemoveEmptyEntries);
        var nuclidTypes = new List<string>(nuclids.Length);
        foreach (var name in nuclids)
        {
            if (RCodeByName.TryGetValue(name, out var code))
            {
                nuclidTypes.Add(code);
            }
        }

        var activityTmp = (activityRaw ?? "").Replace(".", ",").Replace("(", "").Replace(")", "");
        var activity = double.TryParse(activityTmp, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
            new CultureInfo("ru-RU", useUserOverride: false), out var activityDoubleValue)
            ? $"{activityDoubleValue:0.######################################################e+00}"
            : activityTmp;

        var result = new Dictionary<string, string>
        {
            { "alpha", "-" }, { "beta", "-" }, { "tritium", "-" }, { "transuranium", "-" }
        };
        if (nuclidTypes.Count == 0) return result;
        if (nuclidTypes.Count == 1 || nuclidTypes.Skip(1).All(x => string.Equals(nuclidTypes[0], x, StringComparison.Ordinal)))
        {
            switch (nuclidTypes[0])
            {
                case "а": result["alpha"] = activity; break;
                case "б": result["beta"] = activity; break;
                case "т": result["tritium"] = activity; break;
                case "у": result["transuranium"] = activity; break;
            }
        }
        return result;
    }

    #endregion

    #region DTO

    /// <summary>Узкий DTO операции 41 для сопоставления (не полная строка формы).</summary>
    internal sealed class Operation41PairingDto
    {
        public int Id { get; init; }
        public int RepsId { get; set; }
        public int ReportId { get; init; }
        public string OpCode { get; init; } = string.Empty;
        public string OpDate { get; init; } = string.Empty;
        public string PasNum { get; init; } = string.Empty;
        public string FacNum { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Radionuclids { get; init; } = string.Empty;
        public string CreationDate { get; init; } = string.Empty;
        public byte? DocumentVid { get; init; }
        public string DocumentNumber { get; init; } = string.Empty;
        public string DocumentDate { get; init; } = string.Empty;
        public string ProviderOrRecieverOkpo { get; init; } = string.Empty;
        public string TransporterOkpo { get; init; } = string.Empty;
        public string PackNumber { get; init; } = string.Empty;
        public string PackName { get; init; } = string.Empty;
        public string PackType { get; init; } = string.Empty;
        public string Activity { get; init; } = string.Empty;
        public string MainRadionuclids { get; init; } = string.Empty;
        public string TritiumActivity { get; init; } = string.Empty;
        public string BetaGammaActivity { get; init; } = string.Empty;
        public string AlphaActivity { get; init; } = string.Empty;
        public string TransuraniumActivity { get; init; } = string.Empty;
        public string Mass { get; init; } = string.Empty;
        public string Volume { get; init; } = string.Empty;
        public string ActivityMeasurementDate { get; init; } = string.Empty;
        public int? Quantity { get; init; }
        public string FormNum { get; init; } = string.Empty;
        public string CodeRao { get; init; } = string.Empty;
        public byte? AggregateState { get; init; }

        /// <summary>Заполняются при загрузке (период отчёта) / после загрузки (реквизиты организации) для вывода в Excel.</summary>
        public string OrgRegNo { get; set; } = string.Empty;
        public string OrgOkpo { get; set; } = string.Empty;
        public string OrgShortName { get; set; } = string.Empty;
        public string StartPeriod { get; set; } = string.Empty;
        public string EndPeriod { get; set; } = string.Empty;
        public int? NumberInOrder { get; init; }
    }

    #endregion
}
