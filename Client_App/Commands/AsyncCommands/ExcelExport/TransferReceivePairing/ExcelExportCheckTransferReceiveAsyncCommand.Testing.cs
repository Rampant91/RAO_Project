using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using Client_App.Resources.CustomComparers.SnkComparers;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Test access

    /// <summary>
    /// Доступ к private-логике сверки приёма-передачи для unit-тестов (без БД/UI/Excel).
    /// </summary>
    public static class TransferReceiveTestAccess
    {
        public static TransferReceive11Params DefaultForm13ParamsForTests() => DefaultForm13Params();

        public static string NormalizeNumberForTests(string? value) => NormalizeNumber(value);

        public static string NormalizeSerialNumberForTests(string? value) => NormalizeSerialNumber(value);

        public static bool SerialNumbersAreEmptyForTests(string? pasNum, string? facNum) =>
            Operation41PairingKeyComparer.SerialNumbersIsEmpty(pasNum, facNum);

        public static string NormalizeDateForTests(string? value) => NormalizeDate(value);

        public static string NormalizeRadsForTests(string? value) => NormalizeRads(value);

        public static bool DatesEqualExactForTests(string? left, string? right) =>
            DatesEqualExact(left, right);

        public static bool DateWithinToleranceForTests(string? left, string? right, int days = OperationDateToleranceDays) =>
            DateWithinTolerance(left, right, days);

        public static bool OpCodesArePairedForTests(string? leftCode, string? rightCode) =>
            OpCodesArePaired(leftCode, rightCode);

        public static bool ActivityMatchesForTests(string? leftActivity, string? rightActivity, bool checkActivity = true) =>
            ActivityMatches(
                new TransferReceiveDto { Activity = leftActivity ?? string.Empty, OpCode = "21", IsTransfer = true },
                new TransferReceiveDto { Activity = rightActivity ?? string.Empty, OpCode = "31", IsTransfer = false },
                checkActivity);

        public static TransferReceiveScenarioResult RunScenario(TransferReceiveTestCase testCase)
        {
            var ourOkpoNorm = NormalizeNumber(testCase.OurOkpo);
            var ourOps = ToDtoList(testCase.OurOps, fallbackOrgOkpo: testCase.OurOkpo, testCase.FormNum);
            var counterpartOps = ToDtoList(testCase.CounterpartOps, fallbackOrgOkpo: null, testCase.FormNum);
            var aliases = ToAliasMap(testCase.OkpoAliases);

            var (unpaired, _) = AnalyzeForm11ForOrganization(
                ourOps, counterpartOps, ourOkpoNorm, testCase.Params, aliases);

            return new TransferReceiveScenarioResult(
                unpaired.Select(row => row.Id).OrderBy(id => id).ToList());
        }

        public static TransferReceiveClosestMatchResult RunClosestMatches(TransferReceiveTestCase testCase)
        {
            var ourOkpoNorm = NormalizeNumber(testCase.OurOkpo);
            var ourOps = ToDtoList(testCase.OurOps, fallbackOrgOkpo: testCase.OurOkpo, testCase.FormNum);
            var counterpartOps = ToDtoList(testCase.CounterpartOps, fallbackOrgOkpo: null, testCase.FormNum);
            var aliases = ToAliasMap(testCase.OkpoAliases);

            var (unpaired, opsByOrgOkpo) = AnalyzeForm11ForOrganization(
                ourOps, counterpartOps, ourOkpoNorm, testCase.Params, aliases);

            var closest = BuildClosestMatchResults(unpaired, opsByOrgOkpo, testCase.Params)
                .ToDictionary(
                    kv => kv.Key,
                    kv => (IReadOnlyDictionary<TransferReceiveField, bool>)kv.Value.FieldMatches);

            return new TransferReceiveClosestMatchResult(closest);
        }

        private static Dictionary<string, List<int>>? ToAliasMap(
            IReadOnlyDictionary<string, IReadOnlyList<int>>? aliases)
        {
            if (aliases is null || aliases.Count == 0)
            {
                return null;
            }

            return aliases.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.ToList(),
                StringComparer.Ordinal);
        }

        private static List<TransferReceiveDto> ToDtoList(
            IEnumerable<TransferReceiveRow> rows,
            string? fallbackOrgOkpo,
            string formNum) =>
            rows.Select(row => ToDto(row, fallbackOrgOkpo, formNum)).ToList();

        private static TransferReceiveDto ToDto(TransferReceiveRow row, string? fallbackOrgOkpo, string formNum)
        {
            var orgOkpo = string.IsNullOrWhiteSpace(row.OrgOkpo)
                ? fallbackOrgOkpo ?? string.Empty
                : row.OrgOkpo;
            var isTransfer = row.IsTransfer ?? IsTransferCodeForm11(row.OpCode);
            var isForm13 = formNum == "1.3" || row.AggregateState is not null;

            return new TransferReceiveDto
            {
                Id = row.Id,
                RepsId = row.RepsId,
                ReportId = row.ReportId,
                OrgOkpo = orgOkpo,
                OpCode = row.OpCode,
                OpDate = row.OpDate,
                PasNum = row.PasNum,
                FacNum = row.FacNum,
                Type = row.Type,
                Radionuclids = row.Radionuclids,
                PackNumber = row.PackNumber,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo,
                Activity = row.Activity,
                CreatorOkpo = row.CreatorOkpo,
                CreationDate = row.CreationDate,
                Quantity = isForm13 ? 1 : row.Quantity,
                AggregateState = row.AggregateState,
                IsTransfer = isTransfer
            };
        }
    }

    #endregion
}
