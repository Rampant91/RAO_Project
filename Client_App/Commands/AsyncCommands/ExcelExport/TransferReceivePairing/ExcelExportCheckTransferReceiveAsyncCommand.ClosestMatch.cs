using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Closest-match state

    private Dictionary<TransferReceiveFormId, Dictionary<int, ClosestMatchResult>> _closestByForm = new();
    private TransferReceiveParamsSet _currentParams = new(new TransferReceiveFormParams(), DefaultForm13Params());

    #endregion

    #region Closest match

    /// <summary>
    /// Для каждой непарной строки — ближайший кандидат у контрагента:
    /// взвешенный soft-score по полям (не число точных совпадений).
    /// </summary>
    private static Dictionary<int, ClosestMatchResult> BuildClosestMatchResults(
        List<TransferReceiveDto> unpaired,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        TransferReceiveFormParams options,
        ProgressReporter? progress = null)
    {
        var fields = GetEnabledFields(options);
        if (unpaired.Count == 0 || fields.Count == 0)
        {
            return new Dictionary<int, ClosestMatchResult>();
        }

        var result = new Dictionary<int, ClosestMatchResult>(unpaired.Count);
        var fieldCount = fields.Count;
        var bestLevels = new FieldMatchLevel[fieldCount];
        var scratchLevels = new FieldMatchLevel[fieldCount];
        var total = unpaired.Count;
        var done = 0;

        foreach (var source in unpaired)
        {
            var candidates = GetCounterpartCandidates(source, source.IsTransfer, opsByOrgOkpo);
            if (candidates.Count == 0)
            {
                done++;
                progress?.Report(done, total, $"поиск ближайших совпадений: {done} из {total}");
                continue;
            }

            var sourceNorm = CreateNorm(source);
            var bestScore = double.NegativeInfinity;
            var bestMaxWeight = 0.0;
            TransferReceiveDto? bestCandidate = null;
            var bestDateDelta = int.MaxValue;

            foreach (var candidate in candidates)
            {
                if (candidate.Id == source.Id)
                {
                    continue;
                }

                if (options.CheckOperationDate
                    && !DateWithinTolerance(source.OpDate, candidate.OpDate))
                {
                    continue;
                }

                var candidateNorm = CreateNorm(candidate);
                var weighted = 0.0;
                var maxWeight = 0.0;
                for (var i = 0; i < fieldCount; i++)
                {
                    var field = fields[i];
                    var weight = GetFieldWeight(field, source);
                    maxWeight += weight;
                    var sim = FieldSimilarityOf(
                        source, candidate, sourceNorm, candidateNorm, field, source.OrgOkpo);
                    scratchLevels[i] = sim.Level;
                    weighted += weight * sim.Score;
                }

                if (!SerialNumbersAreEmpty(source))
                {
                    var pasIdx = IndexOfField(fields, TransferReceiveField.PassportNumber);
                    var facIdx = IndexOfField(fields, TransferReceiveField.FactoryNumber);
                    if (pasIdx >= 0
                        && facIdx >= 0
                        && scratchLevels[pasIdx] == FieldMatchLevel.Exact
                        && scratchLevels[facIdx] == FieldMatchLevel.Exact)
                    {
                        weighted += BothIdentifiersExactBonus;
                        maxWeight += BothIdentifiersExactBonus;
                    }
                }

                var dateDelta = OperationDateDayDelta(source.OpDate, candidate.OpDate);
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
                result[source.Id] = new ClosestMatchResult(bestCandidate, map, exactMap, confidence, bestScore);
            }

            done++;
            progress?.Report(done, total, $"поиск ближайших совпадений: {done} из {total}");
        }

        return result;
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
        if (options.CheckActivity) fields.Add(TransferReceiveField.Activity);
        if (options.CheckCreatorOkpo) fields.Add(TransferReceiveField.CreatorOkpo);
        if (options.CheckCreationDate) fields.Add(TransferReceiveField.CreationDate);
        if (options.CheckProviderOrRecieverOkpo) fields.Add(TransferReceiveField.ProviderOrRecieverOkpo);
        if (options.CheckPackNumber) fields.Add(TransferReceiveField.PackNumber);
        return fields;
    }

    private static TransferReceiveNorm CreateNorm(TransferReceiveDto row) => new()
    {
        OpCode = row.OpCode.Trim(),
        OpDateRaw = row.OpDate,
        PasNum = NormalizeSerialNumber(row.PasNum),
        FacNum = NormalizeSerialNumber(row.FacNum),
        Type = NormalizeNumber(row.Type),
        Radionuclids = NormalizeRads(row.Radionuclids),
        PackNumber = NormalizeNumber(row.PackNumber),
        ProviderOrRecieverOkpo = NormalizeNumber(row.ProviderOrRecieverOkpo),
        OrgOkpo = NormalizeNumber(row.OrgOkpo),
        CreatorOkpo = NormalizeNumber(row.CreatorOkpo),
        CreationDate = NormalizeDate(row.CreationDate),
        Activity = row.Activity ?? string.Empty,
        Quantity = GetQuantityForComparison(row),
        AggregateState = row.AggregateState
    };

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
        Activity,
        CreatorOkpo,
        CreationDate,
        PackNumber,
        ProviderOrRecieverOkpo
    }

    private sealed class TransferReceiveNorm
    {
        public required string OpCode { get; init; }
        public required string OpDateRaw { get; init; }
        public required string PasNum { get; init; }
        public required string FacNum { get; init; }
        public required string Type { get; init; }
        public required string Radionuclids { get; init; }
        public required string PackNumber { get; init; }
        public required string ProviderOrRecieverOkpo { get; init; }
        public required string OrgOkpo { get; init; }
        public required string CreatorOkpo { get; init; }
        public required string CreationDate { get; init; }
        public required string Activity { get; init; }
        public required int Quantity { get; init; }
        public byte? AggregateState { get; init; }
    }

    #endregion
}
