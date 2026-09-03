namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;

/// <summary>
/// Запуск тестовых сценариев парности 41 без обращения к БД.
/// </summary>
public static class Pairing41ScenarioRunner
{
    public static string NormalizeNumber(string? value) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.NormalizeNumberForTests(value);

    public static string NormalizeSerialNumber(string? value) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.NormalizeSerialNumberForTests(value);

    public static bool SerialNumbersAreEmpty(string? pasNum, string? facNum) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.SerialNumbersAreEmptyForTests(pasNum, facNum);

    public static string NormalizeDate(string? value) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.NormalizeDateForTests(value);

    public static string NormalizeRads(string? value) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.NormalizeRadsForTests(value);

    public static bool NumericWithTolerance(string? left, string? right) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.NumericWithToleranceForTests(left, right);

    public static string ToMassTon(string? massKg) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.ToMassTonForTests(massKg);

    public static Pairing41ScenarioResult Run(Pairing41TestCase testCase) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.RunScenario(testCase);

    public static Pairing41ClosestMatchResult RunClosestMatches(Pairing41TestCase testCase) =>
        ExcelExportCheckPairingOfCode41AsyncCommand.Pairing41TestAccess.RunClosestMatches(testCase);
}
