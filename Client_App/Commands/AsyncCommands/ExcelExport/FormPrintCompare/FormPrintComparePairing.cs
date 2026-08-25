using System;

using System.Collections.Generic;

using System.Linq;



namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;



/// <summary>

/// Сопоставление отчётов: ведущая сторона — файлы сверки (по одному ряду сводки на отчёт из файла).

/// Отчёты только из текущей БД без пары в файлах в результат не попадают.

/// </summary>

internal static class FormPrintComparePairing

{

    public static List<ReportCompareResult> Pair(

        IReadOnlyList<CompareReportDto> sourceReports,

        IReadOnlyList<CompareReportDto> compareReports,

        Action<int, string>? progress = null,

        int progressStartPercent = 0,

        int progressSpanPercent = 100)

    {

        var safeSpan = Math.Max(1, progressSpanPercent);

        var phase1Span = Math.Max(1, safeSpan / 2);

        var phase2Span = Math.Max(1, safeSpan - phase1Span);



        progress?.Invoke(progressStartPercent, "сверка: построение индекса текущей БД");

        var sourceIndex = FormPrintRaodbIndex.ToIndex(sourceReports);

        var usedSourceKeys = new HashSet<ReportMatchKey>();

        var results = new List<ReportCompareResult>(compareReports.Count);

        var unmatchedCompare = new List<CompareReportDto>();



        var compareTotal = Math.Max(1, compareReports.Count);

        var compareDone = 0;

        foreach (var right in compareReports)

        {

            var key = ReportMatchKey.From(right);

            if (sourceIndex.TryGetValue(key, out var left))

            {

                results.Add(FormPrintCompareMatcher.Compare(left, right));

                usedSourceKeys.Add(key);

            }

            else

            {

                unmatchedCompare.Add(right);

            }



            compareDone++;

            if (compareDone == compareTotal || compareDone % 50 == 0)

            {

                var percent = progressStartPercent + (phase1Span * compareDone) / compareTotal;

                progress?.Invoke(

                    percent,

                    $"сверка: сопоставление с БД {compareDone}/{compareTotal}");

            }

        }



        var unusedSource = sourceReports

            .Where(r => !usedSourceKeys.Contains(ReportMatchKey.From(r)))

            .ToList();

        var usedSource = new HashSet<CompareReportDto>();



        var overlapTotal = Math.Max(1, unmatchedCompare.Count);

        var overlapDone = 0;

        foreach (var right in unmatchedCompare)

        {

            var best = FindBestOverlap(right, unusedSource, usedSource);

            if (best is null)

            {

                results.Add(MissingInSource(right));

            }

            else

            {

                usedSource.Add(best);

                results.Add(PeriodOverlap(best, right));

            }



            overlapDone++;

            if (overlapDone == overlapTotal || overlapDone % 50 == 0)

            {

                var percent = progressStartPercent + phase1Span + (phase2Span * overlapDone) / overlapTotal;

                progress?.Invoke(

                    percent,

                    $"сверка: проверка пересечений периодов {overlapDone}/{overlapTotal}");

            }

        }



        progress?.Invoke(progressStartPercent + safeSpan, "сверка: готово");

        return results;

    }



    internal static ReportCompareResult PeriodOverlap(CompareReportDto left, CompareReportDto right) =>

        new()

        {

            Kind = ReportCompareKind.PeriodOverlap,

            Left = left,

            Right = right,

            IsIdentical = false,

            OrderOnlyChanged = false,

            Lines = [],

            DisplayLines = [],

            Message =

                $"Период не совпадает, но пересекается с отчётом в текущей БД ({left.PeriodDisplay}).",

            UnchangedCount = 0,

            ChangedCount = 0,

            MovedCount = 0,

            DeletedCount = 0,

            AddedCount = 0

        };



    internal static ReportCompareResult MissingInSource(CompareReportDto right) =>

        new()

        {

            Kind = ReportCompareKind.MissingInSourceDb,

            Left = right,

            Right = right,

            IsIdentical = false,

            OrderOnlyChanged = false,

            Lines = [],

            DisplayLines = [],

            Message = "Отчёт из файла сверки отсутствует в текущей БД.",

            UnchangedCount = 0,

            ChangedCount = 0,

            MovedCount = 0,

            DeletedCount = 0,

            AddedCount = 0

        };



    /// <summary>Ищем в кандидатах (БД) лучшее пересечение периода с отчётом из файла.</summary>

    private static CompareReportDto? FindBestOverlap(

        CompareReportDto fromFile,

        List<CompareReportDto> sourceCandidates,

        HashSet<CompareReportDto> usedSource)

    {

        CompareReportDto? best = null;

        var bestDays = 0;

        var fileKey = ReportMatchKey.From(fromFile);

        foreach (var candidate in sourceCandidates)

        {

            if (usedSource.Contains(candidate))

            {

                continue;

            }



            var candidateKey = ReportMatchKey.From(candidate);

            if (!string.Equals(fileKey.RegNo, candidateKey.RegNo, StringComparison.Ordinal)

                || !string.Equals(fileKey.Okpo, candidateKey.Okpo, StringComparison.Ordinal))

            {

                continue;

            }



            if (!FormPrintCompareNormalize.PeriodsOverlap(fromFile, candidate))

            {

                continue;

            }



            var days = FormPrintCompareNormalize.OverlapDayCount(fromFile, candidate);

            if (days > bestDays)

            {

                bestDays = days;

                best = candidate;

            }

        }



        return best;

    }

}


