using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources.CustomComparers.SnkComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    private static readonly SnkNumberEqualityComparer NumberComparer = new();

    private static List<Operation41PairingDto> GetUnpairedOperations11To15(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options)
    {
        var sourceWithSerial = source.Where(item => !Operation41PairingKeyComparer.SerialNumbersIsEmpty(item.PasNum, item.FacNum)).ToList();
        var sourceWithoutSerial = source.Where(item => Operation41PairingKeyComparer.SerialNumbersIsEmpty(item.PasNum, item.FacNum)).ToList();
        var referenceWithSerial = reference.Where(item => !Operation41PairingKeyComparer.SerialNumbersIsEmpty(item.PasNum, item.FacNum)).ToList();
        var referenceWithoutSerial = reference.Where(item => Operation41PairingKeyComparer.SerialNumbersIsEmpty(item.PasNum, item.FacNum)).ToList();

        var unpaired = new List<Operation41PairingDto>();
        unpaired.AddRange(FindUnpairedWithSerial(sourceWithSerial, referenceWithSerial, options));
        unpaired.AddRange(FindUnpairedWithoutSerial(sourceWithoutSerial, referenceWithoutSerial, options));
        return unpaired;
    }

    private static List<Operation41PairingDto> FindUnpairedWithSerial(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options)
    {
        var referenceByKey = reference
            .GroupBy(item => BuildPairingKey(item, options, includeSerial: true, includeQuantity: options.CheckQuantity))
            .ToDictionary(group => group.Key, group => group.ToList(), StringComparer.Ordinal);

        var unpaired = new List<Operation41PairingDto>();
        foreach (var sourceItem in source)
        {
            var key = BuildPairingKey(sourceItem, options, includeSerial: true, includeQuantity: options.CheckQuantity);
            if (!referenceByKey.TryGetValue(key, out var candidates) || candidates.Count == 0)
            {
                unpaired.Add(sourceItem);
                continue;
            }

            var matchIndex = candidates.FindIndex(candidate => ActivityMatches(sourceItem, candidate, options.CheckActivity));
            if (matchIndex < 0)
            {
                unpaired.Add(sourceItem);
                continue;
            }

            candidates.RemoveAt(matchIndex);
        }

        return unpaired;
    }

    private static List<Operation41PairingDto> FindUnpairedWithoutSerial(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Pairing11To15Params options)
    {
        var sourceByKey = source
            .GroupBy(item => BuildPairingKey(item, options, includeSerial: false, includeQuantity: false))
            .ToDictionary(group => group.Key, group => group.ToList(), StringComparer.Ordinal);
        var referenceByKey = reference
            .GroupBy(item => BuildPairingKey(item, options, includeSerial: false, includeQuantity: false))
            .ToDictionary(group => group.Key, group => group.ToList(), StringComparer.Ordinal);

        var unpaired = new List<Operation41PairingDto>();
        foreach (var sourceGroup in sourceByKey)
        {
            if (!referenceByKey.TryGetValue(sourceGroup.Key, out var refRows))
            {
                unpaired.AddRange(sourceGroup.Value);
                continue;
            }

            var refStates = refRows
                .Select(row => new RemainingRowState(row, GetQuantityForComparison(row, options.CheckQuantity)))
                .ToList();

            foreach (var sourceRow in sourceGroup.Value)
            {
                var remainingSourceQty = GetQuantityForComparison(sourceRow, options.CheckQuantity);
                for (var i = 0; i < refStates.Count && remainingSourceQty > 0; i++)
                {
                    if (refStates[i].RemainingQuantity <= 0)
                    {
                        continue;
                    }

                    if (!ActivityMatches(sourceRow, refStates[i].Row, options.CheckActivity))
                    {
                        continue;
                    }

                    var matchedQty = Math.Min(remainingSourceQty, refStates[i].RemainingQuantity);
                    remainingSourceQty -= matchedQty;
                    refStates[i].RemainingQuantity -= matchedQty;
                }

                if (remainingSourceQty > 0)
                {
                    unpaired.Add(sourceRow);
                }
            }
        }

        return unpaired;
    }

    private static int GetQuantityForComparison(Operation41PairingDto row, bool checkQuantity) =>
        checkQuantity ? row.Quantity is > 0 ? row.Quantity.Value : 1 : 1;

    private static bool ActivityMatches(Operation41PairingDto left, Operation41PairingDto right, bool checkActivity)
    {
        if (!checkActivity)
        {
            return true;
        }

        if (!TryParseActivity(left.Activity, out var leftActivity) || !TryParseActivity(right.Activity, out var rightActivity))
        {
            return NumberComparer.Equals(left.Activity, right.Activity);
        }

        var scale = Math.Max(Math.Abs(leftActivity), Math.Abs(rightActivity));
        if (scale <= double.Epsilon)
        {
            return true;
        }

        return Math.Abs(leftActivity - rightActivity) <= scale * 0.10;
    }

    private static bool TryParseActivity(string? value, out double activity)
    {
        activity = 0;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = value.Trim().Replace(" ", string.Empty).Replace(',', '.');
        return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out activity);
    }

    private static bool NumericWithTolerance(string? left, string? right)
    {
        if (!TryParseActivity(left, out var leftNum) || !TryParseActivity(right, out var rightNum))
        {
            return NumberComparer.Equals(left, right);
        }
        var scale = Math.Max(Math.Abs(leftNum), Math.Abs(rightNum));
        if (scale <= double.Epsilon) return true;
        return Math.Abs(leftNum - rightNum) <= scale * 0.10;
    }

    private static string BuildPairingKey(
        Operation41PairingDto row,
        Pairing11To15Params options,
        bool includeSerial,
        bool includeQuantity)
    {
        var parts = new List<string>(16);
        if (options.CheckOperationDate) parts.Add(NormalizeDate(row.OpDate));
        if (includeSerial && options.CheckPassportNumber) parts.Add(NormalizeNumber(row.PasNum));
        if (options.CheckType) parts.Add(NormalizeNumber(row.Type));
        if (options.CheckRadionuclids) parts.Add(NormalizeRads(row.Radionuclids));
        if (includeSerial && options.CheckFactoryNumber) parts.Add(NormalizeNumber(row.FacNum));
        if (options.CheckCreationDate) parts.Add(NormalizeDate(row.CreationDate));
        if (options.CheckDocumentVid) parts.Add(NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(row.DocumentVid)));
        if (options.CheckDocumentNumber) parts.Add(NormalizeNumber(row.DocumentNumber));
        if (options.CheckDocumentDate) parts.Add(NormalizeDate(row.DocumentDate));
        if (options.CheckProviderOrRecieverOkpo) parts.Add(NormalizeNumber(row.ProviderOrRecieverOkpo));
        if (options.CheckTransporterOkpo) parts.Add(NormalizeNumber(row.TransporterOkpo));
        if (options.CheckPackName) parts.Add(NormalizeNumber(row.PackName));
        if (options.CheckPackType) parts.Add(NormalizeNumber(row.PackType));
        if (options.CheckPackNumber) parts.Add(NormalizeNumber(row.PackNumber));
        if (includeQuantity && options.CheckQuantity) parts.Add(GetQuantityForComparison(row, true).ToString(CultureInfo.InvariantCulture));
        return string.Join('|', parts);
    }

    private static string NormalizeDate(string? value) =>
        DateOnly.TryParse(value, out var date)
            ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            : NormalizeNumber(value);

    private static string NormalizeNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "-")
        {
            return string.Empty;
        }

        var normalized = Regex.Replace(value.ToLowerInvariant(), @"[\\/:*?""<>|.,_\-;:\s+]", string.Empty);
        return normalized.TrimStart('0');
    }

    private static string NormalizeRads(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var normalizedSet = value.Split([',', ';'])
            .Select(x => SnkRadionuclidsEqualityComparer.SnkRegex().Replace(x, "").ToLowerInvariant())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x
                .Replace('а', 'a')
                .Replace('б', 'b')
                .Replace('в', 'b')
                .Replace('г', 'r')
                .Replace('е', 'e')
                .Replace('ё', 'e')
                .Replace('з', '3')
                .Replace('к', 'k')
                .Replace('м', 'm')
                .Replace('н', 'h')
                .Replace('о', 'o')
                .Replace('0', 'o')
                .Replace('р', 'p')
                .Replace('с', 'c')
                .Replace('т', 't')
                .Replace('у', 'y')
                .Replace('х', 'x'))
            .OrderBy(x => x);

        return string.Join("|", normalizedSet);
    }

    private sealed class RemainingRowState(Operation41PairingDto row, int remainingQuantity)
    {
        public Operation41PairingDto Row { get; } = row;
        public int RemainingQuantity { get; set; } = remainingQuantity;
    }

    private static List<Operation41PairingDto> GetUnpairedOperations(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Operation41PairingProfile profile,
        Func<Operation41PairingDto, Operation41PairingKey> toSourceKey,
        Func<Operation41PairingDto, Operation41PairingKey> toReferenceKey) =>
        Operation41PairingMatcher.FindUnpaired(source, reference, profile, toSourceKey, toReferenceKey);

    private static List<Operation41PairingDto> GetUnpairedOperations12To16(
        List<Operation41PairingDto> source, List<Operation41PairingDto> reference, Pairing12To16Params options) =>
        GetUnpairedByPredicate(source, reference, (left, right) =>
            (!options.CheckOperationDate || NormalizeDate(left.OpDate) == NormalizeDate(right.OpDate))
            && (!options.CheckMass || NumericWithTolerance(left.Mass, right.Mass))
            && (!options.CheckBetaGammaActivity || NumericWithTolerance(left.BetaGammaActivity, right.BetaGammaActivity))
            && (!options.CheckAlphaActivity || NumericWithTolerance(left.AlphaActivity, right.AlphaActivity))
            && (!options.CheckActivityMeasurementDate || NormalizeDate(left.ActivityMeasurementDate) == NormalizeDate(right.ActivityMeasurementDate))
            && (!options.CheckDocumentVid || NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(left.DocumentVid)) == NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(right.DocumentVid)))
            && (!options.CheckDocumentNumber || NormalizeNumber(left.DocumentNumber) == NormalizeNumber(right.DocumentNumber))
            && (!options.CheckDocumentDate || NormalizeDate(left.DocumentDate) == NormalizeDate(right.DocumentDate))
            && (!options.CheckPackName || NormalizeNumber(left.PackName) == NormalizeNumber(right.PackName))
            && (!options.CheckPackType || NormalizeNumber(left.PackType) == NormalizeNumber(right.PackType))
            && (!options.CheckPackNumber || NormalizeNumber(left.PackNumber) == NormalizeNumber(right.PackNumber)));

    private static List<Operation41PairingDto> GetUnpairedOperations13To16(
        List<Operation41PairingDto> source, List<Operation41PairingDto> reference, Pairing13To16Params options) =>
        GetUnpairedByPredicate(source, reference, (left, right) =>
            (!options.CheckOperationDate || NormalizeDate(left.OpDate) == NormalizeDate(right.OpDate))
            && (!options.CheckMainRadionuclids || NormalizeRads(left.MainRadionuclids) == NormalizeRads(right.MainRadionuclids))
            && (!options.CheckTritiumActivity || NumericWithTolerance(left.TritiumActivity, right.TritiumActivity))
            && (!options.CheckBetaGammaActivity || NumericWithTolerance(left.BetaGammaActivity, right.BetaGammaActivity))
            && (!options.CheckAlphaActivity || NumericWithTolerance(left.AlphaActivity, right.AlphaActivity))
            && (!options.CheckTransuraniumActivity || NumericWithTolerance(left.TransuraniumActivity, right.TransuraniumActivity))
            && (!options.CheckActivityMeasurementDate || NormalizeDate(left.ActivityMeasurementDate) == NormalizeDate(right.ActivityMeasurementDate))
            && (!options.CheckDocumentVid || NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(left.DocumentVid)) == NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(right.DocumentVid)))
            && (!options.CheckDocumentNumber || NormalizeNumber(left.DocumentNumber) == NormalizeNumber(right.DocumentNumber))
            && (!options.CheckDocumentDate || NormalizeDate(left.DocumentDate) == NormalizeDate(right.DocumentDate))
            && (!options.CheckPackName || NormalizeNumber(left.PackName) == NormalizeNumber(right.PackName))
            && (!options.CheckPackType || NormalizeNumber(left.PackType) == NormalizeNumber(right.PackType))
            && (!options.CheckPackNumber || NormalizeNumber(left.PackNumber) == NormalizeNumber(right.PackNumber)));

    private static List<Operation41PairingDto> GetUnpairedOperations14To16(
        List<Operation41PairingDto> source, List<Operation41PairingDto> reference, Pairing14To16Params options) =>
        GetUnpairedByPredicate(source, reference, (left, right) =>
            (!options.CheckOperationDate || NormalizeDate(left.OpDate) == NormalizeDate(right.OpDate))
            && (!options.CheckVolume || NumericWithTolerance(left.Volume, right.Volume))
            && (!options.CheckMass || NumericWithTolerance(left.Mass, right.Mass))
            && (!options.CheckMainRadionuclids || NormalizeRads(left.MainRadionuclids) == NormalizeRads(right.MainRadionuclids))
            && (!options.CheckTritiumActivity || NumericWithTolerance(left.TritiumActivity, right.TritiumActivity))
            && (!options.CheckBetaGammaActivity || NumericWithTolerance(left.BetaGammaActivity, right.BetaGammaActivity))
            && (!options.CheckAlphaActivity || NumericWithTolerance(left.AlphaActivity, right.AlphaActivity))
            && (!options.CheckTransuraniumActivity || NumericWithTolerance(left.TransuraniumActivity, right.TransuraniumActivity))
            && (!options.CheckActivityMeasurementDate || NormalizeDate(left.ActivityMeasurementDate) == NormalizeDate(right.ActivityMeasurementDate))
            && (!options.CheckDocumentVid || NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(left.DocumentVid)) == NormalizeNumber(Operation41PairingKeyComparer.NormalizeDocumentVid(right.DocumentVid)))
            && (!options.CheckDocumentNumber || NormalizeNumber(left.DocumentNumber) == NormalizeNumber(right.DocumentNumber))
            && (!options.CheckDocumentDate || NormalizeDate(left.DocumentDate) == NormalizeDate(right.DocumentDate))
            && (!options.CheckPackName || NormalizeNumber(left.PackName) == NormalizeNumber(right.PackName))
            && (!options.CheckPackType || NormalizeNumber(left.PackType) == NormalizeNumber(right.PackType))
            && (!options.CheckPackNumber || NormalizeNumber(left.PackNumber) == NormalizeNumber(right.PackNumber)));

    private static List<Operation41PairingDto> GetUnpairedByPredicate(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Func<Operation41PairingDto, Operation41PairingDto, bool> isMatch)
    {
        var refs = new List<Operation41PairingDto>(reference);
        var unpaired = new List<Operation41PairingDto>();
        foreach (var row in source)
        {
            var idx = refs.FindIndex(x => isMatch(row, x));
            if (idx >= 0) refs.RemoveAt(idx);
            else unpaired.Add(row);
        }
        return unpaired;
    }

    private static Operation41PairingKey ToPairingKey11(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm11(
            dto.OpCode,
            dto.OpDate,
            dto.PasNum,
            dto.FacNum,
            dto.Type,
            dto.Radionuclids,
            dto.CreationDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackNumber,
            dto.Quantity);

    private static Operation41PairingKey ToPairingKey15(Operation41PairingDto dto) =>
        ToPairingKey11(dto);

    private static Operation41PairingKey ToPairingKey12(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm12(
            dto.OpCode,
            dto.OpDate,
            dto.PasNum,
            dto.FacNum,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static Operation41PairingKey ToPairingKey13(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm13(
            dto.OpCode,
            dto.OpDate,
            dto.PasNum,
            dto.FacNum,
            dto.Type,
            dto.Radionuclids,
            dto.CreationDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static Operation41PairingKey ToPairingKey14(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm14(
            dto.OpCode,
            dto.OpDate,
            dto.PasNum,
            dto.Radionuclids,
            dto.ActivityMeasurementDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static Operation41PairingKey ToPairingKey16For12(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm16For12(
            dto.OpCode,
            dto.OpDate,
            dto.MainRadionuclids,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber,
            dto.ActivityMeasurementDate);

    private static Operation41PairingKey ToPairingKey16For13(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm16For13(
            dto.OpCode,
            dto.OpDate,
            dto.MainRadionuclids,
            dto.ActivityMeasurementDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static Operation41PairingKey ToPairingKey16For14(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm16For14(
            dto.OpCode,
            dto.OpDate,
            dto.MainRadionuclids,
            dto.ActivityMeasurementDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static async Task<List<Operation41PairingDto>> LoadOperation41ListAsync(
        DBModel db, int repsId, string formNum, CancellationToken cancellationToken, Pairing11To15Params? pairing11To15Params = null)
    {
        var operations = formNum switch
        {
            "1.1" => await LoadForm11OperationsAsync(db, repsId, cancellationToken, pairing11To15Params),
            "1.2" => await LoadForm12OperationsAsync(db, repsId, cancellationToken),
            "1.3" => await LoadForm13OperationsAsync(db, repsId, cancellationToken),
            "1.4" => await LoadForm14OperationsAsync(db, repsId, cancellationToken),
            "1.5" => await LoadForm15OperationsAsync(db, repsId, cancellationToken, pairing11To15Params),
            "1.6" => await LoadForm16OperationsAsync(db, repsId, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        return operations
            .Where(form => string.Equals(form.OpCode.Trim(), OperationCode, StringComparison.Ordinal))
            .ToList();
    }

    private static Task<List<Operation41PairingDto>> LoadForm11OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken, Pairing11To15Params? options = null) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1")
                .SelectMany(rep => rep.Rows11))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = options == null || options.CheckOperationDate ? form.OperationDate_DB : string.Empty,
                PasNum = options == null || options.CheckPassportNumber ? form.PassportNumber_DB : string.Empty,
                FacNum = options == null || options.CheckFactoryNumber ? form.FactoryNumber_DB : string.Empty,
                Type = options == null || options.CheckType ? form.Type_DB : string.Empty,
                Radionuclids = options == null || options.CheckRadionuclids ? form.Radionuclids_DB : string.Empty,
                CreationDate = options == null || options.CheckCreationDate ? form.CreationDate_DB : string.Empty,
                DocumentVid = options == null || options.CheckDocumentVid ? form.DocumentVid_DB : null,
                DocumentNumber = options == null || options.CheckDocumentNumber ? form.DocumentNumber_DB : string.Empty,
                DocumentDate = options == null || options.CheckDocumentDate ? form.DocumentDate_DB : string.Empty,
                ProviderOrRecieverOkpo = options == null || options.CheckProviderOrRecieverOkpo ? form.ProviderOrRecieverOKPO_DB : string.Empty,
                TransporterOkpo = options == null || options.CheckTransporterOkpo ? form.TransporterOKPO_DB : string.Empty,
                PackName = options == null || options.CheckPackName ? form.PackName_DB : string.Empty,
                PackType = options == null || options.CheckPackType ? form.PackType_DB : string.Empty,
                PackNumber = options == null || options.CheckPackNumber ? form.PackNumber_DB : string.Empty,
                Activity = options == null || options.CheckActivity ? form.Activity_DB : string.Empty,
                Quantity = options == null || options.CheckQuantity ? form.Quantity_DB : null
            })
            .ToListAsync(cancellationToken);

    private static async Task<List<Operation41PairingDto>> LoadForm12OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken)
    {
        var forms = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.2")
                .SelectMany(rep => rep.Rows12))
            .Where(form => form.OperationCode_DB == OperationCode)
            .ToListAsync(cancellationToken);

        return forms.Select(form =>
        {
            var massTon = ToMassTon(form.Mass_DB);
            return new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Mass = massTon,
                BetaGammaActivity = ComputeFromMass(massTon, 25_000_000_000d),
                AlphaActivity = ComputeFromMass(massTon, 16_100_000_000d),
                ActivityMeasurementDate = form.OperationDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB
            };
        }).ToList();
    }

    private static async Task<List<Operation41PairingDto>> LoadForm13OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken)
    {
        var forms = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.3")
                .SelectMany(rep => rep.Rows13))
            .Where(form => form.OperationCode_DB == OperationCode)
            .ToListAsync(cancellationToken);

        return forms.Select(form =>
        {
            var activities = GetActivitiesForExport(form.Radionuclids_DB, form.Activity_DB);
            return new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                CreationDate = form.CreationDate_DB,
                MainRadionuclids = form.Radionuclids_DB,
                TritiumActivity = activities["tritium"],
                BetaGammaActivity = activities["beta"],
                AlphaActivity = activities["alpha"],
                TransuraniumActivity = activities["transuranium"],
                ActivityMeasurementDate = form.CreationDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB
            };
        }).ToList();
    }

    private static async Task<List<Operation41PairingDto>> LoadForm14OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken)
    {
        var forms = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.4")
                .SelectMany(rep => rep.Rows14))
            .Where(form => form.OperationCode_DB == OperationCode)
            .ToListAsync(cancellationToken);

        return forms.Select(form =>
        {
            var activities = GetActivitiesForExport(form.Radionuclids_DB, form.Activity_DB);
            return new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                Radionuclids = form.Radionuclids_DB,
                MainRadionuclids = form.Radionuclids_DB,
                Volume = form.Volume_DB,
                Mass = ToMassTon(form.Mass_DB),
                TritiumActivity = activities["tritium"],
                BetaGammaActivity = activities["beta"],
                AlphaActivity = activities["alpha"],
                TransuraniumActivity = activities["transuranium"],
                ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB
            };
        }).ToList();
    }

    private static Task<List<Operation41PairingDto>> LoadForm15OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken, Pairing11To15Params? options = null) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.5")
                .SelectMany(rep => rep.Rows15))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = options == null || options.CheckOperationDate ? form.OperationDate_DB : string.Empty,
                PasNum = options == null || options.CheckPassportNumber ? form.PassportNumber_DB : string.Empty,
                FacNum = options == null || options.CheckFactoryNumber ? form.FactoryNumber_DB : string.Empty,
                Type = options == null || options.CheckType ? form.Type_DB : string.Empty,
                Radionuclids = options == null || options.CheckRadionuclids ? form.Radionuclids_DB : string.Empty,
                CreationDate = options == null || options.CheckCreationDate ? form.CreationDate_DB : string.Empty,
                DocumentVid = options == null || options.CheckDocumentVid ? form.DocumentVid_DB : null,
                DocumentNumber = options == null || options.CheckDocumentNumber ? form.DocumentNumber_DB : string.Empty,
                DocumentDate = options == null || options.CheckDocumentDate ? form.DocumentDate_DB : string.Empty,
                ProviderOrRecieverOkpo = options == null || options.CheckProviderOrRecieverOkpo ? form.ProviderOrRecieverOKPO_DB : string.Empty,
                TransporterOkpo = options == null || options.CheckTransporterOkpo ? form.TransporterOKPO_DB : string.Empty,
                PackName = options == null || options.CheckPackName ? form.PackName_DB : string.Empty,
                PackType = options == null || options.CheckPackType ? form.PackType_DB : string.Empty,
                PackNumber = options == null || options.CheckPackNumber ? form.PackNumber_DB : string.Empty,
                Activity = options == null || options.CheckActivity ? form.Activity_DB : string.Empty,
                Quantity = options == null || options.CheckQuantity ? form.Quantity_DB : null
            })
            .ToListAsync(cancellationToken);

    private static Task<List<Operation41PairingDto>> LoadForm16OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.6")
                .SelectMany(rep => rep.Rows16))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                MainRadionuclids = form.MainRadionuclids_DB,
                Volume = form.Volume_DB,
                Mass = form.Mass_DB,
                TritiumActivity = form.TritiumActivity_DB,
                BetaGammaActivity = form.BetaGammaActivity_DB,
                AlphaActivity = form.AlphaActivity_DB,
                TransuraniumActivity = form.TransuraniumActivity_DB,
                ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB
            })
            .ToListAsync(cancellationToken);

    private static async Task<Reports> BuildReportsForExportAsync(
        DBModel db,
        Reports masterReports,
        List<Operation41PairingDto> unpairedOperations,
        string formNum,
        CancellationToken cancellationToken)
    {
        var result = new Reports { Master = masterReports.Master };

        if (unpairedOperations.Count == 0)
        {
            return result;
        }

        var formIds = unpairedOperations.Select(operation => operation.Id).Distinct().ToList();
        var reportIds = unpairedOperations
            .Where(operation => operation.ReportId != 0)
            .Select(operation => operation.ReportId)
            .Distinct()
            .ToList();

        if (reportIds.Count == 0)
        {
            return result;
        }

        var reports = await LoadReportsByIdsAsync(db, reportIds, cancellationToken);

        switch (formNum)
        {
            case "1.1":
            {
                var forms = await LoadForm11ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows11, (report, form) => report.Rows11.Add(form));
                break;
            }
            case "1.2":
            {
                var forms = await LoadForm12ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows12, (report, form) => report.Rows12.Add(form));
                break;
            }
            case "1.3":
            {
                var forms = await LoadForm13ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows13, (report, form) => report.Rows13.Add(form));
                break;
            }
            case "1.4":
            {
                var forms = await LoadForm14ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows14, (report, form) => report.Rows14.Add(form));
                break;
            }
            case "1.5":
            {
                var forms = await LoadForm15ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows15, (report, form) => report.Rows15.Add(form));
                break;
            }
            case "1.6":
            {
                var forms = await LoadForm16ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows16, (report, form) => report.Rows16.Add(form));
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null);
        }

        foreach (var report in OrderReportsForExport(reports))
        {
            if (ReportHasUnpairedRows(report, formNum))
            {
                result.Report_Collection.Add(report);
            }
        }

        return result;
    }

    private static void AttachFormsToReports<TForm>(
        List<Report> reports,
        List<TForm> forms,
        Func<Report, ICollection<TForm>> getRows,
        Action<Report, TForm> addRow)
        where TForm : Form1
    {
        var formsByReportId = forms
            .GroupBy(form => form.ReportId ?? 0)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(form => form.NumberInOrder_DB).ToList());

        foreach (var report in OrderReportsForExport(reports))
        {
            if (!formsByReportId.TryGetValue(report.Id, out var rowList) || rowList.Count == 0)
            {
                continue;
            }

            getRows(report).Clear();
            foreach (var form in rowList)
            {
                addRow(report, form);
            }
        }
    }

    private static async Task<List<Report>> LoadReportsByIdsAsync(
        DBModel db,
        IReadOnlyList<int> reportIds,
        CancellationToken cancellationToken)
    {
        var reports = new List<Report>();
        foreach (var idChunk in ChunkIds(reportIds))
        {
            var batch = await db.ReportCollectionDbSet
                .AsNoTracking()
                .Where(rep => idChunk.Contains(rep.Id))
                .ToListAsync(cancellationToken);
            reports.AddRange(batch);
        }

        return reports;
    }

    private static async Task<List<Form11>> LoadForm11ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_11, formIds, cancellationToken);

    private static async Task<List<Form12>> LoadForm12ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_12, formIds, cancellationToken);

    private static async Task<List<Form13>> LoadForm13ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_13, formIds, cancellationToken);

    private static async Task<List<Form14>> LoadForm14ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_14, formIds, cancellationToken);

    private static async Task<List<Form15>> LoadForm15ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_15, formIds, cancellationToken);

    private static async Task<List<Form16>> LoadForm16ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_16, formIds, cancellationToken);

    private static async Task<List<TForm>> LoadFormsByIdsAsync<TForm>(
        DbSet<TForm> dbSet,
        IReadOnlyList<int> formIds,
        CancellationToken cancellationToken)
        where TForm : Form1
    {
        var forms = new List<TForm>();
        foreach (var idChunk in ChunkIds(formIds))
        {
            var batch = await dbSet
                .AsNoTracking()
                .Where(form => idChunk.Contains(form.Id))
                .ToListAsync(cancellationToken);
            forms.AddRange(batch);
        }

        return forms;
    }

    private static IEnumerable<List<int>> ChunkIds(IReadOnlyList<int> ids)
    {
        for (var offset = 0; offset < ids.Count; offset += FirebirdInListMaxCount)
        {
            yield return ids.Skip(offset).Take(FirebirdInListMaxCount).ToList();
        }
    }

    private static bool ReportHasUnpairedRows(Report report, string formNum) =>
        formNum switch
        {
            "1.1" => report.Rows11.Count > 0,
            "1.2" => report.Rows12.Count > 0,
            "1.3" => report.Rows13.Count > 0,
            "1.4" => report.Rows14.Count > 0,
            "1.5" => report.Rows15.Count > 0,
            "1.6" => report.Rows16.Count > 0,
            _ => false
        };

    private static IEnumerable<Report> OrderReportsForExport(List<Report> reports) =>
        reports
            .OrderBy(rep => DateOnly.TryParse(rep.StartPeriod_DB, out var startDate) ? startDate : DateOnly.MaxValue)
            .ThenBy(rep => DateOnly.TryParse(rep.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue);

    private static readonly List<Dictionary<string, string>> R = [];

    private static void EnsureRDictionaryLoaded()
    {
        if (R.Count != 0) return;
        var filePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx");
        if (!File.Exists(filePath))
        {
            filePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", "R.xlsx");
        }
        if (!File.Exists(filePath)) return;

        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using var xls = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath));
        var ws = xls.Workbook.Worksheets["Лист1"];
        if (ws is null) return;
        for (var i = 2; ws.Cells[i, 1].Text != string.Empty; i++)
        {
            R.Add(new Dictionary<string, string>
            {
                { "name", ws.Cells[i, 1].Text },
                { "code", ws.Cells[i, 8].Text }
            });
        }
    }

    private static string ToMassTon(string? mass)
    {
        var massTmp = (mass ?? "").ToLower().Replace('.', ',').Replace("(", "").Replace(")", "").Replace('е', 'e').Trim();
        return double.TryParse(massTmp, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
            new CultureInfo("ru-RU", useUserOverride: false), out var value)
            ? $"{value / 1000:0.######################################################e+00}"
            : "";
    }

    private static string ComputeFromMass(string massTon, double coef) =>
        double.TryParse(massTon.Replace('.', ','), NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent,
            new CultureInfo("ru-RU", useUserOverride: false), out var value)
            ? $"{value * coef:0.######################################################e+00}"
            : "";

    private static Dictionary<string, string> GetActivitiesForExport(string? radionuclids, string? activityRaw)
    {
        EnsureRDictionaryLoaded();
        var nuclids = (radionuclids ?? "").Replace(" ", string.Empty).ToLower().Replace(',', ';').Split(';', StringSplitOptions.RemoveEmptyEntries);
        var nuclidTypes = R.Where(x => nuclids.Contains(x["name"])).Select(x => x["code"]).ToArray();
        var activityTmp = (activityRaw ?? "").Replace(".", ",").Replace("(", "").Replace(")", "");
        var activity = double.TryParse(activityTmp, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
            new CultureInfo("ru-RU", useUserOverride: false), out var activityDoubleValue)
            ? $"{activityDoubleValue:0.######################################################e+00}"
            : activityTmp;

        var result = new Dictionary<string, string>
        {
            { "alpha", "-" }, { "beta", "-" }, { "tritium", "-" }, { "transuranium", "-" }
        };
        if (nuclidTypes.Length == 0) return result;
        if (nuclidTypes.Length == 1 || nuclidTypes.Skip(1).All(x => string.Equals(nuclidTypes[0], x, StringComparison.Ordinal)))
        {
            switch (nuclidTypes[0])
            {
                case "а": result["alpha"] = activity; break;
                case "б": result["beta"] = activity; break;
                case "т": result["tritium"] = activity; break;
                case "у": result["transuranium"] = activity; break;
            }
        }
        return result;
    }

    private sealed class Operation41PairingDto
    {
        public int Id { get; init; }
        public int ReportId { get; init; }
        public string OpCode { get; init; } = string.Empty;
        public string OpDate { get; init; } = string.Empty;
        public string PasNum { get; init; } = string.Empty;
        public string FacNum { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Radionuclids { get; init; } = string.Empty;
        public string CreationDate { get; init; } = string.Empty;
        public byte? DocumentVid { get; init; }
        public string DocumentNumber { get; init; } = string.Empty;
        public string DocumentDate { get; init; } = string.Empty;
        public string ProviderOrRecieverOkpo { get; init; } = string.Empty;
        public string TransporterOkpo { get; init; } = string.Empty;
        public string PackNumber { get; init; } = string.Empty;
        public string PackName { get; init; } = string.Empty;
        public string PackType { get; init; } = string.Empty;
        public string Activity { get; init; } = string.Empty;
        public string MainRadionuclids { get; init; } = string.Empty;
        public string TritiumActivity { get; init; } = string.Empty;
        public string BetaGammaActivity { get; init; } = string.Empty;
        public string AlphaActivity { get; init; } = string.Empty;
        public string TransuraniumActivity { get; init; } = string.Empty;
        public string Mass { get; init; } = string.Empty;
        public string Volume { get; init; } = string.Empty;
        public string ActivityMeasurementDate { get; init; } = string.Empty;
        public int? Quantity { get; init; }
    }
}
