using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Interfaces.Logger;

public class Awaiter
{
    public static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);
    public static readonly Dictionary<string, SemaphoreSlim> Semaphores = new();

    public static async Task Async(string key, Func<Task> task, int count = 1)
    {
        await SemaphoreSlim.WaitAsync();
        try
        {
            if (!Semaphores.ContainsKey(key))
                Semaphores.Add(key, new SemaphoreSlim(count, count));
        }
        finally
        {
            SemaphoreSlim.Release();
        }
        var semaphore = Semaphores[key];
        await semaphore.WaitAsync();
        try
        {
            await task();
        }
        finally
        {
            semaphore.Release();
        }
    }
}