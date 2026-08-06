using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;
using Client_App.Resources.CustomComparers.SnkComparers;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    #region Test access

    /// <summary>
    /// Доступ к private-логике сопоставления для unit-тестов (без БД/UI/Excel).
    /// </summary>
    public static class Pairing41TestAccess
    {
        public static string NormalizeNumberForTests(string? value) => NormalizeNumber(value);

        public static string NormalizeSerialNumberForTests(string? value) => NormalizeSerialNumber(value);

        public static bool SerialNumbersAreEmptyForTests(string? pasNum, string? facNum) =>
            Operation41PairingKeyComparer.SerialNumbersIsEmpty(pasNum, facNum);

        public static string NormalizeDateForTests(string? value) => NormalizeDate(value);

        public static string NormalizeRadsForTests(string? value) => NormalizeRads(value);

        public static bool NumericWithToleranceForTests(string? left, string? right) =>
            NumericWithTolerance(left, right);

        public static string ToMassTonForTests(string? massKg) => ToMassTon(massKg);

        /// <summary>Следующая строка данных на листе (для проверки дописывания в режиме «вся БД»).</summary>
        public static int GetNextDataRowForTests(ExcelWorksheet sheet) => GetNextDataRow(sheet);

        public static void CreatePairingLegendSheetForTests(ExcelPackage excelPackage) =>
            CreatePairingLegendSheet(excelPackage);

        public static IReadOnlyList<string> Form16DataHeadersForTests => Form16DataHeaders;

        public static IReadOnlyList<string> Form12DataHeadersForTests => Form12DataHeaders;

        public static IReadOnlyList<string> Form13DataHeadersForTests => Form13DataHeaders;

        public static IReadOnlyList<string> Form14DataHeadersForTests => Form14DataHeaders;

        public static IReadOnlyList<string> Form1115DataHeadersForTests => Form1115DataHeaders;

        public static int InfoColCountForTests => InfoColCount;

        public static int Layout16SourceColCountForTests => Layout16.SourceColCount;

        public static int Layout12SourceColCountForTests => Layout12.SourceColCount;

        public static int Layout13SourceColCountForTests => Layout13.SourceColCount;

        public static int Layout14SourceColCountForTests => Layout14.SourceColCount;

        public static int Layout1115SourceColCountForTests => Layout1115.SourceColCount;

        public static int? GetForm16Form12FieldOffsetForTests(Pairing12To16Field field) =>
            GetForm16Form12FieldOffset(field);

        public static int? GetForm16Form13FieldOffsetForTests(Pairing13To16Field field) =>
            GetForm16Form13FieldOffset(field);

        public static int? GetForm16Form14FieldOffsetForTests(Pairing14To16Field field) =>
            GetForm16Form14FieldOffset(field);

        public static int? GetForm12FieldOffsetForTests(Pairing12To16Field field) =>
            GetForm12FieldOffset(field);

        public static int? GetForm13FieldOffsetForTests(Pairing13To16Field field) =>
            GetForm13FieldOffset(field);

        public static int? GetForm14FieldOffsetForTests(Pairing14To16Field field) =>
            GetForm14FieldOffset(field);

        public static int? GetForm1115FieldOffsetForTests(Pairing11To15Field field) =>
            GetForm1115FieldOffset(field);

        /// <summary>
        /// Id непарных 1.1 → (RepsId кандидата 1.5, OrgRegNo кандидата).
        /// Для проверки, что closest не «перепрыгивает» на другую организацию.
        /// </summary>
        public static IReadOnlyDictionary<int, (int CandidateRepsId, string CandidateOrgRegNo)> GetClosest11CandidateOrgKeys(
            IEnumerable<Pairing41Row> form11,
            IEnumerable<Pairing41Row> form15,
            Pairing11To15Params? options = null)
        {
            options ??= new Pairing11To15Params();
            var source = ToDtoList(form11);
            var reference = ToDtoList(form15);
            var unpaired = GetUnpairedOperations11To15(source, reference, options);
            var closest = BuildClosestMatchHighlights(unpaired, reference, options);
            return closest.ToDictionary(
                kv => kv.Key,
                kv => (kv.Value.Candidate.RepsId, kv.Value.Candidate.OrgRegNo ?? string.Empty));
        }

        public static IReadOnlyList<int> GetUnpaired11To15Ids(
            IEnumerable<Pairing41Row> source,
            IEnumerable<Pairing41Row> reference,
            Pairing11To15Params? options = null)
        {
            options ??= new Pairing11To15Params();
            return GetUnpairedOperations11To15(ToDtoList(source), ToDtoList(reference), options)
                .Select(row => row.Id)
                .OrderBy(id => id)
                .ToList();
        }

        public static IReadOnlyList<int> GetUnpaired12To16Ids(
            IEnumerable<Pairing41Row> source,
            IEnumerable<Pairing41Row> reference,
            Pairing12To16Params? options = null)
        {
            options ??= new Pairing12To16Params();
            return GetUnpairedOperations12To16(ToDtoList(source), ToDtoList(reference), options)
                .Select(row => row.Id)
                .OrderBy(id => id)
                .ToList();
        }

        public static IReadOnlyList<int> GetUnpairedForm16Ids(
            IEnumerable<Pairing41Row> form16,
            IEnumerable<Pairing41Row> form12,
            IEnumerable<Pairing41Row> form13,
            IEnumerable<Pairing41Row> form14,
            Pairing12To16Params? pairing12To16Params = null,
            Pairing13To16Params? pairing13To16Params = null,
            Pairing14To16Params? pairing14To16Params = null)
        {
            pairing12To16Params ??= new Pairing12To16Params();
            pairing13To16Params ??= new Pairing13To16Params();
            pairing14To16Params ??= new Pairing14To16Params();

            return GetUnpairedForm16(
                    ToDtoList(form16),
                    ToDtoList(form12),
                    ToDtoList(form13),
                    ToDtoList(form14),
                    pairing12To16Params,
                    pairing13To16Params,
                    pairing14To16Params)
                .Select(row => row.Id)
                .OrderBy(id => id)
                .ToList();
        }

        public static Pairing41ScenarioResult RunScenario(Pairing41TestCase testCase)
        {
            var form11 = ToDtoList(testCase.Form11);
            var form12 = ToDtoList(testCase.Form12);
            var form13 = ToDtoList(testCase.Form13);
            var form14 = ToDtoList(testCase.Form14);
            var form15 = ToDtoList(testCase.Form15);
            var form16 = ToDtoList(testCase.Form16);

            var unpaired = ComputeOrganizationUnpaired(
                form11, form12, form13, form14, form15, form16,
                new PairingParamsSet(
                    testCase.Params11To15,
                    testCase.Params12To16,
                    testCase.Params13To16,
                    testCase.Params14To16));

            return new Pairing41ScenarioResult(
                unpaired.Form11.Select(row => row.Id).OrderBy(id => id).ToList(),
                unpaired.Form12.Select(row => row.Id).OrderBy(id => id).ToList(),
                unpaired.Form13.Select(row => row.Id).OrderBy(id => id).ToList(),
                unpaired.Form14.Select(row => row.Id).OrderBy(id => id).ToList(),
                unpaired.Form15.Select(row => row.Id).OrderBy(id => id).ToList(),
                unpaired.Form16.Select(row => row.Id).OrderBy(id => id).ToList());
        }

        public static Pairing41ClosestMatchResult RunClosestMatches(Pairing41TestCase testCase)
        {
            var form11 = ToDtoList(testCase.Form11);
            var form12 = ToDtoList(testCase.Form12);
            var form13 = ToDtoList(testCase.Form13);
            var form14 = ToDtoList(testCase.Form14);
            var form15 = ToDtoList(testCase.Form15);
            var form16 = ToDtoList(testCase.Form16);

            var unpaired = ComputeOrganizationUnpaired(
                form11, form12, form13, form14, form15, form16,
                new PairingParamsSet(
                    testCase.Params11To15,
                    testCase.Params12To16,
                    testCase.Params13To16,
                    testCase.Params14To16));

            var closest11Raw = BuildClosestMatchHighlights(unpaired.Form11, form15, testCase.Params11To15);
            var closest11 = closest11Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing11To15Field, bool>)kv.Value.FieldMatches);
            var closest11Candidates = closest11Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest15Raw = BuildClosestMatchHighlights(unpaired.Form15, form11, testCase.Params11To15);
            var closest15 = closest15Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing11To15Field, bool>)kv.Value.FieldMatches);
            var closest15Candidates = closest15Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest12Raw = BuildClosestMatchHighlights12To16(unpaired.Form12, form16, testCase.Params12To16);
            var closest12 = closest12Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing12To16Field, bool>)kv.Value.FieldMatches);
            var closest12Candidates = closest12Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest13Raw = BuildClosestMatchHighlights13To16(unpaired.Form13, form16, testCase.Params13To16);
            var closest13 = closest13Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing13To16Field, bool>)kv.Value.FieldMatches);
            var closest13AggregateStateMatch = closest13Raw
                .ToDictionary(kv => kv.Key, kv => kv.Value.AggregateStateMatchesCodeRao);
            var closest13Candidates = closest13Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest14Raw = BuildClosestMatchHighlights14To16(unpaired.Form14, form16, testCase.Params14To16);
            var closest14 = closest14Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing14To16Field, bool>)kv.Value.FieldMatches);
            var closest14AggregateStateMatch = closest14Raw
                .ToDictionary(kv => kv.Key, kv => kv.Value.AggregateStateMatchesCodeRao);
            var closest14Candidates = closest14Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest16 = BuildClosestMatchHighlights16(
                unpaired.Form16, form12, form13, form14,
                testCase.Params12To16, testCase.Params13To16, testCase.Params14To16);
            var closest16Candidates = closest16.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            return new Pairing41ClosestMatchResult(
                closest11, closest15, closest12, closest13, closest14, closest16,
                closest13AggregateStateMatch, closest14AggregateStateMatch,
                closest11Candidates, closest15Candidates, closest12Candidates,
                closest13Candidates, closest14Candidates, closest16Candidates);
        }

        private static List<Operation41PairingDto> ToDtoList(IEnumerable<Pairing41Row> rows) =>
            rows.Select(ToDto).ToList();

        private static Operation41PairingDto ToDto(Pairing41Row row) => new()
        {
            Id = row.Id,
            RepsId = row.RepsId,
            ReportId = row.ReportId,
            OpCode = row.OpCode,
            OpDate = row.OpDate,
            PasNum = row.PasNum,
            FacNum = row.FacNum,
            Type = row.Type,
            Radionuclids = row.Radionuclids,
            CreationDate = row.CreationDate,
            DocumentVid = row.DocumentVid,
            DocumentNumber = row.DocumentNumber,
            DocumentDate = row.DocumentDate,
            ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo,
            TransporterOkpo = row.TransporterOkpo,
            PackName = row.PackName,
            PackType = row.PackType,
            PackNumber = row.PackNumber,
            Activity = row.Activity,
            MainRadionuclids = row.MainRadionuclids,
            TritiumActivity = row.TritiumActivity,
            BetaGammaActivity = row.BetaGammaActivity,
            AlphaActivity = row.AlphaActivity,
            TransuraniumActivity = row.TransuraniumActivity,
            Mass = row.Mass,
            Volume = row.Volume,
            ActivityMeasurementDate = row.ActivityMeasurementDate,
            Quantity = row.Quantity,
            FormNum = row.FormNum,
            CodeRao = row.CodeRao,
            AggregateState = row.AggregateState,
            OrgRegNo = row.OrgRegNo
        };
    }

    #endregion
}
