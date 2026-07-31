using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Closest-match state

    private Dictionary<int, ClosestMatchResult> _form11ClosestMatches = new();
    private Dictionary<int, ClosestMatchResult> _form13ClosestMatches = new();
    private TransferReceiveParamsSet _currentParams = new(new(), DefaultForm13Params());

    #endregion

    #region Closest form 1.1 / 1.3

    /// <summary>
    /// Для каждой непарной строки — ближайший кандидат у контрагента и карта совпадений полей.
    /// </summary>
    private static Dictionary<int, ClosestMatchResult> BuildClosestMatchResults(
        List<TransferReceiveDto> unpaired,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        TransferReceive11Params options,
        ProgressReporter? progress = null)
    {
        var fields = GetEnabledFields(options);
        if (unpaired.Count == 0 || fields.Count == 0)
        {
            return new Dictionary<int, ClosestMatchResult>();
        }

        var result = new Dictionary<int, ClosestMatchResult>(unpaired.Count);
        var fieldCount = fields.Count;
        var bestFlags = new bool[fieldCount];
        var scratchFlags = new bool[fieldCount];
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
            var bestScore = -1;
            TransferReceiveDto? bestCandidate = null;

            foreach (var candidate in candidates)
            {
                if (candidate.Id == source.Id)
                {
                    continue;
                }

                // Дата операции — фильтр поиска (±N дней). Подсветка даты — только точное совпадение.
                if (options.CheckOperationDate
                    && !DateWithinTolerance(source.OpDate, candidate.OpDate))
                {
                    continue;
                }

                var candidateNorm = CreateNorm(candidate);
                var score = 0;
                for (var i = 0; i < fieldCount; i++)
                {
                    var matched = FieldMatches(sourceNorm, candidateNorm, fields[i], source.OrgOkpo);
                    scratchFlags[i] = matched;
                    if (matched)
                    {
                        score++;
                    }
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestCandidate = candidate;
                    Array.Copy(scratchFlags, bestFlags, fieldCount);
                    if (bestScore == fieldCount)
                    {
                        break;
                    }
                }
            }

            if (bestScore >= 0 && bestCandidate is not null)
            {
                var map = new Dictionary<TransferReceiveField, bool>(fieldCount);
                for (var i = 0; i < fieldCount; i++)
                {
                    map[fields[i]] = bestFlags[i];
                }

                result[source.Id] = new ClosestMatchResult(bestCandidate, map);
            }

            done++;
            progress?.Report(done, total, $"поиск ближайших совпадений: {done} из {total}");
        }

        return result;
    }

    private static bool FieldMatches(
        TransferReceiveNorm left,
        TransferReceiveNorm right,
        TransferReceiveField field,
        string sourceOrgOkpo) =>
        field switch
        {
            TransferReceiveField.OperationCode => OpCodesArePaired(left.OpCode, right.OpCode),
            TransferReceiveField.OperationDate => DatesEqualExact(left.OpDateRaw, right.OpDateRaw),
            TransferReceiveField.PassportNumber => left.PasNum == right.PasNum,
            TransferReceiveField.Type => left.Type == right.Type,
            TransferReceiveField.Radionuclids => left.Radionuclids == right.Radionuclids,
            TransferReceiveField.FactoryNumber => left.FacNum == right.FacNum,
            TransferReceiveField.Quantity => QuantityMatchesForClosest(left, right),
            TransferReceiveField.AggregateState => left.AggregateState == right.AggregateState,
            TransferReceiveField.Activity => ActivityMatchesNorm(left.Activity, right.Activity),
            TransferReceiveField.CreatorOkpo => left.CreatorOkpo == right.CreatorOkpo,
            TransferReceiveField.CreationDate => left.CreationDate == right.CreationDate,
            TransferReceiveField.PackNumber => left.PackNumber == right.PackNumber,
            TransferReceiveField.ProviderOrRecieverOkpo =>
                right.ProviderOrRecieverOkpo == NormalizeNumber(sourceOrgOkpo)
                || right.ProviderOrRecieverOkpo == left.OrgOkpo,
            _ => false
        };

    /// <summary>
    /// Closest: количество сравнивается построчно (как в Pairing41).
    /// Для пустых серий парность считается по сумме qty, а подсветка — по числам в самой строке
    /// (1↔1 зелёный, 8↔5 красный), иначе qty всегда казалось бы «несовпавшим».
    /// </summary>
    private static bool QuantityMatchesForClosest(TransferReceiveNorm left, TransferReceiveNorm right) =>
        left.Quantity == right.Quantity;

    private static bool ActivityMatchesNorm(string left, string right)
    {
        if (!TryParseActivity(left, out var leftActivity) || !TryParseActivity(right, out var rightActivity))
        {
            return string.Equals(left, right, StringComparison.Ordinal);
        }

        var scale = Math.Max(Math.Abs(leftActivity), Math.Abs(rightActivity));
        if (scale <= double.Epsilon)
        {
            return true;
        }

        return Math.Abs(leftActivity - rightActivity) <= scale * 0.10;
    }

    private static List<TransferReceiveField> GetEnabledFields(TransferReceive11Params options)
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
        IReadOnlyDictionary<TransferReceiveField, bool> fieldMatches)
    {
        public TransferReceiveDto Candidate { get; } = candidate;
        public IReadOnlyDictionary<TransferReceiveField, bool> FieldMatches { get; } = fieldMatches;
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
