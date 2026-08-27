using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;

/// <summary>
/// Общий взвешенный перебор кандидатов closest (soft-score + tie-break по дате и Id).
/// Используется Pairing41 и TransferReceive.
/// </summary>
public static class WeightedClosestMatchEngine
{
    /// <summary>
    /// Потолок «Схожесть, %» для ближайшего совпадения непарной строки.
    /// 100 не показываем: это не подтверждённая пара, а оценка похожести.
    /// </summary>
    public const int MaxConfidencePercent = 99;

    public sealed record BestMatch<TCandidate, TField>(
        TCandidate Candidate,
        IReadOnlyDictionary<TField, FieldMatchLevel> FieldLevels,
        double RawScore,
        double MaxWeight,
        int ConfidencePercent)
        where TField : struct, Enum;

    public delegate double FieldWeightDelegate<TSource, TField>(TSource source, TField field, int fieldIndex)
        where TField : struct, Enum;

    public delegate FieldSimilarity FieldScoreDelegate<TSource, TCandidate, TField>(
        TSource source,
        TCandidate candidate,
        TField field,
        int fieldIndex)
        where TField : struct, Enum;

    public delegate (double WeightedAdd, double MaxAdd) ScoreBonusDelegate<TSource, TCandidate>(
        TSource source,
        TCandidate candidate,
        FieldMatchLevel[] levels);

    public delegate int DateDeltaDelegate<TSource, TCandidate>(TSource source, TCandidate candidate);

    public delegate bool CandidateFilterDelegate<TSource, TCandidate>(TSource source, TCandidate candidate);

    public delegate ClosestReferenceIndex.CandidateSet<TCandidate> CandidateSetProvider<TSource, TCandidate>(
        TSource source);

    /// <summary>
    /// Для каждого source находит лучшего candidate в одном общем пуле.
    /// </summary>
    public static Dictionary<int, BestMatch<TCandidate, TField>> FindBestMatches<TSource, TCandidate, TField>(
        IReadOnlyList<TSource> sources,
        ClosestReferenceIndex.CandidateSet<TCandidate> candidateSet,
        IReadOnlyList<TField> fields,
        Func<TSource, int> getSourceId,
        Func<TCandidate, int> getCandidateId,
        Func<TSource, int?> getSourceOpDay,
        FieldWeightDelegate<TSource, TField> getWeight,
        FieldScoreDelegate<TSource, TCandidate, TField> scoreField,
        DateDeltaDelegate<TSource, TCandidate> dateDelta,
        bool filterCandidatesByDate,
        int dateToleranceDays,
        ScoreBonusDelegate<TSource, TCandidate>? applyBonus = null,
        CandidateFilterDelegate<TSource, TCandidate>? skipCandidate = null,
        bool useParallel = true,
        Action<int, int>? onProgress = null)
        where TField : struct, Enum =>
        FindBestMatchesCore(
            sources,
            fields,
            getSourceId,
            getCandidateId,
            getSourceOpDay,
            getWeight,
            scoreField,
            dateDelta,
            filterCandidatesByDate,
            dateToleranceDays,
            applyBonus,
            skipCandidate,
            useParallel,
            onProgress,
            getCandidateSet: null,
            sharedCandidateSet: candidateSet);

    /// <summary>
    /// Для каждого source находит лучшего candidate в своём наборе (TransferReceive: по ОКПО/направлению).
    /// </summary>
    public static Dictionary<int, BestMatch<TCandidate, TField>> FindBestMatchesPerSource<TSource, TCandidate, TField>(
        IReadOnlyList<TSource> sources,
        IReadOnlyList<TField> fields,
        Func<TSource, int> getSourceId,
        Func<TCandidate, int> getCandidateId,
        Func<TSource, int?> getSourceOpDay,
        CandidateSetProvider<TSource, TCandidate> getCandidateSet,
        FieldWeightDelegate<TSource, TField> getWeight,
        FieldScoreDelegate<TSource, TCandidate, TField> scoreField,
        DateDeltaDelegate<TSource, TCandidate> dateDelta,
        bool filterCandidatesByDate,
        int dateToleranceDays,
        ScoreBonusDelegate<TSource, TCandidate>? applyBonus = null,
        CandidateFilterDelegate<TSource, TCandidate>? skipCandidate = null,
        bool useParallel = true,
        Action<int, int>? onProgress = null)
        where TField : struct, Enum =>
        FindBestMatchesCore(
            sources,
            fields,
            getSourceId,
            getCandidateId,
            getSourceOpDay,
            getWeight,
            scoreField,
            dateDelta,
            filterCandidatesByDate,
            dateToleranceDays,
            applyBonus,
            skipCandidate,
            useParallel,
            onProgress,
            getCandidateSet,
            default(ClosestReferenceIndex.CandidateSet<TCandidate>));

    private static Dictionary<int, BestMatch<TCandidate, TField>> FindBestMatchesCore<TSource, TCandidate, TField>(
        IReadOnlyList<TSource> sources,
        IReadOnlyList<TField> fields,
        Func<TSource, int> getSourceId,
        Func<TCandidate, int> getCandidateId,
        Func<TSource, int?> getSourceOpDay,
        FieldWeightDelegate<TSource, TField> getWeight,
        FieldScoreDelegate<TSource, TCandidate, TField> scoreField,
        DateDeltaDelegate<TSource, TCandidate> dateDelta,
        bool filterCandidatesByDate,
        int dateToleranceDays,
        ScoreBonusDelegate<TSource, TCandidate>? applyBonus,
        CandidateFilterDelegate<TSource, TCandidate>? skipCandidate,
        bool useParallel,
        Action<int, int>? onProgress,
        CandidateSetProvider<TSource, TCandidate>? getCandidateSet,
        ClosestReferenceIndex.CandidateSet<TCandidate> sharedCandidateSet)
        where TField : struct, Enum
    {
        if (sources.Count == 0 || fields.Count == 0)
        {
            return new Dictionary<int, BestMatch<TCandidate, TField>>();
        }

        if (getCandidateSet is null && sharedCandidateSet.All.Count == 0)
        {
            return new Dictionary<int, BestMatch<TCandidate, TField>>();
        }

        var fieldCount = fields.Count;
        var bag = new ConcurrentDictionary<int, BestMatch<TCandidate, TField>>();
        var total = sources.Count;
        var done = 0;

        void ProcessOne(TSource source)
        {
            try
            {
                var candidateSet = getCandidateSet?.Invoke(source) ?? sharedCandidateSet;
                if (candidateSet.All.Count == 0)
                {
                    return;
                }

                var weights = new double[fieldCount];
                var maxWeightBase = 0.0;
                for (var i = 0; i < fieldCount; i++)
                {
                    weights[i] = getWeight(source, fields[i], i);
                    maxWeightBase += weights[i];
                }

                var scratchLevels = new FieldMatchLevel[fieldCount];
                var bestLevels = new FieldMatchLevel[fieldCount];
                var bestScore = double.NegativeInfinity;
                var bestMaxWeight = 0.0;
                TCandidate? bestCandidate = default;
                var bestDateDelta = int.MaxValue;
                var sourceDay = getSourceOpDay(source);

                foreach (var candidate in ClosestReferenceIndex.EnumerateCandidates(
                             candidateSet, sourceDay, dateToleranceDays, filterCandidatesByDate))
                {
                    if (skipCandidate?.Invoke(source, candidate) == true)
                    {
                        continue;
                    }

                    var weighted = 0.0;
                    var maxWeight = maxWeightBase;
                    for (var i = 0; i < fieldCount; i++)
                    {
                        var sim = scoreField(source, candidate, fields[i], i);
                        scratchLevels[i] = sim.Level;
                        weighted += weights[i] * sim.Score;
                    }

                    if (applyBonus?.Invoke(source, candidate, scratchLevels) is { } bonus)
                    {
                        weighted += bonus.WeightedAdd;
                        maxWeight += bonus.MaxAdd;
                    }

                    var delta = dateDelta(source, candidate);
                    var better = weighted > bestScore + 1e-9
                                 || (Math.Abs(weighted - bestScore) <= 1e-9
                                     && (delta < bestDateDelta
                                         || (delta == bestDateDelta
                                             && bestCandidate is not null
                                             && getCandidateId(candidate) < getCandidateId(bestCandidate))));

                    if (better)
                    {
                        bestScore = weighted;
                        bestMaxWeight = maxWeight;
                        bestCandidate = candidate;
                        bestDateDelta = delta;
                        Array.Copy(scratchLevels, bestLevels, fieldCount);
                    }
                }

                if (bestCandidate is null || bestMaxWeight <= 0)
                {
                    return;
                }

                var levelMap = new Dictionary<TField, FieldMatchLevel>(fieldCount);
                for (var i = 0; i < fieldCount; i++)
                {
                    levelMap[fields[i]] = bestLevels[i];
                }

                var confidence = ToConfidencePercent(bestScore, bestMaxWeight);
                bag[getSourceId(source)] = new BestMatch<TCandidate, TField>(
                    bestCandidate, levelMap, bestScore, bestMaxWeight, confidence);
            }
            finally
            {
                var completed = Interlocked.Increment(ref done);
                onProgress?.Invoke(completed, total);
            }
        }

        if (total == 1 || !useParallel)
        {
            foreach (var source in sources)
            {
                ProcessOne(source);
            }
        }
        else
        {
            Parallel.ForEach(sources, ProcessOne);
        }

        return new Dictionary<int, BestMatch<TCandidate, TField>>(bag);
    }

    /// <summary>
    /// Нормирует взвешенный soft-score в проценты 0…<see cref="MaxConfidencePercent"/>.
    /// </summary>
    public static int ToConfidencePercent(double weightedScore, double maxWeight)
    {
        if (maxWeight <= 0)
        {
            return 0;
        }

        var confidence = (int)Math.Round(100.0 * weightedScore / maxWeight);
        return Math.Clamp(confidence, 0, MaxConfidencePercent);
    }
}
