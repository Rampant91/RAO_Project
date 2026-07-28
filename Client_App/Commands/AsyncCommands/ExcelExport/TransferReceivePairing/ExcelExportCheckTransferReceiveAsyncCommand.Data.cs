using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources.CustomComparers.SnkComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.Comparers.FormContent;
using Models.DBRealization;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Normalization / keys

    private static bool SerialNumbersAreEmpty(TransferReceiveDto item) =>
        Operation41PairingKeyComparer.SerialNumbersIsEmpty(item.PasNum, item.FacNum);

    private static string NormalizeSerialNumber(string? value) =>
        Operation41PairingKeyComparer.IsEmptySerial(value) ? string.Empty : NormalizeNumber(value);

    private static string NormalizeNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "-")
        {
            return string.Empty;
        }

        var normalized = Regex.Replace(value.ToLowerInvariant(), @"[\\/:*?""<>|.,_\-;:\s+]", string.Empty)
            .TrimStart('0');
        return LookalikeCharMapper.ReplaceRuEnLookalikes(normalized, includeExtendedSnkSet: true);
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
            .Select(x => LookalikeCharMapper.ReplaceRuEnLookalikes(x, includeExtendedSnkSet: true))
            .OrderBy(x => x);

        return string.Join("|", normalizedSet);
    }

    private static string NormalizeDate(string? value) =>
        DateOnly.TryParse(value, out var date)
            ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            : NormalizeNumber(value);

    private static bool DateWithinTolerance(string? left, string? right, int days = OperationDateToleranceDays)
    {
        if (DateOnly.TryParse(left, out var leftDate) && DateOnly.TryParse(right, out var rightDate))
        {
            return Math.Abs(leftDate.DayNumber - rightDate.DayNumber) <= days;
        }

        return string.Equals(NormalizeDate(left), NormalizeDate(right), StringComparison.Ordinal);
    }

    /// <summary>Точное совпадение даты операции (для пары и подсветки; ±N дней — только фильтр поиска).</summary>
    private static bool DatesEqualExact(string? left, string? right)
    {
        if (DateOnly.TryParse(left, out var leftDate) && DateOnly.TryParse(right, out var rightDate))
        {
            return leftDate == rightDate;
        }

        return string.Equals(NormalizeDate(left), NormalizeDate(right), StringComparison.Ordinal);
    }

    private static int GetQuantityForComparison(TransferReceiveDto row) =>
        row.Quantity is > 0 ? row.Quantity.Value : 1;

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

    private static bool ActivityMatches(TransferReceiveDto left, TransferReceiveDto right, bool checkActivity)
    {
        if (!checkActivity)
        {
            return true;
        }

        if (!TryParseActivity(left.Activity, out var leftActivity) || !TryParseActivity(right.Activity, out var rightActivity))
        {
            return string.Equals(
                NormalizeNumber(left.Activity),
                NormalizeNumber(right.Activity),
                StringComparison.Ordinal);
        }

        var scale = Math.Max(Math.Abs(leftActivity), Math.Abs(rightActivity));
        if (scale <= double.Epsilon)
        {
            return true;
        }

        return Math.Abs(leftActivity - rightActivity) <= scale * 0.10;
    }

    /// <summary>
    /// Ключ учётной единицы с учётом включённых параметров.
    /// ОКПО поставщика/получателя в ключ не входит (сравнивается отдельно как обратная ссылка).
    /// Активность — отдельно с допуском ±10%.
    /// </summary>
    private static string BuildPairingKey(
        TransferReceiveDto row,
        TransferReceive11Params options,
        bool includeSerial,
        bool includeQuantity)
    {
        var parts = new List<string>(10);
        if (includeSerial && options.CheckPassportNumber) parts.Add(NormalizeSerialNumber(row.PasNum));
        if (options.CheckType) parts.Add(NormalizeNumber(row.Type));
        if (options.CheckRadionuclids) parts.Add(NormalizeRads(row.Radionuclids));
        if (includeSerial && options.CheckFactoryNumber) parts.Add(NormalizeSerialNumber(row.FacNum));
        if (options.CheckPackNumber) parts.Add(NormalizeNumber(row.PackNumber));
        if (options.CheckCreatorOkpo) parts.Add(NormalizeNumber(row.CreatorOkpo));
        if (options.CheckCreationDate) parts.Add(NormalizeDate(row.CreationDate));
        if (includeQuantity && options.CheckQuantity)
        {
            parts.Add(GetQuantityForComparison(row).ToString(CultureInfo.InvariantCulture));
        }

        if (options.CheckAggregateState)
        {
            parts.Add(row.AggregateState?.ToString(CultureInfo.InvariantCulture) ?? string.Empty);
        }

        return string.Join('|', parts);
    }

    private static bool IsPairMatch(
        TransferReceiveDto source,
        TransferReceiveDto candidate,
        TransferReceive11Params options,
        string ourOkpoNorm,
        bool includeSerial)
    {
        if (options.CheckOperationCode && !OpCodesArePaired(source.OpCode, candidate.OpCode))
        {
            return false;
        }

        if (options.CheckOperationDate && !DatesEqualExact(source.OpDate, candidate.OpDate))
        {
            return false;
        }

        var includeQuantity = includeSerial && options.CheckQuantity;
        var sourceKey = BuildPairingKey(source, options, includeSerial, includeQuantity);
        var candidateKey = BuildPairingKey(candidate, options, includeSerial, includeQuantity);
        if (!string.Equals(sourceKey, candidateKey, StringComparison.Ordinal))
        {
            return false;
        }

        if (!ActivityMatches(source, candidate, options.CheckActivity))
        {
            return false;
        }

        if (options.CheckProviderOrRecieverOkpo)
        {
            var candidatePointsToUs = NormalizeNumber(candidate.ProviderOrRecieverOkpo) == ourOkpoNorm
                                      || NormalizeNumber(candidate.ProviderOrRecieverOkpo)
                                      == NormalizeNumber(source.OrgOkpo);
            if (!candidatePointsToUs)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Load DTO

    /// <summary>Коды передачи/приёма форм 1.1 и 1.3.</summary>
    private static readonly string[] TransferReceiveCodesForm11And13 =
    [
        "21", "22", "25", "27", "28", "29",
        "31", "32", "35", "37", "38", "39"
    ];

    /// <summary>Алиас для загрузчиков формы 1.1.</summary>
    private static readonly string[] Form11TransferReceiveCodes = TransferReceiveCodesForm11And13;

    private static async Task<List<TransferReceiveDto>> LoadForm11TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        // Запрос через form_11 (без SelectMany по коллекциям) — Firebird не поддерживает APPLY.
        var codes = Form11TransferReceiveCodes;
        var rows = await db.form_11
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                Quantity = form.Quantity_DB,
                Activity = form.Activity_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                StartPeriod = form.Report!.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = repsId,
                ReportId = row.ReportId,
                NumberInOrder = row.NumberInOrder,
                OrgOkpo = orgOkpo,
                OpCode = row.OpCode ?? string.Empty,
                OpDate = row.OpDate ?? string.Empty,
                PasNum = row.PasNum ?? string.Empty,
                FacNum = row.FacNum ?? string.Empty,
                Type = row.Type ?? string.Empty,
                Radionuclids = row.Radionuclids ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = row.Quantity,
                Activity = row.Activity ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            })
            .Where(row => IsTransferOrReceiveCodeForm11(row.OpCode))
            .OrderBy(row => row.Id)
            .ToList();
    }

    /// <summary>
    /// Операции приёма/передачи формы 1.1 для набора организаций (без повторной загрузки «своей» org —
    /// её ops передаются отдельно и мержатся в пул в оркестрации).
    /// </summary>
    private static async Task<List<TransferReceiveDto>> LoadForm11TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null)
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, "загрузка операций контрагентов: нет организаций");
            return [];
        }

        var codes = Form11TransferReceiveCodes;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций контрагентов: 0 из {totalOrgs} орг.");

        // Пакеты меньше Firebird IN-лимита — иначе при ~100 org один запрос и прогресс «0 из N» до конца.
        foreach (var idChunk in ChunkIds(repsIds, CounterpartOpsLoadChunkSize))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций контрагентов: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var rows = await db.form_11
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .Select(form => new
                {
                    form.Id,
                    ReportId = form.ReportId ?? 0,
                    RepsId = form.Report!.Reports.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    FacNum = form.FactoryNumber_DB,
                    Type = form.Type_DB,
                    Radionuclids = form.Radionuclids_DB,
                    PackNumber = form.PackNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    Quantity = form.Quantity_DB,
                    Activity = form.Activity_DB,
                    CreatorOkpo = form.CreatorOKPO_DB,
                    CreationDate = form.CreationDate_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                orgTitlesByRepsId.TryGetValue(row.RepsId, out var title);
                result.Add(new TransferReceiveDto
                {
                    Id = row.Id,
                    RepsId = row.RepsId,
                    ReportId = row.ReportId,
                    NumberInOrder = row.NumberInOrder,
                    OrgOkpo = title?.Okpo ?? string.Empty,
                    OrgRegNo = title?.RegNo ?? string.Empty,
                    OrgShortName = title?.ShortName ?? string.Empty,
                    OpCode = row.OpCode ?? string.Empty,
                    OpDate = row.OpDate ?? string.Empty,
                    PasNum = row.PasNum ?? string.Empty,
                    FacNum = row.FacNum ?? string.Empty,
                    Type = row.Type ?? string.Empty,
                    Radionuclids = row.Radionuclids ?? string.Empty,
                    PackNumber = row.PackNumber ?? string.Empty,
                    ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                    Quantity = row.Quantity,
                    Activity = row.Activity ?? string.Empty,
                    CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                    CreationDate = row.CreationDate ?? string.Empty,
                    StartPeriod = row.StartPeriod ?? string.Empty,
                    EndPeriod = row.EndPeriod ?? string.Empty,
                    IsTransfer = IsTransferCodeForm11(row.OpCode)
                });
            }

            orgsDone += idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций контрагентов: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm13TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveCodesForm11And13;
        var rows = await db.form_13
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB))
            .Select(form => new
            {
                form.Id,
                ReportId = form.ReportId ?? 0,
                NumberInOrder = form.NumberInOrder_DB,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                Activity = form.Activity_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                AggregateState = form.AggregateState_DB,
                StartPeriod = form.Report!.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = repsId,
                ReportId = row.ReportId,
                NumberInOrder = row.NumberInOrder,
                OrgOkpo = orgOkpo,
                OpCode = row.OpCode ?? string.Empty,
                OpDate = row.OpDate ?? string.Empty,
                PasNum = row.PasNum ?? string.Empty,
                FacNum = row.FacNum ?? string.Empty,
                Type = row.Type ?? string.Empty,
                Radionuclids = row.Radionuclids ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = 1,
                AggregateState = row.AggregateState,
                Activity = row.Activity ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            })
            .Where(row => IsTransferOrReceiveCodeForm11(row.OpCode))
            .OrderBy(row => row.Id)
            .ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm13TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null)
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, "загрузка операций 1.3 контрагентов: нет организаций");
            return [];
        }

        var codes = TransferReceiveCodesForm11And13;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций 1.3 контрагентов: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, CounterpartOpsLoadChunkSize))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций 1.3 контрагентов: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var rows = await db.form_13
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .Select(form => new
                {
                    form.Id,
                    ReportId = form.ReportId ?? 0,
                    RepsId = form.Report!.Reports.Id,
                    NumberInOrder = form.NumberInOrder_DB,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    FacNum = form.FactoryNumber_DB,
                    Type = form.Type_DB,
                    Radionuclids = form.Radionuclids_DB,
                    PackNumber = form.PackNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    Activity = form.Activity_DB,
                    CreatorOkpo = form.CreatorOKPO_DB,
                    CreationDate = form.CreationDate_DB,
                    AggregateState = form.AggregateState_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                orgTitlesByRepsId.TryGetValue(row.RepsId, out var title);
                result.Add(new TransferReceiveDto
                {
                    Id = row.Id,
                    RepsId = row.RepsId,
                    ReportId = row.ReportId,
                    NumberInOrder = row.NumberInOrder,
                    OrgOkpo = title?.Okpo ?? string.Empty,
                    OrgRegNo = title?.RegNo ?? string.Empty,
                    OrgShortName = title?.ShortName ?? string.Empty,
                    OpCode = row.OpCode ?? string.Empty,
                    OpDate = row.OpDate ?? string.Empty,
                    PasNum = row.PasNum ?? string.Empty,
                    FacNum = row.FacNum ?? string.Empty,
                    Type = row.Type ?? string.Empty,
                    Radionuclids = row.Radionuclids ?? string.Empty,
                    PackNumber = row.PackNumber ?? string.Empty,
                    ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                    Quantity = 1,
                    AggregateState = row.AggregateState,
                    Activity = row.Activity ?? string.Empty,
                    CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                    CreationDate = row.CreationDate ?? string.Empty,
                    StartPeriod = row.StartPeriod ?? string.Empty,
                    EndPeriod = row.EndPeriod ?? string.Empty,
                    IsTransfer = IsTransferCodeForm11(row.OpCode)
                });
            }

            orgsDone += idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций 1.3 контрагентов: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>
    /// Карта нормализованный ОКПО → Id организаций + титулы (рег/ОКПО/имя).
    /// 1) быстрый exact match по сырым ОКПО;
    /// 2) при «хвосте» — один лёгкий проход по form_10 (без N пакетов и без OkpoRep).
    /// </summary>
    private static async Task<(Dictionary<string, List<int>> ByNormOkpo, Dictionary<int, OrgTitleInfo> OrgTitles)>
        LoadRepsIdsByOkpoAsync(
            DBModel db,
            IReadOnlyList<string> rawOkpos,
            CancellationToken cancellationToken,
            ProgressReporter? progress = null)
    {
        var byNorm = new Dictionary<string, List<int>>(StringComparer.Ordinal);
        var orgTitles = new Dictionary<int, OrgTitleInfo>();
        if (rawOkpos.Count == 0)
        {
            return (byNorm, orgTitles);
        }

        var neededNorms = rawOkpos
            .Select(NormalizeNumber)
            .Where(norm => norm.Length > 0)
            .ToHashSet(StringComparer.Ordinal);

        if (neededNorms.Count == 0)
        {
            return (byNorm, orgTitles);
        }

        // Фаза 1: точное совпадение Okpo_DB IN (...)
        var exactRaw = rawOkpos
            .Where(okpo => okpo.Length > 0)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var exactMasterIds = new HashSet<int>();
        var exactTitleHits = new List<(int MasterReportId, string? Okpo)>();
        var okpoDone = 0;
        var okpoTotal = exactRaw.Count;

        foreach (var okpoChunk in ChunkStrings(exactRaw))
        {
            var hits = await db.form_10
                .AsNoTracking()
                .Where(f => f.ReportId != null
                            && f.Okpo_DB != null
                            && okpoChunk.Contains(f.Okpo_DB))
                .Select(f => new
                {
                    MasterReportId = f.ReportId!.Value,
                    f.Okpo_DB
                })
                .ToListAsync(cancellationToken);

            foreach (var hit in hits)
            {
                exactMasterIds.Add(hit.MasterReportId);
                exactTitleHits.Add((hit.MasterReportId, hit.Okpo_DB));
            }

            okpoDone += okpoChunk.Count;
            progress?.Report(
                okpoDone,
                okpoTotal,
                $"поиск контрагентов по ОКПО: {okpoDone} из {okpoTotal}");
        }

        if (exactMasterIds.Count > 0)
        {
            var masterToRepsExact = await LoadMasterReportIdToRepsIdMapAsync(
                db, exactMasterIds.ToList(), formNum: "1.0", cancellationToken);

            foreach (var (masterReportId, okpo) in exactTitleHits)
            {
                if (!masterToRepsExact.TryGetValue(masterReportId, out var repsId))
                {
                    continue;
                }

                AddOkpoHit(okpo, repsId, neededNorms, byNorm);
            }
        }

        var missingNorms = neededNorms
            .Where(norm => !byNorm.ContainsKey(norm))
            .ToHashSet(StringComparer.Ordinal);

        // Фаза 2 (только если есть ненайденные после нормализации): один проход form_10 + карта Master→Reps.
        Dictionary<int, int> masterToRepsId;
        List<(int MasterReportId, int NumberInOrder, string RegNo, string Okpo, string ShortName)> titleRows;

        if (missingNorms.Count > 0)
        {
            progress?.Status(
                $"догрузка титулов по нормализации ОКПО (осталось {missingNorms.Count} из {neededNorms.Count})…");

            var orgRows = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.DBObservableId != null
                               && reps.Master_DB != null
                               && reps.Master_DB.FormNum_DB == "1.0")
                .Select(reps => new { reps.Id, MasterReportId = reps.Master_DBId })
                .Where(x => x.MasterReportId != null)
                .ToListAsync(cancellationToken);

            masterToRepsId = new Dictionary<int, int>(orgRows.Count);
            foreach (var org in orgRows)
            {
                masterToRepsId[org.MasterReportId!.Value] = org.Id;
            }

            progress?.Status("чтение form_10 для сопоставления ОКПО…");

            // Один запрос вместо пакетов по всем MasterId.
            titleRows = (await db.form_10
                    .AsNoTracking()
                    .Where(f => f.ReportId != null)
                    .Select(f => new
                    {
                        MasterReportId = f.ReportId!.Value,
                        f.NumberInOrder_DB,
                        f.RegNo_DB,
                        f.Okpo_DB,
                        f.ShortJurLico_DB
                    })
                    .ToListAsync(cancellationToken))
                .Select(f => (
                    f.MasterReportId,
                    f.NumberInOrder_DB,
                    f.RegNo_DB ?? string.Empty,
                    f.Okpo_DB ?? string.Empty,
                    f.ShortJurLico_DB ?? string.Empty))
                .ToList();

            foreach (var row in titleRows)
            {
                if (!masterToRepsId.TryGetValue(row.MasterReportId, out var repsId))
                {
                    continue;
                }

                AddOkpoHit(row.Okpo, repsId, missingNorms, byNorm);
            }
        }
        else
        {
            // Титулы только для найденных в фазе 1 — точечная догрузка.
            progress?.Status("загрузка титулов найденных контрагентов…");
            masterToRepsId = await LoadMasterReportIdToRepsIdMapAsync(
                db, exactMasterIds.ToList(), formNum: "1.0", cancellationToken);
            var masterIds = masterToRepsId.Keys.ToList();
            titleRows = [];
            var mastersDone = 0;
            foreach (var idChunk in ChunkIds(masterIds))
            {
                var batch = await db.form_10
                    .AsNoTracking()
                    .Where(f => f.ReportId != null && idChunk.Contains(f.ReportId.Value))
                    .Select(f => new
                    {
                        MasterReportId = f.ReportId!.Value,
                        f.NumberInOrder_DB,
                        f.RegNo_DB,
                        f.Okpo_DB,
                        f.ShortJurLico_DB
                    })
                    .ToListAsync(cancellationToken);
                titleRows.AddRange(batch.Select(f => (
                    f.MasterReportId,
                    f.NumberInOrder_DB,
                    f.RegNo_DB ?? string.Empty,
                    f.Okpo_DB ?? string.Empty,
                    f.ShortJurLico_DB ?? string.Empty)));

                mastersDone += idChunk.Count;
                progress?.Report(
                    mastersDone,
                    masterIds.Count,
                    $"загрузка титулов контрагентов: {mastersDone} из {masterIds.Count}");
            }
        }

        // Собрать представительные титулы для всех RepsId, попавших в byNorm.
        var neededRepsIds = byNorm.Values.SelectMany(ids => ids).ToHashSet();
        var titlesByMaster = titleRows
            .GroupBy(t => t.MasterReportId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.NumberInOrder).ToList());

        foreach (var (masterId, repsId) in masterToRepsId)
        {
            if (!neededRepsIds.Contains(repsId) || orgTitles.ContainsKey(repsId))
            {
                continue;
            }

            if (!titlesByMaster.TryGetValue(masterId, out var rows) || rows.Count == 0)
            {
                continue;
            }

            orgTitles[repsId] = ResolveOrgTitle(rows);
        }

        // Если фаза 1 нашла org, а titleRows для фазы «только exact» уже загружены — ok.
        // Если каких-то repsId всё ещё нет в orgTitles (фаза 1 без полных титулов) — догрузить.
        var missingTitleReps = neededRepsIds.Where(id => !orgTitles.ContainsKey(id)).ToList();
        if (missingTitleReps.Count > 0)
        {
            progress?.Status($"догрузка недостающих титулов: {missingTitleReps.Count} орг.…");
            await LoadOrgTitlesForRepsIdsAsync(db, missingTitleReps, orgTitles, cancellationToken);
        }

        progress?.Report(
            neededRepsIds.Count,
            neededRepsIds.Count,
            $"контрагенты по ОКПО: найдено {neededRepsIds.Count} орг. из {neededNorms.Count} ОКПО");

        return (byNorm, orgTitles);
    }

    private static OrgTitleInfo ResolveOrgTitle(
        IReadOnlyList<(int MasterReportId, int NumberInOrder, string RegNo, string Okpo, string ShortName)> rows)
    {
        var head = rows[0];
        if (rows.Count > 1)
        {
            var branch = rows[1];
            var branchOkpo = branch.Okpo.Trim();
            var branchReg = branch.RegNo.Trim();
            if ((branchReg.Length > 0 || branch.Okpo == "-") && branchOkpo.Length > 0)
            {
                return new OrgTitleInfo(
                    branchReg,
                    branchOkpo,
                    branch.ShortName.Trim());
            }
        }

        return new OrgTitleInfo(
            head.RegNo.Trim(),
            head.Okpo.Trim(),
            head.ShortName.Trim());
    }

    private static async Task LoadOrgTitlesForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        Dictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        foreach (var idChunk in ChunkIds(repsIds))
        {
            var batch = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => idChunk.Contains(reps.Id) && reps.Master_DBId != null)
                .Select(reps => new { reps.Id, MasterId = reps.Master_DBId!.Value })
                .ToListAsync(cancellationToken);

            var masterIds = batch.Select(b => b.MasterId).Distinct().ToList();
            if (masterIds.Count == 0)
            {
                continue;
            }

            var titles = await db.form_10
                .AsNoTracking()
                .Where(f => f.ReportId != null && masterIds.Contains(f.ReportId.Value))
                .Select(f => new
                {
                    MasterId = f.ReportId!.Value,
                    f.NumberInOrder_DB,
                    f.RegNo_DB,
                    f.Okpo_DB,
                    f.ShortJurLico_DB
                })
                .ToListAsync(cancellationToken);

            var byMaster = titles
                .GroupBy(t => t.MasterId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(x => x.NumberInOrder_DB)
                        .Select(x => (
                            x.MasterId,
                            x.NumberInOrder_DB,
                            x.RegNo_DB ?? string.Empty,
                            x.Okpo_DB ?? string.Empty,
                            x.ShortJurLico_DB ?? string.Empty))
                        .ToList());

            foreach (var org in batch)
            {
                if (orgTitles.ContainsKey(org.Id))
                {
                    continue;
                }

                if (!byMaster.TryGetValue(org.MasterId, out var rows) || rows.Count == 0)
                {
                    continue;
                }

                orgTitles[org.Id] = ResolveOrgTitle(rows);
            }
        }
    }

    private static async Task<Dictionary<int, int>> LoadMasterReportIdToRepsIdMapAsync(
        DBModel db,
        IReadOnlyList<int> masterReportIds,
        string formNum,
        CancellationToken cancellationToken)
    {
        var map = new Dictionary<int, int>();
        if (masterReportIds.Count == 0)
        {
            return map;
        }

        foreach (var idChunk in ChunkIds(masterReportIds))
        {
            var batch = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.DBObservableId != null
                               && reps.Master_DBId != null
                               && reps.Master_DB != null
                               && reps.Master_DB.FormNum_DB == formNum
                               && idChunk.Contains(reps.Master_DBId.Value))
                .Select(reps => new { MasterReportId = reps.Master_DBId!.Value, reps.Id })
                .ToListAsync(cancellationToken);

            foreach (var row in batch)
            {
                map[row.MasterReportId] = row.Id;
            }
        }

        return map;
    }

    private static void AddOkpoHit(
        string? okpo,
        int repsId,
        HashSet<string> neededNorms,
        Dictionary<string, List<int>> byNorm)
    {
        var raw = string.IsNullOrWhiteSpace(okpo) ? string.Empty : okpo.Trim();
        if (raw.Length == 0 || raw == "-")
        {
            return;
        }

        var norm = NormalizeNumber(raw);
        if (norm.Length == 0 || !neededNorms.Contains(norm))
        {
            return;
        }

        if (!byNorm.TryGetValue(norm, out var list))
        {
            list = [];
            byNorm[norm] = list;
        }

        if (!list.Contains(repsId))
        {
            list.Add(repsId);
        }
    }

    private static IEnumerable<List<string>> ChunkStrings(IReadOnlyList<string> values)
    {
        for (var offset = 0; offset < values.Count; offset += FirebirdInListMaxCount)
        {
            yield return values.Skip(offset).Take(FirebirdInListMaxCount).ToList();
        }
    }

    /// <summary>Титул организации для Excel и индексации по ОКПО.</summary>
    private sealed record OrgTitleInfo(string RegNo, string Okpo, string ShortName);

    #endregion

    #region Build reports for Excel export

    private static async Task<Reports> BuildReportsForExportAsync(
        DBModel db,
        Reports masterReports,
        List<TransferReceiveDto> unpairedOperations,
        CancellationToken cancellationToken)
    {
        var result = new Reports { Master = masterReports.Master };
        if (unpairedOperations.Count == 0)
        {
            return result;
        }

        var formIds = unpairedOperations.Select(op => op.Id).Distinct().ToList();
        var reportIds = unpairedOperations
            .Where(op => op.ReportId != 0)
            .Select(op => op.ReportId)
            .Distinct()
            .ToList();

        if (reportIds.Count == 0)
        {
            return result;
        }

        var reports = await LoadReportsByIdsAsync(db, reportIds, cancellationToken);
        var forms = await LoadForm11ByIdsAsync(db, formIds, cancellationToken);
        AttachFormsToReports(reports, forms);

        foreach (var report in OrderReportsForExport(reports))
        {
            if (report.Rows11.Count > 0)
            {
                result.Report_Collection.Add(report);
            }
        }

        return result;
    }

    private static void AttachFormsToReports(List<Report> reports, List<Form11> forms)
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

            report.Rows11.Clear();
            foreach (var form in rowList)
            {
                report.Rows11.Add(form);
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
        DBModel db,
        IReadOnlyList<int> formIds,
        CancellationToken cancellationToken)
    {
        var forms = new List<Form11>();
        foreach (var idChunk in ChunkIds(formIds))
        {
            var batch = await db.form_11
                .AsNoTracking()
                .Where(form => idChunk.Contains(form.Id))
                .ToListAsync(cancellationToken);
            forms.AddRange(batch);
        }

        return forms;
    }

    private static IEnumerable<List<int>> ChunkIds(IReadOnlyList<int> ids, int chunkSize = FirebirdInListMaxCount)
    {
        var size = chunkSize > 0 ? chunkSize : FirebirdInListMaxCount;
        for (var offset = 0; offset < ids.Count; offset += size)
        {
            yield return ids.Skip(offset).Take(size).ToList();
        }
    }

    private static IEnumerable<Report> OrderReportsForExport(List<Report> reports) =>
        reports
            .OrderBy(rep => DateOnly.TryParse(rep.StartPeriod_DB, out var startDate) ? startDate : DateOnly.MaxValue)
            .ThenBy(rep => DateOnly.TryParse(rep.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue);

    #endregion

    #region DTO

    /// <summary>Узкий DTO операции приёма/передачи для сопоставления и выгрузки.</summary>
    private sealed class TransferReceiveDto
    {
        public int Id { get; init; }
        public int RepsId { get; init; }
        public int ReportId { get; init; }
        public int NumberInOrder { get; init; }
        public string OrgOkpo { get; set; } = string.Empty;
        public string OrgRegNo { get; set; } = string.Empty;
        public string OrgShortName { get; set; } = string.Empty;
        public string StartPeriod { get; set; } = string.Empty;
        public string EndPeriod { get; set; } = string.Empty;
        public string OpCode { get; init; } = string.Empty;
        public string OpDate { get; init; } = string.Empty;
        public string PasNum { get; init; } = string.Empty;
        public string FacNum { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Radionuclids { get; init; } = string.Empty;
        public string PackNumber { get; init; } = string.Empty;
        public string ProviderOrRecieverOkpo { get; init; } = string.Empty;
        public string Activity { get; init; } = string.Empty;
        public string CreatorOkpo { get; init; } = string.Empty;
        public string CreationDate { get; init; } = string.Empty;
        public int? Quantity { get; init; }
        public byte? AggregateState { get; set; }
        public bool IsTransfer { get; init; }
    }

    #endregion
}
