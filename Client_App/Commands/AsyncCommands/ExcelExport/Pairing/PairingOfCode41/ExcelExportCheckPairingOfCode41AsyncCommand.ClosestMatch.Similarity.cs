using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Resources;
using Client_App.Resources.CustomComparers.SnkComparers;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared.SoftSimilarityCore;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    private const int ClosestOperationDateToleranceDays = 15;
    private const double BothIdentifiersExactBonus = 10.0;

    #region Soft similarity (closest only)

    private static Shared.FieldSimilarity FieldSimilarity11To15(
        Operation41PairingDto source,
        Operation41PairingDto candidate,
        PairingNorm sourceNorm,
        PairingNorm candidateNorm,
        Pairing11To15Field field) =>
        field switch
        {
            Pairing11To15Field.OperationCode => SimilarityOperationCode41(sourceNorm.OpCode, candidateNorm.OpCode),
            Pairing11To15Field.OperationDate => SimilarityOperationDate(
                source.OpDate, candidate.OpDate, ClosestOperationDateToleranceDays, NormalizeDate),
            Pairing11To15Field.PassportNumber => SimilarityPassportOrFactory(source.PasNum, candidate.PasNum, isFactory: false),
            Pairing11To15Field.Type => SimilarityType(source.Type, candidate.Type),
            Pairing11To15Field.Radionuclids => SimilarityRadionuclids(sourceNorm.Radionuclids, candidateNorm.Radionuclids),
            Pairing11To15Field.FactoryNumber => SimilarityPassportOrFactory(
                source.FacNum,
                candidate.FacNum,
                isFactory: true,
                sourceNorm.Quantity,
                candidateNorm.Quantity),
            Pairing11To15Field.Activity => SimilarityActivityConsideringFactoryEnumeration(
                sourceNorm.Activity.Raw,
                candidateNorm.Activity.Raw,
                source.FacNum,
                candidate.FacNum,
                sourceNorm.Quantity,
                candidateNorm.Quantity),
            Pairing11To15Field.Quantity => SimilarityQuantityConsideringFactoryRange(
                sourceNorm.Quantity,
                candidateNorm.Quantity,
                source.FacNum,
                candidate.FacNum),
            Pairing11To15Field.CreationDate => SimilarityCalendarDate(source.CreationDate, candidate.CreationDate),
            Pairing11To15Field.DocumentVid => SimilarityTextNormalized(sourceNorm.DocumentVid, candidateNorm.DocumentVid),
            Pairing11To15Field.DocumentNumber => SimilarityTextNormalized(sourceNorm.DocumentNumber, candidateNorm.DocumentNumber),
            Pairing11To15Field.DocumentDate => SimilarityCalendarDate(source.DocumentDate, candidate.DocumentDate),
            Pairing11To15Field.ProviderOrRecieverOkpo => SimilarityTextNormalized(
                sourceNorm.ProviderOrRecieverOkpo, candidateNorm.ProviderOrRecieverOkpo),
            Pairing11To15Field.TransporterOkpo => SimilarityTextNormalized(
                sourceNorm.TransporterOkpo, candidateNorm.TransporterOkpo),
            Pairing11To15Field.PackName => SimilarityTextNormalized(sourceNorm.PackName, candidateNorm.PackName),
            Pairing11To15Field.PackType => SimilarityType(source.PackType, candidate.PackType),
            Pairing11To15Field.PackNumber => SimilarityPackNumber(source.PackNumber, candidate.PackNumber),
            _ => Shared.FieldSimilarity.Mismatch(0)
        };

    private static Shared.FieldSimilarity FieldSimilarity12To16(
        Operation41PairingDto source,
        Operation41PairingDto candidate,
        PairingNorm sourceNorm,
        PairingNorm candidateNorm,
        Pairing12To16Field field) =>
        field switch
        {
            Pairing12To16Field.OperationDate => SimilarityOperationDate(
                source.OpDate, candidate.OpDate, ClosestOperationDateToleranceDays, NormalizeDate),
            Pairing12To16Field.Mass => SimilarityNumericWithTolerance(sourceNorm.Mass.Raw, candidateNorm.Mass.Raw),
            Pairing12To16Field.BetaGammaActivity => SimilarityNumericWithTolerance(
                sourceNorm.BetaGammaActivity.Raw, candidateNorm.BetaGammaActivity.Raw),
            Pairing12To16Field.AlphaActivity => SimilarityNumericWithTolerance(
                sourceNorm.AlphaActivity.Raw, candidateNorm.AlphaActivity.Raw),
            Pairing12To16Field.ActivityMeasurementDate => SimilarityCalendarDate(
                source.ActivityMeasurementDate, candidate.ActivityMeasurementDate),
            Pairing12To16Field.DocumentVid => SimilarityTextNormalized(sourceNorm.DocumentVid, candidateNorm.DocumentVid),
            Pairing12To16Field.DocumentNumber => SimilarityTextNormalized(sourceNorm.DocumentNumber, candidateNorm.DocumentNumber),
            Pairing12To16Field.DocumentDate => SimilarityCalendarDate(source.DocumentDate, candidate.DocumentDate),
            Pairing12To16Field.PackName => SimilarityTextNormalized(sourceNorm.PackName, candidateNorm.PackName),
            Pairing12To16Field.PackType => SimilarityType(source.PackType, candidate.PackType),
            Pairing12To16Field.PackNumber => SimilarityPackNumber(source.PackNumber, candidate.PackNumber),
            Pairing12To16Field.CodeRao => SimilarityCalculatedCodeRao(source, candidate),
            _ => Shared.FieldSimilarity.Mismatch(0)
        };

    private static Shared.FieldSimilarity FieldSimilarity13To16(
        Operation41PairingDto source,
        Operation41PairingDto candidate,
        PairingNorm sourceNorm,
        PairingNorm candidateNorm,
        Pairing13To16Field field) =>
        field switch
        {
            Pairing13To16Field.OperationDate => SimilarityOperationDate(
                source.OpDate, candidate.OpDate, ClosestOperationDateToleranceDays, NormalizeDate),
            Pairing13To16Field.MainRadionuclids => SimilarityRadionuclids(
                sourceNorm.MainRadionuclids, candidateNorm.MainRadionuclids),
            Pairing13To16Field.TritiumActivity => SimilarityNumericWithTolerance(
                sourceNorm.TritiumActivity.Raw, candidateNorm.TritiumActivity.Raw),
            Pairing13To16Field.BetaGammaActivity => SimilarityNumericWithTolerance(
                sourceNorm.BetaGammaActivity.Raw, candidateNorm.BetaGammaActivity.Raw),
            Pairing13To16Field.AlphaActivity => SimilarityNumericWithTolerance(
                sourceNorm.AlphaActivity.Raw, candidateNorm.AlphaActivity.Raw),
            Pairing13To16Field.TransuraniumActivity => SimilarityNumericWithTolerance(
                sourceNorm.TransuraniumActivity.Raw, candidateNorm.TransuraniumActivity.Raw),
            Pairing13To16Field.ActivityMeasurementDate => SimilarityCalendarDate(
                source.ActivityMeasurementDate, candidate.ActivityMeasurementDate),
            Pairing13To16Field.DocumentVid => SimilarityTextNormalized(sourceNorm.DocumentVid, candidateNorm.DocumentVid),
            Pairing13To16Field.DocumentNumber => SimilarityTextNormalized(sourceNorm.DocumentNumber, candidateNorm.DocumentNumber),
            Pairing13To16Field.DocumentDate => SimilarityCalendarDate(source.DocumentDate, candidate.DocumentDate),
            Pairing13To16Field.PackName => SimilarityTextNormalized(sourceNorm.PackName, candidateNorm.PackName),
            Pairing13To16Field.PackType => SimilarityType(source.PackType, candidate.PackType),
            Pairing13To16Field.PackNumber => SimilarityPackNumber(source.PackNumber, candidate.PackNumber),
            Pairing13To16Field.CodeRao => SimilarityCalculatedCodeRao(source, candidate),
            _ => Shared.FieldSimilarity.Mismatch(0)
        };

    private static Shared.FieldSimilarity FieldSimilarity14To16(
        Operation41PairingDto source,
        Operation41PairingDto candidate,
        PairingNorm sourceNorm,
        PairingNorm candidateNorm,
        Pairing14To16Field field) =>
        field switch
        {
            Pairing14To16Field.OperationDate => SimilarityOperationDate(
                source.OpDate, candidate.OpDate, ClosestOperationDateToleranceDays, NormalizeDate),
            Pairing14To16Field.Volume => SimilarityNumericWithTolerance(sourceNorm.Volume.Raw, candidateNorm.Volume.Raw),
            Pairing14To16Field.Mass => SimilarityNumericWithTolerance(sourceNorm.Mass.Raw, candidateNorm.Mass.Raw),
            Pairing14To16Field.MainRadionuclids => SimilarityRadionuclids(
                sourceNorm.MainRadionuclids, candidateNorm.MainRadionuclids),
            Pairing14To16Field.TritiumActivity => SimilarityNumericWithTolerance(
                sourceNorm.TritiumActivity.Raw, candidateNorm.TritiumActivity.Raw),
            Pairing14To16Field.BetaGammaActivity => SimilarityNumericWithTolerance(
                sourceNorm.BetaGammaActivity.Raw, candidateNorm.BetaGammaActivity.Raw),
            Pairing14To16Field.AlphaActivity => SimilarityNumericWithTolerance(
                sourceNorm.AlphaActivity.Raw, candidateNorm.AlphaActivity.Raw),
            Pairing14To16Field.TransuraniumActivity => SimilarityNumericWithTolerance(
                sourceNorm.TransuraniumActivity.Raw, candidateNorm.TransuraniumActivity.Raw),
            Pairing14To16Field.ActivityMeasurementDate => SimilarityCalendarDate(
                source.ActivityMeasurementDate, candidate.ActivityMeasurementDate),
            Pairing14To16Field.DocumentVid => SimilarityTextNormalized(sourceNorm.DocumentVid, candidateNorm.DocumentVid),
            Pairing14To16Field.DocumentNumber => SimilarityTextNormalized(sourceNorm.DocumentNumber, candidateNorm.DocumentNumber),
            Pairing14To16Field.DocumentDate => SimilarityCalendarDate(source.DocumentDate, candidate.DocumentDate),
            Pairing14To16Field.PackName => SimilarityTextNormalized(sourceNorm.PackName, candidateNorm.PackName),
            Pairing14To16Field.PackType => SimilarityType(source.PackType, candidate.PackType),
            Pairing14To16Field.PackNumber => SimilarityPackNumber(source.PackNumber, candidate.PackNumber),
            Pairing14To16Field.CodeRao => SimilarityCalculatedCodeRao(source, candidate),
            _ => Shared.FieldSimilarity.Mismatch(0)
        };

    private static Shared.FieldSimilarity SimilarityCalculatedCodeRao(
        Operation41PairingDto rvSide,
        Operation41PairingDto raoSide) =>
        RaoCodeHelper.CodeRaoPairingMatches(
            rvSide.FormNum, rvSide.CodeRao, rvSide.Radionuclids, rvSide.MainRadionuclids, rvSide.AggregateState, raoSide.CodeRao)
            ? Shared.FieldSimilarity.Exact
            : Shared.FieldSimilarity.Mismatch(0);

    private static Shared.FieldSimilarity SimilarityOperationCode41(string left, string right)
    {
        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            return Shared.FieldSimilarity.Exact;
        }

        var leftIs41 = left is "41" or Form15ReceiveMistypeOpCode;
        var rightIs41 = right is "41" or Form15ReceiveMistypeOpCode;
        if (leftIs41 && rightIs41)
        {
            return Shared.FieldSimilarity.Near(0.72);
        }

        return Shared.FieldSimilarity.Mismatch(0.15);
    }

    private static double GetFieldWeight11To15(Pairing11To15Field field, Operation41PairingDto source)
    {
        var serialsEmpty = SerialNumbersAreEmpty(source);
        return field switch
        {
            Pairing11To15Field.PassportNumber => serialsEmpty ? 2.0 : 12.0,
            Pairing11To15Field.FactoryNumber => serialsEmpty ? 2.0 : 12.0,
            Pairing11To15Field.OperationCode => 1.5,
            Pairing11To15Field.OperationDate => 2.0,
            Pairing11To15Field.Type => 3.0,
            Pairing11To15Field.Radionuclids => 4.5,
            Pairing11To15Field.Activity => 2.5,
            Pairing11To15Field.Quantity => 2.0,
            Pairing11To15Field.CreationDate => 3.0,
            Pairing11To15Field.DocumentVid => 2.5,
            Pairing11To15Field.DocumentNumber => 3.0,
            Pairing11To15Field.DocumentDate => 2.5,
            Pairing11To15Field.ProviderOrRecieverOkpo => 3.5,
            Pairing11To15Field.TransporterOkpo => 3.0,
            Pairing11To15Field.PackName => 3.0,
            Pairing11To15Field.PackType => 6.5,
            Pairing11To15Field.PackNumber => 3.5,
            _ => 1.0
        };
    }

    private static double GetFieldWeight12To16(Pairing12To16Field field) =>
        field switch
        {
            Pairing12To16Field.OperationDate => 2.0,
            Pairing12To16Field.Mass => 3.0,
            Pairing12To16Field.BetaGammaActivity => 2.5,
            Pairing12To16Field.AlphaActivity => 2.5,
            Pairing12To16Field.ActivityMeasurementDate => 2.5,
            Pairing12To16Field.DocumentVid => 2.5,
            Pairing12To16Field.DocumentNumber => 3.0,
            Pairing12To16Field.DocumentDate => 2.5,
            Pairing12To16Field.PackName => 3.0,
            Pairing12To16Field.PackType => 6.5,
            Pairing12To16Field.PackNumber => 3.5,
            Pairing12To16Field.CodeRao => 4.0,
            _ => 1.0
        };

    private static double GetFieldWeight13To16(Pairing13To16Field field) =>
        field switch
        {
            Pairing13To16Field.OperationDate => 2.0,
            Pairing13To16Field.MainRadionuclids => 4.5,
            Pairing13To16Field.TritiumActivity => 2.5,
            Pairing13To16Field.BetaGammaActivity => 2.5,
            Pairing13To16Field.AlphaActivity => 2.5,
            Pairing13To16Field.TransuraniumActivity => 2.5,
            Pairing13To16Field.ActivityMeasurementDate => 2.5,
            Pairing13To16Field.DocumentVid => 2.5,
            Pairing13To16Field.DocumentNumber => 3.0,
            Pairing13To16Field.DocumentDate => 2.5,
            Pairing13To16Field.PackName => 3.0,
            Pairing13To16Field.PackType => 6.5,
            Pairing13To16Field.PackNumber => 3.5,
            Pairing13To16Field.CodeRao => 4.0,
            _ => 1.0
        };

    private static double GetFieldWeight14To16(Pairing14To16Field field) =>
        field switch
        {
            Pairing14To16Field.OperationDate => 2.0,
            Pairing14To16Field.Volume => 3.0,
            Pairing14To16Field.Mass => 3.0,
            Pairing14To16Field.MainRadionuclids => 4.5,
            Pairing14To16Field.TritiumActivity => 2.5,
            Pairing14To16Field.BetaGammaActivity => 2.5,
            Pairing14To16Field.AlphaActivity => 2.5,
            Pairing14To16Field.TransuraniumActivity => 2.5,
            Pairing14To16Field.ActivityMeasurementDate => 2.5,
            Pairing14To16Field.DocumentVid => 2.5,
            Pairing14To16Field.DocumentNumber => 3.0,
            Pairing14To16Field.DocumentDate => 2.5,
            Pairing14To16Field.PackName => 3.0,
            Pairing14To16Field.PackType => 6.5,
            Pairing14To16Field.PackNumber => 3.5,
            Pairing14To16Field.CodeRao => 4.0,
            _ => 1.0
        };

    private static int IndexOfField<TField>(List<TField> fields, TField field)
        where TField : struct, Enum
    {
        for (var i = 0; i < fields.Count; i++)
        {
            if (EqualityComparer<TField>.Default.Equals(fields[i], field))
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

    private static (int confidence, double rawScore, double maxWeight) ComputeConfidence(
        double weightedScore,
        double maxWeight) =>
        (WeightedClosestMatchEngine.ToConfidencePercent(weightedScore, maxWeight), weightedScore, maxWeight);

    #endregion
}
