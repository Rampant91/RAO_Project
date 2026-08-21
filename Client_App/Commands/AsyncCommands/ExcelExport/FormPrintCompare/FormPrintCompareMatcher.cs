using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

internal static class FormPrintCompareMatcher
{
    private const double SoftMatchMinConfidence = 0.55;

    public static ReportCompareResult Compare(CompareReportDto left, CompareReportDto? right)
    {
        if (right is null)
        {
            return new ReportCompareResult
            {
                Left = left,
                Right = null,
                IsIdentical = false,
                OrderOnlyChanged = false,
                Lines = [],
                DisplayLines = [],
                Message = "Для отчёта отсутствует отчёт для сверки в выбранном .RAODB.",
                UnchangedCount = 0,
                ChangedCount = 0,
                MovedCount = 0,
                DeletedCount = 0,
                AddedCount = 0
            };
        }

        var columns = left.Columns;
        var leftRows = left.Rows;
        var rightRows = right.Rows;

        var matchedRight = new HashSet<int>();
        var pairs = new List<(CompareRowDto L, CompareRowDto R, FieldMatchLevel[] Levels)>();

        // Exact fingerprint 1:1 (prefer close SourceIndex).
        var leftByFp = leftRows
            .Where(r => !string.IsNullOrEmpty(r.Fingerprint))
            .GroupBy(r => r.Fingerprint)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.SourceIndex).ToList());
        var rightByFp = rightRows
            .Where(r => !string.IsNullOrEmpty(r.Fingerprint))
            .GroupBy(r => r.Fingerprint)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.SourceIndex).ToList());

        foreach (var (fp, leftGroup) in leftByFp)
        {
            if (!rightByFp.TryGetValue(fp, out var rightGroup))
            {
                continue;
            }

            var rightAvailable = rightGroup.Where(r => !matchedRight.Contains(r.Id)).ToList();
            foreach (var leftRow in leftGroup)
            {
                if (rightAvailable.Count == 0)
                {
                    break;
                }

                var best = rightAvailable
                    .OrderBy(r => Math.Abs(r.SourceIndex - leftRow.SourceIndex))
                    .ThenBy(r => r.Id)
                    .First();
                rightAvailable.Remove(best);
                matchedRight.Add(best.Id);
                pairs.Add((leftRow, best, ScoreFields(columns, leftRow, best)));
            }
        }

        var unmatchedLeft = leftRows
            .Where(r => pairs.All(p => p.L.Id != r.Id))
            .ToList();
        var unmatchedRight = rightRows
            .Where(r => !matchedRight.Contains(r.Id))
            .ToList();

        // Soft match: только если есть содержательный якорь (Exact по fingerprint-полю).
        foreach (var leftRow in unmatchedLeft.ToList())
        {
            if (unmatchedRight.Count == 0)
            {
                break;
            }

            CompareRowDto? best = null;
            FieldMatchLevel[]? bestLevels = null;
            var bestScore = -1.0;
            foreach (var rightRow in unmatchedRight)
            {
                if (string.IsNullOrEmpty(leftRow.Fingerprint) || string.IsNullOrEmpty(rightRow.Fingerprint))
                {
                    continue;
                }

                var levels = ScoreFields(columns, leftRow, rightRow);
                if (!HasFingerprintExactAnchor(columns, levels, leftRow, rightRow))
                {
                    continue;
                }

                var score = Confidence(columns, levels);
                if (score > bestScore
                    || (Math.Abs(score - bestScore) < 1e-9
                        && best is not null
                        && Math.Abs(rightRow.SourceIndex - leftRow.SourceIndex)
                        < Math.Abs(best.SourceIndex - leftRow.SourceIndex)))
                {
                    bestScore = score;
                    best = rightRow;
                    bestLevels = levels;
                }
            }

            if (best is null || bestLevels is null || bestScore < SoftMatchMinConfidence)
            {
                continue;
            }

            pairs.Add((leftRow, best, bestLevels));
            matchedRight.Add(best.Id);
            unmatchedLeft.Remove(leftRow);
            unmatchedRight.Remove(best);
        }

        var pairByLeftId = pairs.ToDictionary(p => p.L.Id);
        var added = unmatchedRight.OrderBy(r => r.SourceIndex).ToList();
        var deleted = unmatchedLeft.OrderBy(r => r.SourceIndex).ToList();

        var lines = BuildUnifiedLayout(columns, leftRows, pairByLeftId, added, deleted);

        var leftDuplicateNpps = FindDuplicateNpps(leftRows);
        var rightDuplicateNpps = FindDuplicateNpps(rightRows);
        var leftDupSet = leftDuplicateNpps.ToHashSet();
        var rightDupSet = rightDuplicateNpps.ToHashSet();
        lines = lines.Select(l => MarkNppDuplicates(l, leftDupSet, rightDupSet)).ToList();

        var unchanged = lines.Count(l => l.Status == DiffRowStatus.Unchanged);
        var changed = lines.Count(l => l.Status == DiffRowStatus.Changed);
        var moved = lines.Count(l => l.Status == DiffRowStatus.Moved);
        var deletedCount = lines.Count(l => l.Status == DiffRowStatus.Deleted);
        var addedCount = lines.Count(l => l.Status == DiffRowStatus.Added);

        var isIdentical = leftRows.Count == rightRows.Count
            && deletedCount == 0
            && addedCount == 0
            && changed == 0
            && moved == 0
            && (leftRows.Count == 0 || unchanged == leftRows.Count);

        var orderOnly = !isIdentical
            && changed == 0
            && deletedCount == 0
            && addedCount == 0
            && moved > 0;

        string? message = null;
        if (isIdentical)
        {
            message = "Отчёт не изменялся, полностью совпадает.";
        }
        else if (orderOnly)
        {
            message = BuildNppOrderMessage(leftDuplicateNpps, rightDuplicateNpps);
        }

        // Полностью совпавший отчёт — без листа; иначе все строки, включая «Без изменений».
        var displayLines = isIdentical ? [] : lines;

        return new ReportCompareResult
        {
            Left = left,
            Right = right,
            IsIdentical = isIdentical,
            OrderOnlyChanged = orderOnly,
            Lines = lines,
            DisplayLines = displayLines,
            Message = message,
            UnchangedCount = unchanged,
            ChangedCount = changed,
            MovedCount = moved,
            DeletedCount = deletedCount,
            AddedCount = addedCount,
            LeftDuplicateNpps = leftDuplicateNpps,
            RightDuplicateNpps = rightDuplicateNpps
        };
    }

    internal static List<int> FindDuplicateNpps(IEnumerable<CompareRowDto> rows) =>
        rows
            .GroupBy(r => r.NumberInOrder)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .OrderBy(n => n)
            .ToList();

    private static string BuildNppOrderMessage(
        IReadOnlyList<int> leftDups,
        IReadOnlyList<int> rightDups)
    {
        if (rightDups.Count > 0 && leftDups.Count == 0)
        {
            return "Содержимое строк совпало. В файле сравнения сбита нумерация № п/п (повторяются: "
                   + string.Join(", ", rightDups)
                   + ") — дальше номера идут со сдвигом относительно исходника.";
        }

        if (leftDups.Count > 0 && rightDups.Count == 0)
        {
            return "Содержимое строк совпало. В исходнике сбита нумерация № п/п (повторяются: "
                   + string.Join(", ", leftDups) + ").";
        }

        if (leftDups.Count > 0 && rightDups.Count > 0)
        {
            return "Содержимое строк совпало. Сбита нумерация № п/п с обеих сторон.";
        }

        return "Содержимое строк совпало, отличается только № п/п.";
    }

    private static DiffLine MarkNppDuplicates(
        DiffLine line,
        HashSet<int> leftDups,
        HashSet<int> rightDups)
    {
        var leftDup = line.Left is not null && leftDups.Contains(line.Left.NumberInOrder);
        var rightDup = line.Right is not null && rightDups.Contains(line.Right.NumberInOrder);
        if (!leftDup && !rightDup)
        {
            return line;
        }

        return new DiffLine
        {
            Status = line.Status,
            Left = line.Left,
            Right = line.Right,
            FieldLevels = line.FieldLevels,
            ConfidencePercent = line.ConfidencePercent,
            LeftNppDuplicate = leftDup,
            RightNppDuplicate = rightDup
        };
    }

    private static bool HasFingerprintExactAnchor(
        CompareColumn[] columns,
        FieldMatchLevel[] levels,
        CompareRowDto left,
        CompareRowDto right)
    {
        int? passportIndex = null;
        int? factoryIndex = null;
        int? radionuclidesIndex = null;

        for (var i = 0; i < columns.Length; i++)
        {
            if (!columns[i].InFingerprint)
            {
                continue;
            }

            if (columns[i].Kind == CompareColumnKind.Radionuclids)
            {
                radionuclidesIndex ??= i;
            }
            else if (columns[i].Kind == CompareColumnKind.Id)
            {
                if (columns[i].IsFactoryId)
                {
                    factoryIndex ??= i;
                }
                else
                {
                    passportIndex ??= i;
                }
            }
        }

        bool ExactNonEmpty(int index)
        {
            if (index >= levels.Length || levels[index] != FieldMatchLevel.Exact)
            {
                return false;
            }

            var leftVal = index < left.Values.Length ? left.Values[index] : "";
            return !string.IsNullOrEmpty(FormPrintCompareNormalize.FingerprintPart(columns[index], leftVal));
        }

        if (passportIndex is int pasIdx)
        {
            var leftPas = pasIdx < left.Values.Length ? left.Values[pasIdx] : "";
            var rightPas = pasIdx < right.Values.Length ? right.Values[pasIdx] : "";
            var leftNorm = FormPrintCompareNormalize.NormalizeId(leftPas);
            var rightNorm = FormPrintCompareNormalize.NormalizeId(rightPas);
            if (!string.IsNullOrEmpty(leftNorm) || !string.IsNullOrEmpty(rightNorm))
            {
                // Если паспорт заполнен хотя бы с одной стороны — soft только при Exact паспорта.
                return ExactNonEmpty(pasIdx);
            }
        }

        if (factoryIndex is int facIdx && ExactNonEmpty(facIdx))
        {
            return true;
        }

        return radionuclidesIndex is int radIdx && ExactNonEmpty(radIdx);
    }

    private static List<DiffLine> BuildUnifiedLayout(
        CompareColumn[] columns,
        List<CompareRowDto> leftRows,
        Dictionary<int, (CompareRowDto L, CompareRowDto R, FieldMatchLevel[] Levels)> pairByLeftId,
        List<CompareRowDto> added,
        List<CompareRowDto> deleted)
    {
        var lines = new List<DiffLine>();
        var deletedIds = deleted.Select(d => d.Id).ToHashSet();
        var addedRemaining = added.ToList();

        var anchors = leftRows
            .Where(l => pairByLeftId.ContainsKey(l.Id))
            .Select(l => pairByLeftId[l.Id])
            .OrderBy(p => p.L.SourceIndex)
            .ToList();

        var prevRightIndex = -1;
        var leftIndex = 0;

        void FlushAddedBefore(int rightIndexExclusive)
        {
            var batch = addedRemaining
                .Where(a => a.SourceIndex > prevRightIndex && a.SourceIndex < rightIndexExclusive)
                .OrderBy(a => a.SourceIndex)
                .ToList();
            foreach (var a in batch)
            {
                lines.Add(new DiffLine
                {
                    Status = DiffRowStatus.Added,
                    Right = a,
                    ConfidencePercent = 0
                });
                addedRemaining.Remove(a);
            }
        }

        while (leftIndex < leftRows.Count)
        {
            var leftRow = leftRows[leftIndex];
            if (pairByLeftId.TryGetValue(leftRow.Id, out var pair))
            {
                FlushAddedBefore(pair.R.SourceIndex);
                var status = ClassifyPair(pair.L, pair.R, pair.Levels);
                lines.Add(new DiffLine
                {
                    Status = status,
                    Left = pair.L,
                    Right = pair.R,
                    FieldLevels = pair.Levels,
                    ConfidencePercent = ToConfidencePercent(columns, pair.Levels, status)
                });
                prevRightIndex = pair.R.SourceIndex;
                leftIndex++;
                continue;
            }

            if (deletedIds.Contains(leftRow.Id))
            {
                var nextAnchor = anchors.FirstOrDefault(a => a.L.SourceIndex > leftRow.SourceIndex);
                var rightBound = nextAnchor.L is null ? int.MaxValue : nextAnchor.R.SourceIndex;
                FlushAddedBefore(rightBound);

                lines.Add(new DiffLine
                {
                    Status = DiffRowStatus.Deleted,
                    Left = leftRow,
                    ConfidencePercent = 0
                });
                leftIndex++;
                continue;
            }

            leftIndex++;
        }

        foreach (var a in addedRemaining.OrderBy(x => x.SourceIndex))
        {
            lines.Add(new DiffLine
            {
                Status = DiffRowStatus.Added,
                Right = a,
                ConfidencePercent = 0
            });
        }

        return lines;
    }

    /// <summary>Схожесть 0–100; полное совпадение содержимого (Unchanged/Moved) = 100.</summary>
    private static int ToConfidencePercent(
        CompareColumn[] columns,
        FieldMatchLevel[] levels,
        DiffRowStatus status)
    {
        if (status is DiffRowStatus.Unchanged or DiffRowStatus.Moved)
        {
            return 100;
        }

        if (status is DiffRowStatus.Deleted or DiffRowStatus.Added)
        {
            return 0;
        }

        return (int)Math.Round(Confidence(columns, levels) * 100.0);
    }

    private static DiffRowStatus ClassifyPair(
        CompareRowDto left,
        CompareRowDto right,
        FieldMatchLevel[] levels)
    {
        // Содержательные Near/Mismatch (№ п/п в ScoreFields всегда Exact — не здесь).
        var hasContentChange = false;
        for (var i = 0; i < levels.Length; i++)
        {
            if (levels[i] is FieldMatchLevel.Near or FieldMatchLevel.Mismatch)
            {
                hasContentChange = true;
                break;
            }
        }

        if (hasContentChange)
        {
            return DiffRowStatus.Changed;
        }

        // «Перемещение» — только смена № п/п у той же строки по содержимому.
        // SourceIndex не используем: иначе получается «№50 → №50» из‑за сдвига списка.
        return left.NumberInOrder != right.NumberInOrder
            ? DiffRowStatus.Moved
            : DiffRowStatus.Unchanged;
    }

    private static FieldMatchLevel[] ScoreFields(
        CompareColumn[] columns,
        CompareRowDto left,
        CompareRowDto right)
    {
        var levels = new FieldMatchLevel[columns.Length];
        for (var i = 0; i < columns.Length; i++)
        {
            var leftVal = i < left.Values.Length ? left.Values[i] : "";
            var rightVal = i < right.Values.Length ? right.Values[i] : "";
            levels[i] = FormPrintCompareNormalize.CompareCell(columns[i], leftVal, rightVal).Level;
        }

        return levels;
    }

    private static double Confidence(CompareColumn[] columns, FieldMatchLevel[] levels)
    {
        double score = 0;
        double max = 0;
        for (var i = 0; i < columns.Length && i < levels.Length; i++)
        {
            if (columns[i].IsRowNumber)
            {
                continue;
            }

            var weight = columns[i].InFingerprint ? 2.0 : 1.0;
            max += weight;
            score += levels[i] switch
            {
                FieldMatchLevel.Exact => weight,
                FieldMatchLevel.Near => weight * 0.7,
                _ => 0
            };
        }

        return max <= 0 ? 0 : score / max;
    }
}
