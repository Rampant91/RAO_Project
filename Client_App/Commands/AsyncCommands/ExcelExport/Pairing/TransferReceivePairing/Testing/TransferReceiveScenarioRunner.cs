namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;

/// <summary>
/// Запуск тестовых сценариев приёма-передачи без обращения к БД.
/// </summary>
public static class TransferReceiveScenarioRunner
{
    public static string NormalizeNumber(string? value) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.NormalizeNumberForTests(value);

    public static string NormalizeSerialNumber(string? value) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.NormalizeSerialNumberForTests(value);

    public static bool SerialNumbersAreEmpty(string? pasNum, string? facNum) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.SerialNumbersAreEmptyForTests(pasNum, facNum);

    public static string NormalizeDate(string? value) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.NormalizeDateForTests(value);

    public static string NormalizeRads(string? value) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.NormalizeRadsForTests(value);

    public static bool DatesEqualExact(string? left, string? right) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.DatesEqualExactForTests(left, right);

    public static bool DateWithinTolerance(string? left, string? right, int days = 15) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.DateWithinToleranceForTests(left, right, days);

    public static bool OpCodesArePaired(string? leftCode, string? rightCode) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.OpCodesArePairedForTests(leftCode, rightCode);

    public static bool ActivityMatches(string? leftActivity, string? rightActivity, bool checkActivity = true) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.ActivityMatchesForTests(
            leftActivity, rightActivity, checkActivity);

    public static ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveFormParams DefaultForm13Params() =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.DefaultForm13ParamsForTests();

    public static ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveFormParams DefaultForm12Params() =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.DefaultForm12ParamsForTests();

    public static ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveFormParams DefaultForm14Params() =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.DefaultForm14ParamsForTests();

    public static ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveFormParams DefaultForm15Params() =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.DefaultForm15ParamsForTests();

    public static TransferReceiveScenarioResult Run(TransferReceiveTestCase testCase) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.RunScenario(testCase);

    public static TransferReceiveScenarioResult RunWithSharedFullPool(TransferReceiveTestCase testCase) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.RunScenarioWithSharedFullPool(testCase);

    public static TransferReceiveClosestMatchResult RunClosestMatches(TransferReceiveTestCase testCase) =>
        ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveTestAccess.RunClosestMatches(testCase);
}
