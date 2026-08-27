using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using Client_App.Resources.CustomComparers.SnkComparers;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Test access

    /// <summary>
    /// Доступ к private-логике сверки приёма-передачи для unit-тестов (без БД/UI/Excel).
    /// </summary>
    public static class TransferReceiveTestAccess
    {
        public static TransferReceiveFormParams DefaultForm13ParamsForTests() => DefaultForm13Params();

        public static TransferReceiveFormParams DefaultForm12ParamsForTests() => DefaultForm12Params();

        public static TransferReceiveFormParams DefaultForm14ParamsForTests() => DefaultForm14Params();

        public static TransferReceiveFormParams DefaultForm15ParamsForTests() => DefaultForm15Params();

        public static TransferReceiveFormParams DefaultForm16ParamsForTests() => DefaultForm16Params();

        public static TransferReceiveParamsSet MapParamsFromDialogVmForTests(
            Client_App.ViewModels.Messages.GetTransferReceiveParamsVM vm) =>
            MapParamsFromDialogVm(vm);

        public static IReadOnlyList<string> BuildOurOkpoSqlMatchVariantsForTests(string ourOkpoRaw) =>
            BuildOurOkpoSqlMatchVariants(ourOkpoRaw);

        public static bool CounterpartProviderPointsToUsForTests(string? providerRaw, string? ourOkpoRaw) =>
            CounterpartProviderPointsToUs(providerRaw, ourOkpoRaw);

        public static bool OkpoReferencesMatchForTests(string? claimedRaw, string? targetRaw) =>
            OkpoReferencesMatch(claimedRaw, targetRaw);

        public static FieldMatchLevel SimilarityProviderOkpoLevelForTests(
            string? candidateProviderRaw,
            string? sourceOrgRaw,
            string? sourceOrgOkpoRaw) =>
            SimilarityProviderOkpo(candidateProviderRaw, sourceOrgRaw, sourceOrgOkpoRaw).Level;

        public static double SimilarityProviderOkpoScoreForTests(
            string? candidateProviderRaw,
            string? sourceOrgRaw,
            string? sourceOrgOkpoRaw) =>
            SimilarityProviderOkpo(candidateProviderRaw, sourceOrgRaw, sourceOrgOkpoRaw).Score;

        /// <summary>Сид ОКПО→RepsId из титулов (как в whole-DB до SQL-хвоста).</summary>
        public static IReadOnlyDictionary<string, IReadOnlyList<int>> SeedOkpoAliasMapFromTitlesForTests(
            IReadOnlyDictionary<int, string> repsIdToOkpo)
        {
            var titles = repsIdToOkpo.ToDictionary(
                kv => kv.Key,
                kv => new OrgTitleInfo(RegNo: string.Empty, Okpo: kv.Value, ShortName: string.Empty));
            return SeedOkpoAliasMapFromTitles(titles)
                .ToDictionary(
                    kv => kv.Key,
                    kv => (IReadOnlyList<int>)kv.Value,
                    StringComparer.Ordinal);
        }

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

        public static bool MassMatchesForTests(string? leftMass, string? rightMass, bool checkMass = true) =>
            MassMatches(
                new TransferReceiveDto { Mass = leftMass ?? string.Empty, OpCode = "21", IsTransfer = true },
                new TransferReceiveDto { Mass = rightMass ?? string.Empty, OpCode = "31", IsTransfer = false },
                checkMass);

        public static bool VolumeMatchesForTests(string? leftVolume, string? rightVolume, bool checkVolume = true) =>
            VolumeMatches(
                new TransferReceiveDto { Volume = leftVolume ?? string.Empty, OpCode = "21", IsTransfer = true },
                new TransferReceiveDto { Volume = rightVolume ?? string.Empty, OpCode = "31", IsTransfer = false },
                checkVolume);

        public static bool SubsidyMatchesForTests(string? leftSubsidy, string? rightSubsidy, bool checkSubsidy = true) =>
            SubsidyMatches(
                new TransferReceiveDto { Subsidy = leftSubsidy ?? string.Empty, OpCode = "21", IsTransfer = true },
                new TransferReceiveDto { Subsidy = rightSubsidy ?? string.Empty, OpCode = "31", IsTransfer = false },
                checkSubsidy);

        public static void CreateLegendSheetForTests(OfficeOpenXml.ExcelPackage excelPackage) =>
            CreateLegendSheet(excelPackage);

        /// <summary>Создаёт лист «Форма 1.1» с заголовками полей (smoke Excel-layout).</summary>
        public static void CreateForm11SheetForTests(OfficeOpenXml.ExcelPackage excelPackage)
        {
            var sheet = excelPackage.Workbook.Worksheets.Add("Форма 1.1");
            var closestStart = ClosestStartColFor(TransferReceiveSheetLayout.Form11);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: 1, TransferReceiveSheetLayout.Form11);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: closestStart, TransferReceiveSheetLayout.Form11);
            sheet.Cells[1, 1].Value = "Непарная операция выбранной организации";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form11)].Value = "Схожесть, %";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form11)].Style.Font.Bold = true;
        }

        /// <summary>Создаёт лист «Форма 1.2» с заголовками полей (smoke Excel-layout).</summary>
        public static void CreateForm12SheetForTests(OfficeOpenXml.ExcelPackage excelPackage)
        {
            var sheet = excelPackage.Workbook.Worksheets.Add("Форма 1.2");
            var closestStart = ClosestStartColFor(TransferReceiveSheetLayout.Form12);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: 1, TransferReceiveSheetLayout.Form12);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: closestStart, TransferReceiveSheetLayout.Form12);
            sheet.Cells[1, 1].Value = "Непарная операция выбранной организации";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form12)].Value = "Схожесть, %";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form12)].Style.Font.Bold = true;
        }

        /// <summary>Создаёт лист «Форма 1.3» с заголовками полей (smoke Excel-layout).</summary>
        public static void CreateForm13SheetForTests(OfficeOpenXml.ExcelPackage excelPackage)
        {
            var sheet = excelPackage.Workbook.Worksheets.Add("Форма 1.3");
            var closestStart = ClosestStartColFor(TransferReceiveSheetLayout.Form13);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: 1, TransferReceiveSheetLayout.Form13);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: closestStart, TransferReceiveSheetLayout.Form13);
            sheet.Cells[1, 1].Value = "Непарная операция выбранной организации";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form13)].Value = "Схожесть, %";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form13)].Style.Font.Bold = true;
        }

        /// <summary>Создаёт лист «Форма 1.6» с заголовками полей.</summary>
        public static void CreateForm16SheetForTests(OfficeOpenXml.ExcelPackage excelPackage)
        {
            var sheet = excelPackage.Workbook.Worksheets.Add("Форма 1.6");
            var closestStart = ClosestStartColFor(TransferReceiveSheetLayout.Form16);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: 1, TransferReceiveSheetLayout.Form16);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: closestStart, TransferReceiveSheetLayout.Form16);
            sheet.Cells[1, 1].Value = "Непарная операция выбранной организации";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form16)].Value = "Схожесть, %";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form16)].Style.Font.Bold = true;
        }

        /// <summary>Создаёт лист «Форма 1.5» с заголовками (как 1.1 без ОКПО изготовителя).</summary>
        public static void CreateForm15SheetForTests(OfficeOpenXml.ExcelPackage excelPackage)
        {
            var sheet = excelPackage.Workbook.Worksheets.Add("Форма 1.5");
            var closestStart = ClosestStartColFor(TransferReceiveSheetLayout.Form15);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: 1, TransferReceiveSheetLayout.Form15);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: closestStart, TransferReceiveSheetLayout.Form15);
            sheet.Cells[1, 1].Value = "Непарная операция выбранной организации";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form15)].Value = "Схожесть, %";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form15)].Style.Font.Bold = true;
        }

        /// <summary>
        /// Unpaired для одной формы, если она включена в <paramref name="pairingParams"/> (иначе пусто).
        /// </summary>
        public static IReadOnlyList<int> AnalyzeFormUnpairedIfEnabledForTests(
            TransferReceiveFormId formId,
            string ourOkpo,
            IReadOnlyList<TransferReceiveRow> ourOps,
            IReadOnlyList<TransferReceiveRow> counterpartOps,
            TransferReceiveParamsSet pairingParams)
        {
            if (!pairingParams.IsEnabled(formId) || ourOps.Count == 0)
            {
                return [];
            }

            var formNum = GetFormDescriptor(formId).FormNum;
            var options = pairingParams.GetParams(formId);
            var our = ToDtoList(ourOps, ourOkpo, formNum);
            var counterpart = ToDtoList(counterpartOps, null, formNum);
            var (unpaired, _) = AnalyzeFormForOrganization(our, counterpart, ourOkpo, options, null);
            return unpaired.Select(op => op.Id).OrderBy(id => id).ToList();
        }

        /// <summary>
        /// Пишет source и closest через тот же WriteOperationBlock, что и выгрузка.
        /// Подсветка — по переданной карте (на обеих сторонах).
        /// </summary>
        public static void WriteOperationRowPairForTests(
            OfficeOpenXml.ExcelWorksheet sheet,
            TransferReceiveSheetLayout layout,
            int dataRow,
            TransferReceiveRow source,
            TransferReceiveRow closest,
            IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>? fieldLevels = null)
        {
            var formNum = FormNumForLayout(layout);
            var sourceDto = ToDto(source, fallbackOrgOkpo: source.OrgOkpo, formNum);
            var closestDto = ToDto(closest, fallbackOrgOkpo: closest.OrgOkpo, formNum);
            var levels = fieldLevels ?? new Dictionary<TransferReceiveField, FieldMatchLevel>();
            var exact = levels.ToDictionary(kv => kv.Key, kv => kv.Value == FieldMatchLevel.Exact);
            var highlight = new ClosestMatchResult(closestDto, levels, exact, confidencePercent: 80, rawScore: 1);
            var closestMatches = new Dictionary<int, ClosestMatchResult> { [sourceDto.Id] = highlight };

            WriteOperationBlock(
                sheet, dataRow, sourceDto, startCol: 1, applyHighlight: levels.Count > 0, isSource: true,
                closestMatches, layout);
            WriteOperationBlock(
                sheet, dataRow, closestDto, startCol: ClosestStartColFor(layout),
                applyHighlight: levels.Count > 0, isSource: false,
                closestMatches, layout, highlight);
        }

        public static string FormNumForLayoutForTests(TransferReceiveSheetLayout layout) =>
            FormNumForLayout(layout);

        public static string PairingExactFillRgbForTests => "FFC6EFCE";

        public static string PairingMismatchFillRgbForTests => "FFFFCDD2";

        private static string FormNumForLayout(TransferReceiveSheetLayout layout) =>
            layout switch
            {
                TransferReceiveSheetLayout.Form12 => "1.2",
                TransferReceiveSheetLayout.Form13 => "1.3",
                TransferReceiveSheetLayout.Form14 => "1.4",
                TransferReceiveSheetLayout.Form15 => "1.5",
                TransferReceiveSheetLayout.Form16 => "1.6",
                _ => "1.1"
            };

        /// <summary>Создаёт лист «Форма 1.4» с заголовками полей.</summary>
        public static void CreateForm14SheetForTests(OfficeOpenXml.ExcelPackage excelPackage)
        {
            var sheet = excelPackage.Workbook.Worksheets.Add("Форма 1.4");
            var closestStart = ClosestStartColFor(TransferReceiveSheetLayout.Form14);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: 1, TransferReceiveSheetLayout.Form14);
            WriteFieldHeadersToSheet(sheet, row: 2, startCol: closestStart, TransferReceiveSheetLayout.Form14);
            sheet.Cells[1, 1].Value = "Непарная операция выбранной организации";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form14)].Value = "Схожесть, %";
            sheet.Cells[2, ConfidenceColFor(TransferReceiveSheetLayout.Form14)].Style.Font.Bold = true;
        }

        /// <summary>
        /// Smoke записи строки: код операции слева, closest справа, жирный % схожести.
        /// </summary>
        public static void WriteUnpairedSmokeRowForTests(
            OfficeOpenXml.ExcelWorksheet sheet,
            TransferReceiveSheetLayout layout,
            int dataRow,
            string sourceOpCode,
            string closestOpCode,
            int confidencePercent)
        {
            var opCodeOffset = GetComparableColumnOffset(TransferReceiveField.OperationCode, layout)
                               ?? throw new InvalidOperationException("Нет колонки кода операции.");
            var confCol = ConfidenceColFor(layout);
            var closestStart = ClosestStartColFor(layout);
            sheet.Cells[dataRow, 1 + opCodeOffset].Value = sourceOpCode;
            sheet.Cells[dataRow, closestStart + opCodeOffset].Value = closestOpCode;
            var cell = sheet.Cells[dataRow, confCol];
            cell.Value = confidencePercent;
            cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.Fill.SetBackground(ConfidenceFill(confidencePercent), OfficeOpenXml.Style.ExcelFillStyle.Solid);
        }

        public static int SourceColCountForTests => SourceColCount;

        public static int SeparatorColForTests => SeparatorCol;

        public static int ConfidenceColForTests => ConfidenceCol;

        public static int ClosestStartColForTests => ClosestStartCol;

        public static int TotalColCountForTests => TotalColCount;

        public static int SourceColCountForLayoutForTests(TransferReceiveSheetLayout layout) =>
            SourceColCountFor(layout);

        public static int ClosestStartColForLayoutForTests(TransferReceiveSheetLayout layout) =>
            ClosestStartColFor(layout);

        public static int TotalColCountForLayoutForTests(TransferReceiveSheetLayout layout) =>
            TotalColCountFor(layout);

        public static int? GetComparableColumnOffsetForTests(
            TransferReceiveField field,
            TransferReceiveSheetLayout layout = TransferReceiveSheetLayout.Form11) =>
            GetComparableColumnOffset(field, layout);

        public static int GetNextDataRowForTests(OfficeOpenXml.ExcelWorksheet sheet) => GetNextDataRow(sheet);

        /// <summary>Порядок Id после OrderForExport (RegNo → периоды → № п/п → Id).</summary>
        public static IReadOnlyList<int> OrderForExportIdsForTests(
            IEnumerable<(int Id, string OrgRegNo, string StartPeriod, string EndPeriod, int NumberInOrder)> rows) =>
            OrderForExport(rows.Select(r => new TransferReceiveDto
                {
                    Id = r.Id,
                    NumberInOrder = r.NumberInOrder,
                    OrgRegNo = r.OrgRegNo,
                    StartPeriod = r.StartPeriod,
                    EndPeriod = r.EndPeriod
                }).ToList())
                .Select(op => op.Id)
                .ToList();

        public static string FormatMultiOrgExcelStageForTests(
            int orgIndex, int orgCount, string stage, string? orgLabel = null) =>
            FormatOrgExcelStage(orgIndex, orgCount, stage, orgLabel);

        public static TransferReceiveScenarioResult RunScenario(TransferReceiveTestCase testCase)
        {
            var ourOps = ToDtoList(testCase.OurOps, fallbackOrgOkpo: testCase.OurOkpo, testCase.FormNum);
            var counterpartOps = ToDtoList(testCase.CounterpartOps, fallbackOrgOkpo: null, testCase.FormNum);
            var aliases = ToAliasMap(testCase.OkpoAliases);

            var (unpaired, _) = AnalyzeFormForOrganization(
                ourOps, counterpartOps, testCase.OurOkpo, testCase.Params, aliases);

            return new TransferReceiveScenarioResult(
                unpaired.Select(row => row.Id).OrderBy(id => id).ToList());
        }

        /// <summary>
        /// Как whole-DB: один общий пул по всем ops, затем только ComputeUnpaired по ourOps.
        /// </summary>
        public static TransferReceiveScenarioResult RunScenarioWithSharedFullPool(TransferReceiveTestCase testCase)
        {
            var ourOps = ToDtoList(testCase.OurOps, fallbackOrgOkpo: testCase.OurOkpo, testCase.FormNum);
            var counterpartOps = ToDtoList(testCase.CounterpartOps, fallbackOrgOkpo: null, testCase.FormNum);
            var aliases = ToAliasMap(testCase.OkpoAliases);

            var ourIds = ourOps.Select(op => op.Id).ToHashSet();
            var allOps = ourOps
                .Concat(counterpartOps.Where(op => !ourIds.Contains(op.Id)))
                .ToList();
            var sharedPool = BuildOpsPoolByOrgOkpo([], allOps, aliases);
            var indexes = SharedFormSearchIndexes.Build(sharedPool, testCase.Params);
            var unpaired = ComputeUnpairedOperations(
                ourOps, sharedPool, testCase.OurOkpo, testCase.Params, prebuiltIndex: indexes.Pairing);

            return new TransferReceiveScenarioResult(
                unpaired.Select(row => row.Id).OrderBy(id => id).ToList());
        }

        /// <summary>
        /// Как whole-DB после оптимизации: shared pool + заранее построенные индексы pairing/closest.
        /// </summary>
        public static TransferReceiveClosestMatchResult RunClosestMatchesWithSharedIndexes(
            TransferReceiveTestCase testCase)
        {
            var ourOps = ToDtoList(testCase.OurOps, fallbackOrgOkpo: testCase.OurOkpo, testCase.FormNum);
            var counterpartOps = ToDtoList(testCase.CounterpartOps, fallbackOrgOkpo: null, testCase.FormNum);
            var aliases = ToAliasMap(testCase.OkpoAliases);

            var ourIds = ourOps.Select(op => op.Id).ToHashSet();
            var allOps = ourOps
                .Concat(counterpartOps.Where(op => !ourIds.Contains(op.Id)))
                .ToList();
            var sharedPool = BuildOpsPoolByOrgOkpo([], allOps, aliases);
            var indexes = SharedFormSearchIndexes.Build(sharedPool, testCase.Params);
            var unpaired = ComputeUnpairedOperations(
                ourOps, sharedPool, testCase.OurOkpo, testCase.Params, prebuiltIndex: indexes.Pairing);
            var built = BuildClosestMatchResults(
                unpaired, sharedPool, testCase.Params,
                prebuiltCandidateIndex: indexes.Closest,
                prebuiltNorms: indexes.Norms,
                layout: LayoutForFormNum(testCase.FormNum));

            var exact = built.ToDictionary(
                kv => kv.Key,
                kv => (IReadOnlyDictionary<TransferReceiveField, bool>)kv.Value.FieldMatches);
            var levels = built.ToDictionary(
                kv => kv.Key,
                kv => (IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>)kv.Value.FieldLevels);
            var confidence = built.ToDictionary(kv => kv.Key, kv => kv.Value.ConfidencePercent);
            var candidateIds = built.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            return new TransferReceiveClosestMatchResult(exact, levels, confidence, candidateIds);
        }

        public static TransferReceiveClosestMatchResult RunClosestMatches(TransferReceiveTestCase testCase)
        {
            var ourOps = ToDtoList(testCase.OurOps, fallbackOrgOkpo: testCase.OurOkpo, testCase.FormNum);
            var counterpartOps = ToDtoList(testCase.CounterpartOps, fallbackOrgOkpo: null, testCase.FormNum);
            var aliases = ToAliasMap(testCase.OkpoAliases);

            var (unpaired, opsByOrgOkpo) = AnalyzeFormForOrganization(
                ourOps, counterpartOps, testCase.OurOkpo, testCase.Params, aliases);

            var built = BuildClosestMatchResults(
                unpaired, opsByOrgOkpo, testCase.Params, layout: LayoutForFormNum(testCase.FormNum));
            var exact = built.ToDictionary(
                kv => kv.Key,
                kv => (IReadOnlyDictionary<TransferReceiveField, bool>)kv.Value.FieldMatches);
            var levels = built.ToDictionary(
                kv => kv.Key,
                kv => (IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>)kv.Value.FieldLevels);
            var confidence = built.ToDictionary(kv => kv.Key, kv => kv.Value.ConfidencePercent);
            var candidateIds = built.ToDictionary(kv => kv.Key, kv => kv.Value.Candidate.Id);

            return new TransferReceiveClosestMatchResult(exact, levels, confidence, candidateIds);
        }

        public static FieldMatchLevel SimilarityLevelForTests(
            TransferReceiveField field,
            TransferReceiveRow left,
            TransferReceiveRow right,
            string ourOkpo = "10000001")
        {
            var leftDto = ToDto(left, ourOkpo, "1.1");
            var rightDto = ToDto(right, null, "1.1");
            var leftNorm = CreateNorm(leftDto);
            var rightNorm = CreateNorm(rightDto);
            return FieldSimilarityOf(leftDto, rightDto, leftNorm, rightNorm, field, ourOkpo).Level;
        }

        /// <summary>
        /// Включённые формы с учётом <see cref="IsFormCheckEnabled"/> (как BuildOrganizationExportFromLoaded / SharedPools).
        /// </summary>
        public static (IReadOnlyList<int> Unpaired11, IReadOnlyList<int> Unpaired12, IReadOnlyList<int> Unpaired13)
            AnalyzeEnabledFormsUnpairedForTests(
            string ourOkpo,
            IReadOnlyList<TransferReceiveRow> ourOps11,
            IReadOnlyList<TransferReceiveRow> ourOps12,
            IReadOnlyList<TransferReceiveRow> ourOps13,
            IReadOnlyList<TransferReceiveRow> counterpartOps11,
            IReadOnlyList<TransferReceiveRow> counterpartOps12,
            IReadOnlyList<TransferReceiveRow> counterpartOps13,
            TransferReceiveParamsSet pairingParams)
        {
            var unpaired11 = new List<int>();
            var unpaired12 = new List<int>();
            var unpaired13 = new List<int>();

            if (pairingParams.IsEnabled(TransferReceiveFormId.Form11) && ourOps11.Count > 0)
            {
                var our = ToDtoList(ourOps11, ourOkpo, "1.1");
                var counterpart = ToDtoList(counterpartOps11, null, "1.1");
                var (unpaired, _) = AnalyzeFormForOrganization(
                    our, counterpart, ourOkpo, pairingParams.Form11, null);
                unpaired11 = unpaired.Select(op => op.Id).OrderBy(id => id).ToList();
            }

            if (pairingParams.IsEnabled(TransferReceiveFormId.Form12) && ourOps12.Count > 0)
            {
                var our = ToDtoList(ourOps12, ourOkpo, "1.2");
                var counterpart = ToDtoList(counterpartOps12, null, "1.2");
                var (unpaired, _) = AnalyzeFormForOrganization(
                    our, counterpart, ourOkpo, pairingParams.Form12, null);
                unpaired12 = unpaired.Select(op => op.Id).OrderBy(id => id).ToList();
            }

            if (pairingParams.IsEnabled(TransferReceiveFormId.Form13) && ourOps13.Count > 0)
            {
                var our = ToDtoList(ourOps13, ourOkpo, "1.3");
                var counterpart = ToDtoList(counterpartOps13, null, "1.3");
                var (unpaired, _) = AnalyzeFormForOrganization(
                    our, counterpart, ourOkpo, pairingParams.Form13, null);
                unpaired13 = unpaired.Select(op => op.Id).OrderBy(id => id).ToList();
            }

            return (unpaired11, unpaired12, unpaired13);
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
            var forceQtyOne = formNum is "1.2" or "1.3" or "1.4"
                              || row.AggregateState is not null
                              || row.Sort is not null;

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
                CodeRao = row.CodeRao,
                PackType = row.PackType,
                PackNumber = row.PackNumber,
                StatusRao = row.StatusRao,
                PackName = row.PackName,
                Subsidy = row.Subsidy,
                FcpNumber = row.FcpNumber,
                ProviderOrRecieverOkpo = row.ProviderOrRecieverOkpo,
                Activity = row.Activity,
                TritiumActivity = row.TritiumActivity,
                BetaGammaActivity = row.BetaGammaActivity,
                AlphaActivity = row.AlphaActivity,
                TransuraniumActivity = row.TransuraniumActivity,
                Mass = row.Mass,
                Volume = row.Volume,
                ActivityMeasurementDate = row.ActivityMeasurementDate,
                Sort = row.Sort,
                CreatorOkpo = row.CreatorOkpo,
                CreationDate = row.CreationDate,
                Quantity = formNum == "1.6"
                    ? row.Quantity
                    : forceQtyOne ? 1 : row.Quantity,
                AggregateState = row.AggregateState,
                IsTransfer = isTransfer
            };
        }

        private static TransferReceiveSheetLayout LayoutForFormNum(string formNum) =>
            formNum switch
            {
                "1.2" => TransferReceiveSheetLayout.Form12,
                "1.3" => TransferReceiveSheetLayout.Form13,
                "1.4" => TransferReceiveSheetLayout.Form14,
                "1.5" => TransferReceiveSheetLayout.Form15,
                "1.6" => TransferReceiveSheetLayout.Form16,
                _ => TransferReceiveSheetLayout.Form11
            };
    }

    #endregion
}
