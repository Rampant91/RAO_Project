using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

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
        ConcurrentDictionary<int, TransferReceiveNorm>? prebuiltNorms = null)
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
        var done = 0;
        var bag = new ConcurrentDictionary<int, ClosestMatchResult>();

        void ProcessOne(TransferReceiveDto source)
        {
            try
            {
                if (!candidateIndex.TryGetCandidates(source, out var candidateSet))
                {
                    return;
                }

                if (!norms.TryGetValue(source.Id, out var sourceNorm))
                {
                    sourceNorm = CreateNorm(source);
                    norms[source.Id] = sourceNorm;
                }

                var weights = new double[fieldCount];
                var maxWeightBase = 0.0;
                for (var i = 0; i < fieldCount; i++)
                {
                    weights[i] = GetFieldWeight(fields[i], source);
                    maxWeightBase += weights[i];
                }

                var sourceHasSerials = !SerialNumbersAreEmpty(source);
                var scratchLevels = new FieldMatchLevel[fieldCount];
                var bestLevels = new FieldMatchLevel[fieldCount];
                var bestScore = double.NegativeInfinity;
                var bestMaxWeight = 0.0;
                TransferReceiveDto? bestCandidate = null;
                var bestDateDelta = int.MaxValue;

                void Consider(TransferReceiveDto candidate)
                {
                    if (candidate.Id == source.Id)
                    {
                        return;
                    }

                    if (!norms.TryGetValue(candidate.Id, out var candidateNorm))
                    {
                        candidateNorm = CreateNorm(candidate);
                        norms[candidate.Id] = candidateNorm;
                    }

                    if (filterByDate
                        && !DatesWithinToleranceCached(sourceNorm, candidateNorm, source.OpDate, candidate.OpDate))
                    {
                        return;
                    }

                    var weighted = 0.0;
                    var maxWeight = maxWeightBase;
                    for (var i = 0; i < fieldCount; i++)
                    {
                        var field = fields[i];
                        var weight = weights[i];
                        var sim = FieldSimilarityOf(
                            source, candidate, sourceNorm, candidateNorm, field, source.OrgOkpo);
                        scratchLevels[i] = sim.Level;
                        weighted += weight * sim.Score;
                    }

                    if (sourceHasSerials
                        && pasIdx >= 0
                        && facIdx >= 0
                        && scratchLevels[pasIdx] == FieldMatchLevel.Exact
                        && scratchLevels[facIdx] == FieldMatchLevel.Exact)
                    {
                        weighted += BothIdentifiersExactBonus;
                        maxWeight += BothIdentifiersExactBonus;
                    }

                    var dateDelta = OperationDateDayDeltaCached(sourceNorm, candidateNorm, source.OpDate, candidate.OpDate);
                    var better = weighted > bestScore + 1e-9
                                 || (Math.Abs(weighted - bestScore) <= 1e-9
                                     && (dateDelta < bestDateDelta
                                         || (dateDelta == bestDateDelta
                                             && bestCandidate is not null
                                             && candidate.Id < bestCandidate.Id)));

                    if (better)
                    {
                        bestScore = weighted;
                        bestMaxWeight = maxWeight;
                        bestCandidate = candidate;
                        bestDateDelta = dateDelta;
                        Array.Copy(scratchLevels, bestLevels, fieldCount);
                    }
                }

                if (filterByDate
                    && sourceNorm.OpDateDayNumber is int sourceDay
                    && candidateSet.ByDay is not null)
                {
                    for (var day = sourceDay - OperationDateToleranceDays;
                         day <= sourceDay + OperationDateToleranceDays;
                         day++)
                    {
                        if (!candidateSet.ByDay.TryGetValue(day, out var dayList))
                        {
                            continue;
                        }

                        foreach (var candidate in dayList)
                        {
                            Consider(candidate);
                        }
                    }

                    foreach (var candidate in candidateSet.Undated)
                    {
                        Consider(candidate);
                    }
                }
                else
                {
                    foreach (var candidate in candidateSet.All)
                    {
                        Consider(candidate);
                    }
                }

                if (bestCandidate is not null && bestMaxWeight > 0)
                {
                    var map = new Dictionary<TransferReceiveField, FieldMatchLevel>(fieldCount);
                    var exactMap = new Dictionary<TransferReceiveField, bool>(fieldCount);
                    for (var i = 0; i < fieldCount; i++)
                    {
                        map[fields[i]] = bestLevels[i];
                        exactMap[fields[i]] = bestLevels[i] == FieldMatchLevel.Exact;
                    }

                    var confidence = (int)Math.Round(100.0 * bestScore / bestMaxWeight);
                    confidence = Math.Clamp(confidence, 0, 100);
                    bag[source.Id] = new ClosestMatchResult(
                        bestCandidate, map, exactMap, confidence, bestScore);
                }
            }
            finally
            {
                var completed = Interlocked.Increment(ref done);
                progress?.Report(completed, total, $"поиск ближайших совпадений: {completed} из {total}");
            }
        }

        if (total == 1)
        {
            ProcessOne(unpaired[0]);
        }
        else
        {
            Parallel.ForEach(unpaired, ProcessOne);
        }

        return new Dictionary<int, ClosestMatchResult>(bag);
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
                    CandidateSet.Create(transfers, indexByDate),
                    CandidateSet.Create(receives, indexByDate));
            }

            return new ClosestCandidateIndex(byOkpo);
        }

        public bool TryGetCandidates(TransferReceiveDto source, out CandidateSet set)
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

            set = CandidateSet.Empty;
            return false;
        }

        private sealed class OkpoDirectionSets(CandidateSet transfers, CandidateSet receives)
        {
            public CandidateSet Transfers { get; } = transfers;
            public CandidateSet Receives { get; } = receives;
        }

        public sealed class CandidateSet
        {
            public static CandidateSet Empty { get; } = new([], null, []);

            public List<TransferReceiveDto> All { get; }
            public Dictionary<int, List<TransferReceiveDto>>? ByDay { get; }
            public List<TransferReceiveDto> Undated { get; }

            private CandidateSet(
                List<TransferReceiveDto> all,
                Dictionary<int, List<TransferReceiveDto>>? byDay,
                List<TransferReceiveDto> undated)
            {
                All = all;
                ByDay = byDay;
                Undated = undated;
            }

            public static CandidateSet Create(List<TransferReceiveDto> ops, bool indexByDate)
            {
                if (!indexByDate || ops.Count == 0)
                {
                    return new CandidateSet(ops, null, []);
                }

                var byDay = new Dictionary<int, List<TransferReceiveDto>>();
                var undated = new List<TransferReceiveDto>();
                foreach (var op in ops)
                {
                    if (DateOnly.TryParse(op.OpDate, out var date))
                    {
                        var day = date.DayNumber;
                        if (!byDay.TryGetValue(day, out var list))
                        {
                            list = [];
                            byDay[day] = list;
                        }

                        list.Add(op);
                    }
                    else
                    {
                        undated.Add(op);
                    }
                }

                return new CandidateSet(ops, byDay, undated);
            }
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
        if (options.CheckPassportNumber) fields.Add(TransferReceiveField.PassportNumber);
        if (options.CheckType) fields.Add(TransferReceiveField.Type);
        if (options.CheckRadionuclids) fields.Add(TransferReceiveField.Radionuclids);
        if (options.CheckFactoryNumber) fields.Add(TransferReceiveField.FactoryNumber);
        if (options.CheckQuantity) fields.Add(TransferReceiveField.Quantity);
        if (options.CheckAggregateState) fields.Add(TransferReceiveField.AggregateState);
        if (options.CheckSort) fields.Add(TransferReceiveField.Sort);
        if (options.CheckActivity) fields.Add(TransferReceiveField.Activity);
        if (options.CheckActivityMeasurementDate) fields.Add(TransferReceiveField.ActivityMeasurementDate);
        if (options.CheckMass) fields.Add(TransferReceiveField.Mass);
        if (options.CheckVolume) fields.Add(TransferReceiveField.Volume);
        if (options.CheckCreatorOkpo) fields.Add(TransferReceiveField.CreatorOkpo);
        if (options.CheckCreationDate) fields.Add(TransferReceiveField.CreationDate);
        if (options.CheckProviderOrRecieverOkpo) fields.Add(TransferReceiveField.ProviderOrRecieverOkpo);
        if (options.CheckPackType) fields.Add(TransferReceiveField.PackType);
        if (options.CheckPackNumber) fields.Add(TransferReceiveField.PackNumber);
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
            ProviderOrRecieverOkpo = NormalizeNumber(row.ProviderOrRecieverOkpo),
            OrgOkpo = NormalizeNumber(row.OrgOkpo),
            CreatorOkpo = NormalizeNumber(row.CreatorOkpo),
            CreationDate = NormalizeDate(row.CreationDate),
            Activity = row.Activity ?? string.Empty,
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

        /// <summary>Индекс схожести 0–100 (нормированный взвешенный soft-score).</summary>
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
        ProviderOrRecieverOkpo
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
        public required string ProviderOrRecieverOkpo { get; init; }
        public required string OrgOkpo { get; init; }
        public required string CreatorOkpo { get; init; }
        public required string CreationDate { get; init; }
        public required string Activity { get; init; }
        public required string Mass { get; init; }
        public required string Volume { get; init; }
        public required string ActivityMeasurementDate { get; init; }
        public required int Quantity { get; init; }
        public byte? AggregateState { get; init; }
        public byte? Sort { get; init; }
    }

    #endregion
}
