using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

public enum CompareColumnKind
{
    Text,
    Id,
    Date,
    Numeric,
    Quantity,
    Type,
    Radionuclids,
    Code
}

public enum DiffRowStatus
{
    Unchanged,
    Changed,
    Moved,
    Deleted,
    Added
}

public sealed record CompareColumn(
    string Header,
    CompareColumnKind Kind,
    bool InFingerprint = false,
    bool IsFactoryId = false,
    bool IsRowNumber = false);

public sealed class CompareRowDto
{
    public int Id { get; init; }
    public int SourceIndex { get; init; }
    public int NumberInOrder { get; init; }
    public required string[] Values { get; init; }
    public required string Fingerprint { get; set; }
    public string? OpDate { get; init; }
}

public sealed class CompareReportDto
{
    public int ReportId { get; init; }
    public required string FormNum { get; init; }
    public required string PeriodKey { get; init; }
    public required string PeriodDisplay { get; init; }
    public string StartPeriod { get; init; } = "";
    public string EndPeriod { get; init; } = "";
    public string Year { get; init; } = "";
    public byte CorrectionNumber { get; init; }
    public required string RegNo { get; init; }
    public required string Okpo { get; init; }
    /// <summary>Сокращённое наименование организации (из формы 1.0/2.0).</summary>
    public string OrgShortName { get; init; } = "";
    /// <summary>Имя файла .RAODB, из которого взят отчёт (для файлов сверки).</summary>
    public string SourceLabel { get; init; } = "";
    public required List<CompareRowDto> Rows { get; init; }
    public required CompareColumn[] Columns { get; init; }
}

public enum ReportCompareKind
{
    Compared,
    MissingInCompareFiles,
    MissingInSourceDb,
    PeriodOverlap
}

public readonly record struct ReportMatchKey(string RegNo, string Okpo, string FormNum, string PeriodKey)
{
    public static ReportMatchKey From(CompareReportDto report) =>
        new(
            NormalizeOrg(report.RegNo),
            NormalizeOrg(report.Okpo),
            report.FormNum,
            report.PeriodKey);

    public static string NormalizeOrg(string? value) =>
        (value ?? string.Empty).Trim().ToUpperInvariant();
}

/// <summary>
/// Подсказка периода из файла сверки: грузим из БД точный ключ или пересекающийся период той же формы.
/// </summary>
public readonly record struct ComparePeriodHint(
    string FormNum,
    string PeriodKey,
    string StartPeriod,
    string EndPeriod,
    string Year);

public sealed class DiffLine
{
    public DiffRowStatus Status { get; init; }
    public CompareRowDto? Left { get; init; }
    public CompareRowDto? Right { get; init; }
    public FieldMatchLevel[]? FieldLevels { get; init; }
    /// <summary>Схожесть строки 0–100 (для пары); удалённые/добавленные — 0.</summary>
    public int ConfidencePercent { get; init; }
    /// <summary>№ п/п слева встречается в исходнике больше одного раза.</summary>
    public bool LeftNppDuplicate { get; init; }
    /// <summary>№ п/п справа встречается в файле сравнения больше одного раза.</summary>
    public bool RightNppDuplicate { get; init; }

    public string StatusText => Status switch
    {
        DiffRowStatus.Unchanged => "Без изменений",
        DiffRowStatus.Changed => "Изменена",
        DiffRowStatus.Moved => Left is not null && Right is not null
            ? $"№ п/п: {Left.NumberInOrder} → {Right.NumberInOrder}"
            : "Сбит № п/п",
        DiffRowStatus.Deleted => "Удалена",
        DiffRowStatus.Added => "Добавлена",
        _ => Status.ToString()
    };
}

public sealed class ReportCompareResult
{
    public ReportCompareKind Kind { get; init; } = ReportCompareKind.Compared;
    public required CompareReportDto Left { get; init; }
    public CompareReportDto? Right { get; init; }
    public bool HasRight => Right is not null;
    /// <summary>Лист сравнения — только при точном ключе и отличиях в данных/порядке.</summary>
    public bool CreatesDetailSheet => Kind == ReportCompareKind.Compared && !IsIdentical;
    public bool IsIdentical { get; init; }
    public bool OrderOnlyChanged { get; init; }
    /// <summary>Все строки unified-diff (включая Unchanged).</summary>
    public required List<DiffLine> Lines { get; init; }
    /// <summary>Строки для листа сравнения (все, кроме случая полного совпадения отчёта).</summary>
    public required List<DiffLine> DisplayLines { get; init; }
    public string? Message { get; init; }
    public int UnchangedCount { get; init; }
    public int ChangedCount { get; init; }
    public int MovedCount { get; init; }
    public int DeletedCount { get; init; }
    public int AddedCount { get; init; }
    /// <summary>№ п/п, которые в исходнике встречаются больше одного раза.</summary>
    public IReadOnlyList<int> LeftDuplicateNpps { get; init; } = [];
    /// <summary>№ п/п, которые в файле сравнения встречаются больше одного раза.</summary>
    public IReadOnlyList<int> RightDuplicateNpps { get; init; } = [];
    public bool HasNppDuplicates => LeftDuplicateNpps.Count > 0 || RightDuplicateNpps.Count > 0;
}

public static partial class FormPrintCompareSchema
{
    public static readonly HashSet<string> SupportedForms = new(StringComparer.Ordinal)
    {
        "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9",
        "2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12"
    };

    public static CompareColumn[] ColumnsFor(string formNum) => formNum switch
    {
        "1.1" => Columns11,
        "1.2" => Columns12,
        "1.3" => Columns13,
        "1.4" => Columns14,
        "1.5" => Columns15,
        "1.6" => Columns16,
        "1.7" => Columns17,
        "1.8" => Columns18,
        "1.9" => Columns19,
        "2.1" => Columns21,
        "2.2" => Columns22,
        "2.3" => Columns23,
        "2.4" => Columns24,
        "2.5" => Columns25,
        "2.6" => Columns26,
        "2.7" => Columns27,
        "2.8" => Columns28,
        "2.9" => Columns29,
        "2.10" => Columns210,
        "2.11" => Columns211,
        "2.12" => Columns212,
        _ => []
    };

    private static readonly CompareColumn[] Columns11 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Номер паспорта", CompareColumnKind.Id, InFingerprint: true),
        new("Тип", CompareColumnKind.Type, InFingerprint: true),
        new("Радионуклиды", CompareColumnKind.Radionuclids),
        new("Заводской номер", CompareColumnKind.Id, InFingerprint: true, IsFactoryId: true),
        new("Количество", CompareColumnKind.Quantity),
        new("Активность, Бк", CompareColumnKind.Numeric),
        new("ОКПО изготовителя", CompareColumnKind.Id),
        new("Дата выпуска", CompareColumnKind.Date),
        new("Категория", CompareColumnKind.Text),
        new("НСС, мес.", CompareColumnKind.Text),
        new("Код формы собственности", CompareColumnKind.Code),
        new("Владелец", CompareColumnKind.Text),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id),
        new("Дата документа", CompareColumnKind.Date),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id),
        new("ОКПО перевозчика", CompareColumnKind.Id),
        new("Наименование упаковки", CompareColumnKind.Text),
        new("Тип УКТ", CompareColumnKind.Text),
        new("Номер УКТ", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns13 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Номер паспорта", CompareColumnKind.Id, InFingerprint: true),
        new("Тип", CompareColumnKind.Type),
        new("Радионуклиды", CompareColumnKind.Radionuclids),
        new("Заводской номер", CompareColumnKind.Id, InFingerprint: true, IsFactoryId: true),
        new("Активность, Бк", CompareColumnKind.Numeric),
        new("ОКПО изготовителя", CompareColumnKind.Id),
        new("Дата выпуска", CompareColumnKind.Date),
        new("Агрегатное состояние", CompareColumnKind.Code, InFingerprint: true),
        new("Код формы собственности", CompareColumnKind.Code),
        new("Владелец", CompareColumnKind.Text),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id),
        new("Дата документа", CompareColumnKind.Date),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id),
        new("ОКПО перевозчика", CompareColumnKind.Id),
        new("Наименование упаковки", CompareColumnKind.Text),
        new("Тип УКТ", CompareColumnKind.Text),
        new("Номер УКТ", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns14 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Номер паспорта", CompareColumnKind.Id, InFingerprint: true),
        new("Наименование", CompareColumnKind.Text, InFingerprint: true),
        new("Сорт", CompareColumnKind.Code, InFingerprint: true),
        new("Радионуклиды", CompareColumnKind.Radionuclids),
        new("Активность, Бк", CompareColumnKind.Numeric),
        new("Дата измерения активности", CompareColumnKind.Date),
        new("Объём", CompareColumnKind.Numeric),
        new("Масса", CompareColumnKind.Numeric),
        new("Агрегатное состояние", CompareColumnKind.Code),
        new("Код формы собственности", CompareColumnKind.Code),
        new("Владелец", CompareColumnKind.Text),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id),
        new("Дата документа", CompareColumnKind.Date),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id),
        new("ОКПО перевозчика", CompareColumnKind.Id),
        new("Наименование упаковки", CompareColumnKind.Text),
        new("Тип УКТ", CompareColumnKind.Text),
        new("Номер УКТ", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns212 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Код типа объекта", CompareColumnKind.Code, InFingerprint: true),
        new("Радионуклиды", CompareColumnKind.Radionuclids, InFingerprint: true),
        new("Активность, Бк", CompareColumnKind.Numeric),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id, InFingerprint: true)
    ];
}
