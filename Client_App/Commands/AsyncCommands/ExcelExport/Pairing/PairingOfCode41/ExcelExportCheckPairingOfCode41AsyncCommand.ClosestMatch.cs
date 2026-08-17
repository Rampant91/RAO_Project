using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Resources;
using Client_App.Resources.CustomComparers.SnkComparers;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    #region Closest-match state

    private Dictionary<int, ClosestMatchHighlight> _form11ClosestMatchHighlights = new();
    private Dictionary<int, ClosestMatchHighlight> _form15ClosestMatchHighlights = new();
    private Dictionary<int, ClosestMatchResult<Pairing12To16Field>> _form12ClosestMatchHighlights = new();
    private Dictionary<int, ClosestMatchResult<Pairing13To16Field>> _form13ClosestMatchHighlights = new();
    private Dictionary<int, ClosestMatchResult<Pairing14To16Field>> _form14ClosestMatchHighlights = new();
    private Dictionary<int, Form16ClosestMatchHighlight> _form16ClosestMatchHighlights = new();

    #endregion

    #region Closest 1.1 ↔ 1.5

    /// <summary>
    /// Карта closest-match для непарных 1.1/1.5. Пустой reference или нет включённых полей → без подсветки.
    /// </summary>
    private static Dictionary<int, ClosestMatchHighlight> BuildClosestMatchHighlights(
        List<Operation41PairingDto> unpaired,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options,
        ProgressReporter? progress = null)
    {
        var fields = GetEnabledFields(options);
        if (unpaired.Count == 0 || reference.Count == 0 || fields.Count == 0)
        {
            return new Dictionary<int, ClosestMatchHighlight>();
        }

        progress?.ReportNow(0, unpaired.Count, $"поиск ближайших совпадений: 0 из {unpaired.Count}");
        var norms = BuildNormLookup(unpaired, reference);
        var candidateSet = ClosestReferenceIndex.CandidateSet<Operation41PairingDto>.Create(
            reference, static row => row.OpDate, indexByDate: false);
        var fieldList = fields;
        var pasIdx = IndexOfField(fieldList, Pairing11To15Field.PassportNumber);
        var facIdx = IndexOfField(fieldList, Pairing11To15Field.FactoryNumber);

        var matches = WeightedClosestMatchEngine.FindBestMatches(
            unpaired,
            candidateSet,
            fieldList,
            static source => source.Id,
            static candidate => candidate.Id,
            static source => GetOpDateDayNumber(source),
            (source, field, _) => GetFieldWeight11To15(field, source),
            (source, candidate, field, _) =>
                FieldSimilarity11To15(source, candidate, norms[source.Id], norms[candidate.Id], field),
            (source, candidate) => OperationDateDayDelta(source.OpDate, candidate.OpDate),
            filterCandidatesByDate: false,
            dateToleranceDays: ClosestOperationDateToleranceDays,
            applyBonus: (source, _, levels) =>
            {
                if (!SerialNumbersAreEmpty(source)
                    && pasIdx >= 0
                    && facIdx >= 0
                    && levels[pasIdx] == Shared.FieldMatchLevel.Exact
                    && levels[facIdx] == Shared.FieldMatchLevel.Exact)
                {
                    return (BothIdentifiersExactBonus, BothIdentifiersExactBonus);
                }

                return (0, 0);
            },
            onProgress: (done, total) =>
                progress?.Report(done, total, $"поиск ближайших совпадений: {done} из {total}"));

        return matches.ToDictionary(
            kv => kv.Key,
            kv => new ClosestMatchHighlight(
                kv.Value.Candidate,
                kv.Value.FieldLevels,
                kv.Value.ConfidencePercent,
                kv.Value.RawScore));
    }

    private static List<Pairing11To15Field> GetEnabledFields(Pairing11To15Params options)
    {
        var fields = new List<Pairing11To15Field>(17);
        if (options.CheckOperationCode) fields.Add(Pairing11To15Field.OperationCode);
        if (options.CheckOperationDate) fields.Add(Pairing11To15Field.OperationDate);
        if (options.CheckPassportNumber) fields.Add(Pairing11To15Field.PassportNumber);
        if (options.CheckType) fields.Add(Pairing11To15Field.Type);
        if (options.CheckRadionuclids) fields.Add(Pairing11To15Field.Radionuclids);
        if (options.CheckFactoryNumber) fields.Add(Pairing11To15Field.FactoryNumber);
        if (options.CheckActivity) fields.Add(Pairing11To15Field.Activity);
        if (options.CheckQuantity) fields.Add(Pairing11To15Field.Quantity);
        if (options.CheckCreationDate) fields.Add(Pairing11To15Field.CreationDate);
        if (options.CheckDocumentVid) fields.Add(Pairing11To15Field.DocumentVid);
        if (options.CheckDocumentNumber) fields.Add(Pairing11To15Field.DocumentNumber);
        if (options.CheckDocumentDate) fields.Add(Pairing11To15Field.DocumentDate);
        if (options.CheckProviderOrRecieverOkpo) fields.Add(Pairing11To15Field.ProviderOrRecieverOkpo);
        if (options.CheckTransporterOkpo) fields.Add(Pairing11To15Field.TransporterOkpo);
        if (options.CheckPackName) fields.Add(Pairing11To15Field.PackName);
        if (options.CheckPackType) fields.Add(Pairing11To15Field.PackType);
        if (options.CheckPackNumber) fields.Add(Pairing11To15Field.PackNumber);
        return fields;
    }

    public sealed class ClosestMatchHighlight
    {
        internal ClosestMatchHighlight(
            Operation41PairingDto candidate,
            IReadOnlyDictionary<Pairing11To15Field, Shared.FieldMatchLevel> fieldLevels,
            int confidencePercent,
            double rawScore)
        {
            Candidate = candidate;
            FieldLevels = fieldLevels;
            FieldMatches = fieldLevels.ToDictionary(kv => kv.Key, kv => kv.Value == Shared.FieldMatchLevel.Exact);
            ConfidencePercent = confidencePercent;
            RawScore = rawScore;
        }

        internal Operation41PairingDto Candidate { get; }
        public IReadOnlyDictionary<Pairing11To15Field, Shared.FieldMatchLevel> FieldLevels { get; }
        public IReadOnlyDictionary<Pairing11To15Field, bool> FieldMatches { get; }
        public int ConfidencePercent { get; }
        public double RawScore { get; }
    }

    public enum Pairing11To15Field
    {
        OperationCode,
        OperationDate,
        PassportNumber,
        Type,
        Radionuclids,
        FactoryNumber,
        Activity,
        Quantity,
        CreationDate,
        DocumentVid,
        DocumentNumber,
        DocumentDate,
        ProviderOrRecieverOkpo,
        TransporterOkpo,
        PackName,
        PackType,
        PackNumber
    }

    #endregion

    #region Closest 1.2–1.4 ↔ 1.6

    private static Dictionary<int, ClosestMatchResult<Pairing12To16Field>> BuildClosestMatchHighlights12To16(
        List<Operation41PairingDto> unpaired, List<Operation41PairingDto> reference, Pairing12To16Params options,
        ProgressReporter? progress = null) =>
        BuildClosestWeighted(
            unpaired,
            reference,
            GetEnabledFields12To16(options),
            GetFieldWeight12To16,
            FieldSimilarity12To16,
            progress: progress);

    private static Dictionary<int, ClosestMatchResult<Pairing13To16Field>> BuildClosestMatchHighlights13To16(
        List<Operation41PairingDto> unpaired, List<Operation41PairingDto> reference, Pairing13To16Params options,
        ProgressReporter? progress = null) =>
        BuildClosestWeighted(
            unpaired,
            reference,
            GetEnabledFields13To16(options),
            GetFieldWeight13To16,
            FieldSimilarity13To16,
            static (source, candidate) => RaoCodeHelper.AggregateStateMatchesCodeRao(source.AggregateState, candidate.CodeRao),
            progress);

    private static Dictionary<int, ClosestMatchResult<Pairing14To16Field>> BuildClosestMatchHighlights14To16(
        List<Operation41PairingDto> unpaired, List<Operation41PairingDto> reference, Pairing14To16Params options,
        ProgressReporter? progress = null) =>
        BuildClosestWeighted(
            unpaired,
            reference,
            GetEnabledFields14To16(options),
            GetFieldWeight14To16,
            FieldSimilarity14To16,
            static (source, candidate) => RaoCodeHelper.AggregateStateMatchesCodeRao(source.AggregateState, candidate.CodeRao),
            progress);

    private static Dictionary<int, ClosestMatchResult<TField>> BuildClosestWeighted<TField>(
        List<Operation41PairingDto> unpaired,
        List<Operation41PairingDto> reference,
        List<TField> fields,
        Func<TField, double> getWeight,
        Func<Operation41PairingDto, Operation41PairingDto, PairingNorm, PairingNorm, TField, Shared.FieldSimilarity> fieldSimilarity,
        Func<Operation41PairingDto, Operation41PairingDto, bool?>? computeAggregateStateMatch = null,
        ProgressReporter? progress = null)
        where TField : struct, Enum
    {
        if (unpaired.Count == 0 || reference.Count == 0 || fields.Count == 0)
        {
            return new Dictionary<int, ClosestMatchResult<TField>>();
        }

        progress?.ReportNow(0, unpaired.Count, $"поиск ближайших совпадений: 0 из {unpaired.Count}");
        var norms = BuildNormLookup(unpaired, reference);
        var candidateSet = ClosestReferenceIndex.CandidateSet<Operation41PairingDto>.Create(
            reference, static row => row.OpDate, indexByDate: false);

        var matches = WeightedClosestMatchEngine.FindBestMatches(
            unpaired,
            candidateSet,
            fields,
            static source => source.Id,
            static candidate => candidate.Id,
            static source => GetOpDateDayNumber(source),
            (_, field, _) => getWeight(field),
            (source, candidate, field, _) =>
                fieldSimilarity(source, candidate, norms[source.Id], norms[candidate.Id], field),
            (source, candidate) => OperationDateDayDelta(source.OpDate, candidate.OpDate),
            filterCandidatesByDate: false,
            dateToleranceDays: ClosestOperationDateToleranceDays,
            onProgress: (done, total) =>
                progress?.Report(done, total, $"поиск ближайших совпадений: {done} из {total}"));

        var result = new Dictionary<int, ClosestMatchResult<TField>>(matches.Count);
        foreach (var (id, match) in matches)
        {
            var source = unpaired.Find(row => row.Id == id)!;
            var aggregateStateMatch = computeAggregateStateMatch?.Invoke(source, match.Candidate);
            result[id] = new ClosestMatchResult<TField>(
                match.Candidate,
                match.FieldLevels,
                match.ConfidencePercent,
                match.RawScore,
                aggregateStateMatch);
        }

        return result;
    }

    private static bool FieldMatches11To15(PairingNorm left, PairingNorm right, Pairing11To15Field field) =>
        field switch
        {
            Pairing11To15Field.OperationCode => left.OpCode == right.OpCode,
            Pairing11To15Field.OperationDate => left.OpDate == right.OpDate,
            Pairing11To15Field.PassportNumber => left.PasNum == right.PasNum,
            Pairing11To15Field.Type => left.Type == right.Type,
            Pairing11To15Field.Radionuclids => left.Radionuclids == right.Radionuclids,
            Pairing11To15Field.FactoryNumber => left.FacNum == right.FacNum,
            Pairing11To15Field.Activity => NumericTolerance(left.Activity, right.Activity),
            Pairing11To15Field.Quantity => left.Quantity == right.Quantity,
            Pairing11To15Field.CreationDate => left.CreationDate == right.CreationDate,
            Pairing11To15Field.DocumentVid => left.DocumentVid == right.DocumentVid,
            Pairing11To15Field.DocumentNumber => left.DocumentNumber == right.DocumentNumber,
            Pairing11To15Field.DocumentDate => left.DocumentDate == right.DocumentDate,
            Pairing11To15Field.ProviderOrRecieverOkpo => left.ProviderOrRecieverOkpo == right.ProviderOrRecieverOkpo,
            Pairing11To15Field.TransporterOkpo => left.TransporterOkpo == right.TransporterOkpo,
            Pairing11To15Field.PackName => left.PackName == right.PackName,
            Pairing11To15Field.PackType => left.PackType == right.PackType,
            Pairing11To15Field.PackNumber => left.PackNumber == right.PackNumber,
            _ => false
        };

    private static bool FieldMatches12To16(PairingNorm left, PairingNorm right, Pairing12To16Field field) => field switch
    {
        Pairing12To16Field.OperationDate => left.OpDate == right.OpDate,
        Pairing12To16Field.Mass => NumericTolerance(left.Mass, right.Mass),
        Pairing12To16Field.BetaGammaActivity => NumericTolerance(left.BetaGammaActivity, right.BetaGammaActivity),
        Pairing12To16Field.AlphaActivity => NumericTolerance(left.AlphaActivity, right.AlphaActivity),
        Pairing12To16Field.ActivityMeasurementDate => left.ActivityMeasurementDate == right.ActivityMeasurementDate,
        Pairing12To16Field.DocumentVid => left.DocumentVid == right.DocumentVid,
        Pairing12To16Field.DocumentNumber => left.DocumentNumber == right.DocumentNumber,
        Pairing12To16Field.DocumentDate => left.DocumentDate == right.DocumentDate,
        Pairing12To16Field.PackName => left.PackName == right.PackName,
        Pairing12To16Field.PackType => left.PackType == right.PackType,
        Pairing12To16Field.PackNumber => left.PackNumber == right.PackNumber,
        Pairing12To16Field.CodeRao => left.CodeRao == right.CodeRao,
        _ => false
    };

    private static bool FieldMatches13To16(PairingNorm left, PairingNorm right, Pairing13To16Field field) => field switch
    {
        Pairing13To16Field.OperationDate => left.OpDate == right.OpDate,
        Pairing13To16Field.MainRadionuclids => left.MainRadionuclids == right.MainRadionuclids,
        Pairing13To16Field.TritiumActivity => NumericTolerance(left.TritiumActivity, right.TritiumActivity),
        Pairing13To16Field.BetaGammaActivity => NumericTolerance(left.BetaGammaActivity, right.BetaGammaActivity),
        Pairing13To16Field.AlphaActivity => NumericTolerance(left.AlphaActivity, right.AlphaActivity),
        Pairing13To16Field.TransuraniumActivity => NumericTolerance(left.TransuraniumActivity, right.TransuraniumActivity),
        Pairing13To16Field.ActivityMeasurementDate => left.ActivityMeasurementDate == right.ActivityMeasurementDate,
        Pairing13To16Field.DocumentVid => left.DocumentVid == right.DocumentVid,
        Pairing13To16Field.DocumentNumber => left.DocumentNumber == right.DocumentNumber,
        Pairing13To16Field.DocumentDate => left.DocumentDate == right.DocumentDate,
        Pairing13To16Field.PackName => left.PackName == right.PackName,
        Pairing13To16Field.PackType => left.PackType == right.PackType,
        Pairing13To16Field.PackNumber => left.PackNumber == right.PackNumber,
        Pairing13To16Field.CodeRao => left.CodeRao == right.CodeRao,
        _ => false
    };

    private static bool FieldMatches14To16(PairingNorm left, PairingNorm right, Pairing14To16Field field) => field switch
    {
        Pairing14To16Field.OperationDate => left.OpDate == right.OpDate,
        Pairing14To16Field.Volume => NumericTolerance(left.Volume, right.Volume),
        Pairing14To16Field.Mass => NumericTolerance(left.Mass, right.Mass),
        Pairing14To16Field.MainRadionuclids => left.MainRadionuclids == right.MainRadionuclids,
        Pairing14To16Field.TritiumActivity => NumericTolerance(left.TritiumActivity, right.TritiumActivity),
        Pairing14To16Field.BetaGammaActivity => NumericTolerance(left.BetaGammaActivity, right.BetaGammaActivity),
        Pairing14To16Field.AlphaActivity => NumericTolerance(left.AlphaActivity, right.AlphaActivity),
        Pairing14To16Field.TransuraniumActivity => NumericTolerance(left.TransuraniumActivity, right.TransuraniumActivity),
        Pairing14To16Field.ActivityMeasurementDate => left.ActivityMeasurementDate == right.ActivityMeasurementDate,
        Pairing14To16Field.DocumentVid => left.DocumentVid == right.DocumentVid,
        Pairing14To16Field.DocumentNumber => left.DocumentNumber == right.DocumentNumber,
        Pairing14To16Field.DocumentDate => left.DocumentDate == right.DocumentDate,
        Pairing14To16Field.PackName => left.PackName == right.PackName,
        Pairing14To16Field.PackType => left.PackType == right.PackType,
        Pairing14To16Field.PackNumber => left.PackNumber == right.PackNumber,
        Pairing14To16Field.CodeRao => left.CodeRao == right.CodeRao,
        _ => false
    };

    private static List<Pairing12To16Field> GetEnabledFields12To16(Pairing12To16Params o)
    {
        var l = new List<Pairing12To16Field>();
        if (o.CheckOperationDate) l.Add(Pairing12To16Field.OperationDate);
        if (o.CheckMass) l.Add(Pairing12To16Field.Mass);
        if (o.CheckBetaGammaActivity) l.Add(Pairing12To16Field.BetaGammaActivity);
        if (o.CheckAlphaActivity) l.Add(Pairing12To16Field.AlphaActivity);
        if (o.CheckActivityMeasurementDate) l.Add(Pairing12To16Field.ActivityMeasurementDate);
        if (o.CheckDocumentVid) l.Add(Pairing12To16Field.DocumentVid);
        if (o.CheckDocumentNumber) l.Add(Pairing12To16Field.DocumentNumber);
        if (o.CheckDocumentDate) l.Add(Pairing12To16Field.DocumentDate);
        if (o.CheckPackName) l.Add(Pairing12To16Field.PackName);
        if (o.CheckPackType) l.Add(Pairing12To16Field.PackType);
        if (o.CheckPackNumber) l.Add(Pairing12To16Field.PackNumber);
        if (o.CheckCodeRao) l.Add(Pairing12To16Field.CodeRao);
        return l;
    }

    private static List<Pairing13To16Field> GetEnabledFields13To16(Pairing13To16Params o)
    {
        var l = new List<Pairing13To16Field>();
        if (o.CheckOperationDate) l.Add(Pairing13To16Field.OperationDate);
        if (o.CheckMainRadionuclids) l.Add(Pairing13To16Field.MainRadionuclids);
        if (o.CheckTritiumActivity) l.Add(Pairing13To16Field.TritiumActivity);
        if (o.CheckBetaGammaActivity) l.Add(Pairing13To16Field.BetaGammaActivity);
        if (o.CheckAlphaActivity) l.Add(Pairing13To16Field.AlphaActivity);
        if (o.CheckTransuraniumActivity) l.Add(Pairing13To16Field.TransuraniumActivity);
        if (o.CheckActivityMeasurementDate) l.Add(Pairing13To16Field.ActivityMeasurementDate);
        if (o.CheckDocumentVid) l.Add(Pairing13To16Field.DocumentVid);
        if (o.CheckDocumentNumber) l.Add(Pairing13To16Field.DocumentNumber);
        if (o.CheckDocumentDate) l.Add(Pairing13To16Field.DocumentDate);
        if (o.CheckPackName) l.Add(Pairing13To16Field.PackName);
        if (o.CheckPackType) l.Add(Pairing13To16Field.PackType);
        if (o.CheckPackNumber) l.Add(Pairing13To16Field.PackNumber);
        if (o.CheckCodeRao) l.Add(Pairing13To16Field.CodeRao);
        return l;
    }

    private static List<Pairing14To16Field> GetEnabledFields14To16(Pairing14To16Params o)
    {
        var l = new List<Pairing14To16Field>();
        if (o.CheckOperationDate) l.Add(Pairing14To16Field.OperationDate);
        if (o.CheckVolume) l.Add(Pairing14To16Field.Volume);
        if (o.CheckMass) l.Add(Pairing14To16Field.Mass);
        if (o.CheckMainRadionuclids) l.Add(Pairing14To16Field.MainRadionuclids);
        if (o.CheckTritiumActivity) l.Add(Pairing14To16Field.TritiumActivity);
        if (o.CheckBetaGammaActivity) l.Add(Pairing14To16Field.BetaGammaActivity);
        if (o.CheckAlphaActivity) l.Add(Pairing14To16Field.AlphaActivity);
        if (o.CheckTransuraniumActivity) l.Add(Pairing14To16Field.TransuraniumActivity);
        if (o.CheckActivityMeasurementDate) l.Add(Pairing14To16Field.ActivityMeasurementDate);
        if (o.CheckDocumentVid) l.Add(Pairing14To16Field.DocumentVid);
        if (o.CheckDocumentNumber) l.Add(Pairing14To16Field.DocumentNumber);
        if (o.CheckDocumentDate) l.Add(Pairing14To16Field.DocumentDate);
        if (o.CheckPackName) l.Add(Pairing14To16Field.PackName);
        if (o.CheckPackType) l.Add(Pairing14To16Field.PackType);
        if (o.CheckPackNumber) l.Add(Pairing14To16Field.PackNumber);
        if (o.CheckCodeRao) l.Add(Pairing14To16Field.CodeRao);
        return l;
    }

    #endregion

    #region Closest сторона 1.6

    /// <summary>
    /// Closest для непарной 1.6: лучший профиль среди 1.2 / 1.3 / 1.4 (при равенстве — приоритет 1.2).
    /// </summary>
    private static Dictionary<int, Form16ClosestMatchHighlight> BuildClosestMatchHighlights16(
        List<Operation41PairingDto> unpaired,
        List<Operation41PairingDto> form12,
        List<Operation41PairingDto> form13,
        List<Operation41PairingDto> form14,
        Pairing12To16Params pairing12To16Params,
        Pairing13To16Params pairing13To16Params,
        Pairing14To16Params pairing14To16Params,
        ProgressReporter? progress = null)
    {
        var result = new Dictionary<int, Form16ClosestMatchHighlight>();
        if (unpaired.Count == 0)
        {
            return result;
        }

        progress?.ReportNow(0, unpaired.Count, $"поиск ближайших совпадений 1.6: 0 из {unpaired.Count}");
        var fields12 = GetEnabledFields12To16(pairing12To16Params);
        var fields13 = GetEnabledFields13To16(pairing13To16Params);
        var fields14 = GetEnabledFields14To16(pairing14To16Params);
        var norms12 = CreatePairingNorms(form12);
        var norms13 = CreatePairingNorms(form13);
        var norms14 = CreatePairingNorms(form14);

        var done = 0;
        var total = unpaired.Count;
        foreach (var form16 in unpaired)
        {
            var form16Norm = CreatePairingNorm(form16);
            var best12 = FindBestRvMatchWeighted(form16, form16Norm, form12, norms12, fields12, GetFieldWeight12To16, FieldSimilarity12To16);
            var best13 = FindBestRvMatchWeighted(form16, form16Norm, form13, norms13, fields13, GetFieldWeight13To16, FieldSimilarity13To16);
            var best14 = FindBestRvMatchWeighted(form16, form16Norm, form14, norms14, fields14, GetFieldWeight14To16, FieldSimilarity14To16);

            var bestScore = Math.Max(best12.Score, Math.Max(best13.Score, best14.Score));
            if (bestScore >= 0)
            {
                if (Math.Abs(bestScore - best12.Score) <= 1e-9 && best12.Candidate is not null && best12.Levels is not null)
                {
                    result[form16.Id] = new Form16ClosestMatchHighlight(
                        Form16MatchProfile.Form12, best12.Candidate, best12.Levels, null, null, null,
                        best12.Confidence, best12.RawScore);
                }
                else if (Math.Abs(bestScore - best13.Score) <= 1e-9 && best13.Candidate is not null && best13.Levels is not null)
                {
                    var aggregateStateMatch = RaoCodeHelper.AggregateStateMatchesCodeRao(best13.Candidate.AggregateState, form16.CodeRao);
                    result[form16.Id] = new Form16ClosestMatchHighlight(
                        Form16MatchProfile.Form13, best13.Candidate, null, best13.Levels, null, aggregateStateMatch,
                        best13.Confidence, best13.RawScore);
                }
                else if (best14.Candidate is not null && best14.Levels is not null)
                {
                    var aggregateStateMatch = RaoCodeHelper.AggregateStateMatchesCodeRao(best14.Candidate.AggregateState, form16.CodeRao);
                    result[form16.Id] = new Form16ClosestMatchHighlight(
                        Form16MatchProfile.Form14, best14.Candidate, null, null, best14.Levels, aggregateStateMatch,
                        best14.Confidence, best14.RawScore);
                }
            }

            done++;
            progress?.Report(done, total, $"поиск ближайших совпадений 1.6: {done} из {total}");
        }

        return result;
    }

    private readonly record struct WeightedRvMatchResult<TField>(
        double Score,
        int Confidence,
        double RawScore,
        Operation41PairingDto? Candidate,
        Dictionary<TField, Shared.FieldMatchLevel>? Levels)
        where TField : struct, Enum;

    private static WeightedRvMatchResult<TField> FindBestRvMatchWeighted<TField>(
        Operation41PairingDto form16,
        PairingNorm form16Norm,
        List<Operation41PairingDto> rvRows,
        PairingNorm[] rvPool,
        List<TField> fields,
        Func<TField, double> getWeight,
        Func<Operation41PairingDto, Operation41PairingDto, PairingNorm, PairingNorm, TField, Shared.FieldSimilarity> fieldSimilarity)
        where TField : struct, Enum
    {
        if (rvPool.Length == 0 || fields.Count == 0)
        {
            return new WeightedRvMatchResult<TField>(-1, 0, 0, null, null);
        }

        var norms = new Dictionary<int, PairingNorm>(rvRows.Count + 1) { [form16.Id] = form16Norm };
        for (var i = 0; i < rvRows.Count; i++)
        {
            norms[rvRows[i].Id] = rvPool[i];
        }

        var candidateSet = ClosestReferenceIndex.CandidateSet<Operation41PairingDto>.Create(
            rvRows, static row => row.OpDate, indexByDate: false);

        var matches = WeightedClosestMatchEngine.FindBestMatches(
            [form16],
            candidateSet,
            fields,
            static source => source.Id,
            static candidate => candidate.Id,
            _ => GetOpDateDayNumber(form16),
            (_, field, _) => getWeight(field),
            (source, candidate, field, _) =>
                fieldSimilarity(candidate, source, norms[candidate.Id], norms[source.Id], field),
            (_, candidate) => OperationDateDayDelta(candidate.OpDate, form16.OpDate),
            filterCandidatesByDate: false,
            dateToleranceDays: ClosestOperationDateToleranceDays,
            useParallel: false);

        if (!matches.TryGetValue(form16.Id, out var match))
        {
            return new WeightedRvMatchResult<TField>(-1, 0, 0, null, null);
        }

        return new WeightedRvMatchResult<TField>(
            match.RawScore,
            match.ConfidencePercent,
            match.RawScore,
            match.Candidate,
            match.FieldLevels.ToDictionary(kv => kv.Key, kv => kv.Value));
    }

    private static Dictionary<int, PairingNorm> BuildNormLookup(
        IReadOnlyList<Operation41PairingDto> unpaired,
        IReadOnlyList<Operation41PairingDto> reference)
    {
        var norms = new Dictionary<int, PairingNorm>(unpaired.Count + reference.Count);
        foreach (var row in reference)
        {
            norms[row.Id] = CreatePairingNorm(row);
        }

        foreach (var row in unpaired)
        {
            norms[row.Id] = CreatePairingNorm(row);
        }

        return norms;
    }

    private static int? GetOpDateDayNumber(Operation41PairingDto row) =>
        DateOnly.TryParse(row.OpDate, out var date) ? date.DayNumber : null;

    #endregion

    #region Closest-match types

    public enum Form16MatchProfile
    {
        Form12,
        Form13,
        Form14
    }

    public sealed class Form16ClosestMatchHighlight
    {
        internal Form16ClosestMatchHighlight(
            Form16MatchProfile profile,
            Operation41PairingDto candidate,
            IReadOnlyDictionary<Pairing12To16Field, Shared.FieldMatchLevel>? levels12,
            IReadOnlyDictionary<Pairing13To16Field, Shared.FieldMatchLevel>? levels13,
            IReadOnlyDictionary<Pairing14To16Field, Shared.FieldMatchLevel>? levels14,
            bool? aggregateStateMatchesCodeRao = null,
            int confidencePercent = 0,
            double rawScore = 0)
        {
            Profile = profile;
            Candidate = candidate;
            Levels12 = levels12;
            Levels13 = levels13;
            Levels14 = levels14;
            Matches12 = levels12?.ToDictionary(kv => kv.Key, kv => kv.Value == Shared.FieldMatchLevel.Exact);
            Matches13 = levels13?.ToDictionary(kv => kv.Key, kv => kv.Value == Shared.FieldMatchLevel.Exact);
            Matches14 = levels14?.ToDictionary(kv => kv.Key, kv => kv.Value == Shared.FieldMatchLevel.Exact);
            AggregateStateMatchesCodeRao = aggregateStateMatchesCodeRao;
            ConfidencePercent = confidencePercent;
            RawScore = rawScore;
        }

        public Form16MatchProfile Profile { get; }
        internal Operation41PairingDto Candidate { get; }
        public IReadOnlyDictionary<Pairing12To16Field, Shared.FieldMatchLevel>? Levels12 { get; }
        public IReadOnlyDictionary<Pairing13To16Field, Shared.FieldMatchLevel>? Levels13 { get; }
        public IReadOnlyDictionary<Pairing14To16Field, Shared.FieldMatchLevel>? Levels14 { get; }
        public IReadOnlyDictionary<Pairing12To16Field, bool>? Matches12 { get; }
        public IReadOnlyDictionary<Pairing13To16Field, bool>? Matches13 { get; }
        public IReadOnlyDictionary<Pairing14To16Field, bool>? Matches14 { get; }

        /// <summary>Заполняется для профилей 1.3/1.4: AggregateState кандидата vs 1-я цифра CodeRao 1.6.</summary>
        public bool? AggregateStateMatchesCodeRao { get; }
        public int ConfidencePercent { get; }
        public double RawScore { get; }
    }

    /// <summary>
    /// Closest-match результат: лучший кандидат + карта уровней совпадений по полям.
    /// </summary>
    public sealed class ClosestMatchResult<TField>
        where TField : struct, Enum
    {
        internal ClosestMatchResult(
            Operation41PairingDto candidate,
            IReadOnlyDictionary<TField, Shared.FieldMatchLevel> fieldLevels,
            int confidencePercent,
            double rawScore,
            bool? aggregateStateMatchesCodeRao = null)
        {
            Candidate = candidate;
            FieldLevels = fieldLevels;
            FieldMatches = fieldLevels.ToDictionary(kv => kv.Key, kv => kv.Value == Shared.FieldMatchLevel.Exact);
            ConfidencePercent = confidencePercent;
            RawScore = rawScore;
            AggregateStateMatchesCodeRao = aggregateStateMatchesCodeRao;
        }

        internal Operation41PairingDto Candidate { get; }
        public IReadOnlyDictionary<TField, Shared.FieldMatchLevel> FieldLevels { get; }
        public IReadOnlyDictionary<TField, bool> FieldMatches { get; }

        /// <summary>Exact-only карта — для обратной совместимости тестов.</summary>
        public int ConfidencePercent { get; }
        public double RawScore { get; }

        /// <summary>Заполняется для 1.3/1.4↔1.6: AggregateState стороны 1.3/1.4 vs 1-я цифра CodeRao кандидата.</summary>
        public bool? AggregateStateMatchesCodeRao { get; }
    }

    public enum Pairing12To16Field { OperationDate, Mass, BetaGammaActivity, AlphaActivity, ActivityMeasurementDate, DocumentVid, DocumentNumber, DocumentDate, PackName, PackType, PackNumber, CodeRao }
    public enum Pairing13To16Field { OperationDate, MainRadionuclids, TritiumActivity, BetaGammaActivity, AlphaActivity, TransuraniumActivity, ActivityMeasurementDate, DocumentVid, DocumentNumber, DocumentDate, PackName, PackType, PackNumber, CodeRao }
    public enum Pairing14To16Field { OperationDate, Volume, Mass, MainRadionuclids, TritiumActivity, BetaGammaActivity, AlphaActivity, TransuraniumActivity, ActivityMeasurementDate, DocumentVid, DocumentNumber, DocumentDate, PackName, PackType, PackNumber, CodeRao }

    #endregion

    #region PairingNorm

    /// <summary>
    /// Пренормализованные поля строки для closest-match (нормализация один раз на DTO).
    /// </summary>
    private sealed class PairingNorm
    {
        public required string OpCode { get; init; }
        public required string OpDate { get; init; }
        public required string PasNum { get; init; }
        public required string Type { get; init; }
        public required string Radionuclids { get; init; }
        public required string FacNum { get; init; }
        public required string CreationDate { get; init; }
        public required string DocumentVid { get; init; }
        public required string DocumentNumber { get; init; }
        public required string DocumentDate { get; init; }
        public required string ProviderOrRecieverOkpo { get; init; }
        public required string TransporterOkpo { get; init; }
        public required string PackName { get; init; }
        public required string PackType { get; init; }
        public required string PackNumber { get; init; }
        public required string MainRadionuclids { get; init; }
        public required string CodeRao { get; init; }
        public required string ActivityMeasurementDate { get; init; }
        public required int Quantity { get; init; }
        public required NumericNorm Activity { get; init; }
        public required NumericNorm Mass { get; init; }
        public required NumericNorm Volume { get; init; }
        public required NumericNorm TritiumActivity { get; init; }
        public required NumericNorm BetaGammaActivity { get; init; }
        public required NumericNorm AlphaActivity { get; init; }
        public required NumericNorm TransuraniumActivity { get; init; }
    }

    private readonly struct NumericNorm
    {
        public NumericNorm(string? raw)
        {
            Raw = raw ?? string.Empty;
            Parsed = SoftSimilarityCore.TryParseNumeric(raw, out var value);
            Value = value;
        }

        public string Raw { get; }
        public bool Parsed { get; }
        public double Value { get; }
    }

    private static PairingNorm[] CreatePairingNorms(List<Operation41PairingDto> rows)
    {
        var norms = new PairingNorm[rows.Count];
        for (var i = 0; i < rows.Count; i++)
        {
            norms[i] = CreatePairingNorm(rows[i]);
        }

        return norms;
    }

    private static PairingNorm CreatePairingNorm(Operation41PairingDto row) => new()
    {
        OpCode = NormalizeNumber(row.OpCode),
        OpDate = NormalizeDate(row.OpDate),
        PasNum = NormalizeSerialNumber(row.PasNum),
        Type = NormalizeNumber(row.Type),
        Radionuclids = NormalizeRads(row.Radionuclids),
        FacNum = NormalizeSerialNumber(row.FacNum),
        CreationDate = NormalizeDate(row.CreationDate),
        DocumentVid = NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(row.DocumentVid)),
        DocumentNumber = NormalizeNumber(row.DocumentNumber),
        DocumentDate = NormalizeDate(row.DocumentDate),
        ProviderOrRecieverOkpo = NormalizeNumber(row.ProviderOrRecieverOkpo),
        TransporterOkpo = NormalizeNumber(row.TransporterOkpo),
        PackName = NormalizeNumber(row.PackName),
        PackType = NormalizeNumber(row.PackType),
        PackNumber = NormalizeNumber(row.PackNumber),
        MainRadionuclids = NormalizeRads(row.MainRadionuclids),
        CodeRao = NormalizeNumber(row.CodeRao),
        ActivityMeasurementDate = NormalizeDate(row.ActivityMeasurementDate),
        Quantity = GetQuantityForComparison(row, true),
        Activity = new NumericNorm(row.Activity),
        Mass = new NumericNorm(row.Mass),
        Volume = new NumericNorm(row.Volume),
        TritiumActivity = new NumericNorm(row.TritiumActivity),
        BetaGammaActivity = new NumericNorm(row.BetaGammaActivity),
        AlphaActivity = new NumericNorm(row.AlphaActivity),
        TransuraniumActivity = new NumericNorm(row.TransuraniumActivity)
    };

    /// <summary>
    /// Эквивалент ActivityMatches / NumericWithTolerance на препарсенных значениях.
    /// </summary>
    private static bool NumericTolerance(NumericNorm left, NumericNorm right) =>
        SoftSimilarityCore.NumericMatchesWithTolerance(left.Raw, right.Raw, 0.10, NumberComparer.Equals);

    #endregion
}
