using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Closest-match state

    private Dictionary<TransferReceiveFormId, Dictionary<int, ClosestMatchResult>> _closestByForm = new();
    private TransferReceiveParamsSet _currentParams = TransferReceiveParamsSet.Form11And13(
        new TransferReceiveFormParams(), DefaultForm13Params());

    #endregion

    #region Closest match

    /// <summary>
    /// Для каждой непарной строки — ближайший кандидат у контрагента:
    /// взвешенный soft-score по полям (не число точных совпадений).
    /// Оптимизации: кэш норм, индекс кандидатов по ОКПО/направлению/дате (±N дней), параллель по непарным.
    /// </summary>
    private static Dictionary<int, ClosestMatchResult> BuildClosestMatchResults(
        List<TransferReceiveDto> unpaired,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        TransferReceiveFormParams options,
        ProgressReporter? progress = null,
        ClosestCandidateIndex? prebuiltCandidateIndex = null,
        ConcurrentDictionary<int, TransferReceiveNorm>? prebuiltNorms = null,
        TransferReceiveSheetLayout layout = TransferReceiveSheetLayout.Form11)
    {
        var fields = GetEnabledFields(options);
        if (unpaired.Count == 0 || fields.Count == 0)
        {
            return new Dictionary<int, ClosestMatchResult>();
        }

        var total = unpaired.Count;
        ConcurrentDictionary<int, TransferReceiveNorm> norms;
        ClosestCandidateIndex candidateIndex;
        if (prebuiltCandidateIndex is not null && prebuiltNorms is not null)
        {
            norms = prebuiltNorms;
            foreach (var row in unpaired)
            {
                norms.TryAdd(row.Id, CreateNorm(row));
            }

            candidateIndex = prebuiltCandidateIndex;
            progress?.ReportNow(0, total, $"поиск ближайших совпадений: 0 из {total}");
        }
        else
        {
            progress?.Status($"подготовка поиска совпадений: нормализация ({total} непарных)…");
            norms = prebuiltNorms ?? BuildClosestNormCache(unpaired, opsByOrgOkpo);
            progress?.Status("подготовка поиска совпадений: индекс кандидатов…");
            candidateIndex = prebuiltCandidateIndex
                             ?? ClosestCandidateIndex.Build(opsByOrgOkpo, options.CheckOperationDate);
            progress?.ReportNow(0, total, $"поиск ближайших совпадений: 0 из {total}");
        }

        var fieldCount = fields.Count;
        var pasIdx = IndexOfField(fields, TransferReceiveField.PassportNumber);
        var facIdx = IndexOfField(fields, TransferReceiveField.FactoryNumber);
        var filterByDate = options.CheckOperationDate;

        var matches = WeightedClosestMatchEngine.FindBestMatchesPerSource(
            unpaired,
            fields,
            static source => source.Id,
            static candidate => candidate.Id,
            source => norms.TryGetValue(source.Id, out var sourceNorm)
                ? sourceNorm.OpDateDayNumber
                : DateOnly.TryParse(source.OpDate, out var parsed) ? parsed.DayNumber : null,
            source =>
            {
                if (!candidateIndex.TryGetCandidates(source, out var set))
                {
                    return ClosestReferenceIndex.CandidateSet<TransferReceiveDto>.Empty;
                }

                return set;
            },
            (source, field, _) => GetFieldWeight(field, source, layout),
            (source, candidate, field, _) =>
            {
                if (!norms.TryGetValue(source.Id, out var sourceNorm))
                {
                    sourceNorm = CreateNorm(source);
                    norms[source.Id] = sourceNorm;
                }

                if (!norms.TryGetValue(candidate.Id, out var candidateNorm))
                {
                    candidateNorm = CreateNorm(candidate);
                    norms[candidate.Id] = candidateNorm;
                }

                return FieldSimilarityOf(
                    source, candidate, sourceNorm, candidateNorm, field, source.OrgOkpo);
            },
            (source, candidate) =>
            {
                if (!norms.TryGetValue(source.Id, out var sourceNorm))
                {
                    sourceNorm = CreateNorm(source);
                    norms[source.Id] = sourceNorm;
                }

                if (!norms.TryGetValue(candidate.Id, out var candidateNorm))
                {
                    candidateNorm = CreateNorm(candidate);
                    norms[candidate.Id] = candidateNorm;
                }

                return OperationDateDayDeltaCached(
                    sourceNorm, candidateNorm, source.OpDate, candidate.OpDate);
            },
            filterByDate,
            OperationDateToleranceDays,
            applyBonus: (source, _, levels) =>
            {
                if (layout == TransferReceiveSheetLayout.Form16)
                {
                    return (0, 0);
                }

                if (!SerialNumbersAreEmpty(source)
                    && pasIdx >= 0
                    && facIdx >= 0
                    && levels[pasIdx] == FieldMatchLevel.Exact
                    && levels[facIdx] == FieldMatchLevel.Exact)
                {
                    return (BothIdentifiersExactBonus, BothIdentifiersExactBonus);
                }

                return (0, 0);
            },
            skipCandidate: (source, candidate) =>
            {
                if (candidate.Id == source.Id)
                {
                    return true;
                }

                if (!filterByDate)
                {
                    return false;
                }

                if (!norms.TryGetValue(source.Id, out var sourceNorm))
                {
                    sourceNorm = CreateNorm(source);
                    norms[source.Id] = sourceNorm;
                }

                if (!norms.TryGetValue(candidate.Id, out var candidateNorm))
                {
                    candidateNorm = CreateNorm(candidate);
                    norms[candidate.Id] = candidateNorm;
                }

                return !DatesWithinToleranceCached(
                    sourceNorm, candidateNorm, source.OpDate, candidate.OpDate);
            },
            onProgress: (done, count) =>
                progress?.Report(done, count, $"поиск ближайших совпадений: {done} из {count}"));

        var result = new Dictionary<int, ClosestMatchResult>(matches.Count);
        var sourceById = unpaired.ToDictionary(row => row.Id);
        foreach (var (id, match) in matches)
        {
            var fieldLevels = match.FieldLevels;
            if (sourceById.TryGetValue(id, out var sourceRow)
                && (IsStatusRaoExemptOpCode(sourceRow.OpCode)
                    || IsStatusRaoExemptOpCode(match.Candidate.OpCode)))
            {
                fieldLevels = fieldLevels
                    .Where(kv => kv.Key != TransferReceiveField.StatusRao)
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            var exactMap = new Dictionary<TransferReceiveField, bool>(fieldLevels.Count);
            foreach (var (field, level) in fieldLevels)
            {
                exactMap[field] = level == FieldMatchLevel.Exact;
            }

            result[id] = new ClosestMatchResult(
                match.Candidate,
                fieldLevels,
                exactMap,
                match.ConfidencePercent,
                match.RawScore);
        }

        return result;
    }

    private static ConcurrentDictionary<int, TransferReceiveNorm> BuildClosestNormCache(
        List<TransferReceiveDto> unpaired,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo)
    {
        var norms = new ConcurrentDictionary<int, TransferReceiveNorm>();
        foreach (var row in unpaired)
        {
            norms[row.Id] = CreateNorm(row);
        }

        foreach (var list in opsByOrgOkpo.Values)
        {
            foreach (var row in list)
            {
                norms.TryAdd(row.Id, CreateNorm(row));
            }
        }

        return norms;
    }

    private static bool DatesWithinToleranceCached(
        TransferReceiveNorm left,
        TransferReceiveNorm right,
        string? leftRaw,
        string? rightRaw)
    {
        if (left.OpDateDayNumber is int leftDay && right.OpDateDayNumber is int rightDay)
        {
            return Math.Abs(leftDay - rightDay) <= OperationDateToleranceDays;
        }

        return DateWithinTolerance(leftRaw, rightRaw);
    }

    private static int OperationDateDayDeltaCached(
        TransferReceiveNorm left,
        TransferReceiveNorm right,
        string? leftRaw,
        string? rightRaw)
    {
        if (left.OpDateDayNumber is int leftDay && right.OpDateDayNumber is int rightDay)
        {
            return Math.Abs(leftDay - rightDay);
        }

        return OperationDateDayDelta(leftRaw, rightRaw);
    }

    /// <summary>
    /// Пул кандидатов closest: по ОКПО контрагента, направлению (приём/передача)
    /// и (при включённой дате) по дню операции для окна ±N дней.
    /// </summary>
    private sealed class ClosestCandidateIndex
    {
        private readonly Dictionary<string, OkpoDirectionSets> _byOkpo;

        private ClosestCandidateIndex(Dictionary<string, OkpoDirectionSets> byOkpo) =>
            _byOkpo = byOkpo;

        public static ClosestCandidateIndex Build(
            IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
            bool indexByDate)
        {
            var byOkpo = new Dictionary<string, OkpoDirectionSets>(opsByOrgOkpo.Count, StringComparer.Ordinal);
            foreach (var (okpo, ops) in opsByOrgOkpo)
            {
                var transfers = new List<TransferReceiveDto>();
                var receives = new List<TransferReceiveDto>();
                foreach (var op in ops)
                {
                    if (op.IsTransfer)
                    {
                        transfers.Add(op);
                    }
                    else
                    {
                        receives.Add(op);
                    }
                }

                byOkpo[okpo] = new OkpoDirectionSets(
                    ClosestReferenceIndex.CandidateSet<TransferReceiveDto>.Create(
                        transfers, static op => op.OpDate, indexByDate),
                    ClosestReferenceIndex.CandidateSet<TransferReceiveDto>.Create(
                        receives, static op => op.OpDate, indexByDate));
            }

            return new ClosestCandidateIndex(byOkpo);
        }

        public bool TryGetCandidates(
            TransferReceiveDto source,
            out ClosestReferenceIndex.CandidateSet<TransferReceiveDto> set)
        {
            foreach (var key in OkpoIndexKeys(source.ProviderOrRecieverOkpo))
            {
                if (!_byOkpo.TryGetValue(key, out var pools))
                {
                    continue;
                }

                // Передача ищет приём у контрагента и наоборот.
                set = source.IsTransfer ? pools.Receives : pools.Transfers;
                if (set.All.Count > 0)
                {
                    return true;
                }
            }

            set = ClosestReferenceIndex.CandidateSet<TransferReceiveDto>.Empty;
            return false;
        }

        private sealed class OkpoDirectionSets(
            ClosestReferenceIndex.CandidateSet<TransferReceiveDto> transfers,
            ClosestReferenceIndex.CandidateSet<TransferReceiveDto> receives)
        {
            public ClosestReferenceIndex.CandidateSet<TransferReceiveDto> Transfers { get; } = transfers;
            public ClosestReferenceIndex.CandidateSet<TransferReceiveDto> Receives { get; } = receives;
        }
    }

    private static int IndexOfField(List<TransferReceiveField> fields, TransferReceiveField field)
    {
        for (var i = 0; i < fields.Count; i++)
        {
            if (fields[i] == field)
            {
                return i;
            }
        }

        return -1;
    }

    private static int OperationDateDayDelta(string? left, string? right)
    {
        if (DateOnly.TryParse(left, out var leftDate) && DateOnly.TryParse(right, out var rightDate))
        {
            return Math.Abs(leftDate.DayNumber - rightDate.DayNumber);
        }

        return int.MaxValue / 4;
    }

    /// <summary>
    /// Closest: количество сравнивается построчно (как в Pairing41).
    /// Для пустых серий парность считается по сумме qty, а подсветка — по числам в самой строке.
    /// </summary>
    private static bool QuantityMatchesForClosest(TransferReceiveNorm left, TransferReceiveNorm right) =>
        left.Quantity == right.Quantity;

    private static List<TransferReceiveField> GetEnabledFields(TransferReceiveFormParams options)
    {
        var fields = new List<TransferReceiveField>(13);
        if (options.CheckOperationCode) fields.Add(TransferReceiveField.OperationCode);
        if (options.CheckOperationDate) fields.Add(TransferReceiveField.OperationDate);
        if (options.CheckCodeRao) fields.Add(TransferReceiveField.CodeRao);
        if (options.CheckPassportNumber) fields.Add(TransferReceiveField.PassportNumber);
        if (options.CheckType) fields.Add(TransferReceiveField.Type);
        if (options.CheckRadionuclids) fields.Add(TransferReceiveField.Radionuclids);
        if (options.CheckFactoryNumber) fields.Add(TransferReceiveField.FactoryNumber);
        if (options.CheckQuantity) fields.Add(TransferReceiveField.Quantity);
        if (options.CheckAggregateState) fields.Add(TransferReceiveField.AggregateState);
        if (options.CheckSort) fields.Add(TransferReceiveField.Sort);
        if (options.CheckActivity) fields.Add(TransferReceiveField.Activity);
        if (options.CheckTritiumActivity) fields.Add(TransferReceiveField.TritiumActivity);
        if (options.CheckBetaGammaActivity) fields.Add(TransferReceiveField.BetaGammaActivity);
        if (options.CheckAlphaActivity) fields.Add(TransferReceiveField.AlphaActivity);
        if (options.CheckTransuraniumActivity) fields.Add(TransferReceiveField.TransuraniumActivity);
        if (options.CheckActivityMeasurementDate) fields.Add(TransferReceiveField.ActivityMeasurementDate);
        if (options.CheckMass) fields.Add(TransferReceiveField.Mass);
        if (options.CheckVolume) fields.Add(TransferReceiveField.Volume);
        if (options.CheckCreatorOkpo) fields.Add(TransferReceiveField.CreatorOkpo);
        if (options.CheckCreationDate) fields.Add(TransferReceiveField.CreationDate);
        if (options.CheckProviderOrRecieverOkpo) fields.Add(TransferReceiveField.ProviderOrRecieverOkpo);
        if (options.CheckPackType) fields.Add(TransferReceiveField.PackType);
        if (options.CheckPackNumber) fields.Add(TransferReceiveField.PackNumber);
        if (options.CheckStatusRao) fields.Add(TransferReceiveField.StatusRao);
        if (options.CheckPackName) fields.Add(TransferReceiveField.PackName);
        if (options.CheckSubsidy) fields.Add(TransferReceiveField.Subsidy);
        if (options.CheckFcpNumber) fields.Add(TransferReceiveField.FcpNumber);
        return fields;
    }

    private static TransferReceiveNorm CreateNorm(TransferReceiveDto row)
    {
        int? opDateDay = DateOnly.TryParse(row.OpDate, out var opDate) ? opDate.DayNumber : null;
        return new TransferReceiveNorm
        {
            OpCode = row.OpCode.Trim(),
            OpDateRaw = row.OpDate,
            OpDateDayNumber = opDateDay,
            PasNum = NormalizeSerialNumber(row.PasNum),
            FacNum = NormalizeSerialNumber(row.FacNum),
            Type = NormalizeNumber(row.Type),
            Radionuclids = NormalizeRads(row.Radionuclids),
            PackType = NormalizeNumber(row.PackType),
            PackNumber = NormalizeNumber(row.PackNumber),
            StatusRao = NormalizeNumber(row.StatusRao),
            CodeRao = NormalizeNumber(row.CodeRao),
            PackName = NormalizeNumber(row.PackName),
            Subsidy = NormalizeNumber(row.Subsidy),
            FcpNumber = NormalizeSerialNumber(row.FcpNumber),
            ProviderOrRecieverOkpo = NormalizeNumber(row.ProviderOrRecieverOkpo),
            OrgOkpo = NormalizeNumber(row.OrgOkpo),
            CreatorOkpo = NormalizeNumber(row.CreatorOkpo),
            CreationDate = NormalizeDate(row.CreationDate),
            Activity = row.Activity ?? string.Empty,
            TritiumActivity = row.TritiumActivity ?? string.Empty,
            BetaGammaActivity = row.BetaGammaActivity ?? string.Empty,
            AlphaActivity = row.AlphaActivity ?? string.Empty,
            TransuraniumActivity = row.TransuraniumActivity ?? string.Empty,
            Mass = row.Mass ?? string.Empty,
            Volume = row.Volume ?? string.Empty,
            ActivityMeasurementDate = NormalizeDate(row.ActivityMeasurementDate),
            Quantity = GetQuantityForComparison(row),
            AggregateState = row.AggregateState,
            Sort = row.Sort
        };
    }

    private sealed class ClosestMatchResult(
        TransferReceiveDto candidate,
        IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel> fieldLevels,
        IReadOnlyDictionary<TransferReceiveField, bool> fieldMatches,
        int confidencePercent,
        double rawScore)
    {
        public TransferReceiveDto Candidate { get; } = candidate;
        public IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel> FieldLevels { get; } = fieldLevels;

        /// <summary>Exact-only карта (зелёный) — для обратной совместимости тестов.</summary>
        public IReadOnlyDictionary<TransferReceiveField, bool> FieldMatches { get; } = fieldMatches;

        /// <summary>Индекс схожести 0–99 (нормированный взвешенный soft-score; 100 не показываем).</summary>
        public int ConfidencePercent { get; } = confidencePercent;

        public double RawScore { get; } = rawScore;
    }

    public enum TransferReceiveField
    {
        OperationCode,
        OperationDate,
        PassportNumber,
        Type,
        Radionuclids,
        FactoryNumber,
        Quantity,
        AggregateState,
        Sort,
        Activity,
        ActivityMeasurementDate,
        Mass,
        Volume,
        CreatorOkpo,
        CreationDate,
        PackType,
        PackNumber,
        ProviderOrRecieverOkpo,
        StatusRao,
        CodeRao,
        PackName,
        Subsidy,
        FcpNumber,
        TritiumActivity,
        BetaGammaActivity,
        AlphaActivity,
        TransuraniumActivity
    }

    private sealed class TransferReceiveNorm
    {
        public required string OpCode { get; init; }
        public required string OpDateRaw { get; init; }
        public int? OpDateDayNumber { get; init; }
        public required string PasNum { get; init; }
        public required string FacNum { get; init; }
        public required string Type { get; init; }
        public required string Radionuclids { get; init; }
        public required string PackType { get; init; }
        public required string PackNumber { get; init; }
        public required string StatusRao { get; init; }
        public required string CodeRao { get; init; }
        public required string PackName { get; init; }
        public required string Subsidy { get; init; }
        public required string FcpNumber { get; init; }
        public required string ProviderOrRecieverOkpo { get; init; }
        public required string OrgOkpo { get; init; }
        public required string CreatorOkpo { get; init; }
        public required string CreationDate { get; init; }
        public required string Activity { get; init; }
        public required string TritiumActivity { get; init; }
        public required string BetaGammaActivity { get; init; }
        public required string AlphaActivity { get; init; }
        public required string TransuraniumActivity { get; init; }
        public required string Mass { get; init; }
        public required string Volume { get; init; }
        public required string ActivityMeasurementDate { get; init; }
        public required int Quantity { get; init; }
        public byte? AggregateState { get; init; }
        public byte? Sort { get; init; }
    }

    #endregion
}
