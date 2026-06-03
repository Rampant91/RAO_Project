using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Resources.CustomComparers.SnkComparers;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    private Dictionary<int, ClosestMatchHighlight> _form11ClosestMatchHighlights = new();
    private Dictionary<int, ClosestMatchHighlight> _form15ClosestMatchHighlights = new();
    private Dictionary<int, Dictionary<Pairing12To16Field, bool>> _form12ClosestMatchHighlights = new();
    private Dictionary<int, Dictionary<Pairing13To16Field, bool>> _form13ClosestMatchHighlights = new();
    private Dictionary<int, Dictionary<Pairing14To16Field, bool>> _form14ClosestMatchHighlights = new();

    private static Dictionary<int, ClosestMatchHighlight> BuildClosestMatchHighlights(
        List<Operation41PairingDto> unpaired,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options)
    {
        if (unpaired.Count == 0 || reference.Count == 0 || !HasAnyComparisonField(options))
        {
            return new Dictionary<int, ClosestMatchHighlight>();
        }

        var result = new Dictionary<int, ClosestMatchHighlight>(unpaired.Count);
        foreach (var source in unpaired)
        {
            var best = FindClosestMatch(source, reference, options);
            if (best is null)
            {
                continue;
            }

            result[source.Id] = new ClosestMatchHighlight(BuildFieldMatches(source, best, options));
        }

        return result;
    }

    private static Operation41PairingDto? FindClosestMatch(
        Operation41PairingDto source,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options)
    {
        Operation41PairingDto? best = null;
        var bestCount = -1;

        foreach (var candidate in reference)
        {
            var count = CountMatchingParams(source, candidate, options);
            if (count > bestCount)
            {
                bestCount = count;
                best = candidate;
            }
        }

        return best;
    }

    private static int CountMatchingParams(
        Operation41PairingDto source,
        Operation41PairingDto candidate,
        Pairing11To15Params options)
    {
        var count = 0;
        foreach (var field in GetEnabledFields(options))
        {
            if (FieldMatches(source, candidate, field, options))
            {
                count++;
            }
        }

        return count;
    }

    private static Dictionary<Pairing11To15Field, bool> BuildFieldMatches(
        Operation41PairingDto source,
        Operation41PairingDto candidate,
        Pairing11To15Params options)
    {
        var matches = new Dictionary<Pairing11To15Field, bool>();
        foreach (var field in GetEnabledFields(options))
        {
            matches[field] = FieldMatches(source, candidate, field, options);
        }

        return matches;
    }

    private static bool FieldMatches(
        Operation41PairingDto left,
        Operation41PairingDto right,
        Pairing11To15Field field,
        Pairing11To15Params options) =>
        field switch
        {
            Pairing11To15Field.OperationDate =>
                NormalizeDate(left.OpDate) == NormalizeDate(right.OpDate),
            Pairing11To15Field.PassportNumber =>
                NormalizeNumber(left.PasNum) == NormalizeNumber(right.PasNum),
            Pairing11To15Field.Type =>
                NormalizeNumber(left.Type) == NormalizeNumber(right.Type),
            Pairing11To15Field.Radionuclids =>
                NormalizeRads(left.Radionuclids) == NormalizeRads(right.Radionuclids),
            Pairing11To15Field.FactoryNumber =>
                NormalizeNumber(left.FacNum) == NormalizeNumber(right.FacNum),
            Pairing11To15Field.Activity =>
                ActivityMatches(left, right, options.CheckActivity),
            Pairing11To15Field.Quantity =>
                GetQuantityForComparison(left, true) == GetQuantityForComparison(right, true),
            Pairing11To15Field.CreationDate =>
                NormalizeDate(left.CreationDate) == NormalizeDate(right.CreationDate),
            Pairing11To15Field.DocumentVid =>
                NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(left.DocumentVid))
                == NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(right.DocumentVid)),
            Pairing11To15Field.DocumentNumber =>
                NormalizeNumber(left.DocumentNumber) == NormalizeNumber(right.DocumentNumber),
            Pairing11To15Field.DocumentDate =>
                NormalizeDate(left.DocumentDate) == NormalizeDate(right.DocumentDate),
            Pairing11To15Field.ProviderOrRecieverOkpo =>
                NormalizeNumber(left.ProviderOrRecieverOkpo) == NormalizeNumber(right.ProviderOrRecieverOkpo),
            Pairing11To15Field.TransporterOkpo =>
                NormalizeNumber(left.TransporterOkpo) == NormalizeNumber(right.TransporterOkpo),
            Pairing11To15Field.PackName =>
                NormalizeNumber(left.PackName) == NormalizeNumber(right.PackName),
            Pairing11To15Field.PackType =>
                NormalizeNumber(left.PackType) == NormalizeNumber(right.PackType),
            Pairing11To15Field.PackNumber =>
                NormalizeNumber(left.PackNumber) == NormalizeNumber(right.PackNumber),
            _ => false
        };

    private static IEnumerable<Pairing11To15Field> GetEnabledFields(Pairing11To15Params options)
    {
        if (options.CheckOperationDate) yield return Pairing11To15Field.OperationDate;
        if (options.CheckPassportNumber) yield return Pairing11To15Field.PassportNumber;
        if (options.CheckType) yield return Pairing11To15Field.Type;
        if (options.CheckRadionuclids) yield return Pairing11To15Field.Radionuclids;
        if (options.CheckFactoryNumber) yield return Pairing11To15Field.FactoryNumber;
        if (options.CheckActivity) yield return Pairing11To15Field.Activity;
        if (options.CheckQuantity) yield return Pairing11To15Field.Quantity;
        if (options.CheckCreationDate) yield return Pairing11To15Field.CreationDate;
        if (options.CheckDocumentVid) yield return Pairing11To15Field.DocumentVid;
        if (options.CheckDocumentNumber) yield return Pairing11To15Field.DocumentNumber;
        if (options.CheckDocumentDate) yield return Pairing11To15Field.DocumentDate;
        if (options.CheckProviderOrRecieverOkpo) yield return Pairing11To15Field.ProviderOrRecieverOkpo;
        if (options.CheckTransporterOkpo) yield return Pairing11To15Field.TransporterOkpo;
        if (options.CheckPackName) yield return Pairing11To15Field.PackName;
        if (options.CheckPackType) yield return Pairing11To15Field.PackType;
        if (options.CheckPackNumber) yield return Pairing11To15Field.PackNumber;
    }

    private static bool HasAnyComparisonField(Pairing11To15Params options) =>
        GetEnabledFields(options).Any();

    private sealed class ClosestMatchHighlight(IReadOnlyDictionary<Pairing11To15Field, bool> fieldMatches)
    {
        public IReadOnlyDictionary<Pairing11To15Field, bool> FieldMatches { get; } = fieldMatches;
    }

    private enum Pairing11To15Field
    {
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

    private static Dictionary<int, Dictionary<Pairing12To16Field, bool>> BuildClosestMatchHighlights12To16(
        List<Operation41PairingDto> unpaired, List<Operation41PairingDto> reference, Pairing12To16Params options) =>
        BuildClosest(unpaired, reference, GetEnabledFields12To16(options), FieldMatches12To16);

    private static Dictionary<int, Dictionary<Pairing13To16Field, bool>> BuildClosestMatchHighlights13To16(
        List<Operation41PairingDto> unpaired, List<Operation41PairingDto> reference, Pairing13To16Params options) =>
        BuildClosest(unpaired, reference, GetEnabledFields13To16(options), FieldMatches13To16);

    private static Dictionary<int, Dictionary<Pairing14To16Field, bool>> BuildClosestMatchHighlights14To16(
        List<Operation41PairingDto> unpaired, List<Operation41PairingDto> reference, Pairing14To16Params options) =>
        BuildClosest(unpaired, reference, GetEnabledFields14To16(options), FieldMatches14To16);

    private static Dictionary<int, Dictionary<TField, bool>> BuildClosest<TField>(
        List<Operation41PairingDto> unpaired,
        List<Operation41PairingDto> reference,
        List<TField> fields,
        Func<Operation41PairingDto, Operation41PairingDto, TField, bool> isMatch)
        where TField : struct, Enum
    {
        var result = new Dictionary<int, Dictionary<TField, bool>>();
        if (unpaired.Count == 0 || reference.Count == 0 || fields.Count == 0) return result;

        foreach (var row in unpaired)
        {
            Operation41PairingDto? best = null;
            var bestScore = -1;
            foreach (var candidate in reference)
            {
                var score = 0;
                foreach (var f in fields) if (isMatch(row, candidate, f)) score++;
                if (score > bestScore) { bestScore = score; best = candidate; }
            }
            if (best is null) continue;
            var map = new Dictionary<TField, bool>(fields.Count);
            foreach (var f in fields) map[f] = isMatch(row, best, f);
            result[row.Id] = map;
        }

        return result;
    }

    private static bool FieldMatches12To16(Operation41PairingDto left, Operation41PairingDto right, Pairing12To16Field field) => field switch
    {
        Pairing12To16Field.OperationDate => NormalizeDate(left.OpDate) == NormalizeDate(right.OpDate),
        Pairing12To16Field.Mass => NumericWithTolerance(left.Mass, right.Mass),
        Pairing12To16Field.BetaGammaActivity => NumericWithTolerance(left.BetaGammaActivity, right.BetaGammaActivity),
        Pairing12To16Field.AlphaActivity => NumericWithTolerance(left.AlphaActivity, right.AlphaActivity),
        Pairing12To16Field.ActivityMeasurementDate => NormalizeDate(left.ActivityMeasurementDate) == NormalizeDate(right.ActivityMeasurementDate),
        Pairing12To16Field.DocumentVid => NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(left.DocumentVid)) == NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(right.DocumentVid)),
        Pairing12To16Field.DocumentNumber => NormalizeNumber(left.DocumentNumber) == NormalizeNumber(right.DocumentNumber),
        Pairing12To16Field.DocumentDate => NormalizeDate(left.DocumentDate) == NormalizeDate(right.DocumentDate),
        Pairing12To16Field.PackName => NormalizeNumber(left.PackName) == NormalizeNumber(right.PackName),
        Pairing12To16Field.PackType => NormalizeNumber(left.PackType) == NormalizeNumber(right.PackType),
        Pairing12To16Field.PackNumber => NormalizeNumber(left.PackNumber) == NormalizeNumber(right.PackNumber),
        _ => false
    };

    private static bool FieldMatches13To16(Operation41PairingDto left, Operation41PairingDto right, Pairing13To16Field field) => field switch
    {
        Pairing13To16Field.OperationDate => NormalizeDate(left.OpDate) == NormalizeDate(right.OpDate),
        Pairing13To16Field.MainRadionuclids => NormalizeRads(left.MainRadionuclids) == NormalizeRads(right.MainRadionuclids),
        Pairing13To16Field.TritiumActivity => NumericWithTolerance(left.TritiumActivity, right.TritiumActivity),
        Pairing13To16Field.BetaGammaActivity => NumericWithTolerance(left.BetaGammaActivity, right.BetaGammaActivity),
        Pairing13To16Field.AlphaActivity => NumericWithTolerance(left.AlphaActivity, right.AlphaActivity),
        Pairing13To16Field.TransuraniumActivity => NumericWithTolerance(left.TransuraniumActivity, right.TransuraniumActivity),
        Pairing13To16Field.ActivityMeasurementDate => NormalizeDate(left.ActivityMeasurementDate) == NormalizeDate(right.ActivityMeasurementDate),
        Pairing13To16Field.DocumentVid => NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(left.DocumentVid)) == NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(right.DocumentVid)),
        Pairing13To16Field.DocumentNumber => NormalizeNumber(left.DocumentNumber) == NormalizeNumber(right.DocumentNumber),
        Pairing13To16Field.DocumentDate => NormalizeDate(left.DocumentDate) == NormalizeDate(right.DocumentDate),
        Pairing13To16Field.PackName => NormalizeNumber(left.PackName) == NormalizeNumber(right.PackName),
        Pairing13To16Field.PackType => NormalizeNumber(left.PackType) == NormalizeNumber(right.PackType),
        Pairing13To16Field.PackNumber => NormalizeNumber(left.PackNumber) == NormalizeNumber(right.PackNumber),
        _ => false
    };

    private static bool FieldMatches14To16(Operation41PairingDto left, Operation41PairingDto right, Pairing14To16Field field) => field switch
    {
        Pairing14To16Field.OperationDate => NormalizeDate(left.OpDate) == NormalizeDate(right.OpDate),
        Pairing14To16Field.Volume => NumericWithTolerance(left.Volume, right.Volume),
        Pairing14To16Field.Mass => NumericWithTolerance(left.Mass, right.Mass),
        Pairing14To16Field.MainRadionuclids => NormalizeRads(left.MainRadionuclids) == NormalizeRads(right.MainRadionuclids),
        Pairing14To16Field.TritiumActivity => NumericWithTolerance(left.TritiumActivity, right.TritiumActivity),
        Pairing14To16Field.BetaGammaActivity => NumericWithTolerance(left.BetaGammaActivity, right.BetaGammaActivity),
        Pairing14To16Field.AlphaActivity => NumericWithTolerance(left.AlphaActivity, right.AlphaActivity),
        Pairing14To16Field.TransuraniumActivity => NumericWithTolerance(left.TransuraniumActivity, right.TransuraniumActivity),
        Pairing14To16Field.ActivityMeasurementDate => NormalizeDate(left.ActivityMeasurementDate) == NormalizeDate(right.ActivityMeasurementDate),
        Pairing14To16Field.DocumentVid => NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(left.DocumentVid)) == NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(right.DocumentVid)),
        Pairing14To16Field.DocumentNumber => NormalizeNumber(left.DocumentNumber) == NormalizeNumber(right.DocumentNumber),
        Pairing14To16Field.DocumentDate => NormalizeDate(left.DocumentDate) == NormalizeDate(right.DocumentDate),
        Pairing14To16Field.PackName => NormalizeNumber(left.PackName) == NormalizeNumber(right.PackName),
        Pairing14To16Field.PackType => NormalizeNumber(left.PackType) == NormalizeNumber(right.PackType),
        Pairing14To16Field.PackNumber => NormalizeNumber(left.PackNumber) == NormalizeNumber(right.PackNumber),
        _ => false
    };

    private static List<Pairing12To16Field> GetEnabledFields12To16(Pairing12To16Params o){var l=new List<Pairing12To16Field>(); if(o.CheckOperationDate)l.Add(Pairing12To16Field.OperationDate); if(o.CheckMass)l.Add(Pairing12To16Field.Mass); if(o.CheckBetaGammaActivity)l.Add(Pairing12To16Field.BetaGammaActivity); if(o.CheckAlphaActivity)l.Add(Pairing12To16Field.AlphaActivity); if(o.CheckActivityMeasurementDate)l.Add(Pairing12To16Field.ActivityMeasurementDate); if(o.CheckDocumentVid)l.Add(Pairing12To16Field.DocumentVid); if(o.CheckDocumentNumber)l.Add(Pairing12To16Field.DocumentNumber); if(o.CheckDocumentDate)l.Add(Pairing12To16Field.DocumentDate); if(o.CheckPackName)l.Add(Pairing12To16Field.PackName); if(o.CheckPackType)l.Add(Pairing12To16Field.PackType); if(o.CheckPackNumber)l.Add(Pairing12To16Field.PackNumber); return l;}
    private static List<Pairing13To16Field> GetEnabledFields13To16(Pairing13To16Params o){var l=new List<Pairing13To16Field>(); if(o.CheckOperationDate)l.Add(Pairing13To16Field.OperationDate); if(o.CheckMainRadionuclids)l.Add(Pairing13To16Field.MainRadionuclids); if(o.CheckTritiumActivity)l.Add(Pairing13To16Field.TritiumActivity); if(o.CheckBetaGammaActivity)l.Add(Pairing13To16Field.BetaGammaActivity); if(o.CheckAlphaActivity)l.Add(Pairing13To16Field.AlphaActivity); if(o.CheckTransuraniumActivity)l.Add(Pairing13To16Field.TransuraniumActivity); if(o.CheckActivityMeasurementDate)l.Add(Pairing13To16Field.ActivityMeasurementDate); if(o.CheckDocumentVid)l.Add(Pairing13To16Field.DocumentVid); if(o.CheckDocumentNumber)l.Add(Pairing13To16Field.DocumentNumber); if(o.CheckDocumentDate)l.Add(Pairing13To16Field.DocumentDate); if(o.CheckPackName)l.Add(Pairing13To16Field.PackName); if(o.CheckPackType)l.Add(Pairing13To16Field.PackType); if(o.CheckPackNumber)l.Add(Pairing13To16Field.PackNumber); return l;}
    private static List<Pairing14To16Field> GetEnabledFields14To16(Pairing14To16Params o){var l=new List<Pairing14To16Field>(); if(o.CheckOperationDate)l.Add(Pairing14To16Field.OperationDate); if(o.CheckVolume)l.Add(Pairing14To16Field.Volume); if(o.CheckMass)l.Add(Pairing14To16Field.Mass); if(o.CheckMainRadionuclids)l.Add(Pairing14To16Field.MainRadionuclids); if(o.CheckTritiumActivity)l.Add(Pairing14To16Field.TritiumActivity); if(o.CheckBetaGammaActivity)l.Add(Pairing14To16Field.BetaGammaActivity); if(o.CheckAlphaActivity)l.Add(Pairing14To16Field.AlphaActivity); if(o.CheckTransuraniumActivity)l.Add(Pairing14To16Field.TransuraniumActivity); if(o.CheckActivityMeasurementDate)l.Add(Pairing14To16Field.ActivityMeasurementDate); if(o.CheckDocumentVid)l.Add(Pairing14To16Field.DocumentVid); if(o.CheckDocumentNumber)l.Add(Pairing14To16Field.DocumentNumber); if(o.CheckDocumentDate)l.Add(Pairing14To16Field.DocumentDate); if(o.CheckPackName)l.Add(Pairing14To16Field.PackName); if(o.CheckPackType)l.Add(Pairing14To16Field.PackType); if(o.CheckPackNumber)l.Add(Pairing14To16Field.PackNumber); return l;}

    private enum Pairing12To16Field { OperationDate, Mass, BetaGammaActivity, AlphaActivity, ActivityMeasurementDate, DocumentVid, DocumentNumber, DocumentDate, PackName, PackType, PackNumber }
    private enum Pairing13To16Field { OperationDate, MainRadionuclids, TritiumActivity, BetaGammaActivity, AlphaActivity, TransuraniumActivity, ActivityMeasurementDate, DocumentVid, DocumentNumber, DocumentDate, PackName, PackType, PackNumber }
    private enum Pairing14To16Field { OperationDate, Volume, Mass, MainRadionuclids, TritiumActivity, BetaGammaActivity, AlphaActivity, TransuraniumActivity, ActivityMeasurementDate, DocumentVid, DocumentNumber, DocumentDate, PackName, PackType, PackNumber }
}
