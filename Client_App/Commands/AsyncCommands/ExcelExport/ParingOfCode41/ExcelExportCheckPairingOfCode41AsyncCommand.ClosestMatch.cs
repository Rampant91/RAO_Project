using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Resources.CustomComparers.SnkComparers;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    private Dictionary<int, ClosestMatchHighlight> _form11ClosestMatchHighlights = new();
    private Dictionary<int, ClosestMatchHighlight> _form15ClosestMatchHighlights = new();

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
}
