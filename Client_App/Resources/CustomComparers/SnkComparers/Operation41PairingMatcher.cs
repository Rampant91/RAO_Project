using System.Collections.Generic;
using System.Linq;

namespace Client_App.Resources.CustomComparers.SnkComparers;

/// <summary>
/// Сопоставление операций 41 между формами РВ и РАО.
/// </summary>
public static class Operation41PairingMatcher
{
    public static List<T> FindUnpaired<T>(
        IReadOnlyList<T> source,
        IReadOnlyList<T> reference,
        Operation41PairingProfile profile,
        System.Func<T, Operation41PairingKey> toSourceKey,
        System.Func<T, Operation41PairingKey> toReferenceKey)
    {
        var comparer = new Operation41PairingKeyComparer(profile);

        var sourceWithSerial = new List<T>();
        var sourceWithoutSerial = new List<T>();
        foreach (var item in source)
        {
            var key = toSourceKey(item);
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
            var key = toReferenceKey(item);
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
        // Агрегация по количеству при пустых паспорте/зав. номере — только для 1.1 ↔ 1.5.
        if (profile == Operation41PairingProfile.Form11To15)
        {
            unpaired.AddRange(FindUnpairedWithSerialNumbers(
                sourceWithSerial, referenceWithSerial, comparer, toSourceKey, toReferenceKey));
            unpaired.AddRange(FindUnpairedWithoutSerialNumbers(
                sourceWithoutSerial, referenceWithoutSerial, comparer, toSourceKey, toReferenceKey));
        }
        else
        {
            unpaired.AddRange(FindUnpairedWithSerialNumbers(
                source, reference, comparer, toSourceKey, toReferenceKey));
        }

        return unpaired;
    }

    /// <summary>
    /// Строки 1.6 без пары ни в 1.2, ни в 1.3, ни в 1.4.
    /// </summary>
    public static List<T> FindUnpairedForm16<T>(
        IReadOnlyList<T> form16Operations,
        IReadOnlyList<T> form12Operations,
        IReadOnlyList<T> form13Operations,
        IReadOnlyList<T> form14Operations,
        System.Func<T, Operation41PairingKey> toForm16For12Key,
        System.Func<T, Operation41PairingKey> toForm16For13Key,
        System.Func<T, Operation41PairingKey> toForm16For14Key,
        System.Func<T, Operation41PairingKey> toForm12Key,
        System.Func<T, Operation41PairingKey> toForm13Key,
        System.Func<T, Operation41PairingKey> toForm14Key)
    {
        var remaining = form16Operations.ToList();

        remaining = ConsumeMatched(
            remaining,
            form12Operations,
            Operation41PairingProfile.Form12To16,
            toForm16For12Key,
            toForm12Key);

        remaining = ConsumeMatched(
            remaining,
            form13Operations,
            Operation41PairingProfile.Form13To16,
            toForm16For13Key,
            toForm13Key);

        remaining = ConsumeMatched(
            remaining,
            form14Operations,
            Operation41PairingProfile.Form14To16,
            toForm16For14Key,
            toForm14Key);

        return remaining;
    }

    private static List<T> ConsumeMatched<T>(
        IReadOnlyList<T> sourceForm16,
        IReadOnlyList<T> referenceRv,
        Operation41PairingProfile profile,
        System.Func<T, Operation41PairingKey> toForm16Key,
        System.Func<T, Operation41PairingKey> toRvKey)
    {
        if (referenceRv.Count == 0)
        {
            return sourceForm16.ToList();
        }

        var comparer = new Operation41PairingKeyComparer(profile);
        var referenceCounts = new Dictionary<Operation41PairingKey, int>(comparer);
        foreach (var item in referenceRv)
        {
            var key = toRvKey(item);
            referenceCounts.TryGetValue(key, out var count);
            referenceCounts[key] = count + 1;
        }

        var remaining = new List<T>();
        foreach (var item in sourceForm16)
        {
            var key = toForm16Key(item);
            if (referenceCounts.TryGetValue(key, out var count) && count > 0)
            {
                referenceCounts[key] = count - 1;
            }
            else
            {
                remaining.Add(item);
            }
        }

        return remaining;
    }

    private static List<T> FindUnpairedWithSerialNumbers<T>(
        IReadOnlyList<T> source,
        IReadOnlyList<T> reference,
        Operation41PairingKeyComparer comparer,
        System.Func<T, Operation41PairingKey> toSourceKey,
        System.Func<T, Operation41PairingKey> toReferenceKey)
    {
        var referenceCounts = new Dictionary<Operation41PairingKey, int>(comparer);
        foreach (var item in reference)
        {
            var key = toReferenceKey(item);
            referenceCounts.TryGetValue(key, out var count);
            referenceCounts[key] = count + 1;
        }

        var unpaired = new List<T>();
        foreach (var item in source)
        {
            var key = toSourceKey(item);
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

    private static List<T> FindUnpairedWithoutSerialNumbers<T>(
        IReadOnlyList<T> source,
        IReadOnlyList<T> reference,
        Operation41PairingKeyComparer comparer,
        System.Func<T, Operation41PairingKey> toSourceKey,
        System.Func<T, Operation41PairingKey> toReferenceKey)
    {
        var referenceByGroup = reference
            .GroupBy(toReferenceKey, comparer)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(item => Operation41PairingKeyComparer.GetQuantity(toReferenceKey(item))),
                comparer);

        var unpaired = new List<T>();

        foreach (var sourceGroup in source.GroupBy(toSourceKey, comparer))
        {
            var groupKey = sourceGroup.Key;
            var sourceRows = sourceGroup.ToList();
            var sourceTotal = sourceRows.Sum(item => Operation41PairingKeyComparer.GetQuantity(toSourceKey(item)));

            var referenceTotal = referenceByGroup.TryGetValue(groupKey, out var total) ? total : 0;

            if (sourceTotal <= referenceTotal)
            {
                continue;
            }

            var excessQuantity = sourceTotal - referenceTotal;
            unpaired.AddRange(SelectRowsForExcessQuantity(sourceRows, excessQuantity, toSourceKey));
        }

        return unpaired;
    }

    private static List<T> SelectRowsForExcessQuantity<T>(
        List<T> rows,
        int excessQuantity,
        System.Func<T, Operation41PairingKey> toSourceKey)
    {
        var remaining = excessQuantity;
        var result = new List<T>();

        foreach (var row in rows.OrderByDescending(item => Operation41PairingKeyComparer.GetQuantity(toSourceKey(item))))
        {
            if (remaining <= 0)
            {
                break;
            }

            var rowQuantity = Operation41PairingKeyComparer.GetQuantity(toSourceKey(row));
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
}
