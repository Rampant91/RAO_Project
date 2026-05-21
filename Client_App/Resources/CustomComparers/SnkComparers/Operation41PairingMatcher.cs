using System.Collections.Generic;
using System.Linq;

namespace Client_App.Resources.CustomComparers.SnkComparers;

/// <summary>
/// Сопоставление операций 41 между формами 1.1 и 1.5 с учётом количества при пустых паспорте и зав. номере.
/// </summary>
public static class Operation41PairingMatcher
{
    private static readonly Operation41PairingKeyComparer KeyComparer = new();

    public static List<T> FindUnpaired<T>(
        IReadOnlyList<T> source,
        IReadOnlyList<T> reference,
        System.Func<T, Operation41PairingKey> toKey)
    {
        var sourceWithSerial = new List<T>();
        var sourceWithoutSerial = new List<T>();
        foreach (var item in source)
        {
            var key = toKey(item);
            if (Operation41PairingKeyComparer.SerialNumbersIsEmpty(key.PasNum, key.FacNum))
            {
                sourceWithoutSerial.Add(item);
            }
            else
            {
                sourceWithSerial.Add(item);
            }
        }

        var referenceWithSerial = new List<T>();
        var referenceWithoutSerial = new List<T>();
        foreach (var item in reference)
        {
            var key = toKey(item);
            if (Operation41PairingKeyComparer.SerialNumbersIsEmpty(key.PasNum, key.FacNum))
            {
                referenceWithoutSerial.Add(item);
            }
            else
            {
                referenceWithSerial.Add(item);
            }
        }

        var unpaired = new List<T>();
        unpaired.AddRange(FindUnpairedWithSerialNumbers(sourceWithSerial, referenceWithSerial, toKey));
        unpaired.AddRange(FindUnpairedWithoutSerialNumbers(sourceWithoutSerial, referenceWithoutSerial, toKey));
        return unpaired;
    }

    /// <summary>
    /// Построчное сопоставление, когда указаны паспорт и/или заводской номер.
    /// </summary>
    private static List<T> FindUnpairedWithSerialNumbers<T>(
        IReadOnlyList<T> source,
        IReadOnlyList<T> reference,
        System.Func<T, Operation41PairingKey> toKey)
    {
        var referenceCounts = new Dictionary<Operation41PairingKey, int>(KeyComparer);
        foreach (var item in reference)
        {
            var key = toKey(item);
            referenceCounts.TryGetValue(key, out var count);
            referenceCounts[key] = count + 1;
        }

        var unpaired = new List<T>();
        foreach (var item in source)
        {
            var key = toKey(item);
            if (referenceCounts.TryGetValue(key, out var count) && count > 0)
            {
                referenceCounts[key] = count - 1;
            }
            else
            {
                unpaired.Add(item);
            }
        }

        return unpaired;
    }

    /// <summary>
    /// Сопоставление по сумме количества в группе с одинаковыми ключевыми полями (без паспорта и зав. номера).
    /// </summary>
    private static List<T> FindUnpairedWithoutSerialNumbers<T>(
        IReadOnlyList<T> source,
        IReadOnlyList<T> reference,
        System.Func<T, Operation41PairingKey> toKey)
    {
        var referenceByGroup = reference
            .GroupBy(toKey, new AggregateGroupKeyComparer())
            .ToDictionary(
                group => group.Key,
                group => new QuantityGroup<T>(group.ToList(), group.Sum(item => Operation41PairingKeyComparer.GetQuantity(toKey(item)))),
                new AggregateGroupKeyComparer());

        var unpaired = new List<T>();

        foreach (var sourceGroup in source.GroupBy(toKey, new AggregateGroupKeyComparer()))
        {
            var groupKey = sourceGroup.Key;
            var sourceRows = sourceGroup.ToList();
            var sourceTotal = sourceRows.Sum(item => Operation41PairingKeyComparer.GetQuantity(toKey(item)));

            var referenceTotal = referenceByGroup.TryGetValue(groupKey, out var referenceGroup)
                ? referenceGroup.TotalQuantity
                : 0;

            if (sourceTotal <= referenceTotal)
            {
                continue;
            }

            var excessQuantity = sourceTotal - referenceTotal;
            unpaired.AddRange(SelectRowsForExcessQuantity(sourceRows, excessQuantity, toKey));
        }

        return unpaired;
    }

    private static List<T> SelectRowsForExcessQuantity<T>(
        List<T> rows,
        int excessQuantity,
        System.Func<T, Operation41PairingKey> toKey)
    {
        var remaining = excessQuantity;
        var result = new List<T>();

        foreach (var row in rows.OrderByDescending(item => Operation41PairingKeyComparer.GetQuantity(toKey(item))))
        {
            if (remaining <= 0)
            {
                break;
            }

            var rowQuantity = Operation41PairingKeyComparer.GetQuantity(toKey(row));
            if (rowQuantity <= remaining)
            {
                result.Add(row);
                remaining -= rowQuantity;
            }
            else
            {
                result.Add(row);
                remaining = 0;
            }
        }

        return result;
    }

    private sealed class AggregateGroupKeyComparer : IEqualityComparer<Operation41PairingKey>
    {
        private readonly Operation41PairingKeyComparer _comparer = new();

        public bool Equals(Operation41PairingKey x, Operation41PairingKey y) =>
            _comparer.AggregateGroupEquals(x, y);

        public int GetHashCode(Operation41PairingKey obj) =>
            _comparer.GetAggregateGroupHashCode(obj);
    }

    private sealed class QuantityGroup<T>(List<T> rows, int totalQuantity)
    {
        public List<T> Rows { get; } = rows;
        public int TotalQuantity { get; } = totalQuantity;
    }
}
