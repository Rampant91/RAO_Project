using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources.CustomComparers.SnkComparers;
using Microsoft.EntityFrameworkCore;
using Models.Comparers.FormContent;
using Models.DBRealization;

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

        return NumericWithinRelativeTolerance(left.Activity, right.Activity, relativeTolerance: 0.10);
    }

    /// <summary>Масса (кг): для пары — тот же допуск ±10%, что и у активности.</summary>
    private static bool MassMatches(TransferReceiveDto left, TransferReceiveDto right, bool checkMass)
    {
        if (!checkMass)
        {
            return true;
        }

        return NumericWithinRelativeTolerance(left.Mass, right.Mass, relativeTolerance: 0.10);
    }

    private static bool NumericWithinRelativeTolerance(string? leftRaw, string? rightRaw, double relativeTolerance)
    {
        if (!TryParseActivity(leftRaw, out var leftValue) || !TryParseActivity(rightRaw, out var rightValue))
        {
            return string.Equals(
                NormalizeNumber(leftRaw),
                NormalizeNumber(rightRaw),
                StringComparison.Ordinal);
        }

        var scale = Math.Max(Math.Abs(leftValue), Math.Abs(rightValue));
        if (scale <= double.Epsilon)
        {
            return true;
        }

        return Math.Abs(leftValue - rightValue) <= scale * relativeTolerance;
    }

    /// <summary>
    /// Ключ учётной единицы с учётом включённых параметров.
    /// ОКПО поставщика/получателя в ключ не входит (сравнивается отдельно как обратная ссылка).
    /// Активность — отдельно с допуском ±10%.
    /// </summary>
    private static string BuildPairingKey(
        TransferReceiveDto row,
        TransferReceiveFormParams options,
        bool includeSerial,
        bool includeQuantity)
    {
        var parts = new List<string>(10);
        if (includeSerial && options.CheckPassportNumber) parts.Add(NormalizeSerialNumber(row.PasNum));
        if (options.CheckType) parts.Add(NormalizeNumber(row.Type));
        if (options.CheckRadionuclids) parts.Add(NormalizeRads(row.Radionuclids));
        if (includeSerial && options.CheckFactoryNumber) parts.Add(NormalizeSerialNumber(row.FacNum));
        if (options.CheckPackType) parts.Add(NormalizeNumber(row.PackType));
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
        TransferReceiveFormParams options,
        string ourOkpoNorm,
        bool includeSerial,
        string? precomputedSourceKey = null,
        string? precomputedCandidateKey = null,
        string? precomputedSourceOrgOkpoNorm = null)
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
        var sourceKey = precomputedSourceKey
                        ?? BuildPairingKey(source, options, includeSerial, includeQuantity);
        var candidateKey = precomputedCandidateKey
                           ?? BuildPairingKey(candidate, options, includeSerial, includeQuantity);
        if (!string.Equals(sourceKey, candidateKey, StringComparison.Ordinal))
        {
            return false;
        }

        if (!ActivityMatches(source, candidate, options.CheckActivity))
        {
            return false;
        }

        if (!MassMatches(source, candidate, options.CheckMass))
        {
            return false;
        }

        if (options.CheckProviderOrRecieverOkpo)
        {
            var sourceOrgOkpo = precomputedSourceOrgOkpoNorm ?? NormalizeNumber(source.OrgOkpo);
            var candidateProvider = NormalizeNumber(candidate.ProviderOrRecieverOkpo);
            var candidatePointsToUs = candidateProvider == ourOkpoNorm
                                      || candidateProvider == sourceOrgOkpo;
            if (!candidatePointsToUs)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Load DTO

    /// <summary>
    /// Коды передачи/приёма форм 1.1–1.3 (и задел под те же коды на 1.4).
    /// Без 26/36 — эта пара ожидается только на формах 1.5–1.8.
    /// </summary>
    private static readonly string[] TransferReceiveOpCodes =
    [
        "21", "22", "25", "27", "28", "29",
        "31", "32", "35", "37", "38", "39"
    ];

    /// <summary>Диспетчер bulk-загрузки по форме.</summary>
    private static Task<List<TransferReceiveDto>> LoadAllFormOpsAsync(
        TransferReceiveFormId formId,
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken) =>
        formId switch
        {
            TransferReceiveFormId.Form11 => LoadAllForm11TransferReceiveAsync(db, orgTitles, cancellationToken),
            TransferReceiveFormId.Form12 => LoadAllForm12TransferReceiveAsync(db, orgTitles, cancellationToken),
            TransferReceiveFormId.Form13 => LoadAllForm13TransferReceiveAsync(db, orgTitles, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет bulk-загрузчика для формы.")
        };

    /// <summary>Диспетчер точечной загрузки своих ops.</summary>
    private static Task<List<TransferReceiveDto>> LoadFormOpsForRepsAsync(
        TransferReceiveFormId formId,
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken) =>
        formId switch
        {
            TransferReceiveFormId.Form11 => LoadForm11TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            TransferReceiveFormId.Form12 => LoadForm12TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            TransferReceiveFormId.Form13 => LoadForm13TransferReceiveForRepsAsync(db, repsId, orgOkpo, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет загрузчика для формы.")
        };

    /// <summary>Диспетчер загрузки ops контрагентов.</summary>
    /// <param name="providerOkpoRawVariants">
    /// Если задан — в SQL оставляем только строки, где кол.19 ∈ variants
    /// (org-режим при включённом CheckProviderOrRecieverOkpo). Не использовать при выключенной проверке ОКПО.
    /// </param>
    /// <param name="ourOkpoNormFilter">
    /// Доп. отсев в памяти по <see cref="NormalizeNumber"/> после SQL (ведущие нули / мусор в кол.19).
    /// </param>
    private static Task<List<TransferReceiveDto>> LoadFormOpsForRepsIdsAsync(
        TransferReceiveFormId formId,
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        IReadOnlyList<string>? providerOkpoRawVariants = null,
        string? ourOkpoNormFilter = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "контрагентов") =>
        formId switch
        {
            TransferReceiveFormId.Form11 => LoadForm11TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, providerOkpoRawVariants, ourOkpoNormFilter,
                chunkSize, progressEntityLabel),
            TransferReceiveFormId.Form12 => LoadForm12TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, providerOkpoRawVariants, ourOkpoNormFilter,
                chunkSize, progressEntityLabel),
            TransferReceiveFormId.Form13 => LoadForm13TransferReceiveForRepsIdsAsync(
                db, repsIds, orgTitles, cancellationToken, progress, providerOkpoRawVariants, ourOkpoNormFilter,
                chunkSize, progressEntityLabel),
            _ => throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет загрузчика для формы.")
        };

    /// <summary>
    /// Whole-DB: один проход по строкам формы keyset-пагинацией (Id &gt; lastId Take N).
    /// Титулы подставляются позже; прогресс — после каждой страницы.
    /// </summary>
    private static Task<List<TransferReceiveDto>> LoadFormOpsPagedWholeDbAsync(
        TransferReceiveFormId formId,
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum) =>
        formId switch
        {
            TransferReceiveFormId.Form11 => LoadForm11TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            TransferReceiveFormId.Form12 => LoadForm12TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            TransferReceiveFormId.Form13 => LoadForm13TransferReceivePagedWholeDbAsync(
                db, cancellationToken, progress, formNum),
            _ => throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет paged-загрузчика для формы.")
        };

    private static async Task<List<TransferReceiveDto>> LoadForm11TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodes;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 2000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_11
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
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

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(new TransferReceiveDto
                {
                    Id = row.Id,
                    RepsId = row.RepsId,
                    ReportId = row.ReportId,
                    NumberInOrder = row.NumberInOrder,
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

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    private static async Task<List<TransferReceiveDto>> LoadForm12TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodes;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 2000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_12
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
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
                    Name = form.NameIOU_DB,
                    Mass = form.Mass_DB,
                    PackType = form.PackType_DB,
                    PackNumber = form.PackNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                    CreatorOkpo = form.CreatorOKPO_DB,
                    CreationDate = form.CreationDate_DB,
                    StartPeriod = form.Report.StartPeriod_DB,
                    EndPeriod = form.Report.EndPeriod_DB
                })
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(new TransferReceiveDto
                {
                    Id = row.Id,
                    RepsId = row.RepsId,
                    ReportId = row.ReportId,
                    NumberInOrder = row.NumberInOrder,
                    OpCode = row.OpCode ?? string.Empty,
                    OpDate = row.OpDate ?? string.Empty,
                    PasNum = row.PasNum ?? string.Empty,
                    FacNum = row.FacNum ?? string.Empty,
                    Type = row.Name ?? string.Empty,
                    PackType = row.PackType ?? string.Empty,
                    PackNumber = row.PackNumber ?? string.Empty,
                    ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                    Quantity = 1,
                    Mass = row.Mass ?? string.Empty,
                    CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                    CreationDate = row.CreationDate ?? string.Empty,
                    StartPeriod = row.StartPeriod ?? string.Empty,
                    EndPeriod = row.EndPeriod ?? string.Empty,
                    IsTransfer = IsTransferCodeForm11(row.OpCode)
                });
            }

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    private static async Task<List<TransferReceiveDto>> LoadForm13TransferReceivePagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        string formNum)
    {
        var codes = TransferReceiveOpCodes;
        var pageSize = WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 2000;
        var result = new List<TransferReceiveDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_13
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB))
                .OrderBy(form => form.Id)
                .Take(pageSize)
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

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                if (!IsTransferOrReceiveCodeForm11(row.OpCode))
                {
                    continue;
                }

                result.Add(new TransferReceiveDto
                {
                    Id = row.Id,
                    RepsId = row.RepsId,
                    ReportId = row.ReportId,
                    NumberInOrder = row.NumberInOrder,
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

            progress?.ReportNow(
                result.Count,
                rows.Count < pageSize ? result.Count : result.Count + pageSize,
                $"форма {formNum}: загружено {result.Count} строк (пакет {page})");

            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Сырые варианты нашего ОКПО для SQL IN: как в титуле, нормализованный, с ведущими нулями.
    /// Не покрывает lookalike-буквы — для ОКПО обычно цифры; пары после NormalizeNumber всё равно
    /// дополнительно отсекаются <c>ourOkpoNormFilter</c> в памяти.
    /// </summary>
    private static List<string> BuildOurOkpoSqlMatchVariants(string ourOkpoRaw)
    {
        var result = new HashSet<string>(StringComparer.Ordinal);
        void Add(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "-")
            {
                return;
            }

            result.Add(value.Trim());
        }

        Add(ourOkpoRaw);
        var norm = NormalizeNumber(ourOkpoRaw);
        Add(norm);
        if (norm.Length is > 0 and <= 12)
        {
            for (var pad = 1; pad <= 4; pad++)
            {
                result.Add(norm.PadLeft(norm.Length + pad, '0'));
            }
        }

        return result.ToList();
    }

    private static bool CounterpartProviderPointsToUs(string? providerRaw, string ourOkpoNorm) =>
        ourOkpoNorm.Length > 0 && NormalizeNumber(providerRaw) == ourOkpoNorm;

    /// <summary>
    /// Один запрос: все операции приёма/передачи формы 1.1 по БД (RepsId из навигации).
    /// </summary>
    private static async Task<List<TransferReceiveDto>> LoadAllForm11TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_11
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
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

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
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

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>
    /// Один запрос: все операции приёма/передачи формы 1.2 по БД.
    /// NameIOU → Type, Mass → Mass, PackType → PackType; количество всегда 1.
    /// </summary>
    private static async Task<List<TransferReceiveDto>> LoadAllForm12TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_12
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
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
                Name = form.NameIOU_DB,
                Mass = form.Mass_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
                CreatorOkpo = form.CreatorOKPO_DB,
                CreationDate = form.CreationDate_DB,
                StartPeriod = form.Report.StartPeriod_DB,
                EndPeriod = form.Report.EndPeriod_DB
            })
            .ToListAsync(cancellationToken);

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
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
                Type = row.Name ?? string.Empty,
                PackType = row.PackType ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = 1,
                Mass = row.Mass ?? string.Empty,
                CreatorOkpo = row.CreatorOkpo ?? string.Empty,
                CreationDate = row.CreationDate ?? string.Empty,
                StartPeriod = row.StartPeriod ?? string.Empty,
                EndPeriod = row.EndPeriod ?? string.Empty,
                IsTransfer = IsTransferCodeForm11(row.OpCode)
            });
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>
    /// Один запрос: все операции приёма/передачи формы 1.3 по БД.
    /// </summary>
    private static async Task<List<TransferReceiveDto>> LoadAllForm13TransferReceiveAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_13
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
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

        var result = new List<TransferReceiveDto>(rows.Count);
        foreach (var row in rows)
        {
            if (!IsTransferOrReceiveCodeForm11(row.OpCode))
            {
                continue;
            }

            orgTitles.TryGetValue(row.RepsId, out var title);
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

        return result.OrderBy(row => row.Id).ToList();
    }

    /// <summary>
    /// Whole-DB ОКПО-карта: сначала все загруженные титулы (в т.ч. контрагенты с ops);
    /// <see cref="LoadRepsIdsByOkpoAsync"/> — только для сырых ОКПО кол. 19, которых ещё нет после нормализации.
    /// </summary>
    private static async Task<Dictionary<string, List<int>>> BuildOkpoAliasMapForWholeDbAsync(
        DBModel db,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles,
        IReadOnlyList<TransferReceiveDto> allOps,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null)
    {
        var byNorm = SeedOkpoAliasMapFromTitles(orgTitles);

        var counterpartRawOkpos = allOps
            .Select(op => op.ProviderOrRecieverOkpo?.Trim() ?? string.Empty)
            .Where(okpo => okpo.Length > 0 && okpo != "-")
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var missingRaw = counterpartRawOkpos
            .Where(raw =>
            {
                var norm = NormalizeNumber(raw);
                return norm.Length > 0 && !byNorm.ContainsKey(norm);
            })
            .ToList();

        if (missingRaw.Count == 0)
        {
            progress?.Status($"ОКПО — все {counterpartRawOkpos.Count} закрыты титулами");
            return byNorm;
        }

        progress?.Status(
            $"ОКПО — догрузка хвоста {missingRaw.Count} из {counterpartRawOkpos.Count}…");
        var (extra, _) = await LoadRepsIdsByOkpoAsync(db, missingRaw, cancellationToken, progress);
        foreach (var (norm, repsIds) in extra)
        {
            if (!byNorm.TryGetValue(norm, out var list))
            {
                byNorm[norm] = repsIds.ToList();
                continue;
            }

            foreach (var repsId in repsIds)
            {
                if (!list.Contains(repsId))
                {
                    list.Add(repsId);
                }
            }
        }

        return byNorm;
    }

    /// <summary>Нормализованный ОКПО титула → RepsId (без SQL).</summary>
    private static Dictionary<string, List<int>> SeedOkpoAliasMapFromTitles(
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles)
    {
        var byNorm = new Dictionary<string, List<int>>(StringComparer.Ordinal);
        foreach (var (repsId, title) in orgTitles)
        {
            var norm = NormalizeNumber(title.Okpo);
            if (norm.Length == 0)
            {
                continue;
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

        return byNorm;
    }

    private static async Task<List<TransferReceiveDto>> LoadForm11TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        // Запрос через form_11 (без SelectMany по коллекциям) — Firebird не поддерживает APPLY.
        var codes = TransferReceiveOpCodes;
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
        ProgressReporter? progress = null,
        IReadOnlyList<string>? providerOkpoRawVariants = null,
        string? ourOkpoNormFilter = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodes;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var filterByProvider = providerOkpoRawVariants is { Count: > 0 };
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_11
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            if (filterByProvider)
            {
                query = query.Where(form => form.ProviderOrRecieverOKPO_DB != null
                                           && providerOkpoRawVariants!.Contains(form.ProviderOrRecieverOKPO_DB));
            }

            var rows = await query
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

                if (ourOkpoNormFilter is { Length: > 0 }
                    && !CounterpartProviderPointsToUs(row.ProviderOrRecieverOkpo, ourOkpoNormFilter))
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
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm12TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
        var rows = await db.form_12
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
                Name = form.NameIOU_DB,
                Mass = form.Mass_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB,
                ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
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
                Type = row.Name ?? string.Empty,
                PackType = row.PackType ?? string.Empty,
                PackNumber = row.PackNumber ?? string.Empty,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                Quantity = 1,
                Mass = row.Mass ?? string.Empty,
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

    private static async Task<List<TransferReceiveDto>> LoadForm12TransferReceiveForRepsIdsAsync(
        DBModel db,
        IReadOnlyList<int> repsIds,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitlesByRepsId,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        IReadOnlyList<string>? providerOkpoRawVariants = null,
        string? ourOkpoNormFilter = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "1.2 контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodes;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var filterByProvider = providerOkpoRawVariants is { Count: > 0 };
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_12
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            if (filterByProvider)
            {
                query = query.Where(form => form.ProviderOrRecieverOKPO_DB != null
                                           && providerOkpoRawVariants!.Contains(form.ProviderOrRecieverOKPO_DB));
            }

            var rows = await query
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
                    Name = form.NameIOU_DB,
                    Mass = form.Mass_DB,
                    PackType = form.PackType_DB,
                    PackNumber = form.PackNumber_DB,
                    ProviderOrRecieverOkpo = form.ProviderOrRecieverOKPO_DB,
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

                if (ourOkpoNormFilter is { Length: > 0 }
                    && !CounterpartProviderPointsToUs(row.ProviderOrRecieverOkpo, ourOkpoNormFilter))
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
                    Type = row.Name ?? string.Empty,
                    PackType = row.PackType ?? string.Empty,
                    PackNumber = row.PackNumber ?? string.Empty,
                    ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo ?? string.Empty,
                    Quantity = 1,
                    Mass = row.Mass ?? string.Empty,
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
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
        }

        return result.OrderBy(row => row.Id).ToList();
    }

    private static async Task<List<TransferReceiveDto>> LoadForm13TransferReceiveForRepsAsync(
        DBModel db,
        int repsId,
        string orgOkpo,
        CancellationToken cancellationToken)
    {
        var codes = TransferReceiveOpCodes;
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
        ProgressReporter? progress = null,
        IReadOnlyList<string>? providerOkpoRawVariants = null,
        string? ourOkpoNormFilter = null,
        int chunkSize = CounterpartOpsLoadChunkSize,
        string progressEntityLabel = "1.3 контрагентов")
    {
        if (repsIds.Count == 0)
        {
            progress?.Report(0, 0, $"загрузка операций {progressEntityLabel}: нет организаций");
            return [];
        }

        var codes = TransferReceiveOpCodes;
        var result = new List<TransferReceiveDto>();
        var totalOrgs = repsIds.Count;
        var orgsDone = 0;
        var filterByProvider = providerOkpoRawVariants is { Count: > 0 };
        var effectiveChunk = chunkSize > 0 ? chunkSize : CounterpartOpsLoadChunkSize;
        progress?.ReportNow(0, totalOrgs, $"загрузка операций {progressEntityLabel}: 0 из {totalOrgs} орг.");

        foreach (var idChunk in ChunkIds(repsIds, effectiveChunk))
        {
            var chunkFrom = orgsDone + 1;
            var chunkTo = orgsDone + idChunk.Count;
            progress?.ReportNow(
                orgsDone,
                totalOrgs,
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг. (запрос {chunkFrom}–{chunkTo})…");

            var query = db.form_13
                .AsNoTracking()
                .Where(form => form.Report != null
                               && form.Report.Reports != null
                               && idChunk.Contains(form.Report.Reports.Id)
                               && form.OperationCode_DB != null
                               && codes.Contains(form.OperationCode_DB));
            if (filterByProvider)
            {
                query = query.Where(form => form.ProviderOrRecieverOKPO_DB != null
                                           && providerOkpoRawVariants!.Contains(form.ProviderOrRecieverOKPO_DB));
            }

            var rows = await query
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

                if (ourOkpoNormFilter is { Length: > 0 }
                    && !CounterpartProviderPointsToUs(row.ProviderOrRecieverOkpo, ourOkpoNormFilter))
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
                $"загрузка операций {progressEntityLabel}: {orgsDone} из {totalOrgs} орг., строк: {result.Count}");
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

    #region Chunk helpers

    private static IEnumerable<List<int>> ChunkIds(IReadOnlyList<int> ids, int chunkSize = FirebirdInListMaxCount)
    {
        var size = chunkSize > 0 ? chunkSize : FirebirdInListMaxCount;
        for (var offset = 0; offset < ids.Count; offset += size)
        {
            yield return ids.Skip(offset).Take(size).ToList();
        }
    }

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
        public string PackType { get; init; } = string.Empty;
        public string PackNumber { get; init; } = string.Empty;
        public string ProviderOrRecieverOkpo { get; init; } = string.Empty;
        public string Activity { get; init; } = string.Empty;
        /// <summary>Масса обеднённого урана, кг (форма 1.2).</summary>
        public string Mass { get; init; } = string.Empty;
        public string CreatorOkpo { get; init; } = string.Empty;
        public string CreationDate { get; init; } = string.Empty;
        public int? Quantity { get; init; }
        public byte? AggregateState { get; set; }
        public bool IsTransfer { get; init; }
    }

    #endregion
}
