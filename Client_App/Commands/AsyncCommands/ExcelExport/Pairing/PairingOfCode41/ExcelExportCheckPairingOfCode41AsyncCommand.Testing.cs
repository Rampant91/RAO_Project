using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;
using Client_App.Resources;
using Client_App.Resources.CustomComparers.SnkComparers;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41;

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

        public static int Layout1115ConfidenceColForTests => Layout1115.ConfidenceCol;

        public static int Layout1115ClosestStartColForTests => Layout1115.ClosestStartCol;

        public static int Layout1115TotalColCountForTests => Layout1115.TotalColCount;

        public static int Layout12ConfidenceColForTests => Layout12.ConfidenceCol;

        public static int Layout12ClosestStartColForTests => Layout12.ClosestStartCol;

        public static int Layout13ConfidenceColForTests => Layout13.ConfidenceCol;

        public static int Layout13ClosestStartColForTests => Layout13.ClosestStartCol;

        public static int Layout14ConfidenceColForTests => Layout14.ConfidenceCol;

        public static int Layout14ClosestStartColForTests => Layout14.ClosestStartCol;

        public static int Layout16ConfidenceColForTests => Layout16.ConfidenceCol;

        public static int Layout16ClosestStartColForTests => Layout16.ClosestStartCol;

        public static int Layout16DocumentNumberOffsetForTests => InfoColCount + 11;

        /// <summary>Smoke: код операции слева/справа, жирный % схожести с заливкой по порогу.</summary>
        public static void WriteUnpairedSmokeRow1115ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            string sourceOpCode,
            string closestOpCode,
            int confidencePercent)
        {
            var opOffset = GetForm1115FieldOffset(Pairing11To15Field.OperationCode)
                           ?? throw new InvalidOperationException("Нет колонки кода операции.");
            sheet.Cells[dataRow, 1 + opOffset].Value = sourceOpCode;
            sheet.Cells[dataRow, Layout1115.ClosestStartCol + opOffset].Value = closestOpCode;
            var cell = sheet.Cells[dataRow, Layout1115.ConfidenceCol];
            cell.Value = confidencePercent;
            cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.Fill.SetBackground(ConfidenceFill(confidencePercent), OfficeOpenXml.Style.ExcelFillStyle.Solid);
        }

        /// <summary>Smoke: заливка поля по уровню (Exact/Near/Mismatch) на листе 1.1↔1.5.</summary>
        public static void ApplyFieldLevelFill1115ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            int startCol,
            Pairing11To15Field field,
            Shared.FieldMatchLevel level)
        {
            if (GetForm1115FieldOffset(field) is not int offset)
            {
                return;
            }

            ApplyPairingComparisonCellFill(sheet, dataRow, startCol + offset, level);
        }

        /// <summary>Smoke: маркеры слева/справа и жирный % схожести на листе 1.2↔1.6.</summary>
        public static void WriteUnpairedSmokeRow12ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            string sourceMarker,
            string closestMarker,
            int confidencePercent)
        {
            var offset = GetForm12FieldOffset(Pairing12To16Field.DocumentNumber)
                         ?? throw new InvalidOperationException("Нет колонки номера документа.");
            sheet.Cells[dataRow, 1 + offset].Value = sourceMarker;
            sheet.Cells[dataRow, Layout12.ClosestStartCol + offset].Value = closestMarker;
            WriteConfidenceCellForTests(sheet, dataRow, Layout12.ConfidenceCol, confidencePercent);
        }

        /// <summary>Smoke: заливка поля по уровню на листе 1.2↔1.6.</summary>
        public static void ApplyFieldLevelFill12ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            int startCol,
            Pairing12To16Field field,
            Shared.FieldMatchLevel level)
        {
            if (GetForm12FieldOffset(field) is not int offset)
            {
                return;
            }

            ApplyPairingComparisonCellFill(sheet, dataRow, startCol + offset, level);
        }

        /// <summary>Smoke: маркеры слева/справа и жирный % схожести на листе 1.3↔1.6.</summary>
        public static void WriteUnpairedSmokeRow13ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            string sourceMarker,
            string closestMarker,
            int confidencePercent)
        {
            var offset = GetForm13FieldOffset(Pairing13To16Field.DocumentNumber)
                         ?? throw new InvalidOperationException("Нет колонки номера документа.");
            sheet.Cells[dataRow, 1 + offset].Value = sourceMarker;
            sheet.Cells[dataRow, Layout13.ClosestStartCol + offset].Value = closestMarker;
            WriteConfidenceCellForTests(sheet, dataRow, Layout13.ConfidenceCol, confidencePercent);
        }

        /// <summary>Smoke: заливка поля по уровню на листе 1.3↔1.6.</summary>
        public static void ApplyFieldLevelFill13ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            int startCol,
            Pairing13To16Field field,
            Shared.FieldMatchLevel level)
        {
            if (GetForm13FieldOffset(field) is not int offset)
            {
                return;
            }

            ApplyPairingComparisonCellFill(sheet, dataRow, startCol + offset, level);
        }

        /// <summary>Smoke: маркеры слева/справа и жирный % схожести на листе 1.4↔1.6.</summary>
        public static void WriteUnpairedSmokeRow14ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            string sourceMarker,
            string closestMarker,
            int confidencePercent)
        {
            var offset = GetForm14FieldOffset(Pairing14To16Field.DocumentNumber)
                         ?? throw new InvalidOperationException("Нет колонки номера документа.");
            sheet.Cells[dataRow, 1 + offset].Value = sourceMarker;
            sheet.Cells[dataRow, Layout14.ClosestStartCol + offset].Value = closestMarker;
            WriteConfidenceCellForTests(sheet, dataRow, Layout14.ConfidenceCol, confidencePercent);
        }

        /// <summary>Smoke: заливка поля по уровню на листе 1.4↔1.6.</summary>
        public static void ApplyFieldLevelFill14ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            int startCol,
            Pairing14To16Field field,
            Shared.FieldMatchLevel level)
        {
            if (GetForm14FieldOffset(field) is not int offset)
            {
                return;
            }

            ApplyPairingComparisonCellFill(sheet, dataRow, startCol + offset, level);
        }

        /// <summary>Smoke: маркеры слева/справа и жирный % схожести на листе 1.6.</summary>
        public static void WriteUnpairedSmokeRow16ForTests(
            ExcelWorksheet sheet,
            int dataRow,
            string sourceMarker,
            string closestMarker,
            int confidencePercent)
        {
            var offset = Layout16DocumentNumberOffsetForTests;
            sheet.Cells[dataRow, 1 + offset].Value = sourceMarker;
            sheet.Cells[dataRow, Layout16.ClosestStartCol + offset].Value = closestMarker;
            WriteConfidenceCellForTests(sheet, dataRow, Layout16.ConfidenceCol, confidencePercent);
        }

        private static void WriteConfidenceCellForTests(
            ExcelWorksheet sheet,
            int dataRow,
            int confidenceCol,
            int confidencePercent)
        {
            var cell = sheet.Cells[dataRow, confidenceCol];
            cell.Value = confidencePercent;
            cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.Fill.SetBackground(ConfidenceFill(confidencePercent), OfficeOpenXml.Style.ExcelFillStyle.Solid);
        }

        public static void ResetRDictionaryForTests() => ResetRDictionaryCache();

        public static bool TryLoadRDictionaryFromFileForTests(string filePath, out string errorMessage) =>
            TryLoadRDictionaryFromFile(filePath, out errorMessage);

        public static bool TryLoadRDictionaryForTests(out string errorMessage) =>
            TryLoadRDictionary(out errorMessage);

        public static IReadOnlyDictionary<string, string> GetActivitiesForExportForTests(
            string? radionuclids,
            string? activityRaw) =>
            GetActivitiesForExport(radionuclids, activityRaw);

        public static Shared.FieldMatchLevel AggregateStateMatchLevelForTests(bool? matchesCodeRao) =>
            matchesCodeRao switch
            {
                true => Shared.FieldMatchLevel.Exact,
                false => Shared.FieldMatchLevel.Mismatch,
                _ => Shared.FieldMatchLevel.Mismatch
            };

        public static System.Drawing.Color ConfidenceFillForTests(int percent) => ConfidenceFill(percent);

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
            var source = ToDtoList(form11, "1.1");
            var reference = ToDtoList(form15, "1.5");
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
            return GetUnpairedOperations11To15(ToDtoList(source, "1.1"), ToDtoList(reference, "1.5"), options)
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
            return GetUnpairedOperations12To16(ToDtoList(source, "1.2"), ToDtoList(reference, "1.6"), options)
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
                    ToDtoList(form16, "1.6"),
                    ToDtoList(form12, "1.2"),
                    ToDtoList(form13, "1.3"),
                    ToDtoList(form14, "1.4"),
                    pairing12To16Params,
                    pairing13To16Params,
                    pairing14To16Params)
                .Select(row => row.Id)
                .OrderBy(id => id)
                .ToList();
        }

        public static Pairing41ScenarioResult RunScenario(Pairing41TestCase testCase)
        {
            var form11 = ToDtoList(testCase.Form11, "1.1");
            var form12 = ToDtoList(testCase.Form12, "1.2");
            var form13 = ToDtoList(testCase.Form13, "1.3");
            var form14 = ToDtoList(testCase.Form14, "1.4");
            var form15 = ToDtoList(testCase.Form15, "1.5");
            var form16 = ToDtoList(testCase.Form16, "1.6");

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
            var form11 = ToDtoList(testCase.Form11, "1.1");
            var form12 = ToDtoList(testCase.Form12, "1.2");
            var form13 = ToDtoList(testCase.Form13, "1.3");
            var form14 = ToDtoList(testCase.Form14, "1.4");
            var form15 = ToDtoList(testCase.Form15, "1.5");
            var form16 = ToDtoList(testCase.Form16, "1.6");

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
            var closest11Levels = closest11Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing11To15Field, Shared.FieldMatchLevel>)kv.Value.FieldLevels);
            var closest11Confidence = closest11Raw.ToDictionary(kv => kv.Key, kv => kv.Value.ConfidencePercent);
            var closest11Candidates = closest11Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest15Raw = BuildClosestMatchHighlights(unpaired.Form15, form11, testCase.Params11To15);
            var closest15 = closest15Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing11To15Field, bool>)kv.Value.FieldMatches);
            var closest15Levels = closest15Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing11To15Field, Shared.FieldMatchLevel>)kv.Value.FieldLevels);
            var closest15Confidence = closest15Raw.ToDictionary(kv => kv.Key, kv => kv.Value.ConfidencePercent);
            var closest15Candidates = closest15Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest12Raw = BuildClosestMatchHighlights12To16(unpaired.Form12, form16, testCase.Params12To16);
            var closest12 = closest12Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing12To16Field, bool>)kv.Value.FieldMatches);
            var closest12Levels = closest12Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing12To16Field, Shared.FieldMatchLevel>)kv.Value.FieldLevels);
            var closest12Confidence = closest12Raw.ToDictionary(kv => kv.Key, kv => kv.Value.ConfidencePercent);
            var closest12Candidates = closest12Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest13Raw = BuildClosestMatchHighlights13To16(unpaired.Form13, form16, testCase.Params13To16);
            var closest13 = closest13Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing13To16Field, bool>)kv.Value.FieldMatches);
            var closest13Levels = closest13Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing13To16Field, Shared.FieldMatchLevel>)kv.Value.FieldLevels);
            var closest13Confidence = closest13Raw.ToDictionary(kv => kv.Key, kv => kv.Value.ConfidencePercent);
            var closest13AggregateStateMatch = closest13Raw
                .ToDictionary(kv => kv.Key, kv => kv.Value.AggregateStateMatchesCodeRao);
            var closest13Candidates = closest13Raw.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            var closest14Raw = BuildClosestMatchHighlights14To16(unpaired.Form14, form16, testCase.Params14To16);
            var closest14 = closest14Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing14To16Field, bool>)kv.Value.FieldMatches);
            var closest14Levels = closest14Raw
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<Pairing14To16Field, Shared.FieldMatchLevel>)kv.Value.FieldLevels);
            var closest14Confidence = closest14Raw.ToDictionary(kv => kv.Key, kv => kv.Value.ConfidencePercent);
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
                closest11Levels, closest15Levels, closest12Levels, closest13Levels, closest14Levels,
                closest11Confidence, closest15Confidence, closest12Confidence, closest13Confidence, closest14Confidence,
                closest11Candidates, closest15Candidates, closest12Candidates,
                closest13Candidates, closest14Candidates, closest16Candidates);
        }

        private static List<Operation41PairingDto> ToDtoList(IEnumerable<Pairing41Row> rows, string defaultFormNum) =>
            rows.Select(row => ToDto(row, defaultFormNum)).ToList();

        public static Shared.FieldMatchLevel SimilarityLevel11To15ForTests(
            Pairing11To15Field field,
            Pairing41Row left,
            Pairing41Row right)
        {
            var leftDto = ToDto(left, string.IsNullOrEmpty(left.FormNum) ? "1.1" : left.FormNum);
            var rightDto = ToDto(right, string.IsNullOrEmpty(right.FormNum) ? "1.5" : right.FormNum);
            var leftNorm = CreatePairingNorm(leftDto);
            var rightNorm = CreatePairingNorm(rightDto);
            return FieldSimilarity11To15(leftDto, rightDto, leftNorm, rightNorm, field).Level;
        }

        public static Shared.FieldMatchLevel SimilarityLevel12To16ForTests(
            Pairing12To16Field field,
            Pairing41Row left,
            Pairing41Row right)
        {
            var leftDto = ToDto(left, string.IsNullOrEmpty(left.FormNum) ? "1.2" : left.FormNum);
            var rightDto = ToDto(right, string.IsNullOrEmpty(right.FormNum) ? "1.6" : right.FormNum);
            var leftNorm = CreatePairingNorm(leftDto);
            var rightNorm = CreatePairingNorm(rightDto);
            return FieldSimilarity12To16(leftDto, rightDto, leftNorm, rightNorm, field).Level;
        }

        public static Shared.FieldMatchLevel SimilarityLevel13To16ForTests(
            Pairing13To16Field field,
            Pairing41Row left,
            Pairing41Row right)
        {
            var leftDto = ToDto(left, string.IsNullOrEmpty(left.FormNum) ? "1.3" : left.FormNum);
            var rightDto = ToDto(right, string.IsNullOrEmpty(right.FormNum) ? "1.6" : right.FormNum);
            var leftNorm = CreatePairingNorm(leftDto);
            var rightNorm = CreatePairingNorm(rightDto);
            return FieldSimilarity13To16(leftDto, rightDto, leftNorm, rightNorm, field).Level;
        }

        public static Shared.FieldMatchLevel SimilarityLevel14To16ForTests(
            Pairing14To16Field field,
            Pairing41Row left,
            Pairing41Row right)
        {
            var leftDto = ToDto(left, string.IsNullOrEmpty(left.FormNum) ? "1.4" : left.FormNum);
            var rightDto = ToDto(right, string.IsNullOrEmpty(right.FormNum) ? "1.6" : right.FormNum);
            var leftNorm = CreatePairingNorm(leftDto);
            var rightNorm = CreatePairingNorm(rightDto);
            return FieldSimilarity14To16(leftDto, rightDto, leftNorm, rightNorm, field).Level;
        }

        private static Operation41PairingDto ToDto(Pairing41Row row, string defaultFormNum)
        {
            var formNum = string.IsNullOrEmpty(row.FormNum) ? defaultFormNum : row.FormNum;
            return new Operation41PairingDto
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
            FormNum = formNum,
            CodeRao = ResolveCodeRaoForTestRow(row, formNum),
            AggregateState = row.AggregateState,
            OrgRegNo = row.OrgRegNo
            };
        }

        private static string ResolveCodeRaoForTestRow(Pairing41Row row, string formNum)
        {
            if (!string.IsNullOrEmpty(row.CodeRao))
            {
                return row.CodeRao;
            }

            if (formNum is "1.2" or "1.3" or "1.4")
            {
                return RaoCodeHelper.GetCalculatedCodeRaoTemplate(
                    formNum, row.CodeRao, row.Radionuclids, row.MainRadionuclids, row.AggregateState);
            }

            if (formNum != "1.6")
            {
                return string.Empty;
            }

            var template = formNum switch
            {
                _ when !string.IsNullOrEmpty(row.Volume) && row.Volume != "-" =>
                    RaoCodeHelper.GetCalculatedCodeRaoTemplate(
                        "1.4", null, row.MainRadionuclids, row.MainRadionuclids, row.AggregateState),
                _ when !string.IsNullOrEmpty(row.MainRadionuclids) =>
                    RaoCodeHelper.GetCalculatedCodeRaoTemplate(
                        "1.3", null, row.MainRadionuclids, row.MainRadionuclids, row.AggregateState),
                _ => RaoCodeHelper.Form12CodeRao
            };

            return RaoCodeHelper.ExpandCalculatedTemplateToFull(template);
        }
    }

    #endregion
}
