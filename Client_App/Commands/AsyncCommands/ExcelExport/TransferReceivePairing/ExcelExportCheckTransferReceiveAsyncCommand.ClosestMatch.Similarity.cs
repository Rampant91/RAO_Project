using System;
using Client_App.Commands.AsyncCommands.ExcelExport.Shared;
using static Client_App.Commands.AsyncCommands.ExcelExport.Shared.SoftSimilarityCore;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Soft similarity (closest only)

    private static FieldSimilarity FieldSimilarityOf(
        TransferReceiveDto source,
        TransferReceiveDto candidate,
        TransferReceiveNorm sourceNorm,
        TransferReceiveNorm candidateNorm,
        TransferReceiveField field,
        string sourceOrgOkpo) =>
        field switch
        {
            TransferReceiveField.OperationCode => SimilarityOperationCode(sourceNorm.OpCode, candidateNorm.OpCode),
            TransferReceiveField.OperationDate => SimilarityOperationDate(
                sourceNorm.OpDateRaw, candidateNorm.OpDateRaw, OperationDateToleranceDays, NormalizeDate),
            TransferReceiveField.PassportNumber => SimilarityPassportOrFactory(
                source.PasNum, candidate.PasNum, isFactory: false),
            TransferReceiveField.FactoryNumber => SimilarityPassportOrFactory(
                source.FacNum, candidate.FacNum, isFactory: true),
            TransferReceiveField.Type => SimilarityType(source.Type, candidate.Type),
            TransferReceiveField.Radionuclids => SimilarityRadionuclids(
                sourceNorm.Radionuclids, candidateNorm.Radionuclids),
            TransferReceiveField.Quantity => sourceNorm.Quantity == candidateNorm.Quantity
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0),
            TransferReceiveField.AggregateState => sourceNorm.AggregateState == candidateNorm.AggregateState
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0),
            TransferReceiveField.Sort => sourceNorm.Sort == candidateNorm.Sort
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0),
            TransferReceiveField.Activity => SimilarityNumericWithTolerance(
                sourceNorm.Activity, candidateNorm.Activity),
            TransferReceiveField.ActivityMeasurementDate => DatesEqualExact(
                    source.ActivityMeasurementDate, candidate.ActivityMeasurementDate)
                || string.Equals(
                    sourceNorm.ActivityMeasurementDate,
                    candidateNorm.ActivityMeasurementDate,
                    StringComparison.Ordinal)
                ? FieldSimilarity.Exact
                : SimilarityCreationDate(source.ActivityMeasurementDate, candidate.ActivityMeasurementDate),
            TransferReceiveField.Mass => SimilarityNumericWithTolerance(
                sourceNorm.Mass, candidateNorm.Mass),
            TransferReceiveField.Volume => SimilarityNumericWithTolerance(
                sourceNorm.Volume, candidateNorm.Volume),
            TransferReceiveField.CreatorOkpo => SimilarityTextNormalized(
                sourceNorm.CreatorOkpo, candidateNorm.CreatorOkpo),
            TransferReceiveField.CreationDate => SimilarityCreationDate(source.CreationDate, candidate.CreationDate),
            TransferReceiveField.PackType => SimilarityType(source.PackType, candidate.PackType),
            TransferReceiveField.PackNumber => SimilarityPackNumber(source.PackNumber, candidate.PackNumber),
            TransferReceiveField.ProviderOrRecieverOkpo => SimilarityProviderOkpo(
                candidate.ProviderOrRecieverOkpo, source.OrgOkpo, sourceOrgOkpo),
            _ => FieldSimilarity.Mismatch(0)
        };

    private static FieldSimilarity SimilarityOperationCode(string left, string right)
    {
        if (OpCodesArePaired(left, right))
        {
            return FieldSimilarity.Exact;
        }

        var leftTr = IsTransferOrReceiveCodeForm11(left);
        var rightTr = IsTransferOrReceiveCodeForm11(right);
        if (leftTr && rightTr)
        {
            return FieldSimilarity.Near(0.72);
        }

        return FieldSimilarity.Mismatch(0.15);
    }

    private static FieldSimilarity SimilarityProviderOkpo(
        string? candidateProviderRaw,
        string? sourceOrgRaw,
        string? sourceOrgOkpoRaw)
    {
        if (string.IsNullOrWhiteSpace(candidateProviderRaw) || candidateProviderRaw == "-")
        {
            return FieldSimilarity.Mismatch(0.15);
        }

        var candidateProviderNorm = NormalizeNumber(candidateProviderRaw);
        var sourceOrgNorm = NormalizeNumber(sourceOrgRaw);
        var sourceOrgOkpoNorm = NormalizeNumber(sourceOrgOkpoRaw);

        if (candidateProviderNorm.Length > 0
            && (candidateProviderNorm == sourceOrgNorm || candidateProviderNorm == sourceOrgOkpoNorm))
        {
            return FieldSimilarity.Exact;
        }

        if (OkpoIsEightPrefixOfExtended(candidateProviderRaw, sourceOrgRaw)
            || OkpoIsEightPrefixOfExtended(candidateProviderRaw, sourceOrgOkpoRaw)
            || OkpoIsEightPrefixOfExtended(sourceOrgRaw, candidateProviderRaw)
            || OkpoIsEightPrefixOfExtended(sourceOrgOkpoRaw, candidateProviderRaw))
        {
            return FieldSimilarity.Near(0.99);
        }

        var best = Math.Max(
            NormalizedEditSimilarity(candidateProviderNorm, sourceOrgNorm),
            NormalizedEditSimilarity(candidateProviderNorm, sourceOrgOkpoNorm));
        if (best >= 0.9)
        {
            return FieldSimilarity.Near(best);
        }

        return FieldSimilarity.Mismatch(Math.Min(0.25, best));
    }

    private static double GetFieldWeight(TransferReceiveField field, TransferReceiveDto source)
    {
        var serialsEmpty = SerialNumbersAreEmpty(source);
        return field switch
        {
            TransferReceiveField.PassportNumber => serialsEmpty ? 2.0 : 12.0,
            TransferReceiveField.FactoryNumber => serialsEmpty ? 2.0 : 12.0,
            TransferReceiveField.OperationCode => 1.5,
            TransferReceiveField.OperationDate => 2.0,
            TransferReceiveField.Type => 3.0,
            TransferReceiveField.Radionuclids => 4.5,
            TransferReceiveField.Activity => 2.5,
            TransferReceiveField.ActivityMeasurementDate => 2.5,
            TransferReceiveField.Mass => 2.5,
            TransferReceiveField.Volume => 2.5,
            TransferReceiveField.CreatorOkpo => 4.0,
            TransferReceiveField.CreationDate => 3.0,
            TransferReceiveField.ProviderOrRecieverOkpo => 4.0,
            TransferReceiveField.PackType => 6.5,
            TransferReceiveField.PackNumber => 3.5,
            TransferReceiveField.Quantity => 2.0,
            TransferReceiveField.AggregateState => 3.0,
            TransferReceiveField.Sort => 3.0,
            _ => 1.0
        };
    }

    /// <summary>Бонус, если оба ключевых идентификатора совпали точно (и не пустые).</summary>
    private const double BothIdentifiersExactBonus = 10.0;

    #endregion
}
