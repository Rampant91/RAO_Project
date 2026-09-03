using System;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared.SoftSimilarityCore;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Soft similarity (closest only)

    private static FieldSimilarity FieldSimilarityOf(
        TransferReceiveDto source,
        TransferReceiveDto candidate,
        TransferReceiveNorm sourceNorm,
        TransferReceiveNorm candidateNorm,
        TransferReceiveField field,
        string sourceOrgOkpo,
        int operationDateSearchToleranceDays) =>
        field switch
        {
            TransferReceiveField.OperationCode => SimilarityOperationCode(sourceNorm.OpCode, candidateNorm.OpCode),
            TransferReceiveField.OperationDate => SimilarityOperationDate(
                sourceNorm.OpDateRaw, candidateNorm.OpDateRaw, operationDateSearchToleranceDays, NormalizeDate),
            TransferReceiveField.PassportNumber => SimilarityPassportOrFactory(
                source.PasNum, candidate.PasNum, isFactory: false),
            TransferReceiveField.FactoryNumber => SimilarityPassportOrFactory(
                source.FacNum,
                candidate.FacNum,
                isFactory: true,
                sourceNorm.Quantity,
                candidateNorm.Quantity),
            TransferReceiveField.Type => SimilarityType(source.Type, candidate.Type),
            TransferReceiveField.Radionuclids => SimilarityRadionuclids(
                sourceNorm.Radionuclids, candidateNorm.Radionuclids),
            TransferReceiveField.Quantity => SimilarityQuantityConsideringFactoryRange(
                sourceNorm.Quantity,
                candidateNorm.Quantity,
                source.FacNum,
                candidate.FacNum),
            TransferReceiveField.AggregateState => sourceNorm.AggregateState == candidateNorm.AggregateState
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0),
            TransferReceiveField.Sort => sourceNorm.Sort == candidateNorm.Sort
                ? FieldSimilarity.Exact
                : FieldSimilarity.Mismatch(0),
            TransferReceiveField.Activity => SimilarityActivityConsideringFactoryEnumeration(
                sourceNorm.Activity,
                candidateNorm.Activity,
                source.FacNum,
                candidate.FacNum,
                sourceNorm.Quantity,
                candidateNorm.Quantity),
            TransferReceiveField.ActivityMeasurementDate => DatesEqualExact(
                    source.ActivityMeasurementDate, candidate.ActivityMeasurementDate)
                || string.Equals(
                    sourceNorm.ActivityMeasurementDate,
                    candidateNorm.ActivityMeasurementDate,
                    StringComparison.Ordinal)
                ? FieldSimilarity.Exact
                : SimilarityCalendarDate(source.ActivityMeasurementDate, candidate.ActivityMeasurementDate),
            TransferReceiveField.Mass => SimilarityNumericWithTolerance(
                sourceNorm.Mass, candidateNorm.Mass),
            TransferReceiveField.Volume => SimilarityNumericWithTolerance(
                sourceNorm.Volume, candidateNorm.Volume),
            TransferReceiveField.CreatorOkpo => SimilarityTextNormalized(
                sourceNorm.CreatorOkpo, candidateNorm.CreatorOkpo),
            TransferReceiveField.CreationDate => SimilarityCalendarDate(source.CreationDate, candidate.CreationDate),
            TransferReceiveField.PackType => SimilarityType(source.PackType, candidate.PackType),
            TransferReceiveField.PackNumber => SimilarityPackNumber(source.PackNumber, candidate.PackNumber),
            TransferReceiveField.StatusRao => SimilarityTextNormalized(
                sourceNorm.StatusRao, candidateNorm.StatusRao),
            TransferReceiveField.CodeRao => SimilarityTextNormalized(
                sourceNorm.CodeRao, candidateNorm.CodeRao),
            TransferReceiveField.PackName => SimilarityType(source.PackName, candidate.PackName),
            TransferReceiveField.Subsidy => SimilaritySubsidy(source.Subsidy, candidate.Subsidy),
            TransferReceiveField.FcpNumber => SimilarityPassportOrFactory(
                source.FcpNumber, candidate.FcpNumber, isFactory: false),
            TransferReceiveField.TritiumActivity => SimilarityNumericWithTolerance(
                sourceNorm.TritiumActivity, candidateNorm.TritiumActivity),
            TransferReceiveField.BetaGammaActivity => SimilarityNumericWithTolerance(
                sourceNorm.BetaGammaActivity, candidateNorm.BetaGammaActivity),
            TransferReceiveField.AlphaActivity => SimilarityNumericWithTolerance(
                sourceNorm.AlphaActivity, candidateNorm.AlphaActivity),
            TransferReceiveField.TransuraniumActivity => SimilarityNumericWithTolerance(
                sourceNorm.TransuraniumActivity, candidateNorm.TransuraniumActivity),
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

        if (TryProviderOkpoSoftNear(candidateProviderNorm, sourceOrgNorm, out var nearScore)
            || TryProviderOkpoSoftNear(candidateProviderNorm, sourceOrgOkpoNorm, out nearScore))
        {
            return FieldSimilarity.Near(nearScore);
        }

        var best = Math.Max(
            NormalizedEditSimilarity(candidateProviderNorm, sourceOrgNorm),
            NormalizedEditSimilarity(candidateProviderNorm, sourceOrgOkpoNorm));
        return FieldSimilarity.Mismatch(Math.Min(0.25, best));
    }

    /// <summary>
    /// Близкое ОКПО для подсветки: высокая схожесть, либо длинный номер с несколькими опечатками в цифрах.
    /// Короткие (8 цифр) не смягчаем ниже порога 0.9 — слишком легко склеить разных юрлиц.
    /// </summary>
    private const int ProviderOkpoLongMinLength = 12;

    private const int ProviderOkpoLongMaxHammingNear = 5;

    private static bool TryProviderOkpoSoftNear(string left, string right, out double score)
    {
        score = 0;
        if (left.Length == 0 || right.Length == 0)
        {
            return false;
        }

        var similarity = NormalizedEditSimilarity(left, right);
        if (similarity >= 0.9)
        {
            score = similarity;
            return true;
        }

        var maxLen = Math.Max(left.Length, right.Length);
        if (maxLen < ProviderOkpoLongMinLength
            || left.Length < ProviderOkpoLongMinLength
            || right.Length < ProviderOkpoLongMinLength)
        {
            return false;
        }

        if (left.Length == right.Length)
        {
            var hamming = 0;
            for (var i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    hamming++;
                    if (hamming > ProviderOkpoLongMaxHammingNear)
                    {
                        break;
                    }
                }
            }

            if (hamming is > 0 and <= ProviderOkpoLongMaxHammingNear)
            {
                score = Math.Max(0.70, 1.0 - hamming / (double)left.Length);
                return true;
            }
        }

        var distance = LevenshteinDistance(left, right);
        if (distance is > 0 and <= 4 && similarity >= 0.70)
        {
            score = Math.Max(0.70, similarity);
            return true;
        }

        return false;
    }

    private static double GetFieldWeight(
        TransferReceiveField field,
        TransferReceiveDto source,
        TransferReceiveSheetLayout layout = TransferReceiveSheetLayout.Form11)
    {
        if (field == TransferReceiveField.StatusRao && IsStatusRaoExemptOpCode(source.OpCode))
        {
            return 0;
        }

        var serialsEmpty = SerialNumbersAreEmpty(source);
        if (layout == TransferReceiveSheetLayout.Form16)
        {
            return field switch
            {
                TransferReceiveField.PackNumber => 8.0,
                TransferReceiveField.CodeRao => 6.0,
                TransferReceiveField.Radionuclids => 6.0,
                TransferReceiveField.ProviderOrRecieverOkpo => 5.5,
                TransferReceiveField.Volume => 3.5,
                TransferReceiveField.Mass => 3.5,
                TransferReceiveField.TritiumActivity => 3.0,
                TransferReceiveField.BetaGammaActivity => 3.0,
                TransferReceiveField.AlphaActivity => 3.0,
                TransferReceiveField.TransuraniumActivity => 3.0,
                TransferReceiveField.Quantity => 3.0,
                TransferReceiveField.StatusRao => 3.0,
                TransferReceiveField.OperationDate => 2.5,
                TransferReceiveField.ActivityMeasurementDate => 2.5,
                TransferReceiveField.PackType => 2.0,
                TransferReceiveField.Subsidy => 2.0,
                TransferReceiveField.FcpNumber => 2.0,
                TransferReceiveField.OperationCode => 1.5,
                _ => 1.0
            };
        }

        if (layout == TransferReceiveSheetLayout.Form15)
        {
            return field switch
            {
                TransferReceiveField.PassportNumber => serialsEmpty ? 2.0 : 12.0,
                TransferReceiveField.FactoryNumber => serialsEmpty ? 2.0 : 12.0,
                TransferReceiveField.OperationCode => 1.5,
                TransferReceiveField.OperationDate => 2.0,
                TransferReceiveField.Type => 3.0,
                TransferReceiveField.Radionuclids => 4.5,
                TransferReceiveField.Activity => 2.5,
                TransferReceiveField.CreationDate => 2.0,
                TransferReceiveField.StatusRao => 3.0,
                TransferReceiveField.ProviderOrRecieverOkpo => 5.5,
                TransferReceiveField.PackName => 2.0,
                TransferReceiveField.PackType => 2.0,
                TransferReceiveField.PackNumber => 2.0,
                TransferReceiveField.Subsidy => 2.0,
                TransferReceiveField.FcpNumber => 2.0,
                TransferReceiveField.Quantity => 2.0,
                _ => 1.0
            };
        }

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
            TransferReceiveField.ProviderOrRecieverOkpo => 5.5,
            TransferReceiveField.PackType => 6.5,
            TransferReceiveField.PackNumber => 3.5,
            TransferReceiveField.Quantity => 2.0,
            TransferReceiveField.AggregateState => 3.0,
            TransferReceiveField.Sort => 3.0,
            _ => 1.0
        };
    }
    private const double BothIdentifiersExactBonus = 10.0;

    #endregion
}
