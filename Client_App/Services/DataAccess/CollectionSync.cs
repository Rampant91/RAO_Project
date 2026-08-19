using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Синхронизация <see cref="ObservableCollection{T}"/> без пересоздания объекта (меньше мерцания UI).
/// </summary>
public static class CollectionSync
{
    /// <returns>true, если коллекция изменилась.</returns>
    public static bool ReplaceById<T>(
        ObservableCollection<T> target,
        IReadOnlyList<T> source,
        Func<T, int> idSelector)
    {
        if (target.Count == source.Count)
        {
            var same = true;
            for (var i = 0; i < source.Count; i++)
            {
                if (idSelector(target[i]) != idSelector(source[i]))
                {
                    same = false;
                    break;
                }
            }

            if (same)
                return false;
        }

        target.Clear();
        foreach (var item in source)
            target.Add(item);
        return true;
    }
}
