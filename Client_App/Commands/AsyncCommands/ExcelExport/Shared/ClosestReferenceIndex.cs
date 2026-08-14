using System;
using System.Collections.Generic;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Shared;

/// <summary>
/// Индекс кандидатов closest по дню операции (±N) для ускорения перебора без смены результата,
/// когда фильтрация по дате не применяется — используется полный список <see cref="CandidateSet{T}.All"/>.
/// </summary>
public static class ClosestReferenceIndex
{
    public sealed class CandidateSet<T>
    {
        public static CandidateSet<T> Empty { get; } = new([], null, []);

        public IReadOnlyList<T> All { get; }

        /// <summary>DayNumber → кандидаты с этой датой операции (если индекс построен).</summary>
        public IReadOnlyDictionary<int, IReadOnlyList<T>>? ByDay { get; }

        public IReadOnlyList<T> Undated { get; }

        internal CandidateSet(
            IReadOnlyList<T> all,
            IReadOnlyDictionary<int, IReadOnlyList<T>>? byDay,
            IReadOnlyList<T> undated)
        {
            All = all;
            ByDay = byDay;
            Undated = undated;
        }

        public static CandidateSet<T> Create(IReadOnlyList<T> ops, Func<T, string?> getOpDate, bool indexByDate)
        {
            if (!indexByDate || ops.Count == 0)
            {
                return new CandidateSet<T>(ops, null, []);
            }

            var byDay = new Dictionary<int, List<T>>();
            var undated = new List<T>();
            foreach (var op in ops)
            {
                if (DateOnly.TryParse(getOpDate(op), out var date))
                {
                    var day = date.DayNumber;
                    if (!byDay.TryGetValue(day, out var list))
                    {
                        list = [];
                        byDay[day] = list;
                    }

                    list.Add(op);
                }
                else
                {
                    undated.Add(op);
                }
            }

            var frozenByDay = new Dictionary<int, IReadOnlyList<T>>(byDay.Count);
            foreach (var (day, list) in byDay)
            {
                frozenByDay[day] = list;
            }

            return new CandidateSet<T>(ops, frozenByDay, undated);
        }
    }

    /// <summary>
    /// Перечисляет кандидатов: при фильтрации по дате — окно ±toleranceDays вокруг sourceDay + undated; иначе — все.
    /// </summary>
    public static IEnumerable<T> EnumerateCandidates<T>(
        CandidateSet<T> set,
        int? sourceDay,
        int toleranceDays,
        bool filterByDate)
    {
        if (!filterByDate || sourceDay is not int day || set.ByDay is null)
        {
            foreach (var candidate in set.All)
            {
                yield return candidate;
            }

            yield break;
        }

        for (var d = day - toleranceDays; d <= day + toleranceDays; d++)
        {
            if (set.ByDay.TryGetValue(d, out var dayList))
            {
                foreach (var candidate in dayList)
                {
                    yield return candidate;
                }
            }
        }

        foreach (var candidate in set.Undated)
        {
            yield return candidate;
        }
    }
}
