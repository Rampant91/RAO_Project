using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Spravochniki;

/// <summary>
/// Единый справочник ОКСМ: актуальные краткие наименования (для UI),
/// исторические — для проверок; для выпадающего списка — срез на дату операции.
/// Версии раньше исходного ОКСМ в программе (изм. 27/2021) не учитываются.
/// </summary>
public sealed class OksmCatalog
{
    /// <summary>Дата вступления в силу исходного ОКСМ в программе (изм. 27/2021).</summary>
    public static readonly DateOnly BaselineEffectiveDate = new(2021, 6, 1);

    private readonly Dictionary<string, OksmCountryEntry> _currentByKod;
    private readonly HashSet<string> _acceptedShortNames;
    private readonly HashSet<string> _currentShortNames;
    private readonly ReadOnlyDictionary<string, string> _currentKodToShortName;
    private readonly List<OksmShortNamePeriod> _periods;

    public OksmCatalog(
        IEnumerable<OksmCountryEntry> currentEntries,
        IEnumerable<string> legacyShortNames = null,
        IEnumerable<OksmShortNamePeriod> periods = null)
    {
        ArgumentNullException.ThrowIfNull(currentEntries);

        _currentByKod = new Dictionary<string, OksmCountryEntry>(StringComparer.Ordinal);
        _acceptedShortNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        _currentShortNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var kodToShort = new Dictionary<string, string>(StringComparer.Ordinal);
        _periods = [];

        foreach (var entry in currentEntries)
        {
            if (entry is null || string.IsNullOrWhiteSpace(entry.Kod))
                continue;

            var shortName = (entry.ShortName ?? string.Empty).Trim();
            var normalized = new OksmCountryEntry
            {
                Kod = entry.Kod.Trim(),
                ShortName = shortName,
                LongName = (entry.LongName ?? string.Empty).Trim(),
                Alpha2 = (entry.Alpha2 ?? string.Empty).Trim(),
                Alpha3 = (entry.Alpha3 ?? string.Empty).Trim()
            };

            _currentByKod[normalized.Kod] = normalized;
            kodToShort[normalized.Kod] = normalized.ShortName;

            if (shortName.Length > 0)
            {
                _currentShortNames.Add(shortName);
                _acceptedShortNames.Add(shortName);
            }
        }

        if (legacyShortNames != null)
        {
            foreach (var legacy in legacyShortNames)
            {
                var name = (legacy ?? string.Empty).Trim();
                if (name.Length > 0)
                    _acceptedShortNames.Add(name);
            }
        }

        if (periods != null)
        {
            foreach (var period in periods)
            {
                if (period is null || string.IsNullOrWhiteSpace(period.Kod))
                    continue;

                var shortName = (period.ShortName ?? string.Empty).Trim();
                if (shortName.Length == 0)
                    continue;

                var normalizedPeriod = new OksmShortNamePeriod
                {
                    Kod = period.Kod.Trim(),
                    ShortName = shortName,
                    ValidFrom = period.ValidFrom,
                    ValidTo = period.ValidTo
                };
                _periods.Add(normalizedPeriod);
                _acceptedShortNames.Add(shortName);
            }
        }

        _currentKodToShortName = new ReadOnlyDictionary<string, string>(kodToShort);
    }

    /// <summary>Код → актуальное краткое наименование (для выпадающих списков без даты).</summary>
    public IReadOnlyDictionary<string, string> CurrentKodToShortName => _currentKodToShortName;

    public IReadOnlyCollection<OksmCountryEntry> CurrentEntries => _currentByKod.Values;

    public int CurrentCount => _currentByKod.Count;

    public int AcceptedShortNameCount => _acceptedShortNames.Count;

    public IReadOnlyList<OksmShortNamePeriod> Periods => _periods;

    public bool IsCurrentShortName(string value)
    {
        var name = Normalize(value);
        return name.Length > 0 && _currentShortNames.Contains(name);
    }

    /// <summary>
    /// Краткое наименование допустимо в проверках/валидации
    /// (актуальное или любое зафиксированное историческое).
    /// </summary>
    public bool IsAcceptedShortName(string value)
    {
        var name = Normalize(value);
        return name.Length > 0 && _acceptedShortNames.Contains(name);
    }

    /// <summary>
    /// Срез ОКСМ для выпадающего списка на дату операции.
    /// null / неразобранная дата → актуальные наименования.
    /// </summary>
    public IReadOnlyDictionary<string, string> GetKodToShortNameAsOf(DateOnly? operationDate)
    {
        if (operationDate is null)
            return _currentKodToShortName;

        var asOf = operationDate.Value;
        var result = new Dictionary<string, string>(_currentKodToShortName, StringComparer.Ordinal);

        foreach (var period in _periods)
        {
            if (!IsActiveOn(period, asOf))
                continue;

            // Историческое имя с конечной датой — подмена актуального на прежнее.
            if (period.ValidTo is not null)
            {
                result[period.Kod] = period.ShortName;
                continue;
            }

            // Только ValidFrom без ValidTo и имя совпадает с актуальным — дата появления позиции.
            if (period.ValidFrom is not null
                && _currentByKod.TryGetValue(period.Kod, out var current)
                && string.Equals(current.ShortName, period.ShortName, StringComparison.OrdinalIgnoreCase))
            {
                // already in result if asOf >= ValidFrom; handled below by removal pass
            }
        }

        // Убрать позиции, которые ещё не появились на asOf.
        foreach (var period in _periods)
        {
            if (period.ValidFrom is null || period.ValidTo is not null)
                continue;
            if (!_currentByKod.TryGetValue(period.Kod, out var current))
                continue;
            if (!string.Equals(current.ShortName, period.ShortName, StringComparison.OrdinalIgnoreCase))
                continue;
            if (asOf < period.ValidFrom.Value)
                result.Remove(period.Kod);
        }

        return result;
    }

    public static bool TryParseOperationDate(string value, out DateOnly date)
    {
        date = default;
        var text = (value ?? string.Empty).Trim();
        if (text.Length == 0)
            return false;

        string[] formats = ["dd.MM.yyyy", "d.M.yyyy", "yyyy-MM-dd", "dd/MM/yyyy"];
        return DateOnly.TryParseExact(text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
               || DateOnly.TryParse(text, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out date)
               || DateOnly.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }

    public static OksmCatalog LoadFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к ОКСМ не задан.", nameof(filePath));
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл справочника ОКСМ не найден.", filePath);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(new FileInfo(filePath));

        var currentSheet = package.Workbook.Worksheets["Лист1"]
                          ?? package.Workbook.Worksheets.FirstOrDefault()
                          ?? throw new InvalidOperationException($"В файле ОКСМ нет листов: {filePath}");

        var current = ReadCurrentSheet(currentSheet);
        var (legacyNames, periods) = ReadHistorySheet(package);

        return new OksmCatalog(current, legacyNames, periods);
    }

    /// <summary>
    /// Ищет data/Spravochniki/oksm.xlsx относительно BaseDirectory / CurrentDirectory.
    /// Если найдено несколько копий (например bin\Debug и корень репозитория),
    /// предпочитает файл вне bin/obj и более новый по дате записи —
    /// чтобы Debug не цеплял устаревший справочник из выходной папки.
    /// </summary>
    public static string ResolveDefaultFilePath()
    {
        const string fileName = "oksm.xlsx";
        var found = new List<string>();

        foreach (var start in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
        {
            if (string.IsNullOrWhiteSpace(start))
                continue;

            var dir = new DirectoryInfo(start);
            for (var i = 0; i < 8; i++)
            {
                var candidate = Path.Combine(dir.FullName, "data", "Spravochniki", fileName);
                if (File.Exists(candidate))
                {
                    var full = Path.GetFullPath(candidate);
                    if (!found.Exists(p => string.Equals(p, full, StringComparison.OrdinalIgnoreCase)))
                        found.Add(full);
                }

                if (dir.Parent is null)
                    break;
                dir = dir.Parent;
            }
        }

        if (found.Count == 0)
            return null;

        return found
            .OrderBy(IsUnderBuildOutputDirectory) // false (репозиторий) раньше true (bin/obj)
            .ThenByDescending(File.GetLastWriteTimeUtc)
            .First();
    }

    private static bool IsUnderBuildOutputDirectory(string path)
    {
        var normalized = path.Replace('/', '\\');
        return normalized.Contains("\\bin\\", StringComparison.OrdinalIgnoreCase)
               || normalized.Contains("\\obj\\", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsActiveOn(OksmShortNamePeriod period, DateOnly asOf)
    {
        if (period.ValidFrom is not null && asOf < period.ValidFrom.Value)
            return false;
        if (period.ValidTo is not null && asOf >= period.ValidTo.Value)
            return false;
        return true;
    }

    private static List<OksmCountryEntry> ReadCurrentSheet(ExcelWorksheet worksheet)
    {
        var result = new List<OksmCountryEntry>();
        var row = 8;
        while (!string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text)
               || !string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text))
        {
            var kod = worksheet.Cells[row, 2].Text.Trim();
            if (kod.Length > 0)
            {
                result.Add(new OksmCountryEntry
                {
                    Kod = kod,
                    ShortName = worksheet.Cells[row, 3].Text.Trim(),
                    LongName = worksheet.Cells[row, 4].Text.Trim(),
                    Alpha2 = worksheet.Cells[row, 5].Text.Trim(),
                    Alpha3 = worksheet.Cells[row, 6].Text.Trim()
                });
            }

            row++;
            if (row > 10000)
                break;
        }

        return result;
    }

    private static (List<string> LegacyNames, List<OksmShortNamePeriod> Periods) ReadHistorySheet(ExcelPackage package)
    {
        var sheet = package.Workbook.Worksheets["ИсторическиеКраткие"];
        if (sheet is null)
            return ([], []);

        var legacyNames = new List<string>();
        var periods = new List<OksmShortNamePeriod>();
        var row = 2;
        while (!string.IsNullOrWhiteSpace(sheet.Cells[row, 1].Text)
               || !string.IsNullOrWhiteSpace(sheet.Cells[row, 2].Text))
        {
            var kod = sheet.Cells[row, 1].Text.Trim();
            var shortName = sheet.Cells[row, 2].Text.Trim();
            if (shortName.Length > 0)
                legacyNames.Add(shortName);

            if (kod.Length > 0 && shortName.Length > 0)
            {
                periods.Add(new OksmShortNamePeriod
                {
                    Kod = kod,
                    ShortName = shortName,
                    ValidFrom = ParseSheetDate(sheet.Cells[row, 3].Text),
                    ValidTo = ParseSheetDate(sheet.Cells[row, 4].Text)
                });
            }

            row++;
            if (row > 10000)
                break;
        }

        return (legacyNames, periods);
    }

    private static DateOnly? ParseSheetDate(string text)
    {
        if (TryParseOperationDate(text, out var date))
            return date;
        return null;
    }

    private static string Normalize(string value) => (value ?? string.Empty).Trim();
}

public sealed class OksmCountryEntry
{
    public string Kod { get; init; } = "";
    public string ShortName { get; init; } = "";
    public string LongName { get; init; } = "";
    public string Alpha2 { get; init; } = "";
    public string Alpha3 { get; init; } = "";
}

/// <summary>
/// Период действия краткого наименования.
/// ValidTo — дата вступления новой редакции (имя действует при asOf &lt; ValidTo).
/// ValidFrom без ValidTo у актуального имени — дата появления позиции в классификаторе.
/// </summary>
public sealed class OksmShortNamePeriod
{
    public string Kod { get; init; } = "";
    public string ShortName { get; init; } = "";
    public DateOnly? ValidFrom { get; init; }
    public DateOnly? ValidTo { get; init; }
}
