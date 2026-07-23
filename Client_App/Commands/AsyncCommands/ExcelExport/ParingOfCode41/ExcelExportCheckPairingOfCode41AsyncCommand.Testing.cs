using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41.Testing;
using Client_App.Resources.CustomComparers.SnkComparers;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
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
            var unpaired11 = GetUnpaired11To15Ids(testCase.Form11, testCase.Form15, testCase.Params11To15);
            var unpaired15 = GetUnpaired11To15Ids(testCase.Form15, testCase.Form11, testCase.Params11To15);
            var unpaired12 = GetUnpaired12To16Ids(testCase.Form12, testCase.Form16, testCase.Params12To16);
            var unpaired13 = GetUnpairedOperations13To16(
                    ToDtoList(testCase.Form13), ToDtoList(testCase.Form16), testCase.Params13To16)
                .Select(row => row.Id).OrderBy(id => id).ToList();
            var unpaired14 = GetUnpairedOperations14To16(
                    ToDtoList(testCase.Form14), ToDtoList(testCase.Form16), testCase.Params14To16)
                .Select(row => row.Id).OrderBy(id => id).ToList();
            var unpaired16 = GetUnpairedForm16Ids(
                testCase.Form16, testCase.Form12, testCase.Form13, testCase.Form14,
                testCase.Params12To16, testCase.Params13To16, testCase.Params14To16);

            return new Pairing41ScenarioResult(
                unpaired11, unpaired12, unpaired13, unpaired14, unpaired15, unpaired16);
        }

        public static Pairing41ClosestMatchResult RunClosestMatches(Pairing41TestCase testCase)
        {
            var form11 = ToDtoList(testCase.Form11);
            var form12 = ToDtoList(testCase.Form12);
            var form13 = ToDtoList(testCase.Form13);
            var form14 = ToDtoList(testCase.Form14);
            var form15 = ToDtoList(testCase.Form15);
            var form16 = ToDtoList(testCase.Form16);

            var unpaired11 = GetUnpairedOperations11To15(form11, form15, testCase.Params11To15);
            var unpaired15 = GetUnpairedOperations11To15(form15, form11, testCase.Params11To15);
            var unpaired12 = GetUnpairedOperations12To16(form12, form16, testCase.Params12To16);
            var unpaired13 = GetUnpairedOperations13To16(form13, form16, testCase.Params13To16);
            var unpaired14 = GetUnpairedOperations14To16(form14, form16, testCase.Params14To16);
            var unpaired16 = GetUnpairedForm16(
                form16, form12, form13, form14,
                testCase.Params12To16, testCase.Params13To16, testCase.Params14To16);

            var closest11 = BuildClosestMatchHighlights(unpaired11, form15, testCase.Params11To15)
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing11To15Field, bool>)kv.Value.FieldMatches);
            var closest15 = BuildClosestMatchHighlights(unpaired15, form11, testCase.Params11To15)
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing11To15Field, bool>)kv.Value.FieldMatches);
            var closest12 = BuildClosestMatchHighlights12To16(unpaired12, form16, testCase.Params12To16)
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing12To16Field, bool>)kv.Value);
            var closest13 = BuildClosestMatchHighlights13To16(unpaired13, form16, testCase.Params13To16)
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing13To16Field, bool>)kv.Value);
            var closest14 = BuildClosestMatchHighlights14To16(unpaired14, form16, testCase.Params14To16)
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing14To16Field, bool>)kv.Value);
            var closest16 = BuildClosestMatchHighlights16(
                unpaired16, form12, form13, form14,
                testCase.Params12To16, testCase.Params13To16, testCase.Params14To16);

            return new Pairing41ClosestMatchResult(
                closest11, closest15, closest12, closest13, closest14, closest16);
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
            Quantity = row.Quantity
        };
    }
}
